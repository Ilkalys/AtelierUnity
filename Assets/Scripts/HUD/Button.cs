using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public int id;
    public Player player;
    public void CreateUnit()
    {
        WorldObject wO = player.SelectedObjects[0];
        if (wO)
        {
            if (wO is BuildingSpawner)
            {
                BuildingSpawner BS = (BuildingSpawner)wO;
                BS.AskForUnit(id);
            }
            else if(wO is BuilderUnit)
            {
                BuilderUnit BU = (BuilderUnit)wO;
                BU.AskForBuild(id);
            }
        }
    }

    public void AbortCreation()
    {
        WorldObject wO = player.SelectedObjects[0];
        if (wO)
        {
            if (wO is BuildingSpawner)
            {
                BuildingSpawner BS = (BuildingSpawner)wO;
                BS.AbortUnitCreation(id);
            }
        }
    }
}
