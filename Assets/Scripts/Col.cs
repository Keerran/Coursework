using System;
using UnityEngine;

[Serializable]
public class Col : MonoBehaviour
{
	private static float e = 1;
	public static float E {
		get { return e; }
		set { e = Mathf.Clamp(value, 0f, 1f); }
	}
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(!Master.INSTANCE.isPlaying) return;

		Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, 1 << 8);
		foreach(Collider other in colliders)
		{
			
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(!Master.INSTANCE.isPlaying) return;
		// Get the base scripts from each colliding object.
		Base b1 = GetComponentInParent<Base>();
		Base b2 = other.gameObject.GetComponentInParent<Base>();
		// If its colliding with a plane...
		if(b2 == null)
		{
			if(other.gameObject.name == "Plane")
			{
				// Do plane stuff.
				//...Get the velocity towards the plane.
				Vector3 x = (other.ClosestPoint(b1.transform.position) - b1.transform.position).normalized;
				float u = Vector3.Dot(b1.Velocity, x);
				// Apply v = -eu
				float v = e * -u;
				// Get rid of old velocity and apply new velocity
				b1.Velocity = b1.Velocity + (v - u) * x;
			}
		}
		else
		{
			// If the other object had its OnTriggerEnter method called first the return.
			if(b1.collided)
			{
				b1.collided = false;
				return;
			}
			// Get both masses.
			float m1 = b1.Mass;
			float m2 = b2.Mass;
			
			// Get velocities of both towards the other.
			Vector3 x = (b2.transform.position - b1.transform.position).normalized;

			float u1 = Vector3.Dot(b1.Velocity, x);
			float u2 = Vector3.Dot(b2.Velocity, x);
			
			// Get final velocities using equation with restitution and momentum.
			float v1 = ((m1 - m2 * e) * u1 + m2 * (1 + e) * u2) / (m1 + m2);
			float v2 = ((m2 - m1 * e) * u2 + m1 * (1 + e) * u1) / (m1 + m2);

			// Get rid of old velocity and apply new velocity
			b1.Velocity = b1.Velocity + (v1 - u1) * x;
			b2.Velocity = b2.Velocity + (v2 - u2) * x;
			// Makes sure that OnTriggerEnter doesn't get called twice.
			b2.collided = true;
		}
	}
	

}