using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitBehaviour : MonoBehaviour
{
    

    SquadBehaviour thisSquad = null;
    UnitStats thisUnitStats = null;


    
    public string currentStateToString ;
    IUnitStates thisUnitState = null;
   
    /// <summary>
    /// The initial relative position to the parent.
    /// </summary>
    Transform initTransform = null;
    [SerializeField]
    GameObject currentTarget = null; // if in combat state, this will be the target that this unit will fight


    float timeDoingFormation = 0.0f;
    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        thisUnitState = UnitStateIdle.GetInstance();
        thisSquad = transform.parent.GetComponent<SquadBehaviour>();
        thisUnitStats = GetComponent<UnitStats>();
        thisSquad.GetComponent<SquadStats>().AddUnitToSquad(this);
        
        GameObject initialTransform = new GameObject();
        initialTransform.transform.position = transform.position;
        initialTransform.transform.parent = transform.parent;
        initTransform = initialTransform.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisUnitState != null) {
            thisUnitState.OnStateUpdate(this);
        } 
    }
    
   
   
   /// <summary>
   /// If there is an enemy target unit, it informs it that it is dead. Same for enemy squad.
   /// It is coroutine.
   /// </summary>
   /// <param name="timeOffset">offset unit death event triggers</param>
   /// <returns></returns>
    public IEnumerator  OnDeath(float timeDelay) {
        yield return new WaitForSeconds(5); // after 5 seconds
        if (this != null)
        {
            Destroy(this.gameObject); // destroy this gameobject
        }
    }

    public void SetUnitState(IUnitStates stateToGo) {
        if (UnitStatesManager.GetIfUnitTransitionPossible(this, stateToGo))
        {
            GetCurrentState().OnStateExit(this);
            thisUnitState = stateToGo;
            GetCurrentState().OnStateEntry(this);
            currentStateToString = thisUnitState.ToString();
        }
        else {
            try {
                Debug.LogError("Unit " + gameObject.name + " tryied invalid transition: " + GetCurrentState().ToString() + ", to " + stateToGo.ToString());

            }
            catch  {

            }
        }
    }
    
    
    
    /***************/
    #region Animation Events
    public void FootR() {

    }
    public void FootL()
    {

    }
    public void Hit() {
        if (currentTarget != null) {
            UnitStats enemyUnitStats = currentTarget.GetComponent<UnitStats>();
            if (enemyUnitStats != null)
            {
                enemyUnitStats.ChangeHealthByAmount(-thisUnitStats.GetDamage());
            }
            else if (currentTarget.GetComponent<StructureStatsScript>() != null) {
                currentTarget.GetComponent<StructureStatsScript>().ChangeHealthByAmount(-thisUnitStats.GetDamage());
            }
        }
    }
    #endregion
    /***************/
    
    
    public IUnitStates GetCurrentState() {
        return thisUnitState;
    }
    public GameObject GetUnitCurrentTarget() {
        return currentTarget;
    }
    public void SetUnitCurrentTarget(GameObject target)
    {
        currentTarget = target;
    }
    public Transform GetUnitInitalPosInSquad() {
        return initTransform;
    }
    public SquadBehaviour GetThisUnitSquad() {
        return thisSquad;

    }
}
