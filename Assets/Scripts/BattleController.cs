using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour {

	public Transform playerPrefab;

	void OnPlayerConnected(NetworkPlayer player) {
		SpawnPlayer(player);
	}

	void SpawnPlayer(NetworkPlayer player) {
		Transform newPlayer = (Transform)Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
		NetworkView newNetworkView = newPlayer.networkView;
		newNetworkView.RPC ("SetPlayer", RPCMode.AllBuffered, player);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
