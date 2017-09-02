using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
[System.Obsolete("", true)]
public class MasterNoEvent : MonoBehaviour
{
	private Base selected;
	
	public Base Selected
	{
		get
		{
			return selected;
		}
		private set
		{
			if(value == selected) return;
			if(value != null)
				value.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
			if(selected != null)
				selected.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
			selected = value;
		}
	}

	public delegate void onSelectChange(Base selected);

	public event onSelectChange selectChange;

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		// Gets a ray to whatever the mouse is pointing to.
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit info;
		// If the ray hits anything...
		if(Physics.Raycast(ray, out info))
		{
			// get the arrow script...
			Arrow arrow = info.transform.GetComponent<Arrow>();
			// and if the LMB is pressed...
			if(Input.GetMouseButton(0))
			{
				// and the object had an arrow script...
				if(arrow != null)
				{
					// then call the onClicked method of that arrow.
					arrow.onClicked();
				}
				else
				{
					if(Selected == info.transform.GetComponent<Base>())
					{
						if(!Input.GetMouseButtonDown(0)) return;
						// Otherwise if the script isn't there, then check for a MouseDrag script.
						MouseDrag mouseDrag = info.transform.GetComponent<MouseDrag>();
						if(mouseDrag != null && !mouseDrag.doMove)
						{
							// Sets the mode of the MouseDrag to drag anywhere on the xz plane.
							mouseDrag.doMove = true;
							mouseDrag.dir = null;
						}
					}
					else
					{
						// Checks if the layer of the object is 5, which is the UI layer.
						Debug.Log(info.transform);
						if(info.transform.gameObject.layer == 5) return;
						// Change the selected variable to the object that was clicked on.
						this.Selected = info.transform.GetComponent<Base>();
						// Fire the selectChange event.
						if(selectChange != null) selectChange(Selected);
					}
				}
			}
		}
	}
}*/