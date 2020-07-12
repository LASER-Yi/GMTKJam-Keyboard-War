using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemies")]
    public List<Keyboard.EnemiesSpawner> m_EnemiesSpawners = new List<Keyboard.EnemiesSpawner>();
    public List<GameObject> m_EnemiesPrefabs = new List<GameObject>();

    public float m_EnemySpawnRound = 12.0f;

    // private List<Keyboard.Enemy> m_DidSpawnEnemy = new List<Keyboard.Enemy>();
    [Header("Items")]
    public Keyboard.ItemSpawnKey m_ItemSpawner;
    public List<GameObject> m_BattleItems = new List<GameObject>();
    public List<GameObject> m_SupportItems = new List<GameObject>();
    // private List<Keyboard.BaseItem> m_DidSpawnItem = new List<Keyboard.BaseItem>();

    private bool m_GameStarted = false;

    public float m_ItemSpawnRound = 5.0f;

    private void Awake() 
    {
        StartCoroutine(EnemySpawnLoop());
        StartCoroutine(ItemSpawnLoop());
    }

    public void StartGame()
    {
        m_GameStarted = true;
    }

    IEnumerator EnemySpawnLoop()
    {
        while (true)
        {
            if(m_GameStarted)
            {
                yield return new WaitForSeconds(m_EnemySpawnRound);

                foreach(var spawner in m_EnemiesSpawners)
                {
                    spawner.AddToSpawnQueue(Random.Range(0.5f, 5f), m_EnemiesPrefabs[0]);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator ItemSpawnLoop()
    {
        int count = 0;
        while (true)
        {
            if(m_GameStarted)
            {
                yield return new WaitForSeconds(m_ItemSpawnRound);

                if(count % 4 < 3)
                {
                    m_ItemSpawner.Spawn(m_BattleItems[0]);
                }
                else
                {
                    m_ItemSpawner.Spawn(m_SupportItems[0]);
                }

                ++count;
            }
            else
            {
                yield return null;
            }
        }
    }
}
