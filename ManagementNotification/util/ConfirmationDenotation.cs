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
        static public void DateDenotation(TreeView TView1)
        {
            //String childName = NotificationList.list[0].ToString();
            //TView1.Nodes.Add(childName);

            foreach (Notification li in NotificationList.list)
            {
                //管理名を取り出す
                TreeNode childName = new TreeNode(li.ChildName.ToString());
                TView1.Nodes.Add(childName);

                //年を取り出す
                TreeNode year = new TreeNode(li.Date.Year.ToString() + "年");
                childName.Nodes.Add(year);

                //月を取り出す
                TreeNode month = new TreeNode(li.Date.Month.ToString() + "月");
                year.Nodes.Add(month);

                //日付を取り出す
                String day = li.Date.Day.ToString() + "日";
                month.Nodes.Add(day);
            }
        }

        static public void BodyDenotation(DataGridView DGView1 )
        {

        }
    }
}
