using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo{

    public int PlayerNumber = 0;
    public int NumberOfOwnedPlanets;
    public bool IsAlive = true;

    public PlayerInfo(int playernumber, int numberofownedplanets)
    {
        PlayerNumber = playernumber;
        NumberOfOwnedPlanets = numberofownedplanets;
    }
}
