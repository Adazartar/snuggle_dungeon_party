using Sandbox;

public sealed class BarbarianAbility : AbilityTemplate
{
    public override void testing()
    {
        Log.Info("barbarian is working");
    }
    public override void useAbility(Player player)
    {
        Log.Info("barbarian uses ability");
    }
}