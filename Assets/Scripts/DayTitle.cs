using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DayTitle : MonoBehaviour
{
    public Text m_dayLabel;
    public int m_dayOffsetFromBuildIndex = 1;

    void Start()
    {
        var dayNum = SceneManager.GetActiveScene().buildIndex - m_dayOffsetFromBuildIndex;
        m_dayLabel.text = string.Format("Day {0}:\n{1}", dayNum, GameController.Instance.m_dayTitle);
    }
}
