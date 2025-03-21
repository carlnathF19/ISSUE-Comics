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
