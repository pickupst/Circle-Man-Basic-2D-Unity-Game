using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathedProjectileSpawner : MonoBehaviour
{
    public AudioClip spawnSound;

    public Transform Destination;
    public PathedProjectile Projectile;
    public float speed = 6;
    public float FireRate = 2f;

    public GameObject spawnEffect;


    private float _nextShotInSecond;
    // Start is called before the first frame update
    void Start()
    {

        _nextShotInSecond = FireRate;
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((_nextShotInSecond -= Time.deltaTime) > 0)
        {
            return;
        }
        _nextShotInSecond = FireRate;

        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation);

        projectile.Initialize(Destination, speed);

        if (spawnEffect != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, transform.position);
            Instantiate(spawnEffect, transform.position,transform.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        if (Destination == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
