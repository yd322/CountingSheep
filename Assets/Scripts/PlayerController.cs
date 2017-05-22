using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public GameObject projectile;
	public Vector3 projectileSpawnOffset;
	[Range (0f, 10f)]
	public float shotCooldown;
	[Range (0.001f, 25f)]
	public float shotSpeed;

	private float shotCooldownRemaining = 0f;

	void Update()
	{
		if (shotCooldownRemaining <= 0)
		{
			if (Input.GetButton("Fire1"))
			{
				shotCooldownRemaining = shotCooldown;

				FireWeapon();
			}
		}
		else
		{
			shotCooldownRemaining -= Time.deltaTime;
		}
	}

	protected void FireWeapon()
	{
		if (projectile != null)
		{
			GameObject bullet = (GameObject)Instantiate(projectile, Camera.main.transform);
			bullet.transform.localPosition = projectileSpawnOffset; //Camera.main.transform.position + projectileSpawnOffset;

			Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

			if (bulletRB != null)
			{
				bulletRB.velocity = Camera.main.transform.forward * shotSpeed;
			}

			bullet.transform.SetParent(null);
		}
	}
}
