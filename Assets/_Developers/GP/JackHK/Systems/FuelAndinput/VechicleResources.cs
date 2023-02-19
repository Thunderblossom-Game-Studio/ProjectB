using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VechicleResources : MonoBehaviour
{
    public static VechicleResources Instance { get; private set; }
    public float _burnRate;

    public Resource[] _resources;
    public float currentFuel;


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

    private void Update()
    {
        foreach (Resource resource in _resources)
        {
            if(resource.name == "Fuel")
            {
                currentFuel = resource._amount;
                //if (GameMenu.Instance)
                //{
                //    GameMenu.GetInstance().UpdateFuelSlider(GetCurrentFuelNormalized());
                //}
            }
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
            }
        }
    }
    
    public float GetCurrentFuelNormalized()
    {
        return currentFuel = currentFuel / 100;
    }

}

