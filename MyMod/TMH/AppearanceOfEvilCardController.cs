// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Handelabra.Sentinels.Engine.Controller.Ra.AppearanceOfEvilCardController
using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

//Legacy Next Evolution, Ra Flesh of Sun God, Wraith Stun Bomb
namespace ISSUEComics
{
	public class AppearanceOfEvilCardController : CardController
	{
		public AppearanceOfEvilCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
		}
		public override IEnumerator UsePower(int index = 0)
		{
			ImmuneToDamageStatusEffect immuneToDamageVillainCharacterStatusEffect = new ImmuneToDamageStatusEffect();
			immuneToDamageVillainCharacterStatusEffect.TargetCriteria.IsVillain = true;			
			immuneToDamageVillainCharacterStatusEffect.TargetCriteria.IsCharacter = true;
			immuneToDamageVillainCharacterStatusEffect.SourceCriteria.IsHeroCharacterCard = true;
			immuneToDamageVillainCharacterStatusEffect.SourceCriteria.IsNotSpecificCard = base.CharacterCard;
			immuneToDamageVillainCharacterStatusEffect.UntilStartOfNextTurn(base.TurnTaker);

			IEnumerator coroutine = AddStatusEffect(immuneToDamageVillainCharacterStatusEffect);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}

			ImmuneToDamageStatusEffect immuneToDamageHeroCharacterStatusEffect = new ImmuneToDamageStatusEffect();
			immuneToDamageHeroCharacterStatusEffect.TargetCriteria.IsSpecificCard = base.CharacterCard;
			immuneToDamageHeroCharacterStatusEffect.SourceCriteria.IsVillain = true;
			immuneToDamageHeroCharacterStatusEffect.SourceCriteria.IsCharacter = false;
			immuneToDamageHeroCharacterStatusEffect.UntilStartOfNextTurn(base.TurnTaker);
		
			IEnumerator coroutine2 = AddStatusEffect(immuneToDamageHeroCharacterStatusEffect);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}

		}
	}
}

