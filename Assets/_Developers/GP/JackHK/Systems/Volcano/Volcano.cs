using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    public GameObject _lavaPool;

    public void CreateSplatter()
    {
        GameObject lava = Instantiate(_lavaPool, transform.position, Quaternion.identity);
    }
}