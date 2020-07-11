using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class EnemiesSpawner : MonoBehaviour
    {
        public Key m_SpawnKey;

        public GameObject m_SpawnPrefab;

        [ContextMenu("Spawn")]
        public void Spawner()
        {
            var enemey = Instantiate(m_SpawnPrefab);

            enemey.GetComponent<Enemy>().Initialize(m_SpawnKey);
        }
    }
}
