using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementNotification.util
{
    class ConfirmationDenotation
    {
        private TreeView root;
        private TreeNode TNcN, TNy, TNm, TNd;

        public ConfirmationDenotation(TreeView TView1)
        {
            root = TView1;
            createNodes();
        }

        //Treeを生成する
        //誰も読まない、読めないだろうから細かいコメントはなし
        public void createNodes()
        {
            for (int i = 0; i < NotificationList.diffNameList().Count; i++)
            {
                Notification tree = NotificationList.list.Find(x => x.ChildName == NotificationList.diffNameList()[i]);
                this.TNcN = new TreeNode(tree.ChildName.ToString());
                root.Nodes.Add(TNcN);

                List<Notification> nameOnly = NotificationList.list.FindAll(x => x.ChildName == this.TNcN.Text);
                for (int j = 0; j < NotificationList.diffNameListByTime(0, nameOnly).Count; j++)
                {
                    tree = nameOnly.Find(x => x.Date.Year == NotificationList.diffNameListByTime(0, nameOnly)[j]);
                    this.TNy = new TreeNode(tree.getYear());
                    TNcN.Nodes.Add(TNy);

                    List<Notification> yearOnly = nameOnly.FindAll(x => x.getYear() == this.TNy.Text);
                    for (int k = 0; k < NotificationList.diffNameListByTime(1, yearOnly).Count; k++)
                    {
                        tree = yearOnly.Find(x => x.Date.Month == NotificationList.diffNameListByTime(1, yearOnly)[k]);
                        this.TNm = new TreeNode(tree.getMonth());
                        TNy.Nodes.Add(TNm);

                        List<Notification> monthOnly = yearOnly.FindAll(x => x.getMonth() == this.TNm.Text);
                        for (int l = 0; l < NotificationList.diffNameListByTime(2, monthOnly).Count; l++)
                        {
                            tree = monthOnly.Find(x => x.Date.Day == NotificationList.diffNameListByTime(2, monthOnly)[l]);
                            this.TNd = new TreeNode(tree.getDay());
                            TNm.Nodes.Add(TNd);
                        }
                    }
                }
            }

            root.Sort();
        }
    }
}
