using AnyCardGame.Enums;
using Scripts.Utils;
using System;
using UnityEngine;

namespace AnyCardGame.Entity
{
    public class Card : IComparable<Card>
    {
        public int Id { get; private set; }
        public Kind Kind { get; private set; }
        public Suit Suit { get; private set; }
        public int Score { get; private set; }

        public Card(int id)
        {
            Setup(id);
        }

        public Card(string code)
        {
            Setup(CardUtils.GetCardIdByCode(code));
        }

        public Card(Kind kind, Suit suit)
        {
            Setup(CardUtils.GetCardIdByEnums(kind, suit));
        }

        private void Setup(int id)
        {
            Id = id + 1;
            Kind = (Kind)(id % 13);
            Suit = (Suit)Mathf.FloorToInt(id / 13);
            Score = Mathf.Min((int)(Kind + 1), 10);
        }

        public int CompareTo(Card other)
        {
            if (other == null) return 1;
            if (Suit != other.Suit)
                return Suit.CompareTo(other.Suit);
            return Kind.CompareTo(other.Kind);
        }

        public override string ToString()
        {
            switch (Suit)
            {
                case Suit.Clubs:
                    return $"{Id}: ♣️ {Kind} of {Suit}";
                case Suit.Diamonds:
                    return $"{Id}: ♦ {Kind} of {Suit}";
                case Suit.Hearts:
                    return $"{Id}: ♥ {Kind} of {Suit}";
                case Suit.Spades:
                    return $"{Id}: ♠ {Kind} of {Suit}";
            }

            return string.Empty;
        }
    }
}