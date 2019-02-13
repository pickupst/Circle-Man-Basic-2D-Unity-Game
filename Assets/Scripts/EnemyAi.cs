using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float speed = 9f;


    private CharacterController _controller;
    private Vector2 _direction;


    // Start is called before the first frame update
    void Start()
    {

        _controller = GetComponent<CharacterController>();
        _direction = new Vector2(-1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        _controller.SetHorizontalForce(_direction.x * speed);

        if ((_direction.x <= 0 && _controller.State.IsCollidingLeft) || (_direction.x >= 0 && _controller.State.IsCollidingRight))
        {
            _direction = -_direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
