using AnyCardGame.Entity;
using AnyCardGame.Enums;
using AnyCardGame.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckSorterTest
{
    private PlayerDeck _playerDeck = new PlayerDeck(
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
    public void Sorting_With_Straight_Test()
    {
        Debug.Log("---Deck is ready to use: ---");
        foreach (var card in _playerDeck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Deck shuffled.");
        _playerDeck.Shuffle();

        var suitThenKindComparer = new CardComparer(CompareType.SuitThenKind);

        _playerDeck.Sort(suitThenKindComparer);

        Debug.Log("---Sorted deck: Suit Then Kind---");
        foreach (var card in _playerDeck.Cards)
            Debug.Log(card.ToString());
    }

    [Test]
    public void Sorting_With_SameKind_Test()
    {
        Debug.Log("---Deck is ready to use: ---");
        foreach (var card in _playerDeck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Deck shuffled.");
        _playerDeck.Shuffle();

        var kindThenSuitComparer = new CardComparer(CompareType.KindThenSuit);

        _playerDeck.Sort(kindThenSuitComparer);

        Debug.Log("---Sorted deck: Kind Then Suit---");
        foreach (var card in _playerDeck.Cards)
            Debug.Log(card.ToString());
    }

    [Test]
    public void GroupBy_Straight_Test()
    {
        Debug.Log("---Deck: ---");
        foreach (var card in _playerDeck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Grouping deck by Straight: ---");
        _playerDeck.CreateGroup(GroupType.Straight);
    }

    [Test]
    public void GroupBy_SameKind_Test()
    {
        Debug.Log("---Deck: ---");
        foreach (var card in _playerDeck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Grouping deck by SameKind: ---");
        _playerDeck.CreateGroup(GroupType.SameKind);
    }
}
