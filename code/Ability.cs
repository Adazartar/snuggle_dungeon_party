using Sandbox;

public sealed class Ability : Component
{
	[Property] public int ability_meter_max = 50;
	Player player;
	protected override void OnStart()
	{
		player = GameObject.Components.Get<Player>();
	}
	protected override void OnUpdate()
	{
		if(Input.Down("attack2") &&  player.ability_meter == ability_meter_max){
			Log.Info("using ability");
			player.ability_meter = 0;
		}
	}
}