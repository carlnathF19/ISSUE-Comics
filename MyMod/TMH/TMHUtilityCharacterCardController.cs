// ISSUEComicsMod, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// ISSUEComics.TheMachineHeart.TheMachineHeartUtilityCharacterCardController
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics
{
	public class TheMachineHeartUtilityCharacterCardController : HeroCharacterCardController
	{
		public static readonly string TheMachineHeartSolarPoolIdentifier = "TheMachineHeartSolarPool";

		public readonly string TheMachineHeartIdentifier = "TheMachineHeart";

		public TheMachineHeartUtilityCharacterCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
			AllowFastCoroutinesDuringPretend = false;
			SpecialString specialString = base.SpecialStringMaker.ShowTokenPool(GetSolarTokenPool());
			specialString.ShowWhileIncapacitated = true;
		}
		//The AddTriggers is correct see Modrossus in ILSpy
		public override void AddTriggers()
		{
			//AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, StartOfTurnAction, TriggerType.AddTokensToPool);
		}

		protected TokenPool GetSolarTokenPool()
		{
			TokenPool tokenPool = base.Card.FindTokenPool(TheMachineHeartSolarPoolIdentifier);
			if (tokenPool == null)
			{
				tokenPool = base.CardWithoutReplacements.FindTokenPool(TheMachineHeartSolarPoolIdentifier);
			}
			return tokenPool;
		}

		protected IEnumerator SpendTokenResponse(DealDamageAction dd)
		{
			TokenPool solarPool = GetSolarTokenPool();
			List<RemoveTokensFromPoolAction> storedResults = new List<RemoveTokensFromPoolAction>();
			IEnumerator coroutine3 = RemoveTokensFromsolarPool(solarPool, 1, storedResults, optional: true, null, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine3);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine3);
			}
			if (!DidRemoveTokens(storedResults))
			{
				yield break;
			}
			List<SelectDamageTypeDecision> storedDamageTypeResults = new List<SelectDamageTypeDecision>();
			coroutine3 = base.GameController.SelectDamageType(base.HeroTurnTakerController, storedDamageTypeResults, null, null, SelectionType.DamageType, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine3);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine3);
			}
			DamageType? damageType = GetSelectedDamageType(storedDamageTypeResults);
			if (damageType.HasValue)
			{
				coroutine3 = base.GameController.ChangeDamageType(dd, damageType.Value, GetCardSource());
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine3);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine3);
				}
			}
		}

		public override IEnumerator BeforeFlipCardImmediateResponse(FlipCardAction flip)
		{
			if (base.CardWithoutReplacements.IsFlipped)
			{
				yield break;
			}
			IEnumerable<Card> enumerable = from c in base.TurnTakerControllerWithoutReplacements.TurnTaker.GetAllCards()
										   where c.Location.IsEnvironment && (c.Location.IsDeck || c.Location.IsTrash)
										   select c;
			if (enumerable.Count() > 0)
			{
				IEnumerator coroutine = RemoveCardsFromGame(enumerable);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine);
				}
			}
			if (base.CardWithoutReplacements.IsTarget)
			{
				RemoveTargetAction action = new RemoveTargetAction(base.GameController, base.CardWithoutReplacements);
				IEnumerator coroutine2 = DoAction(action);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine2);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine2);
				}
			}
			if (base.TurnTakerControllerWithoutReplacements.IsIncapacitatedOrOutOfGame)
			{
				base.GameController.RemoveTriggers((ITrigger t) => t.CardSource != null && t.CardSource.CardController.TurnTakerControllerWithoutReplacements == base.TurnTakerControllerWithoutReplacements && !t.IsStatusEffect && !t.IsOutOfPlayTrigger && t.CardSource != null && (t.CardSource.CardController == this || !t.CardSource.Card.IsInPlayAndNotUnderCard));
				IEnumerable<Card> cards = base.TurnTakerControllerWithoutReplacements.TurnTaker.GetAllCards();
				List<Card> list = new List<Card>();
				list.AddRange(cards.Where((Card c) => !IsHeroCharacterCard(c)).SelectMany((Card c) => c.UnderLocation.Cards.Where((Card co) => co.Owner != base.TurnTakerControllerWithoutReplacements.TurnTaker)));
				if (IsHero(base.TurnTakerControllerWithoutReplacements.TurnTaker))
				{
					HeroTurnTaker heroTurnTaker = base.TurnTakerControllerWithoutReplacements.ToHero().HeroTurnTaker;
					list.AddRange(from c in heroTurnTaker.Hand.Cards.Union(heroTurnTaker.Deck.Cards).Union(heroTurnTaker.Trash.Cards)
								  where c.Owner != base.TurnTakerControllerWithoutReplacements.TurnTaker
								  select c);
				}
				foreach (Card item in list)
				{
					MoveCardDestination trashDestination = FindCardController(item).GetTrashDestination();
					IEnumerator coroutine3 = base.GameController.MoveCard(base.TurnTakerController, item, trashDestination.Location, trashDestination.ToBottom);
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine3);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine3);
					}
				}
				if (base.Game.IsOblivAeonMode)
				{
					IEnumerable<Card> enumerable2 = FindCardsWhere((Card c) => c.IsObjective && c.IsInLocation(base.TurnTakerControllerWithoutReplacements.TurnTaker.PlayArea));
					Log.Debug("Objectives: " + enumerable2.ToRecursiveString());
					if (enumerable2.Count() > 0)
					{
						TurnTaker turnTaker = FindTurnTakersWhere((TurnTaker tt) => tt.Identifier == "OblivAeon", ignoreBattleZone: true).FirstOrDefault();
						IEnumerator coroutine4 = base.GameController.BulkMoveCards(base.TurnTakerController, enumerable2, turnTaker.FindSubDeck("MissionDeck"), toBottom: true, performBeforeDestroyActions: true, null, isDiscard: false, GetCardSource());
						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(coroutine4);
						}
						else
						{
							base.GameController.ExhaustCoroutine(coroutine4);
						}
					}
					IEnumerable<Card> enumerable3 = FindCardsWhere((Card c) => c.DoKeywordsContain("reward", evenIfUnderCard: true, evenIfFaceDown: true) && !c.IsOutOfGame && c.Owner == base.TurnTakerControllerWithoutReplacements.TurnTaker && (!c.IsUnderCard || !c.Location.OwnerCard.IsIncapacitated), realCardsOnly: false, null, ignoreBattleZone: true);
					if (enumerable3.Count() > 0)
					{
						IEnumerator coroutine5 = base.GameController.BulkMoveCards(base.TurnTakerController, enumerable3, base.Card.UnderLocation);
						if (base.UseUnityCoroutines)
						{
							yield return base.GameController.StartCoroutine(coroutine5);
						}
						else
						{
							base.GameController.ExhaustCoroutine(coroutine5);
						}
					}
				}
				IEnumerator coroutine6 = RemoveCardsFromGame(cards);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine6);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine6);
				}
				IEnumerator coroutine7 = base.GameController.UpdateTurnPhasesForTurnTaker(base.TurnTakerControllerWithoutReplacements, incapacitated: true);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine7);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine7);
				}
			}
			else
			{
				RemoveAllTriggers();
			}
		}

		public IEnumerator RemoveTokensFromsolarPool(TokenPool pool, int numberOfTokens, List<RemoveTokensFromPoolAction> storedResults = null, bool optional = false, GameAction gameAction = null, CardSource cardSource = null)
		{
			bool proceed = true;
			if (optional)
			{
				proceed = false;
				SelectionType type = SelectionType.Custom;
				if (pool.CurrentValue < numberOfTokens)
				{
					type = SelectionType.RemoveTokensToNoEffect;
				}
				YesNoAmountDecision yesNo = new YesNoAmountDecision(base.GameController, FindCardController(pool.CardWithTokenPool).DecisionMaker, type, numberOfTokens, upTo: false, requireUnanimous: false, gameAction, null, cardSource);
				IEnumerator coroutine = base.GameController.MakeDecisionAction(yesNo);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine);
				}
				if (yesNo.Answer.HasValue)
				{
					proceed = yesNo.Answer.Value;
				}
			}
			if (proceed)
			{
				RemoveTokensFromPoolAction removeTokensFromPoolAction = ((cardSource == null) ? new RemoveTokensFromPoolAction(base.GameController, pool, numberOfTokens) : new RemoveTokensFromPoolAction(cardSource, pool, numberOfTokens));
				storedResults?.Add(removeTokensFromPoolAction);
				IEnumerator coroutine2 = DoAction(removeTokensFromPoolAction);
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

		public override CustomDecisionText GetCustomDecisionText(IDecision decision)
		{
			return new CustomDecisionText("Do you want to remove an element token to change the damage type?", "Should they remove an element token to change the damage type?", "Vote for if they should remove an element token to change the damage type?", "remove an element token to change the damage type");
		}
	}
}