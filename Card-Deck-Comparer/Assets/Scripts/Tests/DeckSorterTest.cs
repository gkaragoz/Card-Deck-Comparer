using AnyCardGame.Entity.Cards;
using AnyCardGame.Entity.Decks;
using AnyCardGame.Entity.Players;
using AnyCardGame.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckSorterTest
{
    private Player _player = new Player(
            new List<Card>
        {
            new Card("H01"),
            new Card("S02"),
            new Card("D05"),
            new Card("H04"),
            new Card("S01"),
            new Card("D03"),
            new Card("C04"),
            new Card("S04"),
            new Card("D01"),
            new Card("S03"),
            new Card("D04"),
        });

    [Test]
    public void PlayerDeckAndSameKindSortedDecksAreSameReferenced()
    {
        _player.SortDeck(GroupType.SameKind);

        var playerAllGrouppedCards = _player.GrouppedDeck.AllCards.OrderBy(c => c.Id).ToList();
        var playerAllCards = _player.Deck.Cards.OrderBy(c => c.Id).ToList();

        for (int ii = 0; ii < playerAllGrouppedCards.Count(); ii++)
            Assert.AreEqual(playerAllGrouppedCards[ii], playerAllCards[ii], "Player's card and player's groupped and sorted card are equal.");
    }


    [Test]
    public void PlayerDeckAndStraightSortedDecksAreSameReferenced()
    {
        _player.SortDeck(GroupType.Straight);

        var playerAllGrouppedCards = _player.GrouppedDeck.AllCards.OrderBy(c => c.Id).ToList();
        var playerAllCards = _player.Deck.Cards.OrderBy(c => c.Id).ToList();

        for (int ii = 0; ii < playerAllGrouppedCards.Count(); ii++)
            Assert.AreEqual(playerAllGrouppedCards[ii], playerAllCards[ii], "Player's card and player's groupped and sorted card are equal.");
    }

    [Test]
    public void PlayerDeckAndSmartSortedDecksAreSameReferenced()
    {
        _player.SortDeck(GroupType.Smart);

        var playerAllGrouppedCards = _player.GrouppedDeck.AllCards.OrderBy(c => c.Id).ToList();
        var playerAllCards = _player.Deck.Cards.OrderBy(c => c.Id).ToList();

        for (int ii = 0; ii < playerAllGrouppedCards.Count(); ii++)
            Assert.AreEqual(playerAllGrouppedCards[ii], playerAllCards[ii], "Player's card and player's groupped and sorted card are equal.");
    }

    private void SortByXTestHelper(List<GrouppedCard> grouppedCards, List<Card> ungrouppedCards, GroupType groupType)
    {
        var expectedGrouppedDeck = new GrouppedDeck(groupType);

        foreach (var grouppedCard in grouppedCards)
            expectedGrouppedDeck.AddGrouppedCard(grouppedCard);

        expectedGrouppedDeck.AddUngrouppedCards(new UngrouppedCards(ungrouppedCards, GroupType.None));

        Debug.Log("---Deck: ---");
        foreach (var card in _player.Deck.Cards)
            Debug.Log(card.ToString());

        Debug.Log($"---Grouping deck by : {groupType}---");
        _player.SortDeck(groupType);

        var expectedGrouppedCards = expectedGrouppedDeck.GetGrouppedCards().OrderBy(gc => gc.GroupType).ToList();
        var actualGrouppedCards = _player.GrouppedDeck.GetGrouppedCards().OrderBy(gc => gc.GroupType).ToList();

        for (int ii = 0; ii < expectedGrouppedCards.Count; ii++)
        {
            var expectedCards = expectedGrouppedCards[ii].Group.OrderBy(c => c.Id).ToList();
            var expectedCardsGroupType = expectedGrouppedCards[ii].GroupType;

            var actualCards = actualGrouppedCards[ii].Group.OrderBy(c => c.Id).ToList();
            var actualCardsGroupType = actualGrouppedCards[ii].GroupType;

            Assert.AreEqual(expectedCardsGroupType, actualCardsGroupType, "Groupped cards' group types are matching.");

            for (int jj = 0; jj < expectedCards.Count; jj++)
            {
                var expectedSingleCardId = expectedCards[ii].Id;
                var actualSingleCardId = actualCards[ii].Id;

                Assert.AreEqual(expectedSingleCardId, actualSingleCardId, "Groupped cards' ids are equal. That means, they're in the same order.");
            }
        }

        var expectedUngrouppedCards = expectedGrouppedDeck.GetUngrouppedCards().Group.OrderBy(card => card.Id).ToList();
        var actualUngrouppedCards = _player.GrouppedDeck.GetUngrouppedCards().Group.OrderBy(card => card.Id).ToList();

        for (int ii = 0; ii < expectedUngrouppedCards.Count; ii++)
        {
            var expectedCardId = expectedUngrouppedCards[ii].Id;
            var actualCardId = actualUngrouppedCards[ii].Id;

            Assert.AreEqual(expectedCardId, actualCardId, "Ungroupped card's ids are equal. That means, they're same.");
        }
    }

    [Test]
    public void SortByStraightTest()
    {
        var grouppedCards_01 = new GrouppedCard(new List<Card>
        {
            new Card("S01"),
            new Card("S02"),
            new Card("S03"),
            new Card("S04"),
        }, GroupType.Straight);

        var grouppedCards_02 = new GrouppedCard(new List<Card>
        {
            new Card("D03"),
            new Card("D04"),
            new Card("D05"),
        }, GroupType.Straight);

        var ungrouppedCards = new List<Card>
        {
            new Card("D01"),
            new Card("H01"),
            new Card("H04"),
            new Card("C04"),
        };

        SortByXTestHelper(new List<GrouppedCard>()
        {
            grouppedCards_01,
            grouppedCards_02
        }, ungrouppedCards, GroupType.Straight);
    }

    [Test]
    public void SortBySameKindTest()
    {
        var grouppedCards_01 = new GrouppedCard(new List<Card>
        {
            new Card("H01"),
            new Card("S01"),
            new Card("D01"),
        }, GroupType.SameKind);

        var grouppedCards_02 = new GrouppedCard(new List<Card>
        {
            new Card("S04"),
            new Card("D04"),
            new Card("H04"),
            new Card("C04"),
        }, GroupType.SameKind);

        var ungrouppedCards = new List<Card>
        {
            new Card("S02"),
            new Card("D03"),
            new Card("D05"),
            new Card("S03"),
        };

        SortByXTestHelper(new List<GrouppedCard>()
        {
            grouppedCards_01,
            grouppedCards_02
        }, ungrouppedCards, GroupType.SameKind);
    }


    [Test]
    public void SortBySmartTest()
    {
        var grouppedCards_01 = new GrouppedCard(new List<Card>
        {
            new Card("S01"),
            new Card("S02"),
            new Card("S03"),
        }, GroupType.Straight);

        var grouppedCards_02 = new GrouppedCard(new List<Card>
        {
            new Card("S04"),
            new Card("H04"),
            new Card("C04"),
        }, GroupType.SameKind);

        var grouppedCards_03 = new GrouppedCard(new List<Card>
        {
            new Card("D03"),
            new Card("D04"),
            new Card("D05"),
        }, GroupType.Straight);

        var ungrouppedCards = new List<Card>
        {
            new Card("D01"),
            new Card("H01"),
        };

        SortByXTestHelper(new List<GrouppedCard>()
        {
            grouppedCards_01,
            grouppedCards_02,
            grouppedCards_03
        }, ungrouppedCards, GroupType.Smart);
    }
}
