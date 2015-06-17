﻿using System;
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

            testAdd();
            //loadList();
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

            if (System.IO.File.Exists(fileName))
            {
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
            else
            {
                Console.WriteLine("xmlが存在しません。");
            }

           
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
            //listclear
            list.Clear();

            //テストデータの入力
            Notification nt1 = new Notification(1, new DateTime(2014, 6, 6, 5, 30, 30), "テスト通知1", "テスト通知1内容", "兄");
            Notification nt2 = new Notification(2, new DateTime(2014, 6, 5, 11, 29, 45), "テスト通知2", "テスト通知2内容", "弟");
            Notification nt3 = new Notification(3, new DateTime(2014, 6, 16, 21, 22, 30), "テスト通知3", "テスト通知3内容", "兄");
            Notification nt4 = new Notification(15, new DateTime(2015, 3, 20, 17, 10, 30), "テスト通知15", "テスト通知15内容", "弟");
            list.Add(nt1);
            list.Add(nt2);
            list.Add(nt3);
            list.Add(nt4);
        }

        //Listをコンソールに表示する
        static public void ViewListToConsole()
        {
            foreach(Notification li in list)
            {
                Console.WriteLine(li.ToString());
            }
        }


        //指定したIDの通知を削除する
        static public void removeListByID(int[] id)
        {
            for (int i = 0; id.Length > i; i++)
            {
                foreach (Notification li in list)
                {
                    if (li.NotificationID == id[i])
                    {
                        list.Remove(li);
                        break;
                    }
                }
            }
                
        }

        //指定したIDの通知を削除する
        static public void removeListByID(int id)
        {
                foreach (Notification li in list)
                {
                    if (li.NotificationID == id)
                    {
                        list.Remove(li);
                        return;
                    }
                }
        }

        //指定した表示名のlistを全て削除する
        static public void allRemoveListByChildName(String cn)
        {
            foreach (Notification li in list)
            {
                if (li.ChildName == cn)
                {
                    list.Remove(li);
                }
            }
        }

        //指定した管理名のListを全て削除する
        static public void allRemoveListByNode(String user)
        {
            for (int index = 0; index < list.Count; index++ )
            {
                if (list[index].ChildName == user)
                {
                    list.Remove(list[index]);
                }
            }
        }

        //指定した管理名の同じ年のListを全て削除する
        static public void allRemoveListByNode(String user, String year)
        {
            for (int index = 0; index < list.Count; index++ )
            {
                if (list[index].ChildName == user && list[index].Date.Year.ToString() == year)
                {
                    list.Remove(list[index]);

                }
            }
        }

        //指定した管理名の同じ月のListを全て削除する
        static public void allRemoveListByNode(String user, String year, String month)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (list[index].ChildName == user
                    && list[index].Date.Year.ToString() == year
                    && list[index].Date.Month.ToString() == month)
                {
                    list.Remove(list[index]);
                }
            }
        }

        //指定した管理名の同じ日のListを全て削除する
        static public void allRemoveListByNode(String user, String year, String month, String day)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }

            if (day.Length == 1)
            {
                day = "0" + day;
            }

            String date = year + "/" + month + "/" + day + " 0:00:00";

            for (int index = 0; index < list.Count; index++ )
            {
                if (list[index].ChildName == user && list[index].Date.Date.ToString() == date)
                {
                    list.Remove(list[index]);
                }
            }
        }


        //表示名のリストを返す
        //タブりを消す
        static public List<String>  diffNameList()
        {
            List<String> strList = new List<string>();

            foreach (Notification li in NotificationList.list)
            {
                strList.Add(li.ChildName);
            }

            System.Collections.Generic.HashSet<String> hs = new HashSet<string>(strList);
            List<String> result = new List<String>(hs);

            //Console.WriteLine("diffNameList" + result.Count);
            return result;
        }

        //時間のリストを返す
        //ダブリを消す
        //0:年 1:月 2:日 3:時 4:秒
        static public List<int> diffNameListByTime(int timeNum)
        {
            List<int> timeList = new List<int>();

            switch(timeNum)
            {
                case 0:
                    foreach (Notification li in NotificationList.list)
                    {    
                        timeList.Add(li.Date.Year);
                    }          
                    break;
                case 1:
                    foreach (Notification li in NotificationList.list)
                    {
                        timeList.Add(li.Date.Month);
                    } 
                    break;
                case 2:
                    foreach (Notification li in NotificationList.list)
                    {
                        timeList.Add(li.Date.Day);
                    } 
                    break;
                case 3:
                    foreach (Notification li in NotificationList.list)
                    {
                        timeList.Add(li.Date.Hour);
                    } 
                    break;
                case 4:
                    foreach (Notification li in NotificationList.list)
                    {
                        timeList.Add(li.Date.Second);
                    }
                    break;
                default:
                    Console.WriteLine("diffNameListByTime: 正しい引数を入力してください");
                    break;
            }

            System.Collections.Generic.HashSet<int> hs = new HashSet<int>(timeList);
            List<int> result = new List<int>(hs);       
                     
            //Console.WriteLine("diffNameList" + result.Count);
            return result;
        }
        
    }
}
