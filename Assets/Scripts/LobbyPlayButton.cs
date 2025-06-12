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
        Addressables.LoadContentCatalogAsync("https://github.com/anshulg1998/InterviewTemplate-s01/catalog.json")
        .Completed += (handle) =>
        {
            Addressables.InstantiateAsync("Cube").Completed += (handle) =>
       {
           if (handle.Status == AsyncOperationStatus.Succeeded)
           {
               GameObject obj = handle.Result;
               obj.transform.position = new Vector3(0, -2, 0); // Set spawn position
               obj.transform.rotation = Quaternion.Euler(-30, -40, 25);
           }
           else
           {
               Debug.LogError("Failed to instantiate prefab.");
           }
       };
            playButton.onClick.AddListener(OnPlayClicked);

        };
        // Download the addressable GameObject from server
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
