using AnyCardGame.Entity.Cards;
using AnyCardGame.Entity.Decks;
using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                    return GroupBySmart(deck);
            }

            return null;
        }

        private GrouppedDeck GetBestGrouppedDeck(List<GrouppedDeck> grouppedDecks, GroupType groupType)
        {
            var bestScore = Mathf.Infinity;
            var bestGrouppedDeck = new GrouppedDeck(groupType);

            grouppedDecks.ForEach((GrouppedDeck grouppedDeck) =>
            {
                var ungrouppedSameKindGrouppedDeck = GroupBySameKind(new GrouppedDeck(groupType), new List<Card>(grouppedDeck.GetUngrouppedCards().Group));

                if (ungrouppedSameKindGrouppedDeck.UngrouppedCardsTotalScore <= bestScore)
                {
                    bestScore = ungrouppedSameKindGrouppedDeck.UngrouppedCardsTotalScore;
                    bestGrouppedDeck = grouppedDeck;
                    bestGrouppedDeck.AddGrouppedCards(ungrouppedSameKindGrouppedDeck.GetGrouppedCards());
                    bestGrouppedDeck.SetUnrouppedCards(ungrouppedSameKindGrouppedDeck.GetUngrouppedCards());
                }
            });

            return bestGrouppedDeck;
        }

        #region Straight

        private GrouppedDeck GroupByStraight(List<Card> cards)
        {
            var grouppedDeck = new GrouppedDeck(GroupType.Straight);

            var pendingCards = cards.OrderByDescending(card => card.Id).ToList();

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
                            grouppedDeck.AddUngrouppedCards(new UngrouppedCards(searchedCards, GroupType.None));
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

                        grouppedDeck.AddUngrouppedCards(new UngrouppedCards(searchedCards, GroupType.None));
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
                grouppedDeck.AddUngrouppedCards(new UngrouppedCards(searchedCards, GroupType.None));

            if (pendingCards.Count <= 2)
                grouppedDeck.AddUngrouppedCards(new UngrouppedCards(pendingCards, GroupType.None));
            else
                return GroupBySameKind(grouppedDeck, pendingCards);

            return grouppedDeck;
        }

        #endregion

        #region Smart
        private GrouppedDeck GroupBySmart(Deck deck)
        {
            var straightGrouppedDeck = GroupByStraight(new List<Card>(deck.Cards));
            var sameKindGrouppedDeck = GroupBySameKind(new GrouppedDeck(GroupType.SameKind), new List<Card>(deck.Cards));

            var subDestructedStraightGrouppedDeck = straightGrouppedDeck.DestructToSubDecks();
            var subDestructedSameKindGruppedDeck = sameKindGrouppedDeck.DestructToSubDecks();
            
            var finalStraightGrouppedDeck = GetBestGrouppedDeck(subDestructedStraightGrouppedDeck, GroupType.Straight);
            var finalSameKindGrouppedDeck = GetBestGrouppedDeck(subDestructedSameKindGruppedDeck, GroupType.SameKind);

            return finalStraightGrouppedDeck.UngrouppedCardsTotalScore <= finalSameKindGrouppedDeck.UngrouppedCardsTotalScore ? finalStraightGrouppedDeck : finalSameKindGrouppedDeck;
        }

        #endregion

    }
}
