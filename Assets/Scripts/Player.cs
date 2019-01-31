using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 8f;

    CharacterController _controller;

    int _normalHorizontalSpeed;

    bool _isFacingRight;


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _isFacingRight = transform.localScale.x > 0;
    }

    // Update is called once per frame
    void Update()
    {

        HandleInput();
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalHorizontalSpeed * speed, Time.deltaTime));

    }

    private void HandleInput()
    {

        if (Input.GetKey(KeyCode.D))
        {
            _normalHorizontalSpeed = 1;
            if (!_isFacingRight)
            {
                Flip();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        { 
            _normalHorizontalSpeed = -1;
            if (_isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            _normalHorizontalSpeed = 0;
        }
    }

    private void Flip()
    {

        transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;

    }
}
