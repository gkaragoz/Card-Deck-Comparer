using AnyCardGame.Entity.Cards;
using AnyCardGame.Enums;
using System.Collections.Generic;

namespace AnyCardGame.Entity.Decks
{
    public class GrouppedDeck
    {
        private List<GrouppedCard> _grouppedCards;
        private UngrouppedCards _ungrouppedCards;

        public List<Card> AllCards { get; private set; }
        public GroupType GroupType { get; private set; }
        public int UngrouppedCardsTotalScore => _ungrouppedCards.Score;

        public GrouppedDeck(GroupType groupType)
        {
            _grouppedCards = new List<GrouppedCard>();
            _ungrouppedCards = new UngrouppedCards(new List<Card>(), GroupType.None);

            AllCards = new List<Card>();

            GroupType = groupType;
        }

        public void AddGrouppedCard(GrouppedCard card)
        {
            _grouppedCards.Add(card);
            AllCards.AddRange(card.Group);
        }

        public void SetUngrouppedCards(UngrouppedCards cards)
        {
            _ungrouppedCards.AddCards(cards.Group);
            AllCards.AddRange(cards.Group);
        }

        public List<GrouppedCard> GetGrouppedCards()
        {
            return _grouppedCards;
        }

        public UngrouppedCards GetUngrouppedCards()
        {
            return _ungrouppedCards;
        }
    }
}
