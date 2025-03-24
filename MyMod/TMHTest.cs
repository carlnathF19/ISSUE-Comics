using NUnit.Framework;
using System;
using ISSUEComics;
using ISSUEComics.TMH;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using Handelabra.Sentinels.UnitTest;
using System.Collections.Generic;
using Handelabra;

namespace ISSUEComicsTest
{
    [TestFixture()]
    public class TMHTest : BaseTest
    {
        protected HeroTurnTakerController TMH { get { return FindHero("TMH"); } }
        protected TokenPool SolarPowerPool(Card card)
        {
            return card.FindTokenPool("SolarPowerCellPool");
        }

        [Test()]
        public void TestSolarPowerCellPlay()
        {
            SetupGameController("AkashBhuta", "ISSUEComics.TMH", "InsulaPrimalis");

            StartGame();

            //Check that when played, there are 0 tokens in the token pool.
            Card solar = PlayCard("SolarPowerCell");
            AssertTokenPoolCount(SolarPowerPool(solar), 0);
        }

        [Test()]
        public void TestSolarPowerCellStartOfTurn()
        {
            SetupGameController("AkashBhuta", "ISSUEComics.TMH", "InsulaPrimalis");

            StartGame();

            //At the start of your turn, place a token on this card.

            //Play Solar Power Cell
            Card solar = PlayCard("SolarPowerCell");
            TokenPool pool = SolarPowerPool(solar);

            //Go to the start of TMH's turn
            GoToStartOfTurn(TMH);

            //Check that there is 1 token in the pool
            AssertTokenPoolCount(pool, 1);
        }

        [Test()]
        public void TestSolarPowerCellPower()
        {
            SetupGameController("AkashBhuta", "ISSUEComics.TMH", "InsulaPrimalis");

            StartGame();

            //At the start of your turn, place a token on this card.

            //Play Solar Power Cell
            Card solar = PlayCard("SolarPowerCell");

            //Put 5 tokens in the pool and set TMH's HP to 10 so he has room to heal
            AddTokensToPool(SolarPowerPool(solar), 5);
            SetHitPoints(TMH, 10);

            //Store TMH's HP to compare to after healing
            QuickHPStorage(TMH);

            //Use the power on Solar Power Cell
            UsePower(solar);

            //Check that there are 0 tokens in the pool
            AssertTokenPoolCount(SolarPowerPool(solar), 0);

            //Check that TMH has gained 5 HP since storing the value
            QuickHPCheck(5);
        }
    }
}
