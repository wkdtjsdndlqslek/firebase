using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRanking : UIPage
{
    public UIRankPanel rankPrefab;
    public RectTransform panelLocation;
    public Button lobby;

    private void Awake()
    {
        lobby.onClick.AddListener(LobbyButtonClick);
    }

    private void Start()
    {
        
    }

    private void LobbyButtonClick()
    {
        UIManager.Instance.PageOpen<UIHome>();
    }

}
