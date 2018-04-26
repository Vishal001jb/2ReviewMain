﻿#define OFFLINE_SYNC_ENABLED
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using _2ReviewEmployeeSideHomeScreen.ModelClasses;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
//using Microsoft.Framework.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Net;
//using LogLevel = Microsoft.Framework.Logging.LogLevel;

namespace _2ReviewEmployeeSideHomeScreen.Service
{
    public class AzureDataService
    {
        public MobileServiceClient MobileService { get; set; }

        IMobileServiceSyncTable<Question> QuesTable;
        IMobileServiceSyncTable<Answer> AnswerTable;
        IMobileServiceSyncTable<Assignment> AssignTable;
        IMobileServiceSyncTable<Designation> DesigTable;
        IMobileServiceSyncTable<Employee> EmpTable;
        IMobileServiceSyncTable<EmployeeCredential> EmpCredentialTable;
        IMobileServiceSyncTable<EmployeeDesignation> EmpDesigTable;
        IMobileServiceSyncTable<Employee_Perfomance> EmpPerformanceTable;
        IMobileServiceSyncTable<Employee_Points> EmpPointsTable;
        IMobileServiceSyncTable<Form> FormTable;
        IMobileServiceSyncTable<Form_Question> FormQuesTable;
        IMobileServiceSyncTable<QuestionDesignation> QuesDesigTable;
        IMobileServiceSyncTable<Reviewable> ReviewableTable;
        IMobileServiceSyncTable<Reviewee> RevieweeTable;
        IMobileServiceSyncTable<Round> RoundTable;
        IMobileServiceTable<Employee_Perfomance> EPT;
        static string DesignationId;
        static List<string> Assignment_Id;
        static List<string> Form_Question_Id;
        static List<string> Round_Id;
        List<Employee_Perfomance> EP;
        List<List<KeyValuePair<string, object>>> mEmpRoundDetails;
        public async Task<bool> Initialize()
        {
            //EP = new List<Employee_Perfomance>();
            mEmpRoundDetails = new List<List<KeyValuePair<string, object>>>();
            Assignment_Id = new List<string>();
            Form_Question_Id = new List<string>();
            Round_Id = new List<string>();
            //Get our sync table that will call out to azure
            MobileService = new MobileServiceClient("https://2review.azurewebsites.net");

            //setup our local sqlite store and intialize our table
            const string path = "2Review.db";
            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<Question>();
            store.DefineTable<Answer>();
            store.DefineTable<Assignment>();
            store.DefineTable<Designation>();
            store.DefineTable<Employee>();
            store.DefineTable<EmployeeCredential>();
            store.DefineTable<EmployeeDesignation>();
            store.DefineTable<Employee_Perfomance>();
            store.DefineTable<Employee_Points>();
            store.DefineTable<Form>();
            store.DefineTable<Form_Question>();
            store.DefineTable<QuestionDesignation>();
            store.DefineTable<Reviewable>();
            store.DefineTable<Reviewee>();
            store.DefineTable<Round>();

            Debug.WriteLine("Pending operations in the sync context queue: {0}", MobileService.SyncContext.PendingOperations);

            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            QuesTable = MobileService.GetSyncTable<Question>();
            AnswerTable = MobileService.GetSyncTable<Answer>();
            AssignTable = MobileService.GetSyncTable<Assignment>();
            DesigTable = MobileService.GetSyncTable<Designation>();
            EmpTable = MobileService.GetSyncTable<Employee>();
            EmpCredentialTable = MobileService.GetSyncTable<EmployeeCredential>();
            EmpDesigTable = MobileService.GetSyncTable<EmployeeDesignation>();
            EmpPerformanceTable = MobileService.GetSyncTable<Employee_Perfomance>();
            EmpPointsTable = MobileService.GetSyncTable<Employee_Points>();
            FormTable = MobileService.GetSyncTable<Form>();
            FormQuesTable = MobileService.GetSyncTable<Form_Question>();
            QuesDesigTable = MobileService.GetSyncTable<QuestionDesignation>();
            ReviewableTable = MobileService.GetSyncTable<Reviewable>();
            RevieweeTable = MobileService.GetSyncTable<Reviewee>();
            RoundTable = MobileService.GetSyncTable<Round>();

            Sync();
            //MobileService.SyncContext.PushAsync();

            //var remoteTable = MobileService.GetTable<Employee_Perfomance>();

            //var remoteItems = await remoteTable.ToListAsync();
            //Debug.WriteLine("Items from the server: {0}", string.Join(", ", remoteItems));

            //var Assign = new Assignment();
            //Assign.Form_Id = "33749429419b4c7f98be434a11d08040";
            //Assign.Reviewable_Id = "10863f130d5649cc94f1954f8c10fd4f";
            //Assign.Reviewee_Id = "1032c63b686d44bc909dc02d68d2d5d3";
            //Assign.Round_Id = "4d00bed855d149b0a4a4f24a561747b5";

            //AssignTable.InsertAsync(Assign);

            //var fq = new Form_Question();
            //fq.Form_Id = "33749429419b4c7f98be434a11d08040";
            //fq.Question_Id = "48a7ec8e91df43c1b24c27bd2df9976f";

            //var EP = new Employee_Perfomance();
            //EP.Reviewable_Id = "faadafd2091247f8ae514b0a7d38a043";
            //EP.Progress = 3;
            //EP.Round_Id = "70f588eb88b34151998479dcbfcb1178";
            //EP.Form_Id = "33749429419b4c7f98be434a11d08040";
            //EP.Ranking = 19;
            //EmpPerformanceTable.InsertAsync(EP);

            //Debug.WriteLine("Inserted into local store: {0}", EP.Progress);

            //var emp = new Employee();
            //emp.EmployeeCredential_Id = "1";
            //emp.Employee_First_Name = "Vishal";
            //emp.Employee_Last_Name = "Barot";
            //emp.Employee_Joining_Date = DateTime.UtcNow;

            //EmpTable.InsertAsync(emp);

            //Debug.WriteLine(@"Inserted into local store: {0}", emp.Id);

            //Sync();
            //MobileService.SyncContext.PushAsync();
            //Debug.WriteLine("Pushed the local changes to the server");

            //var remoteItems = await remoteTable.Select(ep => ep.Progress).ToListAsync();
            //Debug.WriteLine(@"Items from the server: {0}", string.Join(", ", remoteItems));

            ////Debug.WriteLine("Inserted into local store : ", EP.Id);
            ////var thingsToDo = await EmpPerformanceTable.Select(E => E.Id).ToListAsync();
            //////Debug.WriteLine("Items from Local {0}", string.Join(", ", thingsToDo));

            ////var remoteTable = MobileService.GetTable<EmployeePerfomance>();

            ////var remoteItems = remoteTable.Select(e => e.Id).ToListAsync();

            ////Debug.WriteLine("Items From Server {0}", string.Join(", ", remoteItems));

            return true;
        }

        public async Task<List<Employee_Perfomance>> GetEmpRoundWisePerformance()
        {
            Sync();
            var result = await EmpPerformanceTable.ToListAsync();
            return result;
        }

        public async Task<List<string>> GetRoundName(string EmpId)
        {
            List<string> RoundName = new List<string>();
            try
            {
                Round_Id = await ReviewableTable.Where(r => r.Employee_Id == EmpId).Select(r => r.Round_Id).ToListAsync();
                Debug.WriteLine("Round Id from Reviewable table : ", string.Join(", ", Round_Id));
                for (int i = 0; i < Round_Id.Count; i++)
                {
                    var result = await RoundTable.Where(r => r.Id == Round_Id[i]).Select(r => r.Round_Name).ToListAsync();
                    RoundName.Add(result[0]);
                    Debug.WriteLine(@"Round Name from Round table {0}", string.Join(", ", result));
                }
                    return RoundName;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error ahya che bhai : ", e.Message);
            }
            return null;
        }

        public async Task<List<DateTime>> GetRoundDate(string EmpId)
        {
            List<DateTime> RoundDate = new List<DateTime>();
            var RoundId = await ReviewableTable.Where(r => r.Employee_Id == EmpId).Select(r => r.Round_Id).ToListAsync();
            Debug.WriteLine(@"Round Id from Reviewable table {0}", string.Join(", ", RoundId));
            for (int i = 0; i < RoundId.Count; i++)
            {
                var result = await RoundTable.Where(r => r.Id == RoundId[i]).Select(r => r.CreatedAt).ToListAsync();
                RoundDate.Add(result[0]);
                Debug.WriteLine(@"Round Date from Round table {0}", string.Join(", ", result));
            }
                return RoundDate;
        }

        public async void getQuestionText(string EmpId , string RoundId)
        {
            List<string> Qtext = new List<string>();
            try
            {
                var rid = await ReviewableTable.OrderByDescending(r => r.CreatedAt).Where(e => e.Employee_Id == EmpId && e.Round_Id == RoundId).Select(r => r.Id).ToListAsync();
                Debug.WriteLine(@"getQuestionText ReviewableId {0}", string.Join(", ", rid));

                //var RoundId = await ReviewableTable.Where(r => r.Employee_Id == EmpId).Select(r => r.Round_Id).ToListAsync();
                //Debug.WriteLine(@"getQuestionText Round Id {0}", string.Join(", ", rid));
                var fid = await AssignTable.Where(a => a.Reviewable_Id == rid[0]).Select(a => a.Form_Id).ToListAsync();
                Debug.WriteLine(@"getQuestionText Form Id {0}", string.Join(", ", fid));
                Assignment_Id = await AssignTable.Where(a => a.Reviewable_Id == rid[0]).Select(a => a.Id).ToListAsync();
                Debug.WriteLine(@"getQuestionText Assignment Id {0}", string.Join(", ", Assignment_Id));
                var qid = await FormQuesTable.Where(fq => fq.Form_Id == fid[0]).Select(fq => fq.Question_Id).ToListAsync();
                Debug.WriteLine(@"getQuestionText Question Id {0}", string.Join(", ", qid));
                Form_Question_Id = await FormQuesTable.Where(fq => fq.Form_Id == fid[0]).Select(fq => fq.Id).ToListAsync();
                Debug.WriteLine(@"getQuestionText Form_Question_Id {0}", string.Join(", ", Form_Question_Id));

                List<Task<List<string>>> tasks = new List<Task<List<string>>>();

                for (int i = 0; i < qid.Count; i++)
                {
                    Debug.WriteLine(@"getQuestionText Question_Id in Loop {0}", string.Join(", ", qid[i]));
                    tasks.Add(QuesTable.Where(q => q.Id == qid[i]).Select(q => q.Question_Text).ToListAsync());
                }
                var res = await Task.WhenAll(tasks);

                for (int i = 0; i < res.Length; i++)
                {
                    var qtext = res[i];
                    Debug.WriteLine(@"getQuestionText Question Text from Azure {0}", string.Join(", ", qtext));
                    Qtext.Add(qtext[0]);
                    Debug.WriteLine(@"getQuestionText Question Text {0}", string.Join(", ", Qtext));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Exception {0}", e.Message);
            }
        }


        public async void getQuestionWiseRating()
        {
            List<int> total = new List<int>();
            float rate = 0;
            for (int i = 0; i < Assignment_Id.Count; i++)
            {
                int indiviualTotal = 0;
                for (int j=0;i<Form_Question_Id.Count;j++)
                {
                    var ans = await AnswerTable.Where(a => a.Assignment_Id == Assignment_Id[i] && a.Form_Question_Id == Form_Question_Id[0]).Select(a => a.Answers).ToListAsync();
                    indiviualTotal = indiviualTotal+ans[0];
                }
                total.Add(indiviualTotal);
            }

            for (int i=0;i<total.Count;i++)
            {
                rate = rate + total[i]/4;
            }

            //return (rate / total.Count) + "";

        }

        public async Task<string> getEmployeeName(string EmpId)
        {
            var FirstName = await EmpTable.Where(e => e.Id == EmpId).Select(e => e.Employee_First_Name).ToListAsync();
            Debug.WriteLine(@"Employee Name {0}", string.Join(", ", FirstName));
            var LastName = await EmpTable.Where(e => e.Id == EmpId).Select(e => e.Employee_Last_Name).ToListAsync();
            return FirstName[0] + " " + LastName[0];
        }

        public async Task<string> getEmployeeDesination(string EmpId)
        {
            var Did = await EmpDesigTable.Where(ed => ed.Id == EmpId).Select(ed => ed.Designation_Id).ToListAsync();
            DesignationId = Did[0];
            Debug.WriteLine(@"Designation Id {0}", string.Join(", ", Did));
            var designation = await DesigTable.Where(d => d.Id == Did[0]).Select(d => d.DesignationName).ToListAsync();
            Debug.WriteLine(@"Designation {0}", string.Join(", ", designation));
            return designation[0];
        }

        public async Task<string> getEmployeeRanking(string EmpId)
        {
            var rid = await ReviewableTable.OrderByDescending(r => r.CreatedAt).Where(e => e.Employee_Id == EmpId).Select(r => r.Id).ToListAsync();
            Debug.WriteLine(@"Reviewable id {0}", string.Join(", ", rid));
            var rank = await EmpPerformanceTable.Where(p => p.Reviewable_Id == rid[0]).Select(p => p.Ranking).ToListAsync();
            Debug.WriteLine(@"Rank {0}", string.Join(", ", rank));
            return rank[0]+"";
        }

        public async Task<List<int>> getRoundProgress(string EmpId)
        {
            List<int> progressList = new List<int>();

            var revId = await ReviewableTable.Where(r => r.Employee_Id == EmpId).Select(r => r.Id).ToListAsync();
            Debug.WriteLine(@"Reviewable Id from Reviewable table {0}", string.Join(", ", revId));

            for (int i = 0; i < revId.Count; i++)
            {
                var progress = await EmpPerformanceTable.Where(ep => ep.Reviewable_Id == revId[i]).Select(e => e.Progress).ToListAsync();
                progressList.Add(progress[0]);
                Debug.WriteLine(@"Progress from Employee Performance table {0}", string.Join(", ", progress));
            }
                return progressList;
        }

        public async void Sync()
        {


            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                // Use a different query name for each unique query in your program.

                await EmpPointsTable.PullAsync("allEmployeePoints", EmpPointsTable.CreateQuery());
                await AnswerTable.PullAsync("allAnswer", AnswerTable.CreateQuery());
                await AssignTable.PullAsync("allAssignment", AssignTable.CreateQuery());
                await DesigTable.PullAsync("allDesignation", DesigTable.CreateQuery());
                await EmpTable.PullAsync("allEmployee", EmpTable.CreateQuery());
                await EmpCredentialTable.PullAsync("allEmployeeCredentials", EmpCredentialTable.CreateQuery());
                await EmpDesigTable.PullAsync("allEmployeeDesignation", EmpDesigTable.CreateQuery());
                await EmpPerformanceTable.PullAsync("allEmployeePerformance", EmpPerformanceTable.CreateQuery());
                await FormTable.PullAsync("allForms", FormTable.CreateQuery());
                await FormQuesTable.PullAsync("allFormQuestion", FormQuesTable.CreateQuery());
                await QuesTable.PullAsync("allQuestion", QuesTable.CreateQuery());
                await QuesDesigTable.PullAsync("allQuestionDesignation", QuesDesigTable.CreateQuery());
                await ReviewableTable.PullAsync("allReviewable", ReviewableTable.CreateQuery());
                await RevieweeTable.PullAsync("allReviewee", RevieweeTable.CreateQuery());
                await RoundTable.PullAsync("allRound", RoundTable.CreateQuery());
                await MobileService.SyncContext.PushAsync();
            }
            catch (MobileServicePushFailedException exc)
            {

                Debug.WriteLine("Error Message : "+exc.Message);

                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                    //Simple error/ conflict handling.
                    if (syncErrors != null)
                    {

                        foreach (var error in syncErrors)
                        {
                            Debug.WriteLine(@"error.Operation Kind: {0}.", error.OperationKind);
                            Debug.WriteLine(@"Mobile Service Table Operation Kind: {0}.", MobileServiceTableOperationKind.Update);
                            Debug.WriteLine(@"error.Result: {0}.", error.Result);
                            Debug.WriteLine(@"error.Status: {0}.", error.Status);

                            Debug.WriteLine(@"HttpStatusCode.PreconditionFailed: {0}.", HttpStatusCode.PreconditionFailed);
                            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                            {
                                //Update failed, reverting to server's copy.
                                await error.CancelAndUpdateItemAsync(error.Result);
                            }
                            else if (error.Status == HttpStatusCode.PreconditionFailed)
                            {
                                // Discard local change.
                                await error.CancelAndDiscardItemAsync();
                            }
                            Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                        }
                    }
                }
            }
        }
    }
}