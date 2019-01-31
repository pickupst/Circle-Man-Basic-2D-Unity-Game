using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public LayerMask PlatformMask;
    public Vector2 Velocity { get { return _velocity; } }
    public ControllerState2D State { get; private set; }

    BoxCollider2D _boxCollider2D;
    Vector2 _velocity;
    
    Vector3 _localScale;
    Vector3 _raycastBottomRight;
    Vector3 _raycastBottomLeft;
    Vector3 _raycastTopLeft;

    Transform _transform;

    const float SkinWidth = 0.02f;
    const int TotalHorizontalRay = 8;

    float _verticalDistanceBetweenRays;
    float _horizontalDistanceBetweenRays;

    private void Awake()
    {
        State = new ControllerState2D();

        _boxCollider2D = GetComponent<BoxCollider2D>();
        _localScale = transform.localScale;
        _transform = transform;

        var colliderHight = (_boxCollider2D.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth));
        _horizontalDistanceBetweenRays = colliderHight / (TotalHorizontalRay - 1);

        var colliderWidth = (_boxCollider2D.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth));
        _verticalDistanceBetweenRays = colliderWidth / (TotalHorizontalRay - 1);
    }

    public void LateUpdate()
    {
        _velocity.y += -25f * Time.deltaTime;

        Move(Velocity * Time.deltaTime);
    }

    public void Jump()
    {
        AddForce(new Vector2(0, 16f));
    }

    public void AddForce(Vector2 vector2)
    {
        _velocity += vector2;
    }

    private void Move(Vector2 deltaMovement)
    {
        CalculateRayOrigins();

        MoveHorizontaly(ref deltaMovement);

        MoveVerticaly(ref deltaMovement);

        _transform.Translate(deltaMovement, Space.World);

        if (Time.deltaTime > 0)
        {
            _velocity = deltaMovement / Time.deltaTime;
        }

    }

    private void MoveVerticaly(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;
        rayOrigin.x += deltaMovement.x;

        for (int i = 0; i < TotalHorizontalRay; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * _verticalDistanceBetweenRays), rayOrigin.y);
            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);


            if (!raycastHit)
            {
                continue;
            }

            deltaMovement.y = raycastHit.point.y - rayVector.y;
            if (isGoingUp)
            {
                State.IsCollidingAbove = true;
                deltaMovement.y -= SkinWidth;
            }
            else
            {
                State.IsCollidingBelow = true;
                deltaMovement.y += SkinWidth;
            }

        }

    }

    private void MoveHorizontaly(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;

        Vector3 rayOrigin;
        if (isGoingRight)
            rayOrigin = _raycastBottomRight;
        else
            rayOrigin = _raycastBottomLeft;



        for (int i = 0; i < TotalHorizontalRay; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + i * _horizontalDistanceBetweenRays);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);

            if (!raycastHit)
            {
                continue;
            }

            deltaMovement.x = raycastHit.point.x - rayVector.x;

            if (isGoingRight)
            {
                State.IsCollidingRight = true;
                deltaMovement.x -= SkinWidth;
            }
            else
            {
                State.IsCollidingLeft = true;
                deltaMovement.x += SkinWidth;
            }
           

        }
    }

    private void CalculateRayOrigins()
    {

        var size = new Vector2(_boxCollider2D.size.x * Mathf.Abs(_localScale.x), 
            _boxCollider2D.size.y * Mathf.Abs(_localScale.y)) / 2;

        var center = new Vector2(_boxCollider2D.offset.x * _localScale.x, 
            _boxCollider2D.offset.y * _localScale.y);

        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth,
                                                                center.y - size.y + SkinWidth);

        _raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth,
                                                               center.y - size.y + SkinWidth);

        _raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth,
                                                            center.y + size.y - SkinWidth);

    }

    public void SetHorizontalForce(float _normalHorizontalSpeed)
    {

        _velocity.x = _normalHorizontalSpeed;

    }
}
