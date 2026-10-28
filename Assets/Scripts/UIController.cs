using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas dieCanvas;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI deathsText;

    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().OnDead += HandleGameOver;
        player.GetComponent<PlayerController>().OnDeathsUpdated += UpdateDeathsUI;
        player.GetComponent<PlayerController>().OnKillsUpdated += UpdateKillsUI;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    #region GameOver UI
    private void HandleGameOver()
    {
        StartCoroutine(ShowDieCanvas());
    }
    private IEnumerator ShowDieCanvas()
    {
        dieCanvas.enabled = true;

        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 0f; //pause the game
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f; //un-pause the game
    }
    #endregion

    #region Gameplay UI
    private void UpdateKillsUI(int amount)
    {
        killsText.text = amount.ToString();
    }
    private void UpdateDeathsUI(int amount)
    {
        deathsText.text = amount.ToString();
    }
    #endregion
}
