using Sandbox;

public sealed class Attack : Component
{
	[Property] float attack_delay = 3;
	[Property] float projectile_speed = 5;
	[Property] float projectile_duration = 1;
	[Property] int attack_damage = 5;
	[Property] float width = 2.5f;
	[Property] bool piercing = true;
	float timer;
	[Property] GameObject projectile_pool = null;
	Pool pool;

	protected override void OnStart(){
		pool = projectile_pool.Components.Get<Pool>();
	}
	
	protected override void OnUpdate()
	{
		timer -= Time.Delta;

		if(Input.Down("attack1") && timer < 0)
		{
			timer = attack_delay;
			GameObject new_attack = pool.getObject();
			new_attack.Components.Get<Projectile>().projectObject(projectile_speed, Transform.Rotation.Forward, 
				projectile_duration, attack_damage, true, GameObject, width, pool, piercing);

		}
	}
}