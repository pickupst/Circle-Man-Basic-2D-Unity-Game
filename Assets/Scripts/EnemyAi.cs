using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float speed = 9f;
    public float fireRate = 1f;

    public Projectile Projectile;

    private CharacterController _controller;
    private Vector2 _direction;
    private float canFireRate;


    // Start is called before the first frame update
    void Start()
    {
        canFireRate = fireRate;
        _controller = GetComponent<CharacterController>();
        _direction = new Vector2(-1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        canFireRate -= Time.deltaTime;

        _controller.SetHorizontalForce(_direction.x * speed);

        if ((_direction.x <= 0 && _controller.State.IsCollidingLeft) || (_direction.x >= 0 && _controller.State.IsCollidingRight))
        {
            _direction = -_direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (canFireRate < 0)
        {
            var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
            projectile.Initialize(gameObject, _direction, _controller.Velocity);
            canFireRate = fireRate;
        }
        
    }
}
