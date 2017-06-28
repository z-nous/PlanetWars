using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        print("Starting new game");
        SceneManager.LoadScene(Constants.SCENE_GAME);
    }

    public void StartLevelEditor()
    {
        print("Starting Level editor");
        SceneManager.LoadScene(Constants.SCENE_MAPBUILDER);
    }

    public void ExitGame()
    {
        print("exit game");
        Application.Quit();
    }
}
