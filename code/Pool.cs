using Sandbox;

public sealed class Pool : Component
{
	List<GameObject> active_items =  new List<GameObject>();
	List<GameObject> inactive_items = new List<GameObject>();
	protected override void OnStart()
	{
		foreach(var child in GameObject.Children){
			if(child.Tags.Has("projectile")){
				Log.Info("added projectile");
				inactive_items.Add(child);
			}
		}
	}
	protected override void OnUpdate()
	{

	}

	public GameObject getFromPool()
	{
			GameObject new_item = inactive_items[inactive_items.Count - 1];
			new_item.Enabled = true;
			inactive_items.RemoveAt(inactive_items.Count - 1);
			active_items.Add(new_item);
			return new_item;
	}

	public void returnToPool(GameObject projectile)
	{
		projectile.Enabled = false;
		active_items.Remove(projectile);
		inactive_items.Add(projectile);
	}
}