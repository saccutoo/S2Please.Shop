using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SHOP.COMMON.Helpers
{
    public static class MapperHelper
    {
        //use for simple object
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        //use for complex object : have child object/List<childObject> inside
        public static TDestination Map<TSource, TDestination, TProfile>(TSource source) where TProfile : Profile, new()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(source);
        }

        public static List<TDestination> MapList<TSource, TDestination>(List<TSource> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }

        public static T1 Map<T1, T2>(T2 data)
        {
            throw new NotImplementedException();
        }

        public static List<TDestination> MapList<TSource, TDestination, TProfile>(List<TSource> source) where TProfile : Profile, new()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<List<TSource>, List<TDestination>>(source);
        }
        public static void AddUnique<T>(this IList<T> self, IEnumerable<T> items, string uniqFields)
        {
            var fields = uniqFields.Split(';').ToList();
            foreach (var item in items)
                if (!self.Any(x => Compare<T>(x, item, fields)))
                    self.Add(item);
        }
        public static bool Compare<T>(T source, T destination, List<string> uniqFields)
        {
            foreach (var uniqField in uniqFields)
            {
                var sourceProp = source.GetType().GetProperty(uniqField, BindingFlags.Instance | BindingFlags.Public);
                var sourceResult = string.Empty;
                if (sourceProp != null)
                {
                    sourceResult = Convert.ToString(sourceProp.GetValue(source, null));
                }
                var destinationProp = destination.GetType().GetProperty(uniqField, BindingFlags.Instance | BindingFlags.Public);
                var destinationResult = string.Empty;
                if (destinationProp != null)
                {
                    destinationResult = Convert.ToString(destinationProp.GetValue(destination, null));
                }
                if (sourceResult != destinationResult) { return false; }
            }
            return true;
        }
        //public static T ConvertModel<T>(T model)
        //{
        //    foreach (var prop in model.GetType().GetProperties())
        //    {
        //        var clientDateFormat = Config.GetConfig(Constant.ClientDateFormat);
        //        if (prop.PropertyType.Name == "DateTime" && model.GetType().GetProperty(prop.Name).GetValue(model, null) != null && DateTime.Parse(model.GetType().GetProperty(prop.Name).GetValue(model, null).ToString()) != DateTime.MinValue)
        //        {
        //            prop.SetValue(prop, DateTime.ParseExact(model.GetType().GetProperty(prop.Name).GetValue(model, null).ToString(), clientDateFormat, CultureInfo.InvariantCulture));
        //        }
        //    }
        //    return model;
        //}
    }
}
