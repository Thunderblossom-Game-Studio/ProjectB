using Pelumi.Juicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParts : MonoBehaviour
{
    [SerializeField] private float explosionForce = 500.0f;
    [SerializeField] private float spreadRadious = 150.8f;
    [SerializeField] private float destroyDelay = 2.5f;
    [SerializeField] private float fadeOutTime = 1f;

    [SerializeField] private List<Renderer> pieceRenderer = new List<Renderer>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, spreadRadious);
            pieceRenderer.Add(child.GetComponent<Renderer>());
        }

        yield return new WaitForSeconds(destroyDelay);
        yield return Juicer.FadeOutAll(pieceRenderer, fadeOutTime, null);

        Destroy(gameObject);
    }
}
