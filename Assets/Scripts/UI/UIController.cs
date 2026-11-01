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


    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        playerController.OnDead += HandleGameOver;
        playerController.OnDeathsUpdated += UpdateDeathsUI;
        playerController.OnKillsUpdated += UpdateKillsUI;

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
        killsText.text = "Kills: " + amount.ToString();
    }
    private void UpdateDeathsUI(int amount)
    {
        deathsText.text = "Deaths: " + amount.ToString();
    }
    #endregion
}
