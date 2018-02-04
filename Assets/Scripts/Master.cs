using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class Master : MonoBehaviour
{
	// This is the only instance of master.
	public static Master INSTANCE { get; private set; }

	private bool playing;

	public bool isPlaying
	{
		get { return playing; }
		set { playing = value; playPause(playing); }
	}
	public float speed = 1;
	
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
				value.transform.FindChild("Sphere").GetComponent<Renderer>().material.shader = Shader.Find("Custom/Outline");
			if(selected != null)
				selected.transform.FindChild("Sphere").GetComponent<Renderer>().material.shader = Shader.Find("Standard");
			selected = value;
			// Fire the selectChange event.
			if(selectChange != null) selectChange(this.Selected);
		}
	}

	public delegate void onSelectChange(Base selected);
	public delegate void onPlayPause(bool playing);

	public event onSelectChange selectChange;
	public event onPlayPause playPause;
	
	// Use this for initialization
	void Awake()
	{
		INSTANCE = this;
	}
	
	// Update is called once per frame
	void Update()
	{
		// Makes sure that if you click on something in the UI,
		// then it won't try to select anything.
		if(UI.UIHover) return;
		// Gets a ray to whatever the mouse is pointing to.
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit info;
		// If the ray hits anything...
		if(Physics.Raycast(ray, out info) && Input.GetMouseButtonDown(0))
		{
			if(info.transform.name != "Sphere") return;
			if(info.transform.parent == null || info.transform.parent.GetComponent<Base>() == null)
			{
				this.Selected = null;
			}
			// Change the selected variable to the object that was clicked on.
			this.Selected = info.transform.parent.GetComponent<Base>();
		}
		else if(Input.GetMouseButtonDown(0))
		{
			this.Selected = null;
		}
	}
}
