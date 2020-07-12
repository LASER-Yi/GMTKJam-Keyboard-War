using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameScreen : MonoBehaviour
{
    public Text m_ItemText;
    public Text m_EnemyText;
    public Text m_ScoreText;

    private StringBuilder m_Builder = new StringBuilder();

    private void Update() 
    {
        UpdateItemText();

        UpdateEnemyText();

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        var score = GameManager.Instance.m_SessionScore;
        m_ScoreText.text = score.ToString();
    }

    private void UpdateEnemyText()
    {
        var enemySecond = GameManager.Instance.m_RemainEnemySecond;

        m_Builder.Clear();
        m_Builder.Append("Next Wave in\n");
        m_Builder.Append(enemySecond.ToString("n2"));
        m_Builder.Append('s');

        m_EnemyText.text = m_Builder.ToString();
        var percent = (1.5f - (enemySecond / GameManager.Instance.m_NextEnemyRound.Value) * 0.5f);

        m_EnemyText.transform.localScale = Vector3.one * percent;

    }

    private void UpdateItemText()
    {
        var itemSecond = GameManager.Instance.m_RemainItemSecond;

        m_Builder.Clear();
        m_Builder.Append("Next Item in\n");
        m_Builder.Append(itemSecond.ToString("n2"));
        m_Builder.Append('s');

        m_ItemText.text = m_Builder.ToString();
        var percent = (1.5f - (itemSecond / GameManager.Instance.m_NextItemRound.Value) * 0.5f);

        m_ItemText.transform.localScale = Vector3.one * percent;

    }
}
