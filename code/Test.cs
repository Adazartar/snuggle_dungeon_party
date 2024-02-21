using Sandbox;

public sealed class Test : Component
{
	protected override void OnUpdate()
	{
		Transform.Rotation = Transform.Local.RotationToWorld(Transform.Parent.Transform.Rotation);
	}
}