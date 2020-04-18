using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public bool m_edible = true;
    public bool m_satisfying = false;

    public void Consume()
    {
        m_edible = false;
    }
}
