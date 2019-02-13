using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamageToPlayer : MonoBehaviour
{
    public Vector2 _lastPosition;
    public Vector2 _velocity;
    public int DamageToGive = 10;

    private void LateUpdate()
    {
        _velocity = (_lastPosition - (Vector2)transform.position) / Time.deltaTime;
        _lastPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (!player)
        {
            return;
        }

        player.TakeDamage(DamageToGive, gameObject);


        var controller = player.GetComponent<CharacterController>();
        var totalVelocty = controller.Velocity + _velocity;

        controller.SetForce(new Vector2((-1 * Mathf.Sign(totalVelocty.x) * Mathf.Clamp (Mathf.Abs(totalVelocty.x) * 6, 10 , 40)),
                                        (-1 * Mathf.Sign(totalVelocty.y) * Mathf.Clamp(Mathf.Abs(totalVelocty.y) * 2, 0, 15))));

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
