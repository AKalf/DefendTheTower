using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawner : MonoBehaviour
{
    [SerializeField]
    List<Transform> thisPlayerSpawnPoints = new List<Transform>();
    [SerializeField]
    List<Transform> enemyPlayerSpawnPoints = new List<Transform>();
    int spawnPointIndex = 0;

    [SerializeField]
    GameObject humanSquadPrefab = null;

    float timerCounter = 0;
    float timeForNextSpawn = 5;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        timerCounter += Time.deltaTime;
        if (timerCounter > timeForNextSpawn) {
            SpawnUnit(humanSquadPrefab);
            timerCounter = 0;
        }
    }
    public void SpawnUnit(GameObject unit) {
        GameObject unitSpawned = Instantiate(unit, thisPlayerSpawnPoints[spawnPointIndex].position, thisPlayerSpawnPoints[spawnPointIndex].rotation);
        unitSpawned.GetComponent<SquadBehaviour>().SetTargetLane(enemyPlayerSpawnPoints[spawnPointIndex].position);

        spawnPointIndex++;
        if (spawnPointIndex > thisPlayerSpawnPoints.Count-1) {
            spawnPointIndex = 0;
        }
    }
}
