using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float health = 50;
    [SerializeField] private float damage = 5;
    private bool alreadyDead = false;

    private GameObject prev;
    private GameObject next;

    [SerializeField] private GameObject gameController;


    private void Start()
    {
        if (!gameController)
            gameController = GameObject.FindGameObjectWithTag("GameController");
        prev = PathGenerator.path[0];
        transform.position = prev.transform.position;
    }

    private void Update()
    {
        if (Convert.ToInt32(prev.name) < PathGenerator.path.Count - 1)
        {
            next = PathGenerator.path[Convert.ToInt32(prev.name) + 1];

            transform.position += (next.transform.position - transform.position).normalized * speed * Time.deltaTime;
            Quaternion rot = Quaternion.LookRotation((next.transform.position - prev.transform.position).normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 5f);

            if (Vector3.Distance(transform.position, next.transform.position) <= 0.01f)
                prev = next;
        }
        else
        {
            //take damage
            Debug.Log("End");
            gameController.GetComponent<Points>().health -= damage;
            gameController.GetComponent<EnemySpawner>().EnemiesKilled += 1;
            Destroy(gameObject);
        }
    


    }

    public void TakeDamage(float dmg)
    {
        
        if (health - dmg <= 0 && !alreadyDead)
        {
            alreadyDead = true;
            Debug.Log("Died");
            gameController.GetComponent<EnemySpawner>().EnemiesKilled += 1;
            gameController.GetComponent<Points>().points += 10;
            Destroy(gameObject);
        }
        else
        {
            health -= dmg;
        }
    }
}
