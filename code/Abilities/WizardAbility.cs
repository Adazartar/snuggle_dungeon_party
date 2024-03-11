using Sandbox;

public sealed class WizardAbility : AbilityTemplate
{
    public override void testing()
    {
        Log.Info("wizard is working");
    }
    public override void useAbility(Player player)
    {
        Log.Info("wizard uses ability");
    }
}