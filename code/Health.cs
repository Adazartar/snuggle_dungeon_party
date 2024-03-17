using Sandbox;

public sealed class Health : Component
{
	[Property] public int max_health = 100;
	[Property] public int current_health;
	bool alive = true;
	public bool unchangeable = false;

	[Property] GameObject game_config_obj = null;
	GameConfig config;
	
	protected override void OnStart()
	{
		current_health = max_health;
		config = game_config_obj.Components.Get<GameConfig>();
	}
	protected override void OnUpdate()
	{

	}

	public void changeHealth(int amount)
	{
		if(!alive){

		}
		else{
			if(!unchangeable){
				current_health += amount;
				if(current_health <= 0){
					alive = false;
					if(GameObject.Tags.Has("enemy")){
						config.notifyEnemyDead();
					}
					GameObject.Enabled = false;
				}
				else if(current_health > max_health){
					current_health = max_health;
				}
			}
			
		}
		
	}

}
