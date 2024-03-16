using Sandbox;

public sealed class GroundElementPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("ground element is working");
    }
    public override void interact(Player player)
    {
        Log.Info("ground element picked up");
        player.attack.element = ElementType.Ground;

    }
}