namespace Eyttymkkn.Objects
{
    public enum WeaponType
    {
        Sword, Lance, Axr, RedBow, BlueBow, GreenBow, ColorlessBow, RedDagger, BlueDagger,
        GreedDagger, Dagger, RedTome, BlueTome, GreenTome, Staff, RedBreath, BlueBreath, GreenBreath,
        ColorlessBreath, RedBeast, BlueBeast, GreenBeast, ColorlessBeast
    };

    public enum Element
    { None, Fire, Thunder, Wind, Light, Dark };

    public enum Move { Infantry, Armored, Cavalry, Flying };

    public enum SkillCategory
    {
        武器,
        支援,
        奥义,
        A,
        B,
        C,
        S,
        锻造,
        化身
    }

    public enum SORT_CRITERIA
    {
        HP,
        ATK,
        SPD,
        DEF,
        RES,
        HP_DEF,
        HP_RES,
        DEF_RES,
        TOTAL
    }
}
