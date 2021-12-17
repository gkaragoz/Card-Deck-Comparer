using AnyCardGame.Entity.Cards;
using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AnyCardGame.Entity.Decks
{
    public class GrouppedDeck
    {
        private List<GrouppedCard> _cards;
        private List<Card> _ungrouppedCards;

        public List<Card> AllCards { get; private set; }
        public GroupType GroupType { get; private set; }
        public int Score => _ungrouppedCards.Count == 0 ? 0 : _ungrouppedCards.Sum(card => card.Score);

        public GrouppedDeck(GroupType groupType)
        {
            _cards = new List<GrouppedCard>();
            _ungrouppedCards = new List<Card>();

            AllCards = new List<Card>();

            GroupType = groupType;
        }

        public void AddGrouppedCard(GrouppedCard card)
        {
            _cards.Add(card);
            AllCards.AddRange(card.Group);
        }

        public void AddUngrouppedCards(List<Card> cards)
        {
            _ungrouppedCards.AddRange(cards);
            AllCards.AddRange(cards);
        }
    }
}
