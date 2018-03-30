using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 跳页
/// </summary>
public class JumpPage : MonoBehaviour
{

    //Button翻页管理类
    public GridPageView gpView;
    /// <summary>
    /// 翻页
    /// </summary>
    public void JumpPageHandle()
    {
        string number = GameObject.FindGameObjectWithTag("InputText").GetComponent<Text>().text;
        if (string.IsNullOrEmpty(number))
            return;
        
        if (int.Parse(number) > 0 && int.Parse(number) <= gpView.pageCount)
        {
            gpView.rollPosByPage(int.Parse(number));
        }
        else
            return;
    }
}
