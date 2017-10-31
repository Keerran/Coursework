using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class Base : MonoBehaviour
{
	[SerializeField]
	private Vector3 vel;

	[ShowValue(order=0)]
	public float Mass { get; set; }
	
	[SerializeField]
	private Vector3 force;
	
	[ShowValue(order=1)]
	public Vector3 Force
	{
		get { return force; }
		set { force = value; setArrow(); }
	}

	public Vector3 Velocity { get; set; }

	static Base()
	{
		UI.registerClass<Base>();		
	}
	
	// Use this for initialization
	void Start()
	{
		Master.INSTANCE.playPause += playPause;
		//resetForce();
	}

	// Update is called once per frame
	void Update()
	{
		if(!Master.INSTANCE.isPlaying) return;
		if(Mass == 0) return;
		
		float td = Time.deltaTime * Master.INSTANCE.speed;
		vel = force * (td / Mass);
		/*vel.x += force.x * td / Mass;
		vel.y += force.y * td / Mass;
		vel.z += force.z * td / Mass;*/

		Vector3 pos = this.transform.position;
		pos += vel * td;
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
		arrow.transform.rotation = Quaternion.FromToRotation(transform.forward, force);
		//arrow.transform.localEulerAngles = new Vector3(xAngle, yAngle, zAngle - 45);

		// This essentially sets the magnitude of the vector to the radius of the sphere.
		// This is the position of the arrow.
		arrow.transform.localPosition = force.normalized * 0.49f;
	}

	public void playPause(bool playing)
	{
		foreach(Transform t in transform)
		{
			if(t.name != "Sphere")
			{
				t.gameObject.SetActive(!playing);
			}
		}
	}
}