using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas dieCanvas;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().OnDead += HandleGameOver;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }




    private void HandleGameOver()
    {
        StartCoroutine(ShowDieCanvas());
    }
    private IEnumerator ShowDieCanvas()
    {
        dieCanvas.enabled = true;

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 0f; // Pause the game
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
    }
}
