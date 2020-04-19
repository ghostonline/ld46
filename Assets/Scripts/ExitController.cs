using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    public GameObject m_exitLights;
    public GameObject m_exitBlock;
    public Collider2D m_doorCloseTrigger;

    float m_enterTime = 0f;
    Player m_player = null;
    bool m_performExitSequence = false;

    private void Start()
    {
        m_exitBlock.SetActive(true);
    }

    void Update()
    {
        if (m_enterTime > 1f)
        {
            if (!m_performExitSequence)
            {
                var canExit = GameController.Instance.CanLeaveLevel();
                SetExitOpen(canExit);
            }
        }
        else
        {
            m_enterTime += Time.deltaTime;
        }
    }

    void SetExitOpen(bool canExit)
    {
        if (m_exitLights.activeSelf != canExit)
        {
            m_exitLights.SetActive(canExit);
            m_exitBlock.SetActive(!canExit);
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayerAtExit() && !m_performExitSequence)
        {
            m_performExitSequence = true;
            SetExitOpen(false);
            AudioPlayer.Play(Clip.Fanfare);
        }
    }

    private bool IsPlayerAtExit()
    {
        if (m_player == null)
        {
            m_player = FindObjectOfType<Player>();
        }

        return m_player && m_doorCloseTrigger.OverlapPoint(m_player.transform.position);
    }
}
