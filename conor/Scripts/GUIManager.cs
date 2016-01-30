using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
	public Text instructionsText;
	public Text dayText;
	public Button continueButton;
	public Button openStoreButton;
	public int levelNumber;

	void Start()
	{
		openStoreButton.gameObject.SetActive(false);

		if (levelNumber == 1) // first level
			displayInstructions(1);
		else
			displayInstructions(3);

		displayDay(1);
	}

	public void displayInstructions(int page)
	{
		switch (page)
		{
			case 1:
				instructionsText.text = "You are a coffee shop owner, and serial-killer. Your objective is to kill one of your customers within the week. Observe their behavior carefully...";
				break;
			case 2:
				instructionsText.text = "Now move the chairs to alter the customers' behavior. You must kill one of them before Monday.";
				openStoreButton.gameObject.SetActive(true);
				break;

			case 3:
				instructionsText.text = "Observe the customers.";
				break;

			case 4:
				instructionsText.text = "Now move the chairs to cause a customer to die.";
				break;
		}
	}

	public void displayDay(int day)
	{
		switch (day)
		{
			case 1:
				dayText.text = "Monday";
				break;
			case 2:
				dayText.text = "Tuesday";
				break;
			case 3:
				dayText.text = "Wednesday";
				break;
			case 4:
				dayText.text = "Thursday";
				break;
			case 5:
				dayText.text = "Friday";
				break;
			case 6:
				dayText.text = "Saturday";
				break;
			case 7:
				dayText.text = "Sunday";
				break;
		}

		day++;
		dayText.gameObject.SetActive(true);
	}

	private IEnumerator turnOffGUI (float delay)
	{
		yield return new WaitForSeconds(delay);
		dayText.gameObject.SetActive(false);
		instructionsText.gameObject.SetActive(false);
	}

	public void buttonClicked (int buttonNumber)
	{
		Debug.Log("Button " + buttonNumber.ToString() + " pressed.");
		switch (buttonNumber)
		{
			case 1: 
				// continue from first instruction
				// start the people moving the first time
				continueButton.gameObject.SetActive(false);
				instructionsText.gameObject.SetActive(false);
				dayText.gameObject.SetActive(false);
				break;

			case 2: 
				// open the store button
				StartCoroutine("turnOffGUI", 0.2f);

				// need a command here to start the people entering the store
				break;
			

		}
	}
	
}
