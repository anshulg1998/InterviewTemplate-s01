using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LobbyPlayButton : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        playButton.interactable = false;
        // Download the addressable GameObject from server
        Addressables.LoadAssetAsync<GameObject>("YourAddressableKeyHere").Completed += OnAddressableLoaded;
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnAddressableLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Optionally instantiate or cache the loaded GameObject here
            // GameObject loadedObj = GameObject.Instantiate(handle.Result);
            playButton.interactable = true;
        }
        else
        {
            Debug.LogError("Failed to load addressable GameObject.");
        }
    }

    void OnPlayClicked()
    {
        StartCoroutine(LoadGameplayScene());

        IEnumerator LoadGameplayScene()
        {
            LoadingScreen.Instance.Show();
            GameObject g = new GameObject("CoroutineRunner");
            DontDestroyOnLoad(g);
            var runner = g.AddComponent<CoroutineRunner>();
            yield return new WaitForSeconds(0.5f); // Optional delay for loading screen visibility
            CustomSceneLoader.LoadSceneAsync(runner, "Gameplay");
        }

    }
}
