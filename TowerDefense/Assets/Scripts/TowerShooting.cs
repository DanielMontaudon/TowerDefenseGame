using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class TowerShooting : MonoBehaviour
{
    [SerializeField] private LineRenderer laser;
    [SerializeField] private Transform gunTip;
    [SerializeField] private Transform gunHead;
    [SerializeField] private ParticleSystem gun;
    [SerializeField] private ParticleSystem sniper;

    private AudioSource s;

    private Tower tower;
    private GameObject target;
    private float nextShootTime = 0;

    private void Start()
    {
        s = gameObject.GetComponent<AudioSource>();

        tower = GetComponent<Tower>();
        if(!laser)
            laser = GetComponent<LineRenderer>();
        InvokeRepeating("SelectTarget", 0, .5f);
    }

    private void Update()
    {

        if(target == null)
        {
            laser.enabled = false;
            return;
        }
        // Lock on to target
        LockOnTarget();
        // Shoot the target
        if(Time.time > nextShootTime)
        {

            //visual shooting
            if(tower.towerType == TOWER_TYPE.LASER)
            {
                ShootLaser();
            }
            else if(tower.towerType == TOWER_TYPE.GUN)
            {
                ShootGun();
            }
            else if(tower.towerType == TOWER_TYPE.SNIPER)
            {
                ShootSniper();
            }
                    //apply damage
            target.GetComponent<Enemy>().TakeDamage(tower.towerDamage);
            
            nextShootTime = Time.time + 1 / tower.shootRate;
        }
    }

    private void LockOnTarget()
    {
        Vector3 dir = target.transform.position - gunHead.position;
        gunHead.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(-90,0,0);
    }

    private void ShootLaser()
    {
        //Debug.Log("Laser Shooting");
        s.Play();
        laser.enabled = true;
        laser.SetPosition(0, gunTip.position + new Vector3(0, .3f, 0));
        laser.SetPosition(1, target.transform.position);

    }

    private void ShootGun()
    {
        s.Play();
        gun.Play();
        //Debug.Log("Gun Shooting");
    }

    private void ShootSniper()
    {
        s.Play();
        sniper.Play();
        //Debug.Log("Sniper Shooting");

    }

    private void SelectTarget()
    {
        //get all enemies
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //nearest to tower
        int selectedEnemy = -1;
        float minDist = Mathf.Infinity;
        for (int i = 0; i < allEnemies.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, allEnemies[i].transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                selectedEnemy = i;
            }
        }

        //select target
        if (selectedEnemy != -1 && minDist <= tower.towerRange)
            target = allEnemies[selectedEnemy];
        else
            target = null;
    }
}
