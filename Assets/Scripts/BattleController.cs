using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour {

	public GameObject playerCreatorPrefab;
	public GameObject playerOwnerPrefab;
	public GameObject playerProxyPrefab;
	
	private Vector3 spawnLocation = Vector3.zero;
	private Quaternion spawnRotation = Quaternion.identity;

	void uLink_OnPlayerConnected(uLink.NetworkPlayer player) {
		SpawnPlayer(player);
	}

	void SpawnPlayer(uLink.NetworkPlayer player) {
		uLink.Network.Instantiate(player, playerProxyPrefab, playerOwnerPrefab, playerCreatorPrefab, spawnLocation, spawnRotation, 0);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
