using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;

    public BoxCollider2D bounds;

    public Vector3 _min, _max;

    public Vector2 smoothing;
    public Vector2 followMargin;

    public float boundsMargin = 0;
   
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

        x = Mathf.Lerp(x, player.position.x, smoothing.x * Time.deltaTime);
        y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);

        var cameraHalfwidth = Camera.main.orthographicSize * ((float) Screen.width / Screen.height);

        if (Mathf.Abs(x - player.position.x) > followMargin.x)
        {
            x = Mathf.Clamp(x, _min.x + cameraHalfwidth + boundsMargin, _max.x - cameraHalfwidth - boundsMargin);
        }
        if (Mathf.Abs(y - player.position.y) > followMargin.y)
        {
            y = Mathf.Clamp(y, _min.y + Camera.main.orthographicSize + boundsMargin, _max.y - Camera.main.orthographicSize - boundsMargin);
        }
        


        transform.position = new Vector3(x, y, transform.position.z);

    }
}
