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
            db.ConnectAndQuery(usernameTB.Text, passwordTB.Text, 2);

        }

        public void thisClose(int num)
        {
            if (num != 0)
            {
                this.Close();
                Con.Show();
            }
            else if (num == 0)
            {
                label4.Text = "ユーザネーム,パスワードが一致しません。";
                label4.BackColor = Color.Red;
            }
        }
    }
}
