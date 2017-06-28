using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapBuilder_LeftHand : MonoBehaviour {

    //Used to save and load maps

    private MapHandler maphandler = new MapHandler();

    private float LeftIndexTrigger;
    private float LeftMiddleTrigger;
    private bool XButton = false;
    private bool YButton = false;
    private bool LeftThumbstickPress = false;
    private Vector2 LeftThumbstick;

    //For press detection
    private bool IsXButtonePressed = false;
    private bool IsYButtonPressed = false;
    private bool IsLeftThumbstickPressed = false;

    // Use this for initialization
    void Start () {
        maphandler.LoadMapForEditor("Map1");
    }
	
	// Update is called once per frame
	void Update () {
        //read controller
        LeftIndexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        LeftMiddleTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        XButton = OVRInput.Get(OVRInput.Button.Three);
        YButton = OVRInput.Get(OVRInput.Button.Four);
        LeftThumbstickPress = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
        LeftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        //do stuff here

        if(XButton == true && IsXButtonePressed == false)
        {
            print("X is for saving the map");
            maphandler.SaveMap("Map1");
            //Destroy all planets before leaving

            GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
            foreach(GameObject planet in planets)
            {
                GameObject.Destroy(planet);
            }

            SceneManager.LoadScene(Constants.SCENE_MAINMENU);

        }

        if(YButton == true && IsYButtonPressed == false)
        {
            print("Y is for loading the map");
            maphandler.LoadMapForEditor("Map1");
        }

        //For press and hold detection
        if (XButton == true) IsXButtonePressed = true;
        else IsXButtonePressed = false;

        if (YButton == true) IsYButtonPressed = true;
        else IsYButtonPressed = false;

        if (LeftThumbstickPress == true) IsLeftThumbstickPressed = true;
        else IsLeftThumbstickPressed = false;
    }


}
