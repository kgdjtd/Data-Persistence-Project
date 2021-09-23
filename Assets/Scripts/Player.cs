using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{
    public static Player Instance;

    // current player data
    public string playerName;
    public float score;
    // recent player data
    public float recentScore;
    public string recentPlayer;
    // high score player data
    public float highScore;
    public string topPlayer;

    private void Awake()
    {

        // destroy duplicate instances
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Player.Instance.LoadPlayerData(); // check and load saved data if found


    }

    // sets current player name back to empty and score back to 0
    public void ResetCurrentPlayer()
    {
        playerName = "";
        score = 0;
    }

    // completely wipes all data in current player instance
    public void TotalReset()
    {
        ResetCurrentPlayer();
        recentPlayer = "";
        recentScore = 0;
        topPlayer = "";
        highScore = 0;
    }


    /*
     * SaveData class
     * 
     * Serializable class that will store any perstitent data 
     * 
     * PlayerName: the most recent player's name
     * Score: the most recent player's score
     * topPlayer: the player with the high score's name
     * topScore: the highest score earned so far
     */
    [System.Serializable]
    class SaveData
    {
        public string PlayerName; // most recent player
        public float Score; // most recent score
        public string topPlayer; // high score player
        public float topScore; // high score
    }

    /*
     * SavePlayerData
     * 
     * Called to save data that will persist after the game is exited
     */
    public void SavePlayerData()
    {
        // create an instance of save data
        // and insert most recent player's name and score
        SaveData data = new SaveData();


        data.PlayerName = playerName;
        data.Score = score;

        // if most recent score beats the current high score, 
        // overwrite high score data as well
        if (score > highScore)
        {
            data.topPlayer = playerName;
            data.topScore = score;
        }
        else // otherwise, keep the same data
        {
            data.topPlayer = Player.Instance.topPlayer;
            data.topScore = Player.Instance.highScore;
        }

        string json = JsonUtility.ToJson(data); // change data to JSON 

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json); // write data to file
    }

    /*
     * LoadPlayerData
     * 
     * Called when we want to load any saved player data from previous sessions
     */
    public void LoadPlayerData()
    {
        // find the file path
        string path = Application.persistentDataPath + "/savefile.json";

        // if file exists
        if (File.Exists(path))
        {
            // read the data into json
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json); // change it from json to our savedata class

            // fill current player instance with data
            recentPlayer = data.PlayerName;
            recentScore = data.Score;
            topPlayer = data.topPlayer;
            highScore = data.topScore;
        }
    }
}