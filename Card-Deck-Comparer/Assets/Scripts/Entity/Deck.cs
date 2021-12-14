using AnyCardGame.Enums;
using System;
using System.Collections.Generic;

namespace AnyCardGame.Entity
{
    public class Deck
    {
        public List<Card> Cards { get; }
        public int Count => Cards.Count;

        public Deck()
        {
            Cards = new List<Card>();

            int cardId = 1;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Kind kind in Enum.GetValues(typeof(Kind)))
                {
                    Cards.Add(new Card(cardId++, kind, suit));
                }
            }
        }

        public Card DrawCardAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Card card = Cards[index];
            Cards.RemoveAt(index);
            return card;
        }

        public Card DrawTopCard()
        {
            return DrawCardAt(0);
        }

        public Card DrawBottomCard()
        {
            return DrawCardAt(Count - 1);
        }

        public Card DrawRandomCard()
        {
            Random random = new Random();
            int index = random.Next(Count);
            return DrawCardAt(index);
        }

        public void AddCardOnTop(Card card)
        {
            if (Cards.Contains(card))
                throw new InvalidOperationException($"Deck already contains card {card}.");

            Cards.Insert(0, card);
        }

        public void AddCardOnBottom(Card card)
        {
            if (Cards.Contains(card))
                throw new InvalidOperationException($"Deck already contains card {card}.");

            Cards.Add(card);
        }

        public void Shuffle()
        {
            // Fisher-Yates shuffle method
            Random random = new Random();
            int n = Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card randomCard = Cards[k];
                Cards[k] = Cards[n];
                Cards[n] = randomCard;
            }
        }

        public void Sort() => Cards.Sort();

        public void Sort(IComparer<Card> comparer) => Cards.Sort(comparer);
    }
}