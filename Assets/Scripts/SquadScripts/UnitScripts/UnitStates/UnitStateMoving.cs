using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitStateMoving : IUnitStates
{
    static UnitStateMoving inst = null;

    public static UnitStateMoving GetInstance() {
        if (inst == null) {
            inst = new UnitStateMoving();
        }
        
        return inst;
    }
    private UnitStateMoving() {
        
    }
    public void OnStateEntry(UnitBehaviour unit)
    {
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        unit.GetComponent<Animator>().SetTrigger("Moving");
    }

    public void OnStateExit(UnitBehaviour unit)
    {
        
    }

    public void OnStateUpdate(UnitBehaviour unit)
    {
        
    }

    
}
