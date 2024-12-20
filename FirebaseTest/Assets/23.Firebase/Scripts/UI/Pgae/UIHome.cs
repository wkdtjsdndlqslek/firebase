using TMPro;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System.Net.Http;
using System;

public class UIHome : UIPage
{
    public TextMeshProUGUI displayName;
    public Image profileImage;
    public Button profileChangeButton;

    public TextMeshProUGUI gold;
    public Button addGoldButton;

    public Button addExpButton;
    public Slider expFillAmount;
    public Text expAmountText;
    public TextMeshProUGUI level;

    public Button addGemButton;
    public TextMeshProUGUI gem;

    public Button signOutButton;
    public Button messageButton;
    public Button inviteButton;
    public Board gameBoard;

    private void Awake()
    {
        profileChangeButton.onClick.AddListener(ProfileChangeButtonClick);
        addGoldButton.onClick.AddListener(AddGoldButtonClick);
        addExpButton.onClick.AddListener(AddExpButtonClick);
        addGemButton.onClick.AddListener(AddGemButtonClick);
        signOutButton.onClick.AddListener(SignOutButtonClick);
        messageButton.onClick.AddListener(MessageButtonClick);
        inviteButton.onClick.AddListener(InviteButtonClick);

        gameBoard.gameObject.SetActive(false);

    }
    private void Start()
    {
        FirebaseManager.Instance.onGameStart += GameStart;
        FirebaseManager.Instance.onTurnProcceed += ProccessTurn;
    }
    private Room currentRoom;

    public void GameStart(Room room, bool isHost)
    {
        print("게임시작");
        currentRoom = room;
        gameBoard.isHost = isHost;
        gameBoard.gameObject.SetActive(true);
    }

    public void ProccessTurn(Turn turn)
    {//새로운 턴 입력이 추가 될 때마다 호출.
        gameBoard.turnCount++;
        gameBoard.Placemark(turn.isHostTurn, turn.coodinate);
        if(turn.isHostTurn == gameBoard.isHost)
        {//내 턴

        }
        else
        {//상대턴

        }
        
    }

    private void InviteButtonClick()
    {
        var popup = UIManager.Instance.PopupOpen<UIInputFieldPopup>();
        popup.SetPopup("초대하기", "누구를 초대하시겠습니까?", InviteTarget);
    }

    private async void InviteTarget(string target)
    {
        Room room = new Room()
        {
            host = FirebaseManager.Instance.Auth.CurrentUser.UserId,
            guest = target,
            state = RoomState.Waiting
        };

        await FirebaseManager.Instance.CreateRoom(room);

        Message message = new Message()
        {
            type = MessageType.Invite,
            sender = FirebaseManager.Instance.Auth.CurrentUser.UserId,
            message = "",
            sendTime = DateTime.Now.Ticks
        };
        await FirebaseManager.Instance.MessageToTarget(target, message);
    }

    string messageTarget;

    private void MessageButtonClick()
    {

        var popup= UIManager.Instance.PopupOpen<UIInputFieldPopup>();
        popup.SetPopup("메시지 보내기", "누구에게 메시지를 보내시겠습니까?", SetMessageTarget);
    }

    private void SetMessageTarget(string target)
    {
        messageTarget = target;
        var popup = UIManager.Instance.PopupOpen<UIInputFieldPopup>();
        popup.SetPopup($"To.{messageTarget}", "뭐라고 메시지를 보내시겠습니까?", MessageToTarget);
    }

    private async void MessageToTarget(string messageText)
    {
        Message message = new Message()
        {
            type = MessageType.Message,
            sender = FirebaseManager.Instance.Auth.CurrentUser.UserId,
            message = messageText,
            sendTime = DateTime.Now.Ticks
        };

        await FirebaseManager.Instance.MessageToTarget(messageTarget, message);
    }

    private void AddExpButtonClick()
    {
        UserData data = FirebaseManager.Instance.CurrentUserData;
        if((data.currentExp + 10)>=data.maxExp)
        {
            data.currentExp = data.currentExp + 10 - data.maxExp;
            data.level ++;
            data.maxExp += 10;
        }
        else
        {
            data.currentExp += 10;
        }
        FirebaseManager.Instance.UpdateUserData("currentExp", data.currentExp,(x)=>SetUserData(data));
        FirebaseManager.Instance.UpdateUserData("maxExp", data.maxExp,(x)=>SetUserData(data));
        FirebaseManager.Instance.UpdateUserData("level",data.level,(x)=>SetUserData(data));
    }

    private void AddGemButtonClick()
    {
        UserData data = FirebaseManager.Instance.CurrentUserData;
        data.gem += 10;
        FirebaseManager.Instance.UpdateUserData("gem", data.gem, (x) => SetUserData(data));
    }

    private void SignOutButtonClick()
    {
        FirebaseManager.Instance.SingOut();
        UIManager.Instance.PageOpen<UIMain>();
    }
    private void ProfileChangeButtonClick()
    {
        UIManager.Instance.PopupOpen<UIInputFieldPopup>().SetPopup("닉네임 변경", "변경할 닉네임입력.", ProfileChangeCallback);
    }

    private void ProfileChangeCallback(string newName)
    {
        FirebaseManager.Instance.UpdateUserProfile(newName, SetInfo);
    }

    public void SetInfo(FirebaseUser user)
    {
        displayName.text = user.DisplayName;
        if (user.PhotoUrl != null)
        {
            SetPhoto(user.PhotoUrl.AbsoluteUri);
        }
        else
        {
            SetPhoto("");
        }
    }

    public void SetUserData(UserData userData)
    {
        gold.text = userData.gold.ToString();
        gem.text = userData.gem.ToString();
        level.text = userData.level.ToString();
        expFillAmount.value = ((float)userData.currentExp / (float)userData.maxExp);
        expAmountText.text = $"{userData.currentExp} / {userData.maxExp}";
    }

    public async void SetPhoto(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] response = await client.GetByteArrayAsync(url);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(response);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                profileImage.sprite = sprite;
            }
        }
        else
        {
            profileImage.sprite = null;
        }
    }

    public void AddGoldButtonClick()
    {
        UserData data = FirebaseManager.Instance.CurrentUserData;
        data.gold += 10;
        FirebaseManager.Instance.UpdateUserData("gold", data.gold, (x) => { SetUserData(data); });
    }
}
