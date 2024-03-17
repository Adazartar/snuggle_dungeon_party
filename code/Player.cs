using Sandbox;
using System;

public sealed class Player : Component
{
	[Property] public NearbyObjects nearby_objects_handler = null;
	[Property] int speed = 10;
	[Property] int rotation_speed = 10;
	public int ability_meter = 0;
	[Property] float time_moving_for_1_ability_pt = 1;
	float time_moving_for_1_ability_pt_timer = 1;
	[Property] float interact_cooldown = 1;
	[Property] float interact_range = 100;
	float interact_timer;
	public Ability ability;
	public Health health;
	public Attack attack;

	Vector3 vel;
	Rotation target_rotation;
	[Property] GameObject interactables = null;
	[Property] GameObject enemies = null;
	List<GameObject> interactable_objects = new List<GameObject>();
	public List<GameObject> enemy_objects = new List<GameObject>();
	[Property] public InputType input_type = InputType.BaseKeyboard;

	public SkinnedModelRenderer model;
	[Property] GameObject model_object = null; 
	protected override void OnStart()
	{
		ability = GameObject.Components.Get<Ability>();
		health = GameObject.Components.Get<Health>();
		attack = GameObject.Components.Get<Attack>();
		interactable_objects = interactables.Children;
		enemy_objects = enemies.Children;
		model = model_object.Components.Get<SkinnedModelRenderer>();


	}
	protected override void OnUpdate()
	{
		updateMove();
		updateRotate();
		chargeAbilityMoving();
		updateInteractKey();
		//Log.Info($"{ability_meter}");
		//Log.Info($"{interact_timer}");
		
	}

	private void updateMove()
	{
		vel = Vector3.Zero;
		if(input_type == InputType.Controller)
		{
			if(Input.Down("forward_con")){
				vel += new Vector3(-1, 0, 0);
			}
			if(Input.Down("backward_con")){
				vel += new Vector3(1,0, 0);
			}
			if(Input.Down("left_con")){
				vel += new Vector3(0, -1, 0);
			}
			if(Input.Down("right_con")){
				vel += new Vector3(0, 1, 0);
			}
		}
		else if(input_type == InputType.BaseKeyboard)
		{
			if(Input.Down("forward")){
				vel += new Vector3(-1, 0, 0);
			}
			if(Input.Down("backward")){
				vel += new Vector3(1,0, 0);
			}
			if(Input.Down("left")){
				vel += new Vector3(0, -1, 0);
			}
			if(Input.Down("right")){
				vel += new Vector3(0, 1, 0);
			}
		}
		else
		{
			if(Input.Down("forward_sec")){
				vel += new Vector3(-1, 0, 0);
			}
			if(Input.Down("backward_sec")){
				vel += new Vector3(1,0, 0);
			}
			if(Input.Down("left_sec")){
				vel += new Vector3(0, -1, 0);
			}
			if(Input.Down("right_sec")){
				vel += new Vector3(0, 1, 0);
			}
		}

		
		model.Set("Moving", vel != Vector3.Zero);
		
		
		Transform.Position += vel.Normal * speed * Time.Delta * 10;
	
	}
	

	private void updateRotate()
	{
		float angle;
		if(vel.x == 0 && vel.y == 0)
		{
			return;
		}
		if(vel.x != 0)
		{
			angle = (float)(Math.Atan2(vel.y, vel.x) * 180/Math.PI);
			target_rotation = new Angles(0, angle, 0).ToRotation();
			Transform.Rotation = Rotation.Slerp(Transform.Rotation, target_rotation, Time.Delta * rotation_speed, true);
		}
		else
		{
			angle = vel.y > 0 ? 90 : -90;
			target_rotation = new Angles(0, angle, 0).ToRotation();
			Transform.Rotation = Rotation.Slerp(Transform.Rotation, target_rotation, Time.Delta * rotation_speed, true);
		}
	}

	private void chargeAbilityMoving()
	{
		if(vel != Vector3.Zero){
			time_moving_for_1_ability_pt_timer -= Time.Delta;
		}
		if(time_moving_for_1_ability_pt_timer < 0){
			time_moving_for_1_ability_pt_timer = time_moving_for_1_ability_pt;
			ability_meter += 1;
			if(ability_meter > ability.ability_meter_max){
				ability_meter = ability.ability_meter_max;
			}
		}
	}

	public void chargeAbilityDamaging(int damage)
	{
		ability_meter += damage;
		if(ability_meter > ability.ability_meter_max){
			ability_meter = ability.ability_meter_max;
		}
	}

	public void updateInteractKey(){
		if(input_type == InputType.Controller){
			updateInteract("interact_con");
		}
		else if(input_type == InputType.BaseKeyboard){
			updateInteract("interact");
		}
		else{
			updateInteract("interact_sec");
		}
	}

	public void updateInteract(string input_name){
		interact_timer -= Time.Delta;
		if(interact_timer < 0 && Input.Down(input_name)){
			interact_timer = interact_cooldown;
			List<GameObject> nearby = nearby_objects_handler.getNearbyObjects(interactable_objects, interact_range);
			interactWithClosest(nearby);
		}
	}

	public void interactWithClosest(List<GameObject> nearby)
	{
		GameObject closestInteractable = null;
        float closestDistance = 999;
        foreach (var interactable_object in nearby){
			if(interactable_object.Enabled == true){
				float distance = Transform.Position.Distance(interactable_object.Transform.Position);
				if (distance < closestDistance){
					closestInteractable = interactable_object;
					closestDistance = distance;
				}
			}
        }

        if (closestInteractable != null){
            closestInteractable.Components.Get<InteractableObject>().interact(this);
			closestInteractable.Enabled = false;
        }
	}
}