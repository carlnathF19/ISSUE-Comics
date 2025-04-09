using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class YearsOfExperienceCardController : CardController
	{
		public YearsOfExperienceCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
		}

		public override IEnumerator Play()
		{
			
			HeroTurnTaker heroTurnTaker = base.HeroTurnTakerController.HeroTurnTaker;
			MoveCardDestination[] possibleDestinations = new MoveCardDestination[1]
			{
			new MoveCardDestination(heroTurnTaker.Hand)
			};
			IEnumerator coroutine = base.GameController.SelectCardFromLocationAndMoveIt(base.HeroTurnTakerController, heroTurnTaker.Deck, new LinqCardCriteria((Card card) => card.IsRealCard, "module"), possibleDestinations, isPutIntoPlay: false, playIfMovingToPlayArea: true, shuffleAfterwards: true, optional: false, null, flipFaceDown: false, showOutput: false, null, isDiscardIfMovingtoTrash: false, allowAutoDecide: false, null, null, GetCardSource());
			IEnumerator coroutine2 = base.GameController.SelectCardFromLocationAndMoveIt(base.HeroTurnTakerController, heroTurnTaker.Deck, new LinqCardCriteria((Card card) => card.IsRealCard, "module"), possibleDestinations, isPutIntoPlay: false, playIfMovingToPlayArea: true, shuffleAfterwards: true, optional: false, null, flipFaceDown: false, showOutput: false, null, isDiscardIfMovingtoTrash: false, allowAutoDecide: false, null, null, GetCardSource());
			IEnumerator coroutine3 = base.GameController.SelectCardFromLocationAndMoveIt(base.HeroTurnTakerController, heroTurnTaker.Deck, new LinqCardCriteria((Card card) => card.IsRealCard, "module"), possibleDestinations, isPutIntoPlay: false, playIfMovingToPlayArea: true, shuffleAfterwards: true, optional: false, null, flipFaceDown: false, showOutput: false, null, isDiscardIfMovingtoTrash: false, allowAutoDecide: false, null, null, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
				yield return base.GameController.StartCoroutine(coroutine2);
				yield return base.GameController.StartCoroutine(coroutine3);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
				base.GameController.ExhaustCoroutine(coroutine2);
				base.GameController.ExhaustCoroutine(coroutine3);
			}
			IEnumerator coroutine4 = SelectAndDiscardCards(DecisionMaker, 1);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine4);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine4);
			}
			IEnumerator coroutine5 = ShuffleDeck(DecisionMaker, base.TurnTaker.Deck);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine5);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine5);
			}
			


		}


		//public override IEnumerator Play()
		//{


		//	IEnumerator coroutine = DrawCards(base.HeroTurnTakerController, 4);
		//	IEnumerator playEnumerator = SelectAndPlayCardFromHand(base.HeroTurnTakerController);
		//	IEnumerator end2 = base.GameController.ImmediatelyEndTurn(base.HeroTurnTakerController, GetCardSource());
		//	if (base.UseUnityCoroutines)
		//	{
		//		yield return base.GameController.StartCoroutine(coroutine);
		//		yield return base.GameController.StartCoroutine(playEnumerator);
		//		yield return base.GameController.StartCoroutine(end2);
		//	}
		//	else
		//	{
		//		base.GameController.ExhaustCoroutine(coroutine);
		//		base.GameController.ExhaustCoroutine(playEnumerator);
		//		base.GameController.ExhaustCoroutine(end2);
		//	}
		//}
	}
}
