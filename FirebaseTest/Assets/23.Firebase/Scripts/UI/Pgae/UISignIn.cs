using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISignIn : UIPage
{
    public TMP_InputField email;
    public TMP_InputField passwd;

    public Button signUpButton;
    public Button signInButton;

    private void Awake()
    {
        signUpButton.onClick.AddListener(SignUpButtonClick);
        signInButton.onClick.AddListener(SignInButtonClick);
    }

    private void SignUpButtonClick()
    {
        UIManager.Instance.PageOpen<UISignUp>();
    }

    private void SignInButtonClick()
    {
        FirebaseManager.Instance.SignIn(email.text, passwd.text, (fuser,UserData) =>
        {
            UIHome home = UIManager.Instance.PageOpen<UIHome>();
            home.SetInfo(fuser);
            home.SetUserData(UserData);
        });
    }
}
