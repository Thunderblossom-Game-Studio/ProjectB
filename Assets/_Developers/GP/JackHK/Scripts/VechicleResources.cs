using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VechicleResources : MonoBehaviour
{
    public static VechicleResources Instance { get; private set; }
    public float _burnRate;

    public Resource[] _resources;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (Resource resource in _resources)
        {
            resource._amount = resource._startingAmount;
        }
    }

    public void GetResourceName(int index)
    {
        Debug.Log(_resources[index]._name);
    }

    public void IncreaseResource(string name, float amount)
    {
        foreach (Resource resource in _resources)
        {
            if (resource._name == name)
            {
                resource._amount += amount;
                if (resource._amount > resource._maxAmount)
                {
                    resource._amount = resource._maxAmount;
                }
                UpdateSlider(resource);
            }
        }
    }

    public void DecreaseResource(string name, float amount)
    {
        foreach (Resource resource in _resources)
        {
            if (resource._name == name)
            {
                resource._amount -= amount;
                if (resource._amount < 0 && !resource._canBeNegative)
                {
                    resource._amount = 0;
                }
                UpdateSlider(resource);
            }
        }
    }

    public void BurnResource(string name, float burnRate)
    {
        foreach (Resource resource in _resources)
        {
            if (resource._name == name)
            {
                resource._amount -= burnRate * Time.deltaTime;
                if (resource._amount < 0 && !resource._canBeNegative)
                {
                    resource._amount = 0;
                }
                UpdateSlider(resource);
            }
        }
    }

    private void UpdateSlider(Resource resource)
    {
        resource._resourceBar.value = resource._amount / resource._maxAmount;
    }
}

