﻿using PicaComic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.Bika.Args
{
    public class CategoryArg
    {
        public string Category{ get; set; }
        public int Page { get; set; } = 1;
        public SortRule SortRule { get; set; } = SortRule.dd;
    }
}