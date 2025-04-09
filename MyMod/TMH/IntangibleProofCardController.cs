using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace ISSUEComics.TMH
{
	public class IntangibleProofCardController : CardController
	{
		public IntangibleProofCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
		}
		public override void AddTriggers()
		{
		}
		public override IEnumerator Play()
		{
            //Reveal the Top card of the Villain deck and the top card of The Machine Hearts deck.  You may place These cards on the top or bottom of their deck.   Draw a Card.
            //Infrared Eyepiece
            List<SelectLocationDecision> storedResults = new List<SelectLocationDecision>();
			List<SelectLocationDecision> storedResults2 = new List<SelectLocationDecision>();

			IEnumerator coroutine = FindVillainDeck(DecisionMaker, SelectionType.RevealCardsFromDeck, storedResults, (Location l) => true);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			Location deck = GetSelectedLocation(storedResults);
			IEnumerator coroutine2 = TakeAPeekResponse(deck);
			IEnumerator coroutine3 = TakeAPeekResponse(this.TurnTaker.Deck);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine2);
				yield return base.GameController.StartCoroutine(coroutine3);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine2);
				base.GameController.ExhaustCoroutine(coroutine3);
			}
			
			IEnumerator coroutine4 = DrawCard(base.HeroTurnTakerController.HeroTurnTaker, optional: false);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine4);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine4);
			}
		}

        private IEnumerator TakeAPeekResponse(Location deck)
        {
            //Modified from Parse's card Updated Intel
            if (deck == null)
            {
                yield break;
            }
            List<Card> storedResultsCard = new List<Card>();
            IEnumerator coroutine = base.GameController.RevealCards(base.TurnTakerController, deck, 1, storedResultsCard, fromBottom: false, RevealedCardDisplay.None, null, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            Card card = storedResultsCard.FirstOrDefault();
            if (card != null)
            {
                List<MoveCardDestination> list = new List<MoveCardDestination>();
                list.Add(new MoveCardDestination(deck));
                list.Add(new MoveCardDestination(deck, toBottom: true));
                GameController gameController = base.GameController;
                HeroTurnTakerController decisionMaker = DecisionMaker;
                CardSource cardSource = GetCardSource();
                coroutine = gameController.SelectLocationAndMoveCard(decisionMaker, card, list, isPutIntoPlay: false, playIfMovingToPlayArea: true, null, null, null, flipFaceDown: false, showOutput: false, null, isDiscardIfMovingToTrash: false, cardSource);
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
            List<Location> list2 = new List<Location>();
            list2.Add(deck.OwnerTurnTaker.Revealed);
            coroutine = base.GameController.CleanupCardsAtLocations(base.TurnTakerController, list2, deck, toBottom: false, addInhibitorException: true, shuffleAfterwards: false, sendMessage: false, isDiscard: false, isReturnedToOriginalLocation: true, storedResultsCard, GetCardSource());
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