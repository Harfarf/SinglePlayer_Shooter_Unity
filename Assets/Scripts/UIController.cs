using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas dieCanvas;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().OnDead += ShowDieCanvas;
    }

    private void Update()
    {
        if (InputManager.Instance.GetInput(InputCommand.Retry))
        {
            Debug.Log("Retrying...");
            SceneManager.LoadScene(1);
        }
    }



    private void ShowDieCanvas()
    {
        dieCanvas.gameObject.SetActive(true);
    }
}
