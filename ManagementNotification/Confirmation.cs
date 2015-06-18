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
        ConfirmationDenotation CD;
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
            CD = new ConfirmationDenotation();
        }


        private void Confirmation_Load(object sender, EventArgs e)
        {
            //ツリー構造（管理名、日付）を表示するメソッドを呼び出す
            CD.DateDenotation(treeView1);
            //CD.SetNode(treeView1);
            //行を追加するオプションを非表示
            dataGridView1.AllowUserToAddRows = false;
           
        }

        /*
         * closingはclose()が呼び出される
         *×が押されてclose()の処理のあとに呼び出される
         */
        private void Confirmation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.Write("確認画面から閉じます");
            Application.Exit();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
                
        }

        /*
         * 選択行の削除する
         */
        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 右ボタンのクリックか？
            if (e.Button == MouseButtons.Right)
            {
                //ヘッダ以外のセルか？
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    RD.deleteRow(dataGridView1, e.RowIndex);
                }
            }
        }

        /*
         * TreeViewの項目からリストデータを削除する
         */
        private void treeView1_MouseDown(object sender, MouseEventArgs MouseEA)
        {
            if (MouseEA.Button == MouseButtons.Left)
            {
                //日付が選択されたときDataGridViewに表示する
                CD.selectLastNode(treeView1, dataGridView1,MouseEA);
            }

            if (MouseEA.Button == MouseButtons.Right)
            {
                RD.deleteNode(treeView1,dataGridView1,MouseEA);

            }
        }
    }
}
