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
                int[] selectId = null;
                
                selectId[0] = int.Parse(dataGridView1[0, selectRow].ToString());
                int selectIdIndex = 1;      //selectId配列の添え字


                //選択された日時以前のログを削除する
                if (radioButton2.Checked)
                {
                    for (int i = 0; dataGridView1.Rows.Count > i; i++)
                    {
                        int testDeleteId = int.Parse(dataGridView1[0, i].ToString());

                        if(selectId[0] > testDeleteId){
                            selectId[selectIdIndex] = testDeleteId;
                            selectIdIndex++;
                        }

                    }
                }

                NotificationList.removeListByID(selectId);
            }
            
        }
    }
}
