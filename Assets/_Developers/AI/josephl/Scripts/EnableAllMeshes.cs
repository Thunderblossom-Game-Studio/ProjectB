
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAllMeshes : MonoBehaviour
{

    private MeshRenderer[] meshes;

    public void OnStartServer()
    {
        StartCoroutine(delayedstart());
    }
    
    void Start()
    {
        //StartCoroutine(delayedstart());
    }

    IEnumerator delayedstart()
    {
        yield return new WaitForSeconds(0.1f);

        meshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
        }
    }
}
