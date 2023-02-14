using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarSelectMenu : BaseMenu<CarSelectMenu>
{
    public enum Direction { Left = -1, Right = 1 }

    [Header("UI")]
    [SerializeField] private AdvanceButton closeButton;
    [SerializeField] private CanvasGroup buttonHolder;

    [Header("Car Select")]
    [SerializeField] private float animSpeed;
    [SerializeField] private float carRotateSpeed;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Transform carLeftSpawnPos;
    [SerializeField] private Transform carRightSpawnPos;
    [SerializeField] private Transform carDestinationPos;
    [SerializeField] private AdvanceButton leftButton;
    [SerializeField] private AdvanceButton rightButton;

    private GameObject selectedCar;
    private bool canRotate;
    
    private void Start()
    {
        closeButton.onClick.AddListener(CloseButton);
        leftButton.onClick.AddListener(() => SwitchCar(Direction.Left));
        rightButton.onClick.AddListener(() => SwitchCar(Direction.Right));
        selectedCar = cars[0];
        canRotate = true;
    }

    public override IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeFloat(null, 0, (pos) => buttonHolder.alpha = pos, new FeelFloatProperties(1, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        yield return base.OpenMenuRoutine(OnComplected);
    }

    public override IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeFloat(null, buttonHolder.alpha, (pos) => buttonHolder.alpha = pos, new FeelFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
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
        RotateSelectedCar();
    }

    public void CloseButton()
    {
        Close(() => lastMenu.OpenMenu());
    }

    public void SwitchCar(Direction direction)
    {
        selectedCar.SetActive(false);
        int nextIndex = 0;
        switch (direction)
        {
            case Direction.Left:
                nextIndex = Array.IndexOf(cars, selectedCar) + 1;
                selectedCar = cars[nextIndex < cars.Length ? nextIndex : 0];
                break;
            case Direction.Right:
                nextIndex = Array.IndexOf(cars, selectedCar) - 1;
                selectedCar =  cars[nextIndex >= 0 ? nextIndex : cars.Length - 1];
                break;
        }
        selectedCar.SetActive(true);
        switch (direction)
        {
            case Direction.Left:
                StartCoroutine(FeelUtility.FadeVector3(DisableRotate, carRightSpawnPos.position,  (pos) => selectedCar.transform.position = pos,
                    new FeelVector3Properties(carDestinationPos.position, 
                    animSpeed, animationCurveType: AnimationCurveType.EaseInOut), EnableRotate));
                break;
            case Direction.Right:
                StartCoroutine(FeelUtility.FadeVector3(DisableRotate, carLeftSpawnPos.position, (pos) => selectedCar.transform.position = pos,
                    new FeelVector3Properties(carDestinationPos.position, animSpeed, animationCurveType: AnimationCurveType.EaseInOut), EnableRotate));
                break;
        }
    }

    public void EnableRotate() => canRotate = true;


    public void DisableRotate() => canRotate = false;

    public void RotateSelectedCar()
    {
        if(selectedCar && canRotate)  selectedCar.transform.Rotate(Vector3.up, carRotateSpeed * Time.deltaTime);
    }
}
