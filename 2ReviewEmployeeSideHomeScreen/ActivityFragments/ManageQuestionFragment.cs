﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace _2ReviewEmployeeSideHomeScreen.ActivityFragment
{
    class ManageQuestionFragment : Android.Support.V4.App.Fragment
    {


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.ManageQuestion, null);
            return root;
        }

        public static implicit operator Fragment(ManageQuestionFragment v)
        {
            throw new NotImplementedException();
        }
    }
}
