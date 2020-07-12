using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class EnemiesSpawner : MonoBehaviour
    {
        public Key m_SpawnKey;

        private Queue<KeyValuePair<float, GameObject>> m_SpawnQueue = new Queue<KeyValuePair<float, GameObject>>();

        private Enemy m_PreviousEnemy;

        private void Awake() 
        {
            StartCoroutine(SpawnLoop());
        }

        public void AddToSpawnQueue(float delay, GameObject prefab)
        {
            m_SpawnQueue.Enqueue(new KeyValuePair<float, GameObject>(delay, prefab));
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                if(m_SpawnQueue.Count != 0)
                {
                    if(m_PreviousEnemy && m_PreviousEnemy.m_StandingKey == m_SpawnKey)
                    {
                        yield return null;
                    }
                    else
                    {
                        var item = m_SpawnQueue.Dequeue();
                        yield return new WaitForSeconds(item.Key);
                        Spawn(item.Value);
                    }

                }
                else
                {
                    yield return null;
                }
            }
        }
        private void Spawn(GameObject prefab)
        {
            var enemey = Instantiate(prefab);
            var script = enemey.GetComponent<Enemy>();
            script.Initialize(m_SpawnKey);
            m_PreviousEnemy = script;
        }
    }
}
