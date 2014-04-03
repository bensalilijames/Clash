using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour {

	public Transform playerPrefab;

	void OnPlayerConnected(NetworkPlayer player) {
		SpawnPlayer(player);
	}

	void SpawnPlayer(NetworkPlayer player) {
		GameObject newPlayer = (GameObject)Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
		NetworkView newNetworkView = newPlayer.networkView;
		newNetworkView.RPC ("CreatePlayer", RPCMode.AllBuffered, player);
	}

	[RPC]
	void CreatePlayer(NetworkPlayer player) {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
