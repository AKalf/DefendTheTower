using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightConfig 
{
    public static Dictionary<GameObject, List<GameObject>> ConfigUnitsTatgets(List<UnitBehaviour> squadWithMoreUnits, List<UnitBehaviour> squadWithLessUnits) {
        Dictionary<GameObject, List<GameObject>> unitsAndTargets = new Dictionary<GameObject, List<GameObject>>();
        int target = 0;
        for (int unit = 0; unit != squadWithMoreUnits.Count; unit++) {
          

        }

        //for (int target = 0; target != squadWithLessUnits.Count; target++)
        //{

        //}


        return unitsAndTargets;
    }
}
