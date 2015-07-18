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

            //NotificationList.loadList();

            

            
            
        }


        private void Confirmation_Load(object sender, EventArgs e)
        {
            //ツリー構造（管理名、日付）を表示するメソッドを呼び出す
            //行を追加するオプションを非表示
            dataGridView1.AllowUserToAddRows = false;



            //CD.sortTreeView();
            DB db = new DB();
            db.ConnectAndQuery("tamura@yahoo.co.jp", 1);  //ログイン時に保存したメールアドレスを使用する、未送信通知の受信は1

            CD = new ConfirmationDenotation(treeView1);

        }

        /*
         * closingはclose()が呼び出される
         *×が押されてclose()の処理のあとに呼び出される
         */
        private void Confirmation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.Write("確認画面から閉じます");
            NotificationList.saveXML();
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
                    treeView1.Nodes.Clear();
                    CD.createNodes();
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

        //削除した通知の再送信
        private void button3_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.ConnectAndQuery("tamura@yahoo.co.jp", 2);

            treeView1.Nodes.Clear();
            CD = new ConfirmationDenotation(treeView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
