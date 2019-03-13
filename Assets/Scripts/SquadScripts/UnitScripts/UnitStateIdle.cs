using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateIdle : IUnitStates
{
    static UnitStateIdle inst = null;
    public static UnitStateIdle GetInstance()
    {
        if (inst == null)
        {
            inst = new UnitStateIdle();
        }

        return inst;
    }
    private UnitStateIdle()
    {
        
    }
    public void OnStateEntry(UnitBehaviour unit)
    {
        unit.GetComponent<Animator>().SetTrigger("Idle");
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    }

    public void OnStateExit(UnitBehaviour unit)
    {
        
    }

    public void OnStateUpdate(UnitBehaviour unit)
    {
        
    }
}
