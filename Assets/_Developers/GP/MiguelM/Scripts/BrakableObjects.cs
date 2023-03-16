using Pelumi.Juicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakableObjects : MonoBehaviour
{
    [SerializeField] private GameObject OriginalWall;
    [SerializeField] private GameObject BreakableWall;
    [SerializeField] private bool isItdestroyed = false;
    [SerializeField] private float explosionForce = 500.0f;
    [SerializeField] private float spreadRadious = 150.8f;

    [Header("Destroy")]
    [SerializeField] private bool destroyAfterHit;
    [SerializeField] private float destroyTime = 9.7f;
    [SerializeField] private float fadeOutTime = 3f;

    [Viewable]  [SerializeField] private List<Renderer> pieceRenderer = new List<Renderer>();

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Controllable Car") && !isItdestroyed)
        {
            Explode();
        }
    }

    public void Explode()
    {
        isItdestroyed = true;
        OriginalWall.SetActive(false);
        BreakableWall.SetActive(true);
        StartCoroutine(ExplodeWall());
    }

    public IEnumerator ExplodeWall()
    {
        foreach (Transform child in BreakableWall.transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, BreakableWall.transform.position, spreadRadious);
            pieceRenderer.Add(child.GetComponent<Renderer>()); 
        }

        if(destroyAfterHit)
        {
            yield return new WaitForSeconds(destroyTime);
            StartCoroutine(Juicer.FadeOutAll(pieceRenderer, fadeOutTime, () => BreakableWall.SetActive(false)));
        }
    }
}
