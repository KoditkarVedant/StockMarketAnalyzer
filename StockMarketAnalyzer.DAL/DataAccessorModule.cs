using Autofac;
using StockMarketAnalyzer.DAL;
using StockMarketAnalyzer.DAL.Helpers;
using StockMarketAnalyzer.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL
{
    public class DataAccessorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DataAccessor>().As<IDataAccessor>().SingleInstance();
            builder.RegisterType<DataSession>().As<IDataSession>().SingleInstance();
            builder.RegisterType<StockMarketDbContext>().As<DbContext>();
            builder.RegisterType<ApiHelper>().As<IApiHelper>();
        }
    }
}
