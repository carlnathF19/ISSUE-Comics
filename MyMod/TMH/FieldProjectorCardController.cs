using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class FieldProjectorCardController : CardController
	{
		public FieldProjectorCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
		}
		public override IEnumerator UsePower(int index = 0)
		{
			int powerNumeral = GetPowerNumeral(0, 1);
			int powerNumeral2 = GetPowerNumeral(1, 2);
			int powerNumeral3 = GetPowerNumeral(2, 1);
			GameController gameController = base.GameController;
			HeroTurnTakerController decisionMaker = DecisionMaker;
			DamageSource source = new DamageSource(base.GameController, base.CharacterCard);
			int? numberOfTargets = powerNumeral3;
			int? requiredTargets = powerNumeral3;
			CardSource cardSource = GetCardSource();
			IEnumerator coroutine = gameController.SelectTargetsAndDealDamage(decisionMaker, source, powerNumeral2, DamageType.Radiant, numberOfTargets, optional: false, requiredTargets, isIrreducible: false, allowAutoDecide: false, autoDecide: false, null, null, null, null, null, selectTargetsEvenIfCannotDealDamage: false, null, null, ignoreBattleZone: false, null, cardSource);
			IEnumerator coroutine2 = DealDamage(base.CharacterCard, base.CharacterCard, powerNumeral, DamageType.Energy, isIrreducible: false, optional: true, isCounterDamage: false, null);

			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine2);
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine2);
				base.GameController.ExhaustCoroutine(coroutine);
			}
		}
	}
}




