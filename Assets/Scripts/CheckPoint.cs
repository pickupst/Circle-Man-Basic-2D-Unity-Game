﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private List<IPlayerRespawnListener> _listeners;

    private void Awake()
    {
        _listeners = new List<IPlayerRespawnListener>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer(Player player)
    {

        player.RespawnAt(transform);
        foreach (var item in _listeners)
        {
            item.OnPlayerRespawnInThisCheckPoint();
        }

    }

    public void AssignObjectToCheckPoint(IPlayerRespawnListener listener)
    {

        _listeners.Add(listener);

    }

    public void PlayerHitCheckPoint()
    {

        StartCoroutine(PlayerHitCheckPointCO(LevelManager.Instance.CurrentTimeBonus));

    }

    public IEnumerator PlayerHitCheckPointCO (int bonus)
    {

        FloatingText.Show("CheckPoint!!", "CheckPointsText", new CenteredTextPositioner(.2f));
        yield return new WaitForSeconds(.3f);
        FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckPointsText", new CenteredTextPositioner(.2f));
    }

}
