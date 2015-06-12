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
        //ConfirmationDenotation CD = new ConfirmationDenotation();

        public Confirmation()
        {
            InitializeComponent();
        }

        private void Confirmation_Load(object sender, EventArgs e)
        {
            //ツリー構造（管理名、日付）を表示するメソッドを呼び出す
            ConfirmationDenotation.DateDenotation(treeView1);
            //treeView1.Nodes.Add("aaa");
        }
    }
}
