using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class KeyboardManager : SceneSingleton<KeyboardManager>
    {

        private Dictionary<KeyCode, Key> mKeyList = new Dictionary<KeyCode, Key>();

        private Key m_SelectingKey;

        public void RegisterKey(KeyCode code, Key key)
        {
            mKeyList.Add(code, key);
        }

        private void Update() 
        {
            foreach(var pair in mKeyList)
            {
                if(Input.GetKeyUp(pair.Key))
                {
                    //TODO: Handle key logic
                    if(m_SelectingKey)
                    {
                        if(m_SelectingKey != pair.Value)
                        {
                            m_SelectingKey.KeyTransfer(pair.Value);
                        }

                        m_SelectingKey.DidDeselectedKey();
                        m_SelectingKey = null;
                    }
                    else if(pair.Value.GetLinkStatus())
                    {
                        m_SelectingKey = pair.Value;
                        pair.Value.DidSelectedKey();
                    }

                    return;
                }
            }

            if(Input.GetKeyUp(KeyCode.Escape))
            {
                if(m_SelectingKey) m_SelectingKey.DidDeselectedKey();
                m_SelectingKey = null;
            }
        }

    }
}
