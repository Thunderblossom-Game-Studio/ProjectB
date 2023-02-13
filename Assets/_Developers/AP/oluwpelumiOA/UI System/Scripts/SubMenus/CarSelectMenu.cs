using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarSelectMenu : BaseMenu<CarSelectMenu>
{
    public enum Direction { Left = -1, Right = 1 }
    
    [SerializeField] private CanvasGroup buttonHolder;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Transform carLeftSpawnPos;
    [SerializeField] private Transform carRightSpawnPos;
    [SerializeField] private Transform carDestinationPos;
    [SerializeField] private AdvanceButton leftButton;
    [SerializeField] private AdvanceButton rightButton;

    private void Start()
    {
        leftButton.onClick.AddListener(() => SwitchCar(Direction.Left));
        rightButton.onClick.AddListener(() => SwitchCar(Direction.Right));
    }

    public override IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeCanvasGroup(buttonHolder, new FeelFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut));
        yield return base.OpenMenuRoutine(OnComplected);
    }

    public override IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeCanvasGroup(buttonHolder, new FeelFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut));
        yield return base.CloseMenuRoutine(OnComplected);
    }

    protected override void Instance_OnTabLeftAction(object sender, EventArgs e)
    {
        SwitchCar(Direction.Left);
    }

    protected override void Instance_OnTabRightAction(object sender, EventArgs e)
    {
        SwitchCar(Direction.Right);
    }

    private void Update()
    {
    }

    public void CloseButton()
    {
        Close(() => lastMenu.OpenMenu());
    }

    public void SwitchCar(Direction direction)
    {
        GameObject carObject = cars.FirstOrDefault(car => car.activeInHierarchy);
        GameObject carToEnable = null;
        carObject.SetActive(false);
        int nextIndex = 0;
        switch (direction)
        {
            case Direction.Left:
                nextIndex = Array.IndexOf(cars, carObject) + 1;
                carToEnable = cars[nextIndex < cars.Length ? nextIndex : 0];
                break;
            case Direction.Right:
                nextIndex = Array.IndexOf(cars, carObject) - 1;
                carToEnable =  cars[nextIndex >= 0 ? nextIndex : cars.Length - 1];
                break;
        }
        carToEnable.SetActive(true);
        switch (direction)
        {
            case Direction.Left:
                StartCoroutine(FeelUtility.FadeVector3(carRightSpawnPos.position, (pos) => carToEnable.transform.position = pos, new FeelVector3Properties(carDestinationPos.position, .2f, animationCurveType: AnimationCurveType.EaseInOut)));
                break;
            case Direction.Right:
                StartCoroutine(FeelUtility.FadeVector3(carLeftSpawnPos.position, (pos) => carToEnable.transform.position = pos, new FeelVector3Properties(carDestinationPos.position, .2f, animationCurveType: AnimationCurveType.EaseInOut)));
                break;
        }
    }
}
