using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform[] backgrounds;

    private Vector3 _lastPosition;

    public float parallaxScale = 0.5f;
    public float smoothing = 2f;
    public float parallaxRedFac = 3f;
    // Start is called before the first frame update
    void Start()
    {
        _lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        var parallax = (_lastPosition.x - transform.position.x) * parallaxScale;

        for (int i = 0; i < backgrounds.Length; i++)
        {

            var backgroundTargetPosition = backgrounds[i].position.x + parallax * (i * parallaxRedFac + 1);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, 
                                                   new Vector3(backgroundTargetPosition,backgrounds[i].position.y, backgrounds[i].position.z),
                                                   Time.deltaTime * smoothing);

        } 

        _lastPosition = transform.position;
    }
}
