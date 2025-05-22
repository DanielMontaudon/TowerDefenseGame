using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;
    [SerializeField] private GameObject enemyPrefab3;
    private int nEnemiesInWave = 0;
    public int EnemiesKilled = 0;
    public int EnemiesLeft = 0;
    private bool isSpawning = false;
    [SerializeField] private float spawnEnemyInWaveTime = 0.5f;

    [SerializeField] private Text EnemiesLeftText;
    [SerializeField] private Text ClearedText;
    [SerializeField] private Text FundsText;
    [SerializeField] private Text HealthText;


    private void Awake()
    {
        ClearedText.text = "";
    }

    private void Update()
    {
        Debug.Log(EnemiesKilled);
        EnemiesLeft = nEnemiesInWave - EnemiesKilled;
        EnemiesLeftText.text = "Enemies Left: " + EnemiesLeft.ToString();
        HealthText.text = "HP: " + gameObject.GetComponent<Points>().health.ToString();
        if (EnemiesKilled >= nEnemiesInWave && !isSpawning)
        {
            isSpawning = true;
            nEnemiesInWave += 1;
            StartCoroutine(SpawnWave());
            EnemiesKilled = 0;
            
        } 

        if(gameObject.GetComponent<Points>().health <= 0)
        {
            StartCoroutine(GameOver());
            //Application.Quit();

        }
        FundsText.text = "$" + gameObject.GetComponent<Points>().points.ToString();

    }

    IEnumerator SpawnWave()
    {
        
        ClearedText.gameObject.SetActive(true);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1;
        ClearedText.gameObject.SetActive(false);
        //yield return new WaitForSeconds(3);

        //nEnemiesInWave += 1;
        for (int i = 0; i < nEnemiesInWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnEnemyInWaveTime);
        }
        
        ClearedText.text = "Cleared";
        isSpawning = false;
    }


    void SpawnEnemy()
    {
        float ranChoice = Random.Range(0,11);
        if(ranChoice < 5)
            Instantiate(enemyPrefab1, transform);
        else if(ranChoice < 8)
            Instantiate(enemyPrefab2, transform);
        else
            Instantiate(enemyPrefab3, transform);
    }

    IEnumerator GameOver()
    {
        ClearedText.text = "GAME OVER";
        ClearedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
        //Application.Quit();
    }
}
