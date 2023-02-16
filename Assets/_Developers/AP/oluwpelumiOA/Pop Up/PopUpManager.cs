using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    private ObjectPool<GameObject> popUpPPool;

    private void Start()
    {
        popUpPPool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy, true);
    }

    public GameObject OnCreate()
    {
        GameObject gameObject = Instantiate(popUpPrefab);
        if (gameObject.TryGetComponent(out PoolObject poolerObject)) poolerObject.AssignPooler(popUpPPool);
        return gameObject;
    }
    
    public void OnGet(GameObject gameObjectToGet)
    {

    }

    public void OnRelease(GameObject gameObjectToRelease)
    {
        gameObjectToRelease.SetActive(false);
    }

    public void OnDestroy(GameObject gameObjectToDestroy)
    {
        Destroy(gameObjectToDestroy);
    }

    public void PopUpText(GameObject textPopUp, Transform spawnPosition, Vector3 randomIntensity, string text, Color color, bool parent = false)
    {
        textPopUp.transform.position = spawnPosition.position += new Vector3(FeelUtility.GetRange(randomIntensity.x), FeelUtility.GetRange(randomIntensity.y), FeelUtility.GetRange(randomIntensity.z));
        if (parent) textPopUp.transform.SetParent(spawnPosition);
        textPopUp.GetComponent<TextMesh>().text = text;
        textPopUp.GetComponent<TextMesh>().color = color;
    }

    public void PopUpAtPosition(GameObject textPopUp, Vector3 spawnPosition, Vector3 randomIntensity, string text, Color color)
    {
        textPopUp.transform.position = spawnPosition += new Vector3(FeelUtility.GetRange(randomIntensity.x), FeelUtility.GetRange(randomIntensity.y), FeelUtility.GetRange(randomIntensity.z));
        textPopUp.GetComponent<TextMesh>().text = text;
        textPopUp.GetComponent<TextMesh>().color = color;
    }
}
