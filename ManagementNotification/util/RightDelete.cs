using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementNotification.util
{
    class RightDelete
    {
        RemoveListByNode RLBN = new RemoveListByNode();

        /*
         * 選択された行の削除
         */
        public void deleteRow(DataGridView DGView1,int rowNum){
       
            DGView1.ClearSelection();
            DGView1.Rows[rowNum].Selected = true;

            //選択された行番号
            int selectRow = rowNum + 1;

            //メッセージボックスに選択した行を削除するか表示
            DialogResult result = MessageBox.Show("行番号:　" + selectRow + "\r\n選択した行を削除します。",
                                                    "削除",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Exclamation);

            if (result == DialogResult.OK)
            {

                //リストから選択された通知IDのデータを削除する
                int idNum = (int)DGView1["NotificationID", rowNum].Value;
                NotificationList.removeListByID(idNum);

                //DataGridViewから指定した通知IDのデータを削除する
                DGView1.Rows.RemoveAt(rowNum);

                //コンソールにリスト内容を表示する
                //NotificationList.ViewListToConsole();
            }
        }

        /*
         * 選択された項目のデータを削除
         */ 
        public void deleteNode(TreeView TView,DataGridView DGView,MouseEventArgs e)
        {
            //マウスの位置にあるノードを取得
            TView.SelectedNode = TView.GetNodeAt(e.X, e.Y);

            if (TView.SelectedNode != null)
            {

                //管理名,年,月,日
                String user, year, month, day, SsYear, SsMonth, SsDay;

                switch (TView.SelectedNode.Level)
                {
                    case 0:

                        //管理名を取得
                        user = TView.SelectedNode.Text;

                        DialogResult result = MessageBox.Show("管理名: " + user + "のデータを全て削除します。",
                                                              "確認", 
                                                               MessageBoxButtons.OKCancel,
                                                               MessageBoxIcon.Exclamation);

                        if (result == DialogResult.OK)
                        {
                            //リストから選択した管理名のデータをすべて削除
                            
                            RLBN.allRemoveListByNode(user,DGView);

                            /*
                             * TreeViewから管理名Nodeを削除
                             */   
                        }

                        break;

                    case 1:

                        //管理名、年を取得
                        user = TView.SelectedNode.Parent.Text;
                        year = TView.SelectedNode.Text;

                        //年の文字を削除する
                        SsYear = year.Substring(0, year.Length - 1);
                        
                        DialogResult result1 = MessageBox.Show("管理名: " + user + " " +  year +
                                                                            "のデータを全て削除します。",
                                                               "確認",
                                                               MessageBoxButtons.OKCancel,
                                                               MessageBoxIcon.Exclamation);
                        if (result1 == DialogResult.OK)
                        {
                            //リストから選択した日付のデータをすべて削除
                            RLBN.allRemoveListByNode(user, SsYear,DGView);

                            /*
                             * TreeViewから年Nodeを削除
                            */
                        }

                        break;

                    case 2:
                       
                        //管理名、年、月を取得
                        user = TView.SelectedNode.Parent.Parent.Text;
                        year = TView.SelectedNode.Parent.Text;
                        month = TView.SelectedNode.Text;

                        //年、月の文字を削除する
                        SsYear = year.Substring(0, year.Length - 1);
                        SsMonth = month.Substring(0, month.Length - 1);
                        

                        DialogResult result2 = MessageBox.Show("管理名: " + user + " " +  year + " " + month +
                                                                            "のデータを全て削除します。",
                                                               "確認",
                                                               MessageBoxButtons.OKCancel,
                                                               MessageBoxIcon.Exclamation);
                        if (result2 == DialogResult.OK)
                        {
                            //リストから選択した日付のデータをすべて削除
                            RLBN.allRemoveListByNode(user, SsYear, SsMonth,DGView);

                            /*
                             * TreeViewから年Nodeを削除
                            */
                        }

                        break;

                    case 3:

                        //管理名、年、月、日
                        user = TView.SelectedNode.Parent.Parent.Parent.Text;
                        year = TView.SelectedNode.Parent.Parent.Text;
                        month = TView.SelectedNode.Parent.Text;
                        day = TView.SelectedNode.Text;

                        //年、月、日の文字を削除する
                        SsYear = year.Substring(0, year.Length - 1);
                        SsMonth = month.Substring(0, month.Length - 1);
                        SsDay = day.Substring(0, day.Length - 1);

                        DialogResult result3 = MessageBox.Show("管理名: " + user + " " +  year + " " + month + " " + day +
                                                                            "のデータを全て削除します。",
                                                               "確認",
                                                               MessageBoxButtons.OKCancel,
                                                               MessageBoxIcon.Exclamation);
                        if (result3 == DialogResult.OK)
                        {
                            //リストから選択した日付のデータをすべて削除
                            RLBN.allRemoveListByNode(user, SsYear, SsMonth, SsDay,DGView);

                            /*
                             * TreeViewから年Nodeを削除
                            */
                        }

                        break;
                }

                NotificationList.ViewListToConsole();
            }
        }
    }
}
