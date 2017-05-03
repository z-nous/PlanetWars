using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour {

    public GameObject Target;
    public int Owner = 0; //Owner of the fighter
    public int Damage = 1; //Damage that the fighter does
    public float Speed = 100f; //Speed of the fighter
    public float OrbitSpeed = 100f; //Orbit speed of Fighter
    public float OrbitDistanceVariance = 0.5f; //Distance of orbit around planets
    public float OrbitInclinationVariance = 0.1f; //Variance in orbit inclinations
    public GameObject IsSelectedMesh; //Mesh to show if the fighter is selected

    private GameObject GameMaster; //Game master
    private bool Orbiting = false; //Is fighter orbiting
    private int TargetOwner = 0; //Owner of the target
    private float DistanceToTarget = 0f; //Distance to target
    private float OrbitDistance = 0f;
    private Vector3 OrbitInclination; //Orbit inclination
    // Use this for initialization
    void Start () {
        //Add gamemaster
        IsSelectedMesh.GetComponent<MeshRenderer>().enabled = false;
        if (GameMaster == null)
        {
            GameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        }
        //Set random orbit distance to add variance
        OrbitDistance = Random.Range(0.3f, 0.3f +OrbitDistanceVariance);

        //Set random orbit inclination to add variance
        OrbitInclination = Vector3.up + new Vector3(Random.Range(0f, OrbitInclinationVariance) - 1f, 0f, 0f);

        //Correct for orbital speed
        OrbitSpeed = (1 - OrbitDistance) * OrbitSpeed;

        SetColor();
    }
	
	// Update is called once per frame
	void Update () {
        //Get distance to target  if not orbiting
        if (Orbiting == false) DistanceToTarget = Vector3.Distance(gameObject.transform.position, Target.transform.position);

        //Move towards target
        if(Orbiting == false) MoveTowards();

        //Orbit around player owned planets if close enough
        if (TargetOwner == Owner && Orbiting == true && DistanceToTarget <= OrbitDistance) OrbitPlanet();
    }

    //private functions
    //Orbit around planet if owned by player
    private void OrbitPlanet()
    {

        transform.RotateAround(Target.transform.position,OrbitInclination, OrbitSpeed * Time.deltaTime);
    }

    private void MoveTowards()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed/500 * Time.deltaTime);
        if (DistanceToTarget < OrbitDistance && TargetOwner == Owner) Orbiting = true;
    }

    private void SetColor()
    {
        if (Owner == 1)
        {
            IsSelectedMesh.GetComponent<Renderer>().material.color = Color.blue;
            Color temp = new Color(0f, 0f, 1f, 0.5f);
            gameObject.GetComponentInChildren<Renderer>().material.color = temp;
        }
        if (Owner == 2)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            Color temp = new Color(1f, 0f, 0f, 0.5f);
            IsSelectedMesh.GetComponent<Renderer>().material.color = temp;
        }
    }

    //Public functions

    //If fighter is selected set selectionmesh visible
    public void IsSelected(bool isselected)
    {
        print("selected");
        if (isselected == true)
        {
            IsSelectedMesh.GetComponent<MeshRenderer>().enabled = true;
        }

        if (isselected == false)
        {
            IsSelectedMesh.GetComponent<MeshRenderer>().enabled = false;
        }

    }


    public void SetTarget(GameObject target)
    {
        Target = target; Orbiting = false;
        TargetOwner = Target.GetComponentInParent<Planet>().GetOwner();
    }

    public void SetOwner(int owner)
    {
        Owner = owner;
    }

    public int GetOwner()
    {
        return Owner;
    }

    void OnTriggerEnter(Collider collision)
    {
        //while hitting own planet
        if (collision.gameObject.tag == "Planet" && collision.gameObject.GetComponentInParent<Planet>().GetOwner() == Owner)
        {
            if (collision.gameObject.GetComponentInParent<Planet>().AddHealth() == true) //if health added. remove fighter 
            {
                GameMaster.GetComponent<GameMaster>().RemoveFighterFromList(this.gameObject);
            }
            else //If health si at max. start orbiting.
            {
                Orbiting = true;
            }
        }

        //while hitting enemy planet
        if (collision.gameObject.tag == "Planet" && collision.gameObject.GetComponentInParent<Planet>().GetOwner() != Owner)  
        {
            collision.gameObject.GetComponentInParent<Planet>().MinusHealth(Owner); //Minus planet health
            GameMaster.GetComponent<GameMaster>().RemoveFighterFromList(this.gameObject); //Remove fighter
        }

        //Collision with enemy fighter
        if(collision.gameObject.tag == "Fighter" && collision.gameObject.GetComponentInParent<Fighter>().GetOwner() != Owner)
        {
            GameMaster.GetComponent<GameMaster>().RemoveFighterFromList(this.gameObject);
        }

    }

}
