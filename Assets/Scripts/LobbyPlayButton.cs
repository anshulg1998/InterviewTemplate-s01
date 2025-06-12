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
        string catalogURL = "https://github.com/anshulg1998/InterviewTemplate-s01/catalog.json";

        Addressables.LoadContentCatalogAsync(catalogURL).Completed += (catalogHandle) =>
        {
            if (catalogHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Addressables.InstantiateAsync("Cube").Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        GameObject obj = handle.Result;
                        obj.transform.position = new Vector3(0, 1, 0);
                        obj.transform.rotation = Quaternion.identity;
                    }
                    else
                    {
                        Debug.LogError("Failed to instantiate prefab.");
                    }
                };
            }
            else
            {
                Debug.LogError("Failed to load catalog.");
            }
            playButton.interactable = true;
        };

        // Download the addressable GameObject from server
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
