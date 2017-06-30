using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI {

    private List<GameObject> Planets;
    private List<GameObject> Fighters;
    private GameObject Target;
    private int PlayerNumber = 0;
    private float Timer1 = 0f;
    private float attackTimer = 0f;
    //Constructors
    public EnemyAI(int playernumber)
    {
        Planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
        Fighters = new List<GameObject>();
        SelectTarget();
        ShufflePlanetList(); //Add ramdomness to attacking
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
        Timer1 += Time.deltaTime;
        attackTimer += Time.deltaTime;
        if (Timer1 > 60f || Target.GetComponent<Planet>().Owner == PlayerNumber)
        {
            Timer1 = 0f;
            SelectTarget();
            
        }

        if (Target && attackTimer > 30f)
        {
            Attack();
            attackTimer = 0f;
        }

    }

    private void Attack()
    {

        //clear target if it is owned.
        //if (Target.GetComponent<Planet>().Owner == PlayerNumber) Target = null;

        if(Fighters.Count > 20)
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
        //ADD things like closest enemy planet and number of enemy fighters at the target planet

        foreach (GameObject planet in Planets)
        {
            int planetowner = planet.GetComponent<Planet>().Owner;

            if (planetowner != PlayerNumber)
            {
                Target = planet;
                if (planetowner == 0)
                {
                    Target = planet;
                    ShufflePlanetList();
                    return;
                }
            }
        }
        ShufflePlanetList();
    }

    private void ShufflePlanetList()
    {
        
        for (int i = 0; i < Planets.Count; i++)
        {
            GameObject temp = Planets[i];
            int randomIndex = Random.Range(i, Planets.Count);
            Planets[i] = Planets[randomIndex];
            Planets[randomIndex] = temp;
        }
    }
}
