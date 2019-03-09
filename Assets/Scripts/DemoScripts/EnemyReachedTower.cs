using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReachedTower : MonoBehaviour
{

    [SerializeField]
    Transform towerTransform = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag.StartsWith("Sq2"))
        {
            SquadBehaviour squad = collision.transform.GetComponent<SquadBehaviour>();
            Debug.Log("Squad " + squad.name + " reached tower");
            foreach (UnitBehaviour unit in squad.GetSquadUnits()) {
                UIManager.GetInstance().AddPassedEnemy();
            }
            // collision.transform.GetComponent<UnitBehaviour>().StartCoroutine("Die");
            squad.SetSquadTarget(towerTransform.gameObject);
            foreach (UnitBehaviour unit in squad.GetSquadUnits()) {
                StartCoroutine(KillUnit(unit));
            }
        }
    }
    IEnumerator KillUnit(UnitBehaviour unit) {
        yield return new WaitForSeconds(12.5f);
        unit.Die();
    }
   
}
