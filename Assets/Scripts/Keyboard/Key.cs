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

        protected List<BaseItem> m_ItemLink = new List<BaseItem>();

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

                Vector3 pos = transform.position + Vector3.up * (GetPlaceHeight() + itemBound);
                item.transform.DOMove(pos, 0.4f);

                m_ItemLink.Add(item);
                return true;
            }
            else 
            {
                return false;
            }
        }

        public void RemoveFromKey(BaseItem item)
        {
            m_ItemLink.Remove(item);
            ReorderItems();
        }

        private float GetPlaceHeight()
        {
            float height = m_Collider.bounds.extents.y;

            foreach(var item in m_ItemLink)
            {
                float boundSize = item.m_Collider.bounds.extents.y;
                height += 2 * boundSize;
            }

            return height;
        }

        private void ReorderItems()
        {
            float height = m_Collider.bounds.extents.y;

            foreach(var item in m_ItemLink)
            {
                float boundSize = item.m_Collider.bounds.extents.y;
                Vector3 position = transform.position + Vector3.up * (height + boundSize);
                item.transform.DOMove(position, 0.2f);

                height += 2 * boundSize;
            }
        }

        private void DeactivateAll()
        {
            foreach(var item in m_ItemLink)
            {
                item.Deactivate();
            }
        }

        private void ActivateAll()
        {
            foreach(var item in m_ItemLink)
            {
                item.Activate();
            }
        }

        private Coroutine m_ReorderRoutine = null;

        IEnumerator ReoderItemsDelay()
        {
            // Disable all items first
            DeactivateAll();
            yield return new WaitForSecondsRealtime(0.6f);
            ReorderItems();
            m_ReorderRoutine = null;
        }

        public void DidExplosion(BaseItem item)
        {
            if(m_ItemLink.Remove(item))
            {
                // Exist, Reorder the tower
                ReorderItems();
            }
        }

    }
}
