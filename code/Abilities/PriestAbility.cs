using Sandbox;

public sealed class PriestAbility : AbilityTemplate
{
    public override void testing()
    {
        Log.Info("preist is working");
    }
    public override void useAbility(Player player)
    {
        Log.Info("priest uses ability");
    }
}