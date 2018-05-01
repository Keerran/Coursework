using UnityEngine;

// Makes Unity able to save instances of this class.
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