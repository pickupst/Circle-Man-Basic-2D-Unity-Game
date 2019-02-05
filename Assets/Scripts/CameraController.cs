using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;

    public BoxCollider2D bounds;

    public Vector3 _min, _max;

    public float margin = 0;

    // Start is called before the first frame update
    void Start()
    {

        _min = bounds.bounds.min;
        _max = bounds.bounds.max;

    }

    // Update is called once per frame
    void Update()
    {

        var x = transform.position.x;
        var y = transform.position.y;

        x = player.position.x;
        y = player.position.y;

        var cameraHalfwidth = Camera.main.orthographicSize * ((float) Screen.width / Screen.height);

        x = Mathf.Clamp(x, _min.x + cameraHalfwidth + margin, _max.x - cameraHalfwidth - margin);
        y = Mathf.Clamp(y, _min.y + Camera.main.orthographicSize + margin, _max.y - Camera.main.orthographicSize - margin);


        transform.position = new Vector3(x, y, transform.position.z);

    }
}
