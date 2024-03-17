using Sandbox;

public sealed class Door : Component, Component.ITriggerListener
{
	int players_on_door;
	public int total_players;

	public GameObject door;
	bool activatable;
	[Property] bool entrance = true;

	Room room;


	protected override void OnUpdate()
	{
		if(players_on_door == total_players && activatable && entrance && room.cleared){
			door.Enabled = false;
			room.active = false;
		}
		else if(players_on_door == total_players && activatable && !entrance){
			door.Enabled = true;
			activatable = false;
			room.config.changeActiveRoom(room.room_center);
			room.active = true;
		}
	}

	public void OnTriggerEnter(Collider other){
		GameObject obj = other.GameObject;
		if(obj != null){
			if(obj.Tags.Has("player")){
				players_on_door += 1;
			}
		}
	}

	public void OnTriggerExit(Collider other){
		GameObject obj = other.GameObject;
		if(obj != null){
			if(obj.Tags.Has("player")){
				players_on_door -= 1;
			}
		}
	}

	public void setUpDoor(int in_total_players, GameObject in_door, bool in_activatable, Room in_room){
		total_players = in_total_players;
		door = in_door;
		activatable = in_activatable;
		room = in_room;
	}
}