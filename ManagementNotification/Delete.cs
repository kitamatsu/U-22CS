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
        public Delete()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //選択されているノードを取得する
            if (treeView1.SelectedNode.Equals(null) || treeView1.SelectedNode.Parent.Equals(null))
            {
                MessageBox.Show("削除するログの日付を選択してください。",
                                "エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else if(dataGridView1.SelectedCells.Equals(null))
            {
                MessageBox.Show("削除するログを選択してください。",
                                "エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                int selectRow = int.Parse(dataGridView1.SelectedRows.ToString());
                int selectId = int.Parse(dataGridView1[0, selectRow].ToString());

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

                NotificationList.deleteNotification(selectId, deletePast);
            }
            
        }
    }
}
