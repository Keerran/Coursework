using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Arrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	// This holds which axis the arrow moves the object in.
	[SerializeField]
	private Axis axis;

	// This returns the color the arrow should be from the axis variable.
	public static Color getColor(Axis axis)
	{
		switch(axis)
		{
			case Axis.X:
				return Color.red;
			case Axis.Y:
				return Color.green;
			case Axis.Z:
				return Color.blue;
			default:
				return Color.grey;
		}
	}
	
	/// Use this for initialization
	void Start()
	{
		// Add the playPause handler of this class to the delegates.
		Master.INSTANCE.playPause += onPlayPause;
		// Set the color of the arrow depending on its axis.
		GetComponent<Renderer>().material.color = getColor(axis);
	}
	
	public void onPlayPause(bool isPlaying)
	{
		// Removes the arrows from the scene if the game
		// is not paused.
		this.gameObject.SetActive(!isPlaying);
	}

	/// Update is called once per frame
	void Update()
	{
		Renderer renderer = GetComponent<Renderer>();
		// This sets the arrow color back to what it should be after it has been pressed.
		if(renderer.material.color == Color.yellow && Input.GetMouseButtonUp(0))
			renderer.material.color = getColor(axis);
	}
	
	// An enumeration for all the types of arrow.
	public enum Axis
	{
		X, Y, Z, Force
	}

	// This function is implemented from the IPointerDownHandler, and
	// so this function will be called whenever the pointer is clicked on this object.
	public void OnPointerDown(PointerEventData eventData)
	{
		MouseDrag parent = this.transform.parent.GetComponentInChildren<MouseDrag>();
		if(parent.doMove) return;
		parent.doMove = true;
		// Sets the direction variable to the axis
		// dependant on the arrow type.
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
		// Sets the color of the arrow to yellow when it is clicked.
		GetComponent<Renderer>().material.color = Color.yellow;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
	}
}
