using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitBehaviour : MonoBehaviour
{
    

    SquadBehaviour thisSquad = null;
    UnitStats thisUnitStats = null;


    
    public string currentStateToString ;
    IUnitStates thisUnitState;
   
    /// <summary>
    /// The initial relative position to the parent.
    /// </summary>
    Transform initTransform = null;
    [SerializeField]
    GameObject currentTarget = null; // if in combat state, this will be the target that this unit will fight
    AudioSource source = null;

    float timeDoingFormation = 0.0f;
   
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

        source = gameObject.AddComponent<AudioSource>();
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
        yield return new WaitForSeconds(timeDelay); 
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
    public void Move() {
        switch (thisUnitStats.GetRace())
        {
            case UnitStats.UnitRace.Human:
                if (source.clip == AudioManager.GetInstance().Cannon_Wheels && source.isPlaying)
                {

                }
                else
                {
                    source.loop = true;
                    MessageDispatch.GetInstance().SendAudioMessageForDispatch(AudioManager.SoundClipPrefab.Human_FootSteps, source);
                }
                break;
            case UnitStats.UnitRace.Elf:
                if (source.clip == AudioManager.GetInstance().Cannon_Wheels && source.isPlaying)
                {

                }
                else
                {
                    source.loop = true;
                    MessageDispatch.GetInstance().SendAudioMessageForDispatch(AudioManager.SoundClipPrefab.Horse_Footsteps, source);
                }
                break;
            case UnitStats.UnitRace.Dwarf:
                if (source.clip == AudioManager.GetInstance().Cannon_Wheels && source.isPlaying)
                {

                }
                else
                {
                    source.loop = true;
                    MessageDispatch.GetInstance().SendAudioMessageForDispatch(AudioManager.SoundClipPrefab.Cannon_Wheels, source);
                }

                break;
        }

    }
    
    public void Hit() {
        if (currentTarget != null) {
            float rnd = 0.5f;
            if (thisUnitStats.GetRace() == UnitStats.UnitRace.Dwarf && thisSquad.GetEnemyTargetedSquad() != null) {
                int enemiesCount = thisSquad.GetEnemyTargetedSquad().GetSquadUnits().Count;
                SquadBehaviour enemySquad = thisSquad.GetEnemyTargetedSquad();
                for (int x = 0; x != enemiesCount; x++) {
                    if (!(enemySquad.GetSquadUnits()[x].GetUnitStats().GetRace() == UnitStats.UnitRace.Elf)) {

                        StartCoroutine(FireCannon(enemySquad.GetSquadUnits()[x], Random.Range(rnd, x/1.5f)));
                    }
                }
                return;
            }
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
    public UnitStats GetUnitStats() {
        return thisUnitStats;
    }
    IEnumerator FireCannon(UnitBehaviour unit, float offset) {
       
        yield return new WaitForSeconds(offset);
        unit.GetUnitStats().ChangeHealthByAmount(-thisUnitStats.GetDamage());
    }
}
