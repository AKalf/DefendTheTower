using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateFormation : IUnitStates
{
    static UnitStateFormation inst = null;
    public static UnitStateFormation GetInstance()
    {
        if (inst == null)
        {
            inst = new UnitStateFormation();
        }

        return inst;
    }
    private UnitStateFormation()
    {
       
    }
    public void OnStateEntry(UnitBehaviour unit)
    {
        unit.SetUnitCurrentTarget(null);
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(unit.GetUnitInitalPosInSquad().position);
        unit.GetComponent<Animator>().SetTrigger("Moving");
    }

    public void OnStateExit(UnitBehaviour unit)
    {
       
    }

    public void OnStateUpdate(UnitBehaviour unit)
    {
        if (Vector3.Distance(unit.transform.position, unit.GetUnitInitalPosInSquad().position) <= 1.0f)
        {
            unit.SetUnitState(UnitStateIdle.GetInstance());
        }
    }
}
