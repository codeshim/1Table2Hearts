using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public static PhotonInit inst = null;

    public Camera mainCam = null;
    public Camera subCam = null;

    [SerializeField]
    private byte maxPlayers = 2;

    void Awake()
    {
        inst = this;

        mainCam.gameObject.SetActive(false);
        subCam.gameObject.SetActive(true);

        PhotonNetwork.SendRate = 60;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 방 참가 실패 (참가할 방이 존재하지 않습니다.)");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 참가 실패");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 완료");

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        if (PhotonNetwork.CountOfPlayers == 0)
        {
            Debug.Log("error : 참가 인원 오류");
        }
        else if (PhotonNetwork.CountOfPlayers == 1)
        {
            pos = HeartManager.hearts[0].ownerTr.position;
            rot = HeartManager.hearts[0].ownerTr.rotation;
            HeartManager.hearts[0].owner = PhotonNetwork.Instantiate("Visitor", pos, rot);
        }
        else if (PhotonNetwork.CountOfPlayers == 2)
        {
            pos = Vector3.zero;
            rot = Quaternion.identity;
            GameObject newVisitor = PhotonNetwork.Instantiate("Visitor", pos, rot);
        }

        if (PhotonNetwork.CountOfPlayers != 0)
        {
            mainCam.gameObject.SetActive(true);
            subCam.gameObject.SetActive(false);
        }
    }

    void OnGUI()
    {
        string str = PhotonNetwork.NetworkClientState.ToString();
        GUI.Label(new Rect(10, 1, 1500, 60), "<color=#00ff00><size=20>" + str + "</size></color>");
    }
}
