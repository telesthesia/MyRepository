using UnityEngine;
using System.Collections;
using System; 
using System.Collections.Generic;

/// <summary>
/// 按时间查询数组助手类
/// </summary>

    public static class ArrayHelper
    {
        /// <summary>
        /// 对象数组的升序排列
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型 如：Enemy</typeparam>
        /// <typeparam name="TKey">对象的属性 如：HP</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">排序的比较条件</param>
        public static void OrderBy<T, TKey>(T[] array, Func<T, TKey> condition) where TKey : IComparable
        {
            //取出数据
            for (int r = 0; r < array.Length - 1; r++)
            {
                //作比较
                for (int c = r + 1; c < array.Length; c++)
                {
                    //array[r].HP        >    array[c].HP
                    // condition(array[r])   >     condition(array[c]) 
                    if (condition(array[r]).CompareTo(condition(array[c])) > 0)
                    {
                        T temp = array[r];
                        array[r] = array[c];
                        array[c] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 对象数组的降序排列
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型 如：Enemy</typeparam>
        /// <typeparam name="TKey">对象的属性 如：HP</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">排序的比较条件</param>
        public static void OrderByDescending<T, TKey>(T[] array, Func<T, TKey> condition) where TKey : IComparable
        {
            //取出数据
            for (int r = 0; r < array.Length - 1; r++)
            {
                //作比较
                for (int c = r + 1; c < array.Length; c++)
                {
                    if (condition(array[r]).CompareTo(condition(array[c])) < 0)
                    {
                        T temp = array[r];
                        array[r] = array[c];
                        array[c] = temp;
                    }
                }
            }
        }

        public static T Find<T>(T[] array, Func<T, bool> condition)
        {
            foreach (var item in array)
            {
                //if (item.HP > 0)
                if (condition(item))
                    return item;
            }
            return default(T); //string b = default(string);  表示将string类型的默认值赋值给变量b
        }

        //练习4：查找满足条件的所有对象
        //例如：生命值大于0   
        public static T[] FindAll<T>(T[] array, Func<T, bool> condition)
        {
            //调用FindAll方法，立即查询 
            List<T> result = new List<T>(array.Length);
            foreach (var item in array)
            {
                if (condition(item))
                    result.Add(item);
            }
            return result.ToArray(); //string b = default(string);  表示将string类型的默认值赋值给变量b

            //调用FindAll方法，创建迭代器对象，等待foreach查询
            //for (int i = 0; i < array.Length; i++)
            //{
            //    if (condition(array[i]))
            //        yield return array[i];//暂停退出
            //} 
            //返回值类型：IEnumerable<T>
        }

        public static T GetMax<T, TKey>(T[] array, Func<T, TKey> condition) where TKey : IComparable
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                //if(max.?  < array[i].?)
                if (condition(max).CompareTo(condition(array[i])) < 0)
                    max = array[i];
            }
            return max;
        }

        public static T GetMin<T, TKey>(T[] array, Func<T, TKey> condition) where TKey : IComparable
        {
            T min = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                //if(max.?  > array[i].?)
                if (condition(min).CompareTo(condition(array[i])) > 0)
                    min = array[i];
            }
            return min;
        }

        public static TKey[] Select<T, TKey>(T[] array, Func<T, TKey> condition)
        {
            TKey[] result = new TKey[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                //result[i] = array[i].GetComponent<TKey>();
                result[i] = condition(array[i]);
            }
            return result;
        }

        //GameObject[]  ---》  Enemy[]  
        //GameObject   -GetComponent<Enemy>-> Enemy
        //GameObject   -?-> Animation
    }