using AnyCardGame.Entity;
using AnyCardGame.Enums;
using AnyCardGame.Utils;
using NUnit.Framework;
using UnityEngine;

public class DeckSorterBaseSortingTest
{

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
        var deck = new Deck();
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
        var deck = new Deck();
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
        var deck = new Deck();
        Debug.Log("---Deck shuffled.");
        deck.Shuffle();

        var suitOnlyComparer = new CardComparer(CompareType.SuitOnly);

        deck.Sort(suitOnlyComparer);

        Debug.Log("---Sorted deck: Suit Only---");
        foreach (var card in deck.Cards)
            Debug.Log(card.ToString());
    }

}
