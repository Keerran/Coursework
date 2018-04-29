using UnityEngine;

[System.Serializable]
public class BaseObj
{
    public Vector position;

    public BaseObj(Vector position)
    {
        this.position = position;
    }

    public BaseObj(Vector3 position)
    {
        this.position = new Vector(position);
    }
}