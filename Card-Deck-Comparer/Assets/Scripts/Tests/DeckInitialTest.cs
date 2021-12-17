using AnyCardGame.Entity;
using AnyCardGame.Entity.Decks;
using NUnit.Framework;

public class DeckInitialTest
{
    [Test]
    public void CheckDeckCountIs52()
    {
        var deck = new Deck();

        var deckCount = deck.Count;

        Assert.AreEqual(52, deckCount, $"Deck count should be 52.");
    }

    [Test]
    public void DrawBottomCardIsEqualToNumber52()
    {
        var deck = new Deck();

        var topCard = deck.DrawBottomCard();

        Assert.AreEqual(52, topCard.Id, $"Drawn top card's should be 52.");
    }

    [Test]
    public void TopCardHasScoreOf1()
    {
        var deck = new Deck();

        var topCard = deck.DrawTopCard();

        Assert.AreEqual(1, topCard.Score, $"Top card score should be 1.");
    }

    [Test]
    public void BottomCardHasScoreOf10()
    {
        var deck = new Deck();

        var bottomCard = deck.DrawBottomCard();

        Assert.AreEqual(10, bottomCard.Score, $"Bottom card score should be 10.");
    }

}
