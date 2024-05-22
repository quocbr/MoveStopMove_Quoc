using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class Constant
{
    public const string SAVE_FILE_NAME = "savegame.data";
    public const string SAVE_KEY = "key.data";

    public const string TAG_CHARACTER = "Character";
    public const string TAG_BOT = "Bot";

    public const string EQUIP_STRING = "Equip";
    public const string EQUIPED_STRING = "Equiped";

    public const string TAG_BLOCK = "Block";

    public class MusicSound
    {
        public const string THEME = "";
        public const string WIN = "Victory";
        public const string LOSE = "Lose";
        public const string HOME = "homeTheme";
    }

    public class SFXSound
    {
        public const string BUTTON_CLICK = "Button Click";
        public const string DEBUTTON_CLICK = "Button Click2";
        public const string WEAPON_TRIGGER = "Vu khi va cham 3";
        public const string WEAPON_THROW = "Nem vui khi";
        public const string CHARACTER_DIE = "dead4";
        public const string COUNTDOWN = "count_down";
        public const string COUNTDOWN_END = "count_down_end";

        public const string SIZE_UP = "size_up5";
    }
}

public class Layer
{
    public const int WATER = 4;
    public const int PLAYER = 7;
    public const int BOT = 8;
    public const int BRICK = 10;
    public const int BRIDGE = 11;
    public const int DOOR = 12;
    public const int FINISHBOX = 13;
    
}

public class Anim
{
    public const string IDLE = "idle";
    public const string RUN = "run";
    public const string DEAD = "die";
    public const string ATTACK = "attack";
    public const string WIN = "win";
    public const string ULTI = "ulti";
    public const string DANCE = "dance";
}

public class PathContant
{
    public const string MAP_PATH = "Level/Map/Map";
    public const string STARTPOINT_PATH = "Level/StartPoint/StartPoint";
    public const string FINISHPOINT_PATH = "Level/FinishPoint/FinishPoint";
    public const string MAPDATA_PATCH = "Level/Map/MapData";
    public const string BRICK_PATCH = "Brick";
}

public enum WeaponType
{
    W_Hammer = PoolType.Hammer,
    W_Axe = PoolType.Axe,
    W_Axe_1 = PoolType.Axe_1,
    W_Candy_1 = PoolType.Candy_1,
    W_Candy_2 = PoolType.Candy_2,
    W_Candy_0 = PoolType.Candy_0,
    W_Candy_4 = PoolType.Candy_4,
    W_Boomerang = PoolType.Boomerang,
    W_Knife = PoolType.Knife,
    W_Uzi = PoolType.Uzi,
}

public enum BulletType
{
    B_Hammer = PoolType.B_Hammer,
    B_Axe = PoolType.B_Axe,
    B_Axe_1 = PoolType.B_Axe_1,
    B_Candy_1 = PoolType.B_Candy_1,
    B_Candy_2 = PoolType.B_Candy_2,
    B_Candy_0 = PoolType.B_Candy_0,
    B_Candy_4 = PoolType.B_Candy_4,
    B_Boomerang = PoolType.B_Boomerang,
    B_Knife = PoolType.B_Knife,
    //B_Uzi = PoolType.B_Uzi,
}

public enum HatType
{
    HAT_None = 0,
    HAT_Arrow = PoolType.Arow,
    HAT_Cap = PoolType.Hat,
    HAT_Cowboy = PoolType.Cowboy,
    HAT_Crown = PoolType.Crown,
    HAT_Ear = PoolType.Ear,
    HAT_StrawHat = PoolType.HatYellow,
    HAT_Headphone = PoolType.Head_Phone,
    HAT_Horn = PoolType.Horn,
    HAT_Police = PoolType.HatCap,
}

public enum PantType
{
    Pant_1,
    Pant_2,
    Pant_3,
    Pant_4,
    Pant_5,
    Pant_6,
    Pant_7,
    Pant_8,
    Pant_9,
}

