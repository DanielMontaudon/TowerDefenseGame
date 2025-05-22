using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TowerSelector : MonoBehaviour
{
    public GameObject towerLocation;
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject towerSelectionPanel;
    [SerializeField] private GameObject towerUpgradePanel;
    [SerializeField] private Text InsufficientFundsText;

    private void Awake()
    {
        if(!gameController)
            gameController = GameObject.FindGameObjectWithTag("GameController");
        towerSelectionPanel = GameObject.Find("TowerSelectionPanel");
        towerUpgradePanel = GameObject.Find("TowerUpgradeSelectionPanel");

    }

    public void AddLaserTower()
    {
        if (gameController.GetComponent<Points>().points >= 10)
        {
            towerLocation.GetComponent<TowerGenerator>().towerSelected = TOWER_TYPE.LASER;
            towerLocation.GetComponent<TowerGenerator>().AddTower();
            //towerLocation = null;
            gameController.GetComponent<Points>().points -= 10;
        }
        else
        {
            StartCoroutine(InsufficientFunds());
        }
    }

    public void AddGunTower()
    {
        if (gameController.GetComponent<Points>().points >= 20)
        {
            towerLocation.GetComponent<TowerGenerator>().towerSelected = TOWER_TYPE.GUN;
            towerLocation.GetComponent<TowerGenerator>().AddTower();
            //towerLocation = null;
            gameController.GetComponent<Points>().points -= 20;
        }
        else
        {
            StartCoroutine(InsufficientFunds());
        }
    }
    public void AddSniperTower()
    {
        if (gameController.GetComponent<Points>().points >= 50)
        {
            towerLocation.GetComponent<TowerGenerator>().towerSelected = TOWER_TYPE.SNIPER;
            towerLocation.GetComponent<TowerGenerator>().AddTower();
            //towerLocation = null;
            gameController.GetComponent<Points>().points -= 50;
        }
        else
        {
            StartCoroutine(InsufficientFunds());
        }
    }

    public void UpgradeSelectedTower()
    {
        bool hasFunds = towerLocation.GetComponent<TowerGenerator>().UpgradeTower();
        if(!hasFunds)
            StartCoroutine(InsufficientFunds());

    }
    public void CloseTowerPanel()
    {
        InsufficientFundsText.gameObject.SetActive(false);
        towerSelectionPanel.SetActive(false);
    }

    public void CloseUpgradePanel()
    {
        InsufficientFundsText.gameObject.SetActive(false);
        towerUpgradePanel.SetActive(false);
    }

    IEnumerator InsufficientFunds()
    {
        InsufficientFundsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        InsufficientFundsText.gameObject.SetActive(false);

    }
}
