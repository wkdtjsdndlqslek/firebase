using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance{ get; private set; }

    public List<UIPage> uiPages;
    public List<UIPopup> popups;
    private Stack<UIPopup> openPopups = new Stack<UIPopup>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (UIPage p in uiPages)
        {
            p.gameObject.SetActive(false);
        }
        foreach (UIPopup p in popups)
        {
            p.gameObject.SetActive(false); 
        }
        _ = PageOpen<UIMain>();
    }

    public T PopupOpen<T>() where T:UIPopup
    {
        T @return = popups.Find((popup) => popup is T) as T; //팝업 찾기
        if (@return != null) //찾는 팝업이 있으면
        {
            @return.gameObject.SetActive(true); //팝업 활성화
            openPopups.Push(@return); //팝업스택에 추가
        }
        return @return;
    }

    public void PopupClose()
    {
        if(openPopups.Count > 0)
        {//팝업 스택에 팝업이 있으면
            UIPopup targetPopup = openPopups.Pop(); //꺼냄
            targetPopup.gameObject.SetActive(false); //비활성화
        }
    }

    public T PageOpen<T>() where T : UIPage
    {
        T @return = null;
        foreach (UIPage uiPage in uiPages)
        {
            bool isActive = uiPage is T;
            uiPage.gameObject.SetActive(isActive);
            if (isActive) @return = uiPage as T;
        }
        return @return;
    }

    public UIPage PageOpen(string pageName)
    {
        UIPage @return = null;
        foreach(UIPage uiPage in uiPages)
        {
            bool isActive = uiPage.GetType().Name.Equals(pageName);
            uiPage.gameObject.SetActive(isActive);
            if (isActive) @return = uiPage;
        }
        return @return;
    }
}
