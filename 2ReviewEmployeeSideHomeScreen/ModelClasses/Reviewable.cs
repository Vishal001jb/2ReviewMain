using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace _2ReviewEmployeeSideHomeScreen.ModelClasses
{
    public class Reviewable
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [Version]
        public string AzureVersion { get; set; }

        public string Employee_Id { get; set; }
        public string Designation_Id { get; set; }
        public string Round_Id { get; set; }
        public string Status { get; set; }
        public int Total { get; set; }

    }
}