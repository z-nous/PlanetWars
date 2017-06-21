using UnityEngine;
using System.Collections;

public class MapBuilder_RightHand : MonoBehaviour {

    public Transform PlanetSpawnLocation;
    public LineRenderer PointingLineRenderer;
    public Transform RaycastLocation;
    //Controller values
    private float RighIndexTrigger;
    private float RightMiddleTrigger;
    private bool AButton = false;
    private bool BButton = false;
    private bool RightThumbstickPress = false;
    private Vector2 RighThumbstick;
    private bool HoldingPlanet = false;
    private GameObject HeldPlanet;
    private GameObject planet;
	// Use this for initialization
	void Start () {

        planet = (GameObject)(Resources.Load(Constants.PREFAB_MAP_PLANET));

    }
	
	// Update is called once per frame
	void Update () {

        //read controller
        RighIndexTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        RightMiddleTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        BButton = OVRInput.Get(OVRInput.Button.Two);
        AButton = OVRInput.Get(OVRInput.Button.One);
        RightThumbstickPress = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
        RighThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        
        if(HoldingPlanet == true)
        {
            MovePlanet();
        }

        Point();

        if(HoldingPlanet == true && RightMiddleTrigger == 0)
        {
            LetGo();
        }

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

            //this is for grabbing planets that player points at
            if(hit.transform.tag == "PlanetChild" && HoldingPlanet == false && RightMiddleTrigger > 0)
            {
                
                //HeldPlanet = (GameObject)hit.transform.gameObject;
                //HeldPlanet.transform.parent = PlanetSpawnLocation;
            }
            PointingLineRenderer.SetPosition(0, transform.position);
            PointingLineRenderer.SetPosition(1, hit.point);
        }
    }

    private void LetGo()
    {
        HeldPlanet.transform.parent = null;
        HoldingPlanet = false;
        PlanetSpawnLocation.localPosition = new Vector3(0f, 0f, 0f);
    }

    private void MovePlanet()
    {
        PlanetSpawnLocation.localPosition += new Vector3(RighThumbstick.x,0f,RighThumbstick.y) * Time.deltaTime;
        //PlanetSpawnLocation.localPosition.y += RighThumbstick.y;
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.tag == "Planetchild" && RightMiddleTrigger > 0 && HoldingPlanet == false)
        {
            HoldingPlanet = true;
            (HeldPlanet = Instantiate(planet, PlanetSpawnLocation.position, Quaternion.identity) as GameObject).transform.parent = PlanetSpawnLocation;
            //Instantiate(Planet, PlanetSpawnLocation, Quaternion.identity);
        }
    }


}
