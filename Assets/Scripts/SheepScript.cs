using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepScript : MonoBehaviour
{
	private static float currentMoveSpeed;
	private static float moveSpeedMod = 0.005f;	// Amount added to movement speed at the end of each round.
	private static float currentTurnSpeed;
	private static float turnSpeedMod = 0.2f;	// Amount added to turning speed at the end of each round.

	public int sheepValue;				// The value of the sheep for the purposes of arithmetic.
	public GameObject bodyPiece;		// The main body segment of the sheep to which materials for different symbols are applied.
	[Range (0f, 10f)]
	public float baseMoveSpeed;			// Speed modifier for sheep forward movement. Current base: 0.05f.
	[Range (0f, 20f)]
	public float maxMoveSpeed;			// Maximum movement speed the sheep can achieve after some number of rounds.
	[Range (2f, 10f)]
	public float baseTurnSpeed;			// Speed in degrees that the sheep will be allowed to turn per frame. Current base: 2f.
	[Range (2f, 20f)]
	public float maxTurnSpeed;			// Maximum turning speed the sheep can achieve after some number of rounds.
	public ParticleSystem explosion;	// The ParticleSystem prefab to be spawned upon sheep murder.

	private MeshRenderer myMR;			// MeshRenderer of the bodyPiece.
	private AudioSource myAS;			// AudioSource for bleating.
	private Transform destination;		// Generated destination guide object.
	private float attentionSpan;		// Semi-randomly generated time that sheep will continue to move forward without colliding before turning.
	private IEnumerator turnRoutine;	// Reference to the sheep's potentially ongoing turning routine.
	private bool inTurnRoutine = false;	// Indicator of whether a turning routine is actually ongoing, to prevent turn looping.
	private IEnumerator walkRoutine;	// Reference to the sheep's potentially ongoing walking routine.
	//private static bool testSheepExists = false;
	//private bool isTestSheep = false;

	void Awake()
	{
		/*
		if (!testSheepExists)
		{
			testSheepExists = true;
			isTestSheep = true;
		}
		*/

		GameController.OnNewRound += IncreaseSheepSpeed;

		if (baseMoveSpeed > maxMoveSpeed)
		{
			baseMoveSpeed = maxMoveSpeed;
		}

		if (baseTurnSpeed > maxTurnSpeed)
		{
			baseTurnSpeed = maxTurnSpeed;
		}

		if (baseMoveSpeed > currentMoveSpeed)
		{
			currentMoveSpeed = baseMoveSpeed;
		}
		else
		{
			baseMoveSpeed = currentMoveSpeed;
		}

		if (baseTurnSpeed > currentTurnSpeed)
		{
			currentTurnSpeed = baseTurnSpeed;
		}
		else
		{
			baseTurnSpeed = currentTurnSpeed;
		}

		if (bodyPiece != null)
		{
			myMR = bodyPiece.GetComponent<MeshRenderer>();
		}

		myAS = GetComponent<AudioSource>();

		attentionSpan = Random.Range(3f, 7.5f);

		if (explosion != null)
		{
			Instantiate(explosion, transform.position, Quaternion.identity);
		}
	}

	void Update()
	{
		// Prevent non-Y rotation.
		transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
	}

	public static void IncreaseSheepSpeedGlobal()
	{
		currentMoveSpeed += moveSpeedMod;
		currentTurnSpeed += turnSpeedMod;
	}

	private void IncreaseSheepSpeed()
	{
		if (baseMoveSpeed < currentMoveSpeed)
		{
			baseMoveSpeed = currentMoveSpeed;
		}

		if (baseTurnSpeed < currentTurnSpeed)
		{
			baseTurnSpeed = currentTurnSpeed;
		}
	}

	private void SetNewDestination()
	{
		if (destination == null)
		{
			destination = (new GameObject()).transform;
			destination.gameObject.name = gameObject.name + "_destination";
		}

		Vector3 destPos = new Vector3(Random.Range(-15f, 15f), transform.position.y, Random.Range(-15f, 15f));
		destination.position = destPos;

		/*
		if (isTestSheep)
		{
			Debug.Log("New destination set at " + destPos + ".");
		}
		*/
	}

	private void ReverseDestination()
	{
		if (destination == null)
		{
			destination = (new GameObject()).transform;
		}

		Vector3 destPos = new Vector3(-1f * destination.position.x, destination.position.y, -1 * destination.position.z);
		destination.position = destPos;
	}

	private IEnumerator Turn()
	{
		//Debug.Log("Starting turning.");
		inTurnRoutine = true;

		if (walkRoutine != null)
		{
			StopCoroutine(walkRoutine);
		}

		if (destination != null)
		{
			float threshold = 0.05f;

			/*
			if (isTestSheep)
			{
				Debug.Log("Starting rotation: " + startRot.eulerAngles + ", current angle: " + currentAngle + ", ending rotation: " + endRot.eulerAngles + ", end angle: " + endAngle + ".");
			}
			*/

			int debugCounter = 120;

			Vector3 relativePos = new Vector3(destination.position.x - transform.position.x, transform.position.y, destination.position.z - transform.position.z);
			Quaternion targetRot = Quaternion.LookRotation(relativePos);
			float previousAngle = transform.rotation.eulerAngles.y;

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, currentTurnSpeed);
			float currentAngle = transform.rotation.eulerAngles.y;

			while (Mathf.Abs(currentAngle - previousAngle) > threshold)//(previousAngle != currentAngle)//(Mathf.Abs(currentAngle - endAngle) % 360f) > threshold)
			{
				yield return null;

				previousAngle = currentAngle;

				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, currentTurnSpeed);
				currentAngle = transform.rotation.eulerAngles.y;

				/*
				if (isTestSheep)
				{
					debugCounter--;
					if (debugCounter == 0)
					{
						debugCounter = 120;

						//Debug.Log("Current angle: " + currentAngle + ". Calculation: " + (Mathf.Abs(currentAngle - endAngle) % 360f) + ".");
						Debug.Log("Current angle: " + currentAngle + ". Previous angle: " + previousAngle + ".");
					}
				}
				*/
			}
		}

		/*
		if (isTestSheep)
		{
			Debug.Log("Done turning.");
		}
		*/

		walkRoutine = Walk();
		StartCoroutine(walkRoutine);
		inTurnRoutine = false;
	}

	private IEnumerator Walk()
	{
		//Debug.Log("Starting walking.");

		float startTime = Time.time;
		float hardEndTime = startTime + attentionSpan;
		Vector3 initPos = transform.position;
		Vector3 dir = transform.forward;

		float move;

		while (initPos != destination.position && ((Time.time - startTime) * currentMoveSpeed) < 1f && Time.time < hardEndTime)
		{ 
			move = Mathf.Lerp (0,1, Mathf.Clamp((Time.time - startTime) * currentMoveSpeed, 0f, currentMoveSpeed));

			transform.position += dir * move;

			yield return null;
		}

		/*
		if (isTestSheep)
		{
			Debug.Log("Done walking.");
		}
		*/

		if (myAS != null && myAS.clip != null)
		{
			int coinflip = Random.Range(0, 2);

			if (coinflip == 0)
			{
				myAS.PlayOneShot(myAS.clip);
			}
		}

		// Stand still and wait out any remaining time.
		if (Time.time < hardEndTime)
		{
			yield return new WaitForSeconds(hardEndTime - Time.time);
		}

		SetNewDestination();

		turnRoutine = Turn();
		StartCoroutine(turnRoutine);
	}

	private void OnCollisionEnter(Collision col)
	{
		GameObject hit = col.gameObject;

		if (hit.tag != "Player" && !inTurnRoutine)
		{
			if (turnRoutine != null)
			{
				StopCoroutine(turnRoutine);
			}

			SetNewDestination();
			//ReverseDestination();

			turnRoutine = Turn();
			StartCoroutine(turnRoutine);
		}
	}

	public void SetSheepValue(int val, Material skin)
	{
		sheepValue = val+1;

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
		GameController.OnNewRound -= IncreaseSheepSpeed;

		if (Arithmetic.primaryArithmetic != null)
		{
			Arithmetic.primaryArithmetic.PrintEquationOnScreen (sheepValue);
		}

		if (GameController.primaryGC != null)
		{
			GameController.primaryGC.SpawnSheepRandom();
		}

		StopAllCoroutines();

		if (explosion != null)
		{
			Instantiate(explosion, transform.position, Quaternion.identity);
		}

		if (destination != null)
		{
			Destroy(destination.gameObject);
		}

		Destroy(gameObject);
	}
}
