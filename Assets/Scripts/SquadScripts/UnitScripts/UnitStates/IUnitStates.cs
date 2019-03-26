using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStates
{
    void OnStateEntry(UnitBehaviour unit);
    void OnStateExit(UnitBehaviour unit);
    void OnStateUpdate(UnitBehaviour unit);
}
