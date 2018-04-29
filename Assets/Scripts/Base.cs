using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class Base : Selectable
{
	Transform arrow;
	public bool collided;
	//Stores the velocity vector.
	[SerializeField]
	private Vector3 vel;

	// Stores the mass of the object.
	[ShowValue(order=0)]
	public float Mass { get; set; }
	
	[SerializeField]
	private Vector3 force;
	
	// Stores the force on the object as a 3D vector
	// After being changed it will change the direction of the
	// force arrow appropriately.
	[ShowValue(order=1)]
	public Vector3 Force
	{
		get { return force; }
		set { force = value; setArrow(); }
	}
	
	[ShowValue(order=2)]
	public Vector3 Velocity { get; set; }

	// Use this for initialization
	static Base()
	{
		// Registers this class as one that should show up in the UI.
		UI.registerClass<Base>();
		//resetForce();
	}

	void Start()
	{
		Init();
	}
	
	public void Init()
	{
		Mass = 1;
		resetForce();
		arrow = null;
		Master.INSTANCE.playPause += onPlayPause;
		Master.INSTANCE.trailChange += onTrailChange;
		// Goes through every child object of this object.
		foreach(Transform transform in transform)
		{
			// Checks if it has the "Force Arrow" tag.
			if(transform.CompareTag("Force Arrow"))
			{
				arrow = transform;
				break;
			}
		}
		
		// Makes sure the Force Arrow was found and if not throws an error.
		if(arrow == null)
		{
			Debug.LogError("Error in setting force: could not find Force Arrow");
			return;
		}
		arrow.gameObject.SetActive(force == Vector3.zero);
	}

	// Update is called once per frame
	void LateUpdate()
	{
		// Makes sure nothing happens if the game is paused.
		if(!Master.INSTANCE.isPlaying) return;
		// If the mass is not set, it will default to 0,
		// which will cause a divide by zero error.
		if(Mass == 0) return;
		Vector3 pos = this.transform.position;
		
		// Scales the time between the last frame by the speed set by the user
		// to get the in-game time between the last frame.
		float td = Time.deltaTime * Master.INSTANCE.speed;
		// Uses a = F/m and a = dv/dt to get velocity after td seconds.
		Velocity += force * (td / Mass);
		Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, 1 << 8);
		
		RaycastHit hit;
		bool didHit = Physics.SphereCast(pos,0.5f,Velocity.normalized, out hit, (Velocity*td).magnitude, 1 << 8);
		if(colliders.Length > 0 || didHit)
		{
			float min = 0;
			if(didHit)
			{
				min = hit.distance;
			}
			foreach(Collider collider in colliders)
			{
				Vector3 point = collider.ClosestPoint(transform.position);
				float u = Vector3.Dot(Velocity, (point - transform.position).normalized);
				if(u < 0)
				{
					min = 0;
				}
				else
				{
					Velocity -= u * (point - transform.position).normalized;
				}
				pos += Velocity * td;
			}
			pos += Velocity.normalized * min;
		}
		else
		{
			pos += Velocity * td;
		}
		
		this.transform.position = pos;
	}

	// This method is called when the game is paused or played.
	public void onPlayPause(bool playing)
	{
		bool t = playing && Master.INSTANCE.Trail;
		TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
		trailRenderer.enabled = t;
		trailRenderer.Clear();
		bool f = !(playing || force == Vector3.zero);
		arrow.gameObject.SetActive(f);
		setArrow();
	}

	// This method is called when the trail variable is changed.
	public void onTrailChange(bool trail)
	{
		if(!Master.INSTANCE.isPlaying) return;
		TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
		trailRenderer.enabled = trail;
		trailRenderer.Clear();
	}

	public void resetForce()
	{
		force = new Vector3(0, -this.Mass * Constants.g, 0);
	}

	public void setForce(int magnitude, Vector3 angle)
	{
		float x = magnitude * Mathf.Cos(angle.x);
		float y = magnitude * Mathf.Cos(angle.y);
		float z = magnitude * Mathf.Cos(angle.z);

		force = new Vector3(x, y, z);
	}

	public void setArrow()
	{
		

		// Sets the rotation of the arrow to point where the force is going.
		arrow.transform.rotation = Quaternion.FromToRotation(transform.forward, force);;

		// This essentially sets the magnitude of the vector to the radius of the sphere.
		// This is the position of the arrow.
		arrow.transform.localPosition = force.normalized * 0.49f;
	}
}