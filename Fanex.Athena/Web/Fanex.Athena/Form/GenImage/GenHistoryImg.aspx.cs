using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JSoft.Utility;
using HandHistoryLibrary.DataHandler;


public partial class GenImage_GenHistoryImg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameType = StringUtil.CStr(Request["GameType"], true); //遊戲分類

        GenHistoryImage objHistoryImg = new GenHistoryImage();

        switch (strGameType.ToLower())
        {
            case "poker": //Poker
                objHistoryImg.GenResultPic(this.Context);
                break;
            case "roulette"://roulette
                objHistoryImg.GenRoulettePic(this.Context);
                break;
            case "sicbo"://Sicbo
                objHistoryImg.GenSicBoPic(this.Context);
                break;
            case "slot"://Taizen 5X3
                objHistoryImg.GenTaizenPic(this.Context);
                break;
            case "fruit"://Fruit 3X3
                objHistoryImg.GenFruit(this.Context);
                break;
            case "classic"://Classic 3X1
                objHistoryImg.GenClassic(this.Context);
                break;
            case "wheel"://MoneyWheel
                objHistoryImg.GenMoneyWheelPic(this.Context);
                break;
            case "mahjong"://mahjong
                objHistoryImg.GenMohjongPic(this.Context);
                break;
            default:
                break;
        }
       
    }
}
