using UnityEngine;
using System.Collections.Generic;

namespace VideoPoker
{
	// The main game manager

	public class GameManager : MonoBehaviour
	{
		private int currentBalance = 0;

		UIManager uiManager;

		private List<Card> fullDeck = new List<Card>();
		List<Card> currentDeck = new List<Card>();

		Card[] currentHand;
		bool[] cardHeld;

		[SerializeField]
		private Sprite[] cardImages;

		private bool started;
		public bool dealed { get; private set; }

		public struct Result
		{
			public string handType;
			public int prize;
		}

		void StartGame()
		{
			uiManager = transform.parent.Find("UIManager").GetComponent<UIManager>();
			currentHand = new Card[5];
			cardHeld = new bool[5];
			InitializeCards();
		}

		// Generates all 52 cards of the deck including matching the appropriate image to the card
        private void InitializeCards()
        {
			for (int s = 0; s < 4; s++)
            {
				for (int r = 0; r < 13; r++)
                {
					fullDeck.Add(new Card { suit = (Card.Suit)s, rank = r + 1, image = cardImages[s*13 + r]});
				}
			}
			foreach (Card card in fullDeck)
			{
				currentDeck.Add(card);
			}
		}

        public void Deal()
		{
			if (!started)
            {
				started = true;
				StartGame();
			}
			if (!dealed)
            {
				// Deal: Draw 5 random cards for the hand
				for (int i = 0; i < 5; i++)
                {
					int randIndex = Random.Range(0, currentDeck.Count);
					currentHand[i] = currentDeck[randIndex];
					currentDeck.RemoveAt(randIndex);
				}
				uiManager.DisplayHand(currentHand);
				uiManager.ClearResultText();
			}
			else
            {
				// Re-deal: Replace cards not held down with random new ones
				for (int i = 0; i < 5; i++)
				{
					if (cardHeld[i] == false)
                    {
						int randIndex = Random.Range(0, currentDeck.Count);
						currentHand[i] = currentDeck[randIndex];
						currentDeck.RemoveAt(randIndex);
					}
					else cardHeld[i] = false;
				}
				uiManager.DisplayHand(currentHand);

				// Calculate the result
				Result result = CalculatePayoff();
				uiManager.DisplayResult(result);

				// Update the balance
				currentBalance += result.prize;
				uiManager.DisplayBalance(currentBalance);

				// Reset the deck back to full
				currentDeck.Clear();
				foreach (Card card in fullDeck)
				{
					currentDeck.Add(card);
				}
			}
			dealed = !dealed;
		}

		public bool ToggleHold(int cardNo)
        {
			cardHeld[cardNo] = !cardHeld[cardNo];
			return cardHeld[cardNo];
		}

		public Result CalculatePayoff()
        {
			bool flush = CheckFlush(currentHand);
			bool straight = CheckStraight(currentHand);

			if (flush && straight)
			{
				// Royal Flush
				if (CheckRoyal(currentHand)) return new Result { handType = "Royal Flush", prize = 800};
				
				// Straight Flush
				return new Result { handType = "Straight Flush", prize = 50};
			}

			// Four of a kind
			if (CheckFourKind(currentHand)) return new Result { handType = "Four of a Kind", prize = 25};

			// Full house
			if (CheckFullHouse(currentHand)) return new Result { handType = "Full house", prize = 9};

			// Flush
			if (flush) return new Result { handType = "Flush", prize = 6};

			// Straight
			if (straight) return new Result { handType = "Straight", prize = 4};

			// Three of a kind
			if (CheckThreeKind(currentHand)) return new Result { handType = "Three of a Kind", prize = 3};

			// Two Pair
			if (CheckTwoPair(currentHand)) return new Result { handType = "Two Pair", prize = 2};

			// Jacks or Better
			if (CheckJacksOrBetter(currentHand)) return new Result { handType = "Jacks or Better", prize = 1};

			// All Other
			return new Result { prize = 0};
		}

		// Returns the counts for all the ranks in the hand
		private Dictionary<int, int> GetCounts(Card[] hand)
        {
			Dictionary<int, int> counts = new Dictionary<int, int>();
			foreach (Card card in hand)
			{
				if (!counts.ContainsKey(card.rank))
				{
					counts.Add(card.rank, 0);
				}
				counts[card.rank]++;
			}
			return counts;
		}

		// Returns true if the hand has at least a pair of Jacks, Queens, Kings, or Aces. False otherwise.
		public bool CheckJacksOrBetter(Card[] hand)
		{
			Dictionary<int, int> counts = GetCounts(hand);
			int[] ranksForPair = { 11, 12, 13, 1 };
			foreach (int i in ranksForPair)
            {
				if (counts.ContainsKey(i) && counts[i] >= 2) return true;
			}
			return false;
        }

		// Returns true if the hand has two pairs of cards of the same rank. False otherwise.
		public bool CheckTwoPair(Card[] hand)
		{
			Dictionary<int, int> counts = GetCounts(hand);
			int pairCount = 0;
			foreach (int c in counts.Values)
			{
				if (c == 2) pairCount++;
			}
			return pairCount == 2;
		}

		// Returns true if the hand has three cards of the same rank and two cards of two other ranks. False otherwise.
		public bool CheckThreeKind(Card[] hand)
		{
			Dictionary<int, int> counts = GetCounts(hand);
			int singleCount = 0;
			int tripCount = 0;
			foreach (int c in counts.Values)
			{
				if (c == 3) tripCount++;
				else if (c == 1) singleCount++;
			}
			return tripCount == 1 && singleCount == 2;
		}

		// Returns true if the hand has exactly four cards of the same rank. False otherwise.
		public bool CheckFourKind(Card[] hand)
		{
			Dictionary<int, int> counts = GetCounts(hand);
			foreach (int c in counts.Values)
			{
				if (c == 4) return true;
			}
			return false;
		}

		// Returns true if the hand includes sequential ranks. False otherwise.
		public bool CheckStraight(Card[] hand)
		{
			Dictionary<int, int> counts = GetCounts(hand);
			// Find the highest rank in the hand
			int highestRank = 0;
			foreach (int r in counts.Keys)
            {
				if (r > highestRank) highestRank = r;
            }
			if (counts.ContainsKey(13) && counts.ContainsKey(1))
            {
				highestRank = 14;
			}

			// Check that all the sequential ranks below the highest rank exist
			for (int i = 1; i < 5; i++)
            {
				if (!counts.ContainsKey(highestRank - i)) return false;
			}
			return true;
        }

		// Returns true if all cards in the hand have the same suit
		public bool CheckFlush(Card[] hand)
		{
			Card.Suit suit = hand[0].suit;
			for (int i = 1; i < 5; i++)
			{
				if (hand[i].suit != suit) return false;
			}
			return true;
		}

		// Returns true if the hand has three cards of the same rank and a pair of another rank. False otherwise.
		public bool CheckFullHouse(Card[] hand)
		{
			Dictionary<int, int> counts = GetCounts(hand);
			int tripCount = 0;
			int pairCount = 0;
			foreach (int c in counts.Values)
			{
				if (c == 3) tripCount++;
				else if (c == 2) pairCount++;
			}
			return tripCount == 1 && pairCount == 1;
		}

		// Returns true if the ranks in the hand are Ace, King, Queen, Jack, and 10. False otherwise.
		// If you know the hand is a straight flush, this tells you if it is a royal flush.
		public bool CheckRoyal(Card[] hand)
		{
			Dictionary<int, int> counts = GetCounts(hand);
			int[] royalRanks = { 11, 12, 13, 1 };
			foreach (int i in royalRanks)
			{
				if (!(counts.ContainsKey(i) && counts[i] == 1)) return false;
			}
			return true;
		}
	}
}
