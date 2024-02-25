using Sandbox;

public sealed class PaladinAbility : AbilityTemplate
{
    public override void testing()
    {
        Log.Info("paladin is working");
    }
    public override void useAbility()
    {
        Log.Info("paladin uses ability");
    }
}