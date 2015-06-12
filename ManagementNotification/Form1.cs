using ManagementNotification.test;
using ManagementNotification.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementNotification
{
    public partial class Form1 : Form
    {
        //画面
        Confirmation con　= new Confirmation();
        Delete del = new Delete();

        public Form1()
        {
            InitializeComponent();
            Confirmation con = new Confirmation();

            //テスト用コード
            Test act = new Test();
            
        }


        //画面遷移
        //×押してもHide()で裏が生きてるから停止ボタンを忘れずに
        //そのうちcontroller作る
        private void confirmation_Click(object sender, EventArgs e)
        {    
            con.Show();
            this.Hide();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            
            del.Show();
            this.Hide();
        }

        
    }
}
