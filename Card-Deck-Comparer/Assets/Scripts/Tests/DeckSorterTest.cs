using AnyCardGame.Entity;
using AnyCardGame.Enums;
using AnyCardGame.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckSorterTest
{
    private Deck _deck = new Deck(
            new List<Card>
        {
            new Card(Kind.Ace, Suit.Hearts),
            new Card(Kind.Two, Suit.Spades),
            new Card(Kind.Five, Suit.Diamonds),
            new Card(Kind.Four, Suit.Hearts),
            new Card(Kind.Ace, Suit.Spades),
            new Card(Kind.Three, Suit.Diamonds),
            new Card(Kind.Four, Suit.Clubs),
            new Card(Kind.Four, Suit.Spades),
            new Card(Kind.Ace, Suit.Diamonds),
            new Card(Kind.Three, Suit.Spades),
            new Card(Kind.Four, Suit.Diamonds)
        });

    [Test]
    public void Sorting_With_1_2_3_Test()
    {
        Debug.Log("---Deck is ready to use: ---");
        foreach (var card in _deck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Deck shuffled.");
        _deck.Shuffle();

        var suitThenKindComparer = new CardComparer(CompareType.SuitThenKind);

        _deck.Sort(suitThenKindComparer);

        Debug.Log("---Sorted deck: Suit Then Kind---");
        foreach (var card in _deck.Cards)
            Debug.Log(card.ToString());
    }

    [Test]
    public void Sorting_With_7_7_7_Test()
    {
        Debug.Log("---Deck is ready to use: ---");
        foreach (var card in _deck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Deck shuffled.");
        _deck.Shuffle();

        var kindThenSuitComparer = new CardComparer(CompareType.KindThenSuit);

        _deck.Sort(kindThenSuitComparer);

        Debug.Log("---Sorted deck: Kind Then Suit---");
        foreach (var card in _deck.Cards)
            Debug.Log(card.ToString());
    }
}
