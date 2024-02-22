using Sandbox;

public sealed class Health : Component
{
	[Property] int max_health = 100;
	int current_health;
	protected override void OnStart()
	{
		current_health = max_health;
	}
	protected override void OnUpdate()
	{

	}

	private void changeHealth(int amount)
	{
		current_health += amount;
	}

}