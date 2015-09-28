using System;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data;
using WesleyLomax.CustomIcons.Fields;

namespace WesleyLomax.CustomIcons.FieldMappers
{
    public class CustomIconHandler : AbstractSitecoreFieldMapper
    {
        public CustomIconHandler()
            : base(typeof(CustomIconField))
        {
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                return null;

            try
            {
                ID id;
                if (ID.TryParse(fieldValue, out id))
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        var item = context.Service.Database.GetItem(id);
                        if (item != null && item.Fields["CssClass"] != null)
                        {
                            var icon = new CustomIconField
                            {
                                CssClass = item.Fields["CssClass"].Value
                            };
                            return icon;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Failed to get custom icon", ex);
                return null;
            }
        }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            var icon = value as CustomIconField;
            return icon == null ? string.Empty : icon.CssClass;
        }
    }
}
