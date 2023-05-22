using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCreatureHelper 
{
    MeleeCreatureController  creatureController;
    public MeleeCreatureHelper(MeleeCreatureController controller)
    {
        creatureController = controller;
    }
    public bool IsTargetOnSight()
    {
        //TODO
        return false;
    }
    public bool IstargetInRange()
    {
        //TODO
        return true;
    }

}
