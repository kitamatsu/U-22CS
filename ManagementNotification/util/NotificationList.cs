using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementNotification.util
{
    //全ての通知をlistで所持,保存するクラス
    public class NotificationList
    {
        ArrayList list = new ArrayList();

        //constructor
        public NotificationList()
        {
            setList();
        }

        //本来はxmlからlistを復元するが、今はテスト用データを入力している
        private void setList()
        {
            //テストデータの入力
            Notification nt1 = new Notification(new DateTime(2014, 6, 6), "テスト通知1", "テスト通知1内容");
            Notification nt2 = new Notification(new DateTime(2014, 6, 5), "テスト通知2", "テスト通知2内容");
            list.Add(nt1);
            list.Add(nt2);
        }

        public void saveXML()
        {
            //保存先のファイル名
            string fileName = @"..\..\xml\list.xml";
            Type[] et = new Type[] { typeof(Notification) };

            //listをxmlに保存
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ArrayList), et);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, list);
            sw.Close();
        }
    }
}
