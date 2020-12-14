using Com.IsartDigital.DontLetThemFall.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour {
	[SerializeField] private GameObject controllerPrefab = null;
	[SerializeField] private Player[] players = null;
	[SerializeField] private InputActionAsset[] inputActionAssets = null;

	private int playerCount = 0;

	//Essayer d'avoir tout les axis pour un seul controller et ensuite les player prennent leurs valeur
	//Pour le moment aucun controller ne se crée, je ne sais pas pourquoi (Problème dans les input actions ?)

	private void OnPlayerJoined(PlayerInput playerInput) {
		playerInput.actions = inputActionAssets[playerCount];
		//playerInput.defaultActionMap = "Player" + playerCount;
		Player player = players[playerCount];
		player.ListenController(playerInput.gameObject.GetComponent<Controller>());

		playerCount++;
	}

	private void Awake() {
		PlayerInputManager playerInputManager = GetComponent<PlayerInputManager>();

		playerInputManager.playerPrefab = controllerPrefab;
		playerInputManager.JoinPlayer();
		playerInputManager.JoinPlayer();
	}
}
