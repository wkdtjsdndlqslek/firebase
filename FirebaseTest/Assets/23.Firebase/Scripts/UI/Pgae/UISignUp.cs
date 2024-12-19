using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISignUp : UIPage
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
        if (passwd.text.Length < 6)
        {
            UIManager.Instance.PopupOpen<UIDialogPopup>().SetPopup("�˸�", "��й�ȣ�� 6���� �̻�");
        }
        else
        {
            FirebaseManager.Instance.Create(email.text, passwd.text,CreateCallback);
        }
    }

    private void DialogCallback()
    {
        UIManager.Instance.PageOpen(GetType().Name);
    }

    private void CreateCallback(FirebaseUser user, UserData userData)
    {
        UIManager.Instance.PopupOpen<UIDialogPopup>().SetPopup("ȸ������ �Ϸ�", "ȸ�������� �Ϸ�Ǿ����ϴ�.\n�α��� ���ּ���");
    }

    private void SignInButtonClick()
    {
        UIManager.Instance.PageOpen<UISignIn>();
    }
}
