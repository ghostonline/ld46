using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController m_instance;
    public static GameController Instance
    {
        get { return m_instance; }
    }

    private bool m_died = false;
    private bool m_chompFed = false;

    private void Awake()
    {
        m_instance = this;
    }

    public void OnLevelExit()
    {
        var targetScene = SceneManager.GetActiveScene().buildIndex;
        if (m_chompFed && !m_died)
        {
            targetScene += 1;
        }
        SceneManager.LoadScene(targetScene);
    }

    public void OnChompFed()
    {
        m_chompFed = true;
    }

    public void OnDied()
    {
        m_died = true;
    }
}
