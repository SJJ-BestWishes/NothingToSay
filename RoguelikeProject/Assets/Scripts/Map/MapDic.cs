using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapType
{
    //非地形
    #region
    //主角出生位置(单例)
    public const int player_glass = -1;
    public const int player_snow = -2;
    public const int player_earth = -3;
    //母亲出生位置(单例)
    public const int mother_glass = -6;
    public const int mother_snow = -7;
    public const int mother_earth = -8;
    //祖母的位置
    public const int grandMother_glass = -11;
    public const int grandMother_snow = -12;
    public const int grandMother_earth = -13;
    //father的位置
    public const int father_glass = -16;
    public const int father_snow = -17;
    public const int father_earth = -18;
    //Enemy
    public const int Enemy_glass = -21;
    #endregion

    //地形
    #region
    //草地
    /// <summary>
    /// 不碰撞
    /// </summary>
    public const int glass_Floor = 1;
    public const int glass_OutWall = 2;
    public const int glass_Wall = 3;
    public const int glass_Exit = 4;
    public const int glass_WaterOutWall = 5;
    //雪地
    /// <summary>
    /// 不碰撞
    /// </summary>
    public const int snow_Floor = 6;
    public const int snow_OutWall = 7;
    public const int snow_Wall = 8;
    public const int snow_Exit = 9;
    //特殊雪地墙面
    public const int snowWall_special = 10;
    //泥地
    /// <summary>
    /// 不碰撞
    /// </summary>
    public const int earth_Floor = 11;
    public const int earth_OutWall = 12;
    public const int earth_Wall = 13;
    public const int earth_exit = 14;
    //TODO
    #endregion


    //public const int nullfloor = 0;
    //public const int floor = 1;
    //public const int outwall = 2;
    //public const int player_born = 3;
    //public const int exit = 4;
    //public const int woman = 5;
    //public const int outpath = 6;
    //public const int outwall_grass = 7;
    //public const int deadpeople = 8;
    //public const int floor_grass = 9;
    //public const int outpath_grass = 10;
    //public const int iceflower = 11;
}
