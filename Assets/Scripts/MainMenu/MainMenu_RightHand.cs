using UnityEngine;
using System.Collections;

public class MainMenu_RightHand : MonoBehaviour
{
    public LineRenderer PointingLineRenderer;
    public Transform RaycastLocation;

    //Controller values
    private float RighIndexTrigger;
    private float RightMiddleTrigger;
    private bool AButton = false;
    private bool BButton = false;
    private bool RightThumbstickPress = false;
    private Vector2 RighThumbstick;

    // Use this for initialization

    //For press detection
    private bool IsAButtonePressed = false;
    private bool IsBButtonPressed = false;
    private bool IsRightThumbstickPressed = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //read controller
        RighIndexTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        RightMiddleTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        BButton = OVRInput.Get(OVRInput.Button.Two);
        AButton = OVRInput.Get(OVRInput.Button.One);
        RightThumbstickPress = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
        RighThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        Point();

        //For press and hold detection
        if (AButton == true) IsAButtonePressed = true;
        else IsAButtonePressed = false;

        if (BButton == true) IsBButtonPressed = true;
        else IsBButtonPressed = false;

        if (RightThumbstickPress == true) IsRightThumbstickPressed = true;
        else IsRightThumbstickPressed = false;

    }

    private void Point()
    {
        RaycastHit hit;

        //Draw the pointing line
        PointingLineRenderer.SetPosition(0, transform.position);
        PointingLineRenderer.SetPosition(1, transform.forward * 20f + transform.position);
        //Cast rays to see if something is hit

        if (Physics.Raycast(RaycastLocation.position, gameObject.transform.forward, out hit, 20f))
        {
            //Draw raycast for debugging
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);

            //Move the planet that the player is pointing at
            print(hit.transform.tag);

            if (hit.transform.tag == "NewGame" && AButton == true && IsAButtonePressed == false)
            {
                hit.transform.GetComponentInParent<MainMenu>().StartGame();
            }

            if (hit.transform.tag == "MapEditor" && AButton == true && IsAButtonePressed == false)
            {
                hit.transform.GetComponentInParent<MainMenu>().StartLevelEditor();
            }

            if (hit.transform.tag == "ExitGame" && AButton == true && IsAButtonePressed == false)
            {
                hit.transform.GetComponentInParent<MainMenu>().ExitGame();
            }

            //draw the pointing line
            PointingLineRenderer.SetPosition(0, transform.position);
            PointingLineRenderer.SetPosition(1, hit.point);
        }
    }
}
