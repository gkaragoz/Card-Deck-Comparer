using AnyCardGame.Entity;
using AnyCardGame.Enums;
using System;
using System.Collections.Generic;

namespace AnyCardGame.Utils
{
    public class CardComparer : IComparer<Card>
    {
        public CompareType CompareBy { get; }

        public CardComparer(CompareType compareBy = CompareType.SuitThenKind)
        {
            CompareBy = compareBy;
        }

        public int Compare(Card x, Card y)
        {
            switch (CompareBy)
            {
                case CompareType.KindOnly:
                    return x.Kind.CompareTo(y.Kind);
                case CompareType.SuitOnly:
                    return x.Suit.CompareTo(y.Suit);
                case CompareType.SuitThenKind:
                    if (x.Suit != y.Suit) return x.Suit.CompareTo(y.Suit);
                    return x.Kind.CompareTo(y.Kind);
                case CompareType.KindThenSuit:
                    if (x.Kind != y.Kind) return x.Kind.CompareTo(y.Kind);
                    return x.Suit.CompareTo(y.Suit);
                default:
                    throw new NotImplementedException($"CardOrderMethod {CompareBy} is not implemented.");
            }
        }
    }
}