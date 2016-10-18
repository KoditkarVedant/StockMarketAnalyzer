using AutoMapper;
using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketAnalyzer.DAL.Mapper
{
    public class AutoMapperConfiguration
    {
        public static IMapper Mapper;
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DataModels.ApiModels.Assetprofile, CompanyProfile>()
                    .ForMember(dest => dest.FullTimeEmployees, opts => opts.MapFrom(src => Convert.ToDecimal(src.FullTimeEmployees != 0 ? src.FullTimeEmployees : -1)))
                    .ReverseMap();

                cfg.CreateMap<DataModels.ApiModels.Companyofficer, CompanyOfficer>()
                    .ForMember(dest => dest.FiscalYear, opts => opts.MapFrom(src => Convert.ToDecimal(src.FiscalYear != 0 ? src.FiscalYear : -1)))
                    .ForMember(dest => dest.Age, opts => opts.MapFrom(src => Convert.ToDecimal(src.Age != 0 ? src.Age : -1)))
                    .ForMember(dest => dest.TotalPay, opts => opts.MapFrom(src => Convert.ToDecimal(src.TotalPay != null ? src.TotalPay.Raw : "-1")))
                    .ForMember(dest => dest.ExercisedValue, opts => opts.MapFrom(src => Convert.ToDecimal(src.ExercisedValue != null ? src.ExercisedValue.Raw : "-1")))
                    .ForMember(dest => dest.UnexercisedValue, opts => opts.MapFrom(src => Convert.ToDecimal(src.UnexercisedValue != null ? src.ExercisedValue.Raw : "-1")))
                    .ReverseMap();
            });

            Mapper = config.CreateMapper();
        }
    }
}
