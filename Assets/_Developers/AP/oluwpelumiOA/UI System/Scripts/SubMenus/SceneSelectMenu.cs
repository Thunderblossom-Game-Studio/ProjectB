using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pelumi.Juicer;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneSelectMenu : BaseMenu<SceneSelectMenu>
{
    public enum Direction { Left = -1, Right = 1 }

    public enum Scenes
    {
        Tutorial = 0,
        TempLevel1 = 1,
        TempLevel2 = 2,
        TempLevel3 = 3,
        TempLevel4 = 4,
    }
    public int[] levels;
    public int SelectedScene;
    [Header("Buttons")]
    [SerializeField] private AdvanceButton backButton;
    [SerializeField] private AdvanceButton leftButton;
    [SerializeField] private AdvanceButton rightButton;
    [SerializeField] private AdvanceButton mapButton;
    [SerializeField] private ImgSwap mapbuttonImage;
    [SerializeField] private CanvasGroup buttonHolder;
    [SerializeField] private TextMeshProUGUI LevelText;

    private System.Action<Direction> leftPressed;
    private System.Action<Direction> rightPressed;

    [SerializeField] private float animSpeed;
    private Transform TextStartpos;
    private Transform PicStartpos;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(CloseButton);
        leftButton.onClick.AddListener(() => SwitchMap(Direction.Left));
        rightButton.onClick.AddListener(() => SwitchMap(Direction.Right));
        mapButton.onClick.AddListener(() => SwitchScene((SceneType)SelectedScene + 1));
        SelectedScene = 0;
        TextStartpos = LevelText.transform;
        PicStartpos = mapButton.transform;
    }
    protected override void Instance_OnTabLeftAction(object sender, System.EventArgs e)
    {

    }

    protected override void Instance_OnTabRightAction(object sender, System.EventArgs e)
    {

    }

    protected override void Instance_OnBackAction(object sender, System.EventArgs e)
    {
        CloseButton();
    }

    public override IEnumerator OpenMenuRoutine(System.Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new JuicerFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut));

        yield return base.OpenMenuRoutine(onCompleted);
    }

    public override IEnumerator CloseMenuRoutine(System.Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut));

        yield return base.CloseMenuRoutine(onCompleted);
    }
    protected override void ResetUI()
    {
        buttonHolder.alpha = 0;
    }
    public void CloseButton()
    {
        Close(() => lastMenu.OpenMenu());
    }
    public void SwitchScene(SceneType sceneType)
    {
        Close(() => LoadingMenu.GetInstance().LoadScene((int)sceneType));
    }
    public void SwitchMap(Direction direction)
    {
        //int nextIndex = 0;
        switch (direction)
        {
            case Direction.Left:
                SelectedScene = levels[SelectedScene + 1 < levels.Length ? SelectedScene + 1 : 0];

                break;
            case Direction.Right:
                SelectedScene = levels[SelectedScene - 1 >= 0 ? SelectedScene - 1 : levels.Length - 1];
                break;
        }
        mapbuttonImage.ChangeImage(SelectedScene);
        switch (SelectedScene)
        {
            case 0:
                LevelText.text = ("tutorial");
                break;
            case 1:
                LevelText.text = ("Jurassic Parking ");
                break;
            case 2:
                LevelText.text = ("Midnight Rush");
                break;
            case 3:
                LevelText.text = ("Sandy Scramble");
                break;
            case 4:
                LevelText.text = ("Party tricks");
                break;
            default:
                break;
        }
        switch (direction)
        {
            case Direction.Left:
                StartCoroutine(Juicer.DoVector3(null, LevelText.transform.position + new Vector3(100, 0, 0), (pos) => LevelText.transform.position = pos,
                    new JuicerVector3Properties(TextStartpos.position, animSpeed, animationCurveType: AnimationCurveType.EaseInOut)));
                StartCoroutine(Juicer.DoVector3(null, mapButton.transform.position + new Vector3(200, 0, 0), (pos) => mapButton.transform.position = pos,
                    new JuicerVector3Properties(PicStartpos.position, animSpeed, animationCurveType: AnimationCurveType.EaseInOut)));
                break;
            case Direction.Right:
                StartCoroutine(Juicer.DoVector3(null, LevelText.transform.position - new Vector3(100, 0, 0), (pos) => LevelText.transform.position = pos,
                    new JuicerVector3Properties(TextStartpos.position, animSpeed, animationCurveType: AnimationCurveType.EaseInOut)));
                StartCoroutine(Juicer.DoVector3(null, mapButton.transform.position - new Vector3(200, 0, 0), (pos) => mapButton.transform.position = pos,
                    new JuicerVector3Properties(PicStartpos.position, animSpeed, animationCurveType: AnimationCurveType.EaseInOut)));
                break;
        }
    }
}
public enum SceneType
{
    MainMenu = 0,
    Tutorial = 1,
    TempLevel1 = 2,
    TempLevel2 = 3,
    TempLevel3 = 4,
    TempLevel4 = 5,
    Credits = 6,
    MultiplayerMenu = 7,
    LobbyScene = 8,
}