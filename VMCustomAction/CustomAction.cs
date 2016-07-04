// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
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
            string[] lineArr = s.Split(new[] {"\r\n"}, StringSplitOptions.None);
            bool hasStaff = false;
            foreach (string str in lineArr)
            {
                string[] staffMac = str.Split(new[] {"\t"}, StringSplitOptions.None);
                if (staffMac.Length == 2 && staffMac[0] == staffNumber)
                {
                    hasStaff = true;
                    session["WIXUI_STAFFMACADDRESS"] = staffMac[1];
                }
            }
            session["PIDACCEPTED"] = hasStaff?"1": "0";
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult SetMacAddress(Session session)
        {
            session.Log("Begin GetMacAddressByStaffNumber");
            //MessageBox.Show("Test");
            string mac = session["WIXUI_STAFFMACADDRESS"];
            string installDir = session["_INSTALLFOLDER"];
            string fileName = "Windows 7 x64.vmx";
            string filePath = Path.Combine(installDir, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine("policySet.vmSettings.ethernet.0.macAddress = \"{0}\"", mac);
                    }
                }
                catch (Exception ex)
                {
                    session.Log("GetMacAddressByStaffNumber Exception:" + ex.Message);
                }
               
            }
            return ActionResult.Success;
        }
    }
}
