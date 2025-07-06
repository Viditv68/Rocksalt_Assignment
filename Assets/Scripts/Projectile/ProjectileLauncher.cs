using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    private PlayerControls controls;
    
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 2f;

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;
    
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float muzzleFlashDuration = 2f;
    
    [SerializeField] private Player player;


    private bool shouldFire = false;
    private float previousFireTime;
    private float muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        AssignInputEvents();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        
        Debug.Log("dstroy");
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("destroy");
    }

    private void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if (muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }
        if (!IsOwner)
        {
            return;
        }

        if (!shouldFire)
        {
            return;
        }

        if (Time.time < (1 / fireRate + previousFireTime))
        {
            return;
        }

        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
        
        previousFireTime = Time.time;

    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectile = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = direction;
        
        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());
        if (projectile.TryGetComponent<Damage>(out Damage damage))
        {
            damage.SetOwner(OwnerClientId);
        }
        
        if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
        
        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }
    
    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner)
        {
            return;
        }
       SpawnDummyProjectile(spawnPos, direction);
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;
        GameObject projectile = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = direction;
        
        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());

        if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }


    private void AssignInputEvents()
    {
        controls = player.controls;
 
        controls.Character.Fire.performed += context =>
        {

            shouldFire = true;
        };
        controls.Character.Fire.canceled += context =>
        {
            shouldFire = false;
        };

    }

}
