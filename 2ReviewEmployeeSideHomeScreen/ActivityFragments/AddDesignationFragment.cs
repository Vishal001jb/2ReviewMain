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
    class AddDesignationFragment : Android.Support.V4.App.Fragment
    {


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.AddDesignation, null);
            return root;
        }

        public static implicit operator Fragment(AddDesignationFragment v)
        {
            throw new NotImplementedException();
        }
    }
}
