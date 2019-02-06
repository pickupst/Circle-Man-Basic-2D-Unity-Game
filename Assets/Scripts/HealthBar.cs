using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Player player;
    public Transform ForegroundSprite;

    public Color MaxHealthColor = new Color(255/255f, 63/255f, 63/255f);
    public Color MinHealthColor = new Color(64 / 255f, 137 / 255f, 255 / 255f);

    public SpriteRenderer ForeGroundRenderer;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var healthPercent = player.Health / (float) player.MaxHealth;
        ForegroundSprite.localScale = new Vector3(healthPercent, ForegroundSprite.localScale.y);
        ForeGroundRenderer.color = Color.Lerp(MaxHealthColor, MinHealthColor, healthPercent); 
    }
}
