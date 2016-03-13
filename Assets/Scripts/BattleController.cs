using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour
{
	public GameObject playerCreatorPrefab;
	public GameObject playerOwnerPrefab;
	public GameObject playerProxyPrefab;
	
	public GameObject[] players = new GameObject[5];
	private int playerCount = 0;

	private GameObject[] turrets = null;
		
	private Vector3 spawnLocation = Vector3.zero;
	private Quaternion spawnRotation = Quaternion.identity;

	void Start()
	{
		turrets = GameObject.FindGameObjectsWithTag("Turret");
		if (turrets == null)
		{
			Debug.Log("Turrets not found!");
		}
	}

	void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
	{
		SpawnPlayer(player);
	}

	void SpawnPlayer(uLink.NetworkPlayer player)
	{
		Debug.Log("Player initialised with an ID of " + playerCount);
		players[playerCount] = uLink.Network.Instantiate(player, playerProxyPrefab, playerOwnerPrefab, playerCreatorPrefab, spawnLocation, spawnRotation, 0);
		playerCount++;
	}

	public int GetGameObjectID(GameObject gameObjectToGet)
	{
		if (gameObjectToGet == null)
		{
			Debug.Log("gameObjectToGet is null");
			return -1;
		}
		
		for (int i = 0; i < playerCount; i++)
		{
			if (players[i] == null)
			{
				Debug.Log("Player at count " + i + " is null");
			}
			if (gameObjectToGet == players[i])
			{
				return i;
			}
		}
		
		for (int i = 0; i < turrets.Length; i++)
		{
			if (gameObjectToGet == turrets[i])
			{
				return 5 + i;
			}
		}
		return -1;
	}

	public GameObject GetGameObject(int ID)
	{
		if (ID >= 0 && ID < 5)
		{
			return players[ID];
		}
		else if (ID >= 5 && ID < 5 + turrets.Length)
		{
			return turrets[ID - 5];
		}
		else
		{
			return null;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.Write(playerCount);
			for (int i = 0; i < playerCount; i++)
			{
				if (players[i] != null)
				{
					stream.Write(uLinkNetworkView.Get(players[i]).viewID);
				}
			}
		}
		else
		{
			playerCount = stream.Read<int>();
			for (int i = 0; i < playerCount; i++)
			{
				players[i] = uLinkNetworkView.Find(stream.Read<uLink.NetworkViewID>()).gameObject;
			}
		}
	}
}
