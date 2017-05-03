using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionScript : MonoBehaviour {

    public Transform SelectionSpherePosition; //Place where selection is spawned
    public Transform RaycastLocation;
    public LineRenderer PointingLineRenderer;

    private bool IsSelectionCreated = false; //is selection cube created
    private bool IsRightThumStickPressed = false;
    private GameObject SelectionSphere;
    private float Scale = 0.05f; //Scale of selection sphere
    private List<GameObject> ListOfSelections;
    private float RightIndexTrigger = 0f;
    private bool BButton = false;
    private bool AButton = false;

    // Use this for initialization
    void Start()
    {
        ListOfSelections = new List<GameObject>();
        SelectionSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere); //instantiate selection sphere
        SelectionSphere.transform.parent = SelectionSpherePosition; //Set selection sphere as a child to selection sphere position
        SelectionSphere.transform.position = SelectionSpherePosition.position; //Set Selection Sphere position
        SelectionSphere.transform.localScale = new Vector3(Scale, Scale, Scale); //Set Selection Shere scale
        SelectionSphere.GetComponent<Collider>().isTrigger = true;
        //Color temp = new Color(0f, 0f, 1f, 0.5f); //Set sphere alpha to .5
        //SelectionSphere.GetComponent<Renderer>().material.color = temp;
        SelectionSphere.GetComponent<MeshRenderer>().enabled = false; //Make the sphere invisible

        //Set right amount of vertices to pointinglinerenderer and starting point
        PointingLineRenderer.SetVertexCount(2);
        PointingLineRenderer.SetPosition(0, RaycastLocation.position);
        PointingLineRenderer.SetPosition(1, RaycastLocation.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Read controller
        RightIndexTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        BButton = OVRInput.Get(OVRInput.Button.Two);
        AButton = OVRInput.Get(OVRInput.Button.One);

        //Hide selectionSPhere and make it small if trigger is not presse
        if (IsSelectionCreated == true && RightIndexTrigger == 0)
        {
            SelectionSphere.GetComponent<MeshRenderer>().enabled = false;
            SelectionSphere.transform.localScale = new Vector3(0f, 0f, 0f); 
            IsSelectionCreated = false;
        }

        //Start selection tool if no fighters are selected and right index trigger is pressed 
        if (RightIndexTrigger > 0) SelectFighters();

        //Clear selection with click of right thumbstick
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) && IsSelectionCreated == false)
        {
            ClearSelection();
        }

        //start destination tool if there are fighters selected
        if (ListOfSelections.Count >= 1 && IsSelectionCreated == false) SetFighterDestination();

    }

    

    //Private functions

    private void ClearSelection()
    {
        //hide linerenderer
        PointingLineRenderer.SetPosition(0, RaycastLocation.position);
        PointingLineRenderer.SetPosition(1, RaycastLocation.position);

        //Go through fighters and deselect them
        for (int i = 0; i < ListOfSelections.Count; i++)
        {
            ListOfSelections[i].GetComponentInParent<Fighter>().IsSelected(false);
        }
        //Clear the list
        ListOfSelections.Clear();
    }


    private void SelectFighters()
    {
        //Do this if the button has not been pressed earlier
        if (IsSelectionCreated == false && ListOfSelections.Count == 0 && IsRightThumStickPressed == false) 
        {
            //Make the sphere visible while button pressed
            SelectionSphere.GetComponent<MeshRenderer>().enabled = true; 

            //SelectionSphere.GetComponent<Collider>().isTrigger = true; //enable trigger to select objects
            IsSelectionCreated = true;
        }

        //Do this while button is pressed
        if (IsSelectionCreated == true ) 
        {
            //Do these until right thumbstic is pressed
            if (IsRightThumStickPressed == false)
            {
                //Get trigger position and set scale
                Scale = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger); 

                //Scale the spehere
                SelectionSphere.transform.localScale = new Vector3(Scale, Scale, Scale);
                
                //Move the selection sphere according to left thumb stick
                SelectionSpherePosition.localPosition = new Vector3(0f, 0f, 1.5f + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y);
            }
             
            //If right thumbstick is clicked, finish selection
            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) && IsRightThumStickPressed == false)
            {
                //print("pushed");
                IsRightThumStickPressed = true;
                //Hide the selection sphere
                SelectionSphere.GetComponent<MeshRenderer>().enabled = false;

                //Scale down the selection sphere
                Scale = 0.005f; 
                SelectionSphere.transform.localScale = new Vector3(Scale, Scale, Scale);

                //For testing.. print the amount of selected fighters
                //print(ListOfSelections.Count);
                //

            }
            //See when the button is released
            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) == false && IsRightThumStickPressed == true)
            {
                IsRightThumStickPressed = false;
                //print("released");
                IsSelectionCreated = false;
            }

        }


    }

    private void SetFighterDestination()
    {
        RaycastHit hit;

        //Cast rays to see if something is hit
        if(Physics.Raycast(RaycastLocation.position,gameObject.transform.forward,out hit, 20f))
        {
            //Draw raycast for debugging
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
            //print(hit.transform.tag);
            PointingLineRenderer.SetPosition(0, transform.position);
            PointingLineRenderer.SetPosition(1, hit.point);

            if(hit.transform.tag == "Planet" && BButton == true)
            {

                //Only do this once
                print(ListOfSelections.Count);
                GameObject Target = hit.transform.gameObject;
                for (int i = 0; i < ListOfSelections.Count; i++)
                {
                    print("Target Set" + i);
                    ListOfSelections[i].GetComponentInParent<Fighter>().SetTarget(Target);
                }
                //Clear selections after target is set
                ClearSelection();
            }
        }
    }

    //Trigger events

    void OnTriggerEnter(Collider collision)
    {
        //To-Do add detection for only player Fighters
        if (collision.gameObject.tag == "Fighter" && IsSelectionCreated == true && IsRightThumStickPressed == false)
        {
            ListOfSelections.Add(collision.gameObject);
            collision.gameObject.GetComponentInParent<Fighter>().IsSelected(true); //Let the fighter know its been selected
            //print(ListOfSelections.Count);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        //To-Do add detection for only player Fighters
        if (collision.gameObject.tag == "Fighter" && IsRightThumStickPressed == false && IsSelectionCreated == true)
        {
            ListOfSelections.Remove(collision.gameObject);
            collision.gameObject.GetComponentInParent<Fighter>().IsSelected(false); //Let the fighter know it's been deselected
            //print(ListOfSelections.Count);
        }
    }
}
