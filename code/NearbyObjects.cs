using Sandbox;

public sealed class NearbyObjects : Component
{
	protected override void OnUpdate()
	{

	}

	public List<GameObject> getNearbyObjects(List<GameObject> targets, float range){
		List<GameObject> nearby_objects = new List<GameObject>();

		foreach (var target in targets){
			if(target.Enabled == true){
				float distance = Transform.Position.Distance(target.Transform.Position);
				if (distance < range){
					nearby_objects.Add(target);
				}
			}
        }

		return nearby_objects;
	}
}