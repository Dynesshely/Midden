﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class paiming : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            DataBase db = new DataBase();
            db.OpenDataBase();
            
            string str3 = "select name,zf=sum(YW_chengji+SX_chengji+YY_chengji),mc=identity(int,1,1) into paiming from chengji group by name order by sum(YW_chengji+SX_chengji+YY_chengji) desc ";
            string str2 = "drop table paiming";
            SqlCommand cmd = new SqlCommand(str2, db.CONN);
            cmd.ExecuteNonQuery();
            cmd.CommandText = str3;
            cmd.ExecuteNonQuery();
            string str1 = "select name,zf,mc from paiming";
            SqlDataAdapter da = new SqlDataAdapter(str1, db.CONN);
            DataSet ds = new DataSet();
            da.Fill(ds, "paiming");
            GridView1.DataSource = ds;
            GridView1.DataBind();
           


                db.CloseDataBase();
        }
        

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataBase db = new DataBase();
        db.OpenDataBase();
        string str1 = "select name,zf,mc from paiming";
        SqlDataAdapter da = new SqlDataAdapter(str1, db.CONN);
        DataSet ds = new DataSet();
        da.Fill(ds, "paiming");
       
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = ds;         //引用刚才建立的数据源
        GridView1.DataBind();
        db.CloseDataBase();

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int numCount = GridView1.Rows.Count;
        if (GridView1.PageIndex == GridView1.PageCount - 1)     //尾页
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 计算自动填充的行数
                numCount++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                // 计算完毕，在此添加缺少的行
                int toLeft = GridView1.PageSize - numCount;
                int numCols = GridView1.Rows[0].Cells.Count;

                for (int i = 0; i < toLeft; i++)
                {
                    GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                    for (int j = 0; j < numCols; j++)
                    {
                        TableCell cell = new TableCell();
                        cell.Text = "&nbsp;";
                        row.Cells.Add(cell);
                    }
                    GridView1.Controls[0].Controls.AddAt(numCount + 1 + i, row);
                }
            }
        }

    }
}