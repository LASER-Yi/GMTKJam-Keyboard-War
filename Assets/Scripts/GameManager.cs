using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager
{
    private static GameManager _instance;
    public static GameManager Instance 
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }


    public void TowerDidExplosion()
    {
        Camera.main.DOShakePosition(1.0f, 0.8f, 25);
    }

    public void TowerDidLink()
    {

    }

    public void EnemeyDidExplosion()
    {
        Camera.main.DOShakePosition(0.6f, 0.3f, 10);
    }
}
