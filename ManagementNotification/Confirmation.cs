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
        Form pre;

        ConfirmationDenotation CD = new ConfirmationDenotation();

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
            //CD.DateDenotation(treeView1);
            //CD.DateDenotation(View1);
        }

        //closingはclose()が呼び出される
        //×が押されてclose()の処理のあとに呼び出される
        private void Confirmation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.Write("確認画面から閉じます");
            Application.Exit();
        }
    }
}
