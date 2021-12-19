using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AnyCardGame.Entity.Cards
{
    public class UngrouppedCards
    {
        public List<Card> Group { get; }
        public GroupType GroupType { get; }
        public int Score { get; private set; }

        public UngrouppedCards(GroupType groupType)
        {
            Group = new List<Card>();
            GroupType = groupType;
            Score = 0;
        }

        public UngrouppedCards(List<Card> cards, GroupType groupType)
        {
            Group = cards;
            GroupType = groupType;
            Score = Group.Sum(card => card.Score);
        }

        public void AddCard(Card card)
        {
            Group.Add(card);
            Score += card.Score;
        }

        public void AddCards(List<Card> cards)
        {
            Group.AddRange(cards);
            Score += cards.Sum(card => card.Score);
        }
    }
}
