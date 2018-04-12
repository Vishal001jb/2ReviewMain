using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using _2ReviewEmployeeSideHomeScreen.ModelClasses;
using System.Collections.Generic;
using _2ReviewEmployeeSideHomeScreen.Service;
using System.Threading.Tasks;
using Android.Content;
using Android.Widget;
using System.Diagnostics;

namespace _2ReviewEmployeeSideHomeScreen.ActivityFragment
{
    class HomeFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mLayoutAdapter;
        private List<string> mPerformanceIdList;
        private static LayoutInflater inflater;
        private Context context;
        private AzureDataService mAzureDataService;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mAzureDataService = new AzureDataService();
            context = Activity;
            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.Home, null);
            mRecyclerView = root.FindViewById<RecyclerView>(Resource.Id.roundProgressRecyclerView);
            setPerformanceList();
            mLayoutManager = new LinearLayoutManager(Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mLayoutAdapter = new RecyclerAdapter(mPerformanceIdList);
            return root;
        }

        public async void setPerformanceList()
        {
            await mAzureDataService.Initialize();
            mPerformanceIdList = await mAzureDataService.GetEmpRoundWisePerformance();
            System.Diagnostics.Debug.WriteLine("Items from Local {0}", string.Join(", ", mPerformanceIdList.Count));
        }

        public static implicit operator Fragment(HomeFragment v)
        {
            throw new NotImplementedException();
        }

        public class RecyclerAdapter : RecyclerView.Adapter
        {
            List<string> mPerformanceIdList;

            public RecyclerAdapter(List<string> mPerformanceIdList)
            {
                this.mPerformanceIdList = mPerformanceIdList;
                System.Diagnostics.Debug.WriteLine("Performance Id {0}", mPerformanceIdList.Count);
            }

            public override int ItemCount
            {
                get { return mPerformanceIdList.Count; }
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView myHolder = holder as MyView;
                myHolder.mRoundName.Text = mPerformanceIdList[position];
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
