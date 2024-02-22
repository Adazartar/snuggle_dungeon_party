using Sandbox;

public sealed class Projectile : Component, Component.ITriggerListener
{
	float projectile_speed = 0;
	Vector3 projectile_direction = Vector3.Zero;
	float projectile_duration = 99;
	float projectile_damage = 0;
	protected override void OnUpdate()
	{
		projectile_duration -= Time.Delta;
		Transform.Position += projectile_speed * projectile_direction;
		if(projectile_duration < 0){
			GameObject.Root.Destroy();
		}
	}

	public void projectObject(float speed, Vector3 direction, float duration, float damage)
	{
		projectile_speed = speed;
		projectile_direction = direction;
		projectile_duration = duration;
		projectile_damage = damage;
	}

	public void OnTriggerEnter(Collider other)
	{
		
		var hit_objects = other.Touching;
		if(other.Rigidbody != null){
			if(other.Rigidbody.GameObject.Tags.Has("enemy")){
				Log.Info("hit enemy");
			}
		}
		
	}
	public void OnTriggerUpdate(Collision other){
		Log.Info("we are in update");
	}
	
	public void OnTriggerExit(Collider other){}
}