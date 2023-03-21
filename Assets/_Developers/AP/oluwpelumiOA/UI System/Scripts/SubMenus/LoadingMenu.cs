using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pelumi.Juicer;

public class LoadingMenu : BaseMenu<LoadingMenu>
{
    [SerializeField] private CanvasGroup loadingContent;
    
    [SerializeField] float amountToLoad;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] Image loadingBar;

    [Header("Tips")]
    [SerializeField] TextMeshProUGUI tipsText;
    [SerializeField] float tipChangeTime = 5f;
    [TextArea(3, 3)]
    [SerializeField] string[] tips;

    protected void Start()
    {
        StartCoroutine(OpenMenuRoutine());
    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsychronously(sceneIndex));
    }
    
    IEnumerator LoadAsychronously(int sceneIndex)
    {
        ShowRandomTip();

        loadingBar.fillAmount = 0;
        loadingText.text = "Loading";

        yield return Juicer.DoFloat(null,0, (v) => loadingContent.alpha = v, new JuicerFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut), ()=> ToggleLoadingContent(true));

        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(sceneIndex);
        sceneToLoad.allowSceneActivation = false;

        float tipTimer = 0;
        while (!sceneToLoad.isDone && loadingBar.fillAmount < 1)
        {
            loadingBar.fillAmount += amountToLoad * Time.unscaledDeltaTime;
            loadingText.text = "Loading " + (int)(loadingBar.fillAmount * 100);

            RandomTipTimer(ref tipTimer);

            yield return null;
        }

        sceneToLoad.allowSceneActivation = true;

        yield return new WaitForSecondsRealtime(.15f);

        yield return Juicer.DoFloat(null,1, (v) => loadingContent.alpha = v, new JuicerFloatProperties(0, .5f, animationCurveType: AnimationCurveType.EaseInOut), () => ToggleLoadingContent(false));
    }

    public void ShowRandomTip()
    {
        string randomTip = tips[UnityEngine.Random.Range(0, tips.Length)];
        tipsText.text = randomTip;
    }

    void RandomTipTimer(ref float tipTimer)
    {
        if (tipTimer >= tipChangeTime)
        {
            ShowRandomTip();
            tipTimer = 0;
        }
        else
        {
            tipTimer += Time.unscaledDeltaTime;
        }
    }

    void ToggleLoadingContent(bool value)
    {
        loadingContent.interactable = value;
        loadingContent.blocksRaycasts = value;
    }
}
