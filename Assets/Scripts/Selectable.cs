using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
public class Selectable : MonoBehaviour
{
    [ShowValue(order=-1)]
	public Vector3 Position
	{
		get { return transform.position; }
		set { transform.position = value; }
	}
}
