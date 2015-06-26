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
            }
            else if (passTB.Text != passConfirmationTB.Text)
            {
                label13.Text = "パスワードが一致しません";
            }
            else
            {
                MessageBox.Show("入力完了");
            }
            
        }
    }
}
