using Sandbox;

public sealed class InteractableObject : Component
{
    [Property] PickupType pickup_type;
    
    private PickupTemplate pickup;

    protected override void OnStart(){
        assignType(pickup_type);
    }
    private void assignType(PickupType type)
    {
        switch (type)
		{
			case PickupType.Health:
				pickup = new HealthPickup();
				break;
			case PickupType.FireElement:
				pickup = new FireElementPickup();
				break;
			case PickupType.IceElement:
				pickup = new IceElementPickup();
				break;
			case PickupType.GroundElement:
				pickup = new GroundElementPickup();
				break;
            case PickupType.AirElement:
				pickup = new AirElementPickup();
				break;
			default:
				Log.Info("Class name not found");
				return;
		}
    }

    public void interact(){
        pickup.interact();
    }

}