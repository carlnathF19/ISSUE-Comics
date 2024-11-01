using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

//Remember, use Lady of The Wood as the example of how to setup a tokenpool;
namespace Workshopping.TMH
{
	//public class SolarPowerCellCardController : CardController
	//{
	//public static readonly string SolarPanelPool1Identifier = "SolarPanelPool1";

	//public readonly string TMHIdentifier = "TMH";






	//public SolarPowerCellCardController(Card card, TurnTakerController turnTakerController)
	//   : base(card, turnTakerController)
	//{	  
	//       base.SpecialStringMaker.ShowNonEnvironmentTargetWithHighestHP(2);
	//       //base.NumberOfTokens = 0;
	//       //base.TriggerTypes = new TriggerType[2]
	//       //{
	//       //	TriggerType.ModifyTokens,
	//       //	TriggerType.DrawCard
	//       //};
	//   }

	//public override void AddTriggers()
	//{
	//	AddStartOfTurnTrigger((TurnTaker tt) => tt == base.TurnTaker, (PhaseChangeAction p) => AddOrRemoveTokens(base.UnluckyPool, 1), new TriggerType[1] { TriggerType.ModifyTokens });
	//}

	//public override IEnumerator UsePower(int index = 0)
	//{

	//	IEnumerator gainHpRoutine = base.GameController.GainHP(base.CharacterCard, 2, null, null, GetCardSource());		
	//	if (base.UseUnityCoroutines)
	//	{
	//		yield return base.GameController.StartCoroutine(gainHpRoutine);
	//	}
	//	else
	//	{
	//		base.GameController.ExhaustCoroutine(gainHpRoutine);			
	//	}
	//}
	//}
}