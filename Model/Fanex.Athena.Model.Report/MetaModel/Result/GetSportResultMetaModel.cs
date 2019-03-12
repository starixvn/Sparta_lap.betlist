namespace Fanex.Athena.Model.Report.MetaModel.Result
{
    using System;
    using System.Web.Script.Serialization;

    [Serializable]
    public class GetSportResultMetaModel : ResultMetaModel
    {
        [ScriptIgnore]
        public string ProductCodeList { get; set; }
    }
}