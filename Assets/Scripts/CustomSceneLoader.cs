using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class CustomSceneLoader
{
    public static event Action<string> OnSceneLoaded;
    public static event System.Action<float> OnProgress;

    private static MonoBehaviour coroutineRunner;
    public static void LoadSceneAsync(MonoBehaviour context, string sceneName)
    {
        coroutineRunner = context;
        context.StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private static IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOp.isDone)
        {
            OnProgress?.Invoke(asyncOp.progress);
            yield return null;
        }
        Debug.Log($"Scene '{sceneName}' loaded successfully.");
        OnProgress?.Invoke(1f);
        OnSceneLoaded?.Invoke(sceneName);
        if (coroutineRunner != null)
        {
            GameObject.Destroy(coroutineRunner.gameObject);
            coroutineRunner = null;
        }

    }
}
