using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using _2ReviewEmployeeSideHomeScreen.Service;
using Android.Content;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using _2ReviewEmployeeSideHomeScreen.ModelClasses;

namespace _2ReviewEmployeeSideHomeScreen.ActivityFragment
{
    class HomeFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mLayoutAdapter;
        private static LayoutInflater inflater;
        private Context context;
        private AzureDataService mAzureDataService;
        private bool isComplete = false;
        private TextView mEmpRankTextView;
        private TextView mEmpNameTextView;
        private TextView mEmpDesignationTextView;
        private int mEmpRanking;
        private string mEmpDesignation;
        private List<string> mRoundName;
        private List<int> mRoundProgress;
        private List<DateTime> mRoundDate;
        private string mEmpName;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CurrentPlatform.Init();
            mRoundName = new List<string>();
            mRoundProgress = new List<int>();
            mRoundDate = new List<DateTime>();
            mAzureDataService = new AzureDataService();
            context = Activity;
            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            init();
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.Home, null);
            mEmpNameTextView = root.FindViewById<TextView>(Resource.Id.empNameTextView);
            mEmpRankTextView = root.FindViewById<TextView>(Resource.Id.empRankTextView);
            mEmpDesignationTextView = root.FindViewById<TextView>(Resource.Id.empDesignationTextView);
            //System.Diagnostics.Debug.WriteLine("Performance Id {0}", mPerformanceIdList.Count);
            mRecyclerView = root.FindViewById<RecyclerView>(Resource.Id.roundProgressRecyclerView);

            return root;
        }

        public async void init()
        {
            isComplete = await mAzureDataService.Initialize();
            if (isComplete)
                setPerformanceList();
        }

        public async void setEmployeeDetail()
        {
            mEmpNameTextView.Text = await mAzureDataService.getEmployeeName("726495f595dd432d922e2da7946a7136");
            mEmpRankTextView.Text = await mAzureDataService.getEmployeeRanking("726495f595dd432d922e2da7946a7136") +"";
        }

        public async void setPerformanceList()
        {
            mRoundName = await mAzureDataService.GetRoundName("726495f595dd432d922e2da7946a7136");
            mRoundDate = await mAzureDataService.GetRoundDate("726495f595dd432d922e2da7946a7136");
            mRoundProgress = await mAzureDataService.getRoundProgress("726495f595dd432d922e2da7946a7136");

            if (mRoundName.Count > 0 && mRoundDate.Count > 0 && mRoundProgress.Count > 0)
            {
                mLayoutManager = new LinearLayoutManager(Activity);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mLayoutAdapter = new RecyclerAdapter(mRoundName, mRoundProgress, mRoundDate);
                mRecyclerView.SetAdapter(mLayoutAdapter);
            }

            //System.Diagnostics.Debug.WriteLine("Performance Id in setperlist {0}", mPerformanceList.Count);
        }

        public static implicit operator Fragment(HomeFragment v)
        {
            throw new NotImplementedException();
        }

        public class RecyclerAdapter : RecyclerView.Adapter
        {
            private List<string> mRoundName;
            private List<int> mRoundProgress;
            private List<DateTime> mRoundDate;
            AzureDataService dataService = new AzureDataService();
            //String RoundName,RoundDate;
            //int RateCount, ProgressRate;
            public RecyclerAdapter(List<string> mRoundName, List<int> mRoundProgress, List<DateTime> mRoundDate)
            {
                this.mRoundName = mRoundName;
                this.mRoundProgress = mRoundProgress;
                this.mRoundDate = mRoundDate;
            }

            public override int ItemCount
            {
                get { return mRoundName.Count; }
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView myHolder = holder as MyView;
            
                myHolder.mRoundName.Text = mRoundName[position];
                string myDate = String.Format("{0:dd-MM-yyyy}", mRoundDate[position]);
                myHolder.mRoundDate.Text = myDate;
                myHolder.mRateCount.Text = mRoundProgress[position].ToString();
                myHolder.mProgress1.Progress = (mRoundProgress[position] * 20);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View performanceData = inflater.Inflate(Resource.Layout.Ratinglist,parent,false);
                TextView mRoundName = performanceData.FindViewById<TextView>(Resource.Id.txtRoundName);
                TextView mRoundDate = performanceData.FindViewById<TextView>(Resource.Id.txtDate);
                TextView mRateCount = performanceData.FindViewById<TextView>(Resource.Id.txtRateCount);
                ProgressBar mProgress1 = performanceData.FindViewById<ProgressBar>(Resource.Id.progressrate);
                MyView view = new MyView(performanceData) { mRoundName=mRoundName,mRoundDate=mRoundDate,mRateCount=mRateCount,mProgress1=mProgress1};
                return view;
            }

            public class MyView : RecyclerView.ViewHolder
            {
                public View mMainView { get; set; }
                public TextView mRoundName { get; set; }
                public TextView mRoundDate { get; set; }
                public TextView mRateCount { get; set; }
                public ProgressBar mProgress1 { get; set; }

                public MyView(View view) : base(view)
                {
                    mMainView = view;
                }
            }

        }
    }
}
