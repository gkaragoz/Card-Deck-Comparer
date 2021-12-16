using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AnyCardGame.Entity
{
    public class GrouppedCards
    {
        public List<Card> Group { get; }
        public GroupType GroupType { get; }

        public GrouppedCards(List<Card> cards, GroupType groupType)
        {
            Group = cards;
            GroupType = groupType;
        }
    }
}
