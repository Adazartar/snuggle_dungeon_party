using Sandbox;

public sealed class GroundElementPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("ground element is working");
    }
    public override void interact()
    {
        Log.Info("ground element picked up");
    }
}