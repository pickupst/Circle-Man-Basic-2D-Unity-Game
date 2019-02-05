using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public LayerMask PlatformMask;
    public Vector2 Velocity { get { return _velocity; } }
    public ControllerState2D State { get; private set; }
    public ControllerParamaters DefaultParameters;
    public ControllerParamaters Parameters { get { return _OverrideParameters ?? DefaultParameters; } }
    public GameObject StandingOn { get; private set; }

    public bool HandleCollisions { get; set; }

    private GameObject _lastStandingOn;

    BoxCollider2D _boxCollider2D;
    Vector2 _velocity;
    
    Vector3 _localScale;
    Vector3 _raycastBottomRight;
    Vector3 _raycastBottomLeft;
    Vector3 _raycastTopLeft;
    Vector3 _activeGlobalPlatformPoint;
    Vector3 _activeLocalPlatformPoint;

    Transform _transform;
    ControllerParamaters _OverrideParameters;

    private static readonly float SlopeLimitTan = Mathf.Tan(75f * Mathf.Deg2Rad);

    const float SkinWidth = 0.02f;
    const int TotalHorizontalRay = 8;

    float _verticalDistanceBetweenRays;
    float _horizontalDistanceBetweenRays;
    float _jumpIn;

    private void Awake()
    {
        HandleCollisions = true;
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
        _jumpIn -= Time.deltaTime;

        _velocity.y += Parameters.Gravity * Time.deltaTime;

        Move(Velocity * Time.deltaTime);
    }

    public bool CanJump
    {
        get
        {
            if (Parameters.JumpRestrictions == ControllerParamaters.JumpBehavior.CanJumpAnywhere)
            {
                return _jumpIn <= 0;
            }
            else if (Parameters.JumpRestrictions == ControllerParamaters.JumpBehavior.CanJumpOnGround)
            {
                return State.IsGrounded;
            }
            else
            {
                return false;
            }
        }
    }

    public void Jump()
    {
        AddForce(new Vector2(0, Parameters.JumpMagnitude));
        _jumpIn = Parameters.JumpFrequency;
    }

    public void AddForce(Vector2 vector2)
    {
        _velocity += vector2;
    }

    public void SetVerticalForce(float y)
    {

        _velocity.y = y;

    }

    private void Move(Vector2 deltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;

        State.Reset();

        if (HandleCollisions)
        {
            Handleplatforms();

            CalculateRayOrigins();

            if (deltaMovement.y < 0 && wasGrounded)
            {
                HandleVerticalSlope(ref deltaMovement);
            }

            if (Mathf.Abs(deltaMovement.x) > 0.0001f)
            {
                MoveHorizontaly(ref deltaMovement);
            }

            MoveVerticaly(ref deltaMovement);

            CorrectHorizontalPlacement(ref deltaMovement, true);
            CorrectHorizontalPlacement(ref deltaMovement, false);
        }

        _transform.Translate(deltaMovement, Space.World);

        if (Time.deltaTime > 0)
        {
            _velocity = deltaMovement / Time.deltaTime;
        }

        if (State.IsMovingUpSlope)
        {
            _velocity.y = 0;

        }

        if (StandingOn != null)
        {
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);

            if (_lastStandingOn != StandingOn)
            {
                StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                _lastStandingOn = StandingOn;
            }
        }

    }

    private void Handleplatforms()
    {
        if (StandingOn != null)
        {
            var globalPlatformPoint = StandingOn.transform.TransformPoint(_activeLocalPlatformPoint);
            var moveDistance = globalPlatformPoint - _activeGlobalPlatformPoint;

            if (moveDistance != Vector3.zero)
            {
                transform.Translate(moveDistance, Space.World);
            }
        }

        StandingOn = null;
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
    {

        var halfWidht = (_boxCollider2D.size.x - _localScale.x) / 2;
        var rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;

        if (isRight)
        {
            rayOrigin.x += SkinWidth - halfWidht;
        }
        else
        {
            rayOrigin.x += -SkinWidth + halfWidht;
        }

        var rayDirection = isRight ? Vector2.right : Vector2.left;
        var offset = 0f;

        for (int i = 1; i < TotalHorizontalRay - 1; i++)
        {

            var rayVector = new Vector2(rayOrigin.x + deltaMovement.x, rayOrigin.y + deltaMovement.y + i * (_verticalDistanceBetweenRays));
            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidht, PlatformMask);

            if (!raycastHit)
            {
                continue;
            }

            offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidht) : (halfWidht - (transform.position.x - raycastHit.point.x));
        } //for


        deltaMovement.x += offset;
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

            if (!isGoingUp)
            {
                StandingOn = raycastHit.collider.gameObject;
                Debug.Log(StandingOn.name);
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

            if (!isGoingUp && deltaMovement.y > 0.0001f)
            {
                State.IsMovingUpSlope = true;
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

            if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), isGoingRight))
            {

                break;

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

    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {

        if (Mathf.RoundToInt(angle) >= 90)
        {
            return false;
        }

        if (angle > Parameters.SlopeLimit)
        {
            deltaMovement.x = 0;
            return true;
        }

        if (_jumpIn > 0)
        {
            return true;
        }

        deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad)) * deltaMovement.x;
        State.IsMovingUpSlope = true;
        State.IsCollidingBelow = true;

        return true;

    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {
        var center = (_raycastBottomLeft.x + _raycastBottomRight.x) / 2;
        var direction = -Vector2.up;
        var slopeDistance = SlopeLimitTan * (_raycastBottomRight.x - center);
        var slopeRayVector = new Vector2(center, _raycastBottomLeft.y);

        var raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PlatformMask);

        if (!raycastHit)
        {
            return;
        }

        var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);

        if (!isMovingDownSlope)
        {
            return;
        }

        var angle = Vector2.Angle(raycastHit.normal, Vector2.up);

        if (Mathf.Abs (angle) < .0001f)
        {
            return;
        }

        State.IsMovingDownSlope = true;
        State.SlopeAngle = angle;

        deltaMovement.y = raycastHit.point.y - slopeRayVector.y;

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

    public void SetForce(Vector2 force)
    {

        _velocity = force;

    }

    public void SetHorizontalForce(float _normalHorizontalSpeed)
    {

        _velocity.x = _normalHorizontalSpeed;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var parameters = collision.gameObject.GetComponent<ControllerVolume>();

        if (parameters == null)
        {
            return;
        }

        _OverrideParameters = parameters.Paramaters;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var parameters = collision.gameObject.GetComponent<ControllerVolume>();

        if (parameters == null)
        {
            return;
        }

        _OverrideParameters = null;
    }

}
