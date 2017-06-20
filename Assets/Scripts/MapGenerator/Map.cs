using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map{

    private Vector3 PlanetLocation;
    private int PlanetOwner;

    public Map(Vector3 location, int owner)
    {
        PlanetLocation = location;
        PlanetOwner = owner;
    }

    public Vector3 GetPlanetLocation()
    {
        return PlanetLocation;
    }

    public int GetPlanetOwner()
    {
        return PlanetOwner;
    }
}
