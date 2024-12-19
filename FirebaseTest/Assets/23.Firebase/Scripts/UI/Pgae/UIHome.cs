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

    private void Awake()
    {
        profileChangeButton.onClick.AddListener(ProfileChangeButtonClick);
        addGoldButton.onClick.AddListener(AddGoldButtonClick);
        addExpButton.onClick.AddListener(AddExpButtonClick);
        addGemButton.onClick.AddListener(AddGemButtonClick);
        signOutButton.onClick.AddListener(SignOutButtonClick);
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
