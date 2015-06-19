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

        //兄弟nodeのチェック
        public Boolean isNextNode(TreeNode node)
        {
            bool flag = false;

            if (node.NextNode != null)
            {
                flag = true;
            }

            return flag;
        }



        /*
         *選択された日付と一致するデータを取得 
         */
        public void selectLastNode(TreeView TView1, DataGridView DGView1, MouseEventArgs e)
        {
            //マウスの位置にあるノードを取得
            TView1.SelectedNode = TView1.GetNodeAt(e.X, e.Y);

            if (TView1.SelectedNode != null)
            {
                if (TView1.SelectedNode.LastNode == null)
                {

                    //DataGridViewに保存内容が一行以上あれば削除
                    if (DGView1.RowCount >= 1)
                    {
                        DGView1.Rows.Clear();
                    }

                    //ノードの文字を取得(管理者名、年、月、日)
                    String user = TView1.SelectedNode.Parent.Parent.Parent.Text;
                    String year = TView1.SelectedNode.Parent.Parent.Text;
                    String month = TView1.SelectedNode.Parent.Text;
                    String day = TView1.SelectedNode.Text;

                    //年、月、日の文字を削除
                    String SsYear = year.Substring(0, 4);
                    String SsMonth = month.Substring(0, month.Length - 1);
                    String SsDay = day.Substring(0, day.Length - 1);

                    //月、日が一文字の場合0を付ける
                    if (SsMonth.Length == 1)
                    {
                        SsMonth = "0" + SsMonth;
                    }

                    if (SsDay.Length == 1)
                    {
                        SsDay = "0" + SsDay;
                    }

                    /*
                     * 検索用の文字列(年+月+日)を作成
                     * 
                     */
                    String date = SsYear + "/" + SsMonth + "/" + SsDay + " 0:00:00";

                    //選択された日付と一致する保存内容を検索、表示する
                    foreach (Notification li in NotificationList.list)
                    {
                        if (li.ChildName == user && li.Date.Date.ToString() == date)
                        {
                            DGView1.Rows.Add(li.NotificationID, li.Date.TimeOfDay, li.Title, li.Body);
                        }
                    }
                }
            }
        }


    }
}
