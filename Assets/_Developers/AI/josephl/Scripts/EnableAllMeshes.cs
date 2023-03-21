using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAllMeshes : NetworkBehaviour
{

    private MeshRenderer[] meshes;

    public override void OnStartServer()
    {
        base.OnStartServer();

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
