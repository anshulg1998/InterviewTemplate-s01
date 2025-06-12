using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class TugOfWarGame : MonoBehaviour
{
    public Transform ropeObject; // The rope to move (centered at start)
    public Transform victoryPanel; // Player 1's object (left side)
    public float moveSpeed = 2f; // How fast the rope moves per tap difference
    public float winLimit = 3f; // How far from center to win (in world units)
    public TMP_Text victoryText;
    public Button lobbyButton;
    public TMP_Text timerText;


    private float ropePosition = 0f; // 0=center, +up, -down
    private bool gameActive = true;
    private float gameTimer = 30f;

    void Start()
    {
        victoryPanel.gameObject.SetActive(false);
        lobbyButton.onClick.AddListener(ReturnToLobby);
    }

    void Update()
    {
        if (!gameActive) return;

        // Timer countdown
        gameTimer -= Time.deltaTime;
        if (gameTimer < 0f) gameTimer = 0f;
        if (timerText) timerText.text = $"Time: {Mathf.CeilToInt(gameTimer)}";

        int effectiveDistance = 0;
        // Player 1 (A key)
        if (Input.GetKeyDown(KeyCode.A))
            effectiveDistance++;
        // Player 2 (D key)
        if (Input.GetKeyDown(KeyCode.D))
            effectiveDistance--;
        // Player 1 (Left mouse button)
        ropeObject.transform.localPosition += Vector3.up * effectiveDistance * moveSpeed * Time.deltaTime;

        // Check win
        if (ropeObject.transform.localPosition.y >= winLimit)
        {
            EndGame("Player 2 Wins!");
        }
        else if (ropeObject.transform.localPosition.y <= -winLimit)
        {
            EndGame("Player 1 Wins!");
        }
        else if (gameTimer <= 0f)
        {
            // Draw condition
            EndGame("Draw!");
        }
    }

    void EndGame(string winner)
    {
        gameActive = false;
        victoryText.text = winner;
        victoryPanel.gameObject.SetActive(true);
    }

    void ReturnToLobby()
    {
        StartCoroutine(co());

        IEnumerator co()
        {
            LoadingScreen.Instance.Show();
            GameObject g = new GameObject("CoroutineRunner");
            DontDestroyOnLoad(g);
            var runner = g.AddComponent<CoroutineRunner>();
            yield return new WaitForSeconds(0.5f); // Optional delay for loading screen visibility
            CustomSceneLoader.LoadSceneAsync(runner, "Lobby");
        }

    }
}
