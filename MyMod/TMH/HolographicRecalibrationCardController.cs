using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Controller.VoidGuardWrithe;
using Handelabra.Sentinels.Engine.Model;

namespace ISSUEComics.TMH
{
        public class HolographicRecalibrationCardController : CardController
    {
        public HolographicRecalibrationCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        { 
        }
        public override IEnumerator Play()
        {
            IEnumerator gainHpRoutine = base.GameController.GainHP(base.CharacterCard, 3, null, null, GetCardSource());
            IEnumerator drawCardRoutine = DrawCards(base.HeroTurnTakerController, 1);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(gainHpRoutine);
                yield return base.GameController.StartCoroutine(drawCardRoutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(gainHpRoutine);
                base.GameController.ExhaustCoroutine(drawCardRoutine);
            }
        
        }
    }
}

