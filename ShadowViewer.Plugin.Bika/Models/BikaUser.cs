using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Models;

public class BikaUser
{
    [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsPrimaryKey = true, IsNullable = false)]
    public string Email { get; set; }
    [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsNullable = false)]
    public string Password { get; set; }
    [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsNullable = false)]
    public string Token { get; set; }
}