using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepScript : MonoBehaviour
{
	public int sheepValue;

	public void KillSheep()
	{
		if (Arithmetic.primaryArithmetic != null) {


			Arithmetic.primaryArithmetic.PrintEquationOnScreen (sheepValue);
		}

		Destroy(gameObject);
	}
}
