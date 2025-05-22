using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TOWER_TYPE { LASER, GUN, SNIPER, TOWER_COUNT };
public class Tower : MonoBehaviour
{
    public TOWER_TYPE towerType = TOWER_TYPE.LASER;
    public float shootRate = 50;
    public float towerDamage = 10;
    public float towerRange = 5;

}
