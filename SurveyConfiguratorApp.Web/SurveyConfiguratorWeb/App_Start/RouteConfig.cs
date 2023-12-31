﻿using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SurveyConfiguratorWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
                routes.MapMvcAttributeRoutes();
                routes.MapRoute(
                name: "Question",
                url: "Question/{action}/{id}",
                defaults: new { controller = "Question", action = "Index", id = UrlParameter.Optional }
                 );

                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Question", action = "Index", id = UrlParameter.Optional }
                );

                routes.MapRoute(
                name: "Error",
                url: "Error/{action}",
                defaults: new { controller = "Error" }
                   );

            }
            catch (System.Exception e)
            {
                Log.Error(e);
            }


        }
    }
}
