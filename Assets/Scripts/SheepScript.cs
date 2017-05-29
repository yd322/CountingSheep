using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepScript : MonoBehaviour
{
	public int sheepValue;
	public GameObject bodyPiece;

	private MeshRenderer myMR;
	//private Rigidbody myRB;

	void Awake()
	{
		if (bodyPiece != null)
		{
			myMR = bodyPiece.GetComponent<MeshRenderer>();
		}

		//myRB = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		/*
		if (myRB)
		{

		}
		*/

		transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
	}

	public void SetSheepValue(int val, Material skin)
	{
		sheepValue = val;

		if (skin != null)
		{
			if (myMR == null && bodyPiece != null)
			{
				myMR = GetComponent<MeshRenderer>();
			}

			if (myMR != null)
			{
				myMR.material = skin;
			}
		}
		else
		{
			Debug.Log("Invalid material provided to SheepScript.SetSheepValue().");
		}
	}

	public void KillSheep()
	{
		if (Arithmetic.primaryArithmetic != null)
		{
			Arithmetic.primaryArithmetic.PrintEquationOnScreen (sheepValue);
		}

		if (GameController.primaryGC != null)
		{
			GameController.primaryGC.SpawnSheepRandom();
		}

		Destroy(gameObject);
	}
}
