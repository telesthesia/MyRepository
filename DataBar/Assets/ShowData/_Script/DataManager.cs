using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

namespace DataShow
{
    public class DataManager : MonoBehaviour
    {
        //当前数据总数
        [HideInInspector] public int allCount;
        //DataTable类
        private DataTable dataTable;
        //可取的时间节点
        string[] rows = {"2017/7/23  1:50:51", "2017/7/23  3:50:51"};
        //可查询的时间和温度两列
        public string[] columns = {"时间", "温度"};
        //可通过时间查询
        private string selectCol = "时间";
        //查询到的时间和温度数组
        public object[,] tableArray;
        //查询到的温度数组
        private float[] tempData;
        //当前温度数组的最大值
        [HideInInspector] public float maxValue;
        //Button翻页管理类
        public GridPageView gpView;
        //上一页
        public Button previousPage;
        //下一页
        public Button nextPage;
        //上一页按钮状态
        private bool previousState = true;
        //下一页按钮状态
        private bool nextState = true;
        //页码Text
        public Text pageText;

        void Start()
        {
            previousPage.onClick.AddListener(PreviousPage);
            nextPage.onClick.AddListener(NextPage);
            LoadData();
            gpView.init(10, 1, allCount, true, 5, 5, updateItem);

        }

        /// <summary>
        /// 更新每个ButtonItem
        /// </summary>
        /// <param name="item">Button预制体</param>
        /// <param name="itemIndex">当前Button索引</param>
        /// <param name="showPageIndex">当前页的索引</param>
        /// <param name="text">读取的当前时间</param>
        /// <param name="value">读取的当前时间对应值</param>
        /// <param name="isReload">是否调用</param>
        private void updateItem(GameObject item, int itemIndex, int showPageIndex, string text, float value,
            bool isReload)
        {
            item.GetComponent<GridItem>().timeText.text = "[" + text + "]";
            item.GetComponent<GridItem>().value = value;
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PreviousPage()
        {
            gpView.curPageIndex --;
            if (gpView.curPageIndex >= 0 && previousState == true)
            {
                gpView.rollPosByPage(gpView.curPageIndex + 1);
            }
            else
            {
                previousState = false;
                gpView.curPageIndex = 0;
            }
        }

        //private bool isNextPage = true;
        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPage()
        {
            if (gpView.curPageIndex + 1 < gpView.pageCount)
            {
                gpView.curPageIndex++;
                gpView.rollPosByPage(gpView.curPageIndex + 1);
            }   
        }

        void Update()
        {
            pageText.text = (gpView.curPageIndex + 1).ToString() + "/" + gpView.pageCount.ToString();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public void LoadData()
        {
            ComExcelSystem.ExcelConnection("阻塞计数据示例" + ".xlsx");
            dataTable = ComExcelSystem.SelectData(selectCol, rows, columns);
            tempData = new float[dataTable.Rows.Count];
            tableArray = new object[dataTable.Rows.Count, dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    string value = dataTable.Rows[i][j].ToString();
                    if (j == 0)
                    {
                        tableArray[i, j] = value;
                    }
                    else
                    {
                        tableArray[i, j] = float.Parse(value);
                        tempData[i] = float.Parse(value);
                    }
                }
            }
            maxValue = GetMax(tempData);
            allCount = dataTable.Rows.Count;
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="data">查询数据数组</param>
        /// <returns></returns>
        public float GetMax(float[] data)
        {
            for (int r = 0; r < data.Length - 1; r++)
            {
                for (int c = r + 1; c < data.Length; c++)
                {
                    if (data[c] > data[r])
                    {
                        float temp = data[r];
                        data[r] = data[c];
                        data[c] = temp;
                    }
                }
            }
            return data[0];
        }
    }
}