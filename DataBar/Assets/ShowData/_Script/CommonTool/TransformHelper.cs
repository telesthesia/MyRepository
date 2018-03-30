using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 按时间查询变化组件助手类
/// </summary>

/// <summary>
/// 变换组件助手类
/// </summary>
public class TransformHelper
{
    /// <summary>
    /// 未知层级关系，根据名称查找后代物体。
    /// </summary>
    /// <param name="parentTF">父物体变换组件</param>
    /// <param name="childName">需要检索的子物体名称</param>
    /// <returns></returns>
    public static Transform FindChild(Transform parentTF, string childName)
    {
        //递归：将问题转移给范围缩小的同类子问题
        //作用：将复杂的问题简单化。
        //步骤：找儿子   如果没有  转移给儿子

        //1.找儿子
        Transform childTF = parentTF.Find(childName);
        //如果查找到 则返回子物体变换组件 退出方法
        if (childTF != null) return childTF;
        //2.如果没有
        for (int i = 0; i < parentTF.childCount; i++)
        {
//3.转移给儿子
            childTF = FindChild(parentTF.GetChild(i), childName);
            if (childTF != null) return childTF;
        }
        return null;
        /*
         3!   阶乘  3  *  2 * 1
         */
    }

    /// <summary>
    /// 逐渐注视目标点旋转
    /// </summary>
    /// <param name="targetTF">变换组件</param>
    /// <param name="targetPos">目标点</param>
    /// <param name="rotateSpeed">旋转速度</param>
    public static void LookPostion(Transform targetTF, Vector3 targetPos, float rotateSpeed)
    {
        Vector3 dir = targetPos - targetTF.position;
        LookDirection(targetTF, dir, rotateSpeed);
    }

    /// <summary>
    /// 逐渐注视目标方向旋转
    /// </summary>
    /// <param name="targetTF">变换组件</param>
    /// <param name="targetDir">目标方向</param>
    /// <param name="rotateSpeed">旋转速度</param>
    public static void LookDirection(Transform targetTF, Vector3 targetDir, float rotateSpeed)
    {
        //transform.LookAt(目标点);      一帧旋转到位
        Quaternion dir = Quaternion.LookRotation(targetDir);
        targetTF.rotation = Quaternion.Lerp(targetTF.rotation, dir, Time.deltaTime*rotateSpeed);
    }

    /// <summary>
    /// 计算周边对象
    /// </summary>
    /// <param name="currentTF">当前对象变换组件</param>
    /// <param name="distance">距离</param>
    /// <param name="angle">角度</param>
    /// <param name="targetTags">检索目标的标签</param>
    /// <returns></returns>
    public static Transform[] CalculateAroundTransform(Transform currentTF, float distance, float angle,
        string[] targetTags)
    {
        //1.找到所有敌人
        List<Transform> list = new List<Transform>();
        foreach (var tag in targetTags)
        {
            GameObject[] tempGos = GameObject.FindGameObjectsWithTag(tag);
            list.AddRange(ArrayHelper.Select(tempGos, o => o.transform));
        }

        //2.查找满足条件的所有敌人：攻击范围内
        list = list.FindAll(tf =>
            Vector3.Distance(tf.position, currentTF.position) <= distance &&
            Vector3.Angle(currentTF.forward, tf.position - currentTF.position) <= angle/2
            );
        return list.ToArray();
    }

    public static GameObject[] FindChild(Transform transform)
    {
        GameObject[] trans = new GameObject[transform.childCount];
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                trans[i] = transform.GetChild(i).gameObject;
            }
            return trans;

        }
        else
            return trans;
    }




}
