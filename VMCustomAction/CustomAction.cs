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

            foreach (Record fileRec in fileView)
            {
                session.Log("\t{0}", fileRec["FileName"]);
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
