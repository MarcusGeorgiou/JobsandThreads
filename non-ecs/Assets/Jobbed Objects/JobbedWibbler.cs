using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

public struct WibbleJob : IJobParallelFor
{
    public NativeArray<float> jobResult;

    public float jobTime;
    
    public float jobWibbleSpeed;
    public float cubeXPos;

    public void Execute(int index)
    {
        jobResult[index] = Mathf.PerlinNoise1D(jobTime + cubeXPos) * jobWibbleSpeed;
    }
}

public class JobbedWibbler : MonoBehaviour
{
    public float wibbleSpeed;
    private Vector3 position;
    
    private NativeArray<float> readResult;
    private JobHandle handle;

    private void Start()
    {
        wibbleSpeed = Random.Range(1f, 5f);
    }

    private void Update()
    {
        position = transform.position;
        
        readResult = new NativeArray<float>(1, Allocator.TempJob);
        WibbleJob jobData = new WibbleJob
        {
            jobWibbleSpeed = wibbleSpeed,
            jobTime = Time.time,
            cubeXPos = position.x,
            
            jobResult = readResult
        };
        
        //Do the job here
        handle = jobData.Schedule(1, Int32.MaxValue);
        
        //Some time later
        handle.Complete();
        position = new Vector3(position.x, readResult[0], position.z);
        transform.position = position;
        
        readResult.Dispose();
    }

}
