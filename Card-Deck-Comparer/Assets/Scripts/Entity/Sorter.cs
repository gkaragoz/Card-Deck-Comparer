using AnyCardGame.Entity.Cards;
using AnyCardGame.Entity.Decks;
using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AnyCardGame.Entity
{
    public class Sorter
    {
        public GrouppedDeck Sort(Deck deck, GroupType groupType)
        {
            switch (groupType)
            {
                case GroupType.Straight:
                    return GroupByStraight(new List<Card>(deck.Cards));
                case GroupType.SameKind:
                    return GroupBySameKind(new GrouppedDeck(GroupType.SameKind), new List<Card>(deck.Cards));
                case GroupType.Smart:
                    // Straight();
                    // SameKind();

                    // (Straight() < SameKind()) ? Straight() : SameKind();
                    break;
            }

            return null;
        }

        #region Straight

        private GrouppedDeck GroupByStraight(List<Card> deck)
        {
            var grouppedDeck = new GrouppedDeck(GroupType.Straight);

            var pendingCards = deck.OrderByDescending(card => card.Id).ToList();

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
                            grouppedDeck.AddGrouppedCard(new GrouppedCard(searchedCards, GroupType.Straight));
                        else
                            grouppedDeck.AddUngrouppedCards(searchedCards);
                    }
                }
                else
                {
                    searchedCards.Add(currentCard);

                    if (searchedCards.Count > 2)
                        grouppedDeck.AddGrouppedCard(new GrouppedCard(searchedCards, GroupType.Straight));
                    else
                    {
                        if (ii == 1)
                            searchedCards.Add(previousCard);

                        grouppedDeck.AddUngrouppedCards(searchedCards);
                    }

                    searchedCards = new List<Card>();
                }
            }

            return grouppedDeck;
        }

        #endregion

        #region SameKind

        private GrouppedDeck GroupBySameKind(GrouppedDeck grouppedDeck, List<Card> pendingCards)
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
                grouppedDeck.AddGrouppedCard(new GrouppedCard(searchedCards, GroupType.SameKind));
            else
                grouppedDeck.AddUngrouppedCards(searchedCards);

            if (pendingCards.Count <= 2)
                grouppedDeck.AddUngrouppedCards(pendingCards);
            else
                return GroupBySameKind(grouppedDeck, pendingCards);

            return grouppedDeck;
        }

        #endregion

        #region Smart

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
        private static void GroupBy_Smart()
        {

        }

        #endregion

    }
}
