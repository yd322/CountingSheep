using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	[Range (0.001f, 25f)]
	public float lifetime;
	public AudioClip[] fireSounds;

	private AudioSource myAS;

	private void Awake()
	{
		myAS = GetComponent<AudioSource>();
	}

	private void Start()
	{
		PlayFireSound();

		// In case the bullet is fired at the sky and never collides with anything.
		Invoke("ClearBullet", lifetime);
	}

	private void OnCollisionEnter(Collision col)
	{
		GameObject hit = col.gameObject;

		if (hit.tag == "Sheep")
		{
			SheepScript sheep = hit.GetComponent<SheepScript> ();

			if (sheep != null)
			{
				sheep.KillSheep();
			}
			// Check the parent.
			else if (hit.transform.parent != null)
			{
				sheep = hit.transform.parent.GetComponent<SheepScript>();

				if (sheep != null)
				{
					sheep.KillSheep();
				}
			}
		}

		ClearBullet();
	}

	protected void PlayFireSound()
	{
		SoundManager.PlayRandomSoundFromSource(myAS, fireSounds);
	}

	/// <summary>
	/// Removes the bullet from play.
	/// </summary>
	public void ClearBullet()
	{
		Destroy(gameObject);
	}
}
