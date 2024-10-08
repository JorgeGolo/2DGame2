using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public string currentScene;
    public int coins;
    public int experience;
    public int weaponLevel;
    public int hitpoint;
    public int maxHitPoint;

    public Vector3 posicionRespawn;

    public List<EnemyData> enemyList;


}

[System.Serializable]

public class EnemyData
{
    public string name;
    public bool isAlive;
}
