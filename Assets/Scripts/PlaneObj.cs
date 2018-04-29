using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaneObj : BaseObj
{
    public Vector rotation;

    public PlaneObj(Vector position, Vector rotation) : base(position)
    {
        this.rotation = rotation;
    }

    public PlaneObj(Vector3 position, Vector3 rotation) : base(position)
    {
        this.rotation = new Vector(rotation);
    }
}
