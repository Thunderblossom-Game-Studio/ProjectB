using System.Collections;
using UnityEngine;

public class BrakableObjects : MonoBehaviour
{
    //[SerializeField] GameObject originalWall;
    //[SerializeField] GameObject brokenWall;
    //[SerializeField] Material targetColor;
    //[SerializeField] Color currentColor;
    //[SerializeField] Color targetColor;
    //List<MeshRenderer> pieceRenderer;
    [SerializeField] GameObject OriginalWall;
    [SerializeField] GameObject BreakableWall;
    public bool isItdestroyed = false;
    GameObject secondChildObject;

    public float explosionForce = 500.0f;
    public float spreadRadious = 150.8f;
    public float destroytime = 9.7f;

    //public float destroyTime = 3.0f;
    //public float fadeTime = 3.0f;
    // Update is called once per frame

    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Controllable Car") && !isItdestroyed)
        {
            OriginalWall.SetActive(false);
            BreakableWall.SetActive(true);
            Debug.Log("Tag worked and bollean Worked");
            ExplodeWall();
        }
        Debug.Log("Trigger with out if");
    }

    public void ExplodeWall()
    {
        foreach (Transform child in BreakableWall.transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, BreakableWall.transform.position, spreadRadious);
            //pieceRenderer.Add(child.GetComponent<MeshRenderer>()); 
        }
        StartCoroutine(DestroyPieces());
    }

    public IEnumerator DestroyPieces()
    {
        yield return new WaitForSeconds(destroytime);
        BreakableWall.SetActive(false);
        isItdestroyed = true;
        Debug.Log(isItdestroyed);
    }
    //public IEnumerator FadePieces()
    //{

    //    Color pieceInitialColor;
    //    Color pieceTargetColor;

    //    float startTime = 0.0f;
    //    float endTime = 1.0f;

    //    for (int i = 0; i < pieceRenderer.Count; i++)
    //    {
    //        Debug.Log("First Loop");
    //        pieceInitialColor = pieceRenderer[i].material.color;
    //        pieceTargetColor = new Color(pieceInitialColor.r, pieceInitialColor.g, pieceInitialColor.b, 0.0f);
    //        foreach (var pieceRenderer in pieceRenderer)
    //        {
    //            Debug.Log("Second Loop");
    //            while (startTime < endTime)
    //            {
    //                Debug.Log("Third Loop");
    //                startTime += Time.deltaTime;
    //                pieceRenderer.material.color = Color.Lerp(pieceInitialColor, pieceTargetColor, startTime);
    //                yield return null;
    //            }
    //            yield return new WaitForSeconds(1.0f);
    //        }
    //    }
    //}

    //private IEnumerator FadeColor(List<MeshRenderer> objectRenderer,Color endColor, float fadeDuration)
    //{
    //    //brokenWall.gameObject.SetActive(false);
    //    float currentDuration = 0;
    //    while (currentDuration < fadeDuration)
    //    {
    //        currentDuration += Time.deltaTime;
    //        for (int i = 0; i < objectRenderer.Count; i++)
    //        {
    //            objectRenderer[i].material.color = Color.Lerp
    //            (objectRenderer[i].material.color, endColor, fadeTime * Time.deltaTime);
    //        }
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(destroyTime);
    //    brokenWall.SetActive(false);
    //}
    //public IEnumerator FadeOutAll(List<MeshRenderer> renderers, float duration)
    //{
    //    List<Color> initialColors = new List<Color>();
    //    List<Color> targetColors = new List<Color>();

    //    // Get initial and target colors for each renderer
    //    foreach (Renderer ren in renderers)
    //    {
    //        initialColors.Add(ren.material.color);
    //        targetColors.Add(new Color(initialColors[initialColors.Count - 1].r, initialColors[initialColors.Count - 1].g, initialColors[initialColors.Count - 1].b, 0f));
    //    }

    //    float rateOfChange = 1f / duration;
    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime * rateOfChange;

    //        // Update the color of each renderer
    //        for (int i = 0; i < renderers.Count; i++)
    //        {
    //            Color newColor = Color.Lerp(initialColors[i], targetColors[i], t);
    //            renderers[i].material.color = newColor;
    //        }

    //        yield return null;
    //    }

    //    // Set the color of each renderer to the target color
    //    for (int i = 0; i < renderers.Count; i++)
    //    {
    //        renderers[i].material.color = targetColors[i];
    //    }
    //}
}
