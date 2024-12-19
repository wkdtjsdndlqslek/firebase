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
        T @return = popups.Find((popup) => popup is T) as T; //�˾� ã��
        if (@return != null) //ã�� �˾��� ������
        {
            @return.gameObject.SetActive(true); //�˾� Ȱ��ȭ
            openPopups.Push(@return); //�˾����ÿ� �߰�
        }
        return @return;
    }

    public void PopupClose()
    {
        if(openPopups.Count > 0)
        {//�˾� ���ÿ� �˾��� ������
            UIPopup targetPopup = openPopups.Pop(); //����
            targetPopup.gameObject.SetActive(false); //��Ȱ��ȭ
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
