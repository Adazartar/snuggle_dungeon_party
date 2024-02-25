using Sandbox;

public sealed class HealthPickup : InteractableObject
{
    public override void interact(){
        Log.Info("health has been picked up");
    }
}
