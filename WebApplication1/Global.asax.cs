using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Dictionary<string, Vlasnik> vlasnici = Data.CitanjeVlasnika();
            HttpContext.Current.Application["Vlasnici"] = vlasnici;
            HttpContext.Current.Application["FitnesCentri"] = Data.CitanjeFitnesCentara(vlasnici);
            HttpContext.Current.Application["GrupniTreninzi"] = Data.CitanjeGrupnihTreninga();
            HttpContext.Current.Application["Posetioci"] = Data.CitanjePosetilaca();
            HttpContext.Current.Application["Treneri"] = Data.CitanjeTrenera();
            HttpContext.Current.Application["Komentari"] = Data.CitanjeKomentara();
        }
        
    }
}
