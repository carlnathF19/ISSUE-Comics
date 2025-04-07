using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class ReconCardController : CardController
	{
		public ReconCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
		}
		public override IEnumerator Play()
		{
			//IEnumerator blahcoroutine = DrawCards(base.HeroTurnTakerController, 4);
			//IEnumerator playEnumerator = SelectAndPlayCardFromHand(base.HeroTurnTakerController);
			//IEnumerator end2 = base.GameController.ImmediatelyEndTurn(base.HeroTurnTakerController, GetCardSource());
			//if (base.UseUnityCoroutines)
			//{
			//	yield return base.GameController.StartCoroutine(blahcoroutine);
			//	yield return base.GameController.StartCoroutine(playEnumerator);
			//	yield return base.GameController.StartCoroutine(end2);
			//}
			//else
			//{
			//	base.GameController.ExhaustCoroutine(blahcoroutine);
			//	base.GameController.ExhaustCoroutine(playEnumerator);
			//	base.GameController.ExhaustCoroutine(end2);
			//}
			//Inconceivable Obstruction Kismet
			List<RevealCardsAction> storedResults = new List<RevealCardsAction>();
			IEnumerator coroutine = base.GameController.RevealCards(base.TurnTakerController, FindEnvironment().TurnTaker.Deck, 1, null, fromBottom: false, RevealedCardDisplay.Message, storedResults, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}

			List<Card> revealed = GetRevealedCards(storedResults, includeRemoved: true);
			Card card = revealed.FirstOrDefault();
			if (card != null)
			{
				if (card.IsTarget)
				{
					GameController gameController = base.GameController;
					TurnTakerController turnTakerController2 = base.TurnTakerController;
					Location trash = FindEnvironment().TurnTaker.Trash;
					CardSource cardSource = GetCardSource();
					IEnumerator gainHpRoutine = base.GameController.GainHP(base.CharacterCard, 2, null, null, GetCardSource());
					IEnumerator coroutine2 = gameController.MoveCard(turnTakerController2, card, trash, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine2);
						yield return base.GameController.StartCoroutine(gainHpRoutine);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine2);
						base.GameController.ExhaustCoroutine(gainHpRoutine);
					}
				}
				else
				{
					GameController gameController = base.GameController;
					TurnTakerController turnTakerController2 = base.TurnTakerController;
					Location deck = FindEnvironment().TurnTaker.Deck;
					CardSource cardSource2 = GetCardSource();
					IEnumerator coroutine3 = gameController.MoveCard(turnTakerController2, card, deck, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: false, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource2);
					IEnumerator coroutine4 = base.GameController.SelectAndUsePower(DecisionMaker, optional: true, null, 1, eliminateUsedPowers: true, null, showMessage: false, allowAnyHeroPower: false, allowReplacements: true, canBeCancelled: true, null, forceDecision: false, allowOutOfPlayPower: false, GetCardSource());
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine3);
						yield return base.GameController.StartCoroutine(coroutine4);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine3);
						base.GameController.ExhaustCoroutine(coroutine4);
					}
				}
			}
			List<Location> list = new List<Location>();
			list.Add(FindEnvironment().TurnTaker.Revealed);
			coroutine = base.GameController.CleanupCardsAtLocations(base.TurnTakerController, list, FindEnvironment().TurnTaker.Deck, toBottom: false, addInhibitorException: true, shuffleAfterwards: false, sendMessage: false, isDiscard: false, isReturnedToOriginalLocation: true, revealed, GetCardSource());
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
