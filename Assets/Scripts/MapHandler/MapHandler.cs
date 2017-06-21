using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MapHandler {

    private List<Map> PlayingField = new List<Map>();

    public MapHandler()
    {

    }

	public void SaveMap(string MapName)
    {
        //"read" the current map
        foreach(GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            //Add all the information about the planets and owners to a list of map.
            PlayingField.Add(new Map(planet.transform.position, planet.GetComponent<Planet>().GetOwner()));
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create(MapName + ".map");
        bf.Serialize(saveFile, PlayingField);
        saveFile.Close();

    }
	
	public void LoadMap(string MapName)
    {
        //Add loading map from file here
        BinaryFormatter bf = new BinaryFormatter();
        FileStream MapData = File.Open(MapName + ".map",FileMode.Open);
        PlayingField = (List<Map>)bf.Deserialize(MapData);
        MapData.Close();


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
