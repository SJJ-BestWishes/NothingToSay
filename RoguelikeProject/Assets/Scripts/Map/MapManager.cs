using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class MapManager : MonoBehaviour
{
    //地形
    public GameObject[] glass_Floor;
    public GameObject[] glass_OutWall;
    public GameObject[] glass_Wall;

    public GameObject[] snow_Floor;
    public GameObject[] snow_OutWall;
    public GameObject[] snow_Wall;

    public GameObject[] walls;
    public GameObject[] exits;
    //非地形
    public GameObject player;
    public GameObject mother;
    public GameObject grandMother;
    public GameObject father;
    public GameObject[] enemys;
    public GameObject[] foods;

    //创建地图
    private int[,] map;
    private int line;
    private int columns;

    //存储地图资源的路径
    private string path = "LevelEdit/level";
    //地图父物体
    private Transform mapManager;
    //摄像机
    private CameraFollow cameraFollow;
    //记录空闲位置
    private List<Location> locations;


    public void InitMap()
    {
        int level = GameManager.Instance.level;
        mapManager = new GameObject("Map").transform;
        locations = new List<Location>();
        //生成地形
        if ((map = LoadMap(level, path)) != null)//给摄像机跟随设置边界在LoadMap中
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    //j为x轴坐标,i为y轴坐标,(即行对应y轴，列对应x轴)
                    switch (map[i, j])
                    {
                        case MapType.player_glass:
                            CreatePlayer(j, i, MapType.glass_Floor);
                            break;
                        case MapType.player_snow:
                            CreatePlayer(j, i, MapType.snow_Floor);
                            break;
                        case MapType.player_earth:
                            CreatePlayer(j, i, MapType.earth_Floor);
                            break;

                        case MapType.mother_glass:
                            CreateMotner(j, i, MapType.glass_Floor);
                            break;
                        case MapType.mother_snow:
                            CreateMotner(j, i, MapType.snow_Floor);
                            break;
                        case MapType.mother_earth:
                            CreateMotner(j, i, MapType.earth_Floor);
                            break;

                        case MapType.grandMother_glass:
                            CreateGrandMother(j, i, MapType.glass_Floor);
                            break;
                        case MapType.grandMother_snow:
                            CreateGrandMother(j, i, MapType.snow_Floor);
                            break;
                        case MapType.grandMother_earth:
                            CreateGrandMother(j, i, MapType.earth_Floor);
                            break;

                        case MapType.father_glass:
                            CreateFather(j, i, MapType.glass_Floor);
                            break;
                        case MapType.father_snow:
                            CreateFather(j, i, MapType.snow_Floor);
                            break;
                        case MapType.father_earth:
                            CreateFather(j, i, MapType.earth_Floor);
                            break;

                        case MapType.glass_Floor:
                            CreateFloor(j, i, glass_Floor);
                            break;
                        case MapType.glass_OutWall:
                            CreateOutWall(j, i, glass_OutWall);
                            break;
                        case MapType.glass_Wall:
                            CreateWall(j, i, glass_Wall, MapType.glass_Floor);
                            break;
                        case MapType.glass_Exit:
                            CreateExit(j, i, exits , MapType.glass_Floor);
                            break;

                        case MapType.snow_Floor:
                            CreateFloor(j, i, snow_Floor);
                            break;
                        case MapType.snow_OutWall:
                            CreateOutWall(j, i, snow_OutWall);
                            break;
                        case MapType.snow_Wall:
                            CreateWall(j, i, snow_Wall, MapType.snow_Floor);
                            break;
                        case MapType.snow_Exit:
                            CreateExit(j, i, exits, MapType.snow_Floor);
                            break;
                    }
                }
            }
        }
        else
        {
            Debug.Log("没有配置这一关地图");
        }
        //随机生成

        if (level != 7)
        {
            CreateEnemy(enemys, level);

            CreateFood(foods, level);

            switch (level)
            {
                case 1:
                case 2:
                case 3:
                    CreateWall(glass_Wall, level);
                    break;
                case 4:
                case 5:
                case 6:
                    CreateWall(snow_Wall, level);
                    break;
            }
        }
    }
    private int[,] LoadMap(int level, string path)
    {
        if (Resources.Load<TextAsset>(path + level))
        {
            string[] lines = Resources.Load<TextAsset>(path + level).text.Split('\n');
            //行
            line = lines.Length;
            //列
            columns = lines[0].Split(',').Length;
            //给摄像机跟随设置边界
            cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
            cameraFollow.SetBorder(line, columns);

            int[,] map = new int[line, columns];
            for (int i = 0; i < line; i++)
            {
                string[] thisLine = lines[line - i - 1].Split(',');//Map存的和读取的行数相反
                for (int j = 0; j < columns; j++)
                {
                    map[i, j] = int.Parse(thisLine[j]);
                }
            }
            //检测是否读到,TODO
            //for (int i = 0; i < line; i++)
            //{
            //    string vs = "";
            //    for (int j = 0; j < columns; j++)
            //    {
            //        vs += map[i, j] + ",";
            //    }
            //    Debug.Log(vs);
            //}
            return map;
        }
        else return null;
    }



    //Create方法
    #region

    private void CreateFloor(int x, int y, GameObject[] prefabs)
    {
        //空闲
        locations.Add(new Location(y, x));//对应Map[y,x]
        int random = Random.Range(0, prefabs.Length);
        GameObject obj = Instantiate(prefabs[random], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.SetParent(mapManager, false);
    }

    private void CreateOutWall(int x, int y, GameObject[] prefabs)
    {
        int random = Random.Range(0, prefabs.Length);
        GameObject obj = Instantiate(prefabs[random], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.SetParent(mapManager, false);
    }

    private void CreateWall(int x, int y, GameObject[] prefabs ,int floor_type)
    {
        CreateBgFloor(x, y, floor_type);
        int random = Random.Range(0, prefabs.Length);
        GameObject obj = Instantiate(prefabs[random], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.SetParent(mapManager, false);
    }

    private void CreateExit(int x, int y, GameObject[] prefabs ,int floor_type)
    {
        CreateBgFloor(x, y, floor_type);
        int random = Random.Range(0, prefabs.Length);
        GameObject obj = Instantiate(prefabs[random], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.SetParent(mapManager, false);
    }

    private void CreatePlayer(int x, int y, int floor_type)
    {
        CreateBgFloor(x, y, floor_type);
        if (!Player.Instance)
        {
            Instantiate(player, new Vector3(x, y, 0), Quaternion.identity);
        }
        else
        {
            GameObject player = Player.Instance.gameObject;
            if (!player.activeInHierarchy)
            {
                player.SetActive(true);
            }
            //目标位置和当前位置都要设置
            Player.Instance.targetPos = new Vector3(x, y, 0);
            player.transform.position = new Vector3(x, y, 0);
        }
        //摄像机跟随
        cameraFollow.Follow();
    }

    private void CreateMotner(int x, int y, int floor_type)
    {
        CreateBgFloor(x, y, floor_type);
        if (!Mother.Instance)
        {
            Instantiate(mother, new Vector3(x, y, 0), Quaternion.identity);
        }
        //存在且添加了
        else if (Mother.Instance.isADD)
        {
            GameObject mother = Mother.Instance.gameObject;
            if (!mother.activeSelf)
            {
                mother.SetActive(true);
            }
            Mother.Instance.targetPos = new Vector3(x, y, 0);
            mother.transform.position = new Vector3(x, y, 0);
        }
        //存在但是没有添加
        else
        {
            GameObject mother = Mother.Instance.gameObject;
            if (mother.activeSelf)
            {
                mother.SetActive(false);
            }
        }
    }

    private void CreateGrandMother(int x, int y, int floor_type)
    {       
        CreateBgFloor(x, y, floor_type);
        GameObject obj =Instantiate(grandMother, new Vector3(x, y, 0), Quaternion.identity);
    }
    private void CreateFather(int x, int y, int floor_type)
    {
        CreateBgFloor(x, y, floor_type);
        GameObject obj = Instantiate(father, new Vector3(x, y, 0), Quaternion.identity);
    }

    //随机产生的部分
    private void CreateEnemy(GameObject[] prefabs, int level)
    {
        int number = level;
        for (int i = 1; i <= number; i++)
        {
            int index = Random.Range(0, locations.Count);
            Location location = locations[index];
            locations.RemoveAt(index);

            int random = Random.Range(0, prefabs.Length);
            Instantiate(prefabs[random], new Vector3(location.columnsIndex, location.lineIndex, 0), Quaternion.identity);
        }
    }

    private void CreateFood(GameObject[] prefabs, int level)
    {
        int number = level;
        for (int i = 1; i <= number; i++)
        {
            int index = Random.Range(0, locations.Count);
            Location location = locations[index];
            locations.RemoveAt(index);

            int random = Random.Range(0, prefabs.Length);
            GameObject obj = Instantiate(prefabs[random], new Vector3(location.columnsIndex, location.lineIndex, 0), Quaternion.identity);
            obj.transform.SetParent(mapManager, false);
        }
    }

    private void CreateWall(GameObject[] prefabs, int level)
    {
        int number = level;
        for (int i = 1; i <= number; i++)
        {
            int index = Random.Range(0, locations.Count);
            Location location = locations[index];
            locations.RemoveAt(index);

            int random = Random.Range(0, prefabs.Length);
            GameObject obj = Instantiate(prefabs[random], new Vector3(location.columnsIndex, location.lineIndex, 0), Quaternion.identity);
            obj.transform.SetParent(mapManager, false);
        }
    }
    /*
    /// <summary>
    /// 由于Wall没有设置背景,根据选择的Floor生成背景
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="prefabs"></param>
    /// <param name="floor_type">MapType类型</param>
    private void CreateWallWithoutBg(int x, int y, GameObject[] prefabs, int floor_type)
    {
        CreateBgFloor(x, y, floor_type);

        int random = Random.Range(0, prefabs.Length);
        GameObject obj = Instantiate(prefabs[random], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.SetParent(mapManager, false);
    }

    /// <summary>
    /// 由于Exit没有设置背景,根据选择的Floor生成背景
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="prefabs"></param>
    /// <param name="floor_type">MapType类型</param>
    private void CreateExitWithoutBg(int x, int y, GameObject[] prefabs, int floor_type)
    {
        CreateBgFloor(x, y, floor_type);

        int random = Random.Range(0, prefabs.Length);
        GameObject obj = Instantiate(prefabs[random], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.SetParent(mapManager, false);
    }
    

    */
    private void CreateBgFloor(int x, int y, int floor_type)
    {
        switch (floor_type)
        {
            case MapType.glass_Floor:
                CreateFloor(x, y, glass_Floor);
                break;
            case MapType.snow_Floor:
                CreateFloor(x, y, snow_Floor);
                break;
        }
        //移除最后一个元素，即刚才草地加的
        locations.RemoveAt(locations.Count - 1);
    }
    #endregion
}