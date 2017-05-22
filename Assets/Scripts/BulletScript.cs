using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	[Range (0.001f, 25f)]
	public float lifetime;

	private void Start()
	{
		// In case the bullet is fired at the sky and never collides with anything.
		Invoke("ClearBullet", lifetime);
	}

	private void OnCollisionEnter(Collision col)
	{
		GameObject hit = col.gameObject;

		if (hit.tag == "Sheep")
		{
			// Do some mathy stuff here, Yash!
		}

		ClearBullet();
	}

	/// <summary>
	/// Removes the bullet from play.
	/// </summary>
	public void ClearBullet()
	{
		Destroy(gameObject);
	}
}
