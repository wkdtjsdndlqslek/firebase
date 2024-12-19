using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable] //json���� ������ ���̱� ������ ����ȭ
public class UserData
{
    public enum UserClass
    {
        Worrior,
        Wizard,
        Rogue,
        Archer
    }

    public string UserId{  get; set; }
    public string userName;
    public int currentExp;
    public int maxExp;
    public int gem;
    public int level;
    public int gold;
    public UserClass userClass;

    public UserData()
    {//ȸ�������� �� ����� ������
        gem = 0;
        currentExp = 0;
        maxExp = 100;
        userName = "������ ����";
        level = 1;
        gold = 0;
        userClass = UserClass.Worrior;
    }

    public UserData(string userName,int gem, int level, int gold, int currentExp, int maxExp, UserClass userClass)
    {// �α����� �� �� ������
        this.gem = gem;
        this.userName = userName;
        this.currentExp = currentExp;
        this.maxExp = maxExp;
        this.level = level;
        this.gold = gold;
        this.userClass = userClass;
    }
}
