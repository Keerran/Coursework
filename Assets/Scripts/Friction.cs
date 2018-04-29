using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
	private float coefficient;
	private Vector3 force;
	
	//Coefficient of Friction
	[SerializeField]
	[ShowValue]
	public float Coefficient {
		get { return coefficient; }
		set { coefficient = Mathf.Clamp(value, 0f, 1f); }
	}

	static Friction()
	{
		UI.registerClass<Friction>();
	}

	void Start()
	{
		Coefficient = 0.1f;	
	}

	void OnTriggerStay(Collider other)
	{
		if(!Master.INSTANCE.isPlaying) return;
		Base b = this.GetComponentInParent<Base>();
		Vector3 x = (other.ClosestPoint(transform.position)-transform.position).normalized;
		float reaction = Math.Max(0,Vector3.Dot(b.Force, x));
		float max = coefficient * reaction;
		Vector3 velocity = (b.Velocity - Vector3.Dot(b.Velocity,x)*x).normalized;
		float p = Vector3.Dot(b.Force, velocity);
		b.Force += force;
		if(max > p && velocity.magnitude == 0)
		{
			this.force = p * velocity;
		}
		else
		{
			this.force = max * velocity;
		}
		b.Force -= force;
	}
	
	void OnTriggerExit(Collider other)
	{
		if(!Master.INSTANCE.isPlaying) return;
		Base b = this.GetComponentInParent<Base>();
		b.Force += force;
		force = Vector3.zero;
	}
}
