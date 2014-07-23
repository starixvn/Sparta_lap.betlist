// --------------------------------------------------------------------------
// <copyright file="RNGCasinoResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bravo.Tran" date="2013.7.11">Created</change>
// </history>
// ----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Casino
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.Models.DataDomain;

    /// <summary>
    /// RNGCasinoResult class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class RNGCasinoResult
    {
        public string SessionLogId { get; set; }

        public int Index { get; set; }

        public string GameId { get; set; }

        public string GameNo { get; set; }

        public int GameTypeId { get; set; }

        public string GameTypeNo { get; set; }

        public string GameTypeName { get; set; }

        public string PlayerHistoryNo { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public decimal BeginBalance { get; set; }

        public decimal EndBlance { get; set; }

        public int UserBetId { get; set; }

        public decimal Bet { get; set; }

        public decimal Stake { get; set; }

        public decimal Win { get; set; }

        public decimal WinLoss { get; set; }

        public int ExchangeId { get; set; }

        public int GameTableId { get; set; }

        public string ShoeNo { get; set; }

        public string HandNo { get; set; }

        public string ResultId { get; set; }

        public string UserName { get; set; }

        public decimal CompanyTotal { get; set; }

        /// <summary>
        /// Gets or sets result.
        /// Support multiple language.
        /// We have two kind result.it's  picture or text.
        /// Example:
        /// 1.Text : Dealer(17): C2,C5,SK<BR/>Player(16): H6,S4,D6.
        /// 2.Picture:<![CDATA[<img width="60" height="60"
        /// src="/Form/GenImage/HistoryImage_TattooArt.aspx
        /// ?Content=09,06,06,S1,02,06,07,09,06&GameTypeNo=S8001" align="absmiddle" border="0" class="code" /> ]]>
        /// </summary>
        /// <value>String value.</value>
        public string Result { get; set; }

        public int Status { get; set; }

        public int GameGroupId { get; set; }

        public string GameGroupName { get; set; }

        public string Currency { get; set; }

        public string TableName { get; set; }

        public decimal RollingComm { get; set; }

        public bool IsSpecialGame
        {
            get
            {
                return RNGCasinoSetting.SpecialGames.Contains(GameTypeNo);
            }
        }
    }
}