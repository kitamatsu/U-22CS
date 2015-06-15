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
        Confirmation con;
        Delete del;

        public Form1()
        {
            InitializeComponent();
            con = new Confirmation(this);
            del = new Delete(this);
            
        }

        //画面遷移
        private void confirmation_Click(object sender, EventArgs e)
        {    
            con.Show();
            this.Visible = false;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            
            del.Show();
            this.Visible = false;
        }
            
    }
}
