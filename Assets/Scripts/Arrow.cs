using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
	[SerializeField]
	private Axis axis;
	
	/// Use this for initialization
	void Start()
	{
		Color color;
		switch(axis)
		{
			case Axis.X:
				color = Color.red;
				break;
			case Axis.Y:
				color = Color.green;
				break;
			case Axis.Z:
				color = Color.blue;
				break;
			default:
				color = Color.grey;
				break;
		}
		// Set the color of the arrow depending on its axis.
		GetComponent<Renderer>().material.color = color;
	}
	
	/// Update is called once per frame
	void Update()
	{
		
	}

	public void onClicked()
	{
		MouseDrag parent = GetComponentInParent<MouseDrag>();
		if(parent.doMove) return;
		parent.doMove = true;
		switch(axis)
		{
			case Axis.X:
				parent.dir = Vector3.right;
				break;
			case Axis.Y:
				parent.dir = Vector3.up;
				break;
			case Axis.Z:
				parent.dir = Vector3.forward;
				break;
		}
	}

	public enum Axis
	{
		X, Y, Z
	}
}
