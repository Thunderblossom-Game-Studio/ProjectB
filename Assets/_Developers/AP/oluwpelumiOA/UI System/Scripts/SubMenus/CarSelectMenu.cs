using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Pelumi.Juicer;

public class CarSelectMenu : BaseMenu<CarSelectMenu>
{

    public enum SelectMode { Car, Weapon }

    public enum Direction { Left = -1, Right = 1 }

    [SerializeField] private SelectMode selectMode;

    [Header("UI")]
    [SerializeField] private AdvanceButton closeButton;
    [SerializeField] private CanvasGroup buttonHolder;
    [SerializeField] private AdvanceButton selectButton;

    [Header("Car Select")]
    [SerializeField] private float animSpeed;
    [SerializeField] private float carRotateSpeed;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Transform carLeftSpawnPos;
    [SerializeField] private Transform carRightSpawnPos;
    [SerializeField] private Transform carDestinationPos;
    [SerializeField] private AdvanceButton leftButton;
    [SerializeField] private AdvanceButton rightButton;

    [Header("Weapon Select")]
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private Transform weaponDestinationPos;

    private GameObject selectedCar;
    private GameObject selectedWeapon;
    private bool canRotate;
    private Action<Direction> leftPressed;
    private Action<Direction> rightPressed;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseButton);
        leftButton.onClick.AddListener(() => leftPressed?.Invoke(Direction.Left));
        rightButton.onClick.AddListener(() => rightPressed?.Invoke(Direction.Right));
        selectButton.onClick.AddListener(SelectButton);
        selectedCar = cars[0];
        canRotate = true;
        SwithMode(SelectMode.Car);
    }

    public override IEnumerator OpenMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, 0, (pos) => buttonHolder.alpha = pos, new JuicerFloatProperties(1, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        yield return base.OpenMenuRoutine(onCompleted);
    }

    public override IEnumerator CloseMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, buttonHolder.alpha, (pos) => buttonHolder.alpha = pos, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        yield return base.CloseMenuRoutine(onCompleted);
    }

    protected override void Instance_OnTabLeftAction(object sender, EventArgs e)
    {
        leftPressed?.Invoke(Direction.Left);
    }

    protected override void Instance_OnTabRightAction(object sender, EventArgs e)
    {
        rightPressed?.Invoke(Direction.Right);
    }

    private void Update()
    {
        RotateSelectedCar();
    }

    public void SelectButton()
    {
        switch (selectMode)
        {
            case SelectMode.Car: SwithMode(SelectMode.Weapon); break;
            case SelectMode.Weapon: break;
        }
    }

    public void CloseButton()
    {
        switch (selectMode)
        {
            case SelectMode.Car: Close(() => lastMenu.OpenMenu()); break;
            case SelectMode.Weapon: ExitWeaponSelect(); break;
        }     
    }

    public void SwithMode(SelectMode mode)
    {
        selectMode = mode;
        switch (selectMode)
        {
            case SelectMode.Car: OnEnterCarSelect(); break;
            case SelectMode.Weapon: OnEnterWeaponSelect(); break;
        }
    }

    public void OnEnterCarSelect()
    {
        selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT CAR";
        leftPressed = SwitchCar;
        rightPressed = SwitchCar;
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
                selectedCar = cars[nextIndex >= 0 ? nextIndex : cars.Length - 1];
                break;
        }
        selectedCar.SetActive(true);
        switch (direction)
        {
            case Direction.Left:
                StartCoroutine(Juicer.DoVector3(DisableRotate, carRightSpawnPos.position, (pos) => selectedCar.transform.position = pos,
                    new JuicerVector3Properties(carDestinationPos.position,
                    animSpeed, animationCurveType: AnimationCurveType.EaseInOut), EnableRotate));
                break;
            case Direction.Right:
                StartCoroutine(Juicer.DoVector3(DisableRotate, carLeftSpawnPos.position, (pos) => selectedCar.transform.position = pos,
                    new JuicerVector3Properties(carDestinationPos.position, animSpeed, animationCurveType: AnimationCurveType.EaseInOut), EnableRotate));
                break;
        }
    }

    public void RotateSelectedCar()
    {
        if (selectedCar && canRotate) selectedCar.transform.Rotate(Vector3.up, carRotateSpeed * Time.deltaTime);
    }

    public void EnableRotate() => canRotate = true;


    public void DisableRotate() => canRotate = false;

    public void OnEnterWeaponSelect()
    {
        selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT WEAPON";
        leftPressed = SwitchWeapon;
        rightPressed = SwitchWeapon;
        selectedWeapon = weapons[0];
        selectedWeapon.SetActive(true);
        weaponDestinationPos = selectedCar.transform.Find("Turrent Slot");
        OnWeponReachRoot();
    }

    public void SwitchWeapon(Direction direction)
    {
        selectedWeapon.SetActive(false);
        int nextIndex = 0;
        switch (direction)
        {
            case Direction.Left:
                nextIndex = Array.IndexOf(weapons, selectedWeapon) + 1;
                selectedWeapon = weapons[nextIndex < cars.Length ? nextIndex : 0];
                break;
            case Direction.Right:
                nextIndex = Array.IndexOf(weapons, selectedWeapon) - 1;
                selectedWeapon = weapons[nextIndex >= 0 ? nextIndex : weapons.Length - 1];
                break;
        }
        selectedWeapon.SetActive(true);
        switch (direction)
        {
            case Direction.Left:
                StartCoroutine(Juicer.DoVector3(null, carRightSpawnPos.position, (pos) => selectedWeapon.transform.position = pos,
                    new JuicerVector3Properties(weaponDestinationPos.position,
                    animSpeed, animationCurveType: AnimationCurveType.EaseInOut), OnWeponReachRoot));
                break;
            case Direction.Right:
                StartCoroutine(Juicer.DoVector3(null, carLeftSpawnPos.position, (pos) => selectedWeapon.transform.position = pos,
                    new JuicerVector3Properties(weaponDestinationPos.position, animSpeed, animationCurveType: AnimationCurveType.EaseInOut), OnWeponReachRoot));
                break;
        }
    }

    public void OnWeponReachRoot()
    {
        selectedWeapon.transform.SetParent(weaponDestinationPos);
        selectedWeapon.transform.localPosition = Vector3.zero;
        selectedWeapon.transform.localRotation = Quaternion.identity;
    }

    public void ExitWeaponSelect()
    {
        selectedWeapon?.SetActive(false);
        selectedWeapon = null;
        SwithMode(SelectMode.Car);
    }

    public void OnWeaponSelectComplete()
    {
        Close(() => lastMenu.OpenMenu());
    }
}
