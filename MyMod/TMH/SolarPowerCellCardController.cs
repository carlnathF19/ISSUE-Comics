using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

//Remember, use Lady of The Wood as the example of how to setup a tokenpool;
//namespace ISSUEComics.TMH
//{
//	public class SolarPowerCellController : CardController
//	{
//		public SolarPowerCellController(Card card, TurnTakerController turnTakerController)
//			: base(card, turnTakerController)
//		{
//			base.SpecialStringMaker.ShowTokenPool(base.Card.FindTokenPool(TokenPool.SolarPowerCellPoolIdentifier));
//		}

//		public override IEnumerator Play()
//		{
//			IEnumerator coroutine = ResetTokenValue();
//			if (base.UseUnityCoroutines)
//			{
//				yield return base.GameController.StartCoroutine(coroutine);
//			}
//			else
//			{
//				base.GameController.ExhaustCoroutine(coroutine);
//			}
//		}

//		public override void AddTriggers()
//		{
//			AddTrigger((DealDamageAction dd) => dd.DidDealDamage && dd.DamageSource.IsSameCard(base.Card), (DealDamageAction dd) => base.GameController.AddTokensToPool(base.Card.FindTokenPool(TokenPool.SolarPowerCellPoolIdentifier), 1, GetCardSource()), TriggerType.AddTokensToPool, TriggerTiming.After);
//			AddTrigger((DealDamageAction dd) => dd.DidDealDamage && dd.Target == base.Card, (DealDamageAction dd) => base.GameController.AddTokensToPool(base.Card.FindTokenPool(TokenPool.SolarPowerCellPoolIdentifier), 1, GetCardSource()), TriggerType.AddTokensToPool, TriggerTiming.After, ActionDescription.DamageTaken);
//			AddEndOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, EndOfTurnResponse, TriggerType.DealDamage);
//			AddStartOfTurnTrigger(base.GameController.AddTokensToPool(base.Card.FindTokenPool(TokenPool.SolarPowerCellPoolIdentifier), 1, GetCardSource()));
//			AddWhenDestroyedTrigger((DestroyCardAction dc) => ResetTokenValue(), TriggerType.Hidden);
//			AddTrigger((MoveCardAction mc) => mc.Origin.IsInPlayAndNotUnderCard && !mc.Destination.IsInPlayAndNotUnderCard && mc.CardToMove == base.Card, (MoveCardAction mc) => ResetTokenValue(), TriggerType.ModifyTokens, TriggerTiming.After, ActionDescription.Unspecified, isConditional: false, requireActionSuccess: true, null, outOfPlayTrigger: true);
//		}

//		public IEnumerator ResetTokenValue()
//		{
//			base.Card.FindTokenPool(TokenPool.SolarPowerCellPoolIdentifier).SetToInitialValue();
//			yield return null;
//		}

//		private IEnumerator EndOfTurnResponse(PhaseChangeAction p)
//		{
//			int X = base.Card.FindTokenPool(TokenPool.SolarPowerCellPoolIdentifier).CurrentValue + 1;
//			IEnumerator coroutine = DealDamageToHighestHP(base.Card, 2, (Card c) => c.IsNonEnvironmentTarget, (Card c) => X, DamageType.Fire);
//			if (base.UseUnityCoroutines)
//			{
//				yield return base.GameController.StartCoroutine(coroutine);
//			}
//			else
//			{
//				base.GameController.ExhaustCoroutine(coroutine);
//			}
//		}
//	}
//}
namespace ISSUEComics.TMH
{
	public class SolarPowerCellCardController : CardController
	{
		public SolarPowerCellCardController(Card card, TurnTakerController turnTakerController)
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
		}
	}
}