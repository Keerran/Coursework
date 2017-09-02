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
		Master.INSTANCE.selectChange += selectChange;

		foreach(Transform t in transform)
		{
			switch(t.name)
			{
					case "Selection":
						selection = t.gameObject;
						break;
					case "Time":
						time = t.gameObject;
						break;
			}
		}
	}

	public void selectChange(Base selected)
	{
		foreach(Transform child in selection.transform) Destroy(child.gameObject);
		if(selected == null) return;
		int y = 0;
		foreach(Type clazz in classes)
		{
			y += 10;
			var component = selected.GetComponent(clazz);
			if(component != null)
			{
				PropertyInfo[] values = ShowValue.getValues(clazz);
				foreach(PropertyInfo value in values)
				{
					y += 22;
					drawValues(value, selected.gameObject.GetComponent(clazz), y);
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
				if(val != null) value.SetValue(selected, Int32.Parse(val), null);
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
				if(val != null) value.SetValue(selected, float.Parse(val), null);
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
				if(val == null) return;
				Vector3 v = (Vector3) value.GetValue(selected, null) + new Vector3(float.Parse(val), 0, 0);
				value.SetValue(selected, v, null);
			});
			
			y.onValueChanged.AddListener(val =>
			{
				if(val == null ) return;
				Vector3 v = (Vector3) value.GetValue(selected, null) + new Vector3(0, float.Parse(val), 0);
				value.SetValue(selected, v, null);
			});
			
			z.onValueChanged.AddListener(val =>
			{
				if(val == null) return;
				Vector3 v = (Vector3) value.GetValue(selected, null) + new Vector3(0, 0, float.Parse(val));
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

	// Update is called once per frame
	void Update ()
	{
		
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