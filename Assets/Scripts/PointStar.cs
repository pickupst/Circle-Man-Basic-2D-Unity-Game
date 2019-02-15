using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointStar : MonoBehaviour, IPlayerRespawnListener
{
    public AudioClip hitStarSound;

    public GameObject Effect;
    public int PointToAdd = 10;

    public void OnPlayerRespawnInThisCheckPoint()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(hitStarSound, transform.position);

            GameMenager.Instance.addPoint(PointToAdd);
            Instantiate(Effect, transform.position, transform.rotation);
            gameObject.SetActive(false);

            FloatingText.Show(string.Format("+{0}!", PointToAdd), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
        }
        
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
