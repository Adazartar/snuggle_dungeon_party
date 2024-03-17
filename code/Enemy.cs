using Sandbox;
using System;

public sealed class Enemy : Component
{
	[Property] int player_count_to_spawn = 1;
	[Property] float attack_windup = 1;
	float attack_windup_timer;
	[Property] float projectile_speed = 5;
	[Property] float projectile_duration = 1;
	[Property] bool piercing = true;
	[Property] int attack_damage = 5;
	[Property] float width = 2.5f;
	[Property] GameObject projectile_pool = null;
	Pool pool;
	[Property] GameObject players_container = null;
	[Property] float target_range = 10000f;
	[Property] float aggro_range = 500f;
	[Property] float attackingRange = 100f;
	GameObject targetPlayer = null;
	float targetDistance = 1000f;
	private List<GameObject> players = new List<GameObject>();
	EnemyState state = EnemyState.Idle;
	NavMeshAgent agent;
	float re_adjustposition_timer;
	[Property] float re_adjustposition_interval = 0.3f;
	[Property] float rotation_speed = 100f;
	Rotation target_rotation;
	bool finding_target = true;
	[Property] float rotation_time = 0.4f;
	float rotation_timer;

	float burn_timer = 0;
    float burn_tick_rate = 0.5f;
    int burn_tick_times = 6;
    int burn_tick_count = 0;
    bool burning;
    int burn_tick_damage = 3;

	float slow_timer;
	[Property] float slow_duration = 2f;
	[Property] float speed = 100;
	[Property] float slowed_speed = 60;
	

	Health health;

	protected override void OnEnabled()
	{
		pool = projectile_pool.Components.Get<Pool>();
		players = players_container.Children;
		agent = GameObject.Components.Get<NavMeshAgent>();
		health = GameObject.Components.Get<Health>();
		agent.MaxSpeed = speed;
		attack_windup_timer = attack_windup;
		rotation_timer = rotation_time;
	}
	protected override void OnUpdate()
	{
		updateState();
		updateBurn();
		updateSlow();
	}

	private void findNearestPlayer()
	{
		GameObject closestPlayer = null;
        float closestDistance = 10000000;
        foreach (var player in players){
			if(player.Enabled == true){
				float distance = Transform.Position.Distance(player.Transform.Position);
				if (distance < closestDistance){
					closestPlayer = player;
					closestDistance = distance;
				}
			}
        }

        if (closestPlayer != null && closestDistance < target_range){
            targetPlayer = closestPlayer;
			state = EnemyState.Chasing;
			re_adjustposition_timer = re_adjustposition_interval;
        }
	}
	
	private void updateState()
	{
		switch (state)
		{
			case EnemyState.Chasing:
				chasingState();
				break;
			case EnemyState.Aiming:
				aimingState();
				break;
			case EnemyState.Idle:
				idleState();
				break;
			case EnemyState.Attacking:
				attackingState();
				break;
			default:
				Log.Info("Class name not found");
				return;
		}
	}

	private void chasingState()
	{
		re_adjustposition_timer -= Time.Delta;
		
		targetDistance = Transform.Position.Distance(targetPlayer.Transform.Position);
		if(targetDistance > aggro_range){
			state = EnemyState.Idle;
			//findNearestPlayer();
		}
		if(targetDistance < attackingRange){
			state = EnemyState.Aiming;
		}
		if(re_adjustposition_timer < 0){
			//Log.Info("readjusting pos");
			agent.MoveTo(targetPlayer.Transform.Position);
			agent.UpdateRotation = true;
			re_adjustposition_timer = re_adjustposition_interval;
		}
		
	}

	private void idleState()
	{
		//Log.Info("in idle state");
		findNearestPlayer();
	}

	private void attackingState()
	{
		//Log.Info("in attacking state");
		if(!targetPlayer.Enabled){
			state = EnemyState.Idle;
		}
		re_adjustposition_timer -= Time.Delta;
		targetDistance = Transform.Position.Distance(targetPlayer.Transform.Position);
		attack_windup_timer -= Time.Delta;

		if(attack_windup_timer < 0){
			GameObject new_attack = pool.getObject();
			new_attack.Transform.Position = Transform.Position;
			new_attack.Components.Get<Projectile>().projectObject(projectile_speed, Transform.Rotation.Forward, 
				projectile_duration, attack_damage, false, GameObject, width, pool, piercing, false, 0, ElementType.None);
			attack_windup_timer = attack_windup;
			findNearestPlayer();
			state = EnemyState.Chasing;
		}

		
	}

	private void aimingState()
	{
		agent.Stop();
		agent.UpdateRotation = false;
		//Log.Info("in aiming state");
		rotation_timer -= Time.Delta;
		if(finding_target){
			
			Vector3 directionToPlayer = targetPlayer.Transform.Position - Transform.Position;
			float angle;
			if(directionToPlayer.x != 0){
				angle = (float)(Math.Atan2(directionToPlayer.y, directionToPlayer.x) * 180/Math.PI);
				target_rotation = new Angles(0, angle, 0).ToRotation();
			}
			else{
				angle = directionToPlayer.y > 0 ? 90 : -90;
				target_rotation = new Angles(0, angle, 0).ToRotation();
			}
			
			rotation_timer = rotation_time;
			finding_target = false;
			
		}
		else if(rotation_timer > 0){
			Transform.Rotation = Rotation.Slerp(Transform.Rotation, target_rotation, Time.Delta * rotation_speed, true);
		}
		else{
			state = EnemyState.Attacking;
			finding_target = true;
			rotation_timer = rotation_time;
		}
		
	}

	public void startBurn(){
		burning = true;
		burn_tick_count = 0;
	}

	public void updateBurn(){
		burn_timer -= Time.Delta;
        if(burning && burn_timer < 0 && burn_tick_count < burn_tick_times){
            burn_tick_count++;
            burn_timer = burn_tick_rate;
            health.changeHealth(-1 * burn_tick_damage);
        }
        else if(burning && burn_tick_count >= burn_tick_times){
            burn_tick_count = 0;
            burning = false;
        }
	}

	public void startSlow(){
		agent.MaxSpeed = slowed_speed;
		slow_timer = slow_duration;
	}

	public void updateSlow(){
		slow_timer -= Time.Delta;
		if(slow_timer < 0){
			agent.MaxSpeed = speed;
		}
	}

	public void checkOn(int num_players){
		if(num_players < player_count_to_spawn){
			GameObject.Enabled = false;
		}
	}

}