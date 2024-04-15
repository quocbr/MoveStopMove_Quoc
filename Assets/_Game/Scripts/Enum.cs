using UnityEngine;

public enum ColorType 
{
    None = -1,
    Yellow = 0,
    Red = 1,
    Orange = 2,
    Green = 3,
    Blue = 4,
    Black = 5,
}

public enum EquipmentType
{
    Weapon,
    Head,
    Pant,
    Wing,
    Tail,
    Shield,
    Set,
}

public enum ParticleType
{
    BloodExplosionRound = 0,
    SingleThunder = 10,
}

public enum SetType
{
    None = 0,
    Set_1 = 1,
    Set_2 = 2,
    Set_3 = 3,
    Set_4 = 4,
    Set_5 = 5,
}

public enum PoolType
{
    None = 0,

    [InspectorName("=======Weapon=====")] we,
    Axe=2,
    Knife,
    Boomerang,
    Hammer,
    Candy_2,
    Uzi,
    Candy_0,

    [InspectorName("=======Head=====")] eq,
    Arow =30,
    Cowboy,
    Crown,
    Ear,
    Hat,
    Hat_Thor,
    HatCap,
    HatWitch,
    HatYellow,
    Head_Angel,
    Head_Phone,
    Horn,

    [InspectorName("=======Effec=====")] ef,
    DashShadow= 100,
    HpText,

    [InspectorName("=======Character=====")] ch,
    Player = 150,
    Bot,

    [InspectorName("=======Bullet=====")] bu,
    B_Axe = 170,
    B_Knife,
    B_Boomerang,
    B_Hammer,
    B_Candy_2,
    B_Candy_0,

    [InspectorName("=======Tail=====")] tail,
    Tail_David = 200,

    [InspectorName("=======Wing=====")] Wing,
    Wing_Angel = 300,
    Wing_Devil,

    [InspectorName("=======Shield=====")] Shield,
    Blade_Death = 350,
    Book,
    Bow_Angel,
    Shield_1,
    Shield_2,

    [InspectorName("======Pant========")] Panttt,
    Batman = 400,
    Cham_bi,
    Comy,
    Da_Bao,
    Onion,
    Pokemon,
    Rainbow,
    Skull,
    Van_Tim,
    Davil,
    Angel,

    [InspectorName("======UI========")] uii,
    ItemDataUI = 500,
}


