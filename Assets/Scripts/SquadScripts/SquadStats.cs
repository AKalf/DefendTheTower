using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SquadStats : MonoBehaviour
{
    [SerializeField]
    int health;
    [SerializeField]
    float speed;
    [SerializeField]
    int attackDamage;

    Vector3 targetLanePos = Vector3.zero;
    NavMeshAgent thisAgent = null;
    // holds the units that belong to this squad
    List<UnitBehaviour> squadUnits = new List<UnitBehaviour>();

    private void Awake()
    {
        

    }
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<SquadBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /**************************/
    #region GetMethods
    /**************************/
    public int GetAttackDamage() {
        return attackDamage;
    }
    public List<UnitBehaviour> GetSquadUnits()
    {
        return squadUnits;
    }
    public NavMeshAgent GetAgent() {
        return thisAgent;
    }
    public Vector3 GetTargetLane() {
        return targetLanePos;
    }

    // ..speed()

    // ..health()
    #endregion
    /**************************/


    // ...LooseHealth(int amount)
    // if (hp <= 0) {
    // SqaudBehaivour.TriggerDeathEvent()

    /**************************/
    #region UnitManagement
    /**************************/
    /// <summary>
    /// Add a new unit to the squad
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitToSquad(UnitBehaviour unit) {
        if (!GetSquadUnits().Contains(unit))
        {
            squadUnits.Add(unit);
        }
    }
    /// <summary>
    /// Remove a unit from the squad
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveUnitFromSquad(UnitBehaviour unit)
    {
        if (GetSquadUnits().Contains(unit))
        {
            squadUnits.Remove(unit);

            if (squadUnits.Count == 0 && this != null)
            {
                transform.DetachChildren();
                Destroy(this.gameObject);
            }
        }
    }
    #endregion
    /**************************/
    public void SetTargetLane(Vector3 pos)
    {
        targetLanePos = pos;
        thisAgent = this.gameObject.AddComponent<NavMeshAgent>();
        thisAgent.SetDestination(targetLanePos);
        
    }
}
