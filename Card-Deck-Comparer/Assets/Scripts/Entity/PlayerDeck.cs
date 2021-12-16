using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AnyCardGame.Entity
{
    public class PlayerDeck : Deck
    {
        public List<GrouppedCards> GrouppedCards { get; set; }
        public List<Card> UngrouppedCards { get; set; }

        public PlayerDeck() : base()
        {
            GrouppedCards = new List<GrouppedCards>();
            UngrouppedCards = new List<Card>();
        }

        public PlayerDeck(List<Card> cards) : base(cards)
        {
            GrouppedCards = new List<GrouppedCards>();
            UngrouppedCards = new List<Card>();
        }

        public void CreateGroup(GroupType groupType)
        {
            switch (groupType)
            {
                case GroupType.Straight:
                    GroupBy_Straight();
                    break;
                case GroupType.SameKind:
                    GroupBySameKind();
                    break;
                case GroupType.Smart:
                    GroupBy_Smart();
                    break;
            }
        }

        private void GroupBy_Straight()
        {
            var pendingCards = Cards.OrderBy(card => card.Id).ToList();

            var searchedCards = new List<Card>();

            for (int ii = pendingCards.Count - 1; ii > 0; ii--)
            {
                var currentCard = pendingCards[ii];
                var previousCard = pendingCards[ii - 1];

                if (currentCard.Id - 1 == previousCard.Id && currentCard.Suit == previousCard.Suit)
                {
                    searchedCards.Add(currentCard);

                    if (ii == 1)
                    {
                        searchedCards.Add(previousCard);

                        if (searchedCards.Count > 2)
                            GrouppedCards.Add(new GrouppedCards(searchedCards, GroupType.Straight));
                        else
                            UngrouppedCards.AddRange(searchedCards);
                    }
                }
                else
                {
                    searchedCards.Add(currentCard);

                    if (searchedCards.Count > 2)
                        GrouppedCards.Add(new GrouppedCards(searchedCards, GroupType.Straight));
                    else
                    {
                        if (ii == 1)
                            searchedCards.Add(previousCard);

                        UngrouppedCards.AddRange(searchedCards);
                    }

                    searchedCards = new List<Card>();
                }
            }
        }

        private void GroupBySameKind()
        {
            GroupBySameKind(new List<Card>(Cards));
        }

        private void GroupBySameKind(List<Card> pendingCards)
        {
            var searchedCards = new List<Card>();
            var searchingCard = pendingCards[0];

            pendingCards.Remove(searchingCard);
            searchedCards.Add(searchingCard);

            for (int ii = pendingCards.Count - 1; ii >= 0; ii--)
            {
                var nextSearchingCard = pendingCards[ii];

                if (searchingCard.Kind == nextSearchingCard.Kind)
                {
                    pendingCards.Remove(nextSearchingCard);
                    searchedCards.Add(nextSearchingCard);

                    if (searchedCards.Count == 4)
                        break;
                }
            }

            if (searchedCards.Count > 2)
                GrouppedCards.Add(new GrouppedCards(searchedCards, GroupType.SameKind));
            else
                UngrouppedCards.AddRange(searchedCards);

            if (pendingCards.Count <= 2)
                            UngrouppedCards.AddRange(searchedCards);
            else
                GroupBySameKind(pendingCards);
        }

        private void GroupBy_Smart()
        {

        }
    }
}
