using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using Pelumi.Juicer;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }

    [SerializeField] private GameObject popUpPrefab;
    private ObjectPool<GameObject> popUpPPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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

    public void PopUpTextAtTransfrom(Transform spawnPosition, Vector3 randomIntensity, string text, Color color, bool parent = false)
    {
        GameObject textPopUp = popUpPPool.Get();
        if (parent) textPopUp.transform.SetParent(spawnPosition);
        SetPopUpInfo(textPopUp, spawnPosition.position, randomIntensity, text, color);
    }

    public void PopUpAtTextPosition(Vector3 spawnPosition, Vector3 randomIntensity, string text, Color color)
    {
        GameObject textPopUp = popUpPPool.Get();
        SetPopUpInfo(textPopUp, spawnPosition, randomIntensity, text, color);
    }

    public void SetPopUpInfo(GameObject textPopUp, Vector3 spawnPosition, Vector3 randomIntensity, string text, Color color)
    {
        textPopUp.transform.position = spawnPosition += new Vector3(Juicer.GetRange(randomIntensity.x), Juicer.GetRange(randomIntensity.y), Juicer.GetRange(randomIntensity.z));
        textPopUp.GetComponent<TextPopUp>().SetText(text, color);
        textPopUp.SetActive(true);
    }
}
