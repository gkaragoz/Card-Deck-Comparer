using AnyCardGame.Entity.Cards;
using AnyCardGame.Entity.Decks;
using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnyCardGame.Entity.Players
{
    public class Player
    {
        public Deck Deck { get; private set; }

        public GrouppedDeck GrouppedDeck { get; private set; }

        public Player()
        {
        }

        public Player(List<Card> cards)
        {
            CreateDeck(cards);
        }

        public void CreateDeck(List<Card> cards)
        {
            Deck = new Deck(cards);
        }

        public void SortDeck(GroupType groupType)
        {
            GrouppedDeck = Deck.Sort(groupType);

            var grpAll = GrouppedDeck.AllCards.OrderBy(c => c.Id).ToList();
            var deck = Deck.Cards.OrderBy(c => c.Id).ToList();

            for (int ii = 0; ii < grpAll.Count(); ii++)
            {
                Debug.Log(grpAll[ii] == deck[ii]);
            }
        }
    }
}
