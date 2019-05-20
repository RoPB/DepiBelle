using System;
using System.Collections.Generic;
using DepiBelleDepi.ViewModels;
using Newtonsoft.Json;

namespace DepiBelleDepi.Models
{
    public static class DeepLinksMapper
    {
        private const string NEW_ORDER = "NUEVA_ORDEN";

        static Dictionary<string, Type> deepLinkViewModelTypeMapper = new Dictionary<string, Type>()
        {
            { NEW_ORDER, typeof(OrdersViewModel)}
        };

        static Dictionary<string, Type> deepLinkParamTypeMapper = new Dictionary<string, Type>()
        {
            { NEW_ORDER, typeof(NewOrder)}
        };

        public static Type ResolveViewModelType(string deepLinkType)
        {
            try
            {
                if (!deepLinkViewModelTypeMapper.ContainsKey(deepLinkType))
                    return null;

                return deepLinkViewModelTypeMapper[deepLinkType];
            }
            catch
            {
                return null;
            }
        }

        public static object ResolveNavigationParam(string param, string deepLinkType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(param) || !deepLinkParamTypeMapper.ContainsKey(deepLinkType))
                    return null;

                return JsonConvert.DeserializeObject(param, deepLinkParamTypeMapper[deepLinkType]);
            }
            catch
            {
                return null;
            }
        }
    }
}
