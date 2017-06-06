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
			countdown.GetComponent<UnityEngine.UI.Text> ().text = "0:" + countdownValue;
		} else {
			countdown.GetComponent<UnityEngine.UI.Text> ().text = "1:" + (countdownValue - 60);
		}

	
		if (countdownFill != null) {
			if (countdownValue == 90) {
				countdownFill.fillAmount = 1f;
				Debug.Log ("YO YO YO");
			} else {
				countdownFill.fillAmount -= (1f / 180f);
				Debug.Log ("NO NO NO");

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
		countdownValue = 90;
	}


	public void PrintEquationOnScreen(int val)
	{
		
		// STEP 1: Calculating Result


		// If score is an integer between 1-10
		if ((val < 11)){
			
			if ((beforeOperator == "+") || (beforeOperator == ""))
				value += val;
			if (beforeOperator == "-")
				value -= val;
			if (beforeOperator == "x")
				value *= val;
			if (beforeOperator == "/")
				value /= val;
			else // When a sheep with an Operator was not selected in the previous shot, nothing happens
				value = value;	
			
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
			errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "Values Succesfully Matched!";



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


	void OnDestroy()
	{
		if (primaryArithmetic == this) {
			primaryArithmetic = null;
		}
	}
}
