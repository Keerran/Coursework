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
		// Get the base component from this object.
		Base b = this.GetComponentInParent<Base>();
		// Get normal vector.
		Vector3 x = (other.ClosestPoint(transform.position)-transform.position).normalized;
		// Get the reaction force from that surface.
		float reaction = Math.Max(0,Vector3.Dot(b.Force, x));
		// Get the maximum frictional force.
		float max = coefficient * reaction;
		// Get the velocity parallel to the surface by taking the
		// velocity perpendicular to the surface and take that away from the
		// total velocity.
		Vector3 velocity = (b.Velocity - Vector3.Dot(b.Velocity,x)*x).normalized;
		// Get the force perpendicular to the surface.
		float p = Vector3.Dot(b.Force, velocity);
		// Get rid of the old force.
		b.Force -= -force;
		// Get the new force.
		if(max > p && velocity.magnitude == 0)
		{
			this.force = p * velocity;
		}
		else
		{
			this.force = max * velocity;
		}
		// Apply the new force.
		b.Force += -force;
	}
	
	void OnTriggerExit(Collider other)
	{
		if(!Master.INSTANCE.isPlaying) return;
		// Get rid of the old force when the object leaves the plane.
		Base b = this.GetComponentInParent<Base>();
		b.Force -= -force;
		force = Vector3.zero;
	}
}
