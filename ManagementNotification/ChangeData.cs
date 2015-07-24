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
    public partial class ChangeData : Form
    {
        DB db;
        Confirmation Con;
        String email;
        public ChangeData()
        {
            InitializeComponent();
            db = new DB();
        }

        public void getEmail(String email)
        {
            this.email = email;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(inputName.Text.Length < 1 &&
               inputPassword.Text.Length < 1 &&
               inputEmail.Text.Length < 1)
            {
                MessageBox.Show("変更内容を入力してください", "エラー",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                String message = "";
                if (inputName.Text.Length > 0)
                {
                    message += "\r\nユーザネーム：" + inputName.Text;
                }

                if (inputPassword.Text.Length > 0)
                {
                    message += "\r\nパスワード：" + inputPassword.Text;
                }

                if (inputEmail.Text.Length > 0)
                {
                    message += "\r\nメールアドレス：" + inputEmail.Text;
                }


                DialogResult result = MessageBox.Show("以下の内容を変更しますか？" + message,
                "確認",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    string newName = inputName.Text;
                    string newPassword = inputPassword.Text;
                    string newEmail = inputEmail.Text;

                    String[] accountData = { newName, newPassword, newEmail, email };
                    if (newEmail.Length > 0)
                    {
                        if (db.CheckEmailConnectAndQuery(newEmail).Equals(""))
                        {
                            ChangeAccountData(accountData);
                        }else{
                            MessageBox.Show("入力したメールアドレスは既に登録されています。", "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        ChangeAccountData(accountData);
                    }

                }

                
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Con = new Confirmation();
            Con.Show();
            Con.getEmail(email);
            this.Visible = false;
        }


        private void ChangeAccountData(String[] accountData)
        {
            db.ChangeDataConnectAndQuery(accountData);

            MessageBox.Show("ユーザ情報変更が完了しました",
                "完了", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            if (inputEmail.Text.Length > 0)
            {
                email = inputEmail.Text;
            }
            Con = new Confirmation();
            Con.Show();
            Con.getEmail(email);
            this.Visible = false;
        }

        private void ChangeData_FormClosed(object sender, FormClosedEventArgs e)
        {
            NotificationList.saveXML();
            Application.Exit();
        }
    }
}
