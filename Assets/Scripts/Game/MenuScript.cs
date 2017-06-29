using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    private bool IsMenuVisible = true;
    private MapHandler maphandler = new MapHandler();


    private enum MenuMode
    {
        mainmenu,
        game,
        mapeditor
    }

    private MenuMode menumode;

    void Start () {
        switch (SceneManager.GetActiveScene().name)
        {
            case Constants.SCENE_GAME:
                //Button 1 pressed;
                menumode = MenuMode.game;
                print("game mode");
                break;
            case Constants.SCENE_MAINMENU:
                menumode = MenuMode.mainmenu;
                print("Main menu mode");
                break;
            case Constants.SCENE_MAPBUILDER:
                menumode = MenuMode.mapeditor;
                print("Map editor mode");
                maphandler.LoadMapForEditor("Map1");
                break;
            default:
                break;
        }
        
	}
	
	// Update is called once per frame
	void Update () {

    }

    //Private functions
    //how to behave in main menu
    private void MainMenuMode(int ButtonNumber)
    {
        switch (ButtonNumber)
        {
            case 1:
                //Button 1 pressed;
                menumode = MenuMode.game;
                SceneManager.LoadScene(Constants.SCENE_GAME);
                break;
            case 2:
                menumode = MenuMode.mapeditor;
                SceneManager.LoadScene(Constants.SCENE_MAPBUILDER);
                break;
            case 3:
                //Button 3 pressed;
                Application.Quit();
                break;
            default:
                break;
        }
    }
    //How to behave in game mode
    private void GameMode(int ButtonNumber)
    {
        switch (ButtonNumber)
        {
            case 1:
                //Button 1 pressed;
                break;
            case 2:
                //Button 2 pressed;
                break;
            case 3:
                menumode = MenuMode.mainmenu;
                SceneManager.LoadScene(Constants.SCENE_MAINMENU);
                break;
            default:
                break;
        }
    }
    //How to behave in Map editor mode
    private void MapEditorMode(int ButtonNumber)
    {
        switch (ButtonNumber)
        {
            case 1:
                //Button 1 pressed;
                maphandler.SaveMap("Map1");
                break;
            case 2:
                //Button 2 pressed;
                DeleteAllPlanets();
                maphandler.LoadMapForEditor("Map1");
                break;
            case 3:
                //Button 3 pressed;
                menumode = MenuMode.mainmenu;
                SceneManager.LoadScene(Constants.SCENE_MAINMENU);
                break;
            default:
                break;
        }
    }
    
    private void ToggleMenuVisible()
    {
        //IsButtonPressed = true;
        transform.GetComponent<MeshRenderer>().enabled = IsMenuVisible;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = IsMenuVisible;
        }
        IsMenuVisible = !IsMenuVisible;
    }
    
    private void DeleteAllPlanets()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject planet in planets)
        {
            GameObject.Destroy(planet);
        }
    }

    //Public functions

    //Get info if button is being touched
    public void ButtonPress(int buttonnumber)
    {
        switch (menumode)
        {
            case MenuMode.mainmenu:
                MainMenuMode(buttonnumber);
                break;
            case MenuMode.game:
                GameMode(buttonnumber);
                break;
            case MenuMode.mapeditor:
                MapEditorMode(buttonnumber);
                break;
            default:
                break;
        }
    }

    public void ButtonReleased()
    {

    }

    public void SetMenuMode(int mode)
    {
        switch (mode)
        {
            case 0:
                menumode = MenuMode.mainmenu;
                break;
            case 1:
                menumode = MenuMode.game;
                break;
            case 2:
                menumode = MenuMode.mapeditor;
                break;
            default:
                break;
        }
    }
}
