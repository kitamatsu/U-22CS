using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagementNotification.db;

namespace ManagementNotification
{
    public partial class Reissue : Form
    {
        AccountCertification AC;

        public Reissue()
        {
            InitializeComponent();
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {

            DB db = new DB();
            db.ConnectAndQuery(textBox1.Text, 3);

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            AC = new AccountCertification();
            AC.Show();
            this.Visible = false;
        }

    }
}
