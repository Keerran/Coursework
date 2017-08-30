using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
	private float coefficient;
	
	//Coefficient of Friction
	[SerializeField]
	public float Coefficient {
		get { return coefficient; }
		set { coefficient = Mathf.Clamp(value, 0, 1); }
	}

	public void applyForce(Base obj)
	{
		
	}
}
