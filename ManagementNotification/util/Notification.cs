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
        private DateTime date{ get; set; } //日付
        private String title { get; set; }  //タイトル
        private String body { get; set; }   //本文

        //引数なしConstructor
        public Notification()
        {
          //シリアライズ?
        }

        //Constructor
        public Notification(DateTime date, String title, String body)
        {
            this.date = date;
            this.title = title;
            this.body = body;
        }

        //getter, setter
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
    }
}
