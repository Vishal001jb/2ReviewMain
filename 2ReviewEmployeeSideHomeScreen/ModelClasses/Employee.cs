﻿using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace _2ReviewEmployeeSideHomeScreen.ModelClasses
{
    public class Employee
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [Version]
        public string AzureVersion { get; set; }

        public string Employee_Credential_Id { get; set; }
        public string Employee_First_Name { get; set; }
        public string Employee_Middle_Name { get; set; }
        public string Employee_Last_Name { get; set; }
        public string Employee_Gender { get; set; }
        public string Employee_Mobile_No { get; set; }
        public string Employee_Image { get; set; }
        public string Employee_Email_Id { get; set; }
        public string Employee_Address { get; set; }
        public string Employee_City { get; set; }
        public string Employee_Pincode { get; set; }
        public string Employee_State { get; set; }
        public string Employee_Country { get; set; }
        public DateTime Employee_Joining_Date { get; set; }
    }
}