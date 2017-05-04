using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour {

    public GameObject Target;
    public int Owner = 0; //Owner of the fighter
    public int Damage = 1; //Damage that the fighter does
    public float Speed = 100f; //Speed of the fighter
    public float OrbitSpeed = 100f; //Orbit speed of Fighter
    public float OrbitDistanceVariance = 0.5f; //Distance of orbit around planets
    public float MinOrbitDistance = 0.4f;
    public float OrbitInclinationVariance = 0.1f; //Variance in orbit inclinations
    public GameObject IsSelectedMesh; //Mesh to show if the fighter is selected

    private GameObject GameMaster; //Game master
    private GameObject LastTarge; //Last target of the fighter
    private bool Orbiting = false; //Is fighter orbiting
    private bool LineOfSightToTarget = true;
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
        OrbitDistance = Random.Range(MinOrbitDistance, MinOrbitDistance + OrbitDistanceVariance);

        //Set random orbit inclination to add variance
        OrbitInclination = Vector3.up + new Vector3(Random.Range(0f, OrbitInclinationVariance) - 1f, 0f, 0f);

        //Correct for orbital speed
        OrbitSpeed = (1 - OrbitDistance) * OrbitSpeed;

        SetColor();
    }
	
	// Update is called once per frame
	void Update () {
        //Get distance to target  if not orbiting and move towards the target
        

        if (Orbiting == false)
        {
            MoveTowards();
        }
        //Orbit around player owned planets if close enough
        if (Orbiting == true)
        {
            OrbitPlanet();
        }
    }

    //private functions
    //Orbit around planet if owned by player
    private void OrbitPlanet()
    {
        transform.RotateAround(Target.transform.position,OrbitInclination, OrbitSpeed * Time.deltaTime);
    }
    //Orbit around certain target
    private void OrbitPlanet(GameObject PlanetToOrbit)
    {
        transform.RotateAround(PlanetToOrbit.transform.position, OrbitInclination, OrbitSpeed * Time.deltaTime);
    }

    private void MoveTowards()
    {
        //get distance to target and move forward
        
        CheckLineOfSight();

        if (LineOfSightToTarget == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed / 500 * Time.deltaTime);
        }

        //If there is no line of sight, keep orbiting.
        if (LineOfSightToTarget == false)
        {
            OrbitPlanet(LastTarge);
        }

        //If the figther is close enough to player owned planet, start orbiting
        DistanceToTarget = Vector3.Distance(gameObject.transform.position, Target.transform.position);
        if (DistanceToTarget < OrbitDistance && TargetOwner == Owner)
        {
            Orbiting = true;
        }
    }

    private void CheckLineOfSight()
    {
        RaycastHit hit;
        Vector3 RayDirection = Target.transform.position - transform.position; //Get the right direction

        //Cast rays to see if something is hit
        if (Physics.Raycast(transform.position, RayDirection, out hit, 80f))
        {
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
            if(Target == hit.transform.parent.gameObject)
            { 
                LineOfSightToTarget = true;

            }
            else LineOfSightToTarget = false;
            
        }
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
        LastTarge = Target;
        Target = target; 
        TargetOwner = Target.GetComponentInParent<Planet>().GetOwner();
        LineOfSightToTarget = false;
        Orbiting = false;
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
