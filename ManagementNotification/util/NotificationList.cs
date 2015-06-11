using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementNotification.util
{
    //全ての通知をlistで所持,保存するクラス
    public static class NotificationList
    {
        public static List<Notification> list = new List<Notification>();

        static NotificationList()
        {       
            System.Console.WriteLine("静的コンストラクタ");
            loadList();
        }

        static public void setList()
        {
            //jsonからlistへセットする予定
        }

        //xmlからListに復元する
        static public void loadList()
        {
            //保存元のファイル名    デバッグ先から階層を指定している
            string fileName = @"..\..\xml\list.xml";

            //XmlSerializerオブジェクトを作成
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Notification>));
            //読み込むファイルを開く
            System.IO.StreamReader sr = new System.IO.StreamReader(
                fileName, new System.Text.UTF8Encoding(false));
            //XMLファイルから読み込み、逆シリアル化する
            list = (List<Notification>)serializer.Deserialize(sr);
            //ファイルを閉じる
            sr.Close();
        }

        //ArrayListをxmlで保存するメソッド
        static public void saveXML()
        {
            //保存先のファイル名
            string fileName = @"..\..\xml\list.xml";
            Type[] et = new Type[] { typeof(Notification) };

            //listをxmlに保存
            System.Xml.Serialization.XmlSerializer serializer = 
                new System.Xml.Serialization.XmlSerializer(typeof(List<Notification>), et);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, list);
            sw.Close();
        }

        //テストデータの追加
        static public void testAdd()
        {
            //テストデータの入力
            Notification nt1 = new Notification(1, new DateTime(2014, 6, 6), "テスト通知1", "テスト通知1内容", "兄");
            Notification nt2 = new Notification(2, new DateTime(2014, 6, 5), "テスト通知2", "テスト通知2内容", "弟");
            list.Add(nt1);
            list.Add(nt2);
        }

        //Listをコンソールに表示する
        static public void ViewListToConsole()
        {
            foreach(Notification li in list)
            {
                Console.WriteLine(li.ToString());
            }
        }

        public void deleteNotification(int selectId, Boolean deletePast)
        {
            
            //list.Remove
        }
    }
}
