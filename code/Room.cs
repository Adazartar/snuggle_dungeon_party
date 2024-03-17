using Sandbox;

public sealed class Room : Component
{
	public bool active = false;
	[Property] public bool cleared = false;
	[Property] int enemy_count_1player = 0;
	[Property] int enemy_count_2player = 0;
	[Property] int enemy_count_3player = 0;

	[Property] bool N_exit = false;
	GameObject N_trigger;
	GameObject N_door;
	[Property] bool E_exit = false;
	GameObject E_trigger;
	GameObject E_door;
	[Property] bool S_exit = false;
	GameObject S_trigger;
	GameObject S_door;
	[Property] bool W_exit = false;
	GameObject W_trigger;
	GameObject W_door;

	public Vector3 room_center;
	public GameConfig config;

	protected override void OnStart(){
		foreach(GameObject child in GameObject.Children){
			if(child.Name == "N_door"){ N_door = child; }
			if(child.Name == "N_door-trigger"){ N_trigger = child; }
			if(child.Name == "E_door"){ E_door = child; }
			if(child.Name == "E_door-trigger"){	E_trigger = child; }
			if(child.Name == "S_door"){ S_door = child; }
			if(child.Name == "S_door-trigger"){ S_trigger = child; }
			if(child.Name == "W_door"){ W_door = child; }
			if(child.Name == "W_door-trigger"){ W_trigger = child; }
			if(child.Name == "Plane"){ room_center = child.Transform.Position; }
		}
	}
	protected override void OnUpdate()
	{
		if(active){
			handleRoomClear();
		}
	}

	public void setUpTriggers(int num_players, GameConfig in_config){
		config = in_config;
		N_trigger.Components.Get<Door>().setUpDoor(num_players, N_door, N_exit, this);
		E_trigger.Components.Get<Door>().setUpDoor(num_players, E_door, E_exit, this);
		S_trigger.Components.Get<Door>().setUpDoor(num_players, S_door, S_exit, this);
		W_trigger.Components.Get<Door>().setUpDoor(num_players, W_door, W_exit, this);
	}

	public void handleRoomClear(){
		if(config.num_players == 1){
			if(config.enemy_dead_count >= enemy_count_1player){
				cleared = true;
				reviveDeadPlayers();
			}
		}
		if(config.num_players == 2){
			if(config.enemy_dead_count >= enemy_count_2player){
				cleared = true;
				reviveDeadPlayers();
			}
		}
		if(config.num_players == 3){
			if(config.enemy_dead_count >= enemy_count_3player){
				cleared = true;
				reviveDeadPlayers();
			}
		}
	}

	public void reviveDeadPlayers(){
		foreach(GameObject player in config.active_players){
			if(!player.Enabled){
				player.Enabled = true;
				player.Transform.Position = room_center;
				player.Components.Get<Player>().health.current_health = player.Components.Get<Player>().health.max_health/2;
			}
		}
	}
	
}