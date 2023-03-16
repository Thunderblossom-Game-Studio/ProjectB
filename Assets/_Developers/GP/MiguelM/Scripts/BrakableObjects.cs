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
    [SerializeField] private float destroytime = 9.7f;
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
        Debug.Log("Trigger with out if");
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

        yield return new WaitForSeconds(destroytime);

        StartCoroutine(Juicer.FadeOutAll(pieceRenderer, fadeOutTime, ()=> BreakableWall.SetActive(false)));
    }
}
