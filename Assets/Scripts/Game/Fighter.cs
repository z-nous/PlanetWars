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
    public float SeeingDistance = 0.5f;
    public GameObject IsSelectedMesh; //Mesh to show if the fighter is selected
    public int IsTargeted = 0;

    private GameObject GameMaster; //Game master
    public GameObject LastTarge; //Last target of the fighter
    public Transform EnemyFighter;
    public bool Orbiting = false; //Is fighter orbiting
    public bool LineOfSightToTarget = true; //Is there a line of sight to target
    public bool IsEnemyNear = false;
    public int TargetOwner = 0; //Owner of the target
    public float DistanceToTarget = 0f; //Distance to target
    public float OrbitDistance = 0f;
    public Vector3 OrbitInclination; //Orbit inclination
    public int PlanetLayerMask;
    public int EnemyLayerMask;

    public float orbitTimer = 0f; //Timer to steer clear of planets
    public float orbitDelay = .3f; //Delay before heading to new target

    // Use this for initialization
    void Start () {
        //Set Planets layer mask for raycasting
        PlanetLayerMask = LayerMask.GetMask("Planets");

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

        //construct enemy layermask to be used in spherecast to find enemies
        if (gameObject.layer == 8) EnemyLayerMask = 1 << 9 | 1 << 10 | 1 << 11;
        if (gameObject.layer == 9) EnemyLayerMask = 1 << 8 | 1 << 10 | 1 << 11;
        if (gameObject.layer == 10) EnemyLayerMask = 1 << 8 | 1 << 9 | 1 << 11;
        if (gameObject.layer == 11) EnemyLayerMask = 1 << 8 | 1 << 9 | 1 << 10;
    }
	
	// Update is called once per frame
	void Update () {
        //Get distance to target  if not orbiting and move towards the target

        if(IsEnemyNear == false) CheckForEnemies();
        if (IsEnemyNear == true) Attack();

        if (Orbiting == false && IsEnemyNear == false)
        {
            MoveTowards();
        }
        //Orbit around player owned planets if close enough
        if (Orbiting == true && IsEnemyNear == false)
        {
            OrbitPlanet();
        }
    }

    private void Attack()
    {
        //Check if the enemy is still in the game
        if(EnemyFighter == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, EnemyFighter.position, Speed / 500 * Time.deltaTime);
            transform.LookAt(EnemyFighter.transform);
            Debug.DrawLine(transform.position, EnemyFighter.position,Color.red);
        }
        else
        {
            IsEnemyNear = false;
            Orbiting = false;
        }
    }

    //Check if there are enemies nearby. if so Go and kill them
    private void CheckForEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SeeingDistance, EnemyLayerMask);
        if (hitColliders.Length > 0)
        {
            Debug.DrawLine(transform.position, hitColliders[0].transform.position, Color.red);
            /*
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].GetComponentInParent<Fighter>().IsTargeted < 20)
                {
                    EnemyFighter = hitColliders[i].transform;
                    hitColliders[i].GetComponentInParent<Fighter>().SetAsTarget();
                }
                else return;
            }*/

            //EnemyFighter = hitColliders[0].transform;
            EnemyFighter = hitColliders[Random.Range(0, hitColliders.Length)].transform;
            //enemies are near. 
            IsEnemyNear = true;
        }
    }

    //Orbit around planet if owned by player
    private void OrbitPlanet()
    {
        transform.RotateAround(Target.transform.position,OrbitInclination, OrbitSpeed * Time.deltaTime);
        transform.LookAt(Target.transform);
    }
    //Orbit around certain target
    private void OrbitPlanet(GameObject PlanetToOrbit)
    {
        transform.RotateAround(PlanetToOrbit.transform.position, OrbitInclination, OrbitSpeed * Time.deltaTime);
        transform.LookAt(PlanetToOrbit.transform);
    }

    private void MoveTowards()
    {

        //Check target owner and distance. If ok, move towards
        DistanceToTarget = Vector3.Distance(gameObject.transform.position, Target.transform.position);
        TargetOwner = Target.GetComponentInParent<Planet>().GetOwner();
        if (DistanceToTarget < OrbitDistance && TargetOwner == Owner)
        {
            Orbiting = true;
            return;
        }

        if (LineOfSightToTarget == false)CheckLineOfSight();

        if (LineOfSightToTarget == true)
        {
            orbitTimer += Time.deltaTime;
            if (orbitTimer < orbitDelay && Orbiting == false)
            {
                if(LastTarge)OrbitPlanet(LastTarge);
                else //FIX THIS. Fighters still migh hit the wrong planet
                {
                    transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed / 500 * Time.deltaTime);
                    transform.LookAt(Target.transform);
                }
                return;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed / 500 * Time.deltaTime);
                transform.LookAt(Target.transform);
                return;
            }

        }

        //If there is no line of sight, keep orbiting.
        if (LineOfSightToTarget == false)
        {
            if (LastTarge) OrbitPlanet(LastTarge);
            else OrbitPlanet();
            return;
        }

    }

    private void CheckLineOfSight()
    {
        RaycastHit hit;
        Vector3 RayDirection = Target.transform.position - transform.position; //Get the right direction

        //Cast rays to see if something is hit
        if (Physics.Raycast(transform.position, RayDirection, out hit, 80f, PlanetLayerMask))
        {
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.green);
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
        if (Owner == 3)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
            Color temp = new Color(1f, 0f, 0f, 0.5f);
            IsSelectedMesh.GetComponent<Renderer>().material.color = temp;
        }
        if (Owner == 4)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.yellow;
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
        orbitTimer = 0f;
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
        if (collision.gameObject.tag == "Planetchild" && collision.gameObject.GetComponentInParent<Planet>().GetOwner() == Owner)
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
        if (collision.gameObject.tag == "Planetchild" && collision.gameObject.GetComponentInParent<Planet>().GetOwner() != Owner)  
        {
            collision.gameObject.GetComponent<Distorter>().distort(.2f, 0.5f);
            collision.gameObject.GetComponentInParent<Planet>().MinusHealth(Owner); //Minus planet health
            GameMaster.GetComponent<GameMaster>().RemoveFighterFromList(this.gameObject); //Remove fighter
        }

        //Collision with enemy fighter
        if(collision.gameObject.tag == "Fighter" && collision.gameObject.GetComponentInParent<Fighter>().GetOwner() != Owner)
        {
            GameMaster.GetComponent<GameMaster>().RemoveFighterFromList(this.gameObject);
        }

    }

    public void SetAsTarget()
    {
        IsTargeted ++;
    }
}
