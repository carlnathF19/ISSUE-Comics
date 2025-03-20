using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace ISSUEComics.TMH
{
    public class HolographicDisguisesCardController : CardController
    {
        public HolographicDisguisesCardController(Card card, TurnTakerController turnTakerController)
           : base(card, turnTakerController)
        {
        }
        public override void AddTriggers()
        {
            AddMakeDamageIrreducibleTrigger((DealDamageAction dd) => dd.DamageSource.IsHeroTarget);
            AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, delegate
            {
                GameController gameController = base.GameController;
                HeroTurnTakerController decisionMaker = DecisionMaker;
                Card card = base.Card;
                CardSource cardSource = GetCardSource();
                return gameController.DestroyCard(decisionMaker, card, optional: false, null, null, null, null, null, null, null, null, cardSource);
            }, TriggerType.DestroySelf);
        }
    }
}