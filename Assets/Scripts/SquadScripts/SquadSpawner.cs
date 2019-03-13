using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawner : MonoBehaviour
{
    [SerializeField][Tooltip ("Spawning points")]
    List<Transform> thisPlayerSpawnPoints = new List<Transform>();
    [SerializeField][Tooltip("Units will march to this positions")]
    List<Transform> enemyPlayerSpawnPoints = new List<Transform>();
    int spawnPointIndex = 0;

    [SerializeField]
    GameObject humanSquadPrefab = null;
    [SerializeField]
    int numberOfTotalSpawns = 1;
    int howManyTimesSpawned = 0;
    float timerCounter = 0;
    [SerializeField][Tooltip("Delay time between spawns")]
    float timeForNextSpawn = 5;
    [SerializeField][Tooltip("Decrease spawn delay by this amount")]
    float decreasyTimeForNextSpawnBy = 0.25f;
    [SerializeField][Tooltip("after this seconds, spawn delay will be decreased")]
    float afterSeconds = 10f;
    /// <summary>
    ///  counts whether the spawn delay should decrease
    /// </summary>
    float timeCounterForDecSpawnTime = 0.0f; 

    bool isThisPlayerSpawner = false;
    // Start is called before the first frame update
    void Start()
    {
        if (tag == "PlSp")
        {
            isThisPlayerSpawner = true;
        }
        else {
            SpawnUnit(humanSquadPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isThisPlayerSpawner)
        {
            timeCounterForDecSpawnTime += Time.deltaTime;
            if (timeCounterForDecSpawnTime >= afterSeconds)
            {
                timeForNextSpawn -= decreasyTimeForNextSpawnBy;
                timeCounterForDecSpawnTime = 0.0f;
            }
            if (howManyTimesSpawned < numberOfTotalSpawns)
            {
                timerCounter += Time.deltaTime;
                if (timerCounter > timeForNextSpawn)
                {
                    SpawnUnit(humanSquadPrefab);
                    timerCounter = 0;
                    howManyTimesSpawned++;
                }
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SpawnUnit(humanSquadPrefab);
            }
        }
    }
    public void SpawnUnit(GameObject unit) {
        GameObject unitSpawned = Instantiate(unit, thisPlayerSpawnPoints[spawnPointIndex].position, thisPlayerSpawnPoints[spawnPointIndex].rotation);
        unitSpawned.GetComponent<SquadBehaviour>().SetTargetLane(enemyPlayerSpawnPoints[spawnPointIndex].position);
        unitSpawned.transform.LookAt(enemyPlayerSpawnPoints[spawnPointIndex].position);
        spawnPointIndex++;
        if (spawnPointIndex > thisPlayerSpawnPoints.Count-1) {
            spawnPointIndex = 0;
        }
    }
}
