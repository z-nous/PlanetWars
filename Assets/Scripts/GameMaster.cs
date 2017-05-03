using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

    public List<GameObject> ListOfFighters;

	// Use this for initialization
	void Start () {
        ListOfFighters = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddFighterToList(GameObject fighter, int owner,GameObject initialTarget)
    {
        ListOfFighters.Add((GameObject)fighter);
        ListOfFighters[ListOfFighters.Count - 1].GetComponent<Fighter>().SetOwner(owner);
        ListOfFighters[ListOfFighters.Count - 1].GetComponent<Fighter>().SetTarget(initialTarget);
    }

    public void RemoveFighterFromList(GameObject FighterToRemove)
    {
        ListOfFighters.Remove(FighterToRemove);
        Destroy(FighterToRemove);
    }
}
