using PicaComic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowViewer.Plugin.Bika.Enums;

namespace ShadowViewer.Plugin.Bika.Args
{
    public class CategoryArg
    {
        public CategoryMode Mode { get; set; }
        public string Category{ get; set; }
        public int Page { get; set; } = 1;
        public SortRule SortRule { get; set; } = SortRule.dd;
    }
}
