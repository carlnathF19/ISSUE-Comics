// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Handelabra.Sentinels.Engine.Controller.LanternJack.iGrenadeCardController
using System;
using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
//Needs to be updated to TMH location using Handelabra.Sentinels.Engine.Controller.LanternJack; 
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class IGrenadeCardController : TMHOneShotCardController
	{
		public IGrenadeCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
			base.SpecialStringMaker.ShowSpecialString(() => "The difference between The Machine Heart's current and maximum HP is " + GetXDamage() + ".");
		}

		protected override IEnumerator OneShotEffect()
		{
			GameController gameController = base.GameController;
			HeroTurnTakerController decisionMaker = DecisionMaker;
			DamageSource source = new DamageSource(base.GameController, base.CharacterCard);
			int xDamage = GetXDamage();
			int? numberOfTargets = 1;
			int? requiredTargets = 1;
			CardSource cardSource = GetCardSource();

			IEnumerator coroutine = base.GameController.SelectTargetsAndDealDamage(decisionMaker, source, xDamage, DamageType.Radiant, numberOfTargets, optional: false, requiredTargets, isIrreducible: false, allowAutoDecide: false, autoDecide: false, null, null, null, null, null, selectTargetsEvenIfCannotDealDamage: false, null, null, ignoreBattleZone: false, null, cardSource);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}

			IEnumerator coroutine2 = base.GameController.ShuffleCardIntoLocation(DecisionMaker, base.Card, base.TurnTaker.Deck, optional: false, toBottom: false, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine2);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine2);
			}
		}

		private int GetXDamage()
		{
			if (base.CharacterCard.HitPoints.Value > 13)
			{
				return Math.Abs(13 - base.CharacterCard.HitPoints.Value);
			}
			else
			{
				return 0;
			}
		}
	}
}