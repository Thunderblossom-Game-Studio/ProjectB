using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakableObjects : MonoBehaviour
{
    [SerializeField] GameObject originalWall;
    [SerializeField] GameObject brokenWall;

    public float explosionForce = 500.0f;
    public float spreadRadious = 150.8f;

    public float destroyTime = 1.5f;
    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            originalWall.SetActive(false);
            brokenWall.SetActive(true);
            ExplodeWall();
        }
    }

    public void ExplodeWall()
    {
        foreach (Transform child in brokenWall.transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, brokenWall.transform.position, spreadRadious);
        }
        StartCoroutine(FadePieces());
    }

    public IEnumerator FadePieces()
    {
        yield return new WaitForSeconds(destroyTime);
        brokenWall.gameObject.SetActive(false);


    }
}
