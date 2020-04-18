using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    Player m_player;
    BoxCollider2D m_collider;
    float m_leftTime = 0f;

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!IsPlayerInCage())
        {
            m_leftTime += Time.deltaTime;
            if (m_leftTime > 2f)
            {
                RestartLevel();
            }
        }
    }

    private bool IsPlayerInCage()
    {
        if (m_player == null)
        {
            m_player = FindObjectOfType<Player>();
        }

        return m_player && m_collider.OverlapPoint(m_player.transform.position);
    }

    private void RestartLevel()
    {
        GameController.Instance.OnLevelExit();
    }
}
