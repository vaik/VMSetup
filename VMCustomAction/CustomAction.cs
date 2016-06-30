// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;

namespace VMCustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult StaffNumberValidate(Session session)
        {
            session.Log("Begin StaffNumberValidate");
            MessageBox.Show("Test");
            var staffNumber = session["StaffNumber"];
            session.Log(staffNumber);
            var fileView = session.Database.OpenView("SELECT * FROM Binary WHERE Name = 'StaffNumbertxt'");
            fileView.Execute(null);

            Record r = fileView.Fetch();
            int datalen = r.GetDataSize("Data");
            System.IO.Stream strm = r.GetStream("Data");
            byte[] rawData = new byte[datalen];
            int res = strm.Read(rawData, 0, datalen);
            strm.Close();

            String s = System.Text.Encoding.UTF8.GetString(rawData);
            if (s.Length > 0 && s[0] == '\uFEFF')
            {
                s = s.Substring(1);
            }

            session["PIDACCEPTED"] = "0";
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult GetMacAddressByStaffNumber(Session session)
        {
            session.Log("Begin GetMacAddressByStaffNumber");

            return ActionResult.Success;
        }
    }
}
