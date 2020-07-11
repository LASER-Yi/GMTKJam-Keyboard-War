using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class CoolerItem : BaseItem
    {
        [Header("Cooler")]
        public float m_CoolingInterval = 2.0f;
        public int m_CoolingValue = 35;

        private float m_LastCoolingTimeStamp = 0f;

        private bool m_IsCoolingDown
        {
            get
            {
                return Time.time - m_LastCoolingTimeStamp < m_CoolingInterval;
            }
        }

        private bool m_Activation = false;

        public override void Activate()
        {
            m_Activation = true;
        }

        public override void Deactivate()
        {
            m_Activation = false;
        }

        new protected void Update()
        {
            base.Update();

            if (m_Activation && !m_IsCoolingDown && m_LinkedKey)
            {
                Cooling();
            }
        }

        private void Cooling()
        {
            int index = m_LinkedKey.GetFloorIndex(this);
            m_LinkedKey.UpdateAroundHeat(index, -m_CoolingValue);
            m_LastCoolingTimeStamp = Time.time;
        }
    }
}
