using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player player { get; private set; }

    private List<CheckPoint> _checkPoints;
    private int currentCheckPointIndex;

    private DateTime _started;
    private int _savedPoint;
    public TimeSpan RunningTime
    {
        get
        {
            return DateTime.UtcNow - _started;
        }
    }
    public int BonusCutoffSeconds = 10;
    public int BonusSecondMultiplier = 3;
    public int CurrentTimeBonus
    {
        get
        {
            var secondDifference = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondDifference) * BonusSecondMultiplier;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _started = DateTime.UtcNow;

        _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(t => t.transform.position.x).ToList<CheckPoint>();

        currentCheckPointIndex = _checkPoints.Count > 0 ? 0 : -1;

        player = FindObjectOfType<Player>();

        var listener = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();

        foreach (var item in listener)
        {
            for (int i = _checkPoints.Count - 1; i >= 0; i--)
            {
                var distance = ((MonoBehaviour)item).transform.position.x - _checkPoints[i].transform.position.x;
                if (distance < 0)
                {
                    continue;
                }
                else
                {
                    _checkPoints[i].AssignObjectToCheckPoint(item);
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        var isAtLastCheckpoint = currentCheckPointIndex + 1 >= _checkPoints.Count;

        if (isAtLastCheckpoint)
        {
            //LEVEL DONE!
            return;
        }

        var distanceToNextCheckpoint = _checkPoints[currentCheckPointIndex + 1].transform.position.x - player.transform.position.x;

        if (distanceToNextCheckpoint >= 0)
        {
            return;
        }

        currentCheckPointIndex++;

        GameMenager.Instance.addPoint(CurrentTimeBonus);
        _savedPoint = GameMenager.Instance.point;
        _started = DateTime.UtcNow;
    }

    public void KillPlayer()
    {

        StartCoroutine(KillPlayerCo());

    }

    private IEnumerator KillPlayerCo()
    {
        player.Kill();

        yield return new WaitForSeconds(1f);

        if (currentCheckPointIndex != -1)
        {
            _checkPoints[currentCheckPointIndex].SpawnPlayer(player);
        }

        _started = DateTime.UtcNow;
        GameMenager.Instance.resetPoint(_savedPoint);

    }
}
