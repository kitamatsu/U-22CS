using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementNotification.util
{
    public partial class Delete : Form
    {
        NotificationList NList = new NotificationList();
        public Delete()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //選択されているノードを取得する
            String selectLog = treeView1.SelectedNode.Text;

            //選択された日付以前のログを削除するか判定
            Boolean deletePast;

            if (radioButton1.Checked)
            {
                deletePast = false;
            }
            else
            {
                deletePast = true;
            }

            NList.deleteNotification(selectLog, deletePast);
        }
    }
}
