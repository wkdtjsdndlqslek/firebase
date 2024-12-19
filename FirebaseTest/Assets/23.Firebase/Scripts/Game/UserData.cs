using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable] //json으로 변경할 것이기 때문에 직렬화
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
    {//회원가입할 때 사용할 생성자
        gem = 0;
        currentExp = 0;
        maxExp = 100;
        userName = "무명의 전사";
        level = 1;
        gold = 0;
        userClass = UserClass.Worrior;
    }

    public UserData(string userName,int gem, int level, int gold, int currentExp, int maxExp, UserClass userClass)
    {// 로그인할 때 쓸 생성자
        this.gem = gem;
        this.userName = userName;
        this.currentExp = currentExp;
        this.maxExp = maxExp;
        this.level = level;
        this.gold = gold;
        this.userClass = userClass;
    }
}
