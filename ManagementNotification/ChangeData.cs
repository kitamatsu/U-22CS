﻿using ManagementNotification.db;
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
            inputPassword.Text = this.email;
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

                    if (newEmail.Length > 0)
                    {
                        if (db.CheckEmailConnectAndQuery(newEmail).Equals(""))
                        {
                            inputPassword.Text = "ok";
                        }else{
                            inputPassword.Text = "ng";
                        }
                    }

                }

                
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Con = new Confirmation();
            Con.Show();
            this.Visible = false;
        }
    }
}
