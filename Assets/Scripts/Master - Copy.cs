/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

	private float e;
	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		if(!Master.INSTANCE.isPlaying) return;
		Base b1 = GetComponentInParent<Base>();
		Base b2 = other.gameObject.GetComponentInParent<Base>();
		if(b2 == null)
		{
			if(other.gameObject.name == "Plane")
			{
				// Do plane stuff.
				Vector3 x = (other.ClosestPoint(b1.transform.position) - b1.transform.position).normalized;
				float u = Vector3.Dot(b1.Velocity, x);
				float v = e * -u;
				b1.Velocity = b1.Velocity + (v - u) * x;
			}
		}
		else
		{
			float m1 = b1.Mass;
			float m2 = b2.Mass;

			Vector3 x = (b2.transform.position - b1.transform.position).normalized;

			float u1 = Vector3.Dot(b1.Velocity, x);
			float u2 = Vector3.Dot(b2.Velocity, x);

			float v1 = ((m1 - m2 * e) * u1 + m2 * (1 + e) * u2) / (m1 + m2);

			b1.Velocity = b1.Velocity + (v1 - u1) * x;
		}
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
}

*/