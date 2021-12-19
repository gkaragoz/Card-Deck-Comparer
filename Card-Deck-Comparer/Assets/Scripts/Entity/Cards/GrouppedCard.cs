using AnyCardGame.Enums;
using System.Collections.Generic;

namespace AnyCardGame.Entity.Cards
{
    public class GrouppedCard
    {
        public List<Card> Group { get; }
        public GroupType GroupType { get; }

        public GrouppedCard(GroupType groupType)
        {
            Group = new List<Card>();
            GroupType = groupType;
        }

        public GrouppedCard(List<Card> cards, GroupType groupType)
        {
            Group = cards;
            GroupType = groupType;
        }

        public void AddCard(Card card)
        {
            Group.Add(card);
        }
    }
}
