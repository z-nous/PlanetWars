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
        PlayingField.Clear();

        foreach(GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            //Add all the information about the planets and owners to a list of map.
            PlayingField.Add(new Map(planet.transform.position, planet.GetComponent<Map_Planet>().GetOwner()));

        }




        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.dataPath + "/StreamingAssets" + "/Maps/" + MapName + ".map");
        bf.Serialize(saveFile, PlayingField);
        saveFile.Close();

    }
	
	public void LoadMap(string MapName)
    {
        PlayingField.Clear();
        //Add loading map from file here
        BinaryFormatter bf = new BinaryFormatter();
        FileStream MapData = File.Open(Application.dataPath + "/StreamingAssets" + "/Maps/" + MapName + ".map",FileMode.Open);
        PlayingField = (List<Map>)bf.Deserialize(MapData);
        MapData.Close();

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

    public void LoadMapForEditor(string MapName)
    {
        PlayingField.Clear();
        //Add loading map from file here
        BinaryFormatter bf = new BinaryFormatter();
        FileStream MapData = File.Open(Application.dataPath + "/StreamingAssets" +"/Maps/" + MapName + ".map", FileMode.Open);
        PlayingField = (List<Map>)bf.Deserialize(MapData);
        MapData.Close();

        int i = 0;
        foreach (Map planet in PlayingField)
        {
            //Instantiate the planets
            GameObject newplanet = (GameObject)MonoBehaviour.Instantiate(Resources.Load(Constants.PREFAB_MAP_PLANET), planet.GetPlanetLocation(), Quaternion.identity);
            newplanet.GetComponent<Map_Planet>().Owner = planet.GetPlanetOwner();

            MonoBehaviour.print("Planet number:" + i);
            i++;
            MonoBehaviour.print("Location:" + planet.GetPlanetLocation().ToString("F2"));
            MonoBehaviour.print("Owner:" + planet.GetPlanetOwner());
        }
    }
}
