using System.Collections.Generic;
using System.Collections.ObjectModel;
using PicaComic.Models;
using ShadowViewer.Plugin.Bika.Models;

namespace ShadowViewer.Plugin.Bika;

public class BikaData
{
    public Profile CurrentUser { get; set; }
    public ObservableCollection<string> Keywords { get; } = new ObservableCollection<string>();
    public ObservableCollection<BikaLock> Locks { get; } = new ObservableCollection<BikaLock>();
    public static BikaData Current { get; } = new BikaData();
    public static IReadOnlyList<string> Categories { get; } = new List<string>()
        {
    "嗶咔漢化",
    "全彩",
    "長篇",
    "同人",
    "短篇",
    "圓神領域",
    "碧藍幻想",
    "CG雜圖",
    "英語 ENG",
    "生肉",
    "純愛",
    "百合花園",
    "耽美花園",
    "偽娘哲學",
    "後宮閃光",
    "扶他樂園",
    "單行本",
    "姐姐系",
    "妹妹系",
    "SM",
    "性轉換",
    "足の恋",
    "人妻",
    "NTR",
    "強暴",
    "非人類",
    "艦隊收藏",
    "Love Live",
    "SAO 刀劍神域",
    "Fate",
    "東方",
    "WEBTOON",
    "禁書目錄",
    "歐美",
    "Cosplay",
    "重口地帶",
        };
}