using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string sceneNameToBeLoaded;
    public void LoadScene(string _sceneName)
    {
        sceneNameToBeLoaded = _sceneName;
        StartCoroutine(InitializeSceneLoading(1));
    }

    public void LoadMyScene(string _sceneName)
    {
        sceneNameToBeLoaded = _sceneName;
        StartCoroutine(InitializeSceneLoading(2));
    }
    
    IEnumerator InitializeSceneLoading(int scene)
    {
        string sceneToLoad;
        sceneToLoad = scene == 1 ? "Scene_Loading" : "My_Scene_Loading";

        // First, we load the loading scene
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        // Load the actual scene
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);

        // This value stops the scene from displaying when it is still loading
        asyncSceneLoading.allowSceneActivation = false;

        while(!asyncSceneLoading.isDone)
        {
            if(asyncSceneLoading.progress >= 0.9f)
            {
                asyncSceneLoading.allowSceneActivation = true;
            }

            yield return null;

        }
    }
}
