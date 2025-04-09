using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class MixedOrdersCardController : CardController
	{
		public MixedOrdersCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
		}
		public override IEnumerator Play()
		{
            IEnumerator coroutine = DrawCards(base.HeroTurnTakerController, 4);
            IEnumerator playEnumerator = SelectAndPlayCardFromHand(base.HeroTurnTakerController);
            IEnumerator end2 = base.GameController.ImmediatelyEndTurn(base.HeroTurnTakerController, GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
                yield return base.GameController.StartCoroutine(playEnumerator);
                yield return base.GameController.StartCoroutine(end2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
                base.GameController.ExhaustCoroutine(playEnumerator);
                base.GameController.ExhaustCoroutine(end2);
            }
            IEnumerator coroutine2 = GameController.SelectCardAndStoreResults();
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
                yield return base.GameController.StartCoroutine(playEnumerator);
                yield return base.GameController.StartCoroutine(end2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
                base.GameController.ExhaustCoroutine(playEnumerator);
                base.GameController.ExhaustCoroutine(end2);
            }

            ////IntoTheStratosphere
            //List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
            //IEnumerator coroutine = base.GameController.SelectCardAndStoreResults(base.HeroTurnTakerController, SelectionType.MoveCardOnDeck, new LinqCardCriteria((Card c) => !c.IsOneShot && c.IsInPlay && c.IsTarget && IsVillain(c) && base.GameController.IsCardVisibleToCardSource(c, GetCardSource()), "non-indestructible non-character villain cards in play", useCardsSuffix: false), storedResults, optional: false, allowAutoDecide: false, null, includeRealCardsOnly: true, GetCardSource());
            //if (base.UseUnityCoroutines)
            //{
            //	yield return base.GameController.StartCoroutine(coroutine);
            //}
            //else
            //{
            //	base.GameController.ExhaustCoroutine(coroutine);
            //}
            ////Necessary??
            //SelectCardDecision selectCardDecision = storedResults.Where((SelectCardDecision d) => d.Completed).FirstOrDefault();
            //if (selectCardDecision != null && selectCardDecision.SelectedCard != null)
            //{
            //	Card card = selectCardDecision.SelectedCard;
            //	if (selectCardDecision.Choices.Count() == 1)
            //	{
            //		string text = (IsVillain(base.TurnTaker) ? "hero" : "villain");
            //		IEnumerator coroutine2 = base.GameController.SendMessageAction(card.Title + " is the only non-indestructible " + text + " card in play.", Priority.Low, GetCardSource(), selectCardDecision.Choices, showCardSource: true);
            //		if (base.UseUnityCoroutines)
            //		{
            //			yield return base.GameController.StartCoroutine(coroutine2);
            //		}
            //		else
            //		{
            //			base.GameController.ExhaustCoroutine(coroutine2);
            //		}
            //	}
            //	GameController gameController = base.GameController;
            //	TurnTakerController turnTakerController = base.TurnTakerController;
            //	Location nativeDeck = GetNativeDeck(card);
            //	CardSource cardSource = GetCardSource();
            //	IEnumerator coroutine3 = gameController.MoveCard(turnTakerController, card, nativeDeck, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
            //	if (base.UseUnityCoroutines)
            //	{
            //		yield return base.GameController.StartCoroutine(coroutine3);
            //	}
            //	else
            //	{
            //		base.GameController.ExhaustCoroutine(coroutine3);
            //	}
            //}

            ////[]


            //List<DealDamageAction> storedDamage = new List<DealDamageAction>();
            //IEnumerator coroutine4 = DealDamageToHighestHP(base.CharacterCard, 1, (Card c) => IsHeroTarget(c), (Card c) => base.H - 1, DamageType.Psychic, isIrreducible: false, optional: false, storedDamage);
            //if (base.UseUnityCoroutines)
            //{
            //	yield return base.GameController.StartCoroutine(coroutine4);
            //}
            //else
            //{
            //	base.GameController.ExhaustCoroutine(coroutine4);
            //}
            //if (!DidDealDamage(storedDamage))
            //{
            //	yield break;
            //}
            //DealDamageAction dealDamageAction = storedDamage.First();
            //Card target = dealDamageAction.Target;
            //if (dealDamageAction.WasVillainTarget && target.IsInPlayAndHasGameText)
            //{
            //	IEnumerator coroutine5 = DealDamage(target, (Card c) => IsVillainTarget(c), 1, DamageType.Melee);
            //	if (base.UseUnityCoroutines)
            //	{
            //		yield return base.GameController.StartCoroutine(coroutine5);
            //	}
            //	else
            //	{
            //		base.GameController.ExhaustCoroutine(coroutine5);
            //	}
            //}


        }
	}
}
