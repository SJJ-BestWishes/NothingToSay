using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Location
{
    public int lineIndex { get; set; }
    public int columnsIndex { get; set; }
    public Location(int lineIndex, int columnsIndex)
    {
        this.lineIndex = lineIndex;
        this.columnsIndex = columnsIndex;
    }
    public static bool CreateObject(int[,] map)
    {
        //遍历记录0的位置
        List<Location> locations = new List<Location>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 0)
                    locations.Add(new Location(i, j));
            }
        }
        if (locations.Count > 0)
        {
            //随机位置
            Location randomlocation = locations[Random.Range(0, locations.Count)];
            //赋值给Map
            //
            return true;
        }
        else
            return false;
    }
}