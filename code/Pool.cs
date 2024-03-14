using Sandbox;

public sealed class Pool : Component
{
	[Property] private GameObject prefab = null;
	[Property] private int pool_size = 5;
	[Property] private bool expandable = true;
	private List<GameObject> free_list = new List<GameObject>();
	private List<GameObject> used_list = new List<GameObject>();
	protected override void OnStart()
	{
		for(int i = 0; i < pool_size; i++){
			generateNewObject();
		}
	}
	protected override void OnUpdate()
	{

	}

	private void generateNewObject()
	{
		GameObject g = prefab.Clone();
		g.Enabled = false;
		free_list.Add(g);
	}

	public GameObject getObject()
	{
		if(free_list.Count == 0 && !expandable) return null;
		else if(free_list.Count == 0) generateNewObject();

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
			obj.Enabled = false;
			used_list.Remove(obj);
			free_list.Add(obj);
		}
	}
}