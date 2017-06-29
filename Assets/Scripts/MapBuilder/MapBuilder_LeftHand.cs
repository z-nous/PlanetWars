using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapBuilder_LeftHand : MonoBehaviour {

    //Used to save and load maps

    

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
        

        //For press and hold detection
        if (XButton == true) IsXButtonePressed = true;
        else IsXButtonePressed = false;

        if (YButton == true) IsYButtonPressed = true;
        else IsYButtonPressed = false;

        if (LeftThumbstickPress == true) IsLeftThumbstickPressed = true;
        else IsLeftThumbstickPressed = false;
    }


}
