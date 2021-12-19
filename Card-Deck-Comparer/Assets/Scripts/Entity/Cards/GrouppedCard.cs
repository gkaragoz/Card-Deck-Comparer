using AnyCardGame.Enums;
using System.Collections.Generic;

namespace AnyCardGame.Entity.Cards
{
    public class GrouppedCard
    {
        public List<Card> Group { get; }
        public GroupType GroupType { get; }

        public GrouppedCard(List<Card> cards, GroupType groupType)
        {
            Group = cards;
            GroupType = groupType;
        }
    }
}
