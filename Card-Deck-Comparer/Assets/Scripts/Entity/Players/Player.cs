using AnyCardGame.Entity.Cards;
using AnyCardGame.Entity.Decks;
using AnyCardGame.Enums;
using System.Collections.Generic;

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
        }

        public int[] GetGrouppedDeckCardIds()
        {
            var allCards = GrouppedDeck == null ? Deck.Cards : GrouppedDeck.GetAllCardBySorted();

            int cardCount = allCards.Count;
            int[] ids = new int[cardCount];
            for (int ii = 0; ii < cardCount; ii++)
                ids[ii] = allCards[ii].Id;

            return ids;
        }
    }
}
