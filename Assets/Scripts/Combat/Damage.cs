using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;
    
    
    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        if (other.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
        {
            if (ownerClientId == networkObject.OwnerClientId)
            {
                return;
            }
        }
        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage((damage));
        }
            
        
    }
}
