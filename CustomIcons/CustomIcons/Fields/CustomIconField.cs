using System;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;

namespace WesleyLomax.CustomIcons.Fields
{
    public class CustomIconField
    {
        public CustomIconField()
        {
        }

        internal CustomIconField(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Guid g;
                if (Guid.TryParse(value, out g))
                {
                    CssClass = ResolveIconClass(g);
                }
            }
        }
        public string CssClass { get; set; }

        private static string ResolveIconClass(Guid g)
        {
            var cssClass = string.Empty;

            var db = Factory.GetDatabase("master");
            if (db != null)
            {
                Item iconItem;

                using (new SecurityDisabler())
                {
                    iconItem = db.GetItem(new ID(g));
                }

                if (iconItem != null)
                {
                    cssClass = iconItem["CssClass"];
                }
            }
            return cssClass;
        }
    }
}