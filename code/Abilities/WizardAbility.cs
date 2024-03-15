using Sandbox;

public sealed class WizardAbility : AbilityTemplate
{
    float projectile_speed = 10f;
    float projectile_duration = 2f;
    int attack_damage = 10;
    float width = 0.5f;
    public override void testing()
    {
        //Log.Info("wizard is working");
    }
    public override void useAbility(Player player)
    {
        Log.Info("wizard uses ability");
        Pool projectile_pool = player.Components.Get<Attack>().pool;
        GameObject new_attack = projectile_pool.getObject();
        new_attack.Components.Get<Projectile>().projectObject(projectile_speed, Transform.Rotation.Forward, 
            projectile_duration, attack_damage, true, GameObject, width, projectile_pool, false, true, 300);
    }
}