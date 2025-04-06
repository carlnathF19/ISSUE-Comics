using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
    public class AppearanceOfGoodCardController : CardController
    { 

        public AppearanceOfGoodCardController(Card card, TurnTakerController turnTakerController)
           : base(card, turnTakerController)
        {
        }
       public override void AddTriggers()
	    {
            AddReduceDamageTrigger((Card c) => IsHeroCharacterCard(c), 1);
            AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, base.DestroyThisCardResponse, TriggerType.DestroySelf);
            AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, (PhaseChangeAction p) => base.GameController.GainHP(base.CharacterCard, 2, null, null, GetCardSource()), TriggerType.GainHP, null, ignoreBattleZone: true);
        }
    }
}
