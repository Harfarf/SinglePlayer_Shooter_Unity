using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadSceneHandler : Singleton<LoadSceneHandler>
{
    private float initialDelay = 2f;
    private bool isLoading = false;
    private string loadingSceneName = "LoadScene";


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }





    public void LoadLoadingScene(string nextScene)
    {
        if (isLoading || string.IsNullOrEmpty(nextScene) || (nextScene == loadingSceneName)) return;
        StartCoroutine(LoadSceneCoroutine( SceneManager.GetActiveScene(), nextScene));
    }

    private IEnumerator LoadSceneCoroutine(Scene currentScene, string nextScene)
    {
        isLoading = true;

        //load the loading screen
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

        Scene loadingScene = SceneManager.GetSceneByName(loadingSceneName);
        SceneManager.SetActiveScene(loadingScene);

        yield return new WaitForSecondsRealtime(initialDelay);

        //unload the previous scene
        if (currentScene.IsValid() && currentScene.isLoaded) yield return SceneManager.UnloadSceneAsync(currentScene);

        //load and enable the final scene
        yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        Scene destinationScene = SceneManager.GetSceneByName(nextScene);
        if (destinationScene.IsValid() && destinationScene.isLoaded) SceneManager.SetActiveScene(destinationScene);

        //unload the loading screen
        yield return SceneManager.UnloadSceneAsync(loadingScene);

        isLoading = false;
    }

}
