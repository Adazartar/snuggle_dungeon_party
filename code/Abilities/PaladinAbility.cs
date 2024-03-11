using Sandbox;

public sealed class PaladinAbility : AbilityTemplate
{
    float invulnerability_duration = 5f;
    Health health_controller;
    int stored_health;
    float timer;
    bool active = false;

    protected override void OnUpdate(){
        timer -= Time.Delta;
        if(active && timer < 0){
            health_controller.setHealth(stored_health);
            active = false;
        }
    }
    public override void testing()
    {
        Log.Info("paladin is working");
    }
    public override void useAbility(Player player)
    {
        Log.Info("paladin uses ability");
        active = true;
        health_controller = player.health;
        stored_health = health_controller.current_health;
        health_controller.setHealth(1000);
        timer = invulnerability_duration;
        
    }


}