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
		updateAbilityKey();
	}

	public void updateAbilityKey(){
		if(player.input_type == InputType.Controller){
			updateAbility("ability_con");
		}
		else if(player.input_type == InputType.BaseKeyboard){
			updateAbility("ability");
		}
		else{
			updateAbility("ability_sec");
		}
	}

	public void updateAbility(string input_name){
		if(Input.Down(input_name) &&  player.ability_meter == ability_meter_max){
			Log.Info("using ability");
			ability.useAbility(player);
			player.ability_meter = 0;
		}
	}

	private void assignAbility(AbilityType name)
	{
		switch (name)
		{
			case AbilityType.Paladin:
				ability = GameObject.Components.Create<PaladinAbility>();
				break;
			case AbilityType.Barbarian:
				ability = GameObject.Components.Create<BarbarianAbility>();
				break;
			case AbilityType.Wizard:
				ability = GameObject.Components.Create<WizardAbility>();
				break;
			case AbilityType.Priest:
				ability = GameObject.Components.Create<PriestAbility>();
				break;
			default:
				Log.Info("Class name not found");
				return;
		}
	}

}