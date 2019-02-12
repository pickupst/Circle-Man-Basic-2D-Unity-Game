using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float Speed;
    public Vector2 Direction { get; private set; }
    public Vector2 InitialVelocity { get; private set; }
    public GameObject Owner;

    public void Initialize(GameObject owner, Vector2 direction, Vector2 initialVelocity)
    {
        Owner = owner;
        Direction = direction;
        InitialVelocity = initialVelocity;

    }

}
