using Sandbox;

public sealed class Health : Component
{
	[Property] int max_health = 100;
	int current_health;
	bool alive = true;
	
	protected override void OnStart()
	{
		current_health = max_health;
	}
	protected override void OnUpdate()
	{

	}

	public void changeHealth(int amount)
	{
		if(!alive){

		}
		else{
			current_health += amount;
			Log.Info($"{current_health}");
			if(current_health <= 0){
				alive = false;
			}
		}
		
	}

}
