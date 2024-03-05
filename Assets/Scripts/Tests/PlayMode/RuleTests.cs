using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RuleTests
{
    private GameObject testObject;
    private VideoPoker.GameManager gameManager;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject();
        gameManager = testObject.AddComponent<VideoPoker.GameManager>();
    }

    [Test]
    public void TestJacksOrBetter()
    {
        Card[] hand = new Card[] { new Card { rank = 11 }, new Card { rank = 11 }, new Card { rank = 3 }, new Card { rank = 2 }, new Card { rank = 9 } };
        Assert.IsTrue(gameManager.CheckJacksOrBetter(hand));

        hand = new Card[] { new Card { rank = 4 }, new Card { rank = 12 }, new Card { rank = 1 }, new Card { rank = 12 }, new Card { rank = 9 } };
        Assert.IsTrue(gameManager.CheckJacksOrBetter(hand));

        hand = new Card[] { new Card { rank = 3 }, new Card { rank = 11 }, new Card { rank = 13 }, new Card { rank = 13 }, new Card { rank = 8 } };
        Assert.IsTrue(gameManager.CheckJacksOrBetter(hand));

        hand = new Card[] { new Card { rank = 1 }, new Card { rank = 11 }, new Card { rank = 1 }, new Card { rank = 2 }, new Card { rank = 4 } };
        Assert.IsTrue(gameManager.CheckJacksOrBetter(hand));

        hand = new Card[] { new Card { rank = 1 }, new Card { rank = 11 }, new Card { rank = 12 }, new Card { rank = 13 }, new Card { rank = 4 } };
        Assert.IsFalse(gameManager.CheckJacksOrBetter(hand));
    }

    [Test]
    public void TestTwoPair()
    {
        Card[] hand = new Card[] { new Card { rank = 3 }, new Card { rank = 3 }, new Card { rank = 6 }, new Card { rank = 6 }, new Card { rank = 9 } };
        Assert.IsTrue(gameManager.CheckTwoPair(hand));

        hand = new Card[] { new Card { rank = 1 }, new Card { rank = 13 }, new Card { rank = 1 }, new Card { rank = 13 }, new Card { rank = 7 } };
        Assert.IsTrue(gameManager.CheckTwoPair(hand));


        hand = new Card[] { new Card { rank = 3 }, new Card { rank = 3 }, new Card { rank = 3 }, new Card { rank = 6 }, new Card { rank = 9 } };
        Assert.IsFalse(gameManager.CheckTwoPair(hand));

        hand = new Card[] { new Card { rank = 5 }, new Card { rank = 2 }, new Card { rank = 5 }, new Card { rank = 2 }, new Card { rank = 5 } };
        Assert.IsFalse(gameManager.CheckTwoPair(hand));

        hand = new Card[] { new Card { rank = 5 }, new Card { rank = 2 }, new Card { rank = 4 }, new Card { rank = 7 }, new Card { rank = 1 } };
        Assert.IsFalse(gameManager.CheckTwoPair(hand));
    }

    [Test]
    public void TestThreeKind()
    {
        Card[] hand = new Card[] { new Card { rank = 3 }, new Card { rank = 3 }, new Card { rank = 3 }, new Card { rank = 6 }, new Card { rank = 9 } };
        Assert.IsTrue(gameManager.CheckThreeKind(hand));

        hand = new Card[] { new Card { rank = 4 }, new Card { rank = 3 }, new Card { rank = 4 }, new Card { rank = 6 }, new Card { rank = 4 } };
        Assert.IsTrue(gameManager.CheckThreeKind(hand));

        hand = new Card[] { new Card { rank = 10 }, new Card { rank = 10 }, new Card { rank = 13 }, new Card { rank = 13 }, new Card { rank = 10 } };
        Assert.IsFalse(gameManager.CheckThreeKind(hand));
    }


    [Test]
    public void TestStraight()
    {
        // Ordered
        Card[] hand = new Card[] { new Card { rank = 5 }, new Card { rank = 4 }, new Card { rank = 3 }, new Card { rank = 2 }, new Card { rank = 1 } };
        Assert.IsTrue(gameManager.CheckStraight(hand));

        hand = new Card[] { new Card { rank = 7 }, new Card { rank = 6 }, new Card { rank = 5 }, new Card { rank = 4 }, new Card { rank = 3 } };
        Assert.IsTrue(gameManager.CheckStraight(hand));

        hand = new Card[] { new Card { rank = 1 }, new Card { rank = 13 }, new Card { rank = 12 }, new Card { rank = 11 }, new Card { rank = 10 } };
        Assert.IsTrue(gameManager.CheckStraight(hand));

        // Unordered
        hand = new Card[] { new Card { rank = 4 }, new Card { rank = 5 }, new Card { rank = 1 }, new Card { rank = 3 }, new Card { rank = 2 } };
        Assert.IsTrue(gameManager.CheckStraight(hand));

        hand = new Card[] { new Card { rank = 6 }, new Card { rank = 7 }, new Card { rank = 5 }, new Card { rank = 9 }, new Card { rank = 8 } };
        Assert.IsTrue(gameManager.CheckStraight(hand));

        hand = new Card[] { new Card { rank = 11 }, new Card { rank = 12 }, new Card { rank = 1 }, new Card { rank = 10 }, new Card { rank = 13 } };
        Assert.IsTrue(gameManager.CheckStraight(hand));

        // Not a straight
        hand = new Card[] { new Card { rank = 6 }, new Card { rank = 4 }, new Card { rank = 13 }, new Card { rank = 9 }, new Card { rank = 11 } };
        Assert.IsFalse(gameManager.CheckStraight(hand));

        hand = new Card[] { new Card { rank = 6 }, new Card { rank = 4 }, new Card { rank = 3 }, new Card { rank = 5 }, new Card { rank = 11 } };
        Assert.IsFalse(gameManager.CheckStraight(hand));
    }

    [Test]
    public void TestFlush()
    {
        Card[] hand = new Card[] { new Card { suit = Card.Suit.Clubs }, new Card { suit = Card.Suit.Clubs }, new Card { suit = Card.Suit.Clubs }, new Card { suit = Card.Suit.Clubs }, new Card { suit = Card.Suit.Clubs } };
        Assert.IsTrue(gameManager.CheckFlush(hand));

        hand = new Card[] { new Card { suit = Card.Suit.Clubs }, new Card { suit = Card.Suit.Clubs }, new Card { suit = Card.Suit.Clubs }, new Card { suit = Card.Suit.Diamonds }, new Card { suit = Card.Suit.Clubs } };
        Assert.IsFalse(gameManager.CheckFlush(hand));

        hand = new Card[] { new Card { suit = Card.Suit.Hearts }, new Card { suit = Card.Suit.Spades }, new Card { suit = Card.Suit.Diamonds }, new Card { suit = Card.Suit.Diamonds }, new Card { suit = Card.Suit.Clubs } };
        Assert.IsFalse(gameManager.CheckFlush(hand));
    }

    [Test]
    public void TestFullHouse()
    {
        Card[] hand = new Card[] { new Card { rank = 3 }, new Card { rank = 3 }, new Card { rank = 3 }, new Card { rank = 6 }, new Card { rank = 6 } };
        Assert.IsTrue(gameManager.CheckFullHouse(hand));

        hand = new Card[] { new Card { rank = 4 }, new Card { rank = 3 }, new Card { rank = 4 }, new Card { rank = 3 }, new Card { rank = 4 } };
        Assert.IsTrue(gameManager.CheckFullHouse(hand));

        hand = new Card[] { new Card { rank = 1 }, new Card { rank = 13 }, new Card { rank = 1 }, new Card { rank = 11 }, new Card { rank = 1 } };
        Assert.IsFalse(gameManager.CheckFullHouse(hand));
    }

    [Test]
    public void TestFourKind()
    {
        Card[] hand = new Card[] { new Card { rank = 1 }, new Card { rank = 3 }, new Card { rank = 1 }, new Card { rank = 1 }, new Card { rank = 1 } };
        Assert.IsTrue(gameManager.CheckFourKind(hand));

        hand = new Card[] { new Card { rank = 2 }, new Card { rank = 13 }, new Card { rank = 13 }, new Card { rank = 13 }, new Card { rank = 13 } };
        Assert.IsTrue(gameManager.CheckFourKind(hand));

        hand = new Card[] { new Card { rank = 5 }, new Card { rank = 5 }, new Card { rank = 2 }, new Card { rank = 5 }, new Card { rank = 1 } };
        Assert.IsFalse(gameManager.CheckFourKind(hand));

        hand = new Card[] { new Card { rank = 8 }, new Card { rank = 13 }, new Card { rank = 1 }, new Card { rank = 9 }, new Card { rank = 3 } };
        Assert.IsFalse(gameManager.CheckFourKind(hand));
    }


    [Test]
    public void TestStraightFlush()
    {
        Card[] hand = new Card[] { new Card { suit = Card.Suit.Diamonds, rank = 13 }, new Card { suit = Card.Suit.Diamonds, rank = 12 }, new Card { suit = Card.Suit.Diamonds, rank = 11 }, new Card { suit = Card.Suit.Diamonds, rank = 10 }, new Card { suit = Card.Suit.Diamonds, rank = 9 } };
        Assert.IsTrue(gameManager.CheckStraight(hand) && gameManager.CheckFlush(hand));

        hand = new Card[] { new Card { suit = Card.Suit.Hearts, rank = 1 }, new Card { suit = Card.Suit.Hearts, rank = 2 }, new Card { suit = Card.Suit.Hearts, rank = 5 }, new Card { suit = Card.Suit.Hearts, rank = 4 }, new Card { suit = Card.Suit.Hearts, rank = 3 } };
        Assert.IsTrue(gameManager.CheckStraight(hand) && gameManager.CheckFlush(hand));

        hand = new Card[] { new Card { suit = Card.Suit.Diamonds, rank = 13 }, new Card { suit = Card.Suit.Diamonds, rank = 8 }, new Card { suit = Card.Suit.Diamonds, rank = 11 }, new Card { suit = Card.Suit.Diamonds, rank = 10 }, new Card { suit = Card.Suit.Diamonds, rank = 12 } };
        Assert.IsFalse(gameManager.CheckStraight(hand) && gameManager.CheckFlush(hand));

        hand = new Card[] { new Card { suit = Card.Suit.Clubs, rank = 5 }, new Card { suit = Card.Suit.Diamonds, rank = 6 }, new Card { suit = Card.Suit.Diamonds, rank = 7 }, new Card { suit = Card.Suit.Diamonds, rank = 8 }, new Card { suit = Card.Suit.Diamonds, rank = 9 } };
        Assert.IsFalse(gameManager.CheckStraight(hand) && gameManager.CheckFlush(hand));
    }

    [Test]
    public void TestRoyal()
    {
        Card[] hand = new Card[] { new Card { suit = Card.Suit.Diamonds, rank = 1 }, new Card { suit = Card.Suit.Diamonds, rank = 13 }, new Card { suit = Card.Suit.Diamonds, rank = 11 }, new Card { suit = Card.Suit.Diamonds, rank = 10 }, new Card { suit = Card.Suit.Diamonds, rank = 12 } };
        Assert.IsTrue(gameManager.CheckStraight(hand) && gameManager.CheckFlush(hand) && gameManager.CheckRoyal(hand));

        hand = new Card[] { new Card { suit = Card.Suit.Hearts, rank = 9 }, new Card { suit = Card.Suit.Hearts, rank = 10 }, new Card { suit = Card.Suit.Hearts, rank = 11 }, new Card { suit = Card.Suit.Hearts, rank = 12 }, new Card { suit = Card.Suit.Hearts, rank = 13 } };
        Assert.IsTrue(gameManager.CheckStraight(hand) && gameManager.CheckFlush(hand));
        Assert.IsFalse(gameManager.CheckRoyal(hand));
    }
}
