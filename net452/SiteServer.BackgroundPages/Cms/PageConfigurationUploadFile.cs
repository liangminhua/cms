﻿using System;
using System.Web.UI.WebControls;
using SiteServer.BackgroundPages.Utils;
using SiteServer.CMS.Caches;
using SiteServer.Utils;
using SiteServer.CMS.Database.Core;
using SiteServer.CMS.Fx;
using SiteServer.Utils.Enumerations;

namespace SiteServer.BackgroundPages.Cms
{
	public class PageConfigurationUploadFile : BasePageCms
    {
		public TextBox TbFileUploadDirectoryName;
		public DropDownList DdlFileUploadDateFormatString;
		public DropDownList DdlIsFileUploadChangeFileName;
        public TextBox TbFileUploadTypeCollection;
        public DropDownList DdlFileUploadTypeUnit;
        public TextBox TbFileUploadTypeMaxSize;

		public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            WebPageUtils.CheckRequestParameter("siteId");

			if (!IsPostBack)
			{
                VerifySitePermissions(ConfigManager.WebSitePermissions.Configration);

                TbFileUploadDirectoryName.Text = SiteInfo.FileUploadDirectoryName;

                DdlFileUploadDateFormatString.Items.Add(new ListItem("按年存入不同目录(不推荐)", EDateFormatTypeUtils.GetValue(EDateFormatType.Year)));
                DdlFileUploadDateFormatString.Items.Add(new ListItem("按年/月存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)));
                DdlFileUploadDateFormatString.Items.Add(new ListItem("按年/月/日存入不同目录", EDateFormatTypeUtils.GetValue(EDateFormatType.Day)));
			    SystemWebUtils.SelectSingleItemIgnoreCase(DdlFileUploadDateFormatString, SiteInfo.FileUploadDateFormatString);

				FxUtils.AddListItems(DdlIsFileUploadChangeFileName, "自动修改文件名", "保持文件名不变");
			    SystemWebUtils.SelectSingleItemIgnoreCase(DdlIsFileUploadChangeFileName, SiteInfo.IsFileUploadChangeFileName.ToString());

                TbFileUploadTypeCollection.Text = SiteInfo.FileUploadTypeCollection.Replace("|", ",");
                var mbSize = GetMbSize(SiteInfo.FileUploadTypeMaxSize);
				if (mbSize == 0)
				{
                    DdlFileUploadTypeUnit.SelectedIndex = 0;
                    TbFileUploadTypeMaxSize.Text = SiteInfo.FileUploadTypeMaxSize.ToString();
				}
				else
				{
                    DdlFileUploadTypeUnit.SelectedIndex = 1;
                    TbFileUploadTypeMaxSize.Text = mbSize.ToString();
				}
			}
		}

		private static int GetMbSize(int kbSize)
		{
			var retval = 0;
			if (kbSize >= 1024 && ((kbSize % 1024) == 0))
			{
				retval = kbSize / 1024;
			}
			return retval;
		}

		public override void Submit_OnClick(object sender, EventArgs e)
		{
			if (Page.IsPostBack && Page.IsValid)
			{
                SiteInfo.FileUploadDirectoryName = TbFileUploadDirectoryName.Text;

                SiteInfo.FileUploadDateFormatString = EDateFormatTypeUtils.GetValue(EDateFormatTypeUtils.GetEnumType(DdlFileUploadDateFormatString.SelectedValue));
                SiteInfo.IsFileUploadChangeFileName = TranslateUtils.ToBool(DdlIsFileUploadChangeFileName.SelectedValue);

                SiteInfo.FileUploadTypeCollection = TbFileUploadTypeCollection.Text.Replace(",", "|");
                var kbSize = int.Parse(TbFileUploadTypeMaxSize.Text);
                SiteInfo.FileUploadTypeMaxSize = (DdlFileUploadTypeUnit.SelectedIndex == 0) ? kbSize : 1024 * kbSize;
				
				try
				{
                    DataProvider.Site.Update(SiteInfo);

                    AuthRequest.AddSiteLog(SiteId, "修改附件上传设置");

                    SuccessMessage("上传附件设置修改成功！");
				}
				catch(Exception ex)
				{
                    FailMessage(ex, "上传附件设置修改失败！");
				}
			}
		}

	}
}
