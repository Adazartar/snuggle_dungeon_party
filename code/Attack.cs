using Sandbox;

public sealed class Attack : Component
{
	[Property] public ElementType element = ElementType.None;
	[Property] float attack_delay = 3;
	[Property] float projectile_speed = 5;
	[Property] float projectile_duration = 1;
	[Property] int attack_damage = 5;
	[Property] float width = 2.5f;
	[Property] bool piercing = true;
	float timer;
	[Property] GameObject projectile_pool = null;
	public Pool pool;

	Player player;

	protected override void OnStart(){
		pool = projectile_pool.Components.Get<Pool>();
		player = GameObject.Components.Get<Player>();
	}
	
	protected override void OnUpdate()
	{
		timer -= Time.Delta;
		updateAttackKey();
		
	}

	public void updateAttackKey(){
		if(player.input_type == InputType.Controller){
			updateAttack("attack_con");
		}
		else if(player.input_type == InputType.BaseKeyboard){
			updateAttack("attack");
		}
		else{
			updateAttack("attack_sec");
		}
	}

	public void updateAttack(string input_name){
		if(Input.Down(input_name) && timer < 0)
		{
			timer = attack_delay;
			GameObject new_attack = pool.getObject();
			new_attack.Components.Get<Projectile>().projectObject(projectile_speed, Transform.Rotation.Forward, 
				projectile_duration, attack_damage, true, GameObject, width, pool, piercing, false, 0, element);

		}
	}
}