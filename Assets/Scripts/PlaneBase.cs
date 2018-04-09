using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBase : Selectable
{
	static PlaneBase()
	{
		UI.registerClass<PlaneBase>();
	}

	[ShowValue]
	public Vector3 Rotation
	{
		get { return transform.eulerAngles; }
		set { transform.eulerAngles = value; }
	}
}
