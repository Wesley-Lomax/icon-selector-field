using System;
using System.Text;
using Sitecore.Diagnostics;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;

namespace WesleyLomax.CustomIcons.Fields
{
    public class CustomIconLink : Input
    {
        protected Panel DescriptionPanel;
        public string Source { get; set; }

        protected override void OnInit(EventArgs e)
        {
            DescriptionPanel = new Panel();
            FormatPanel(DescriptionPanel);
            Controls.Add(DescriptionPanel);
            DescriptionPanel.ID = GetID("DescriptionPanel");
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            RefreshDescriptionPanel();
            base.OnLoad(e);
        }

        protected virtual void FormatPanel(Panel panel)
        {
            panel.Background = "white";
            panel.Border = "1px solid #cccccc";
            panel.Padding = "8px 4px 8px 4px";
        }

        protected virtual void RefreshDescriptionPanel()
        {
            var builder = new StringBuilder("");
            builder.Append("<div style=\"height:50px;\">");
            if (string.IsNullOrEmpty(Value))
            {
                builder.Append("No icon has been selected.");
            }
            else
            {
                builder.Append("<link rel='stylesheet' href='/sitecore/shell/client/Your Apps/Dialogs/InsertCustomIconDialog/Icons.css'>");

                var field = new CustomIconField(Value);
                builder.AppendFormat("<i class=\"{0}\"></i>", field.CssClass);
            }
            builder.Append("</div>");
            DescriptionPanel.InnerHtml = builder.ToString();
        }

        public override void HandleMessage(Message message)
        {
            if (message["id"] == ID)
            {
                switch (message.Name)
                {
                    case "customiconlink:edit":
                        Sitecore.Context.ClientPage.Start(this, "ShowEditCustomIcon");
                        break;
                    case "customiconlink:clear":
                        Value = string.Empty;
                        SetModified();
                        break;
                }
            }
            base.HandleMessage(message);
        }

        protected virtual void ShowEditCustomIcon(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (args.IsPostBack)
            {
                if (args.HasResult)
                {
                    if (!string.Equals(args.Result, Value, StringComparison.OrdinalIgnoreCase))
                    {
                        Value = args.Result;
                        SetModified();
                    }
                }
            }
            else
            {
                var urlString = new UrlString("/sitecore/client/Your%20Apps/Dialogs/InsertCustomIconDialog");
                var handle = new UrlHandle();
                handle["value"] = Value;
                handle.Add(urlString);
                SheerResponse.ShowModalDialog(urlString.ToString(), "480px", "300px", string.Empty, true);
                args.WaitForPostBack();
            }
        }

        protected override void SetModified()
        {
            base.SetModified();
            if (TrackModified)
            {
                Sitecore.Context.ClientPage.Modified = true;
                RefreshDescriptionPanel();
            }
        }
    }
}