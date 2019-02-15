using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathedProjectile : MonoBehaviour, ITakeDamage
{
    private Transform _destination;
    private float _speed;

    public AudioClip DestroySound;

    public GameObject destroyEffect;
    public int PointsToGivePlayer = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);

        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;

        if (distanceSquared > 0.1f * 0.1f)
        {
            return;
        }

        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
        }

        AudioSource.PlayClipAtPoint(DestroySound, transform.position);
        Destroy(gameObject);
    }

    internal void Initialize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);

        var projectile = instigator.GetComponent<Projectile>();

        if (projectile != null && projectile.Owner.GetComponent<Player>() != null && PointsToGivePlayer != 0)
        {
            GameMenager.Instance.addPoint(PointsToGivePlayer);

            FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
        }
    }
}
