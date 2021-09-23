using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{


    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;


    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private string pName;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        // save player name in variable
        pName = Player.Instance.playerName;

        // set score text line
        ScoreText.text = $"{pName} Score : ";

        // set best score text line
        UpdateHighScore(Player.Instance.topPlayer, Player.Instance.highScore);

    }

    /*
     * SetName
     * 
     * Parameters: 
     *  - string: name of the player
     *  
     *  Sets the best score text
     *  
     *  IMPORTANT: not currently in use, possibly for setting high score.
     */
    void SetName(string name)
    {
        BestScoreText.text += name;
    }

    private void Update()
    {
        if (!m_Started)
        {
            // when space is pressed, start the game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            GameOverText.SetActive(true);
            ScoreText.enabled = false;
            BestScoreText.enabled = false;

            Player.Instance.score = m_Points; // saves the final score to the player instance
            Player.Instance.SavePlayerData(); // saves the most recent player
            Player.Instance.LoadPlayerData();
            // if game over and space is pressed again, start the game again
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            // else if game over and q is pressed, quit
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Player.Instance.ResetCurrentPlayer();
                SceneManager.LoadScene(0);
            }
        }
    }

    /*
     * AddPoint
     * 
     * Parameters:
     *  - point: integer representing point earned
     *  
     *  Adds points earned to total points
     */
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{pName} Score : {m_Points}";

        if (m_Points > Player.Instance.highScore)
        {
            UpdateHighScore(pName, m_Points);
        }
    }

    /*
     * UpdateHighScore
     * 
     * Parameters:
     *  name - string representing high score player's name
     *  score - float representing high score
     *  
     *  Updates the high score text field
     */
    public void UpdateHighScore(string name, float score)
    {
        BestScoreText.text = $"Best Score : {name} : {score}";
    }

    /*
     * GameOver
     * 
     * ends game and activates game over text
     */
    public void GameOver()
    {
        m_GameOver = true;
    }


}
