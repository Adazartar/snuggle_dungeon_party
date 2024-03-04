using Sandbox;

public sealed class HealthPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("health is working");
    }
    public override void interact()
    {
        Log.Info("health picked up");
    }
}