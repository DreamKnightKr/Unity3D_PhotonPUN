using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataTransferIn10Sec : Photon.MonoBehaviour {
	public Text txtRoom;
	public Text txtData;
	public Text txtState;

	public Button btnSend;
	public Button btnRecv;

	public GameObject[] icons;

	System.DateTime centuryBegin = new System.DateTime(2001, 1, 1);
	long elapsedTicks = 0;

	
	// Use this for initialization
	void OnEnable () {
		PhotonNetwork.ConnectUsingSettings ("v1.0.0");

		btnSend.enabled = false;
		btnRecv.enabled = false;
	}
	
	void OnDisable() {
		PhotonNetwork.Disconnect();
	}

	void Update() {
		System.DateTime currentDate = System.DateTime.UtcNow;
		elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;

		UpdateTimeIcon (currentDate);
	}

	void UpdateTimeIcon(System.DateTime currentT) {
		for (int i = 0; i < icons.Length; i++) {
			icons[i].SetActive(( i == (currentT.Second / 10)));
		}
	}

	string GetRoomName() {
		
		txtRoom.text = (elapsedTicks / (10 * 1000)).ToString ();

		return txtRoom.text;
	}
	
	public void OnSend() {
		PhotonNetwork.CreateRoom(GetRoomName());

		btnSend.enabled = false;
		btnRecv.enabled = false;
	}
	
	public void OnRecv() {
		PhotonNetwork.JoinRoom(GetRoomName());

		btnSend.enabled = false;
		btnRecv.enabled = false;
	}
	
	void OnPhotonCreateGameFailed() {
		txtState.text = "Create Failed";
	}
	
	void OnConnectedToMaster() {
		txtState.text = "Ready";

		btnSend.enabled = true;
		btnRecv.enabled = true;
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
