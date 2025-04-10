﻿// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
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

            if (base.CharacterCard.HitPoints > 13)
            {
                //{TMH} deals one target X radiant damage, where X = the difference between his current HP and 13.
                IEnumerator coroutine2 = base.GameController.SelectTargetsAndDealDamage(decisionMaker, source, xDamage, DamageType.Radiant, numberOfTargets, optional: false, requiredTargets, isIrreducible: false, allowAutoDecide: false, autoDecide: false, null, null, null, null, null, selectTargetsEvenIfCannotDealDamage: false, null, null, ignoreBattleZone: false, null, cardSource);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine2);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine2);
				}

				//Reduce [i]The Machine Heart's[/i] HP to 13.
			
                IEnumerator coroutine = base.GameController.SetHP(base.CharacterCard, 13, GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
            

            //Shuffle this card into [i]The Machine Heart's[/i] deck.
            IEnumerator coroutine3 = base.GameController.ShuffleCardIntoLocation(DecisionMaker, base.Card, base.TurnTaker.Deck, optional: false, toBottom: false, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine3);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine3);
			}
		}

		//Calculate Current HP - 13.
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

		//Tell the game to not move this card to the trash after it is moved to the deck.
		public override bool DoNotMoveOneShotToTrash => true;
    }
}