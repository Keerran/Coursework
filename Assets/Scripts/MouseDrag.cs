using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
 {
	/// The direction to move in.
	public Vector3? dir;
	/// Should the object move to the mouse position.
	public bool doMove;
	
	/// Use this for initialization
	void Start()
	{
		
	}
	
	/// Update is called once per frame
	void Update()
	{
		if(doMove)
		{
			if(!Input.GetMouseButton(0))
			{
				// Stops moving is the mouse button is no longer held.
				doMove = false;
			}
			
			Plane plane;
			if(dir == null)
			{
				// If dir is null, then just default to the xz plane.
				plane = new Plane(Vector3.up, this.transform.position);
			}
			else
			{
				// Otherwise create a plane parallel to the direction of movement.
				plane = new Plane(Vector3.RotateTowards((Vector3) dir, Vector3.down, (float) Math.PI / 2, 1), this.transform.position);
			}
			// Creates a ray from where the mouse if pointing to.
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hit;
			// Finds where the mouse points to on the plane.
			if(plane.Raycast(ray, out hit))
			{
				// Sets the objects position to said point.
				Vector3 point = ray.GetPoint(hit);
				// posDir is used to scale to the direction, which can then be used to find the distance
				//  between the object and the desired position. 
				Vector3 posDir = this.transform.position;
				if(dir != null)
				{
					// Scales that point so that the object only moves in the required direction.
					point.Scale((Vector3) dir);
					posDir.Scale((Vector3) dir);

					// Offsets the position to account for the position of the arrow.
					posDir += (Vector3) dir;
				}

				// Sets the position.
				this.transform.position += point - posDir;
			}
		}
	}
	 
	 public void OnBeginDrag(PointerEventData eventData)
	 {
		 // Otherwise if the script isn't there, then check for a MouseDrag script.
		 MouseDrag mouseDrag = eventData.pointerDrag.transform.GetComponent<MouseDrag>();
		 if(mouseDrag != null && !mouseDrag.doMove)
		 {
			 // Sets the mode of the MouseDrag to drag anywhere on the xz plane.
			 mouseDrag.doMove = true;
			 mouseDrag.dir = null;
		 }
	 }

	 public void OnDrag(PointerEventData eventData)
	 {
		 
	 }
 }
 