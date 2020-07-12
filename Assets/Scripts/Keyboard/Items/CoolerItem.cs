using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class CoolerItem : BaseItem
    {
        [Header("Cooler")]
        public int m_CoolingValue = 35;

        private bool m_Activation = false;

        public override void Activate()
        {
            m_Activation = true;
        }

        public override void Deactivate()
        {
            m_Activation = false;
        }

        private void LateUpdate() 
        {
            if (m_Activation && m_LinkedKey)
            {
                Cooling();
            }
        }

        private void Cooling()
        {
            int index = m_LinkedKey.GetFloorIndex(this);
            m_LinkedKey.UpdateAroundHeat(index, -m_CoolingValue * Time.deltaTime);
        }
    }
}
