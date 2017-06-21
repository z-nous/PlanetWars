using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Map{

    //private Vector3 PlanetLocation;
    private float x;
    private float y;
    private float z;
    private int PlanetOwner;

    public Map(Vector3 location, int owner)
    {
        //PlanetLocation = location;
        PlanetOwner = owner;
        x = location.x;
        y = location.y;
        z = location.z;
    }

    public Vector3 GetPlanetLocation()
    {
        var PlanetLocation = new Vector3();
        PlanetLocation.Set(x, y, z);
        return PlanetLocation;
    }

    public int GetPlanetOwner()
    {
        return PlanetOwner;
    }
}
