using Sandbox;

public sealed class Ability : Component
{
	[Property] public int ability_meter_max = 50;
	Player player;
	[Property] public AbilityType ability_type;
	private AbilityTemplate ability;
	protected override void OnStart()
	{
		assignAbility(ability_type);
		player = GameObject.Components.Get<Player>();
		ability.testing();
	}
	protected override void OnUpdate()
	{
		if(Input.Down("attack2") &&  player.ability_meter == ability_meter_max){
			Log.Info("using ability");
			ability.useAbility();
			player.ability_meter = 0;
		}
	}

	private void assignAbility(AbilityType name)
	{
		switch (name)
		{
			case AbilityType.Paladin:
				ability = new PaladinAbility();
				break;
			case AbilityType.Barbarian:
				ability = new BarbarianAbility();
				break;
			case AbilityType.Wizard:
				ability = new WizardAbility();
				break;
			case AbilityType.Priest:
				ability = new PriestAbility();
				break;
			default:
				Log.Info("Class name not found");
				return;
		}
	}

}