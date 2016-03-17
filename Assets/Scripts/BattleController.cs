using UnityEngine;
using System.Collections;

public enum TeamTag {
	Red,
	Blue
}

public class BattleController : MonoBehaviour
{
	public GameObject playerCreatorPrefab;
	public GameObject playerOwnerPrefab;
	public GameObject playerProxyPrefab;
	public GameObject aiPrefab;

	public GameObject[] players = new GameObject[5];
	private int playerCount = 0;

	private GameObject[] turrets = null;
		
	private Vector3 spawnLocation = new Vector3(10.0f, 0.5f, 10.0f);
	private Vector3 aiSpawnLocation = new Vector3(20.0f, 0.5f, 20.0f);
	private Quaternion spawnRotation = Quaternion.identity;

	private int minionSpawnCount;
	private float lastMinionSpawnTime;

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
		SpawnAI();
	}

	void SpawnPlayer(uLink.NetworkPlayer player)
	{
		Debug.Log("Player initialised with an ID of " + playerCount);
		players[playerCount] = uLink.Network.Instantiate(player, playerProxyPrefab, playerOwnerPrefab, playerCreatorPrefab, spawnLocation, spawnRotation, 0);
		playerCount++;
	}

	void SpawnAI()
	{
		uLink.Network.Instantiate(uLink.NetworkPlayer.server, aiPrefab, aiSpawnLocation, spawnRotation, 0);
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

	public TeamTag GetPlayerTeamTag(GameObject player)
	{
		return GetGameObjectID(player) % 2 == 0 ? TeamTag.Red : TeamTag.Blue;
	}
	
	void Update()
	{
		if (minionSpawnCount % 6 == 0 && lastMinionSpawnTime + 2.0f < Time.fixedTime)
		{
			minionSpawnCount++;
			lastMinionSpawnTime = Time.fixedTime;
			SpawnAI(); // spawn super minion
		}
		else if ((minionSpawnCount + 1) % 6 == 0 && lastMinionSpawnTime + 10.0f < Time.fixedTime)
		{
			minionSpawnCount++;
			lastMinionSpawnTime = Time.fixedTime;
			SpawnAI(); // spawn normal minion (first in wave)
		}
		else if ((minionSpawnCount + 1) % 6 != 0 && lastMinionSpawnTime + 2.0f < Time.fixedTime)
		{
			minionSpawnCount++;
			lastMinionSpawnTime = Time.fixedTime;
			SpawnAI(); // spawn normal minion
		}
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
