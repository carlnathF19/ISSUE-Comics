using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace ISSUEComics
{
	public class FieldGeneratorCardController : CardController
	{
		public FieldGeneratorCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
		}
		public override IEnumerator UsePower(int index = 0)
		{
			int powerNumeral = GetPowerNumeral(0, 2);
			IEnumerator gainHpRoutine = base.GameController.GainHP(base.CharacterCard, powerNumeral, null, null, GetCardSource());
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(gainHpRoutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(gainHpRoutine);
			}
		}
	}
}