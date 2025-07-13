using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPoint : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    
    private static List<Transform>  spawnPoints= new List<Transform>();
    
    private void OnEnable()
    {
        foreach (Transform spawnPoint in SpawnPoints)
        {
            spawnPoints.Add(spawnPoint);
        }
    }

    private void OnDisable()
    {
        spawnPoints.Clear();
    }

    public static Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints.Count == 0)
        {
            return Vector3.zero;
        }
        return spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform.position;
    }

}
