using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class TMHNonVitalHitCardController : CardController
	{
		public TMHNonVitalHitCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
		}

		public override void AddTriggers()
		{
			AddImmuneToDamageTrigger((DealDamageAction d) =>
				d.Target == base.CharacterCard &&
				(d.DamageType == DamageType.Melee || d.DamageType == DamageType.Projectile));
		}

		public override IEnumerator UsePower(int index = 0)
		{
			ImmuneToDamageStatusEffect immuneToDamageStatusEffect = new ImmuneToDamageStatusEffect();
			immuneToDamageStatusEffect.TargetCriteria.IsSpecificCard = base.CharacterCard;
			immuneToDamageStatusEffect.UntilStartOfNextTurn(base.TurnTaker);
			immuneToDamageStatusEffect.CardDestroyedExpiryCriteria.Card = base.CharacterCard;
			IEnumerator coroutine1 = AddStatusEffect(immuneToDamageStatusEffect);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine1);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine1);
			}

			GameController gameController = base.GameController;
			HeroTurnTakerController decisionMaker = DecisionMaker;
			Card card = base.Card;
			CardSource cardSource = GetCardSource();
			IEnumerator coroutine2 = gameController.DestroyCard(decisionMaker, card, optional: false, null, null, null, null, null, null, null, null, cardSource);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine2);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine2);
			}
		}


	}
} 
