using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinManager : MonoBehaviour
{
    [SerializeField]
    int humanSquadCost = 8;
    [SerializeField]
    int elfSquadCost = 8;
    [SerializeField]
    int dwarfSquadCost = 8;



    [SerializeField]
    int headShotKillValue = 1;
    [SerializeField]
    int bodyKillValue = 1;
    [SerializeField]
    int playerUnitsKillValue = 1;

    [SerializeField]
    GameObject humanPanel = null;
    [SerializeField]
    GameObject elfPanel = null;
    [SerializeField]
    GameObject dwarfPanel = null;
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
        elfPanel.GetComponentInChildren<Text>().text = GetElfSquadCost().ToString();
        dwarfPanel.GetComponentInChildren<Text>().text = GetDwarfSquadCost().ToString();
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
        ActivateHumanPanel(amount);
    }
    void ActivateHumanPanel(int amount)
    {
        if (totalCoins >= humanSquadCost && humanPanel.activeInHierarchy == true)
        {
            humanPanel.SetActive(false);
            Debug.Log(amount.ToString());
        }
        else if (totalCoins < humanSquadCost && humanPanel.activeInHierarchy == false)
        {
            humanPanel.SetActive(true);
        }
    }
    void ActivateElfPanel(int amount)
    {
        if (totalCoins >= elfSquadCost && elfPanel.activeInHierarchy == true)
        {
            elfPanel.SetActive(false);
            Debug.Log(amount.ToString());
        }
        else if (totalCoins < elfSquadCost && elfPanel.activeInHierarchy == false)
        {
            elfPanel.SetActive(true);
        }
    }
    void ActivateDwarfPanel(int amount)
    {
        if (totalCoins >= dwarfSquadCost && dwarfPanel.activeInHierarchy == true)
        {
            dwarfPanel.SetActive(false);
            Debug.Log(amount.ToString());
        }
        else if (totalCoins < dwarfSquadCost && dwarfPanel.activeInHierarchy == false)
        {
            dwarfPanel.SetActive(true);
        }
    }
    public int GetTotalCoins() {
        return totalCoins;
    }
    public int GetHumanSquadCost()
    {
        return humanSquadCost;
    }
    public int GetElfSquadCost()
    {
        return elfSquadCost;
    }
    public int GetDwarfSquadCost()
    {
        return dwarfSquadCost;
    }

}
