#define OFFLINE_SYNC_ENABLED
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using _2ReviewEmployeeSideHomeScreen.ModelClasses;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
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
        IMobileServiceSyncTable<EmployeeCredentials> EmpCredentialTable;
        IMobileServiceSyncTable<EmployeeDesignation> EmpDesigTable;
        IMobileServiceSyncTable<EmployeePerfomance> EmpPerformanceTable;
        IMobileServiceSyncTable<EmployeePoints> EmpPointsTable;
        IMobileServiceSyncTable<Form> FormTable;
        IMobileServiceSyncTable<FormQuestion> FormQuesTable;
        IMobileServiceSyncTable<QuestionDesignation> QuesDesigTable;
        IMobileServiceSyncTable<Reviewable> ReviewableTable;
        IMobileServiceSyncTable<Reviewee> RevieweeTable;
        IMobileServiceSyncTable<Round> RoundTable;

        public async Task Initialize()
        {
            //Get our sync table that will call out to azure
            CurrentPlatform.Init();
            MobileService = new MobileServiceClient("https://2review.azurewebsites.net");

            //setup our local sqlite store and intialize our table
            const string path = "2Review.db";
            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<Question>();
            store.DefineTable<Answer>();
            store.DefineTable<Assignment>();
            store.DefineTable<Designation>();
            store.DefineTable<Employee>();
            store.DefineTable<EmployeeCredentials>();
            store.DefineTable<EmployeeDesignation>();
            store.DefineTable<EmployeePerfomance>();
            store.DefineTable<EmployeePoints>();
            store.DefineTable<Form>();
            store.DefineTable<FormQuestion>();
            store.DefineTable<QuestionDesignation>();
            store.DefineTable<Reviewable>();
            store.DefineTable<Reviewee>();
            store.DefineTable<Round>();

            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            QuesTable = MobileService.GetSyncTable<Question>();
            AnswerTable = MobileService.GetSyncTable<Answer>();
            AssignTable = MobileService.GetSyncTable<Assignment>();
            DesigTable = MobileService.GetSyncTable<Designation>();
            EmpTable = MobileService.GetSyncTable<Employee>();
            EmpCredentialTable = MobileService.GetSyncTable<EmployeeCredentials>();
            EmpDesigTable = MobileService.GetSyncTable<EmployeeDesignation>();
            EmpPerformanceTable = MobileService.GetSyncTable<EmployeePerfomance>();
            EmpPointsTable = MobileService.GetSyncTable<EmployeePoints>();
            FormTable = MobileService.GetSyncTable<Form>();
            FormQuesTable = MobileService.GetSyncTable<FormQuestion>();
            QuesDesigTable = MobileService.GetSyncTable<QuestionDesignation>();
            ReviewableTable = MobileService.GetSyncTable<Reviewable>();
            RevieweeTable = MobileService.GetSyncTable<Reviewee>();
            RoundTable = MobileService.GetSyncTable<Round>();

            //EmpPerformanceTable = MobileService.GetSyncTable<EmployeePerfomance>();
            //var EP = new EmployeePerfomance();
            //EP.Reviewable_Id = "1";
            //EP.Round_Id = "1";
            //EP.Ranking = 1;
            //EP.Progress = 50;
            //EP.Form_Id = "1";

            //await EmpPerformanceTable.InsertAsync(EP);

            var thingsToDo = await EmpPerformanceTable.Select(E => E.Id).ToListAsync();
            //Debug.WriteLine("Items from Local {0}", string.Join(", ", thingsToDo));

            var remoteTable = MobileService.GetTable<EmployeePerfomance>();

            var remoteItems = remoteTable.Select(e => e.Id).ToListAsync();

            Debug.WriteLine("Items From Server {0}", string.Join(", ", remoteItems));
        }

        public async Task<List<string>> GetEmpRoundWisePerformance()
        {
            await Sync();
            var result = await EmpPerformanceTable.Select(EPT => EPT.Id).ToListAsync();
            return result;
        }

        public async Task<bool> DeleteQuestion(Question id)
        {
            //await QuesTable.DeleteAsync(id);
            await Sync();
            return true;
        }

        public async Task AddQuestion(string QuestionText)
        {
            var review = new Question
            {
                Question_Text = QuestionText
            };

            //await QuesTable.InsertAsync(review);
            await Sync();
        }

        public async Task Sync()
        {


            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                // Use a different query name for each unique query in your program.
                await EmpPerformanceTable.PullAsync("allRoundWiseEmpPerformance", EmpPerformanceTable.CreateQuery());
                await MobileService.SyncContext.PushAsync();
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                    //Simple error/ conflict handling.
                    if (syncErrors != null)
                    {
                        foreach (var error in syncErrors)
                        {
                            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                            {
                                //Update failed, revert to server's copy
                                await error.CancelAndUpdateItemAsync(error.Result);
                            }
                            else
                            {
                                //Discard local change
                               await error.CancelAndDiscardItemAsync();
                            }

                            Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                        }
                    }
                    else
                    {
                        Debug.WriteLine(@"SyncErrors : ", syncErrors);
                    }
                }
            }
        }
    }
}