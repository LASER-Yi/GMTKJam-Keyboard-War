using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemies")]
    public List<Keyboard.EnemiesSpawner> m_EnemiesSpawners = new List<Keyboard.EnemiesSpawner>();
    public List<GameObject> m_EnemiesPrefabs = new List<GameObject>();

    // private List<Keyboard.Enemy> m_DidSpawnEnemy = new List<Keyboard.Enemy>();
    [Header("Items")]
    public Keyboard.ItemSpawnKey m_ItemSpawner;
    public List<GameObject> m_BattleItems = new List<GameObject>();
    public List<GameObject> m_SupportItems = new List<GameObject>();
    // private List<Keyboard.BaseItem> m_DidSpawnItem = new List<Keyboard.BaseItem>();

    private void Awake() 
    {
        StartCoroutine(EnemySpawnLoop());
        StartCoroutine(ItemSpawnLoop());
    }

    IEnumerator EnemySpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);

            foreach(var spawner in m_EnemiesSpawners)
            {
                spawner.AddToSpawnQueue(1.0f, m_EnemiesPrefabs[0]);
            }
        }
    }

    IEnumerator ItemSpawnLoop()
    {
        int count = 0;
        while (true)
        {
            yield return new WaitForSeconds(5.0f);

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
    }
}
