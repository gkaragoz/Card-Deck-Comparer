using AnyCardGame.Entity;
using AnyCardGame.Enums;
using AnyCardGame.Utils;
using NUnit.Framework;
using UnityEngine;

public class DeckSorterTest
{
    [Test]
    public void CheckDeckCountIs52()
    {
        Deck deck = new Deck();

        var deckCount = deck.Count;

        Assert.AreEqual(52, deckCount, $"Deck count should be 52.");
    }

    [Test]
    public void DrawBottomCardIsEqualToNumber52()
    {
        Deck deck = new Deck();

        var topCard = deck.DrawBottomCard();

        Assert.AreEqual(52, topCard.Id, $"Drawn top card's should be 52.");
    }

    [Test]
    public void TopCardHasScoreOf1()
    {
        Deck deck = new Deck();

        var topCard = deck.DrawTopCard();

        Assert.AreEqual(1, topCard.Score, $"Top card score should be 1.");
    }

    [Test]
    public void BottomCardHasScoreOf10()
    {
        Deck deck = new Deck();

        var bottomCard = deck.DrawBottomCard();

        Assert.AreEqual(10, bottomCard.Score, $"Bottom card score should be 10.");
    }

    [Test]
    public void SuitThenKindResultTest()
    {
        var deck = new Deck();
        Debug.Log("---Deck shuffled.");
        deck.Shuffle();

        var suitThenKindComparer = new CardComparer(CompareType.SuitThenKind);

        deck.Sort(suitThenKindComparer);

        Debug.Log("---Sorted deck: SuitThenKind---");
        foreach (var card in deck.Cards)
            Debug.Log(card.ToString());
    }

    [Test]
    public void KindThenSuitResultTest()
    {
        Deck deck = new Deck();
        Debug.Log("---Deck shuffled.");
        deck.Shuffle();

        var kindThenSuitComparer = new CardComparer(CompareType.KindThenSuit);

        deck.Sort(kindThenSuitComparer);

        Debug.Log("---Sorted deck: Kind Then Suit---");
        foreach (var card in deck.Cards)
            Debug.Log(card.ToString());
    }

    [Test]
    public void KindOnlyResultTest()
    {
        Deck deck = new Deck();
        Debug.Log("---Deck shuffled.");
        deck.Shuffle();

        var kindOnlyComparer = new CardComparer(CompareType.KindOnly);

        deck.Sort(kindOnlyComparer);

        Debug.Log("---Sorted deck: Kind Only---");
        foreach (var card in deck.Cards)
            Debug.Log(card.ToString());
    }

    [Test]
    public void SuitOnlyResultTest()
    {
        Deck deck = new Deck();
        Debug.Log("---Deck shuffled.");
        deck.Shuffle();

        var suitOnlyComparer = new CardComparer(CompareType.SuitOnly);

        deck.Sort(suitOnlyComparer);

        Debug.Log("---Sorted deck: Suit Only---");
        foreach (var card in deck.Cards)
            Debug.Log(card.ToString());
    }
}
