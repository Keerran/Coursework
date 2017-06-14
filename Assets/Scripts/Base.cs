using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Base : MonoBehaviour
{
	[SerializeField]
	private Vector3 vel;

	[SerializeField]
	private int mass;

	[SerializeField]
	private Vector3 force;

	// Use this for initialization
	void Start()
	{
		resetForce();
	}
	
	// Update is called once per frame
	void Update()
	{
		float td = Time.deltaTime;
		vel.x += force.x * td / mass;
		vel.y += force.y * td / mass;
		vel.z += force.z * td / mass;

		Vector3 pos = this.transform.position;
		Vector3 offset = vel * td;
		/*pos.x += vel.x * td;
		pos.y += vel.y * td;
		pos.z += vel.z * td;*/
		RaycastHit hitInfo = new RaycastHit();
		Physics.Raycast(pos, offset.normalized,offset.magnitude);

		this.transform.position = pos;
	}

	public void resetForce()
	{
		force = new Vector3(0, -this.mass * Constants.g, 0);
	}

	public void addForce(Vector3 force)
	{
		/*
		float xAngle = Vector3.Angle(Vector3.right, force);
		print(xAngle);

		float yAngle = Vector3.Angle(Vector3.up, force);
		print(yAngle);

		float zAngle = Vector3.Angle(Vector3.forward, force);
		print(zAngle);*/
		this.force += force;
	}

	public void addForce(int magnitude, Vector3 angle)
	{
		float x = magnitude * Mathf.Cos(angle.x);
		float y = magnitude * Mathf.Cos(angle.y);
		float z = magnitude * Mathf.Cos(angle.z);

		this.force += new Vector3(x, y, z);
	}
}