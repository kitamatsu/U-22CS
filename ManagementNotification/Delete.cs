using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace ManagementNotification.util
{
    public partial class Delete : Form
    {
        Form pre;

        ConfirmationDenotation con;

        public Delete()
        {
            InitializeComponent();
            
        }

        //前の画面を引数とするコンストラクタ
        public Delete(Form form)
        {
            InitializeComponent();
            pre = form;

            //テストデータ
            con =  new ConfirmationDenotation(treeView1);

            con.DateDenotation();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            int idx = dataGridView1.Rows.Count - 3;
            dataGridView1.Rows[idx].Cells[0].Value = "1";
            dataGridView1.Rows[idx].Cells[1].Value = "1時2分3秒";
            dataGridView1.Rows[idx].Cells[2].Value = "LINE";
            dataGridView1.Rows[idx].Cells[3].Value = "テスト";
            
            idx = dataGridView1.Rows.Count - 2;
            dataGridView1.Rows[idx].Cells[0].Value = "2";
            dataGridView1.Rows[idx].Cells[1].Value = "4時5分6秒";
            dataGridView1.Rows[idx].Cells[2].Value = "Twitter";
            dataGridView1.Rows[idx].Cells[3].Value = "テスト2";

            idx = dataGridView1.Rows.Count - 1;
            dataGridView1.Rows[idx].Cells[0].Value = "3";
            dataGridView1.Rows[idx].Cells[1].Value = "7時8分9秒";
            dataGridView1.Rows[idx].Cells[2].Value = "アプリ";
            dataGridView1.Rows[idx].Cells[3].Value = "テスト3";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                //選択されているノードを取得する
                if(dataGridView1.SelectedCells.Equals(null))  //DataGridViewのログが選択されていない場合
                {
                    MessageBox.Show("削除するログを選択してください。",
                                    "エラー",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else
                {
                    

                    int selectRow = dataGridView1.CurrentCell.RowIndex;
                    ArrayList selectId = new ArrayList();
                    int[] deleteId = null;

                    selectId.Add(dataGridView1[0, selectRow].Value.ToString());
                    int selectIdIndex = 1;      //selectId配列の添え字


                    //選択された日時以前のログを削除する
                    if (radioButton2.Checked)
                    {
                        for (int i = 0; dataGridView1.Rows.Count > i; i++)
                        {
                            int testDeleteId = int.Parse(dataGridView1[0, i].Value.ToString());

                            if (int.Parse(selectId[0].ToString()) > testDeleteId)
                            {
                                selectId.Add(testDeleteId);
                                selectIdIndex++;
                            }

                        }
                        
                    }

                    deleteId = new int[selectIdIndex];
                    for (int i = 0; selectIdIndex > i; i++)
                    {
                        deleteId[i] = int.Parse(selectId[i].ToString());
                    }

                    

                    DialogResult result = MessageBox.Show("選択されたログを削除しますか？",
                                    "ログの削除",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Exclamation,
                                    MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        NotificationList.removeListByID(deleteId);
                        NotificationList.ViewListToConsole();
                    }

                    


                }
            
            
            
        }

        private void Delete_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.Write("削除画面から終了");
            Application.Exit();
        }

        //戻るボタンの処理
        private void button2_Click(object sender, EventArgs e)
        {
            pre.Show();
            this.Visible = false;
        }
    }
}
