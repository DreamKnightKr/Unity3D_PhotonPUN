using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstanceDataTransferMain : Photon.MonoBehaviour {
	public Text txtRoom;
	public Text txtData;
	public Text txtState;

	// Use this for initialization
	void OnEnable () {
		PhotonNetwork.ConnectUsingSettings ("v1.0.0");
	}

	void OnDisable() {
		PhotonNetwork.Disconnect();
	}
	
	public void OnSend() {
		PhotonNetwork.CreateRoom("TestRoom");
	}

	public void OnRecv() {
		PhotonNetwork.JoinRoom("TestRoom");
	}

	void OnPhotonCreateGameFailed() {
		txtState.text = "Create Failed";
	}

	void OnConnectedToMaster() {
		txtState.text = "Ready";
	}

	void OnJoinedRoom() {
		txtState.text = "Join Room";

		if(!PhotonNetwork.isMasterClient)
			photonView.RPC ("OnReqData", PhotonTargets.MasterClient);
	}

	void OnDisconnectedFromPhoton() {
		txtState.text = "Disconnected";
	}

	[PunRPC]
	void OnReqData() {
		photonView.RPC("OnRecvData", PhotonTargets.Others, "Data - 12345..");

		PhotonNetwork.LeaveRoom();
	}

	[PunRPC]
	void OnRecvData(string parameter) {
		txtData.text = parameter;

		PhotonNetwork.LeaveRoom();
	}
}
