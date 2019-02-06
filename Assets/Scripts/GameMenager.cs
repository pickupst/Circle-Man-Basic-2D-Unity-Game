using System.Collections.Generic;
using UnityEngine;

public class GameMenager
{

    private static GameMenager _instance;
    public static GameMenager Instance
    {
        get
        {

            return _instance ?? (_instance = new GameMenager());

        }

    }

    public int point { get; private set; }

    public void resetPoint(int _point)
    {

        point = _point;

    }

    public void Reset ()
    {

        point = 0;

    }

    public void addPoint(int pointToAdd)
    {

        point += pointToAdd;

    }

    private GameMenager()
    {

    }

}
