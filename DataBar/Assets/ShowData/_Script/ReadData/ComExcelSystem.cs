using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.IO;
using System.Data;
using UnityEngine.UI;
using System;
public enum ExcelType
{
    XLSX,
    XLS
}
public class ComExcelSystem : MonoBehaviour
{
    public string filePath = "气相氚渗透装置数据示例.xls";
    public static DataSet result;
    public static int ColumnCount;
    public static int RowCount;
    public static IExcelDataReader excelReader;
    public static string dataType;

    protected ComExcelSystem()
    {
        comExcelSystem = this;
    }
    private static ComExcelSystem comExcelSystem;
    public static ComExcelSystem Instance()
    {
        return comExcelSystem;
    }
    // Use this for initialization
    void Start()
    {
        //ExcelConnection(filePath);
        string[] rows = { "2017/7/23  1:50:51", "2017/7/23  1:51:03" };
        string[] columns = { "毫秒", "温度" };
        string selectRowName = "时间";
        //SelectData(selectRowName, TimeFormat(rows), columns);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static string[] TimeFormat(string[] rows)
    {
        string tempStr;
        for (int i = 0; i < rows.Length; i++)
        {
            string[] temps = rows[i].Split(' ');
            string[] temps2 = temps[0].Split('/');
            tempStr = temps2[1] + "/" + temps2[2] + "/" + temps2[0] + " " + temps[2];
            rows[i] = tempStr;
        }
        return rows;
    }
    private static string SwitchExcelType(ExcelType excelType)
    {
        switch (excelType)
        {
            case ExcelType.XLSX:
                return ".xlsx";
            case ExcelType.XLS:
                return ".xls";
            default:
                return "";
        }
    }
    /// <summary>
    /// 连接Excel文件：通过直接在编辑器将所需Excel文件拽即可
    /// </summary>
    /// <param name="dataSource">拖拽到的Excel文件</param>
    /// <param name="excelType">Excel文件类型</param>
    public static void ExcelConnection(UnityEngine.Object dataSource, ExcelType excelType)
    {
        string ssssa = dataSource.GetType().ToString();
        dataType = SwitchExcelType(excelType);
        FileStream stream = File.Open(Application.streamingAssetsPath + @"/" + dataSource.name + SwitchExcelType(excelType), FileMode.Open, FileAccess.Read);
        //dataType = dataSource.name.Substring(dataSource.name.LastIndexOf('.') + 1);
        switch (dataType)
        {
            case ".xls":
                //读取2003以后版本
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                break;
            case ".xlsx":
                //读取2007以后版本
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                break;
            default:
                break;
        }
        while (excelReader.Read())
        {
            result = excelReader.AsDataSet();
        }
        ColumnCount = result.Tables[0].Columns.Count;//获取列数
        RowCount = result.Tables[0].Rows.Count;//获取行数
    }
    /// <summary>
    /// 连接Excel文件：在编辑器中写上详细"文件名.文件类型"格式
    /// </summary>
    /// <param name="ExcelName">文件名.文件类型</param>
    public static void ExcelConnection(string ExcelName)
    {
        FileStream stream = File.Open(Application.streamingAssetsPath + @"/" + ExcelName, FileMode.Open, FileAccess.Read);
        dataType = ExcelName.Substring(ExcelName.LastIndexOf('.') + 1);
        switch (dataType)
        {
            case "xls":
                //读取2003以后版本

                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                break;
            case "xlsx":
                //读取2007以后版本

                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                break;
            default:
                break;
        }
        while (excelReader.Read())
        {
            result = excelReader.AsDataSet();
        }
        ColumnCount = result.Tables[0].Columns.Count;//获取列数
        RowCount = result.Tables[0].Rows.Count;//获取行数
        //excelReader.Close();
    }
    private static int selectStartColumn;
    private static int selectEndColumn;

    private static int selectRowIndex;

    private static int selectStartRow;
    private static int selectEndRow;
    /// <summary>
    /// 选择需要的数据:
    /// </summary>
    /// <param name="selectCol">以该列为基准，进行行选择</param>
    /// <param name="rows">选择的行：最大值和最小值</param>
    /// <param name="columns">选择的列：开始列的名称和结束列的名称</param>
    /// <returns>DataTable返回值</returns>
    public static DataTable SelectData(string selectCol, string[] rows, string[] columns)
    {
        rows = TimeFormat(rows);
        //找到选择的列的索引
        for (int i = 0; i < ColumnCount; i++)
        {
            for (int k = 0; k < columns.Length; k++)
            {
                if (result.Tables[0].Rows[0][i].Equals(columns[k]))
                {
                    if (columns[k].Equals(columns[0]))
                    {
                        selectStartColumn = i;
                    }
                    else
                    {
                        selectEndColumn = i;
                    }
                }
            }
        }
        if (selectStartColumn >= selectEndColumn)
        {
            selectEndColumn = selectStartColumn;
        }
        //找到选择的行的列索引
        for (int i = 0; i < ColumnCount; i++)
        {
            if (result.Tables[0].Rows[0][i].Equals(selectCol))
            {
                selectRowIndex = i;
                break;
            }
        }

        for (int i = 1; i < RowCount; i++)
        {
            for (int k = 0; k < rows.Length; k++)
            {
                //string sss = result.Tables[0].Rows[i][selectRowIndex].ToString();

                if ((result.Tables[0].Rows[i][selectRowIndex].ToString().Replace(" AM", "")).Equals(rows[k]))
                {
                    if (rows[k].Equals(rows[0]))
                    {
                        selectStartRow = i;
                    }
                    else
                    {
                        selectEndRow = i;
                    }
                }
            }
        }

        return GetInArray(selectStartRow, selectEndRow, selectStartColumn, selectEndColumn);
    }

    /// <summary>
    /// 根据输入位置返回数据
    /// </summary>
    /// <param name="RowBegin">起始行</param>
    /// <param name="RowEnd">结束行</param>
    /// <param name="ColumnBegin">起始列</param>
    /// <param name="ColumnEnd">结束列</param>
    /// <returns></returns>
    private static DataTable GetInArray(int RowBegin, int RowEnd, int ColumnBegin, int ColumnEnd)
    {
        DataTable table = new DataTable();
        //Temp : 目前处理方式：只能选择2列或者一列
       
        //for (int i = 0; i < ColumnEnd - ColumnBegin + 1; i++)
        //{
        //    table.Columns.Add();
        //}

        for (int i = 0; i < 2; i++)
        {
            table.Columns.Add();
        }
        for (int i = RowBegin; i < RowEnd + 1; i++)
        {
            DataRow dataRow = table.NewRow();
            for (int j = ColumnBegin; j < ColumnBegin + 1; j++)
            {
                dataRow[j - ColumnBegin] = result.Tables[0].Rows[i][j];
            }
            for (int j = ColumnEnd; j < ColumnEnd + 1; j++)
            {
                dataRow[j - ColumnEnd + 1] = result.Tables[0].Rows[i][j];
            }

            //for (int j = ColumnBegin; j < ColumnEnd + 1; j++)
            //{
            //    dataRow[j - ColumnBegin] = result.Tables[0].Rows[i][j];
            //}

            table.Rows.Add(dataRow);
        }


        return table;
    }
    public static void ShowOnText(DataTable dataTable, Text readData)
    {
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                string nvalue = dataTable.Rows[i][j].ToString();
                Debug.Log(nvalue);
                if (i > 0)
                {
                    readData.text += nvalue + "\t\t";
                }
                else
                {
                    readData.text += nvalue + "   \t";
                }
            }
            readData.text += "\n";
        }
    }

    private void ShowOnText(Text readData)
    {
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                string nvalue = result.Tables[0].Rows[i][j].ToString();
                Debug.Log(nvalue);
                if (i > 0)
                {
                    readData.text += "\t\t" + nvalue;
                }
                else
                {
                    readData.text += "   \t" + nvalue;
                }
            }
            readData.text += "\n";
        }
    }
}



