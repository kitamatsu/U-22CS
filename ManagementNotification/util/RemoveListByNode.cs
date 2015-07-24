using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementNotification.util
{
    class RemoveListByNode
    {
        //listの変数を作成
        List<Notification> RLBNlist = NotificationList.list;

        //指定した管理名のList内のデータを全て削除する
        public void allRemoveListByNode(String user, DataGridView DGview)
        {
            for (int index = 0; index < RLBNlist.Count; index++)
            {
                if (RLBNlist[index].ChildName == user)
                {
                    deleteDGViewByNode(DGview, index);

                    RLBNlist.Remove(RLBNlist[index]);
                }
            }
        }

        //指定した管理名の同じ年のList内のデータを全て削除する
        public void allRemoveListByNode(String user, String year, DataGridView DGview)
        {
            for (int index = 0; index < RLBNlist.Count; index++)
            {
                if (RLBNlist[index].ChildName == user && RLBNlist[index].Date.Year.ToString() == year)
                {
                    deleteDGViewByNode(DGview, index);

                    RLBNlist.Remove(RLBNlist[index]);

                }
            }
        }

        //指定した管理名の同じ月のList内のデータを全て削除する
        public void allRemoveListByNode(String user, String year, String month, DataGridView DGview)
        {
            for (int index = 0; index < RLBNlist.Count; index++)
            {
                if (RLBNlist[index].ChildName == user
                    && RLBNlist[index].Date.Year.ToString() == year
                    && RLBNlist[index].Date.Month.ToString() == month)
                {
                    deleteDGViewByNode(DGview, index);

                    RLBNlist.Remove(RLBNlist[index]);
                }
            }
        }

        //指定した管理名の同じ日のList内のデータを全て削除する
        public void allRemoveListByNode(String user, String year, String month, String day, DataGridView DGview)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }

            if (day.Length == 1)
            {
                day = "0" + day;
            }

            String strDate = year + "/" + month + "/" + day + " 0:00:00";

            
            for (int index = 0; index < RLBNlist.Count; index++)
            {
                if (RLBNlist[index].ChildName == user && RLBNlist[index].Date.Date.ToString() == strDate)
                {
                    deleteDGViewByNode(DGview, RLBNlist[index].NotificationID);

                    RLBNlist.Remove(RLBNlist[index]);

                    index--;
                }
            }

            //DGview.Rows.Clear();
        }

        //DataGridView上のTreeViewで選択された項目と同じ内容を削除
        public void deleteDGViewByNode(DataGridView DGView, int id)
        {
            for (int num = 0; num < DGView.RowCount; num++)
            {
                if (id == (int)DGView["NotificationId", num].Value)
                {
                    DGView.Rows.RemoveAt(num);
                }
            }
        }   
    }
}
