using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string SceneNameToBeLoaded;
	private string LoadingScreen = "Scene_Loading";


    public void LoadScene(string _sceneName)
    {
		SceneNameToBeLoaded = _sceneName;
        StartCoroutine(InitializeSceneLoading(1));
    }

    public void LoadMyScene(string _sceneName)
    {
		SceneNameToBeLoaded = _sceneName;
        StartCoroutine(InitializeSceneLoading(2));
    }
    
    IEnumerator InitializeSceneLoading(int scene)
    {
        yield return SceneManager.LoadSceneAsync(LoadingScreen);
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(SceneNameToBeLoaded);

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
