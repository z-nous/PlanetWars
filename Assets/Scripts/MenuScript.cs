using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    private int PressedButtonNumber = 0;
    private bool BButton = false;
    private bool AButton = false;
    private bool XButton = false;
    private bool YButton = false;

    private bool IsMenuVisible = true;
    private bool IsButtonPressed = false;


    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //Read controller 
        AButton = OVRInput.Get(OVRInput.Button.One);
        BButton = OVRInput.Get(OVRInput.Button.Two);
        XButton = OVRInput.Get(OVRInput.Button.Three);
        YButton = OVRInput.Get(OVRInput.Button.Four);

        //Reset buttonpress if all buttons are released
        if (!BButton && !AButton && !XButton && !YButton) IsButtonPressed = false;

        //Toggle menu visible
        if (YButton && IsButtonPressed == false) ToggleMenuVisible();


        //ButtonInteractions

        //Button 1 pressed
        if (PressedButtonNumber == 1 && AButton == true && IsButtonPressed == false)
        {

        }

        //Button 2 pressed
        if (PressedButtonNumber == 1 && AButton == true && IsButtonPressed == false)
        {

        }

        //Button 3 pressed
        if (PressedButtonNumber == 1 && AButton == true && IsButtonPressed == false)
        {

        }


    }

    //Private functions
    private void ToggleMenuVisible()
    {
        IsButtonPressed = true;
        transform.GetComponent<MeshRenderer>().enabled = IsMenuVisible;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = IsMenuVisible;
        }
        IsMenuVisible = !IsMenuVisible;
    }
    
    //Public functions

    //Get info if button is being touched
    public void ButtonPress(int buttonnumber)
    {
        if(IsMenuVisible) PressedButtonNumber = buttonnumber;
    }

    public void ButtonReleased()
    {
        if (IsMenuVisible) PressedButtonNumber = 0;
    }
}
