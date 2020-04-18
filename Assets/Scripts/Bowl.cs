using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    public void Put(Carryable carryable)
    {
        GameObject.Destroy(carryable.gameObject);
    }
}
