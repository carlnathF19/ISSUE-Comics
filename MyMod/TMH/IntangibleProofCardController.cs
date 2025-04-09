using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;


namespace ISSUEComics.TMH
{
	public class IntangibleProofCardController : CardController
	{
		public IntangibleProofCardController(Card card, TurnTakerController turnTakerController)
		   : base(card, turnTakerController)
		{
		}
		public override void AddTriggers()
		{
		}
		public override IEnumerator Play()
		{
			//Infrared Eyepiece
			List<SelectLocationDecision> storedResults = new List<SelectLocationDecision>();
			List<SelectLocationDecision> storedResults2 = new List<SelectLocationDecision>();

			IEnumerator coroutine = FindVillainDeck(DecisionMaker, SelectionType.RevealCardsFromDeck, storedResults, (Location l) => true);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
				
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
				
			}
			Location deck = GetSelectedLocation(storedResults);

			List<Card> storedCards = new List<Card>();
			List<Card> storedCards2 = new List<Card>();
			if (deck != null)
			{
				int powerNumeral = GetPowerNumeral(0, 2);
				int powerNumeral2 = GetPowerNumeral(1, 2);
				IEnumerator coroutine2 = RevealCardsFromTopOfDeck_PutOnTopAndOnBottom(base.HeroTurnTakerController, base.TurnTakerController, deck, powerNumeral, powerNumeral2, 1, storedCards);
				IEnumerator coroutine3 = RevealCardsFromTopOfDeck_PutOnTopAndOnBottom(base.HeroTurnTakerController, base.TurnTakerController, base.TurnTaker.Deck, powerNumeral, powerNumeral2, 1, storedCards2);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine2);
					yield return base.GameController.StartCoroutine(coroutine3);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine2);
					base.GameController.ExhaustCoroutine(coroutine3);

				}
			}
				IEnumerator coroutine4 = DrawCard(base.HeroTurnTakerController.HeroTurnTaker, optional: false);
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