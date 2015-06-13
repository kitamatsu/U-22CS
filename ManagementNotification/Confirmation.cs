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
        Form pre;

        public Confirmation()
        {
            InitializeComponent();
        }

        public Confirmation(Form form)
        {
            InitializeComponent();
            pre = form;
        }

        private void Confirmation_Load(object sender, EventArgs e)
        {
            //ツリー構造（管理名、日付）を表示するメソッドを呼び出す
            CD.DateDenotation(treeView1);

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
    }
}
