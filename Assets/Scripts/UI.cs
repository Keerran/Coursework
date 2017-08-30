using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	// This is a list of all the classes that should show up in the UI.
	private static List<Type> classes = new List<Type>();

	// This method adds the classes to the list.
	public static void registerClass<T>()
	{
		classes.Add(typeof(T));
	}
	
	// Use this for initialization
	void Start ()
	{
		Master master = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
		master.selectChange += selectChange;
	}

	public void selectChange(Base selected)
	{
		foreach(Transform child in transform) Destroy(child.gameObject);
		if(selected == null) return;
		foreach(Type clazz in classes)
		{
			var component = selected.GetComponent(clazz);
			if(component != null)
			{
				PropertyInfo[] values = ShowValue.getValues(clazz);
				for(int i = 0; i < values.Length; i++)
				{
					PropertyInfo value = values[i];
					
					drawValues(value, selected, -80);
				}
			}
		}
	}

	private void drawValues(PropertyInfo value, Base selected, int y)
	{
		Type t = value.PropertyType;
		Debug.Log(t);
		GameObject g = null;
		if(t == typeof(int))
		{
			g = (GameObject) Instantiate(Resources.Load("Number"), transform);
			g.GetComponentInChildren<InputField>().contentType = InputField.ContentType.IntegerNumber;
		}
		else if(t == typeof(float) || t == typeof(double))
		{
			g = (GameObject) Instantiate(Resources.Load("Number"), transform);
			g.GetComponentInChildren<InputField>().contentType = InputField.ContentType.DecimalNumber;
		}
		else if(t == typeof(bool))
		{
			
		}

		if(g == null) return;
		
		foreach(Transform tf in  g.transform)
		{
			if(tf.CompareTag("Tag"))
			{
				tf.GetComponent<Text>().text = value.Name;
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
