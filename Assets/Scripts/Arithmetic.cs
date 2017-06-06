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

	// Lives

	public GameObject Life1;
	public GameObject Life2;
	public GameObject Life3;
	public GameObject Life4;
	public GameObject Life5;

	// Points

	public GameObject Point1;
	public GameObject Point2;
	public GameObject Point3;
	public GameObject Point4;
	public GameObject Point5;
	public GameObject Point6;
	public GameObject Point7;
	public GameObject Point8;
	public GameObject Point9;
	public GameObject Point10;
	public GameObject Point11;
	public GameObject Point12;
	public GameObject Point13;
	public GameObject Point14;
	public GameObject Point15;
	public GameObject Point16;
	public GameObject Point17;
	public GameObject Point18;

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

		// Enabling all lives at the start of game
		countLivesIconUpdate();

		// Disabling all points at the start of game
		countPointsIconUpdate();
	}
	
	void BeginGame()
	{
		GameController.OnNewRound -= BeginGame;

		// Printing the Integer on Screen at Time Intervals
		//InvokeRepeating("PrintIntegerOnScreen", 0.0f, 90.0f);
		PrintIntegerOnScreen();
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
		if (countdownValue == 0)
		{
			// ADD ROUND LOSS UPDATE CODE HERE
			countLives--;
			StartCoroutine (LossofLife ());
			countLivesIconUpdate ();

			PrintIntegerOnScreen();
		}
	}

	void countLivesIconUpdate (){

		if (countLives == 5) {
			Life1.SetActive (true);
			Life2.SetActive (true);
			Life3.SetActive (true);
			Life4.SetActive (true);
			Life5.SetActive (true);
		}

		if (countLives == 4) {
			Life1.SetActive (true);
			Life2.SetActive (true);
			Life3.SetActive (true);
			Life4.SetActive (true);
			Life5.SetActive (false);

		}

		if (countLives == 3) {

			Life1.SetActive (true);
			Life2.SetActive (true);
			Life3.SetActive (true);
			Life4.SetActive (false);
			Life5.SetActive (false);
		}

		if (countLives == 2) {

			Life1.SetActive (true);
			Life2.SetActive (true);
			Life3.SetActive (false);
			Life4.SetActive (false);
			Life5.SetActive (false);
		}

		if (countLives == 1) {
			Life1.SetActive (true);
			Life2.SetActive (false);
			Life3.SetActive (false);
			Life4.SetActive (false);
			Life5.SetActive (false);
		}

		if (countLives == 0) {
			Life1.SetActive (false);
			Life2.SetActive (false);
			Life3.SetActive (false);
			Life4.SetActive (false);
			Life5.SetActive (false);

			// Calling gameOver Routine since all lives are lost
			StartCoroutine(gameOver());

		}

	}

	// Display Instructions when the game starts for a few seconds and then make it disappear
	IEnumerator gameOver() {
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "All your lives are lost. Game over.";
		yield return new WaitForSeconds(2f);
		Application.Quit ();
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
		else if (val == 11)
			beforeOperator = "+";

		// If score is a - operator
		else if (val == 12)
			beforeOperator = "-";
		
		// If score is a x operator
		else if (val == 13)
			beforeOperator = "x";
		
		// If score is a / operator
		else if (val == 14)
			beforeOperator = "/";

		result =  value;




		// STEP 2: Setting GUItext Equation to Collided Sheep's Value

		// If a Succesfull Match is Made for the Equation==Result
		if (value.Equals (randomInt)) {

			// ADD POINT SYSTEM UPDATE CODE HERE

			countPoints++;
			countPointsIconUpdate ();

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

			if (countdownFill != null)
			{
				countdownFill.fillAmount = 1f;
			}

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
		yield return new WaitForSeconds(6f);
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "";
	}

	// Display Loss of Life Prompt
	IEnumerator LossofLife() {
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "Ran out of time! One life has been lost.";
		yield return new WaitForSeconds(2f);
		errorPrompt.GetComponent<UnityEngine.UI.Text> ().text = "";
	}


	void countPointsIconUpdate (){

		if (countPoints == 0) {
			Point1.SetActive (false);
			Point2.SetActive (false);
			Point3.SetActive (false);
			Point4.SetActive (false);
			Point5.SetActive (false);
			Point6.SetActive (false);
			Point7.SetActive (false);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 1) {
			Point1.SetActive (true);
			Point2.SetActive (false);
			Point3.SetActive (false);
			Point4.SetActive (false);
			Point5.SetActive (false);
			Point6.SetActive (false);
			Point7.SetActive (false);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);

		}

		if (countPoints == 2) {

			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (false);
			Point4.SetActive (false);
			Point5.SetActive (false);
			Point6.SetActive (false);
			Point7.SetActive (false);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 3) {

			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (false);
			Point5.SetActive (false);
			Point6.SetActive (false);
			Point7.SetActive (false);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 4) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (false);
			Point6.SetActive (false);
			Point7.SetActive (false);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 5) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (false);
			Point7.SetActive (false);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 6) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (false);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 7) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (false);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 8) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (false);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 9) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (false);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 10) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (false);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 11) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (false);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 12) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (true);
			Point13.SetActive (false);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 13) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (true);
			Point13.SetActive (true);
			Point14.SetActive (false);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 14) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (true);
			Point13.SetActive (true);
			Point14.SetActive (true);
			Point15.SetActive (false);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 15) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (true);
			Point13.SetActive (true);
			Point14.SetActive (true);
			Point15.SetActive (true);
			Point16.SetActive (false);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 16) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (true);
			Point13.SetActive (true);
			Point14.SetActive (true);
			Point15.SetActive (true);
			Point16.SetActive (true);
			Point17.SetActive (false);
			Point18.SetActive (false);
		}

		if (countPoints == 17) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (true);
			Point13.SetActive (true);
			Point14.SetActive (true);
			Point15.SetActive (true);
			Point16.SetActive (true);
			Point17.SetActive (true);
			Point18.SetActive (false);
		}

		if (countPoints == 18) {
			Point1.SetActive (true);
			Point2.SetActive (true);
			Point3.SetActive (true);
			Point4.SetActive (true);
			Point5.SetActive (true);
			Point6.SetActive (true);
			Point7.SetActive (true);
			Point8.SetActive (true);
			Point9.SetActive (true);
			Point10.SetActive (true);
			Point11.SetActive (true);
			Point12.SetActive (true);
			Point13.SetActive (true);
			Point14.SetActive (true);
			Point15.SetActive (true);
			Point16.SetActive (true);
			Point17.SetActive (true);
			Point18.SetActive (true);
		}

	}






	void OnDestroy()
	{
		if (primaryArithmetic == this) {
			primaryArithmetic = null;
		}
	}
}
