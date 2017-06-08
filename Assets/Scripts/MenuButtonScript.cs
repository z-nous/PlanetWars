using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {
    public int ButtonNumber = 0;

    private GameObject Menu;

    void Start()
    {
        Menu = gameObject.transform.parent.gameObject;

    }

    void OnTriggerEnter(Collider collision)
    {
        Menu.GetComponent<MenuScript>().ButtonPress(ButtonNumber);

    }

    void OnTriggerExit(Collider collision)
    {
        Menu.GetComponent<MenuScript>().ButtonReleased();
    }

    }
