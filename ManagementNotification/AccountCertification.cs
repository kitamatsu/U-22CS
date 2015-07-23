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
    public partial class AccountCertification : Form
    {
        Confirmation Con;
        AccountCreate AC;
        DB db;
        Reissue Re;
        String email = "";

        public AccountCertification()
        {
            InitializeComponent();
            Con = new Confirmation(this);
            AC = new AccountCreate(this);
            db = new DB();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AC.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //メールアドレスを取得する
            email = db.LoginConnectAndQuery(usernameTB.Text, passwordTB.Text);

            //アカウントIDが存在すればリストにデータを格納しページ遷移する
            if (!email.Equals(""))
            {

                //データをリストに格納するメソッドを作成
                Con.getEmail(email);
                Con.Show();
                
                this.Visible = false;
            }
            else if (email.Equals(""))
            {
                label4.Text = "ユーザネーム,パスワードが一致しません。";
                label4.BackColor = Color.Red;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Re = new Reissue();
            Re.Show();
            this.Visible = false;
        }
    }
}
