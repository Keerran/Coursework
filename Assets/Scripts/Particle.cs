using UnityEngine;

[System.Serializable]
public class Particle : BaseObj
{
    public float mass;
    public Vector force;
    public Vector velocity;
    public float coefficient;


    public Particle(Vector position, float mass, Vector force, Vector velocity, float coefficient) : base(position)
    {
        this.mass = mass;
        this.force = force;
        this.velocity = velocity;
        this.coefficient = coefficient;
    }

    public Particle(Vector3 position, float mass, Vector3 force,
                    Vector3 velocity, float coefficient) : base(position)
    {
        this.mass = mass;
        this.force = new Vector(force);
        this.velocity = new Vector(velocity);
        this.coefficient = coefficient;
    }
}