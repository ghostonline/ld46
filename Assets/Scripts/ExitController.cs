using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    public GameObject m_exitLights;
    public GameObject m_exitBlock;

    float m_enterTime = 0f;

    private void Start()
    {
        m_exitBlock.SetActive(true);
    }

    void Update()
    {
        if (m_enterTime > 1f)
        {
            var canExit = GameController.Instance.CanLeaveLevel();
            if (m_exitLights.activeSelf != canExit)
            {
                m_exitLights.SetActive(canExit);
                m_exitBlock.SetActive(!canExit);
            }
        }
        else
        {
            m_enterTime += Time.deltaTime;
        }
    }
}
