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

            listBox1.Text = "山口拓真";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AC.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Con.Show();
            this.Visible = false;
        }
    }
}
