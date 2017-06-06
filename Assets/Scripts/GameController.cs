using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public delegate void NewRound();
	public static event NewRound OnNewRound;

	public static void RaiseOnNewRound()
	{
		if (isFirstRound)
		{
			isFirstRound = false;
		}
		else
		{
			SheepScript.IncreaseSheepSpeedGlobal();
		}

		if (OnNewRound != null)
		{
			OnNewRound();
		}
	}

	public static GameController primaryGC;

	public Transform[] spawnPoints;
	public SheepScript sheepTemplate;
	public Material[] sheepSkins;

	private static bool isFirstRound = true;

	void Awake()
	{
		if (primaryGC == null)
		{
			primaryGC = this;
		}
	}

	void Start()
	{
		StartCoroutine(InitializeGame());
	}

	protected IEnumerator InitializeGame()
	{
		// Spawn some initial number of sheep. Currently 5 for every spawn point.
		for (int x = 0; x < 7; x++)
		{
			// Wait a moment for the new sheep to move out of the way.
			yield return new WaitForSeconds(2f);

			for (int y = 0; y < spawnPoints.Length; y++)
			{
				if (spawnPoints[y])
				{
					SpawnSheep(spawnPoints[y]);
				}
				else
				{
					Debug.Log("Invalid spawn point found in GameController.SheepSpawnRandom(). Index: " + y + ".");
				}
			}
		}

		StartNewRound();
	}

	public void SpawnSheepRandom()
	{
		if (spawnPoints.Length > 0)
		{
			int choice = Random.Range(0, spawnPoints.Length);

			if (spawnPoints[choice])
			{
				SpawnSheep(spawnPoints[choice]);
			}
			else
			{
				Debug.Log("Invalid spawn point found in GameController.SheepSpawnRandom(). Index: " + choice + ".");
			}
		}
	}

	protected void SpawnSheep(Transform spawnPoint)
	{
		if (sheepTemplate)
		{
			SheepScript newSheep = (SheepScript)Instantiate(sheepTemplate, spawnPoint.position, spawnPoint.rotation);

			if (newSheep)
			{
				int sheepVal;

				// We're making this sheep have a number.
				if (ShouldBeNumber())
				{
					sheepVal = Random.Range(0, 10);
				}
				// We're making this sheep have an operator.
				else
				{
					int result = Random.Range(0, 3);

					// Give operator sheep a plus sign a third of the time.
					if (result == 0)
					{
						sheepVal = 10;
					}
					else
					{
						sheepVal = Random.Range(11, sheepSkins.Length);
					}
				}

				newSheep.SetSheepValue(sheepVal, sheepSkins[sheepVal]);

				Rigidbody sheepRB = newSheep.GetComponent<Rigidbody>();

				if (sheepRB)
				{
					//sheepRB.AddForce(1000f * newSheep.transform.forward);
					//sheepRB.velocity = 20f * Vector3.Normalize(spawnPoint.up + spawnPoint.forward);
					//sheepRB.AddForce(Vector3.Normalize(spawnPoint.up + (spawnPoint.right * -1f)) * 20f, ForceMode.VelocityChange);
					//sheepRB.AddForce(Vector3.Normalize(spawnPoint.up + spawnPoint.forward) * 20f);
				}
				else
				{
					Debug.Log("No rigidbody found on spawned sheep.");
				}

				//Debug.Log("Sheep spawned.");
			}
		}
		else
		{
			Debug.Log("Invalid sheep template provided to GameController.");
		}
	}

	/// <summary>
	/// Determines whether a spawn sheep should have a number or an operator.
	/// 
	/// Current ratio is number:operator 2:1.
	/// </summary>
	/// <returns><c>true</c>, if the spawned sheep should have a number, <c>false</c> otherwise.</returns>
	protected bool ShouldBeNumber()
	{
		int result = Random.Range(0, 3);

		if (result == 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public void StartNewRound()
	{
		RaiseOnNewRound();
	}

	void OnDestroy()
	{
		if (primaryGC == this)
		{
			primaryGC = null;
		}
	}
}
