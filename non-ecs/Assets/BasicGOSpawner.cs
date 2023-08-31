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

    [Header("Just know this is important")]
    public bool usingJobs;
    public CubeJobHandler jobHandler;

    private void Start()
    {
        spawnLocation = transform.position;
        StartCoroutine(SpawnObjs());
    }

    IEnumerator SpawnObjs()
    {
        while (true)
        {
            GameObject cube = Instantiate(prefab,spawnLocation, Quaternion.identity);
            spawnLocation += Vector3.right;
            cubesSpawned++;
            
            
            if(usingJobs)
                jobHandler.AddCube(cube);
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
