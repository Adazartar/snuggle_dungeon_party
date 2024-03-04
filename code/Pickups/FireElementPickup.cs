using Sandbox;

public sealed class FireElementPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("fire element is working");
    }
    public override void interact()
    {
        Log.Info("fire element picked up");
    }
}