using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void LoadSceneByID(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
