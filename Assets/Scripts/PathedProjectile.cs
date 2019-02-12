using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathedProjectile : MonoBehaviour
{
    private Transform _destination;
    private float _speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);

        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;

        if (distanceSquared > 0.1f * 0.1f)
        {
            return;
        }

        Destroy(gameObject);
    }

    internal void Initialize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }
}
