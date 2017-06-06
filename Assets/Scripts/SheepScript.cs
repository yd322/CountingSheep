using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepScript : MonoBehaviour
{
	public int sheepValue;
	public GameObject bodyPiece;
	public float moveSpeed = 2f;
	public float turnSpeed = 0.001f;
	public ParticleSystem explosion;

	private MeshRenderer myMR;
	private AudioSource myAS;
	private Transform destination;
	private float attentionSpan;
	private IEnumerator turnRoutine;
	private bool inTurnRoutine = false;
	private IEnumerator walkRoutine;
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

		if (bodyPiece != null)
		{
			myMR = bodyPiece.GetComponent<MeshRenderer>();
		}

		myAS = GetComponent<AudioSource>();

		attentionSpan = Random.Range(3f, 7.5f);
	}

	void Update()
	{
		// Prevent non-Y rotation.
		transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
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

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 2f);
			float currentAngle = transform.rotation.eulerAngles.y;

			while (Mathf.Abs(currentAngle - previousAngle) > threshold)//(previousAngle != currentAngle)//(Mathf.Abs(currentAngle - endAngle) % 360f) > threshold)
			{
				yield return null;

				previousAngle = currentAngle;

				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 2f);
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

		while (initPos != destination.position && ((Time.time - startTime) * moveSpeed) < 1f && Time.time < hardEndTime)
		{ 
			move = Mathf.Lerp (0,1, Mathf.Clamp((Time.time - startTime) * moveSpeed, 0f, moveSpeed));

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
