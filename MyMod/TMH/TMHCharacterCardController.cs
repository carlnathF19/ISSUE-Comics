﻿// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
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


                case 1:
                    {
                        List<Card> cards = new List<Card>();
                        IEnumerator coroutine = base.GameController.RevealCards(base.HeroTurnTakerController, FindEnvironment().TurnTaker.Deck, 1, cards, fromBottom: false, RevealedCardDisplay.None, null, GetCardSource());
                        if (base.UseUnityCoroutines)
                        {
                            yield return base.GameController.StartCoroutine(coroutine);
                        }
                        else
                        {
                            base.GameController.ExhaustCoroutine(coroutine);
                        }
                        Card revealedCard = GetRevealedCard(cards);
                        if (revealedCard != null)
                        {
                            YesNoDecision yesNo = new YesNoCardDecision(base.GameController, DecisionMaker, SelectionType.DiscardCard, revealedCard, null, null, GetCardSource());
                            List<IDecision> decisionSources = new List<IDecision> { yesNo };
                            IEnumerator coroutine2 = base.GameController.MakeDecisionAction(yesNo);
                            if (base.UseUnityCoroutines)
                            {
                                yield return base.GameController.StartCoroutine(coroutine2);
                            }
                            else
                            {
                                base.GameController.ExhaustCoroutine(coroutine2);
                            }
                            if (DidPlayerAnswerYes(yesNo))
                            {
                                IEnumerator coroutine3 = base.GameController.DiscardCard(DecisionMaker, revealedCard, decisionSources, null, null, GetCardSource());
                                if (base.UseUnityCoroutines)
                                {
                                    yield return base.GameController.StartCoroutine(coroutine3);
                                }
                                else
                                {
                                    base.GameController.ExhaustCoroutine(coroutine3);
                                }
                            }
                            if (yesNo != null && yesNo.Completed && yesNo.Answer.HasValue)
                            {
                                decisionSources.Add(yesNo);
                                if (!yesNo.Answer.Value)
                                {
                                    GameController gameController = base.GameController;
                                    TurnTakerController turnTakerController = base.TurnTakerController;
                                    Location deck = FindEnvironment().TurnTaker.Deck;
                                    CardSource cardSource = GetCardSource();
                                    IEnumerator coroutine4 = gameController.MoveCard(turnTakerController, revealedCard, deck, toBottom: false, isPutIntoPlay: false, playCardIfMovingToPlayArea: true, null, showMessage: false, null, null, null, evenIfIndestructible: false, flipFaceDown: false, null, isDiscard: false, evenIfPretendGameOver: false, shuffledTrashIntoDeck: false, doesNotEnterPlay: false, cardSource);
                                    if (base.UseUnityCoroutines)
                                    {
                                        yield return base.GameController.StartCoroutine(coroutine4);
                                    }
                                    else
                                    {
                                        base.GameController.ExhaustCoroutine(coroutine4);
                                    }
                                }
                            }
                        }
                        IEnumerator coroutine5 = CleanupCardsAtLocations(new List<Location> { base.TurnTaker.Revealed }, FindEnvironment().TurnTaker.Deck, toBottom: false, addInhibitorException: true, shuffleAfterwards: false, sendMessage: false, isDiscard: false, isReturnedToOriginalLocation: false, cards);
                        if (base.UseUnityCoroutines)
                        {
                            yield return base.GameController.StartCoroutine(coroutine5);
                        }
                        else
                        {
                            base.GameController.ExhaustCoroutine(coroutine5);
                        }


                        break;
                    }
                case 2:
                    {

                        CannotDealDamageStatusEffect cannotDealDamageStatusEffect = new CannotDealDamageStatusEffect();
                        cannotDealDamageStatusEffect.TargetCriteria.IsHero = true;
                        cannotDealDamageStatusEffect.NumberOfUses = 1;
                        cannotDealDamageStatusEffect.IsPreventEffect = true;
                        cannotDealDamageStatusEffect.BattleZoneSource = base.CharacterCard;

                        IEnumerator TMHPreventDamage = AddStatusEffect(cannotDealDamageStatusEffect);
                        if (base.UseUnityCoroutines)
                        {
                            yield return base.GameController.StartCoroutine(TMHPreventDamage);
                        }
                        else
                        {
                            base.GameController.ExhaustCoroutine(TMHPreventDamage);
                        }                      

                        break;
                    }                                   
            }
        }
        public override IEnumerator UsePower(int index = 0)
        {
            //Void Guard Road Warrior base power
            int powerNumeral = GetPowerNumeral(0, 1);
            int[] powerNumerals = new int[1] { powerNumeral };
            OnDealDamageStatusEffect onDealDamageStatusEffect = new OnDealDamageStatusEffect(base.CardWithoutReplacements, "CounterDamageResponse", $"Whenever a target deals damage to {base.Card.Title}, he may reflect (deal) {powerNumeral} damage of that type to that target.", new TriggerType[1] { TriggerType.DealDamage }, base.TurnTaker, base.Card, powerNumerals);
            onDealDamageStatusEffect.SourceCriteria.IsTarget = true;
            onDealDamageStatusEffect.TargetCriteria.IsSpecificCard = base.Card;
            onDealDamageStatusEffect.DamageAmountCriteria.GreaterThan = 0;
            onDealDamageStatusEffect.UntilStartOfNextTurn(base.TurnTaker);
            onDealDamageStatusEffect.UntilTargetLeavesPlay(base.Card);
            onDealDamageStatusEffect.BeforeOrAfter = BeforeOrAfter.After;
            onDealDamageStatusEffect.DoesDealDamage = true;
            IEnumerator coroutine = AddStatusEffect(onDealDamageStatusEffect);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }
        public IEnumerator CounterDamageResponse(DealDamageAction dd, TurnTaker hero, StatusEffect effect, int[] powerNumerals = null)
        {
            DamageType? damageTypeThatTMHWillReflect = GetDamageTypeThatTMHWillReflect();
            int? num = null;
            if (powerNumerals != null)
            {
                num = powerNumerals.ElementAtOrDefault(0);
            }
            if (!num.HasValue)
            {
                num = 1;
            }
            if (dd.DamageSource.IsCard)
            {
                Card source = base.Card;
                if (hero != null && hero.IsPlayer)
                {
                    source = hero.CharacterCard;
                }

                IEnumerator coroutine = DealDamage(source, dd.DamageSource.Card, num.Value, damageTypeThatTMHWillReflect.Value /* DamageType.Melee*/, isIrreducible: false, optional: true, isCounterDamage: true, null, null, null, ignoreBattleZone: false, GetCardSource(effect));
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
       
        ////AdaptivePlatingSubroutine
        private DamageType? GetDamageTypeThatTMHWillReflect()
        {
            DealDamageJournalEntry dealDamageJournalEntry = base.GameController.Game.Journal.MostRecentDealDamageEntry((DealDamageJournalEntry e) => e.TargetCard == base.CharacterCard && e.Amount > 0);
            PlayCardJournalEntry playCardJournalEntry = base.GameController.Game.Journal.QueryJournalEntries((PlayCardJournalEntry e) => e.CardPlayed == base.Card).LastOrDefault();
            //if (playCardJournalEntry != null)
            //{
                int? entryIndex = base.GameController.Game.Journal.GetEntryIndex(dealDamageJournalEntry);
                //int? entryIndex2 = base.GameController.Game.Journal.GetEntryIndex(playCardJournalEntry);
                if (entryIndex.HasValue)
                {
                    return dealDamageJournalEntry.DamageType;
                }
            //}
            return null;

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