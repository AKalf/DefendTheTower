using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadStats : MonoBehaviour
{
    [SerializeField]
    int health;
    [SerializeField]
    float speed;
    [SerializeField]
    int attackDamage;

    // holds the units that belong to this squad
    List<UnitBehaviour> squadUnits = new List<UnitBehaviour>();


    // Start is called before the first frame update
    void Start()
    {
        
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
        squadUnits.Add(unit);
    }
    /// <summary>
    /// Remove a unit from the squad
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveUnitToSquad(UnitBehaviour unit)
    {
        squadUnits.Remove(unit);

        if (squadUnits.Count == 0) {
            Destroy(this.gameObject);
        } 
    }
    #endregion
    /**************************/
}
