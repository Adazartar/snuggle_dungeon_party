using Sandbox;

public sealed class Pool : Component
{
	[Property] private GameObject prefab = null;
	[Property] private int pool_size = 5;
	[Property] private bool expandable = true;
	private List<GameObject> free_list = new List<GameObject>();
	private List<GameObject> used_list = new List<GameObject>();
	int top_id = 0;

	protected override void OnStart()
	{
		for(int i = 0; i < pool_size; i++){
			generateNewObject(top_id);
			top_id++;
		}
	}
	protected override void OnUpdate()
	{

	}

	private void generateNewObject(int id)
	{
		GameObject g = prefab.Clone();
		g.Enabled = false;
		g.Transform.Position = Transform.Position;
		g.Name = $"projectile_{id}";
		free_list.Add(g);
	}

	public GameObject getObject()
	{
		if(free_list.Count == 0 && !expandable) return null;
		else if(free_list.Count == 0){
			generateNewObject(top_id);
			top_id++;
		} 

		GameObject g = free_list[free_list.Count - 1];
		free_list.RemoveAt(free_list.Count - 1);
		used_list.Add(g);
		g.Enabled = true;
		return g;
	}

	public void returnObject(GameObject obj)
	{
		if(used_list.Contains(obj)){
			obj.Transform.Position = Transform.Position;
			used_list.Remove(obj);
			free_list.Add(obj);
			obj.Enabled = false;
		}
	}
}