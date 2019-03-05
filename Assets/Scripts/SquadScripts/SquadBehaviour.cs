using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SquadBehaviour : MonoBehaviour
{
    NavMeshAgent thisAgent = null;
    Vector3 targetLanePos = Vector3.zero;
    SquadStats thisSquadStats = null;


    enum SquadStates { Moving, Engaging, Idle }
    SquadStates currentState = SquadStates.Moving;
    bool init = false;

    // Start is called before the first frame update
    void Start()
    {
        thisSquadStats = GetComponent<SquadStats>();
        init = true;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (init) {
            SetSquadState(SquadStates.Moving);
            init = false;
            /****************  TO-DO  ******************/

            // If onMoving state
            // play moving animation
            // play moving sounds
            // ...
            // Else if OnAttacking state
            // Attack method
            // attack anim
            // attack sounds
            // ...
            // Else if OnWaitingForEnemySquadToApproach state
            // play idle anim OR taunt anim if we have
        }
    }
    /// <summary>
    /// Called by spawn manager when spawned, to set lane path.  
    /// </summary>
    /// <param name="pos"></param>
    public void SetTargetLane(Vector3 pos) {
        targetLanePos = pos;
        thisAgent = GetComponent<NavMeshAgent>();
        thisAgent.SetDestination(targetLanePos);
        init = true;
    }


    private void SetSquadState(SquadStates state) {
        switch (state) {
            case SquadStates.Idle:
                foreach (UnitBehaviour unit in GetSquadUnits()) {
                    unit.SetUnitState(UnitBehaviour.UnitStates.Idle);
                }
                break;
            case SquadStates.Moving:
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(UnitBehaviour.UnitStates.Moving);
                }
                break;
            case SquadStates.Engaging:
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(UnitBehaviour.UnitStates.Attacking);
                }
                break;

        }
        currentState = state;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        //if (this.tag == "Sq2") {
        //    if (other.GetComponent<SquadBehaviour>() != null && other.tag != this.tag) {
        //        SquadBehaviour enemySquad = other.GetComponent<SquadBehaviour>();
        //        SetSquadState(SquadStates.Engaging);
        //        if (enemySquad.GetSquadUnits().Count > this.GetSquadUnits().Count) {

        //        }



        //    }
        //}
    }

    public List<UnitBehaviour> GetSquadUnits() {
        return thisSquadStats.GetSquadUnits();
    }

    
}
