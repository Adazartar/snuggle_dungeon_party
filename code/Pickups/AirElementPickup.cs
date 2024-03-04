using Sandbox;

public sealed class AirElementPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("air element is working");
    }
    public override void interact()
    {
        Log.Info("air element picked up");
    }
}