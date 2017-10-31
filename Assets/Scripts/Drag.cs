using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
	private Vector3 dragForce;

	private float constant = 0.47f;
	private float density = 1.225f;
	private Vector3 wind = new Vector3(2, 2, 2);
	
	[ShowValue] public float Constant
	{
		get { return constant; } 
		set { constant = value; recalculateForce(); }
	}
	[ShowValue] public float Density
	{
		get { return density; }
		set { density = value; recalculateForce(); }
	}
	[ShowValue] public Vector3 Wind
	{
		get { return wind; }
		set { wind = value; recalculateForce(); }
	}

	static Drag()
	{
		UI.registerClass<Drag>();
	}
	
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void recalculateForce()
	{
		Base b = GetComponent<Base>();
		b.Force -= dragForce;
		dragForce = 0.5f * Density * (b.Velocity - Wind) * Constant * Mathf.PI * transform.localScale.x * 2;
		b.Force += dragForce;
	}
}
