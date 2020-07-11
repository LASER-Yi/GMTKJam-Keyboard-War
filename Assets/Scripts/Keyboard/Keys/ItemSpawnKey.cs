using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class ItemSpawnKey : Key
    {
        [Header("Spawner")]
        public List<GameObject> m_SpawnPrefabs;
        protected new void Awake() 
        {
            base.Awake();
            m_KeyCanLink = false;
            StartCoroutine(SpawnLoop());
        }

        public bool m_EnableSpawn = true;
        public float m_SpawnInterval = 15.0f;
        public int m_ErrorDamage = 10;

        private int m_Index = 0;

        IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_SpawnInterval);
                if (m_EnableSpawn)
                {
                    if (m_ItemLink.Count < 3)
                    {
                        // Pick a random object from list
                        int index = Random.Range(0, m_SpawnPrefabs.Count);

                        var item = Instantiate(m_SpawnPrefabs[index]).GetComponent<BaseItem>();
                        item.ForceLink(this);

                        item.name = "Spawn Tower_" + m_Index;
                        ++m_Index;
                    }
                    else
                    {
                        // Do some damage
                        foreach(var key in m_KeyLink)
                        {
                            key.TookHeatDamage(0, m_ErrorDamage);
                        }
                        
                    }
                }
            }
        }
    }
}
