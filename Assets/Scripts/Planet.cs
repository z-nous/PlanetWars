using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public GameObject Fighter; //Fighter prefab that is instantiated
    public Transform FighterSpawnPoint;
    public int MaxHealth = 100;
    public int Health = 100; //health
    public int Owner = 0; //Owner of the planet
    public float FigtherSpawnInterval = 1f; //how often fighters are spawned
    

    private float FighterSpawnIntervalTimer = 0f; //timer used to spawn fighters
    private GameObject GameMaster;


    // Use this for initialization
    void Start () {
        //Add Gamemaster object if null
        if (GameMaster == null)
        {
            GameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        }
        SetColor();//set initial color of planet according to owner

    }
	
	// Update is called once per frame
	void Update () {
        SpawnFigthers();
	}

    //################################private functions################################################

    // Function to spawn fighters
    private void SpawnFigthers()
    {
        FighterSpawnIntervalTimer += Time.deltaTime;
        if (FighterSpawnIntervalTimer >= FigtherSpawnInterval && Owner != 0)
        {
            //Spawn new fighter and send it to gamemaster list
            GameMaster.GetComponent<GameMaster>().AddFighterToList((GameObject)Instantiate(Fighter, FighterSpawnPoint.position, Quaternion.identity),Owner,this.gameObject);

            //reset FighterSpawnIntervalTimer
            FighterSpawnIntervalTimer = 0f;
        }
    }

    private void SetColor()
    {
        if (Owner == 0) gameObject.GetComponentInChildren<Renderer>().material.color = Color.gray;
        if (Owner == 1)gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
        if (Owner == 2) gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    //###################################Public functions################################################
    public int GetOwner()
    {
        return Owner;
    }

    public bool AddHealth()
    {
        if(Health < MaxHealth)
        {
            Health++;
            return true; //Return false if added health
        }
        else
        {
            return false; //Return true if health is at max
        }
    }

    public void MinusHealth(int NewOwner)
    {
        Health--;
        if(Health == 0)
        {
            Owner = NewOwner;

            SetColor(); //Set right color to planet
            Health++;
        }
    }
}
