using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public AudioClip shootSound;

    public GameObject DestroyedEffect;

    public Vector2 _startPostion;

    public float speed = 9f;
    public float fireRate = 1f;
    public int PointsToGivePlayer = 15;

    public Projectile Projectile;

    private CharacterController _controller;
    private Vector2 _direction;
    private float canFireRate;

    public void OnPlayerRespawnInThisCheckPoint()
    {
        if (!gameObject.activeSelf)
        {
            _direction = new Vector2(-1, 0);
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = _startPostion;
            gameObject.SetActive(true);
        }
        
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGivePlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();

            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameMenager.Instance.addPoint(PointsToGivePlayer);
                FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
            }
        }

        Instantiate(DestroyedEffect,  transform.position, transform.rotation);
        gameObject.SetActive(false);

    }


    // Start is called before the first frame update
    void Start()
    {
        _startPostion = transform.position;
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
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        }

        if (canFireRate < 0)
        {

            var rayCast = Physics2D.Raycast(transform.position, _direction, 30, 1 << LayerMask.NameToLayer("Player"));
            if (!rayCast)
            {
                return;
            }

            var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
            projectile.Initialize(gameObject, _direction, _controller.Velocity);
            canFireRate = fireRate;
        }

        

    }
}
