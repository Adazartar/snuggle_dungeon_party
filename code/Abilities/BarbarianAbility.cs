using Sandbox;

public sealed class BarbarianAbility : AbilityTemplate
{
    float timer = 0;
    float tick_rate = 0.5f;
    int tick_times = 6;
    int tick_count = 0;
    Player player;
    bool active;
    int tick_damage = 3;
    protected override void OnUpdate()
    {
        timer -= Time.Delta;
        if(active && timer < 0 && tick_count < tick_times){
            List<GameObject> nearby_enemies = player.nearby_objects_handler.getNearbyObjects(player.enemy_objects, 200);
            tick_count++;
            timer = tick_rate;
            foreach(var enemy in nearby_enemies){
                enemy.Components.Get<Health>().changeHealth(-1 * tick_damage);
            }
        }
        else if(active && tick_count == tick_times){
            tick_count = 0;
            active = false;
            Log.Info("barbarian ability is finished");
        }
    }
    public override void testing()
    {
        //Log.Info("barbarian is working");
    }
    public override void useAbility(Player this_player)
    {
        active = true;
        player = this_player;
        Log.Info("barbarian uses ability");
    }

}