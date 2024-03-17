using Sandbox;

public sealed class FireElementPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("fire element is working");
    }
    public override void interact(Player player)
    {
        Log.Info("fire element picked up");
        player.attack.element = ElementType.Fire;
    }
}