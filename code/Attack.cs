using Sandbox;

public sealed class Attack : Component
{
	[Property] float attack_delay = 3;
	[Property] GameObject attack_item = null;
	[Property] float projectile_speed = 5;
	[Property] float projectile_duration = 1;
	[Property] int attack_damage = 5;
	float timer;
	protected override void OnUpdate()
	{
		timer -= Time.Delta;

		if(Input.Down("attack1") && timer < 0)
		{
			timer = attack_delay;
			GameObject new_attack = attack_item.Clone(Transform.Position, Transform.Rotation);
			new_attack.Components.Get<Projectile>().projectObject(projectile_speed, Transform.Rotation.Forward, projectile_duration, attack_damage, true, GameObject);

		}
	}
}