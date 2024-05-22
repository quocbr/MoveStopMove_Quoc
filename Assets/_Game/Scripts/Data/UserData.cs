using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


[Serializable]
public class UserData
{
    public string userName; 
    public int coin;
    public int currentLevel;
    public Material currentColor;
    public PoolType currentWeapon;
    public PoolType currentPant;
    public PoolType currentHead;
    public PoolType currentShield;
    public PoolType currentWing;
    public PoolType currentTail;
    public SetType currentSet;
    public List<PoolType> listWeaponOwn;
    public List<PoolType> listPantOwn;
    public List<PoolType> listHeadOwn;
    public List<PoolType> listShieldOwn;
    public List<PoolType> listWingOwn;
    public List<PoolType> listTailOwn;
    public List<SetType> listSetOwn;

    public UserData()
    {
        this.userName = "You";
        this.coin = 0;
        this.currentLevel = 0;
        this.currentColor = null;
        this.currentWeapon = PoolType.Hammer;
        this.currentPant = PoolType.None;
        this.currentHead = PoolType.None;
        this.currentShield = PoolType.None;
        this.currentWing = PoolType.None;
        this.currentTail = PoolType.None;
        this.currentSet = SetType.None;
        this.listWeaponOwn = new List<PoolType>() {PoolType.Hammer };
        this.listPantOwn = new List<PoolType>();
        this.listHeadOwn = new List<PoolType>();
        this.listShieldOwn = new List<PoolType>();
        this.listWingOwn = new List<PoolType>();
        this.listTailOwn = new List<PoolType>();
        this.listSetOwn = new List<SetType>();
    }

    public string UserName { get => userName; set => userName = value; }
    public int Coin { get => coin; set => coin = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public PoolType CurrentWeapon { get => currentWeapon; set => currentWeapon = value; }
    public PoolType CurrentPant { get => currentPant; set => currentPant = value; }
    public PoolType CurrentHead { get => currentHead; set => currentHead = value; }
    public PoolType CurrentShield { get => currentShield; set => currentShield = value; }
    public PoolType CurrentWing { get => currentWing; set => currentWing = value; }
    public PoolType CurrentTail { get => currentTail; set => currentTail = value; }
    public SetType CurrentSet { get => currentSet; set => currentSet = value; }
    public List<PoolType> ListWeaponOwn { get => listWeaponOwn; set => listWeaponOwn = value; }
    public List<PoolType> ListPantOwn { get => listPantOwn; set => listPantOwn = value; }
    public List<PoolType> ListHeadOwn { get => listHeadOwn; set => listHeadOwn = value; }
    public List<PoolType> ListShieldOwn { get => listShieldOwn; set => listShieldOwn = value; }
    public List<PoolType> ListWingOwn { get => listWingOwn; set => listWingOwn = value; }
    public List<PoolType> ListTailOwn { get => listTailOwn; set => listTailOwn = value; }
    public List<SetType> ListSetOwn { get => listSetOwn; set => listSetOwn = value; }
}
