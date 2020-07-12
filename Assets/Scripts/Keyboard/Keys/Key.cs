using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Keyboard
{
    [RequireComponent(typeof(BoxCollider))]
    public class Key : MonoBehaviour
    {
        public KeyCode m_KeyCode;
        public Text m_UiText;
        public bool m_ShowUiText = true;

        [SerializeField]
        public List<Key> m_KeyLink = new List<Key>();

        protected List<BaseItem> m_ItemLink = new List<BaseItem>();

        public int m_ItemCount
        {
            get
            {
                return m_ItemLink.Count;
            }
        }

        protected BoxCollider m_Collider;

        public float m_TopSurfaceOffset
        {
            get
            {
                return m_Collider.bounds.extents.y;
            }
        }

        protected void Awake()
        {
            m_Collider = GetComponent<BoxCollider>();
            KeyboardManager.Instance.RegisterKey(m_KeyCode, this);
            if (m_UiText && m_ShowUiText)
            {
                m_UiText.text = m_KeyCode.ToString();
            }

            m_Material = GetComponent<Renderer>().material;
            m_OriginalColor = m_Material.GetColor(m_BaseColorName);
        }
        protected bool m_KeyCanLink = false;

        public bool GetLinkStatus()
        {
            return m_KeyCanLink;
        }

        private void ChangeLinkStatus(bool status)
        {
            if(m_KeyCanLink == status) return;

            m_KeyCanLink = status;

            if(!m_ShowUiText || !m_UiText) return;

            m_UiText.enabled = m_KeyCanLink;
        }

        private Material m_Material;

        [Header("Status"), ColorUsage(false, false)]
        public Color m_ExplosionColor;
        private Color m_OriginalColor;

        public void DidSelectedKey()
        {
        }

        public void DidDeselectedKey()
        {
        }

        // Once the key has item, the around can link 
        // The method will be call by around key
        protected void LinkStatusChanged()
        {

            if(m_ItemCount > 0)
            {
                ChangeLinkStatus(true);
                return;
            }

            bool canLink = false;
            foreach(var key in m_KeyLink)
            {
                if(key.m_ItemCount > 0)
                {
                    canLink = true;
                    break;
                }
            }

            ChangeLinkStatus(canLink);
        }

        public bool LinkToKey(BaseItem item, bool forced = false)
        {
            if(m_KeyCanLink || forced)
            {
                float itemBound = item.m_Collider.bounds.extents.y;

                Vector3 pos = transform.position + Vector3.up * (GetPlaceHeight() + itemBound);
                if(forced)
                {
                    item.transform.position = pos;
                }
                else
                {
                    item.transform.DOJump(pos, 5.0f, 1, 0.5f);
                    // item.transform.DOMove(pos, 0.4f);
                    GameManager.Instance.TowerDidLink();
                }

                m_ItemLink.Add(item);
                if(m_ItemCount <= 2)
                {
                    foreach(var key in m_KeyLink)
                    {
                        key.LinkStatusChanged();
                    }
                }

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

            if(m_ItemCount == 0)
            {
                foreach(var key in m_KeyLink)
                {
                    key.LinkStatusChanged();
                }
            }
        }

        protected float GetPlaceHeight()
        {
            float height = m_TopSurfaceOffset;

            foreach(var item in m_ItemLink)
            {
                float boundSize = item.m_Collider.bounds.extents.y;
                height += 2 * boundSize;
            }

            return height;
        }

        public int GetFloorIndex(BaseItem item)
        {
            int index = m_ItemLink.FindIndex(link => link == item);

            return index;
        }

        private void ReorderItems()
        {
            float height = m_TopSurfaceOffset;

            foreach(var item in m_ItemLink)
            {
                float boundSize = item.m_Collider.bounds.extents.y;
                Vector3 position = transform.position + Vector3.up * (height + boundSize);
                item.transform.DOMove(position, 0.2f);

                height += 2 * boundSize;
            }
        }

        public void Transfer(Key to)
        {
            // get the toppest item in self and transfer to to key
            if(m_ItemCount == 0) return;

            var item = m_ItemLink[m_ItemCount - 1];
            item.TryLink(to, false);
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

        public void UpdateAroundHeat(int layer, float damage)
        {
            this.TookHeatDamage(layer, damage);

            foreach(var key in m_KeyLink)
            {
                key.TookHeatDamage(layer, damage);
            }
        }

        private string m_BaseColorName = "_BaseColor";

        public void EnemeyDidExplosion(Enemy enemy)
        {
            UpdateAroundHeat(0, enemy.m_ExplosionDamage);
        }

        public void DidExplosion(BaseItem item)
        {
            int index = GetFloorIndex(item);
            UpdateAroundHeat(index, item.m_ExplosionDamage);
            RemoveFromKey(item);
        }

        protected BaseItem SafeItemIndex(int index)
        {
            if(index < 0 || index >= m_ItemCount) return null;

            return m_ItemLink[index];
        }

        public void TookHeatDamage(int layer, float damage)
        {
            int start = layer - 1;
            int end = layer + 1;

            for (int i = start; i <= end; ++i)
            {
                var item = SafeItemIndex(i);
                if(item) item.Damage(damage);
            }

            if(damage > 0)
            {
                m_Material.SetColor(m_BaseColorName, m_ExplosionColor);
                m_Material.DOColor(m_OriginalColor, m_BaseColorName, 0.6f);
            }
        }

    }
}
