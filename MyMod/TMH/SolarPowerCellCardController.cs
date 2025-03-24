using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
	public class SolarPowerCellCardController : CardController
	{
		public SolarPowerCellCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
            base.SpecialStringMaker.ShowTokenPool(this.Card.Identifier, "SolarPowerCellPool");
        }

        private TokenPool SolarPowerCellPool { get { return base.Card.FindTokenPool("SolarPowerCellPool"); } }

        public override IEnumerator Play()
		{
            //Make sure this card has no tokens on it when it enteres play.
            IEnumerator coroutine = ResetTokenValue();
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        public IEnumerator ResetTokenValue()
        {
            SolarPowerCellPool.SetToInitialValue();
            yield return null;
        }

        public override void AddTriggers()
        {
            //At the start of your turn, place a token on this card.
            AddStartOfTurnTrigger((TurnTaker tt) => tt == this.TurnTaker, (PhaseChangeAction pca) => base.GameController.AddTokensToPool(SolarPowerCellPool, 1, GetCardSource()), TriggerType.AddTokensToPool);

            //Reset token pool to 0 when this card leaves play
            AddWhenDestroyedTrigger((DestroyCardAction dc) => ResetTokenValue(), TriggerType.Hidden);
            AddTrigger((MoveCardAction mc) => mc.Origin.IsInPlayAndNotUnderCard && !mc.Destination.IsInPlayAndNotUnderCard && mc.CardToMove == base.Card, (MoveCardAction mc) => ResetTokenValue(), TriggerType.ModifyTokens, TriggerTiming.After, ActionDescription.Unspecified, isConditional: false, requireActionSuccess: true, null, outOfPlayTrigger: true);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            //Remove all tokens from this card...

            //The results of removing tokens are stored in "results" so we can tell how many were removed
            List<RemoveTokensFromPoolAction> results = new List<RemoveTokensFromPoolAction>();

            IEnumerator coroutine = base.GameController.RemoveTokensFromPool(SolarPowerCellPool, SolarPowerCellPool.CurrentValue, results, cardSource: GetCardSource());
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }

            //...and restore X HP, where X is the number of tokens discarded this way.
            int X = GetNumberOfTokensRemoved(results);
            coroutine = base.GameController.GainHP(this.CharacterCard, X, cardSource: GetCardSource());
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