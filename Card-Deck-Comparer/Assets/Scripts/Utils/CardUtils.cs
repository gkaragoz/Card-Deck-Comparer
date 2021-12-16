using AnyCardGame.Entity;
using AnyCardGame.Enums;
using System;

namespace Scripts.Utils
{
    public static class CardUtils
    {
        /// <summary>
        /// Takes the card name and returns card id.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetCardIdByCode(string name)
        {
            if (name.Length != 3)
                throw new ArgumentException("Name should be length of 3.");

            var suitCode = name.Substring(0, 1).ToUpper();
            var kindCode = name.Substring(1, 2);

            var suit = 0;
            var kind = int.Parse(kindCode);

            switch (suitCode)
            {
                case "S":
                    suit = 0;
                    break;
                case "D":
                    suit = 1;
                    break;
                case "H":
                    suit = 2;
                    break;
                case "C":
                    suit = 3;
                    break;
                default:
                    throw new ArgumentException();
            }

            var id = suit * 13 + kind;

            return id - 1;
        }

        /// <summary>
        /// Get card id by its' enums.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="suit"></param>
        /// <returns></returns>
        public static int GetCardIdByEnums(Kind kind, Suit suit)
        {
            var cardCode = suit.ToString()[0] + ((int)(kind + 1)).ToString("00");

            return GetCardIdByCode(cardCode);
        }
    }
}
