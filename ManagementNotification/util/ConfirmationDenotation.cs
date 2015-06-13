﻿using System;
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
        TreeNode TNcN, TNy, TNm, TNd;

        public void DateDenotation(TreeView TView1)
        {
            //String childName = NotificationList.list[0].ToString();
            //TView1.Nodes.Add(childName);

            TView1.Nodes.Add(NotificationList.list[0].ToString());

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

        public void BodyDenotation(DataGridView DGView1 )
        {
            
        }

        public void selectLastNode(TreeView View1,DataGridView DGView1)
        {

            //DGView1.Rows.Add(1, "a", "a", "a");
            if (View1.SelectedNode.LastNode == null)
            {
                DGView1.Rows.Add(1,"a","a","a");
            }
        }
    }
}
