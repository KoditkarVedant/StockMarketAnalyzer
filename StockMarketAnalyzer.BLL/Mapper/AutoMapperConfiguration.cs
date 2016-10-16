using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using StockMarketAnalyzer.DAL.DataModels;

namespace StockMarketAnalyzer.BLL.Mapper
{
    public class AutoMapperConfiguration
    {
        public static IMapper Mapper;
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyDetail, BusinessModel.CompanyDetail>().ReverseMap();
                
            });
            
            Mapper = config.CreateMapper();
        }
    }
}
