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

        #region I, Grenade
        [Test()]
        public void TestIGrenade()
        {
            SetupGameController("AkashBhuta", "ISSUEComics.TMH", "InsulaPrimalis");

            StartGame();

            //{TMH} deals one target X radiant damage, where X = the difference between his current HP and 13. Reduce [i]The Machine Heart's[/i] HP to 13.
            //Shuffle this card into [i]The Machine Heart's[/i] deck.

            //Store HP of Akash and TMH to compare later
            QuickHPStorage(akash, TMH);

            //Store TMH's deck's shuffle state to compare later
            QuickShuffleStorage(TMH);

            //Play I, Grenade
            Card grenade = PlayCard("IGrenade");

            //Check that the correct amount of damage has been dealt
            QuickHPCheck(-13, -13);

            //Check that I, Grenade has been shuffled into the deck
            AssertInDeck(grenade);
            QuickShuffleCheck(1);
        }

        [Test()]
        public void TestIGrenadeLessThan13()
        {
            SetupGameController("AkashBhuta", "ISSUEComics.TMH", "InsulaPrimalis");

            StartGame();

            //{TMH} deals one target X radiant damage, where X = the difference between his current HP and 13. Reduce [i]The Machine Heart's[/i] HP to 13.
            //Shuffle this card into [i]The Machine Heart's[/i] deck.

            //Check that no damage is dealt when TMH's HP is less than 13
            SetHitPoints(TMH, 10);

            //Store HP of Akash and TMH to compare later
            QuickHPStorage(akash, TMH);

            //Store TMH's deck's shuffle state to compare later
            QuickShuffleStorage(TMH);

            //Play I, Grenade
            Card grenade = PlayCard("IGrenade");

            //Check that the correct amount of damage has been dealt
            QuickHPCheck(0, 0);

            //Check that I, Grenade has been shuffled into the deck
            AssertInDeck(grenade);
            QuickShuffleCheck(1);
        }
        #endregion I, Grenade

        #region Solar Power Cell
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
        #endregion Solar Power Cell

        #region Intangible Proof
        [Test()]
        public void TestIntangibleProofTop()
        {
            SetupGameController("AkashBhuta", "ISSUEComics.TMH", "InsulaPrimalis");

            StartGame();

            //Reveal the Top card of the Villain deck and the top card of The Machine Hearts deck.  You may place These cards on the top or bottom of their deck.   Draw a Card.

            //Move Solar Power Cell and Entomb to the top of their decks
            Card solar = PutOnDeck("SolarPowerCell");
            Card entomb = PutOnDeck("Entomb");

            //Choose to put each card on the top of its deck
            DecisionMoveCardDestinations = new MoveCardDestination[] { new MoveCardDestination(akash.TurnTaker.Deck, false), new MoveCardDestination(TMH.TurnTaker.Deck, false) };

            //Store number of cards in TMH's hand to check later
            QuickHandStorage(TMH);

            //Play Intangible Proof
            PlayCard("IntangibleProof");

            //Check that TMH drew a card
            QuickHandCheck(1);

            //Since Solar Power Cell was put on TMH's deck, it should now be in his hand
            AssertInHand(solar);

            //Check that Entomb is on top of the villain deck
            AssertOnTopOfDeck(entomb);
        }

        [Test()]
        public void TestIntangibleProofBottom()
        {
            SetupGameController("AkashBhuta", "ISSUEComics.TMH", "InsulaPrimalis");

            StartGame();

            //Reveal the Top card of the Villain deck and the top card of The Machine Hearts deck.  You may place These cards on the top or bottom of their deck.   Draw a Card.

            //Move Solar Power Cell, I Grenade, and Entomb to the top of their decks
            Card grenade = PutOnDeck("IGrenade");
            Card solar = PutOnDeck("SolarPowerCell");
            Card entomb = PutOnDeck("Entomb");

            //Choose to put each card on the bottom of its deck
            DecisionMoveCardDestinations = new MoveCardDestination[] { new MoveCardDestination(akash.TurnTaker.Deck, true), new MoveCardDestination(TMH.TurnTaker.Deck, true) };

            //Store number of cards in TMH's hand to check later
            QuickHandStorage(TMH);

            //Play Intangible Proof
            PlayCard("IntangibleProof");

            //Check that TMH drew a card
            QuickHandCheck(1);

            //Since Solar Power Cell was not put on TMH's deck, the next card I Grenade should be in his hand
            AssertInHand(grenade);

            //Check that Solar Power Cell is on the bottom of the deck
            AssertOnBottomOfDeck(solar);

            //Check that Entomb is on the bottom of the villain deck
            AssertOnBottomOfDeck(entomb);
        }

        [Test()]
        public void TestIntangibleProofOblivAeon()
        {
            SetupGameController(new string[] { "OblivAeon", "ISSUEComics.TMH", "Legacy", "Haka", "Tachyon", "Luminary", "InsulaPrimalis", "MobileDefensePlatform", "Megalopolis", "TheBlock", "ChampionStudios" }, shieldIdentifier: "ThePrimaryObjective", scionIdentifiers: new List<string>() { "DarkMindCharacter" });

            StartGame();

            //Reveal the Top card of the Villain deck and the top card of The Machine Hearts deck.  You may place These cards on the top or bottom of their deck.   Draw a Card.

            //Check that if OblivAeon is in the other battle zone, Intangible Proof can still see the Scion and Aeon Man decks

            //Move Solar Power Cell, Aeon Thrall, and Aeon Assault to the top of their decks
            Card solar = PutOnDeck("SolarPowerCell");
            Card thrall = MoveCard(oblivaeon, "AeonThrall", aeonDeck);
            Card assault = MoveCard(oblivaeon, "AeonAssault", scionDeck);

            //Play another Aeon Man so that TMH can see the Aeon Man deck
            Card locus = PlayCard("AeonLocus");

            //Move OblivAeon to the other battle zone
            SwitchBattleZone(oblivaeon);

            //Select the Aeon Man deck
            DecisionSelectLocation = new LocationChoice(aeonDeck);

            //Choose to put each card on the bottom of its deck
            DecisionMoveCardDestinations = new MoveCardDestination[] { new MoveCardDestination(aeonDeck, true), new MoveCardDestination(TMH.TurnTaker.Deck, false) };

            //Store number of cards in TMH's hand to check later
            QuickHandStorage(TMH);

            //Make sure the choice of villain decks includes the Aeon Man deck and the Scion deck, but not OblivAeon's deck or the Mission deck
            AssertNextDecisionChoices(new LocationChoice[] { new LocationChoice(aeonDeck), new LocationChoice(scionDeck) }, new LocationChoice[] { new LocationChoice(oblivaeon.TurnTaker.Deck), new LocationChoice(missionDeck) });

            //Play Intangible Proof
            PlayCard("IntangibleProof");

            //Check that TMH drew a card
            QuickHandCheck(1);

            //Since Solar Power Cell was put on TMH's deck, it should be in his hand
            AssertInHand(solar);

            //Check that Aeon Thrall is on the bottom of the villain deck
            AssertAtLocation(thrall, aeonDeck, true);

            //Check that Aeon Assault is still on top of the Scion deck
            AssertAtLocation(assault, scionDeck);
        }

        [Test()]
        public void TestIntangibleProofOblivAeonNoVillainDecks()
        {
            SetupGameController(new string[] { "OblivAeon", "ISSUEComics.TMH", "Legacy", "Haka", "Tachyon", "Luminary", "InsulaPrimalis", "MobileDefensePlatform", "Megalopolis", "TheBlock", "ChampionStudios" }, shieldIdentifier: "none", scionIdentifiers: new List<string>() { "DarkMindCharacter" });

            StartGame();

            //Reveal the Top card of the Villain deck and the top card of The Machine Hearts deck.  You may place These cards on the top or bottom of their deck.   Draw a Card.

            //Check that if OblivAeon is in the other battle zone, Intangible Proof can still see the Scion and Aeon Man decks

            //Move Solar Power Cell, Aeon Thrall, and Aeon Assault to the top of their decks
            Card solar = PutOnDeck("SolarPowerCell");
            Card thrall = MoveCard(oblivaeon, "AeonThrall", aeonDeck);
            Card assault = MoveCard(oblivaeon, "AeonAssault", scionDeck);

            //Move OblivAeon to the other battle zone so there are no villain decks in BZ1
            SwitchBattleZone(oblivaeon);

            //Choose to put each card on the bottom of its deck
            DecisionMoveCardDestination = new MoveCardDestination(TMH.TurnTaker.Deck, false);

            //Store number of cards in TMH's hand to check later
            QuickHandStorage(TMH);

            //Make sure the choice of villain decks includes the Aeon Man deck and the Scion deck, but not OblivAeon's deck or the Mission deck
            AssertNextDecisionChoices(new LocationChoice[] { }, new LocationChoice[] { new LocationChoice(aeonDeck), new LocationChoice(scionDeck) , new LocationChoice(oblivaeon.TurnTaker.Deck), new LocationChoice(missionDeck) });

            //Play Intangible Proof
            PlayCard("IntangibleProof");

            //Check that TMH drew a card
            QuickHandCheck(1);

            //Since Solar Power Cell was put on TMH's deck, it should be in his hand
            AssertInHand(solar);

            //Check that Aeon Thrall is still on the top of the villain deck
            AssertAtLocation(thrall, aeonDeck, false);

            //Check that Aeon Assault is still on top of the Scion deck
            AssertAtLocation(assault, scionDeck);
        }
        #endregion Intangible Proof
    }
}
