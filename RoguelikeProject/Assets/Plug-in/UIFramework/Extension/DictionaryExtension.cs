using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对Dictionary扩展
public static class DictionaryExtension
{
    /// <summary>
    /// 只能在对象上调用
    /// 尝试取得Key得到Value，得到返回Value，没有返回null
    /// this Dictionary<Tkey, Tvalue> dict 表示我们获取值的字典
    /// </summary>
    /// <typeparam name="Tkey">键类型</typeparam>
    /// <typeparam name="Tvalue">值类型</typeparam>
    /// <param name="dict">字典对象</param>
    /// <param name="key">键</param>
    /// <returns>返回对应键的值</returns>
    public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;
    }
}
