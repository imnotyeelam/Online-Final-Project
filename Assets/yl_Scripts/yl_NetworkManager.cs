using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class yl_NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    public TMP_Text connectionStatusText;

    [Header("Main UI Panel")]
    public GameObject Main_UI_Panel;

    [Header("Login UI Panel")]
    public TMP_InputField playerNameInput;
    public GameObject Login_UI_Panel;

    [Header("Game Options UI Panel")]
    public GameObject GameOptions_UI_Panel;

    [Header("Create Room UI Panel")]
    public GameObject CreateRoom_UI_Panel;
    public TMP_InputField roomNameInputField;


    [Header("Inside Room UI Panel")]
    public GameObject InsideRoom_UI_Panel;
    public TMP_Text roomInfoText;
    public GameObject playerListPrefab;
    public GameObject playerListContent;
    public GameObject startGameButton;

    [Header("Room List UI Panel")]
    public GameObject RoomList_UI_Panel;
    public GameObject roomListEntryPrefab;
    public GameObject roomListParentGameobject;

    [Header("Player Card Colors")]
    public Color myPlayerColor = new Color(0.1f, 0.6f, 0.8f, 0.8f);
    public Color otherPlayerColor = new Color(0.08f, 0.08f, 0.08f, 0.6f);

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameobjects;
    private Dictionary<int, GameObject> playerListGameobjects;

    private int selectedCharacter = 1;

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(Main_UI_Panel.name);

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameobjects = new Dictionary<string, GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;
    }
    #endregion

    #region UI Callbacks

    public void OnStartButtonClicked()
    {
        ActivatePanel(Login_UI_Panel.name);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;

            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["Character"] = selectedCharacter;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Player name is invalid");
        }
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room" + Random.Range(1000, 10000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;//**********************

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        ActivatePanel(RoomList_UI_Panel.name);
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(GameOptions_UI_Panel.name);
    }
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    /*public void OnJoinRandomRoomButtonClicked()
    {
        ActivatePanel(JoinRandomRoom_UI_Panel.name);
        PhotonNetwork.JoinRandomRoom();
    }*/

    public void OnStartGameButtonClicked()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)//**********
        {
            Debug.Log("Need 4 players to start.");
            return;
        }

        PhotonNetwork.LoadLevel("GameScene");
    }

    public void SelectCharacter1()
    {
        selectedCharacter = 1;
        PlayerPrefs.SetInt("Character", 1);
        PlayerPrefs.Save();
        Debug.Log("Selected Character 1");
    }

    public void SelectCharacter2()
    {
        selectedCharacter = 2;
        PlayerPrefs.SetInt("Character", 2);
        PlayerPrefs.Save();
        Debug.Log("Selected Character 2");
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(InsideRoom_UI_Panel.name);

        UpdateStartButton();

        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + "\n" +
                            "Players/Max.players: " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                            PhotonNetwork.CurrentRoom.MaxPlayers;

        if (playerListGameobjects == null)
        {
            playerListGameobjects = new Dictionary<int, GameObject>();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListGameobject = Instantiate(playerListPrefab);
            playerListGameobject.transform.SetParent(playerListContent.transform, false);
            playerListGameobject.transform.SetAsLastSibling();

            Debug.Log("Player card created: " + player.NickName);
            Debug.Log("Parent: " + playerListContent.name);
            Debug.Log("Prefab active: " + playerListGameobject.activeSelf);

            playerListGameobject.transform.Find("PlayerNameText")
                .GetComponent<TMP_Text>().text = player.NickName;

            bool isMe = player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;

            Image cardBg = playerListGameobject.GetComponent<Image>();
            if (cardBg != null)
            {
                cardBg.color = isMe ? myPlayerColor : otherPlayerColor;
            }

            playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(isMe);

            playerListGameobjects.Add(player.ActorNumber, playerListGameobject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                            "Players/Max.players: " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                            PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerListGameobject = Instantiate(playerListPrefab);
        playerListGameobject.transform.SetParent(playerListContent.transform, false);
        playerListGameobject.transform.SetAsLastSibling();

        Debug.Log("New player card created: " + newPlayer.NickName);
        Debug.Log("Parent: " + playerListContent.name);
        Debug.Log("Prefab active: " + playerListGameobject.activeSelf);

        playerListGameobject.transform.Find("PlayerNameText")
            .GetComponent<TMP_Text>().text = newPlayer.NickName;

        bool isMe = newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;

        Image cardBg = playerListGameobject.GetComponent<Image>();
        if (cardBg != null)
        {
            cardBg.color = isMe ? myPlayerColor : otherPlayerColor;
        }

        playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(isMe);

        playerListGameobjects.Add(newPlayer.ActorNumber, playerListGameobject);

        UpdateStartButton();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //update room info text
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                            "Players/Max.players: " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                            PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameobjects[otherPlayer.ActorNumber].gameObject);
        playerListGameobjects.Remove(otherPlayer.ActorNumber);

        UpdateStartButton();
    }
    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptions_UI_Panel.name);

        foreach (GameObject playerListGameobject in playerListGameobjects.Values)
        {
            Destroy(playerListGameobject);
        }
        playerListGameobjects.Clear();
        playerListGameobjects = null;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate called");

        if (roomListEntryPrefab == null)
        {
            Debug.LogError("roomListEntryPrefab is NOT assigned");
            return;
        }

        if (roomListParentGameobject == null)
        {
            Debug.LogError("roomListParentGameobject is NOT assigned");
            return;
        }

        if (roomListGameobjects == null)
            roomListGameobjects = new Dictionary<string, GameObject>();

        if (cachedRoomList == null)
            cachedRoomList = new Dictionary<string, RoomInfo>();

        ClearRoomListView();

        foreach (RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                    cachedRoomList.Remove(room.Name);
            }
            else
            {
                cachedRoomList[room.Name] = room;
            }
        }

        foreach (RoomInfo room in cachedRoomList.Values)
        {
            GameObject roomListEntryGameobject = Instantiate(roomListEntryPrefab, roomListParentGameobject.transform, false);

            Transform roomNameText = roomListEntryGameobject.transform.Find("RoomNameText");
            Transform roomPlayersText = roomListEntryGameobject.transform.Find("RoomPlayersText");
            Transform joinRoomButton = roomListEntryGameobject.transform.Find("JoinRoomButton");

            if (roomNameText == null)
            {
                Debug.LogError("RoomNameText not found inside RoomListEntryPrefab");
                return;
            }

            if (roomPlayersText == null)
            {
                Debug.LogError("RoomPlayersText not found inside RoomListEntryPrefab");
                return;
            }

            if (joinRoomButton == null)
            {
                Debug.LogError("JoinRoomButton not found inside RoomListEntryPrefab");
                return;
            }

            roomNameText.GetComponent<TMP_Text>().text = room.Name;
            roomPlayersText.GetComponent<TMP_Text>().text = room.PlayerCount + "/" + room.MaxPlayers;
            joinRoomButton.GetComponent<Button>().onClick.AddListener(() => OnJoinRoomButtonClicked(room.Name));

            roomListGameobjects.Add(room.Name, roomListEntryGameobject);
        }
    }

    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cachedRoomList.Clear();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        string roomName = "Room" + Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected: " + cause);
    }

    #endregion

    #region Private Methods

    void OnJoinRoomButtonClicked(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(_roomName);
    }

    void ClearRoomListView()
    {
        if (roomListGameobjects == null) return;

        foreach (var roomListGameobject in roomListGameobjects.Values)
        {
            if (roomListGameobject != null)
            {
                Destroy(roomListGameobject);
            }
        }

        roomListGameobjects.Clear();
    }

    void UpdateStartButton()
    {
        bool canStart =
            PhotonNetwork.IsMasterClient &&
            PhotonNetwork.CurrentRoom.PlayerCount == 2;//********

        startGameButton.SetActive(canStart);
    }

    #endregion

    #region Public Methods
    public void ActivatePanel(string panelToBeActivated)
    {
        Main_UI_Panel.SetActive(panelToBeActivated.Equals(Main_UI_Panel.name));
        Login_UI_Panel.SetActive(panelToBeActivated.Equals(Login_UI_Panel.name));
        GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
        CreateRoom_UI_Panel.SetActive(panelToBeActivated.Equals(CreateRoom_UI_Panel.name));
        InsideRoom_UI_Panel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Panel.name));
        RoomList_UI_Panel.SetActive(panelToBeActivated.Equals(RoomList_UI_Panel.name));
    }
    #endregion
}
