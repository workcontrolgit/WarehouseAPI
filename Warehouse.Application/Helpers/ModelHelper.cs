using System;
using System.Linq;
using Warehouse.Application.Interfaces;

namespace Warehouse.Application.Helpers
{
    public class ModelHelper : IModelHelper
    {
        /// <summary>
        /// Check field name in the model class
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string ValidateModelFields<Entity>(string fields)
        {
            string retString = string.Empty;

            var bindingFlags = System.Reflection.BindingFlags.Instance |
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Public;
            var listFields = typeof(Entity).GetProperties(bindingFlags).Select(f => f.Name).ToList();
            string[] arrayFields = fields.Split(',');
            foreach (var field in arrayFields)
            {
                if (listFields.Contains(field.Trim(), StringComparer.OrdinalIgnoreCase))
                    retString += field + ",";
            };
            return retString;
        }

        /// <summary>
        /// Get list of field names in the model class
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <returns></returns>
        public string GetModelFields<Entity>()
        {
            string retString = string.Empty;

            var bindingFlags = System.Reflection.BindingFlags.Instance |
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Public;
            var listFields = typeof(Entity).GetProperties(bindingFlags).Select(f => f.Name).ToList();

            foreach (string field in listFields)
            {
                retString += field + ",";
            }

            return retString;
        }
    }
}