using Sandbox;
using System;

public sealed class Player : Component
{
	Rigidbody rb;
	[Property] int speed = 10;
	[Property] int rotation_speed = 10;
	public int ability_meter = 0;
	[Property] float time_moving_for_1_ability_pt = 1;
	float time_moving_for_1_ability_pt_timer = 1;
	Ability ability;

	Vector3 vel;
	Rotation target_rotation;
	[Property] bool using_controller = true;
	protected override void OnStart(){
		Log.Info("We have started");
		rb = GameObject.Components.Get<Rigidbody>();
		ability = GameObject.Components.Get<Ability>();

	}
	protected override void OnFixedUpdate()
	{
		rb.Velocity = vel.Normal * Time.Delta * 1000 * speed;
	}
	protected override void OnUpdate()
	{
		updateMove();
		updateRotate();
		chargeAbilityMoving();
		Log.Info($"{ability_meter}");
		
	}

	private void updateMove()
	{
		vel = Vector3.Zero;
		if(using_controller)
		{
			vel += Input.AnalogMove;
		}
		else
		{
			if(Input.Down("forward")){
				vel += new Vector3(1, 0, 0);
			}
			if(Input.Down("backward")){
				vel += new Vector3(-1,0, 0);
			}
			if(Input.Down("left")){
				vel += new Vector3(0, 1, 0);
			}
			if(Input.Down("right")){
				vel += new Vector3(0, -1, 0);
			}
		}
		
		
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
}