using AnyCardGame.Entity.Cards;
using AnyCardGame.Enums;
using System.Collections.Generic;
using System.Linq;

namespace AnyCardGame.Entity.Decks
{
    public class GrouppedDeck
    {
        private List<GrouppedCard> _grouppedCards;
        private UngrouppedCards _ungrouppedCards;

        public GroupType GroupType { get; private set; }
        public int UngrouppedCardsTotalScore => _ungrouppedCards.Score;

        public GrouppedDeck(GroupType groupType)
        {
            _grouppedCards = new List<GrouppedCard>();
            _ungrouppedCards = new UngrouppedCards(new List<Card>(), GroupType.None);

            GroupType = groupType;
        }

        public int GetAllCardsCount()
        {
            var count = 0;

            foreach (var grouppedCards in _grouppedCards)
                count += grouppedCards.Group.Count;

            count += _ungrouppedCards.Group.Count;

            return count;
        }

        public List<Card> GetAllCardBySorted()
        {
            var cards = new List<Card>();

            foreach (var grouppedCards in _grouppedCards)
                cards.AddRange(grouppedCards.Group);

            cards.AddRange(_ungrouppedCards.Group);

            return cards;
        }

        public void AddGrouppedCard(GrouppedCard card)
        {
            _grouppedCards.Add(card);

            if (card.Group.Any(c1 => GetAllCardBySorted().Any(c2 => c1.Id == c2.Id)))
                return;
        }

        public void AddGrouppedCards(List<GrouppedCard> cards)
        {
            foreach (var gc in cards)
                AddGrouppedCard(gc);
        }

        public void AddUngrouppedCards(UngrouppedCards cards)
        {
            _ungrouppedCards.AddCards(cards.Group);
        }

        public List<GrouppedCard> GetGrouppedCards()
        {
            return _grouppedCards;
        }

        public UngrouppedCards GetUngrouppedCards()
        {
            return _ungrouppedCards;
        }

        public void SetUnrouppedCards(UngrouppedCards ungrouppedCards)
        {
            _ungrouppedCards = ungrouppedCards;
        }

        public List<GrouppedCard> GetGrouppedCardsExcept(GrouppedCard exceptGrouppedCard)
        {
            var grouppedCards = new List<GrouppedCard>(_grouppedCards);
            grouppedCards.Remove(exceptGrouppedCard);

            return grouppedCards;
        }

        public List<GrouppedDeck> DestructToSubDecks()
        {
            var destructedSubDecks = new List<GrouppedDeck>();

            switch (GroupType)
            {
                case GroupType.Straight:
                    for (int ii = 0; ii < _grouppedCards.Count; ii++)
                    {
                        if (_grouppedCards[ii].Group.Count <= 3)
                            continue;

                        var subDecks = StraightDestructGroup(_grouppedCards[ii].Group);

                        for (int jj = 0; jj < subDecks.Count; jj++)
                        {
                            var deck = subDecks[jj];
                            var remainingGrouppedCards = GetGrouppedCardsExcept(_grouppedCards[ii]);
                            var newUngrouppedCard = GetUngrouppedCards();

                            deck.AddGrouppedCards(remainingGrouppedCards);
                            deck.AddUngrouppedCards(newUngrouppedCard);

                            destructedSubDecks.Add(deck);
                        }
                    }

                    break;
                case GroupType.SameKind:
                    const int REQUIRED_CARD_COUNT = 4;

                    for (int ii = 0; ii < _grouppedCards.Count; ii++)
                    {
                        if (_grouppedCards[ii].Group.Count != REQUIRED_CARD_COUNT)
                            continue;

                        var subDecks = SameKindDestructGroup(_grouppedCards[ii].Group);

                        for (int jj = 0; jj < subDecks.Count; jj++)
                        {
                            var deck = subDecks[jj];
                            var remainingGrouppedCards = GetGrouppedCardsExcept(_grouppedCards[ii]);
                            var newUngrouppedCard = GetUngrouppedCards();

                            deck.AddGrouppedCards(remainingGrouppedCards);
                            deck.AddUngrouppedCards(newUngrouppedCard);

                            destructedSubDecks.Add(deck);
                        }
                    }
                    break;
            }

            return destructedSubDecks;
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
        private List<GrouppedDeck> StraightDestructGroup(List<Card> grouppedCards)
        {
            var destructedGrouppedDeck = new List<GrouppedDeck>();

            var cardsCount = grouppedCards.Count;
            var takingStartIndex = 0;
            var takingGroupCount = 3;

            while (takingGroupCount < cardsCount)
            {
                if (takingStartIndex + takingGroupCount > cardsCount)
                {
                    takingStartIndex = 0;
                    takingGroupCount++;
                }

                var grouppedDeck = new GrouppedDeck(GroupType.Straight);

                var destructedGrouppedCards = new List<Card>();
                var destructedUngrouppedCards = new List<Card>();
                for (int ii = 0; ii < cardsCount; ii++)
                {
                    if (ii >= takingStartIndex && ii < takingStartIndex + takingGroupCount)
                        destructedGrouppedCards.Add(grouppedCards[ii]);
                    else
                        destructedUngrouppedCards.Add(grouppedCards[ii]);
                }

                grouppedDeck.AddGrouppedCard(new GrouppedCard(destructedGrouppedCards, GroupType.Straight));
                grouppedDeck.AddUngrouppedCards(new UngrouppedCards(destructedUngrouppedCards, GroupType.None));

                destructedGrouppedDeck.Add(grouppedDeck);

                takingStartIndex++;
            }

            return destructedGrouppedDeck;
        }

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
        private List<GrouppedDeck> SameKindDestructGroup(List<Card> grouppedCards)
        {
            var destructedGrouppedDeck = new List<GrouppedDeck>();
            var cardsCount = grouppedCards.Count;

            int skippingIndex = 0;
            for (int ii = 0; ii < cardsCount; ii++)
            {
                var grouppedDeck = new GrouppedDeck(GroupType.SameKind);

                var destructedGrouppedCards = new List<Card>();
                var destructedUngrouppedCards = new List<Card>();
                for (int jj = 0; jj < cardsCount; jj++)
                {
                    if (jj == skippingIndex)
                        destructedUngrouppedCards.Add(grouppedCards[jj]);
                    else
                        destructedGrouppedCards.Add(grouppedCards[jj]);
                }

                grouppedDeck.AddGrouppedCard(new GrouppedCard(destructedGrouppedCards, GroupType.SameKind));
                grouppedDeck.AddUngrouppedCards(new UngrouppedCards(destructedUngrouppedCards, GroupType.None));

                destructedGrouppedDeck.Add(grouppedDeck);

                skippingIndex++;
            }

            return destructedGrouppedDeck;
        }
    }
}
