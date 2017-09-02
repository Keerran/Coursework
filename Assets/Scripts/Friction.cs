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

	void Start()
	{
		UI.registerClass<Friction>();
	}

	public void applyForce(Base obj)
	{
		
	}
}
