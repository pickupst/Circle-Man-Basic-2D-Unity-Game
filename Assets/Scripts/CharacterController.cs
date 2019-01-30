using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public LayerMask PlatformMask;
    public Vector2 Velocity { get { return _velocity; } }

    BoxCollider2D _boxCollider2D;
    Vector2 _velocity;
    
    Vector3 _localScale;
    Vector3 _raycastBottomRight;

    Transform _transform;

    const float SkinWidth = 0.02f; 

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _localScale = transform.localScale;
        _transform = transform;
    }

    public void LateUpdate()
    {
        Move(Velocity * Time.deltaTime);
    }

    private void Move(Vector2 deltaMovement)
    {
        CalculateRayOrigins();

        MoveHorizontaly(ref deltaMovement);

        _transform.Translate(deltaMovement, Space.World);

    }

    private void MoveHorizontaly(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = _raycastBottomRight;
        var rayVector = rayOrigin;

        var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);

        if (!raycastHit)
        {
            return;
        }

        //Destroy(raycastHit.transform.gameObject);

        deltaMovement.x = raycastHit.point.x - rayVector.x;
        deltaMovement.x -= SkinWidth;
    }

    private void CalculateRayOrigins()
    {

        var size = new Vector2(_boxCollider2D.size.x * Mathf.Abs(_localScale.x), 
            _boxCollider2D.size.y * Mathf.Abs(_localScale.y)) / 2;

        var center = new Vector2(_boxCollider2D.offset.x * _localScale.x, 
            _boxCollider2D.offset.y * _localScale.y);

        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth,
                                                                center.y - size.y + SkinWidth);


    }

    public void SetHorizontalForce(float _normalHorizontalSpeed)
    {

        _velocity.x = _normalHorizontalSpeed;

    }
}
