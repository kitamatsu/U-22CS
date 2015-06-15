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
                //DataGridViewから指定した通知IDのデータを削除する
                DGView1.Rows.RemoveAt(rowNum);

                //リストから選択された通知IDのデータを削除する
                int idNum = (int)DGView1["NotificationID", rowNum].Value;
                NotificationList.removeListByID(idNum);

                //コンソールにリスト内容を表示する
                NotificationList.ViewListToConsole();
            }

            /*ヘッダ以外のセルか？
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {

                // 右クリックされたセル
                //DataGridViewCell cell = dataGridView1[e.ColumnIndex, e.RowIndex];
                // セルの選択状態を反転
                //cell.Selected = !cell.Selected;
            }
             */
        }
    }
}
