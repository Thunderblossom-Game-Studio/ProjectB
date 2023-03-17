
using Pelumi.Juicer;
using System.Collections;
using UnityEngine;

public class DestroyByInterval : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private bool fadeMaterial;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        if(fadeMaterial)
            StartCoroutine(Juicer.FadeOutMaterial
                (transform.GetComponent<Renderer>().material, 2f,null));

        Destroy(gameObject);
    }

}
