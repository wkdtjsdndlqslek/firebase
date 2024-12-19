using System;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance {  get; private set; }

    public FirebaseApp App { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase DB { get; private set; }

    private DatabaseReference usersRef;
    private DatabaseReference rankUsersRef;

    

    public UserData CurrentUserData { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        //파이어베이스 초기화 함수 상태 체크. 비동기(Async) 함수이므로 완료될 때까지 대기
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync(); 
        if(status == DependencyStatus.Available)
        {//초기화 성공
            App = FirebaseApp.DefaultInstance;
            Auth = FirebaseAuth.DefaultInstance;
            DB = FirebaseDatabase.DefaultInstance;
            DataSnapshot dummyData = await DB.GetReference("users").Child("dummy").GetValueAsync();
            if(dummyData.Exists)
            {
                print(dummyData.GetRawJsonValue());
            }
        }
        else
        {//초기화 실패
            Debug.LogWarning($"파이어베이스 초기화 실패 : {status}");
        }
    }

    //회원가입 함수
    public async void Create(string email, string passwd, Action<FirebaseUser,UserData> callback = null)
    {
        try
        {
            var result = await Auth.CreateUserWithEmailAndPasswordAsync(email, passwd);
            //생성된 회원의 database reference를 설정
            usersRef = DB.GetReference($"users/{result.User.UserId}");
            //회원의 데이터를 database에 생성
            UserData userData = new UserData();
            string userDataJson = JsonConvert.SerializeObject(userData);
            await usersRef.SetRawJsonValueAsync(userDataJson);
            callback?.Invoke(result.User, userData);
        }
        catch (FirebaseException e)
        {
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public async void SignIn(string email, string passwd, Action<FirebaseUser, UserData> callback=null)
    {
        try
        {
            var result = await Auth.SignInWithEmailAndPasswordAsync(email, passwd);
            usersRef = DB.GetReference($"users/{result.User.UserId}");
            DataSnapshot userDataValues = await usersRef.GetValueAsync();
            UserData userData = null;
            if (userDataValues != null) //DB에 경로가 존재하는 지 검사
            {
                string json = userDataValues.GetRawJsonValue();
                userData = JsonConvert.DeserializeObject<UserData>(json);
            }
            CurrentUserData = userData;
            callback?.Invoke(result.User, userData);
        }
        catch (FirebaseException e)
        {
            {
                UIManager.Instance.PopupOpen<UIDialogPopup>().SetPopup("로그인 실패", "아이디, 비밀번호를 확인해주세요");
                Debug.LogError(e.Message);
            }
        }
    }

    public async void UpdateUserProfile(string displayName, Action<FirebaseUser> callback = null)
    {//UserProfile 생성.
        UserProfile profile = new UserProfile()
        {
            DisplayName = displayName,
            PhotoUrl = new Uri("https://picsum.photos/120")
        };
        await Auth.CurrentUser.UpdateUserProfileAsync(profile);
        callback?.Invoke(Auth.CurrentUser);
    }

    //database의 유저 데이터 수정
    public async void UpdateUserData(string childName, object value, Action<object> callback = null)
    {
        DatabaseReference targetRef = usersRef.Child(childName);
        await targetRef.SetValueAsync(value);
        callback?.Invoke(value);
    }

    public void SingOut()
    {
        Auth.SignOut();
    }

    public async void GetAllUsers()
    {
        DataSnapshot rankSnapshot = await rankUsersRef.Child("users").GetValueAsync();
        foreach (var user in rankSnapshot.Children)
        {
            

        }
    }
}
