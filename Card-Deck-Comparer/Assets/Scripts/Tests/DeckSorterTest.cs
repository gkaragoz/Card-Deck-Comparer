using AnyCardGame.Entity.Cards;
using AnyCardGame.Entity.Players;
using AnyCardGame.Enums;
using NUnit.Framework;
using System.Collections.Generic;
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
    public void Sort_By_Straight_Test()
    {
        Debug.Log("---Deck: ---");
        foreach (var card in _player.Deck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Grouping deck by Straight: ---");
        _player.SortDeck(GroupType.Straight);
    }

    [Test]
    public void Sort_By_SameKind_Test()
    {
        Debug.Log("---Deck: ---");
        foreach (var card in _player.Deck.Cards)
            Debug.Log(card.ToString());

        Debug.Log("---Grouping deck by SameKind: ---");
        _player.SortDeck(GroupType.SameKind);
    }
}
