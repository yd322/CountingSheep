using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour
{
	void Awake()
	{
		Invoke("Kill", 1f);
	}

	private void Kill()
	{
		Destroy(gameObject);
	}
}
