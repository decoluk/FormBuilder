using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace ModelLib
{
    [XmlRoot("mfbProject_COLLECTION")]
    public class mfbProject
    {
        public int? fbp_id { get; set; }
        [DisplayName("Name")]
        public string fbp_name { get; set; }
        [DisplayName("DB")]
        public string fbp_db { get; set; }
        [DisplayName("IP")]
        public string fbp_ip { get; set; }
    }

    [XmlRoot("mfbEntity_COLLECTION")]
    public class mfbEntity
    {
        public int fbe_id;
        public int fbp_id;
        [DisplayName("Name")]
        public string fbe_name { get; set; }
        [DisplayName("DB Name")]
        public string fbe_mapping_db { get; set; }
        [DisplayName("Table Name")]
        public string fbe_mapping_table { get; set; }
        [DisplayName("Description")]
        public string fbe_desc{ get; set; }
        public DateTime? fbe_date;
    }

    [XmlRoot("mfbEntityColumn_COLLECTION")]
    public class mfbEntityColumn
    {
        public int fbec_id;
        public int fbe_id;
        public int? fbec_parent_entity_fbe_id;
        public int? fbft_id;
        public string fbec_name;
        public string fbec_db_field_name;
        public int? fbec_db_field_len;
        public string fbec_default_value;
        public bool fbec_is_key;
        public bool fbec_is_cust_column;
        public decimal fbec_sort_seq;
        public string fbec_desc;
        public DateTime? fbec_date;
    }

    [XmlRoot("mfbFieldType_COLLECTION")]
    public class mfbFieldType
    {
        public int fbft_id;
        public int? fblv_id;
        public string fbft_name;
        public bool fbft_is_listview;
        public DateTime? fbft_date;
    }

    [XmlRoot("mfbListView_COLLECTION")]
    public class mfbListView
    {
        public int fblv_id;
        public int fbp_id;
        public string fblv_name;
        public DateTime? fblv_date;
    }

    [XmlRoot("mfbListViewItem_COLLECTION")]
    public class mfbListViewItem
    {
        public int fblvi_id;
        public int fblv_id;
        public int? fbec_id;
        public string fblvi_name;
        public int fblvi_fontzie;
        public int fblvi_width;
        public bool fblvi_is_cust_column;
        public bool fblvi_is_button;
        public decimal fblvi_seq;
        public DateTime? fblvi_date;
    }

    [XmlRoot("mFormBuilder_COLLECTION")]
    public class mFormBuilder
    {
        public int fb_id;
        public int fbp_id;
        public string fb_name;
        public string fb_key;
        public DateTime? fb_date;
        public string fb_desc;
        public DateTime? fb_is_auto_gen;
    }
}
