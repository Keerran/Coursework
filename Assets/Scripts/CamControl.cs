using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControl : MonoBehaviour
{
	public float speed;
	public float sensitivity;
	public float panSensitivity;
	public float wheelSpeed;

	// stores the last mouse position;
	private Vector3? lastMouse;
	
	// Use this for initialization
	void Start ()
	{
		lastMouse = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButton(1))
		{
			Cursor.visible = false;
			if(lastMouse == null)
			{
				lastMouse = Input.mousePosition;
			}
			Vector3 deltaMouse = Input.mousePosition - (Vector3) lastMouse;
			deltaMouse *= sensitivity;
			float x = transform.eulerAngles.x - deltaMouse.y;
			float y = transform.eulerAngles.y + deltaMouse.x;
			if(x < 90) x = 90f;
			if(x > 270) x = 270f;
			/*if(x < 270 && x > 90)
			{
				if(Mathf.Abs(x - 270) > Mathf.Abs(x - 90))
				{
					x = 90f;
				}
				else
				{
					x = 270f;
				}
			}*/
			deltaMouse = new Vector3(x, y, 0);
			transform.eulerAngles = deltaMouse;
			lastMouse = Input.mousePosition;
		}
		else if(Input.GetMouseButton(2))
		{
			Cursor.visible = false;
			
			if(lastMouse == null)
			{
				lastMouse = Input.mousePosition;
			}
			Vector3 deltaMouse = Input.mousePosition - (Vector3) lastMouse;
			deltaMouse *= speed * panSensitivity;
			deltaMouse *= -1;

			transform.Translate(deltaMouse);

			lastMouse = Input.mousePosition;
			return;
		}
		
		if(Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
		{
			lastMouse = null;
			Cursor.visible = true;
		}
				
		Vector3 offset = new Vector3(0, 0, 0);

		offset.z += Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;

		if(Input.GetKey(KeyCode.W))
		{
			offset += Vector3.forward;
		}
		
		if(Input.GetKey(KeyCode.S))
		{
			offset += Vector3.back;
		}
		
		if(Input.GetKey(KeyCode.A))
		{
			offset += Vector3.left;
		}
		
		if(Input.GetKey(KeyCode.D))
		{
			offset += Vector3.right;
		}

		offset *= speed;

		this.transform.Translate(offset);
	}
}
