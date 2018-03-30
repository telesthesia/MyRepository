using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataShow;

/// <summary>
/// 控制每个Button事件响应
/// </summary>
public class GridItem : MonoBehaviour {
    //时间控制按钮Text
    public Text timeText;
    //当前时间对应值
    [HideInInspector]
    public float value;
    //当前时间对应值Text
    private Text dataValueText;
    //当前时间对应的滑条
    private Slider slider;
    //当前时间对应值/最大值的比率
    private float rate;    
    //数据管理类
    private DataManager dataManager;

    void Start ()
    {
        slider = GameObject.Find("DataControllerBar").GetComponent<Slider>();
        dataValueText = TransformHelper.FindChild(slider.transform, "DataValueText").GetComponent<Text>();
        dataManager = GameObject.Find("DataShowPanel").GetComponent<DataManager>();
    }

    /// <summary>
    /// 点击Button按钮触发事件
    /// </summary>
    public void OnClickHandle()
    {
        rate = value / dataManager.maxValue;
        slider.value = rate;  
        dataValueText.text = value.ToString();
    }
}
