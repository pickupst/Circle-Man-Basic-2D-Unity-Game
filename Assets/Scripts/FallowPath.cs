using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowPath : MonoBehaviour
{
    public PathDefinition path;
    public float speed = 5f;
    public float maxDistanceToGoal = 0.1f;


    IEnumerator<Transform> _currentPoint;
    float maxDistanceToGoalSQR;

    // Start is called before the first frame update
    void Start()
    {
        if (path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        _currentPoint = path.GetPathEnumerator();
        _currentPoint.MoveNext();

        transform.position = _currentPoint.Current.position;

        maxDistanceToGoalSQR = maxDistanceToGoal * maxDistanceToGoal;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentPoint == null || _currentPoint.Current == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, Time.deltaTime * speed);

        var distanceSQR = (transform.position - _currentPoint.Current.position).sqrMagnitude;
        if (distanceSQR < maxDistanceToGoalSQR)
        {
            _currentPoint.MoveNext();
        }
    }
}
