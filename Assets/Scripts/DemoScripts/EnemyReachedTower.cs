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
        if (collision.transform.tag == "Squad")
        {
            UIManager.GetInstance().AddPassedEnemy();
            // collision.transform.GetComponent<UnitBehaviour>().StartCoroutine("Die");
            collision.transform.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(towerTransform.position);
        }
    }
   
}
