// -----------------------------------------------------------------------
// <copyright file="CasinoWinLossDetailHeader.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.DTO.RNGResultDetail
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// CasinoWinLossDetailHeader class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class CasinoWinLossDetailHeader
    {
        /// <summary>
        /// Gets or sets currency of member.
        /// </summary>
        /// <value>Currency name.</value>
        public string OwnCurrencyName { get; set; }

        /// <summary>
        /// Gets or sets show type.
        /// Per one row appear need to according to “ShowType” column.
        /// </summary>
        /// <value>BasicInfo, AddRow_HitBetLine, Result_HitBetLine, Add_TotalLineWinLoss, Add_BonusGame, MaxMultiply.</value>
        public string ShowType { get; set; }

        /// <summary>
        /// Gets or sets table name.
        /// Support multiple  language.
        /// </summary>
        /// <value>The table name,ex: TableName_XX.</value>
        public string TableName { get; set; }

        public DateTime BetTime { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets result.
        /// </summary>
        /// <value>It's a image link.</value>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets result name for each game type.
        /// Support multiple language.
        /// it will be displayed in the title.
        /// </summary>
        /// <value>The result name.</value>
        public string ResultName { get; set; }

        public string PlayerHistoryNo { get; set; }

        public string AutoSpinResult { get; set; }

        public string ModeResult { get; set; }

        public string FreeSpinResult { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Reviewed.")]
        public string Type { get; set; }

        public string Multiplier { get; set; }

        public string BonusName { get; set; }

        public string TotalLineBet { get; set; }

        public string LineNo { get; set; }

        /// <summary>
        /// Gets or sets sysbol.
        /// </summary>
        /// <value>The symbol,ex: Image x1.</value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets coin size.
        /// </summary>
        /// <value>The coin size,ex: UUS 5.00.</value>
        public string CoinSize { get; set; }

        /// <summary>
        /// Gets or sets coin size amount .
        /// Only for group 7-3: game type 10's or Better.
        /// </summary>
        /// <value>The coin size amount,ex: IN 0.50 X 1 .</value>
        public string CoinSizeAmount { get; set; }

        /// <summary>
        /// Gets or sets coin size.
        /// it's caculated by getting number from string then multiply  this number by exchange rate.
        /// </summary>
        /// <value>The coin size ,ex: 123.33.</value>
        public decimal CoinSizeValue { get; set; }

        /// <summary>
        /// Gets or sets bet text.
        /// </summary>
        /// <value>The bet value, ex: UUS 2.00.</value>
        public string Bet { get; set; }

        /// <summary>
        /// Gets or sets bet value.
        /// </summary>
        /// <value>The bet Value, ex: 2.00.</value>
        public decimal BetValue { get; set; }

        public string Odds { get; set; }

        /// <summary>
        /// Gets or sets win text.
        /// </summary>
        /// <value>The win value ,ex: UUS 5.00.</value>
        public string Win { get; set; }

        /// <summary>
        /// Gets or sets game win result.
        /// Only used from Champions game.
        /// </summary>
        /// <value>The win result,ex: IN 1.50 .</value>
        public string WinResult { get; set; }

        /// <summary>
        /// Gets or sets win-loss text.
        /// </summary>
        /// <value>The win loss,ex: UUS -30.00.</value>
        public string WinLoss { get; set; }

        /// <summary>
        /// Gets or sets win loss result.
        /// Only used for Mamak Corner game.
        /// </summary>
        /// <value>The win loss result,ex: IN -0.60 .</value>
        public string WinLossResult { get; set; }

        public string BetPerLine { get; set; }

        /// <summary>
        /// Gets or sets Round type result.
        /// Support multiple language.
        /// </summary>
        /// <value>The bet per line, value.</value>
        public string RoundTypeResult { get; set; }

        public string HandNoResult { get; set; }

        public string RankResult { get; set; }

        public string FinalHandResult { get; set; }

        public string BetTypeName { get; set; }

        public string AutoPlayResult { get; set; }

        /// <summary>
        /// Gets or sets action name.
        /// Support multiple language.
        /// It's property of group 4.
        /// </summary>
        /// <value>The action name.</value>
        public string ActionName { get; set; }

        public decimal CoinSizeBaseCurrencyValue { get; set; }

        public decimal WinBaseCurrencyValue { get; set; }

        /// <summary>
        /// Gets or sets win result base on currency value.
        /// Only used for Champions game.
        /// </summary>
        /// <value>The Win result value.</value>
        public decimal WinResultBaseCurrencyValue { get; set; }

        public decimal BetBaseCurrencyValue { get; set; }

        public decimal WinLossResultBaseCurrecyValue { get; set; }

        /// <summary>
        /// Gets or sets pickNo
        /// Only used for  keno chinese and keno camera.
        /// </summary>
        /// <value>The PickNo Value.</value>
        public string PickNo { get; set; }

        /// <summary>
        ///  Gets or sets HitNo
        /// Only used for  keno chinese and keno camera.
        /// </summary>
        /// <value>The hitNo Value.</value>
        public string HitNo { get; set; }

        /// <summary>
        ///  Gets or sets PickCount
        /// Only used for  keno chinese and keno camera.
        /// </summary>
        /// <value>The PickCount Value.</value>
        public int PickCount { get; set; }

        /// <summary>
        /// Gets or sets PickCount
        /// Only used for  keno chinese and keno camera.
        /// </summary>
        /// <value>The HitCount Value.</value>
        public int HitCount { get; set; }
    }
}