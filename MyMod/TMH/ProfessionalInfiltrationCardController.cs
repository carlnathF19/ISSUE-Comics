// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Handelabra.Sentinels.Engine.Controller.Tachyon.ProfessionalInfiltrationCardController

//Tachyon Pushing The Limits, Pterodactyl Insula Prime, Captain Thunder Shocking Rebuke, Psuedo Try Not To think about it, Haka Savage Mana
//Special thanks to n202015226ac, and the other members of the Sentinels Custom Playtesting Discord
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class ProfessionalInfiltrationCardController : CardController
	{
		public override bool DoesHaveActivePlayMethod => false;

		public ProfessionalInfiltrationCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
			base.SpecialStringMaker.ShowNumberOfCardsUnderCard(base.Card);
		}

		public override void AddTriggers()
		{
			AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, (PhaseChangeAction p) => DealDamageOrDestroyThisCardResponse(p, base.CharacterCard, base.CharacterCard, 3, DamageType.Psychic), new TriggerType[2]
			{
				TriggerType.DealDamage,
				TriggerType.DestroySelf
			}
			);
			AddBeforeLeavesPlayActions(PlayCardsToOwnersPlayAreaResponse);
			AddBeforeLeavesPlayAction(PlayCardsToOwnersPlayAreaResponse, TriggerType.MoveCard);
		}

		public override IEnumerator Play()
		{
			/* Villain deck search, under card, and shuffle*/
			List<SelectLocationDecision> storedVillain = new List<SelectLocationDecision>();
			IEnumerator coroutine = FindVillainDeck(DecisionMaker, SelectionType.MoveCardOnDeck, storedVillain, (Location l) => true);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			Location deck = GetSelectedLocation(storedVillain);
			if (deck == null)
			{
				yield break;
			}


			List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
			GameController gameController = base.GameController;
			HeroTurnTakerController heroTurnTakerController = base.HeroTurnTakerController;
			LinqCardCriteria cardCriteria = new LinqCardCriteria((Card c) => c.Location == deck);
			Func<Card, IEnumerator> actionWithCard = delegate (Card c)
			{
				GameController gameController3 = base.GameController;
				HeroTurnTakerController heroTurnTakerController3 = base.HeroTurnTakerController;
				//Location offToTheSide = base.TurnTaker.OffToTheSide;
				CardSource cardSource2 = GetCardSource();
				return gameController3.MoveCard(heroTurnTakerController3, c, base.Card.UnderLocation, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource2);
			};
			int? numberOfCards = 1;
			List<SelectCardDecision> storedResults2 = storedResults;
			CardSource cardSource = GetCardSource();
			coroutine = gameController.SelectCardsAndDoAction(heroTurnTakerController, cardCriteria, SelectionType.SearchDeck, actionWithCard, numberOfCards, optional: false, null, storedResults2, allowAutoDecide: false, null, cardSource);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			coroutine = ShuffleDeck(DecisionMaker, deck);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			if (DidSelectCard(storedResults))
			{
				Card selectedCard = GetSelectedCard(storedResults);
				GameController gameController2 = base.GameController;
				HeroTurnTakerController heroTurnTakerController2 = base.HeroTurnTakerController;
				MoveCardDestination[] possibleDestinations = new MoveCardDestination[1]
				{
					new MoveCardDestination(base.Card.UnderLocation, toBottom: false, showMessage: true),
					//new MoveCardDestination(deck, toBottom: true, showMessage: true)
				};
				cardSource = GetCardSource();
				coroutine = gameController2.SelectLocationAndMoveCard(heroTurnTakerController2, selectedCard, possibleDestinations, isPutIntoPlay: false, playIfMovingToPlayArea: true, null, null, null, flipFaceDown: false, showOutput: false, null, isDiscardIfMovingToTrash: false, cardSource);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine);
				}
			}


			//			n202015226ac — Today at 8:10 PM
			//Use GameController.SelectCardFromLocationAndMoveIt
			//Define the location to move from as the environment deck first.
			//And the location to move to is under that card.  If you need an example of a card using that, Shortcut in Impulse's deck in BartKF's mod uses it.
			//			ILSpy can give you all the examples, actually, so you don't need me to list ones.
			//Good luck.

			Location location = FindEnvironment().TurnTaker.Deck;
			Location location2 = Card.UnderLocation;

			IEnumerator coroutineEnvironment = GameController.SelectCardFromLocationAndMoveIt(base.HeroTurnTakerController, location, new LinqCardCriteria((Card c) => true), new List<MoveCardDestination> { new MoveCardDestination(location2) }, isPutIntoPlay: false, playIfMovingToPlayArea: false, shuffleAfterwards: true, optional: false, null, flipFaceDown: false, showOutput: true, base.TurnTaker, isDiscardIfMovingtoTrash: true, allowAutoDecide: false, null, null, GetCardSource());

			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutineEnvironment);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutineEnvironment);
			}

		}

		private IEnumerator PlayCardsToOwnersPlayAreaResponse(GameAction gameAction)
		{
			IEnumerator coroutine = PlayCardsToOwnersPlayAreaResponse();
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
		}
		private IEnumerator PlayCardsToOwnersPlayAreaResponse()
		{
			Card thisCard = base.Card;
			//Console.WriteLine("\tLooking at cards under " + thisCard.Title + ": " + thisCard.UnderLocation.Cards.Select((Card c) => c.Title).ToCommaList());
			while (thisCard.UnderLocation.Cards.Count() > 0)
			{
				Card topCard = thisCard.UnderLocation.TopCard;
				//MoveCardDestination PlayAreaDestination = FindCardController(topCard).GetDeckDestination();
				GameController gameController = base.GameController;
				TurnTakerController turnTakerController = base.TurnTakerController;
				Location location = topCard.Owner.PlayArea; //PlayAreaDestination.Location;
															//bool toBottom = PlayAreaDestination.ToBottom;
				CardSource cardSource = GetCardSource();
				IEnumerator coroutine = gameController.MoveCard(turnTakerController, topCard, location, false, isPutIntoPlay: true, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
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
}