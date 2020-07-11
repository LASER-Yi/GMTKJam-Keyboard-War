using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Keyboard
{
    [RequireComponent(typeof(BoxCollider))]
    public class Key : MonoBehaviour
    {
        public KeyCode m_KeyCode;

        [SerializeField]
        protected HashSet<Key> m_KeyLink = new HashSet<Key>();

        protected BaseItem m_ItemLink = null;

        protected BoxCollider m_Collider;

        private void Awake()
        {
            m_Collider = GetComponent<BoxCollider>();
        }

        public bool CanLink()
        {
            return true;
        }

        public bool LinkToKey(BaseItem item)
        {
            if(CanLink())
            {
                float itemBound = item.m_Collider.bounds.extents.y;
                float keyBound = m_Collider.bounds.extents.y;

                Vector3 pos = transform.position + Vector3.up * (itemBound + keyBound);
                item.transform.DOMove(pos, 0.4f);
                return true;
            }
            else 
            {
                return false;
            }
        }

    }
}
