// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Handelabra.Sentinels.Engine.Controller.TMH.TMHCharacterCardController

//until the start of your next turn, -1 damage to TMH.
//until the start of your next turn, whenever TMH is dealt damage by a target other than TMH, 
//TMH deals the source of that damage 1 damage of the most recent damage type dealt to TMH.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Controller.VoidGuardWrithe;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
    public class TMHCharacterCardController : HeroCharacterCardController
    {
        public TMHCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        IEnumerator drawCard = base.GameController.SelectHeroToDrawCard(base.HeroTurnTakerController, optionalSelectHero: false, optionalDrawCard: true, allowAutoDecideHero: false, null, null, null, GetCardSource());
                        if (base.UseUnityCoroutines)
                        {
                            yield return base.GameController.StartCoroutine(drawCard);
                        }
                        else
                        {
                            base.GameController.ExhaustCoroutine(drawCard);
                        }
                        break;
                    }

                    //    case 1:
                    //        {
                    //            List<SelectLocationDecision> chosenDeckDecisions = new List<SelectLocationDecision>();
                    //            IEnumerator chooseDeck = base.GameController.SelectADeck(DecisionMaker, SelectionType.RevealCardsFromDeck, (Location l) => l.IsHero && l.HasCards, chosenDeckDecisions, optional: false, "All hero decks are empty! What happened?!", GetCardSource());
                    //            if (base.UseUnityCoroutines)
                    //            {
                    //                yield return base.GameController.StartCoroutine(chooseDeck);
                    //            }
                    //            else
                    //            {
                    //                base.GameController.ExhaustCoroutine(chooseDeck);
                    //            }
                    //            if (DidSelectLocation(chosenDeckDecisions))
                    //            {
                    //                Location chosenDeck = chosenDeckDecisions.FirstOrDefault().SelectedLocation.Location;
                    //                List<Card> revealedCards = new List<Card>();
                    //                List<RevealCardsAction> revealCardActions = new List<RevealCardsAction>();
                    //                IEnumerator revealTopThree = base.GameController.RevealCards(DecisionMaker, chosenDeck, (Card c) => true, 3, revealCardActions, RevealedCardDisplay.Message, GetCardSource());
                    //                if (base.UseUnityCoroutines)
                    //                {
                    //                    yield return base.GameController.StartCoroutine(revealTopThree);
                    //                }
                    //                else
                    //                {
                    //                    base.GameController.ExhaustCoroutine(revealTopThree);
                    //                }
                    //                IEnumerator replaceTopThree = base.GameController.BulkMoveCards(DecisionMaker, revealedCards, chosenDeck, toBottom: false, performBeforeDestroyActions: true, null, isDiscard: false, GetCardSource());
                    //                if (base.UseUnityCoroutines)
                    //                {
                    //                    yield return base.GameController.StartCoroutine(replaceTopThree);
                    //                }
                    //                else
                    //                {
                    //                    base.GameController.ExhaustCoroutine(replaceTopThree);
                    //                }
                    //                int randomNumber = base.Game.RNG.Next(Math.Min(3, chosenDeck.NumberOfCards));
                    //                string nth = "";
                    //                switch (randomNumber)
                    //                {
                    //                    case 0:
                    //                        nth = "first";
                    //                        break;
                    //                    case 1:
                    //                        nth = "second";
                    //                        break;
                    //                    case 2:
                    //                        nth = "third";
                    //                        break;
                    //                }
                    //                string message = base.CharacterCard.Title + " plays the " + nth + " card from the top of " + chosenDeck.GetFriendlyName() + "!";
                    //                base.GameController.SendMessageAction(message, Priority.High, GetCardSource(), null, showCardSource: true);
                    //                Card selectedCard = chosenDeck.GetTopCards(randomNumber + 1).LastOrDefault();
                    //                IEnumerator shuffleDeck = base.GameController.ShuffleLocation(chosenDeck, null, GetCardSource());
                    //                if (base.UseUnityCoroutines)
                    //                {
                    //                    yield return base.GameController.StartCoroutine(shuffleDeck);
                    //                }
                    //                else
                    //                {
                    //                    base.GameController.ExhaustCoroutine(shuffleDeck);
                    //                }
                    //                base.GameController.PlayCard(base.GameController.FindTurnTakerController(selectedCard.Owner), selectedCard, isPutIntoPlay: false, null, optional: false, null, null, evenIfAlreadyInPlay: false, null, null, null, associateCardSource: false, fromBottom: false, canBeCancelled: true, GetCardSource());
                    //            }
                    //            break;
                    //        }
                    //    case 2:

                    //        {
                    //            List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
                    //            IEnumerator coroutine = base.GameController.SelectCardAndStoreResults(DecisionMaker, SelectionType.ReduceDamageDealt, new LinqCardCriteria((Card c) => c.IsInPlay && c.IsTarget, "target", useCardsSuffix: false, useCardsPrefix: false, null, "targets"), storedResults, optional: false, allowAutoDecide: false, null, includeRealCardsOnly: true, GetCardSource());
                    //            if (base.UseUnityCoroutines)
                    //            {
                    //                yield return base.GameController.StartCoroutine(coroutine);
                    //            }
                    //            else
                    //            {
                    //                base.GameController.ExhaustCoroutine(coroutine);
                    //            }
                    //            SelectCardDecision selected = storedResults.FirstOrDefault();
                    //            if (selected != null && selected.SelectedCard != null)
                    //            {
                    //                ReduceDamageStatusEffect reduceDamageStatusEffect = new ReduceDamageStatusEffect(GetPowerNumeral(0, 1));
                    //                reduceDamageStatusEffect.SourceCriteria.IsSpecificCard = selected.SelectedCard;
                    //                reduceDamageStatusEffect.UntilStartOfNextTurn(base.TurnTaker);
                    //                reduceDamageStatusEffect.UntilTargetLeavesPlay(selected.SelectedCard);
                    //                coroutine = AddStatusEffect(reduceDamageStatusEffect);
                    //                if (base.UseUnityCoroutines)
                    //                {
                    //                    yield return base.GameController.StartCoroutine(coroutine);
                    //                }
                    //                else
                    //                {
                    //                    base.GameController.ExhaustCoroutine(coroutine);
                    //                }

                    //            }
                    //        }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            ReduceDamageStatusEffect reduceDamageStatusEffect = new ReduceDamageStatusEffect(GetPowerNumeral(0, 1));
            reduceDamageStatusEffect.TargetCriteria.IsSpecificCard = base.Card;
            reduceDamageStatusEffect.UntilStartOfNextTurn(base.TurnTaker);

            //Will this reduce the damage until the start of the next turn.
            //DamageType? damageTypeThatTMHWillDeal = GetDamageTypeThatTMHWillDeal();

            AddCounterDamageTrigger((DealDamageAction dd) => dd.Target == base.CharacterCard, () => base.CharacterCard, () => base.CharacterCard, oncePerTargetPerTurn: true, 1, DamageType.Energy);

            //OnDealDamageStatusEffect onDealDamageStatusEffect = new OnDealDamageStatusEffect(base.Card, DealDamage, );

            return AddStatusEffect(reduceDamageStatusEffect);

        }
        //private DamageType? GetDamageTypeThatTMHWillDeal()
        //{
        //    DealDamageJournalEntry dealDamageJournalEntry = base.GameController.Game.Journal.MostRecentDealDamageEntry((DealDamageJournalEntry e) => e.TargetCard == base.CharacterCard && e.Amount > 0);
        //    PlayCardJournalEntry playCardJournalEntry = base.GameController.Game.Journal.QueryJournalEntries((PlayCardJournalEntry e) => e.CardPlayed == base.Card).LastOrDefault();
        //    if (playCardJournalEntry != null)
        //    {
        //        int? entryIndex = base.GameController.Game.Journal.GetEntryIndex(dealDamageJournalEntry);
        //        int? entryIndex2 = base.GameController.Game.Journal.GetEntryIndex(playCardJournalEntry);
        //        if (entryIndex.HasValue && entryIndex2.HasValue && entryIndex.Value > entryIndex2.Value)
        //        {
        //            return dealDamageJournalEntry.DamageType;
        //        }
        //    }
        //    return null;
        //}
    }
}