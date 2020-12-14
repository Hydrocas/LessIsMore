using Com.IsartDigital.DontLetThemFall.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject controllerPrefab = null;
    [SerializeField] private Player[] players = null;
    [SerializeField] private InputActionAsset[] inputActionAssets = null;

    private int playerCount = 0;

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.actions = inputActionAssets[playerCount];
        Player player = players[playerCount];
        player.ListenController(playerInput.gameObject.GetComponent<Controller>());

        playerCount++;
    }

    private void Awake()
    {
        PlayerInputManager playerInputManager = GetComponent<PlayerInputManager>();

        playerInputManager.playerPrefab = controllerPrefab;
        playerInputManager.JoinPlayer();
        //playerInputManager.JoinPlayer();
    }
}
