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
		InvokeRepeating("Motion", 0, 1);
	}
	
	// Update is called once per frame
	void Update()
	{
		float td = Time.deltaTime;
		vel.x += force.x * td / mass;
		vel.y += force.y * td / mass;
		vel.z += force.z * td / mass;

		Vector3 pos = this.transform.position;
		pos.x += vel.x * td;
		pos.y += vel.y * td;
		pos.z += vel.z * td;
		this.transform.position = pos;
	}
}
