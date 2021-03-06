﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public List<GameObject> ListOfFighters;
    public List<GameObject> ListOfPlanets;
    public int NumberOfPlayers = 1;
    public int NumberOfAIPlayers = 1;

    //List of all Enemy AIs
    private List<EnemyAI> EnemyAIList = new List<EnemyAI>();
    // List containing player info
    private List<PlayerInfo> Players = new List<PlayerInfo>();
    private int TotalNumberOfPlayers = 0;
    private int NumberOfAlivePlayers = 0;

    //Map handler to load map
    private MapHandler maphandler = new MapHandler();

	void Start () {
        //Load Map
        maphandler.LoadMap("Map1");

        //Count how many players there are in a map
        int playersinmap = 0;
        bool player1 = false;
        bool player2 = false;
        bool player3 = false;
        bool player4 = false;
        foreach (GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            if (planet.GetComponent<Planet>().Owner == 1 && player1 == false)
            {
                playersinmap++;
                player1 = true;
            }
            if (planet.GetComponent<Planet>().Owner == 2 && player2 == false)
            {
                playersinmap++;
                player2 = true;
            }
            if (planet.GetComponent<Planet>().Owner == 3 && player3 == false)
            {
                playersinmap++;
                player3 = true;
            }
            if (planet.GetComponent<Planet>().Owner == 4 && player4 == false)
            {
                playersinmap++;
                player4 = true;
            }
        }
        //Set correct amount of ai players
        NumberOfAIPlayers = playersinmap - NumberOfPlayers;
        //Create AI players

        for (int i = 0; i < NumberOfAIPlayers; i++)
        {
            EnemyAIList.Add(new EnemyAI(NumberOfPlayers + i + 1));
        }

        //List containing all fighters
        ListOfFighters = new List<GameObject>();

        //Add all planets to List of planets
        ListOfPlanets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));




        //create and initialize the list of owned planets
        List<int> NumberOfOwnedPlanets = new List<int>();

        TotalNumberOfPlayers = NumberOfPlayers + NumberOfAIPlayers;
        NumberOfAlivePlayers = TotalNumberOfPlayers;
        for (int i = 0; i < TotalNumberOfPlayers; i++)
        {
            NumberOfOwnedPlanets.Add(0);
        }

        //add owned planets to each player
        foreach (GameObject planet in ListOfPlanets)
        {
            if (planet.GetComponent<Planet>().Owner > 0) NumberOfOwnedPlanets[planet.GetComponent<Planet>().Owner - 1]++;
        }

        //Initialize player info
        for (int i = 0; i < TotalNumberOfPlayers; i++)
        {
            Players.Add(new PlayerInfo(i + 1, NumberOfOwnedPlanets[i]));
        }
    }
	
	// Update is called once per frame
	void Update () {

        //Make enemyAIs do things
        foreach(EnemyAI AI in EnemyAIList)
        {
            AI.DoThings();
        }

        //Check for winner
        if (NumberOfAlivePlayers == 1)
        {
            CheckForWinner();
            //Load main menu after game
            SceneManager.LoadScene(Constants.SCENE_MAINMENU);
        }

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
        //Add one planet for the new owner
        Players[newowner - 1].NumberOfOwnedPlanets += 1;

        //Remove planet from old owner and check for alive status of that player
        if (oldowner != 0)
        {
            Players[oldowner - 1].NumberOfOwnedPlanets -= 1;
            if (Players[oldowner - 1].NumberOfOwnedPlanets <= 0)
            {
                Players[oldowner - 1].IsAlive = false;
                NumberOfAlivePlayers--;
            }
        }

    }

    //Used to move fighters and children to right layer
    private void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }

    private int CheckForWinner()
    {
        int winner = 0;

        //Find the winner
        for (int i = 0; i < TotalNumberOfPlayers; i++)
        {
            if (Players[i].IsAlive == true)
            {
                winner = Players[i].PlayerNumber;
                print("Player " + winner + " WON!!");
            }
        }


        return winner;
    }

}
