using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : Projectile, ITakeDamage
{
    public int Damage = 10;

    public GameObject DestroyedEffect;

    public float TimeToLive = 10;


    public void TakeDamage(int damage, GameObject instigator)
    {



    }

    private void Update()
    {
        if ((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }

        transform.Translate(Direction * (Mathf.Abs(InitialVelocity.x) + 5f) * Time.deltaTime, Space.World);
    }

    private void DestroyProjectile()
    {

        if (DestroyedEffect != null)
        {
            Instantiate(DestroyedEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }


    protected override void OnCollideOther(Collider2D other)
    {

        DestroyProjectile();

    }

    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {

        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

}
