using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinManager : MonoBehaviour
{
    [SerializeField]
    int humanSquadCost = 8;

   

    [SerializeField]
    int headShotKillValue = 1;
    [SerializeField]
    int bodyKillValue = 1;
    [SerializeField]
    int playerUnitsKillValue = 1;

    [SerializeField]
    GameObject humanPanel = null;
    static CoinManager inst = null;
    int totalCoins = 0;

    public static CoinManager GetInstance() {
        return inst;
    }
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        humanPanel.GetComponentInChildren<Text>().text = GetHumanSquadCost().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetHeadValue() {
        return headShotKillValue;
    }
    public int GetBodyValue() {
        return bodyKillValue;
    }
    public int GetPlayerUnitKillValue() {
        return playerUnitsKillValue;
    }
    public void ChangeTotalCoinsByAmount (int amount) {
        totalCoins += amount;
        GUIManager.GetInstance().InformCoinText(totalCoins);
        if (totalCoins >= humanSquadCost && humanPanel.activeInHierarchy == true) {
            humanPanel.SetActive(false);
            Debug.Log(amount.ToString());
        }
        else if (totalCoins < humanSquadCost && humanPanel.activeInHierarchy == false) {
            humanPanel.SetActive(true);
        }
    }
    public int GetTotalCoins() {
        return totalCoins;
    }
    public int GetHumanSquadCost() {
        return humanSquadCost;
    }
     
}
