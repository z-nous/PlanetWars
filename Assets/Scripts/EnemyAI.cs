using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

    private List<GameObject> Planets;
    private List<GameObject> Fighters;
    private GameObject Target;
    private int PlayerNumber = 0;

    //Constructors
    public EnemyAI(int playernumber)
    {
        Planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
        Fighters = new List<GameObject>();
        PlayerNumber = playernumber;
    }

    public void AddFighter(GameObject fighter)
    {
        Fighters.Add((GameObject)fighter);
    }

    public void RemoveFighterFromList(GameObject FighterToRemove)
    {
        Fighters.Remove(FighterToRemove);
    }

    public void DoThings () {
        if(!Target)SelectTarget();

        if (Target) Attack();

	}

    private void Attack()
    {

        //clear target if it is owned.
        if (Target.GetComponent<Planet>().Owner == PlayerNumber) Target = null;

        if(Fighters.Count > 50)
        {
            bool fliplop = true;
            foreach (GameObject fighter in Fighters)
            {
                if (fliplop == true)fighter.GetComponent<Fighter>().SetTarget(Target);
                fliplop = !fliplop;
            }
        }
    }

    private void SelectTarget()
    {
        //Very simple target selection
        //ADD things like closest enemy planet and other stuff, like number of enemy fighters at the target planet

        foreach (GameObject planet in Planets)
        {
            int planetowner = planet.GetComponent<Planet>().Owner;

            if (planetowner == 0)
            {
                Target = planet;
                return;
            }

            if (planetowner != PlayerNumber)
            {
                Target = planet;
                return;
            }
        }
    }
}
