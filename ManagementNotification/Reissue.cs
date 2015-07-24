using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagementNotification.db;
using ManagementNotification.util;

namespace ManagementNotification
{
    public partial class Reissue : Form
    {
        AccountCertification AC;
        String systemEmail = "kj4managementnotification@gmail.com"; //システムのGmailアドレスとパスワード
        String systemPassword = "sp8z5n49";

        public Reissue()
        {
            InitializeComponent();
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            String email = "";
            DB db = new DB();
            email = db.CheckEmailConnectAndQuery(textBox1.Text);

            if (email.Equals(""))
            {
                MessageBox.Show("入力されたメールアドレスは登録されていません", "エラー",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                String[] accountData = db.ReissueConnectAndQuery(email);

                sendGmail(email, accountData);
                
                MessageBox.Show("入力されたメールアドレスに再発行後のアカウント情報を送信しました。",
                                "再発行の完了",　MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                AC = new AccountCertification();
                AC.Show();
                this.Visible = false;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            AC = new AccountCertification();
            AC.Show();
            this.Visible = false;
            
        }

        //Gmailでメールを送信する
        private void sendGmail(String email, String[] accountData)
        {
            

            //SSL3.0しか使わないようにする
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Ssl3;

            //MailMessageの作成
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(
                systemEmail, email,
                "ManagementNotification：アカウントの再発行", "仮のユーザネーム：" + accountData[0] +
                "\r\n仮のパスワード：" + accountData[1]);

            System.Net.Mail.SmtpClient sc = new System.Net.Mail.SmtpClient();
            //SMTPサーバーなどを設定する
            sc.Host = "smtp.gmail.com";
            sc.Port = 587;
            sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //ユーザー名とパスワードを設定する
            sc.Credentials = new System.Net.NetworkCredential(systemEmail, systemPassword);
            //SSLを使用する
            sc.EnableSsl = true;
            //メッセージを送信する
            sc.Send(msg);

            //後始末
            msg.Dispose();
            //後始末（.NET Framework 4.0以降）
            sc.Dispose();


        }

        private void Reissue_FormClosed(object sender, FormClosedEventArgs e)
        {
            NotificationList.saveXML();
            Application.Exit();
        }

    }
}
