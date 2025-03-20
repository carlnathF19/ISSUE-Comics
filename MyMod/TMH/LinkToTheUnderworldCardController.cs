// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Handelabra.Sentinels.Engine.Controller.LaComodora.LinkToTheUnderworldCardController
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


//bobbertoriley — Today at 8:45 PM
//I think you can use GameController.SelectAndPlayCard and just pass in card.UnderCardLocation.Cards.PlayCard is the underlying method and it handles making sure it goes to the right location and the limited rule

namespace ISSUEComics.TMH
{
	public class LinkToTheUnderworldCardController : CardController
	{
		public LinkToTheUnderworldCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
		}

		public override IEnumerator UsePower(int index = 0)
		{

			switch (index)
			{
				case 0:
					{

						IEnumerator coroutine = base.GameController.SelectAndPlayCard(base.HeroTurnTakerController, base.Card.UnderLocation.Cards, false, true);

						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(coroutine);
						}
						else
						{
							base.GameController.ExhaustCoroutine(coroutine);
						}

						break;
					}
				case 1:
					{
						int numberOfTargets = GetPowerNumeral(0, 1);
						//int damageMultiplier = GetPowerNumeral(1, 2); //From Haka
						IEnumerator coroutine = DestroyCardsAndDoActionBasedOnNumberOfCardsDestroyed(DecisionMaker, new LinqCardCriteria((Card c) => c.Location == base.Card.UnderLocation || c.Location == base.Card.BelowLocation, "cards below " + base.Card.Title, useCardsSuffix: false), delegate (int X)
						{
							GameController gameController = base.GameController;
							HeroTurnTakerController decisionMaker = DecisionMaker;
							DamageSource source = new DamageSource(base.GameController, base.CharacterCard);
							int amount = X;
							int? numberOfTargets2 = numberOfTargets;
							int? requiredTargets = numberOfTargets;
							CardSource cardSource = GetCardSource();
							return gameController.SelectTargetsAndDealDamage(decisionMaker, source, amount, DamageType.Infernal, numberOfTargets2, optional: false, requiredTargets, isIrreducible: false, allowAutoDecide: false, autoDecide: false, null, null, null, null, null, selectTargetsEvenIfCannotDealDamage: false, null, null, ignoreBattleZone: false, null, cardSource);
						});
						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(coroutine);
						}
						else
						{
							base.GameController.ExhaustCoroutine(coroutine);
						}
						break;
					}
			}
		}

		public override void AddTriggers()
		{
			AddTrigger((DestroyCardAction dc) => IsHero(dc.CardToDestroy.Card) && IsEquipment(dc.CardToDestroy.Card), PutUnderThisCardResponse, new TriggerType[2]
			{
				TriggerType.MoveCard,
				TriggerType.ChangePostDestroyDestination,
			}, TriggerTiming.After);
			AddBeforeLeavesPlayActions(ReturnCardsToOwnersTrashResponse);
			AddBeforeLeavesPlayAction(ReturnCardsToOwnersTrashResponse, TriggerType.MoveCard);
		}

		private IEnumerator PutUnderThisCardResponse(DestroyCardAction destroyCard)
		{
			List<YesNoCardDecision> storedResults = new List<YesNoCardDecision>();
			IEnumerator coroutine = base.GameController.MakeYesNoCardDecision(base.HeroTurnTakerController, SelectionType.MoveCardToUnderCard, destroyCard.CardToDestroy.Card, null, storedResults, null, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			if (DidPlayerAnswerYes(storedResults))
			{
				destroyCard.SetPostDestroyDestination(base.Card.UnderLocation, toBottom: false, storedResults.CastEnumerable<YesNoCardDecision, IDecision>());
			}
		}

		private IEnumerator ReturnCardsToOwnersTrashResponse(GameAction gameAction)
		{
			IEnumerator coroutine = ReturnCardsToOwnersTrashResponse();
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
		}

		private IEnumerator ReturnCardsToOwnersTrashResponse()
		{
			Card thisCard = base.Card;
			Console.WriteLine("\tLooking at cards under " + thisCard.Title + ": " + thisCard.UnderLocation.Cards.Select((Card c) => c.Title).ToCommaList());
			while (thisCard.UnderLocation.Cards.Count() > 0)
			{
				Card topCard = thisCard.UnderLocation.TopCard;
				MoveCardDestination trashDestination = FindCardController(topCard).GetTrashDestination();
				GameController gameController = base.GameController;
				TurnTakerController turnTakerController = base.TurnTakerController;
				Location location = trashDestination.Location;
				bool toBottom = trashDestination.ToBottom;
				CardSource cardSource = GetCardSource();
				IEnumerator coroutine = gameController.MoveCard(turnTakerController, topCard, location, toBottom, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
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