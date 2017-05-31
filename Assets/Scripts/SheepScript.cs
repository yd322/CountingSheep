using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepScript : MonoBehaviour
{
	public int sheepValue;
	public GameObject bodyPiece;
	public float moveSpeed = 5f;
	public float turnSpeed = 0.1f;

	private MeshRenderer myMR;
	//private Rigidbody myRB;
	private Transform destination;
	private float attentionSpan;
	private IEnumerator turnRoutine;
	private IEnumerator walkRoutine;

	void Awake()
	{
		if (bodyPiece != null)
		{
			myMR = bodyPiece.GetComponent<MeshRenderer>();
		}

		//myRB = GetComponent<Rigidbody>();

		SetNewDestination();
		attentionSpan = Random.Range(3f, 7.5f);
		//turnRoutine = StartCoroutine(Turn());
	}

	void FixedUpdate()
	{
		/*
		if (myRB)
		{

		}
		*/

		// Prevent non-Y rotation.
		transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
	}

	private void SetNewDestination()
	{
		if (destination == null)
		{
			destination = (new GameObject()).transform;
		}

		Vector3 destPos = new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(-15f, 15f));
		destination.position = destPos;
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
		if (walkRoutine != null)
		{
			StopCoroutine(walkRoutine);
		}

		Quaternion startRot = transform.rotation;
		float currentRot = transform.rotation.y;
		//transform.rotation = Quaternion.Slerp(startRot, to.rotation, Time.time * speed);

		yield return null;

		walkRoutine = Walk();
		StartCoroutine(walkRoutine);
	}

	private IEnumerator Walk()
	{
		float startTime = Time.time;
		float hardEndTime = startTime + attentionSpan;
		Vector3 initPos = transform.position;
		Vector3 dir = transform.forward;

		float move;

		while (initPos != destination.position && ((Time.time - startTime) * moveSpeed) < 1f && startTime < hardEndTime)
		{ 
			move = Mathf.Lerp (0,1, (Time.time - startTime) * moveSpeed);

			transform.position += dir * move;

			yield return null;
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

		if (hit.tag != "Player")
		{
			if (turnRoutine != null)
			{
				StopCoroutine(turnRoutine);
			}

			//ReverseDestination();
			SetNewDestination();

			turnRoutine = Turn();
			//StartCoroutine(turnRoutine);
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
