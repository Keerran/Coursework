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
		Debug.Log("HELLO");
		if(!Master.INSTANCE.isPlaying) return;		
		Base b1  = other.gameObject.GetComponentInParent<Base>();
		Base b2  = GetComponentInParent<Base>();
		if(b1 == null) return;
		other.transform.rotation = Quaternion.FromToRotation(transform.forward,b1.Velocity);
		this.transform.rotation = Quaternion.FromToRotation(transform.forward,b2.Velocity);
		float m1 = b1.Mass;
		float m2 = b2.Mass;
		float u1 = other.transform.TransformDirection(b1.Velocity).x;
		float u2 = this.transform.TransformDirection(b2.Velocity).x;
		float p  = m1*u1 + m2*u2;
		float ke = m1*u1*u1 + m2*u2*u2;
		
		m2 *= 2;

		float denominator = 0.5f * m2 * m2 + m2 * m1;
		float rest = m2 * p;
		float discriminant = Mathf.Sqrt(m2 * m2 * (p * p) - (m2 * m2 + 2f * m2 * m1) * (p * p - ke * m1));

        float v2;
        if((rest - discriminant) / denominator == u2)
        {
            v2 = (rest + discriminant) / denominator;
        }
        else
        {
	        v2 = (rest - discriminant) / denominator;	        
        }
		
		Vector3 velocity = transform.TransformDirection(Vector3.forward * v2);
		b2.Velocity = velocity;
		b2.Force = Vector3.zero;
		this.transform.rotation = Quaternion.identity;
	}
}