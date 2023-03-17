
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
            yield return Juicer.FadeOutMaterial
                (transform.GetComponent<Renderer>().material, 3f,null);

        Destroy(gameObject);
    }

}
