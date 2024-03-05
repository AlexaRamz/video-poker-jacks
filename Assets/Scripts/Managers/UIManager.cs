using UnityEngine;
using UnityEngine.UI;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	///
	/// Manages UI including button events and updates to text fields
	/// 
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private Text currentBalanceText;

		[SerializeField]
		private Text winningText;

		[SerializeField]
		private Button dealButton;

		[SerializeField]
		private CardTemplate[] cards;

		private GameManager gameManager;

		void Start()
		{
			gameManager = transform.parent.Find("GameManager").GetComponent<GameManager>();
			dealButton.onClick.AddListener(OnDealButtonPressed);

			for (int i = 0; i < cards.Length; i++)
            {
				int index = i;
				cards[i].button.onClick.AddListener(() => OnCardPressed(index));
			}
		}

		// Event that triggers when deal button is pressed
		private void OnDealButtonPressed()
		{
			gameManager.Deal();
		}

		// Event that triggers when a card button is pressed
		public void OnCardPressed(int cardNo)
		{
			if (gameManager.dealed)
            {
				bool hold = gameManager.ToggleHold(cardNo);
				if (hold)
				{
					cards[cardNo].holdText.SetActive(true);
				}
				else
				{
					cards[cardNo].holdText.SetActive(false);
				}
			}
		}

		public void DisplayHand(Card[] hand)
        {
			for (int i = 0; i < cards.Length; i++)
            {
				cards[i].image.sprite = hand[i].image;
				cards[i].holdText.SetActive(false);
			}
        }

		public void ClearResultText()
		{
			winningText.text = "";
		}

		public void DisplayResult(GameManager.Result result)
        {
			if (result.prize > 0)
			{
				// Won: Show name of the hand and number of credits won
				winningText.text = result.handType + "! You won " + result.prize + " credits.";
			}
			else
			{
				// Lost
				winningText.text = "You lose. Better luck next time.";
			}
		}

		public void DisplayBalance(int balance)
        {
			currentBalanceText.text = "Balance: " + balance.ToString() + " Credits";
		}
	}
}