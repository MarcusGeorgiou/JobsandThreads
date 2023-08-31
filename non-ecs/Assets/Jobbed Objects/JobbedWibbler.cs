using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class JobbedWibbler : MonoBehaviour
{
    public float wibbleSpeed;
    [HideInInspector]
    //public Vector3 myPos;
    public float3 myPos;

    private void Start()
    {
        wibbleSpeed = Random.Range(1f, 5f);
    }

    [BurstCompile]
    public void UpdateTransform(float yPos)
    {
        myPos = transform.position;
        myPos = new float3(myPos.x, yPos, myPos.z);
        transform.position = myPos;
    }

}
