using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicaComic.Models;

namespace ShadowViewer.Plugin.Bika.Args
{
    public class ComicArg
    {
        public ComicInfo ComicInfo { get; set; }
        public int CurrentEpisode { get; set; }
        public ObservableCollection<Episode> Episodes { get; set; }
    }
}
