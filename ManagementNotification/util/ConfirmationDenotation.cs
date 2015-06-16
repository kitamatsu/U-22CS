using System;
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
        TreeNode root;
        TreeNode TNcN, TNy, TNm, TNd;

        public void DateDenotation(TreeView TView1)
        {
            //TView1.Nodes.Add(NotificationList.list[0].ToString());

            foreach (Notification li in NotificationList.list)
            {

                //管理名、年月日のノードを作成
                this.TNcN = new TreeNode(li.ChildName.ToString());
                this.TNy = new TreeNode(li.Date.Year.ToString() + "年");
                this.TNm = new TreeNode(li.Date.Month.ToString() + "月");
                this.TNd = new TreeNode(li.Date.Day.ToString() + "日");
                
                //TreeViewにNodeを追加
                TView1.Nodes.Add(TNcN);
                TNcN.Nodes.Add(TNy);
                TNy.Nodes.Add(TNm);
                TNm.Nodes.Add(TNd);
            }
        }

        //リストからtreeを作成する
        public TreeNode Node()
        {
            root = new TreeNode();

            foreach(Notification li in NotificationList.list){
         
                if(true)
                {
                     if (root.Parent == null)
                     {
                    
                     }
                    else
                    {
                    //追加
                    }
                }
               

            }            

            return null;
        }

        /**表示名で分けた複数のListを持つList
        public List<List<Notification>> differntNodeList()
        {
            List<List<Notification>> diffList = new List<List<Notification>>();

            foreach(Notification li )

            return diffList;

        }
        **/

        //兄弟nodeのチェック
        public Boolean isNextNode(TreeNode node)
        {
            bool flag = false;

            if(node.NextNode != null)
            {
                flag = true;
            }

            return flag;
        }



        /*
         *選択された日付と一致するデータを取得 
         */
        public void selectLastNode(TreeView View1,DataGridView DGView1)
        {

            if (View1.SelectedNode.LastNode == null)
            {
                //ノードの文字を取得(管理者名、年、月、日)
                String user = View1.SelectedNode.Parent.Parent.Parent.Text;
                String year = View1.SelectedNode.Parent.Parent.Text;
                String month = View1.SelectedNode.Parent.Text;
                String day = View1.SelectedNode.Text;

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
                String date = SsYear +"/"+ SsMonth + "/" + SsDay + " 0:00:00";

                //選択された日付と一致するデータを検索、表示する
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
