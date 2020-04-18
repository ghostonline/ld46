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

    private static readonly string m_gameSceneName = "GameScene";
    private int m_currentLevel = 0;
    private bool m_chompFed = false;

    private void Awake()
    {
        m_instance = this;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    void Start()
    {
        m_currentLevel = 0;
        SceneManager.LoadScene(m_gameSceneName, LoadSceneMode.Additive);
    }

    void StartLevel()
    {
        Debug.LogFormat("Starting level {0}", m_currentLevel);
        m_chompFed = false;
        SceneManager.LoadSceneAsync(m_gameSceneName, LoadSceneMode.Additive);
    }

    public void OnLevelExit()
    {
        SceneManager.UnloadSceneAsync(m_gameSceneName);
        if (m_chompFed)
        {
            m_currentLevel += 1;
        }
    }

    private void SceneManager_sceneUnloaded(Scene scene)
    {
        if (scene.name == m_gameSceneName)
        {
            StartLevel();
        }
    }

    public void OnChompFed()
    {
        m_chompFed = true;
    }
}
