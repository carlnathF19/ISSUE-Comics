// SentinelsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Handelabra.Sentinels.Engine.Controller.LanternJack.TMHOneShotCardController
using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Workshopping.TMH
{
	public abstract class TMHOneShotCardController : CardController
	{
		public TMHOneShotCardController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
		}

		public override IEnumerator Play()
		{
			IEnumerator coroutine = OneShotEffect();
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
		}

		protected abstract IEnumerator OneShotEffect();
	}
}