using System.Web.Mvc;

namespace template.Areas.Customer
{
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { controller = "Home2", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "template.Areas.Customer.Controllers" }
            );
        }
    }
}