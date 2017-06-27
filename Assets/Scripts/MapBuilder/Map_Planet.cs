using UnityEngine;
using System.Collections;

public class Map_Planet : MonoBehaviour {

    public int MaxHealth = 20;
    public int Health = 20; //health
    public int Owner = 0; //Owner of the planet
    public float FigtherSpawnInterval = 1f; //how often fighters are spawned

    public void ShowMenu()
    {

    }

    private void SetColor()
    {
        if (Owner == 0) gameObject.GetComponentInChildren<Renderer>().material.color = Color.gray;
        if (Owner == 1) gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
        if (Owner == 2) gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        if (Owner == 3) gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
        if (Owner == 4) gameObject.GetComponentInChildren<Renderer>().material.color = Color.yellow;
    }
}
