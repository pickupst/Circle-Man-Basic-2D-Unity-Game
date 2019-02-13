using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveHealth : MonoBehaviour, IPlayerRespawnListener
{
    public GameObject Effect;
    public int HealthToGive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player == null)
        {
            return; 
        }

        player.GiveHealth(HealthToGive, gameObject);

        Instantiate(Effect, transform.position, transform.rotation);
        gameObject.SetActive(false);


    }

    public void OnPlayerRespawnInThisCheckPoint()
    {
        gameObject.SetActive(true);
    }
}
