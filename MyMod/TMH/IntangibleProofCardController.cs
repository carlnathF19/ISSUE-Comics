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

			List<SelectLocationDecision> storedResults = new List<SelectLocationDecision>();
			IEnumerator coroutine = FindVillainDeck(DecisionMaker, SelectionType.RevealCardsFromDeck, storedResults, (Location l) => true);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine);
			}
			Location selectedLocation = GetSelectedLocation(storedResults);
			if (DidSelectDeck(storedResults))
			{
				coroutine = RevealCard_DiscardItOrPutItOnDeck(DecisionMaker, base.TurnTakerController, selectedLocation, toBottom: false);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine);
				}
			}
			IEnumerator coroutine2 = DrawCard(base.HeroTurnTakerController.HeroTurnTaker, optional: false);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine2);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine2);
			}
		}
	}
}