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

    public ChompAI m_chomp;
    public Bowl m_bowl;
    public Transform m_spike;
    public string m_dayTitle = "Just another day";

    public bool m_wakeWhenFed = false;
    public bool m_foodMustBeInRange = true;
    public bool m_foodMustBeOnFloor = true;

    private bool m_died = false;
    private bool m_chompFed = false;

    private void Awake()
    {
        m_instance = this;
    }

    public void OnLevelExit()
    {
        var targetScene = SceneManager.GetActiveScene().buildIndex;
        if (CanLeaveLevel() && !m_died)
        {
            targetScene += 1;
        }
        SceneManager.LoadScene(targetScene);
    }

    public void OnBowlFilled()
    {
        if (m_chomp && m_wakeWhenFed)
        {
            m_chomp.WakeUp();
        }
    }

    public void OnChompFed()
    {
        m_chompFed = true;
    }

    public bool CanLeaveLevel()
    {
        if (m_chompFed)
        {
            return true;
        }
        else if (m_foodMustBeInRange && !IsBowlInSpikeRange())
        {
            return false;
        }
        else if (m_foodMustBeOnFloor && !IsBowlOnFloor())
        {
            return false;
        }
        else if (!IsBowlFull())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool IsBowlFull()
    {
        return m_bowl.IsFull();
    }

    private bool IsBowlOnFloor()
    {
        return m_bowl.transform.position.y < -3.5f;
    }

    private bool IsBowlInSpikeRange()
    {
        Vector2 offset = m_bowl.transform.position - m_spike.transform.position;
        return offset.magnitude <= m_chomp.m_chainLength;
    }

    public void OnDied()
    {
        m_died = true;
    }
}
