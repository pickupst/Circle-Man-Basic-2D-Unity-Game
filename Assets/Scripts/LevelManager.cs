using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player player { get; private set; }

    private List<CheckPoint> _checkPoints;
    private int currentCheckPointIndex; 

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(t => t.transform.position.x).ToList<CheckPoint>();

        currentCheckPointIndex = _checkPoints.Count > 0 ? 0 : -1;

        player = FindObjectOfType<Player>();
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

    }

    public void KillPlayer()
    {

        StartCoroutine(KillPlayerCo());

    }

    private IEnumerator KillPlayerCo()
    {
        player.Kill();

        yield return new WaitForSeconds(1f);

        _checkPoints[currentCheckPointIndex].SpawnPlayer(player);

    }
}
