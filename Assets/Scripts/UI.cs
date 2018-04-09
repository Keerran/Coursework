using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public static bool UIHover;

	private GameObject selection;
	private GameObject time;
	private GameObject restitution;
	
	// This is a list of all the classes that should show up in the UI.
	private static List<Type> classes = new List<Type>();

	public GUISkin skin;

	// This method adds the classes to the list.
	public static void registerClass<T>()
	{
		classes.Add(typeof(T));
	}

	void OnGUI()
	{
		GUI.skin = skin;
	}
	
	// Use this for initialization
	void Start ()
	{
		//Master.INSTANCE.selectChange += selectChange;

		foreach(Transform t in transform)
		{
			switch(t.name)
			{
					case "Selection":
						selection = t.gameObject;
						selection.GetComponent<Image>().enabled = false;
						break;
					case "Time":
						time = t.gameObject;
						break;
			}
		}

		foreach(Transform t in time.transform)
		{
			switch(t.name)
			{
				case "Play":
					t.GetComponent<Button>().onClick.AddListener(() => { onPlayClick(t); });
					break;
				case "Speed":
					InputField input = t.GetComponentInChildren<InputField>();
					input.onValueChanged.AddListener(value =>
					{
						Master.INSTANCE.speed = float.Parse(value);
					});
					input.onEndEdit.AddListener(value =>
					{
						input.text = Master.INSTANCE.speed.ToString();
					});
					input.text = "1.0";
					break;
				case "Restitution":
					InputField i = t.GetComponentInChildren<InputField>();
					i.onValueChanged.AddListener(value =>
					{
						Col.E = float.Parse(value);
					});
					i.onEndEdit.AddListener(value =>
					{
						i.text = Col.E.ToString();
					});
					i.text = "1.0";
					break;
				case "Add Ball":
					Button button = t.GetComponent<Button>();
					button.onClick.AddListener(() =>
					{
						GameObject g = (GameObject)Instantiate(Resources.Load("Object"));
						RaycastHit info;
						Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height) / 2);
						if(Physics.Raycast(ray, out info, 10))
						{
							g.transform.position = info.point + info.normal.normalized * 0.5f;
						}
						else
						{
							g.transform.position = ray.origin + ray.direction * 2;
						}
						if(Master.INSTANCE.isPlaying)
						{
							foreach(Transform a in g.transform)
							{
								Arrow arrow = a.GetComponent<Arrow>();
								if(arrow != null)
								{
									arrow.onPlayPause(false);
								}
							}
						}
					});
					break;
				case "Add Plane":
					Button b = t.GetComponent<Button>();
					b.onClick.AddListener(() =>
					{
						Instantiate(Resources.Load("Plane"));
					});
					break;
				case "Trail":
					Toggle toggle = t.GetComponent<Toggle>();
					toggle.onValueChanged.AddListener(value =>
					{
						Master.INSTANCE.Trail = value;
					});
					break;
			}
		}
	}

	public void onPlayClick(Transform t)
	{
		Master.INSTANCE.isPlaying = !Master.INSTANCE.isPlaying;
		if(Master.INSTANCE.isPlaying)
		{
			t.GetComponentInChildren<Text>().text = "Pause";
		}
		else
		{
			t.GetComponentInChildren<Text>().text = "Play";
		}
		
	}

	void Update()
	{
		if(UI.UIHover) return;
		Selectable selected = Master.INSTANCE.Selected;
		// Resets the selection UI.
		foreach(Transform child in selection.transform) Destroy(child.gameObject);
		// If nothing is selected, then get turn off the selection pane and return.
		if(selected == null)
		{
			selection.GetComponent<Image>().enabled = false;
			return;
		}
		// Makes sure the selection pane is turned on if something is selected.
		selection.GetComponent<Image>().enabled = true;
		// This is the y coordinate of each component, it increases by 10 between each class
		// and 22 between each component in the class.
		int y = 0;
		// Loops through each class registered for the UI.
		foreach(Type clazz in classes)
		{
			y += 10;
			// Gets the instance of the current class in the selected object.
			var component = selected.GetComponent(clazz);
			if(component == null)
				component = selected.GetComponentInChildren(clazz);
			// If the selected object actually has that class.
			if(component != null)
			{
				// Gets each property with the show value in that class.
				PropertyInfo[] values = ShowValue.getValues(clazz);
				foreach(PropertyInfo value in values)
				{
					y += 22;
					drawValues(value, component, y);
				}
			}			
		}
	}

	private void drawValues(PropertyInfo value, object selected, int yPos)
	{
		Type t = value.PropertyType;
		GameObject g = null;
		if(t == typeof(int))
		{
			g = (GameObject) Instantiate(Resources.Load("Number"), selection.transform);
			InputField input = g.GetComponentInChildren<InputField>();
			input.contentType = InputField.ContentType.IntegerNumber;
			input.onValueChanged.AddListener(val =>
			{
				int x;
				if(val != null && Int32.TryParse(val, out x)) value.SetValue(selected, x, null);
			});
			input.onEndEdit.AddListener(val =>
			{
				input.text = value.GetValue(selected, null).ToString();
			});
			input.text = value.GetValue(selected, null).ToString();
		}
		else if(t == typeof(float))
		{
			g = (GameObject) Instantiate(Resources.Load("Number"), selection.transform);
			InputField input = g.GetComponentInChildren<InputField>();
			input.contentType = InputField.ContentType.DecimalNumber;
			input.onValueChanged.AddListener(val =>
			{
				float x;
				if(val != null && float.TryParse(val, out x)) value.SetValue(selected, x, null);
			});
			input.onEndEdit.AddListener(val =>
			{
				input.text = value.GetValue(selected, null).ToString();
			});
			input.text = value.GetValue(selected, null).ToString();
		}
		else if(t == typeof(Vector3))
		{
			g = (GameObject) Instantiate(Resources.Load("Vector3"), selection.transform);
			InputField x,y,z;
			x = y = z = null;
			foreach(Transform transform in g.transform)
			{
				switch(transform.name)
				{
					case "X":
						x = transform.GetComponent<InputField>();
						break;
					case "Y":
						y = transform.GetComponent<InputField>();
						break;
					case "Z":
						z = transform.GetComponent<InputField>();
						break;
				}
			}
			
			if(x == null || y == null || z == null) return;
			
			x.onValueChanged.AddListener(val =>
			{
				float x1;
				if(val == null || !float.TryParse(val, out x1)) return;
				Vector3 v = (Vector3) value.GetValue(selected, null);
				v.x = x1;
				value.SetValue(selected, v, null);
			});
			
			y.onValueChanged.AddListener(val =>
			{
				float y1;
				if(val == null || !float.TryParse(val, out y1)) return;
				Vector3 v = (Vector3) value.GetValue(selected, null);
				v.y = y1;
				value.SetValue(selected, v, null);
			});
			
			z.onValueChanged.AddListener(val =>
			{
				float z1;
				if(val == null || !float.TryParse(val, out z1)) return;
				Vector3 v = (Vector3) value.GetValue(selected, null);
				v.z = z1;
				value.SetValue(selected, v, null);
			});

			x.text = ((Vector3) value.GetValue(selected, null)).x.ToString();
			y.text = ((Vector3) value.GetValue(selected, null)).y.ToString();
			z.text = ((Vector3) value.GetValue(selected, null)).z.ToString();
		}
		else if(t == typeof(bool))
		{
			g = (GameObject) Instantiate(Resources.Load("Bool"));
			Toggle input = g.GetComponentInChildren<Toggle>();
			input.onValueChanged.AddListener(val =>
			{
				value.SetValue(selected, val, null);
			});
			input.isOn = (bool) value.GetValue(selected, null);
		}

		if(g == null) return;
		
		foreach(Transform tf in  g.transform)
		{
			if(tf.CompareTag("Tag"))
			{
				tf.GetComponent<Text>().text = value.Name;
			}
		}

		g.transform.position += new Vector3(0, -yPos, 0);
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		UIHover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIHover = false;
	}
}
