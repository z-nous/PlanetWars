using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapHandler {

    private List<Map> PlayingField = new List<Map>();

    public MapHandler()
    {

    }

	public void SaveMap()
    {
        foreach(GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            //Add all the information about the planets and owners to a list of map.
            PlayingField.Add(new Map(planet.transform.position, planet.GetComponent<Planet>().GetOwner()));
        }

        //Add saving map to a file here

    }
	
	public void LoadMap()
    {
        //Add loading map from file here

        
        //testing

        int i = 0;
        foreach(Map planet in PlayingField)
        {
            //Instantiate the planets
            GameObject newplanet = (GameObject) MonoBehaviour.Instantiate(Resources.Load(Constants.PREFAB_PLANET), planet.GetPlanetLocation(), Quaternion.identity);
            newplanet.GetComponent<Planet>().Owner = planet.GetPlanetOwner();

            MonoBehaviour.print("Planet number:" + i);
            i++;
            MonoBehaviour.print("Location:" + planet.GetPlanetLocation().ToString("F2"));
            MonoBehaviour.print("Owner:" + planet.GetPlanetOwner());
        }
    }
}
