using Sandbox;

public sealed class Projectile : Component, Component.ITriggerListener
{
	float projectile_speed = 0;
	Vector3 projectile_direction = Vector3.Zero;
	float projectile_duration = 99;
	int projectile_damage = 0;
	bool player_projectile = false;
	GameObject projectile_source;
	protected override void OnUpdate()
	{
		projectile_duration -= Time.Delta;
		Transform.Position += projectile_speed * projectile_direction * Time.Delta * 100;
		if(projectile_duration < 0){
			GameObject.Parent.Components.Get<Pool>().returnToPool(GameObject);
		}
	}

	public void projectObject(float speed, Vector3 direction, float duration, int damage, bool from_player, GameObject source, float width)
	{
		projectile_speed = speed;
		projectile_direction = direction;
		projectile_duration = duration;
		projectile_damage = damage;
		player_projectile = from_player;
		projectile_source = source;
		Transform.Position = source.Transform.Position;
		Transform.Rotation = source.Transform.Rotation;
		Transform.Scale = new Vector3(0.4f,width,0.4f);

	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.Rigidbody != null){
			var hit_gameObject = other.Rigidbody.GameObject;
			if(player_projectile && hit_gameObject.Tags.Has("enemy")){
				Log.Info("player hit enemy");
				hit_gameObject.Components.Get<Health>().changeHealth(-1 * projectile_damage);
				projectile_source.Components.Get<Player>().chargeAbilityDamaging(projectile_damage);
			}
			if(!player_projectile && hit_gameObject.Tags.Has("player")){
				//Log.Info("enemy hit player");
				hit_gameObject.Components.Get<Health>().changeHealth(-1 * projectile_damage);
			}
		}
		
	}
	public void OnTriggerUpdate(Collision other){}
	
	public void OnTriggerExit(Collider other){}
}