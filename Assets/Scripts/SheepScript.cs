using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepScript : MonoBehaviour
{
	public int sheepValue;
	public GameObject bodyPiece;

	private MeshRenderer myMR;

	void Awake()
	{
		if (bodyPiece != null)
		{
			myMR = GetComponent<MeshRenderer>();
		}
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
	}

	public void KillSheep()
	{
		if (Arithmetic.primaryArithmetic != null) {


			Arithmetic.primaryArithmetic.PrintEquationOnScreen (sheepValue);
		}

		Destroy(gameObject);
	}
}
