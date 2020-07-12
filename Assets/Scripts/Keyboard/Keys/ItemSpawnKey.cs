using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class ItemSpawnKey : Key
    {
        [Header("Spawner")]
        public int m_ErrorDamage = 10;
        private int m_Index = 0;
        protected new void Awake() 
        {
            base.Awake();
            m_KeyCanLink = false;
        }
        public bool Spawn(GameObject prefab)
        {
            if (m_ItemLink.Count < 3)
            {
                var item = Instantiate(prefab).GetComponent<BaseItem>();
                item.ForceLink(this);

                item.name = "Spawn Tower_" + m_Index;
                ++m_Index;
                return true;
            }
            else
            {
                // Do some damage
                foreach(var key in m_KeyLink)
                {
                    key.TookHeatDamage(0, m_ErrorDamage);
                }
                return false;
            }
        }
    }
}
