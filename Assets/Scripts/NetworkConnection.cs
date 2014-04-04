using UnityEngine;
using System.Collections;

public class NetworkConnection : MonoBehaviour {

	public string connectionIP = "127.0.0.1";
	public int connectionPort = 25001;

	public GUIStyle menuButtonStyle;
	public GUIStyle menuLabelStyle;
	public GUIStyle menuBoxStyle;
	
	public Texture2D menuBackground;

	void OnGUI () {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			GUI.Box (new Rect(Screen.width/2 - 250, Screen.height/2 - 288, 500, 577), menuBackground, menuBoxStyle);
			GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2 - 90, 200, 20), "Status: Disconnected", menuLabelStyle);
			if (GUI.Button(new Rect(Screen.width/2 - 120, Screen.height/2 - 60, 240, 60), "Client Connect", menuButtonStyle)) {
				Network.Connect(connectionIP, connectionPort);
			}

			if (GUI.Button(new Rect(Screen.width/2 - 120, Screen.height/2 + 10, 240, 60), "Initialize Server", menuButtonStyle)) {
				Network.InitializeServer(32, connectionPort, false);
			}
		} else if (Network.peerType == NetworkPeerType.Client) {
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Client", menuLabelStyle);
			if (GUI.Button(new Rect(10, 30, 120, 40), "Disconnect", menuButtonStyle)) {
				Network.Disconnect(200);
			}
		} else if (Network.peerType == NetworkPeerType.Server) {
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Server", menuLabelStyle);
			if (GUI.Button(new Rect(10, 30, 120, 40), "Disconnect", menuButtonStyle)) {
				Network.Disconnect(200);
			}
		}
	}
}
