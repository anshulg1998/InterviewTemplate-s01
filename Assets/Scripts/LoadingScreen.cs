using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;

public class LoadingScreen : GenericSingleton<LoadingScreen>
{
    public TMP_Text loadingText;
    public TMP_Text progressText;
    public Image progressBar;
    public GameObject loadingPanel;
    private Coroutine loadingAnimCoroutine;

    protected void Start()
    {
        Hide();
    }
    void OnEnable()
    {
        CustomSceneLoader.OnSceneLoaded += OnSceneLoaded;

    }

    void OnDisable()
    {
        CustomSceneLoader.OnSceneLoaded -= OnSceneLoaded;

    }
    public void Show()
    {
        CustomSceneLoader.OnProgress += SetProgress;
        loadingPanel.SetActive(true);
        if (loadingAnimCoroutine != null) StopCoroutine(loadingAnimCoroutine);
        loadingAnimCoroutine = StartCoroutine(LoadingTextAnimation());
        progressText.text = "0%";
        progressBar.fillAmount = 0f;
    }

    public void Hide()
    {
        CustomSceneLoader.OnProgress -= SetProgress;
        loadingPanel.SetActive(false);
        if (loadingAnimCoroutine != null) StopCoroutine(loadingAnimCoroutine);
    }

    private void OnSceneLoaded(string sceneName)
    {
        StartCoroutine(HideAfterDelay(2f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
    }

    private IEnumerator LoadingTextAnimation()
    {
        string baseText = "Loading";
        int dotCount = 0;
        while (true)
        {
            loadingText.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetProgress(float progress)
    {
        progressBar.DOFillAmount(progress, .1f);
        int percent = Mathf.RoundToInt(progress * 100f);
        progressText.text = percent + "%";
    }
}
