using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class WallDestroyer : MonoBehaviour
{
    [SerializeField] GameObject originalWall;
    [SerializeField] GameObject brokenWall;

    public float explosionForce = 500.0f;
    public float spreadRadious = 150.8f;

    float fadeSpeed = 7.0f;
    float fadeColourAmount;
    Material mat1;
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
        Color pieceColor = mat1.color;
        fadeColourAmount = mat1.color.a;
        foreach (Transform pieces in brokenWall.transform)
        {
            pieces.GetComponent<Renderer>().material.color = new Color(pieceColor.r, pieceColor.g, pieceColor.b, Mathf.Lerp(pieceColor.a, fadeColourAmount, fadeSpeed));
            mat1.color = pieces.GetComponent<Renderer>().material.color;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
