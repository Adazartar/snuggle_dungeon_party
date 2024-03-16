using Sandbox;

public sealed class Health : Component
{
	[Property] public int max_health = 100;
	[Property] public int current_health;
	bool alive = true;
	public bool unchangeable = false;
	
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
			if(!unchangeable){
				current_health += amount;
				if(current_health <= 0){
					alive = false;
					GameObject.Enabled = false;
				}
				else if(current_health > max_health){
					current_health = max_health;
				}
			}
			
		}
		
	}

}
