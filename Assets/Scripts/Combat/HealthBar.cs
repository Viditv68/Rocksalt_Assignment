using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthBar;

    public override void OnNetworkSpawn()
    {
        if (!IsClient)
        {
            return;
        }

        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
        HandleHealthChanged(0, health.MaxHealth);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient)
        {
            return;
        }
        health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
        
    }

    private void HandleHealthChanged(int oldHealth, int newHealth)
    {
        healthBar.fillAmount = (float)newHealth / health.MaxHealth;
    }
}
