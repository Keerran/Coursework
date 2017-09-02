using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class Master : MonoBehaviour
{
	public static Master INSTANCE { get; private set; }
	
	private Base selected;
	
	public Base Selected
	{
		get
		{
			return selected;
		}
		set
		{
			if(value == selected) return;
			if(value != null)
				value.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
			if(selected != null)
				selected.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
			selected = value;
			// Fire the selectChange event.
			if(selectChange != null) selectChange(this.Selected);
		}
	}

	public delegate void onSelectChange(Base selected);

	public event onSelectChange selectChange;

	// Use this for initialization
	void Awake()
	{
		INSTANCE = this;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(UI.UIHover) return;
		// Gets a ray to whatever the mouse is pointing to.
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit info;
		// If the ray hits anything...
		if(Physics.Raycast(ray, out info) && Input.GetMouseButtonDown(0))
		{
			if(!info.transform.GetComponent<Base>())
			{
				this.Selected = null;
			}
			// Change the selected variable to the object that was clicked on.
			this.Selected = info.transform.GetComponent<Base>();
		}
		else if(Input.GetMouseButtonDown(0))
		{
			this.Selected = null;
		}
	}
}