using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementNotification.util
{
    //通知を所持するクラス
    public class Notification
    {
        private int notificationID { get; set; } //通知ID
        private DateTime date{ get; set; } //日付
        private String title { get; set; }  //タイトル
        private String body { get; set; }   //本文
        private String childName { get; set; }  //子の管理名

        //引数なしConstructor
        public Notification()
        {
          //シリアライズ?
        }

        //Constructor
        public Notification(int id, DateTime date, String title, String body, String cn)
        {
            this.Date = date;
            this.Title = title;
            this.Body = body;
            this.NotificationID = id;
            this.ChildName = cn;
        }

        //getter, setter

        public int NotificationID
        {
            get { return notificationID; }
            set { notificationID = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Body
        {
            get { return body; }
            set { body = value; }
        }

        public String ChildName
        {
            get { return childName; }
            set { childName = value; }
        }

        public override string ToString()
        {
            return NotificationID + "-" + Date + "-" + Title + "-" + Body + "-" + ChildName;
        }

        public String getYear()
        {
            return this.Date.Year.ToString() + "年";
        }

        public String getMonth()
        {
            return this.Date.Month.ToString() + "月";
        }

        public String getDay()
        {
            return this.Date.Day.ToString() + "日";
        }
    }
}
