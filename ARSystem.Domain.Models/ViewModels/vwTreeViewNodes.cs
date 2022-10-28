using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vwTreeViewNodes
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public vwTreeViewState state { get; set; }
        public string a_attr { get; set; }
        public List<vwRABapsDone> data { get; set; }
        public vwTreeViewNodeData li_attr { get; set; }
    }

    public class vwTreeViewState
    {
        public bool opened { get; set; } = false;
        public bool disabled { get; set; } = false;
        public bool selected { get; set; } = false;
    }

    public class vwTreeViewNodeData
    {
        public string data_node { get; set; }
        public string data_value { get; set; }
    }
}
