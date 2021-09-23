using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]

public class MenuUIHandler : MonoBehaviour
{
    // input field for player name
    public InputField playerName;
    public GameObject recentScore; // recent score text field
    public GameObject highScore; // high score text field

    /*
     * NameEntered
     * 
     * Parameters:
     *  - name: player name entered
     *  
     *  Sets the players name for this instance
     */
    public void NameEntered(string name)
    {
        Player.Instance.playerName = name;
    }

    // Start is called before the first frame update
    void Start()
    {
        // add listener to text field
        playerName.onValueChanged.AddListener(delegate { NameEntered(playerName.text); });
        // if player name text field is left empty,
        // change it to default
        if (playerName.text == "")
        {
            Player.Instance.playerName = "[no name]";
        }

    }

    /*
     * Awake
     * 
     * Updates the recent score and high score text fields when scene awakens
     */
    private void Awake()
    {
        // if a recent game data exists, fill text field
        if (Player.Instance.recentPlayer != "")
        {
            recentScore.GetComponent<Text>().text = $"{Player.Instance.recentPlayer}: {Player.Instance.recentScore}";
        }

        // if high score exists, fill text field
        if (Player.Instance.highScore > 0)
        {
            highScore.GetComponent<Text>().text = $"{Player.Instance.topPlayer}: {Player.Instance.highScore}";
        }
    }

    /*
     * StartNew
     * 
     * Starts the main scene when the start button is presssed
     */
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    /*
     * ResetScores
     * 
     * Resets the recent and high scores when reset button is pressed
     */
    public void ResetScores()
    {
        string path = Application.persistentDataPath + "/savefile.json"; // file path

        // if file path exists
        if (File.Exists(path))
        {
            File.Delete(path); // delete path

            // call the refresh editor function
            RefreshEditor();

            // erase the player instance of all data
            Player.Instance.TotalReset();
            // refresh the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /*
     *  RefreshEditor
     *  
     *  if using the unity editor to play, refresh its database
     */
    void RefreshEditor()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    /*
     * Quit
     * 
     * Exits the game when the quit button is pressed
     */
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif 
    }


}
