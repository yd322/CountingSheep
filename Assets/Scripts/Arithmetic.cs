using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Arithmetic : MonoBehaviour {

	public GameObject score;
	public GameObject equation;

	public static Arithmetic primaryArithmetic;
	private int value ;
	private string equationnew;
	public int initial = 1;
	public int randomInt = 0;


	void Awake()
	{
		if (primaryArithmetic == null) {
			primaryArithmetic = this;
		}
	}

	// Use this for initialization
	void Start () {

		// Printing the Integer on Screen at Time Intervals
		InvokeRepeating("PrintIntegerOnScreen", 0.0f, 7.0f);

		equation.GetComponent<UnityEngine.UI.Text> ().text = "Equation: ";
	}
	
	// Update is called once per frame
	void Update () {


	}

	void PrintIntegerOnScreen()
	{
		// Generating a Random Integer between 1 and 100
		randomInt = Random.Range(0,100);

		// Setting GUItext Score to a Random Generated Value
		score.GetComponent<UnityEngine.UI.Text>().text = "Result: " + randomInt.ToString();

	}


	public void PrintEquationOnScreen(int val)
	{
		


		// Setting GUItext Equation to Collided Sheep's Value
		value += val;
		var result = " = " + value;

		// If its the initial bullet
		if (initial == 1) {
			equationnew += val;
			initial = 0;
		}
		else
			equationnew += "+ " + val;
		
		equation.GetComponent<UnityEngine.UI.Text> ().text = "Equation: " + equationnew + result;


	}


	void OnDestroy()
	{
		if (primaryArithmetic == this) {
			primaryArithmetic = null;
		}
	}
}
