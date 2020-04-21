using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;   
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
 
namespace demoado
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strSQL = "";

            if (textBox1.Text != "")
                strSQL = textBox1.Text;

            if (radioButton1.Checked)
            {
                DataTable dt = DataAccess.ExecuteQuery(DataAccess.GetConnectionString(), strSQL);
                ShowData(dt);
            }
            else if(radioButton2.Checked) 
            {
                if (DataAccess.ExecuteNoQuery(DataAccess.GetConnectionString(), strSQL))
                    textBox1.Text = "Access非查询SQL执行成功！"; 
                else
                    textBox1.Text = "Access非查询SQL执行失败！";
            }
            else if (radioButton3.Checked)
            {
                MySqlDataReader dr = DataAccess.ExecuteReader2(DataAccess.GetConnectionString2(), strSQL);
                ShowData(dr); 
            }
            else if (radioButton4.Checked)
            {
                if (DataAccess.ExecuteNoQuery2(DataAccess.GetConnectionString2(), strSQL))
                    textBox1.Text = "MySQL非查询SQL执行成功！";
                else
                    textBox1.Text = "MySQL非查询SQL执行失败！";
            }
        }

        private void ShowData(DataTable dt)
        {
            if (dt == null) return;

            listBox1.Items.Clear();
            
            string strValue = "";

            //显示字段名
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strValue += dt.Columns[i].ToString();
                strValue += "|";
            }
            listBox1.Items.Add(strValue);

            foreach (DataRow row in dt.Rows)
            {
                strValue = ""; 

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strValue += row[i].ToString();
                    strValue += "|";
                }
                listBox1.Items.Add(strValue);
            }
        }

        private void ShowData(MySqlDataReader dr)
        {
            if (dr == null) return;

            listBox1.Items.Clear();

            string strValue = "";

            //显示字段名
            for (int i = 0; i < dr.FieldCount; i++)
            {
                strValue += dr.GetName(i);
                strValue += "|";
            }
            listBox1.Items.Add(strValue);

            //显示记录集
            while (dr.Read()) 
            {
                strValue = "";

                for (int j = 0; j < dr.FieldCount; j++)
                {
                    if (dr.IsDBNull(j))
                        strValue += "NULL";
                    else
                        strValue += dr.GetValue(j).ToString();
                    strValue += "|";
                }

                listBox1.Items.Add(strValue);

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(listBox1.SelectedIndex.ToString());   
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "检索张三老师所授课程的课程号、课程名";
            textBox1.Text = "SELECT 课程号, 课程名\r\nFROM 课程 \r\nWHERE 课程.教师名称 = '张三'";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "检索年龄大于 20 岁的男同学的学号和姓名";
            textBox1.Text = "SELECT 学号, 姓名\r\n" +
                "FROM 学生\r\n" +
                "WHERE 学生.年龄 > 20 AND 学生.性别 = '男'; ";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "检索至少选修张三老师所授课程中一门课的学生姓名";
            textBox1.Text = "SELECT 学生.姓名\r\n" +
                "FROM 学生, 选课, 课程\r\n" +
                "WHERE 学生.学号 = 选课.学号 AND 选课.课程号 = 课程.课程号 AND 课程.教师名称 = '张三'; ";
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "检索小王同学不学的课程的课程号";
            textBox1.Text = "SELECT 课程.课程号\r\n" +
                "FROM 课程\r\n" +
                "WHERE 课程.课程号 NOT IN(\r\n" +
                "\tSELECT 选课.课程号\r\n" +
                "\tFROM 选课, 学生\r\n" +
                "\tWHERE 学生.姓名 = '小王' AND 学生.学号 = 选课.学号); ";
        }
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "检索至少选修两门课的学生学号";
            textBox1.Text = "SELECT 学号\r\n" +
                "FROM 选课\r\n" +
                "GROUP BY 学号\r\n" +
                "HAVING COUNT(课程号) >= 2;";
        }
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "检索全部学生都选修的课程号与课程名";
            textBox1.Text = "SELECT 课程号, 课程名\r\n" +
                "FROM 课程\r\n" +
                "WHERE NOT EXISTS(\r\n" +
                "\tSELECT *\r\n" +
                "\tFROM 学生\r\n" +
                "\tWHERE NOT EXISTS(\r\n" +
                "\t\tSELECT *\r\n" +
                "\t\tFROM 选课\r\n" +
                "\t\tWHERE 选课.课程号 = 课程.课程号 AND 学生.学号 = 选课.学号\r\n)); ";
        }
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "检索选修课程包括李四老师所授课程的学生学号";
            textBox1.Text = "SELECT 学号\r\n" +
                "FROM 选课\r\n" +
                "WHERE EXISTS(\r\n" +
                "\tSELECT*\r\n" +
                "\tFROM 课程\r\n" +
                "\tWHERE 选课.课程号= 课程.课程号 AND 课程.教师名称= '李四'\r\n); ";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "SELECT * \r\n" +
                "FROM 学生 \r\n";
            this.button1.PerformClick();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "SELECT * \r\n" +
                "FROM 课程 \r\n";
            this.button1.PerformClick();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "SELECT * \r\n" +
                "FROM 选课 \r\n";
            this.button1.PerformClick();
        }
    }
}