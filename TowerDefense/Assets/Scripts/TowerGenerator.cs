using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private GameObject towerSelectionPanel;
    [SerializeField] private GameObject towerUpgradeSelectionPanel;

    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject thisTower;
    [SerializeField] private GameObject towerLevelText;
    [SerializeField] private GameObject towerDamageText;
    [SerializeField] private GameObject UpgradeButtonText;


    //[SerializeField] private GameObject InsufficientFundsText;




    private int numbOfUpgrades = 0;


    [SerializeField] private bool hasTower = false;
    public TOWER_TYPE towerSelected = TOWER_TYPE.TOWER_COUNT;

    private void Awake()
    {
        if (!gameController)
            gameController = GameObject.FindGameObjectWithTag("GameController");
        towerSelectionPanel = GameObject.Find("TowerSelectionPanel");
        towerUpgradeSelectionPanel = GameObject.Find("TowerUpgradeSelectionPanel");
        towerLevelText = GameObject.Find("TowerLevelText");
        towerDamageText = GameObject.Find("TowerDamageText");
        UpgradeButtonText = GameObject.Find("UpgradeButtonText");

    }

    private void Start()
    {
        towerSelectionPanel.SetActive(false);
        towerUpgradeSelectionPanel.SetActive(false);
        //InsufficientFundsText.SetActive(false);

    }
    public void AddTower()
    {
        if (!hasTower)
        {
            if (towerSelected == TOWER_TYPE.TOWER_COUNT)
            {
                //open selection
                towerSelectionPanel.GetComponent<TowerSelector>().towerLocation = gameObject;
                towerUpgradeSelectionPanel.SetActive(false);
                towerSelectionPanel.SetActive(true);

            }
            else
            {

                thisTower = Instantiate(towerPrefabs[(int)towerSelected], transform.position, Quaternion.identity);
                Debug.Log((int)towerSelected);
                hasTower = true;
                //close selection
                towerSelectionPanel.SetActive(false);
                towerSelected = TOWER_TYPE.TOWER_COUNT;
            }
        }
        else
        {
            towerUpgradeSelectionPanel.GetComponent<TowerSelector>().towerLocation = gameObject;
            towerLevelText.GetComponent<Text>().text = "Tower Level: " + numbOfUpgrades;
            towerDamageText.GetComponent<Text>().text = "Tower DPS: " + (thisTower.GetComponent<Tower>().towerDamage * thisTower.GetComponent<Tower>().shootRate);
            if (thisTower.GetComponent<Tower>().towerType == TOWER_TYPE.LASER)
                UpgradeButtonText.GetComponent<Text>().text = "Upgrade Tower: 15";
            if (thisTower.GetComponent<Tower>().towerType == TOWER_TYPE.GUN)
                UpgradeButtonText.GetComponent<Text>().text = "Upgrade Tower: 25";
            if (thisTower.GetComponent<Tower>().towerType == TOWER_TYPE.SNIPER)
                UpgradeButtonText.GetComponent<Text>().text = "Upgrade Tower: 30";
            towerSelectionPanel.SetActive(false);
            towerUpgradeSelectionPanel.SetActive(true);
        }
    }


    public bool UpgradeTower()
    {
        bool ret = true;
        Tower towerStats = thisTower.GetComponent<Tower>();
        if (numbOfUpgrades < 3)
        {
            if (towerStats.towerType == TOWER_TYPE.LASER)
            {
                if (gameController.GetComponent<Points>().points >= 15)
                {
                    towerStats.towerDamage += 0.4f;
                    gameController.GetComponent<Points>().points -= 15;
                    numbOfUpgrades++;
                    towerLevelText.GetComponent<Text>().text = "Tower Level: " + numbOfUpgrades;
                    //towerDamageText.GetComponent<Text>().text = "Tower Damage: " + towerStats.towerDamage;

                }
                else
                {
                    ret = false;
                }
            }
            else if (towerStats.towerType == TOWER_TYPE.GUN)
            {
                if (gameController.GetComponent<Points>().points >= 25)
                {
                    towerStats.towerDamage += 2;
                    gameController.GetComponent<Points>().points -= 25;
                    numbOfUpgrades++;
                    towerLevelText.GetComponent<Text>().text = "Tower Level: " + numbOfUpgrades;
                }
                else
                {
                    ret = false;
                }
            }
            else if (towerStats.towerType == TOWER_TYPE.SNIPER)
            {
                if (gameController.GetComponent<Points>().points >= 30)
                {
                    towerStats.towerDamage += 15;
                    gameController.GetComponent<Points>().points -= 30;
                    numbOfUpgrades++;
                    towerLevelText.GetComponent<Text>().text = "Tower Level: " + numbOfUpgrades;
                }
                else
                {
                    ret = false;
                }
            }
            towerDamageText.GetComponent<Text>().text = "Tower DPS: " + (towerStats.towerDamage * towerStats.shootRate);

        }
        return ret;
        //towerUpgradeSelectionPanel.SetActive(false);
    }
}
