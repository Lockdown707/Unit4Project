using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    private float spawnPosY = 20;
    public float xMin = -10;
    public float xMax = 10;
    public int waveNumber = 1;
    public int enemyCount;
    public GameObject powerupPrefab;


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        //Instantiate(powerupPrefab, GenerateSpawnPostion(), powerupPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        

        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab, RandomPos(), powerupPrefab.transform.rotation);
        }
    }
    
    public Vector2 RandomPos()
    {
        return new Vector2(Random.Range(xMin, xMax), spawnPosY);
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefabs[0], RandomPos(), enemyPrefabs[0].transform.rotation);
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
            SpawnEnemy();
    }
    //Hi
}
