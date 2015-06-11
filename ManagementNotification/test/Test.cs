using ManagementNotification.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementNotification.test
{
    class Test
    {
       

        public Test()
        {
            NotificationList.ViewListToConsole();
            NotificationList.removeList(2);
            NotificationList.ViewListToConsole();
        }
    }
}
