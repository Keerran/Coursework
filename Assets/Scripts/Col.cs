using System;
using UnityEngine;

public class Col : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		// This makes sure that the collisions only happen when
		// the game is not paused.
		if(!Master.INSTANCE.isPlaying) return;	
		// Gets the base script from the current object.
		Base b1  = GetComponentInParent<Base>();
		// Gets the base script from the colliding object.
		Base b2  = other.gameObject.GetComponentInParent<Base>();
		// Makes sure the colliding object is particle.
		if(b2 == null) return;
		// Makes sure the collision calculation only happens once.
		if(b1.collided)
		{
			b1.collided = false;
			return;
		}
		//other.transform.rotation = Quaternion.FromToRotation(transform.forward,b1.Velocity);
		//this.transform.rotation = Quaternion.FromToRotation(transform.forward,b2.Velocity);
		// Gets the mass of both objects.
		float m1 = b1.Mass;
		float m2 = b2.Mass;
		// This gets the direction from one object to the other.
		Vector3 x = (b2.transform.position - b1.transform.position).normalized;
		// Gets the velocity of both objects in the direction x.
		float u1 = Vector3.Dot(b1.Velocity, x) / x.magnitude;
		float u2 = Vector3.Dot(b2.Velocity, x) / x.magnitude;
		//Stuff gets called one after the other arghhhhhh....
		// Gets the final velocity.
		float v1 = ((m1-m2)*u1 + m2*(2)*u2) / (m1+m2);
		float v2 = ((m2-m1)*u2 + m1*(2)*u1) / (m1+m2);
		//b2.Force = Vector3.zero;
		// Adds the final velocity to the tangential component of the initial velocity.
		b1.Velocity = b1.Velocity - u1 * x + v1 * x;
		b2.Velocity = b2.Velocity - u2 * x + v2 * x;
		// Sets the collided variable to true to make sure
		// that the collision only gets calculated once.
		b2.collided = true;
		//this.transform.rotation = Quaternion.identity;
	}
}
