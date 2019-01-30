using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDefinition : MonoBehaviour
{
    public Transform[] points;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (points == null && points.Length < 2)
        {
            return;
        }

        for (int i = 1; i < points.Length; i++)
        {
            if (points[i - 1] != null)
            {

                Gizmos.DrawLine(points[i - 1].position, points[i].position);

            }
        }
    }

    public IEnumerator<Transform> GetPathEnumerator()
    {

        var index = 0;
        var direction = 1;

        while (true)
        {
            yield return points[index];
            if (index <= 0)
            {
                direction = 1;
            }
            else if (index >= points.Length - 1)
            {
                direction = -1;
            }
            index += direction;
        }
    }
}
