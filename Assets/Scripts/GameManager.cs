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
    public KeyValuePair<float, float> m_NextItemRound = new KeyValuePair<float, float>(0, 5.0f);

    public KeyValuePair<float, float> m_NextEnemyRound = new KeyValuePair<float, float>(0, 15.0f);

    public float m_RemainItemSecond
    {
        get
        {
            return Mathf.Max(0.0f, m_NextItemRound.Value - (Time.time - m_NextItemRound.Key));
        }
    }

    public float m_RemainEnemySecond
    {
        get
        {
            return Mathf.Max(0.0f, m_NextEnemyRound.Value - (Time.time - m_NextEnemyRound.Key));
        }
    }

    public int m_HighestScore = 0;
    public int m_SessionScore = 0;

    public void StartGame()
    {
        GameObject.FindObjectOfType<SpawnManager>().StartGame();
        GameObject.FindObjectOfType<UIManager>().StartGame();

        m_HighestScore = Mathf.Max(m_HighestScore, m_SessionScore);
        m_SessionScore = 0;
    }

    public void StopGame(bool result)
    {

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
