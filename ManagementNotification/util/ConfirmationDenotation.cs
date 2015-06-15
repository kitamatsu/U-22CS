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
        TreeView root;
        TreeNode TNcN, TNy, TNm, TNd;

        public ConfirmationDenotation(TreeView treeView)
        {
            root = treeView; 
        }


        public void DateDenotation()
        {
            root.Nodes.Add(NotificationList.list[0].ToString());

            foreach (Notification li in NotificationList.list)
            {

                //管理名、年月日のノードを作成
                this.TNcN = new TreeNode(li.ChildName.ToString());
                this.TNy = new TreeNode(li.Date.Year.ToString() + "年");
                this.TNm = new TreeNode(li.Date.Month.ToString() + "月");
                this.TNd = new TreeNode(li.Date.Day.ToString() + "日");
                
                //TreeViewにNodeを追加
                root.Nodes.Add(TNcN);
                TNcN.Nodes.Add(TNy);
                TNy.Nodes.Add(TNm);
                TNm.Nodes.Add(TNd);
            }
        }

        //リストからtreeを作成する
        public void SetNode()
        {

            foreach(List<Notification> listCN in differntNodeListByCN() )
            {
                for (int i = 0; i < listCN.Count;  i++)
                {
                    if (i == 0)
                    {
                        this.TNcN = new TreeNode(listCN[i].ChildName.ToString());
                        root.Nodes.Add(TNcN);
                        foreach(List<Notification> a in differntNodeListByTime(listCN, 0))
                        {

                        }


                    }
                    root.Nodes.Add(TNcN);
                }
            }
        }

        public void addNodeYear()
        {
            foreach(List<Notification> list in differntNodeListByCN())
            {
                for (int i = 0; i < list.Count;  i++)
                {
                    if (i == 0)
                    {
                        this.TNy = new TreeNode(list[i].Date.Year.ToString());
                        TNcN.Nodes.Add(TNy);
                    }
                    TNcN.Nodes.Add(TNy);
                }
            }
        }

        //表示名で分けた複数のListを持つListを返す
        public List<List<Notification>> differntNodeListByCN()
        {
            List<List<Notification>> diffList = new List<List<Notification>>();
       
                for(int i = 0; i < NotificationList.diffNameList().Count; i++)
                {
                    List<Notification> addList = new List<Notification>();
                    foreach(Notification li in NotificationList.list)
                    {
                        if(li.ChildName == NotificationList.diffNameList()[i])
                        {
                            addList.Add(li);
                        }
                    }
                    diffList.Add(addList);
                }
            return diffList;
        }

        //時間で分けた複数のListを持つListを返す
        //0:年 1:月 2:日 3:時間 4:秒
        public List<List<Notification>> differntNodeListByTime(List<Notification> parentList, int timeNum)
        {
            List<List<Notification>> diffList = new List<List<Notification>>();
;

            switch(timeNum)
            {
                case 0:
                    for (int i = 0; i < NotificationList.diffNameList().Count; i++)
                    {
                        List<Notification> addList = new List<Notification>();
                        foreach (Notification li in parentList)
                        {
                            if (li.Date.Year == NotificationList.diffNameListByTime(0)[i])
                            {
                                addList.Add(li);
                            }
                        }
                        diffList.Add(addList);
                    }
                    break;

                case 1:
                    for (int i = 0; i < NotificationList.diffNameList().Count; i++)
                    {
                        List<Notification> addList = new List<Notification>();
                        foreach (Notification li in parentList)
                        {
                            if (li.Date.Month == NotificationList.diffNameListByTime(1)[i])
                            {
                                addList.Add(li);
                            }
                        }
                        diffList.Add(addList);
                    }
                    break;

                case 2:
                    for (int i = 0; i < NotificationList.diffNameList().Count; i++)
                    {
                        List<Notification> addList = new List<Notification>();
                        foreach (Notification li in parentList)
                        {
                            if (li.Date.Day == NotificationList.diffNameListByTime(2)[i])
                            {
                                addList.Add(li);
                            }
                        }
                        diffList.Add(addList);
                    }
                    break;

                case 3:
                    for (int i = 0; i < NotificationList.diffNameList().Count; i++)
                    {
                        List<Notification> addList = new List<Notification>();
                        foreach (Notification li in parentList)
                        {
                            if (li.Date.Hour == NotificationList.diffNameListByTime(3)[i])
                            {
                                addList.Add(li);
                            }
                        }
                        diffList.Add(addList);
                    }
                    break;
                case 4:
                    for (int i = 0; i < NotificationList.diffNameList().Count; i++)
                    {
                        List<Notification> addList = new List<Notification>();
                        foreach (Notification li in parentList)
                        {
                            if (li.Date.Second == NotificationList.diffNameListByTime(4)[i])
                            {
                                addList.Add(li);
                            }
                        }
                        diffList.Add(addList);
                    }
                    break;

                default:
                    Console.WriteLine("differntNodeListByTime: 正しい引数を入力してください");
                    break;
            }         
            return diffList;
        }


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

        public void BodyDenotation(DataGridView DGView1 )
        {

        }

        public void selectLastNode(TreeView View1,DataGridView DGView1)
        {

            //DGView1.Rows.Add(1, "a", "a", "a");
            if (View1.SelectedNode.LastNode == null)
            {
                DGView1.Rows.Add(15,"a","a","a");
            }
        }
    }
}
