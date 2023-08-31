using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;

public struct WibbleJob : IJobParallelFor
{
    public NativeArray<float> jobResult;

    public float jobTime;
    
    public NativeArray<float> jobWibbleSpeed;
    public NativeArray<float> cubeXPos;

    [BurstCompile]
    public void Execute(int index)
    {
        //Old Math stuff. Incompatible with burst compiler
        /*jobResult[index] = Mathf.PerlinNoise1D(jobTime + cubeXPos[index]) * jobWibbleSpeed[index];*/
        
        //New Math stuff. Is compatible with burst compiler
        jobResult[index] = noise.cnoise(new float2(jobTime + cubeXPos[index], 0)) * jobWibbleSpeed[index];
    }
}

public class CubeJobHandler : MonoBehaviour
{
    public List<JobbedWibbler> cubes;

    private NativeArray<float> readResult;
    private NativeArray<float> cubesWibbleSpeed;
    private NativeArray<float> cubesX;
    private JobHandle handle;
    
    //private Vector3 position;
    private float3 position;

    private WibbleJob jobData;

    public void AddCube(GameObject spawnedCube)
    {
        cubes.Add(spawnedCube.GetComponent<JobbedWibbler>());
    }
    
    [BurstCompile]
    private void Update()
    {
        InitializeJob();
        for (int i = 0; i < cubes.Count; i++)
        {
            position = cubes[i].myPos;

            jobData.jobWibbleSpeed[i] = cubes[i].wibbleSpeed;
            jobData.cubeXPos[i] = cubes[i].myPos.x;
        }
        //Do the job here
        handle = jobData.Schedule(cubes.Count, cubes.Count);
        
        //Some time later
        handle.Complete();
        for (int j = 0; j < readResult.Length; j++)
        {
            cubes[j].UpdateTransform(readResult[j]);
        }
        
        readResult.Dispose();
    }
    
    private void InitializeJob()
    {
        readResult = new NativeArray<float>(cubes.Count, Allocator.TempJob);
        cubesWibbleSpeed = new NativeArray<float>(cubes.Count, Allocator.TempJob);
        cubesX = new NativeArray<float>(cubes.Count, Allocator.TempJob);
        jobData = new WibbleJob()
        {
            jobWibbleSpeed = cubesWibbleSpeed,
            cubeXPos = cubesX,
            jobTime = Time.time,
            
            jobResult = readResult
        };
    }
}
