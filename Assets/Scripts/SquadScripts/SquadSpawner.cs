using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquadSpawner : MonoBehaviour
{
    [SerializeField] [Tooltip("Spawning points")]
    List<Transform> thisPlayerSpawnPoints = new List<Transform>();
    [SerializeField] [Tooltip("Units will march to this positions")]
    List<Transform> enemyPlayerSpawnPoints = new List<Transform>();
    int spawnPointIndex = 0;

    [SerializeField]
    GameObject humanSquadPrefab = null;
    [SerializeField]
    GameObject elfSquadPrefab = null;
    [SerializeField]
    GameObject dwarfSquadPrefab = null;
    [SerializeField]
    int howManyTimesSpawned = 0; 
    float timerCounter = 0;
    [SerializeField]
    List<UnitStats.UnitRace> wave;
    [SerializeField]
    List<float> wavesOffsetFromPreviousWave;

    /// <summary>
    ///  counts whether the spawn delay should decrease
    /// </summary>
    bool isThisPlayerSpawner = false;
    // Start is called before the first frame update
    void Start()
    {
        if (tag == "PlSp")
        {
            isThisPlayerSpawner = true;
        }
        else {
            //SpawnUnit(UnitStats.UnitRace.Human);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isThisPlayerSpawner)
        {
            if (wave.Count > 0)
            {
                timerCounter += Time.deltaTime;
                if (timerCounter >= wavesOffsetFromPreviousWave[0])
                {
                    SpawnUnit(wave[0]);
                    timerCounter = 0;
                    wave.Remove(wave[0]);
                    wavesOffsetFromPreviousWave.Remove(wavesOffsetFromPreviousWave[0]);
                }
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Alpha1) && CoinManager.GetInstance().GetTotalCoins() >= CoinManager.GetInstance().GetHumanSquadCost())
            {
                SpawnUnit(UnitStats.UnitRace.Human);
                CoinManager.GetInstance().ChangeTotalCoinsByAmount(-CoinManager.GetInstance().GetHumanSquadCost());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && CoinManager.GetInstance().GetTotalCoins() >= CoinManager.GetInstance().GetElfSquadCost())
            {
                SpawnUnit(UnitStats.UnitRace.Elf);
                CoinManager.GetInstance().ChangeTotalCoinsByAmount(-CoinManager.GetInstance().GetElfSquadCost());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && CoinManager.GetInstance().GetTotalCoins() >= CoinManager.GetInstance().GetDwarfSquadCost())
            {
                SpawnUnit(UnitStats.UnitRace.Dwarf);
                CoinManager.GetInstance().ChangeTotalCoinsByAmount(-CoinManager.GetInstance().GetDwarfSquadCost());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                SpawnUnit(UnitStats.UnitRace.Human);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SpawnUnit(UnitStats.UnitRace.Elf);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SpawnUnit(UnitStats.UnitRace.Dwarf);
            }
        }
    }
    public void SpawnUnit(UnitStats.UnitRace race) {
        GameObject squad = null;
        switch (race) {
            case UnitStats.UnitRace.Human:
                squad = humanSquadPrefab;
                break;
            case UnitStats.UnitRace.Elf:
                squad = elfSquadPrefab;
                break;
            case UnitStats.UnitRace.Dwarf:
                squad = dwarfSquadPrefab;
                break;
        }
        GameObject squadSpawned = Instantiate(squad, thisPlayerSpawnPoints[spawnPointIndex].position, squad.transform.rotation);
        squadSpawned.GetComponent<SquadStats>().SetTargetLane(enemyPlayerSpawnPoints[spawnPointIndex].position);
     
    
        spawnPointIndex++;
        if (spawnPointIndex > thisPlayerSpawnPoints.Count-1) {
            spawnPointIndex = 0;
        }
    }
}
