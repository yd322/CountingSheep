using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Arithmetic : MonoBehaviour {

	public GameObject operation;
	public GameObject currentvalue;
	public GameObject expected;
	public GameObject countdown;

	public GameObject errorPrompt;
	public Image countdownFill;

	private int countLives = 5;
	private int countPoints = 0;

	public static Arithmetic primaryArithmetic;
	private int valueGenMin = 1;
	private int valueGenMax = 10;
	private int valueGenMod = 2;
	private int value;
	private string equationnew;
	public int initial = 1;
	public int randomInt = 0;
	private string beforeOperator = "+";
	private int result = 0;
	private int countdownValue = 90;

	void Awake()
	{
		if (primaryArithmetic == null) {
			primaryArithmetic = this;
		}
	}

	// Use this for initialization
	void Start () {

		GameController.OnNewRound += BeginGame;


		operation.GetComponent<UnityEngine.UI.Text> ().text = "+";
		currentvalue.GetComponent<UnityEngine.UI.Text> ().text = "0";
		expected.GetComponent<UnityEngine.UI.Text> ().text = "";
		countdown.GetComponent<UnityEngine.UI.Text> ().text = "1:" + (countdownValue-60);

		// Initial Display Message
		StartCoroutine(InitialLoad());
	}
	
	void BeginGame()
	{
		GameController.OnNewRound -= BeginGame;

		// Printing the Integer on Screen at Time Intervals
		InvokeRepeating("PrintIntegerOnScreen", 0.0f, 90.0f);
		InvokeRepeating("Countdown", 0.0f, 1.0f);
	}

	void Countdown()
	{
		if (countdownValue < 60) {
			int display = countdownValue;
			countdown.GetComponent<UnityEngine.UI.Text> ().text = "0:" + display.ToString ("D2");
		} else {
			int display = (countdownValue - 60);
			countdown.GetComponent<UnityEngine.UI.Text> ().text = ("1:" + display.ToString ("D2"));
		}

		if (countdownFill != null) {
			if (countdownValue == 90) {
				countdownFill.fillAmount = 1f;
			} else {
				countdownFill.fillAmount -= (1f / 180f);
			}
		}

		
		countdownValue--;
	}


	void PrintIntegerOnScreen()
	{
		GameController.RaiseOnNewRound();

		// Generating a Random Integer between 1 and 100
		randomInt = Random.Range(valueGenMin,valueGenMax);

		valueGenMin += valueGenMod;
		valueGenMax += (int)(1.5f * (float)valueGenMod);

		// Setting GUItext Score to a Random Generated Value
		expected.GetComponent<UnityEngine.UI.Text>().text = "" + randomInt.ToString();
		// Setting UI to Default
		currentvalue.GetComponent<UnityEngine.UI.Text> ().text = "0";
		operation.GetComponent<UnityEngine.UI.Text> ().text = "+";


		// Countdown Value RESET back to 90
		countdownValue = 90;

		// Resetting all variables
		value = 0;
		equationnew = "";
		initial = 1;
		beforeOperator = "+";
	
	}


	public void PrintEquationOnScreen(int val)
	{
		
		// STEP 1: Calculating Result


		// If score is an integer between 1-10
		if ((val < 11)){
			
			if ((beforeOperator == "+") || (beforeOperator == ""))
				value += val;
			if (beforeOperator == "-") {
				value -= val;

				// Setting Threshold as 0, and not allowing any negative values
				if (value < 0)
					value = 0;
			}
			if (beforeOperator == "x")
				value *= val;
			if (beforeOperator == "/")
				value /= val;
			// When a sheep with an Operator was not selected in the previous shot, nothing happens

		}

		// If score is a + operator
		if (val == 11)
			beforeOperator = "+";

		// If score is a - operator
		if (val == 12)
			beforeOperator = "-";
		
		// If score is a x operator
		if (val == 13)
			beforeOperator = "x";
		
		// If score is a / operator
		if (val == 14)
			beforeOperator = "/";

		result =  value;




		// STEP 2: Setting GUItext Equation to Collided Sheep's Value

		// If a Succesfull Match is Made for the Equation==Result
		if (value.Equals (randomInt)) {

			// Setting UI to Default
			currentvalue.GetComponent<UnityEngine.UI.Text> ().text = "0";
			operation.GetComponent<UnityEngine.UI.Text> ().text = "+";

			// Displaying Success
			StartCoroutine(SuccessfullyMatched());



			// Resetting all variables
			value = 0;
			val = 0;
			equationnew = "";
			initial = 1;
			beforeOperator = "+";
			PrintIntegerOnScreen ();
		}
		else {
			operation.GetComponent<UnityEngine.UI.Text> ().text = "" + beforeOperator;
			currentvalue.GetComponent<UnityEngine.UI.Text> ().text = "" + result;
		}

	}

	// Display "Values Matched" for a second and make it disappear
	IEnumerator SuccessfullyMatched() {
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "Values Succesfully Matched!";
		yield return new WaitForSeconds(2);
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "";
	}

	// Display Instructions when the game starts for a few seconds and then make it disappear
	IEnumerator InitialLoad() {
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "Shoot Sheep to choose an Operator or Value for the equation below";
		yield return new WaitForSeconds(10f);
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "";
	}


	void OnDestroy()
	{
		if (primaryArithmetic == this) {
			primaryArithmetic = null;
		}
	}
}
