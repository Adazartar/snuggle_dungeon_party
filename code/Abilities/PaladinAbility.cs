using Sandbox;

public sealed class PaladinAbility : AbilityTemplate
{
    float invulnerability_duration = 5f;
    float timer;
    bool active = false;
    Player player;

    protected override void OnUpdate(){
        timer -= Time.Delta;
        if(active && timer < 0){
            player.health.unchangeable = false;
            active = false;
            Log.Info("paladin ability is finished");
        }
    }
    public override void testing()
    {
        Log.Info("paladin is working");
    }
    public override void useAbility(Player this_player)
    {
        Log.Info("paladin uses ability");
        player = this_player;
        active = true;
        player.health.unchangeable = true;
        timer = invulnerability_duration;
        
    }


}