using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("CategoryID"),
                       Selected = item.GetPropertyValue("CategoryID").Equals(selectedValue.ToString())
                   };
        }
        public static IEnumerable<SelectListItem> ToSelectListItem1<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("ProductCategoryID"),
                       Selected = item.GetPropertyValue("ProductCategoryID").Equals(selectedValue.ToString())
                   };
        }
        public static IEnumerable<SelectListItem> ToSelectListItem2<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Number"),
                       Value = item.GetPropertyValue("SizeID"),
                       Selected = item.GetPropertyValue("SizeID").Equals(selectedValue.ToString())
                   };
        }
        public static IEnumerable<SelectListItem> ToSelectListItem3<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("NameColor"),
                       Value = item.GetPropertyValue("ColorID"),
                       Selected = item.GetPropertyValue("ColorID").Equals(selectedValue.ToString())
                   };
        }

        public static IEnumerable<SelectListItem> ToSelectListItemString<T>(this IEnumerable<T> items, string selectedValue)
        {
            if (selectedValue == null)
            {
                selectedValue = "";
            }
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
                   };
        }
    }
}
