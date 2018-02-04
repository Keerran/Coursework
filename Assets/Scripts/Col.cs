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
		if(!Master.INSTANCE.isPlaying) return;		
		Base b1  = GetComponentInParent<Base>();
		Base b2  = other.gameObject.GetComponentInParent<Base>();
		if(b2 == null) return;
		if(b1.collided)
		{
			b1.collided = false;
			return;
		}
		Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHHH");
		//other.transform.rotation = Quaternion.FromToRotation(transform.forward,b1.Velocity);
		//this.transform.rotation = Quaternion.FromToRotation(transform.forward,b2.Velocity);
		float m1 = b1.Mass;
		float m2 = b2.Mass;
		Vector3 x = (b2.transform.position - b1.transform.position).normalized;
		float u1 = Vector3.Dot(b1.Velocity, x) / x.magnitude;
		float u2 = Vector3.Dot(b2.Velocity, x) / x.magnitude;
		//Stuff gets called one after the other arghhhhhh....
		float v1 = ((m1-m2)*u1 + m2*(2)*u2) / (m1+m2);
		float v2 = ((m2-m1)*u2 + m1*(2)*u1) / (m1+m2);
		Debug.Log(v1);
		Debug.Log(v2);
		Debug.Log(v1 * x);
		//b2.Force = Vector3.zero;
		b1.Velocity = b1.Velocity - u1 * x + v1 * x;
		b2.Velocity = b2.Velocity - u2 * x + v2 * x;
		b2.collided = true;
		//this.transform.rotation = Quaternion.identity;
	}
}