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
            var pendingCards = Cards.OrderByDescending(card => card.Id).ToList();

            var searchedCards = new List<Card>();

            for (int ii = pendingCards.Count - 1; ii > 0; ii--)
            {
                var currentCard = pendingCards[ii];
                var previousCard = pendingCards[ii - 1];

                if (currentCard.Id + 1 == previousCard.Id && currentCard.Suit == previousCard.Suit)
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
                UngrouppedCards.AddRange(pendingCards);
            else
                GroupBySameKind(pendingCards);
        }

        /* ____________________STRAIGHT - POSSIBLE GROUPPABLE CARDS__________________________
         * || group of 3 cards          = n - 2;                                           ||
           || total card groups count   = n * (n + 1) , "n" is equal to "group of 3 cards" ||
		   ||		                      ___________                                      ||
		   ||			                       2                                           ||
		   ||*******************ALGORITHM STEPS****************                            ||
           ||   Hearts 1  ||       Hearts 1     Hearts 2     ||                            ||
           ||   Hearts 2  ||       Hearts 2     Hearts 3     ||                            ||
           ||   Hearts 3  ||       Hearts 3     Hearts 4     ||                            ||
           ||   Hearts 4  ||                                 ||                            ||
           ||             ||       Hearts 1                  ||                            ||
           ||             ||       Hearts 2                  ||                            ||
           ||             ||       Hearts 3                  ||                            ||
           ||             ||       Hearts 4                  ||                            ||
           ||_____________||_________________________________||____________________________||
         * */

        /* ____________________SAME KINDS - POSSIBLE GROUPPABLE CARDS________________________
         * || required cards amount for group = n = 4                                      ||
         * || total card groups count         = n combination of 3                         ||
         * ||		                                                                       ||
         * ||			                                                                   ||
         * ||*******************ALGORITHM STEPS****************                            ||
         * ||   Spades   7  ||     Diamonds 7   Spades   7   ||                            ||
         * ||   Diamonds 7  ||     Hearts   7   Hearts   7   ||                            ||
         * ||   Hearts   7  ||     Clubs    7   Clubs    7   ||                            ||
         * ||   Clubs    7  ||                               ||                            ||
         * ||               ||     Spades   7   Spades   7   ||                            ||
         * ||               ||     Diamonds 7   Diamonds 7   ||                            ||
         * ||               ||     Clubs    7   Hearts   7   ||                            ||
         * ||               ||                               ||                            ||
         * ||_______________||_______________________________||____________________________||
         * */
        private void GroupBy_Smart()
        {

        }
    }
}
