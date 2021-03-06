﻿using System;
using Datory;
using SiteServer.Plugin;

namespace SiteServer.CMS.Database.Models
{
    [Table("siteserver_Channel")]
    public partial class ChannelInfo : Entity, IChannelInfo, ICloneable
    {
        [TableColumn]
        public string ChannelName { get; set; }

        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public string ContentModelPluginId { get; set; }

        [TableColumn(Text = true)]
        public string ContentRelatedPluginIds { get; set; }

        [TableColumn]
        public int ParentId { get; set; }

        [TableColumn]
        public string ParentsPath { get; set; }

        [TableColumn]
        public int ParentsCount { get; set; }

        [TableColumn]
        public int ChildrenCount { get; set; }

        [TableColumn]
        private string IsLastNode { get; set; }

        public bool LastNode
        {
            get => IsLastNode == "True";
            set => IsLastNode = value.ToString();
        }

        [TableColumn]
        public string IndexName { get; set; }

        [TableColumn(Text = true)]
        public string GroupNameCollection { get; set; }

        [TableColumn]
        public int Taxis { get; set; }

        [TableColumn]
        public DateTime? AddDate { get; set; }

        [TableColumn(Length = 1000)]
        public string ImageUrl { get; set; }

        [TableColumn(Text = true)]
        public string Content { get; set; }

        [TableColumn(Length = 1000)]
        public string FilePath { get; set; }

        [TableColumn(Length = 1000)]
        public string ChannelFilePathRule { get; set; }

        [TableColumn(Length = 1000)]
        public string ContentFilePathRule { get; set; }

        [TableColumn(Length = 1000)]
        public string LinkUrl { get; set; }

        [TableColumn]
        public string LinkType { get; set; }

        [TableColumn]
        public int ChannelTemplateId { get; set; }

        [TableColumn]
        public int ContentTemplateId { get; set; }

        [TableColumn(Length = 2000)]
        public string Keywords { get; set; }

        [TableColumn(Length = 2000)]
        public string Description { get; set; }

        [TableColumn(Text = true, Extend = true)]
        public string ExtendValues { get; set; }

        public object Clone()
        {
            return (ChannelInfo)MemberwiseClone();
        }
    }
}
