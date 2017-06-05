using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

    public List<GameObject> ListOfFighters;
    public List<GameObject> ListOfPlanets;
    public int NumberOfPlayers = 2;
    public int NumberOfAIPlayers = 1;

    //test AI
    private List<EnemyAI> EnemyAIList = new List<EnemyAI>();
    private List<int> NumberOfOwnedPlanets = new List<int>();
	// Use this for initialization
	void Start () {

        //Create AI players

        for (int i = 0; i < NumberOfAIPlayers; i++)
        {
            EnemyAIList.Add(new EnemyAI(NumberOfPlayers + i + 1));
        }

        //List containing all fighters
        ListOfFighters = new List<GameObject>();

        //Add all planets to List of planets
        ListOfPlanets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));

        //initialize the list of owned planets
        int TotalNumberOfPlayers = NumberOfPlayers + NumberOfAIPlayers;
        for (int i = 0; i < TotalNumberOfPlayers; i++)
        {
            NumberOfOwnedPlanets.Add(0);
        }
        
        //add owned planets to each player
        foreach (GameObject planet in ListOfPlanets)
        {
            if (planet.GetComponent<Planet>().Owner > 0) NumberOfOwnedPlanets[planet.GetComponent<Planet>().Owner - 1]++;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Make enemyAIs do things
        foreach(EnemyAI AI in EnemyAIList)
        {
            AI.DoThings();
        }

        //Check winner
        CheckForWinner();

	}


    public void AddFighterToList(GameObject fighter, int owner,GameObject initialTarget)
    {
        ListOfFighters.Add((GameObject)fighter);
        //Set fighter and childs to right layers
        MoveToLayer(fighter.transform, owner + 7);

        //If needed add fighter to AIs list of fighters
        if(NumberOfPlayers < owner)
        {
            EnemyAIList[owner - 1 - NumberOfPlayers].AddFighter(fighter);
        }

        ListOfFighters[ListOfFighters.Count - 1].GetComponent<Fighter>().SetOwner(owner);
        ListOfFighters[ListOfFighters.Count - 1].GetComponent<Fighter>().SetTarget(initialTarget);
    }

    public void RemoveFighterFromList(GameObject FighterToRemove)
    {
        //If needed remove fighter from AIs list of fighters
        //FIX this.. now player 2 is considered to be AI
        int owner = FighterToRemove.GetComponent<Fighter>().Owner;

        if (NumberOfPlayers < owner)
        {
            //print("AI" + owner + " figther");
            EnemyAIList[owner - 1 - NumberOfPlayers].RemoveFighterFromList(FighterToRemove);
        }

        ListOfFighters.Remove(FighterToRemove);
        Destroy(FighterToRemove);
    }

    public void SetNumberOfPlayers(int numberofplayers)
    {
        NumberOfPlayers = numberofplayers;
    }

    public void PlanetOwnershipChanged(int newowner, int oldowner)
    {
        //FIX this... NOT working atm
       NumberOfOwnedPlanets[newowner - 1] ++;
       if (oldowner != 0) NumberOfOwnedPlanets[oldowner - 1]--;
    }

    //Used to move fighters and children to right layer
    private void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }

    private void CheckForWinner()
    {
        int winner;
        for (int i = 0; i < NumberOfOwnedPlanets.Count; i++)
        {
            print("Player" + i + 1 + " " + NumberOfOwnedPlanets[i]);
        }
        //return winner;
    }

}
