using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class Base : MonoBehaviour
{
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
	void Start()
	{
		// Registers this class as one that should show up in the UI.
		UI.registerClass<Base>();
		//resetForce();
	}

	// Update is called once per frame
	void Update()
	{
		// Makes sure nothing happens if the game is paused.
		if(!Master.INSTANCE.isPlaying) return;
		// If the mass is not set, it will default to 0,
		// which will cause a divide by zero error.
		if(Mass == 0) return;
		
		// Scales the time between the last frame by the speed set by the user
		// to get the in-game time between the last frame.
		float td = Time.deltaTime * Master.INSTANCE.speed;
		// Uses a = F/m and a = dv/dt to get velocity after td seconds.
		Velocity = force * (td / Mass);
		/*vel.x += force.x * td / Mass;
		vel.y += force.y * td / Mass;
		vel.z += force.z * td / Mass;*/

		Vector3 pos = this.transform.position;
		pos += Velocity * td;
		/*pos.x += vel.x * td;
		pos.y += vel.y * td;
		pos.z += vel.z * td;
		
		/*RaycastHit hitInfo = new RaycastHit();
		bool move = Physics.Raycast(pos, offset.normalized, out hitInfo, offset.magnitude);
		if(move)
			pos = hitInfo.point + (this.transform.localScale/2);*/

		this.transform.position = pos;
	}

	public void resetForce()
	{
		force = new Vector3(0, 0, 0); //-this.mass * Constants.g, 0);
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
		Transform arrow = null;
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

		
		/*float xAngle = Vector3.Angle(Vector3.right, Vector3.Project(Vector3.right,force));
		float yAngle = Vector3.Angle(Vector3.up, Vector3.Project(Vector3.up,force));
		float zAngle = Vector3.Angle(Vector3.forward, Vector3.Project(Vector3.forward ,force));*/

		// Sets the rotation of the arrow to point where the force is going.
		arrow.transform.rotation = Quaternion.FromToRotation(transform.forward, force);;
		//arrow.transform.localEulerAngles = new Vector3(xAngle, yAngle, zAngle - 45);

		// This essentially sets the magnitude of the vector to the radius of the sphere.
		// This is the position of the arrow.
		arrow.transform.localPosition = force.normalized * 0.49f;
	}
}