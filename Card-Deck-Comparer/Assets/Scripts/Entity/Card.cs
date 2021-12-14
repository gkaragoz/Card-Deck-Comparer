using AnyCardGame.Enums;
using System;

namespace AnyCardGame.Entity
{
    public class Card : IComparable<Card>
    {
        public int Id { get; }
        public Kind Kind { get; }
        public Suit Suit { get; }
        public int Score { get; }

        public Card(int id, Kind kind, Suit suit)
        {
            Id = id;
            Kind = kind;
            Suit = suit;
            Score = (int)Kind;

            switch (Kind)
            {
                case Kind.Jack:
                case Kind.Queen:
                case Kind.King:
                    Score = 10;
                    break;
            }
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
                    return $"♣️ {Kind} of {Suit}";
                case Suit.Diamonds:
                    return $"♦ {Kind} of {Suit}";
                case Suit.Hearts:
                    return $"♥ {Kind} of {Suit}";
                case Suit.Spades:
                    return $"♠ {Kind} of {Suit}";
            }

            return string.Empty;
        }
    }
}