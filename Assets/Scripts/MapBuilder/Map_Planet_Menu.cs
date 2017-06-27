using UnityEngine;
using System.Collections;

public class Map_Planet_Menu : MonoBehaviour {

    private GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.parent.position + new Vector3(0f,0.5f,0f);
        //transform.Position = transform.parent.transform.position + new Vector3(0f, 0f, 0f);
        //transform.rotation = transform.position - Player.transform.position
        transform.LookAt(2*transform.position - Player.transform.position);
	}
}
