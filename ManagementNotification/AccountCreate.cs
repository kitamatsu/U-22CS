using ManagementNotification.db;
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
    public partial class AccountCreate : Form
    {
        Form pre;
        DB db = new DB();

        public AccountCreate()
        {
            InitializeComponent();
        }

        public AccountCreate(Form form)
        {
            InitializeComponent();
            pre = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(userNameTB.Text.Length < 1 ||
                emailTB.Text.Length < 1 ||
                passTB.Text.Length < 1 ||
                passConfirmationTB.Text.Length < 1)
            {
                label13.Text = "未入力項目があります";
                label13.BackColor = Color.Red;
            }
            else if (passTB.Text != passConfirmationTB.Text)
            {
                label13.Text = "パスワードが一致しません";
                label13.BackColor = Color.Red;
            }
            else
            {
                DialogResult result = MessageBox.Show("入力内容で登録します。",
                                                    "確認",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Exclamation);

                if (result == DialogResult.OK)
                {
                    //DBクラスのアカウント追加メソッドを呼び出す
                    db.ConnectAndQuery(userNameTB.Text, emailTB.Text, passTB.Text);

                    //Transmitテーブルにデータを追加
                    db.TransmitConnectAndQuery(userNameTB.Text, passTB.Text);

                    //アカウント認証画面に戻る
                    pre.Show();
                    this.Visible = false;
                }
            }
        }

        private void AccountCreate_FormClosed(object sender, FormClosedEventArgs e)
        {
            NotificationList.saveXML();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //前のページに戻る
            
        }
    }
}
