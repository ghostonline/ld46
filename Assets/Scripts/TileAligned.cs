using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAligned : MonoBehaviour
{
    void Awake()
    {
        var pos = transform.position;
        pos.x = Mathf.Round(pos.x * 2f) / 2f;
        pos.y = Mathf.Round(pos.y * 2f) / 2f;
        transform.position = pos;
    }
}
