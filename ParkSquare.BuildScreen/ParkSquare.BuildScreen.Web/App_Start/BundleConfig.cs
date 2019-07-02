﻿using System.Web.Optimization;

namespace ParkSquare.BuildScreen.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}