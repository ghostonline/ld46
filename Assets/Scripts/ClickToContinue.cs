using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToContinue : MonoBehaviour
{
    public int m_forcedBuildIndex = -1;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            var buildIndexToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            if (m_forcedBuildIndex > -1)
            {
                buildIndexToLoad = m_forcedBuildIndex;
            }
            SceneManager.LoadScene(buildIndexToLoad);
        }
    }
}
