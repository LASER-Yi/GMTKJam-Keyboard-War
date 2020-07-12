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
                        m_SelectingKey.KeyTransfer(pair.Value);
                        m_SelectingKey = null;
                    }
                    else
                    {
                        m_SelectingKey = pair.Value;
                    }

                    return;
                }
            }

            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_SelectingKey = null;
            }
        }

    }
}
