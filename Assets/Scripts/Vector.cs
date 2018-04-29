using UnityEngine;

[System.Serializable]
public class Vector
{
    public float x, y, z;

    public Vector(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 toVector()
    {
        return new Vector3(x, y, z);
    }
}