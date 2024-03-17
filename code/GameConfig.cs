using Sandbox;

public sealed class GameConfig : Component
{
	[Property] GameObject player_holder = null;
	[Property] GameObject enemy_holder = null;
	[Property] GameObject camera = null;
	List<GameObject> players;
	List<GameObject> enemies;
	public int num_players;

	[Property] GameObject rooms_holder = null;
	List<GameObject> rooms;

	float timer = 0;
	float camera_travel_time = 10;

	Vector3 room_center;

	public int enemy_dead_count;

	public List<GameObject> active_players = new List<GameObject>();
	[Property] GameObject interactables_holder = null;
	public List<GameObject> interactables_list;

	protected override void OnStart()
	{
		players = player_holder.Children;
		rooms = rooms_holder.Children;
		enemies = enemy_holder.Children;
		interactables_list = interactables_holder.Children;
	}
	protected override void OnUpdate()
	{
		if(timer > 0){
			camera.Transform.Position = Vector3.Lerp(camera.Transform.Position, new Vector3(room_center.x + 900, room_center.y, 900), 5 * Time.Delta);
		}
	}

	public void StartGame(Dictionary<string, int> selected_options)
	{
		foreach(var option in selected_options){
			foreach(var player in players){
				if(option.Key == player.Name && option.Value != 4){
					player.Enabled = true;
					active_players.Add(player);
					assignControl(option.Value, player);
				}
			}
		}

		foreach(var room in rooms){
			room.Components.Get<Room>().setUpTriggers(num_players, this);
		}

		foreach(var enemy in enemies){
			enemy.Components.Get<Enemy>().checkOn(num_players);
		}
	}

	public void assignControl(int i, GameObject player){
		Player player_controller = player.Components.Get<Player>();
		if(i == 1){
			player_controller.input_type = InputType.BaseKeyboard;
		}
		else if(i == 2){
			player_controller.input_type = InputType.SecondaryKeyboard;
		}
		else if(i == 3){
			player_controller.input_type = InputType.Controller;
		}
	}

	public void changeActiveRoom(Vector3 in_room_center){
		Log.Info("switching room");
		room_center = in_room_center;
		timer = camera_travel_time;
		enemy_dead_count = 0;
	}

	public void notifyEnemyDead(){
		enemy_dead_count++;
	}
}