using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class SideKickCardController : CardController
	{
		public SideKickCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
		}
		//From ImpaleCardController
		public override IEnumerator DeterminePlayLocation(List<MoveCardDestination> storedResults, bool isPutIntoPlay, List<IDecision> decisionSources, Location overridePlayArea = null, LinqTurnTakerCriteria additionalTurnTakerCriteria = null)
		{
			return SelectCardThisCardWillMoveNextTo(new LinqCardCriteria((Card c) => c.IsTarget && IsHeroCharacterCard(c), "Hero Characters", useCardsSuffix: false), storedResults, isPutIntoPlay, decisionSources);
		}

        public override void AddTriggers()
        {
            AddRedirectDamageTrigger((DealDamageAction dd) => dd.DamageSource.IsCard && dd.DamageSource.Card.IsTarget && dd.Target == GetCardThisCardIsNextTo(), () => base.CharacterCard, optional: true);
        }


    }
}


