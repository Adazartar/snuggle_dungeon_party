using Sandbox;

public sealed class GameConfig : Component
{
	[Property] GameObject player_holder = null;
	[Property] GameObject camera = null;
	List<GameObject> players;
	public int num_players;

	[Property] GameObject rooms_holder = null;
	List<GameObject> rooms;

	float timer = 0;
	float camera_travel_time = 10;

	Vector3 room_center;

	protected override void OnStart()
	{
		players = player_holder.Children;
		rooms = rooms_holder.Children;
	}
	protected override void OnUpdate()
	{
		if(timer > 0){
			camera.Transform.Position = Vector3.Lerp(camera.Transform.Position, new Vector3(room_center.x, room_center.y, 1400), 3 * Time.Delta);
		}
	}

	public void StartGame(Dictionary<string, int> selected_options)
	{
		foreach(var option in selected_options){
			foreach(var player in players){
				if(option.Key == player.Name && option.Value != 4){
					player.Enabled = true;
					assignControl(option.Value, player);
				}
			}
		}
		foreach(var room in rooms){
			room.Components.Get<Room>().setUpTriggers(num_players, this);
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
		room_center = in_room_center;
		timer = camera_travel_time;
	}


}