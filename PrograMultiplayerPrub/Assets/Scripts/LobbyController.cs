using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private string sceneName;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //pra cuando comience todo comience junto
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        GameData.playerName=playerNameInputField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options= new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("Room1", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
}