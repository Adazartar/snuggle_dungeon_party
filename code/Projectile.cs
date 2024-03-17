using Sandbox;

public sealed class Projectile : Component, Component.ITriggerListener
{
	[Property] NearbyObjects nearby_objects_handler = null;
	float projectile_speed = 0;
	Vector3 projectile_direction = Vector3.Zero;
	float projectile_duration = 99;
	int projectile_damage = 0;
	bool player_projectile = false;
	GameObject projectile_source;
	bool piercing;
	Pool pool;
	bool projectile_explodes;
	float proj_explosion_radius;
	ElementType projectile_elem;
	protected override void OnUpdate()
	{
		projectile_duration -= Time.Delta;
		Transform.Position += projectile_speed * projectile_direction * Time.Delta * 100;
		if(projectile_duration < 0){
			explode();
			pool.returnObject(GameObject);
		}
	}

	public void projectObject(float speed, Vector3 direction, float duration, int damage, bool from_player,
	 GameObject source, float width, Pool obj_pool, bool is_piercing, bool explodes, float explosion_radius, ElementType elem)
	{
		projectile_source = source;
		GameObject.Components.Get<BoxCollider>().Scale = new Vector3(50f,50f * width,50f);
		Transform.Position = source.Transform.Position;
		Transform.Rotation = source.Transform.Rotation;
		pool = obj_pool;
		projectile_speed = speed;
		projectile_direction = direction;
		projectile_duration = duration;
		projectile_damage = damage;
		player_projectile = from_player;
		
		piercing = is_piercing;
		projectile_explodes = explodes;
		proj_explosion_radius = explosion_radius;
		projectile_elem = elem;

	}

	public void OnTriggerEnter(Collider other)
	{
		GameObject hit_target = other.GameObject;
		if(hit_target != null){
			if(player_projectile && hit_target.Tags.Has("enemy")){
				if(projectile_elem == ElementType.Fire){
					hit_target.Components.Get<Enemy>().startBurn();
				}
				if(projectile_elem == ElementType.Ice){
					hit_target.Components.Get<Enemy>().startSlow();
				}
				if(projectile_elem == ElementType.Ground){
					List<GameObject> nearby_enemies = nearby_objects_handler.getNearbyObjects(projectile_source.Components.Get<Player>().enemy_objects, 100);
					foreach(var enemy in nearby_enemies){
						enemy.Components.Get<Health>().changeHealth(-2);
					}
				}
				hit_target.Components.Get<Health>().changeHealth(-1 * projectile_damage);
				projectile_source.Components.Get<Player>().chargeAbilityDamaging(projectile_damage);
				if(!piercing){
					explode();
					pool.returnObject(GameObject);
				}
			}
			if(!player_projectile && hit_target.Tags.Has("player")){
				hit_target.Components.Get<Health>().changeHealth(-1 * projectile_damage);
				if(!piercing){
					pool.returnObject(GameObject);
				}
			}

			if(hit_target.Tags.Has("wall")){
				explode();
				pool.returnObject(GameObject);
			}
		}
		
	}
	public void OnTriggerUpdate(Collision other){}
	
	public void OnTriggerExit(Collider other){}

	public void explode(){
		if(projectile_explodes){
			List<GameObject> nearby_enemies = nearby_objects_handler.getNearbyObjects(projectile_source.Components.Get<Player>().enemy_objects, proj_explosion_radius);
			foreach(var enemy in nearby_enemies){
				enemy.Components.Get<Health>().changeHealth(-1 * projectile_damage * 2);
			}
		}
	}
}