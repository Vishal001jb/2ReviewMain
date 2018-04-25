#define OFFLINE_SYNC_ENABLED
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

        List<Employee_Perfomance> EP;
        List<List<KeyValuePair<string, object>>> mEmpRoundDetails;
        public async Task<bool> Initialize()
        {
            //EP = new List<Employee_Perfomance>();
            mEmpRoundDetails = new List<List<KeyValuePair<string, object>>>();
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

            //var rev = new Reviewable();
            //rev.Designation_Id = "0253be21ede5455ea5a02f336b5fde64";
            //rev.Employee_Id = "726495f595dd432d922e2da7946a7136";
            //rev.Round_Id = "70f588eb88b34151998479dcbfcb1178";
            //rev.Status = "Complete";
            //rev.Total = 5;

            //ReviewableTable.InsertAsync(rev);

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
                var RoundId = await ReviewableTable.Where(r => r.Employee_Id == EmpId).Select(r => r.Round_Id).ToListAsync();
                Debug.WriteLine("Round Id from Reviewable table : ", string.Join(", ", RoundId));
                for (int i = 0; i < RoundId.Count; i++)
                {
                    var result = await RoundTable.Where(r => r.Id == RoundId[i]).Select(r => r.Round_Name).ToListAsync();
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

        public async Task<string> getEmployeeName(string EmpId)
        {
            var FirstName = await EmpTable.Where(e => e.Id == EmpId).Select(e => e.Employee_First_Name).ToListAsync();
            var LastName = await EmpTable.Where(e => e.Id == EmpId).Select(e => e.Employee_Last_Name).ToListAsync();
            return FirstName + " " + LastName;
        }

        public async Task<string> getEmployeeRanking(string EmpId)
        {
            var rid = await ReviewableTable.OrderByDescending(r => r.CreatedAt).Where(e => e.Employee_Id == EmpId).Select(r => r.Id).ToListAsync();
            var rank = await EmpPerformanceTable.Where(p => p.Round_Id == rid[0]).Select(p => p.Ranking).ToListAsync();
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