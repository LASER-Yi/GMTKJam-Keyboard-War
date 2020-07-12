using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndScreen : MonoBehaviour
{
    public Text m_StatusText;

    public Text m_ScoreText;

    private void Start() 
    {
        m_ScoreText.text = GameManager.Instance.m_SessionScore.ToString();

        bool result = GameManager.Instance.m_SessionResult;

        if(result)
        {
            m_StatusText.text = "YOU WIN!";
        }
        else
        {
            m_StatusText.text = "ALMOST THERE";
        }
    }

    public void RestartGame()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
