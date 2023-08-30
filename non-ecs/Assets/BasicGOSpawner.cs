using System;
using System.Collections;
using UnityEngine;

public class BasicGOSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnTimer;
    
    public int cubesSpawned;
    private float targetFrameRate = 60f;
    private Vector3 spawnLocation;

    private void Start()
    {
        spawnLocation = transform.position;
        StartCoroutine(SpawnObjs());
    }

    IEnumerator SpawnObjs()
    {
        while (true)
        {
            Instantiate(prefab,spawnLocation, Quaternion.identity);
            spawnLocation += Vector3.right;
            cubesSpawned++;

            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
