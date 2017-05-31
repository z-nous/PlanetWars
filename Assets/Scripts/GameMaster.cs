using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

    public List<GameObject> ListOfFighters;
    public List<GameObject> ListOfPlanets;
    public int NumberOfPlayers = 2;

    //test AI
    private EnemyAI EnemyAI1;
    private EnemyAI EnemyAI2;

	// Use this for initialization
	void Start () {
        EnemyAI1 = new EnemyAI(1);
        EnemyAI2 = new EnemyAI(2);
        ListOfFighters = new List<GameObject>();

        //Add all planets to List of planets
        ListOfPlanets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
     }
	
	// Update is called once per frame
	void Update () {
        EnemyAI1.DoThings();
        EnemyAI2.DoThings();

	}


    public void AddFighterToList(GameObject fighter, int owner,GameObject initialTarget)
    {
        ListOfFighters.Add((GameObject)fighter);
        //Set fighter and childs to right layers
        MoveToLayer(fighter.transform, owner + 7);

        //If needed add fighter to AIs list of fighters
        if (owner == 2) EnemyAI2.AddFighter(fighter);
        if (owner == 1) EnemyAI1.AddFighter(fighter);

        ListOfFighters[ListOfFighters.Count - 1].GetComponent<Fighter>().SetOwner(owner);
        ListOfFighters[ListOfFighters.Count - 1].GetComponent<Fighter>().SetTarget(initialTarget);
    }

    public void RemoveFighterFromList(GameObject FighterToRemove)
    {
        //If needed remove fighter from AIs list of fighters
        //FIX this.. now player 2 is considered to be AI
        if (FighterToRemove.GetComponent<Fighter>().Owner == 2) EnemyAI2.RemoveFighterFromList(FighterToRemove);
        if (FighterToRemove.GetComponent<Fighter>().Owner == 1) EnemyAI1.RemoveFighterFromList(FighterToRemove);

        ListOfFighters.Remove(FighterToRemove);
        Destroy(FighterToRemove);
    }

    public void SetNumberOfPlayers(int numberofplayers)
    {
        NumberOfPlayers = numberofplayers;
    }

    //Used to move fighters and children to right layer
    private void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }


}
