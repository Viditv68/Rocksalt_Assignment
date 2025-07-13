using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private PlayerTank player;
    [SerializeField] private TMP_Text playerName;
    private void OnEnable()
    {
        HandlePlayerNameChanged(string.Empty, player.playerName.Value);
        player.playerName.OnValueChanged += HandlePlayerNameChanged;
    }

    private void HandlePlayerNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        playerName.text = newName.Value;
    }

    private void OnDestroy()
    {
        player.playerName.OnValueChanged -= HandlePlayerNameChanged;
    }
}
