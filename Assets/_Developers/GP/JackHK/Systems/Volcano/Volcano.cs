using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    public GameObject _lavaPool;

    public void CreateSplatter(GameObject sender)
    {
        Instantiate(_lavaPool, sender.transform.position, Quaternion.identity);
    }
}