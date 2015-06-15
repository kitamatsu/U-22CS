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
    public partial class Confirmation : Form
    {
        //ConfirmationDenotationクラスのインスタンス作成
        ConfirmationDenotation CD = new ConfirmationDenotation();
        RightDelete RD = new RightDelete();
        Form pre;

        public Confirmation()
        {
            InitializeComponent();
        }

        public Confirmation(Form form)
        {
            InitializeComponent();
            pre = form;
            CD = new ConfirmationDenotation(treeView1);
        }


        private void Confirmation_Load(object sender, EventArgs e)
        {
            //ツリー構造（管理名、日付）を表示するメソッドを呼び出す
            //CD.DateDenotation(treeView1);
            //CD.SetNode(treeView1);
            //行を追加するオプションを非表示
            dataGridView1.AllowUserToAddRows = false;
           
        }

        //closingはclose()が呼び出される
        //×が押されてclose()の処理のあとに呼び出される
        private void Confirmation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.Write("確認画面から閉じます");
            Application.Exit();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //日付が選択されたときDataGridViewに表示する
            CD.selectLastNode(treeView1, dataGridView1);
        }

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 右ボタンのクリックか？
            if (e.Button == MouseButtons.Right)
            {
                    
                //RD.deleteRow(dataGridView1, e.RowIndex);

                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;

                //選択された行番号
                int selectRow = e.RowIndex + 1;

                //メッセージボックスに選択した行を削除するか表示
                DialogResult result = MessageBox.Show("行番号:　" + selectRow + "\r\n選択した行を削除します。",
                                                        "削除",
                                                        MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Exclamation);

                if (result == DialogResult.OK)
                {
                    //DataGridViewから選択された行の通知IDのデータを削除する
                    dataGridView1.Rows.RemoveAt(e.RowIndex);

                    //リストから選択された行の通知IDのデータを削除する
                    int idNum = (int)dataGridView1["NotificationID", e.RowIndex].Value;
                    NotificationList.removeListByID(idNum);

                    //コンソールにリスト内容を表示する
                    NotificationList.ViewListToConsole();
                }

            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                {

            }
        }
    }
}
