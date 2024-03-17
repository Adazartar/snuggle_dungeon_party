using Sandbox;

public sealed class HealthPickup : PickupTemplate
{
    public override void testing()
    {
        Log.Info("health is working");
    }
    public override void interact(Player player)
    {
        Log.Info("health picked up");
        player.health.changeHealth(20);
    }
}