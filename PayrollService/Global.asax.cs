using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayrollService.Services;
using PayrollService.Services.Interfaces;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace PayrollService
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register); 
            GlobalConfiguration.Configuration.Formatters.Clear();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter
            {
                SerializerSettings = jsonSerializerSettings
            });

            // Add di dependency resolver
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<IncomeCalculator>().As<IGrossIncomeCalculator>();
            builder.RegisterType<IncomeCalculator>().As<ITaxesDeductionCalculator>();
            builder.RegisterType<TaxCalculatorFactory>().As<ITaxCalculatorFactory>();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
