using Sandbox;
using System.Collections.Generic;

public sealed class InteractBubble : Component, Component.ITriggerListener
{
	List<GameObject> hit_objects = new List<GameObject>();
	protected override void OnUpdate()
	{

	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.Rigidbody != null){
			var hit_gameObject = other.Rigidbody.GameObject;
			hit_objects.Add(hit_gameObject);
		}
        
	}
	public void OnTriggerUpdate(Collision other){}
	public void OnTriggerExit(Collider other){}

	public void getClosest()
	{
		GameObject closestInteractable = null;
        float closestDistance = 999;

        foreach (var hit_object in hit_objects)
        {
            if (hit_object.Tags.Has("interactable"))
            {
                float distance = Transform.Position.Distance(hit_object.Transform.Position);
                if (distance < closestDistance)
                {
                    closestInteractable = hit_object;
                    closestDistance = distance;
                }
            }
        }

        if (closestInteractable != null){
            closestInteractable.Components.Get<HealthPickup>().interact();
        }
	}
}