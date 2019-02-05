using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player player { get; private set; }

    private List<CheckPoint> _checkPoints;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(t => t.transform.position.x).ToList<CheckPoint>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillPlayer()
    {

        StartCoroutine(KillPlayerCo());

    }

    private IEnumerator KillPlayerCo()
    {
        player.Kill();

        yield return new WaitForSeconds(2f);

        _checkPoints[0].SpawnPlayer(player);

    }
}
