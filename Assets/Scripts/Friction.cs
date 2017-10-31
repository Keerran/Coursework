using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
	private float coefficient;
	
	//Coefficient of Friction
	[SerializeField]
	[ShowValue]
	public float Coefficient {
		get { return coefficient; }
		set { coefficient = Mathf.Clamp(value, 0f, 1f); }
	}

	static Friction()
	{
		UI.registerClass<Friction>();
	}

	void Start()
	{
		
	}

	public void applyForce(Base obj)
	{
		
	}
}
