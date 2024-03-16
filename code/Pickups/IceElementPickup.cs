using Sandbox;

public sealed class IceElementPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("ice element is working");
    }
    public override void interact(Player player)
    {
        Log.Info("ice element picked up");
        player.attack.element = ElementType.Ice;

    }
}