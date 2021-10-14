using System;
using System.Net;
using Models.Library.Exceptions;
using System.Collections.Generic;
using System.Globalization;


namespace Models.Library.Helper
{
    public static class Helper
    {    
        public static string GetSupportedCultureString(string customCulture)
        {            
            try
            {
                var culture = GetCorrectSupportedCulture(customCulture);
                if (culture != null)
                    return culture.Name;
                
                return "en-US";
            }
            catch (Exception e)
            {
                return "en-US";
            }                        
        } 
        
        public static string GetHost(string hostName)
        {
            try
            {
                var uri = new Uri(hostName);    
                return uri.Authority;
            }
            catch (Exception e)
            {
                return null; 
            }
        }                
        
        public static CultureInfo GetCorrectSupportedCulture(string customCulture)
        {
            try
            {
                var culture = "da-DK";
                if (string.IsNullOrWhiteSpace(customCulture))
                        return new CultureInfo(culture);

                if (customCulture.Contains("en"))
                    return new CultureInfo("en-US");

                culture = customCulture.Contains("da") ? "da-DK" : "en-US";
                return new CultureInfo(culture);                
            }
            catch (Exception e)
            {
                return new CultureInfo("en-US");
            }
        }
            
        public static T Get<T>(this Dictionary<string, object> dictionary, string key)
        {
            try
            {
                object value;
                bool valueExist = dictionary.TryGetValue(key, out value);
                if (!valueExist)
                    return default(T);

                var type = typeof(T);
                if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) 
                {
                    if (value == null) 
                    { 
                        return default(T); 
                    }

                    type = Nullable.GetUnderlyingType(type);
                }

                                
                return (T)Convert.ChangeType(dictionary[key], type);    
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
        
        public static ApiException CreateApiException(string message, HttpStatusCode statuscode, bool requireTranslation = true)
        {
            return new ApiException
            {
                Errors = new List<string>{message},
                StatusCode = (int)statuscode
            };
        }
               
        public static ApiException CreateApiExceptions(List<string> messages, HttpStatusCode statuscode, bool requireTranslation = true)
        {
            return new ApiException
            {
                Errors = messages,
                StatusCode = (int)statuscode
            };
        }
    }
}