using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    public static HostManager Instance { get; private set; }

    public HostGameManager GameManager { get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();

    }

    private void OnDestroy()
    {
        GameManager?.Dispose();
    }
}
