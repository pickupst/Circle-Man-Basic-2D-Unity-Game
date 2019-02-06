﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 8f;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;

    public bool IsDead { get; private set; }

    CharacterController _controller;

    int _normalHorizontalSpeed;

    bool _isFacingRight;


    // Start is called before the first frame update
    void Start()
    {
        IsDead = false;
        _controller = GetComponent<CharacterController>();
        _isFacingRight = transform.localScale.x > 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!IsDead)
        {
            HandleInput();
        }

        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

        if (IsDead)
        {
            _controller.SetHorizontalForce(0);
        }
        else
        {
            _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalHorizontalSpeed * speed, Time.deltaTime * movementFactor));
        }


    }

    public void Kill()
    {

        _controller.HandleCollisions = false;
        GetComponent<Collider2D>().enabled = false;

        IsDead = true;

        _controller.SetForce(new Vector2(0, 10f));


    }

    public void RespawnAt(Transform spawnPoint)
    {

        if (!_isFacingRight)
        {
            Flip();
        }

        GetComponent<Collider2D>().enabled = true;
        _controller.HandleCollisions = true;
        IsDead = false;

        transform.position = spawnPoint.position;

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

        if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump();
        }
    }

    private void Flip()
    {

        transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;

    }
}


