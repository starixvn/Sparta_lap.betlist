namespace Fanex.Athena.BetList.Controller.Utility
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using BetListNPOI.SS.UserModel;
    using Common.Utility.Services;
    using Common.Utility.Services.Interfaces;
    using Excel;
    using Excel.Model;
    using Ionic.Zip;
    using Model.DTO;
    using Model.MetaModel;
    using Newtonsoft.Json;
    using Service.Helpers;
    using Service.Interfaces;

    public class BettingTicketCenterZipBuilder : IBettingTicketCenterZipBuilder
    {
        private readonly IAppSettings _appSettings;
        private readonly IIPService _ipService;

        private readonly ILogAggregateService _logAggregateService;

        private readonly ITempFileService _tempFileService;
        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        public BettingTicketCenterZipBuilder(
            IAppSettings appSettings,
            ITempFileService tempFileService,
            IBettingTicketCenterExcel bettingTicketCenterExcel,
            IIPService ipService,
            ILogAggregateService logAggregateService
        )
        {
            _logAggregateService = logAggregateService;
            _appSettings = appSettings ?? new AppSettings();
            _ipService = ipService;
            _tempFileService = tempFileService ?? new TempFileService(_appSettings, null);
            _bettingTicketCenterExcel = bettingTicketCenterExcel ?? new BettingTicketCenterExcel(null, _ipService, _logAggregateService);
        }

        public IBettingTicketCenterExcel ExcelBuilder
        {
            get { return _bettingTicketCenterExcel ?? new BettingTicketCenterExcel(null, _ipService, _logAggregateService); }
            set { _bettingTicketCenterExcel = value; }
        }

        public MemoryStream BuildZip(
            HttpResponseBase response,
            GroupTicketResult ticketResult,
            string title,
            AdvancedSearchBetListMetaModel metaModel,
            string totalText)
        {
            var excelFiles = new List<string>();

            for (int i = 0; i < ticketResult.DataPath.Count; i++)
            {
                _logAggregateService.Append($"Start export file {i}");
                var contentData = _tempFileService.ReadString(ticketResult.DataPath[i]);
                var excelModel = CreateExcelModel(ticketResult, contentData, title, metaModel, totalText);
                if (excelModel != null)
                {
                    var workbook = ExcelBuilder.Export(excelModel);
                    var dataFileName = ticketResult.DataPath[i] + ".dat";
                    var dataFilePath = Path.Combine(_appSettings.TempFilePath, dataFileName);

                    SaveFile(workbook, dataFilePath);

                    excelFiles.Add(dataFileName);
                    BetListHelper.ClearData(excelModel.Data);
                    excelModel.Data = null;
                    ExcelBuilder.Dispose();
                    ExcelBuilder = null;
                }

                _logAggregateService.Append($"Done export file {i}");
            }

            CleanDataFile(ticketResult.DataPath);
            return CreateZipFile(excelFiles);
        }

        protected internal virtual void SaveFile(IWorkbook workbook, string dataFilePath)
        {
            using (var stream = new FileStream(dataFilePath, FileMode.CreateNew, FileAccess.Write))
            using (var bufferStream = new BufferedStream(stream))
            {
                workbook.Write(bufferStream, true);
            }
        }

        private static BettingTicketCenterExcelModel CreateExcelModel(
            GroupTicketResult groupData,
            string contentData,
            string title,
            AdvancedSearchBetListMetaModel metaModel,
            string totalText)
        {
            if (string.IsNullOrEmpty(contentData))
            {
                return null;
            }

            var dataBySheet = JsonConvert.DeserializeObject<TicketResult>(contentData);

            dataBySheet.SumStake = groupData.SumStake;
            dataBySheet.SumWinLoss = groupData.SumWinLoss;
            dataBySheet.SumLicenseeWL = groupData.SumLicenseeWL;
            dataBySheet.SumBetEffective = groupData.SumBetEffective;
            dataBySheet.SumComm = groupData.SumComm;
            dataBySheet.SumPlayerComm = groupData.SumPlayerComm;
            dataBySheet.TotalRows = groupData.TotalRows;

            return new BettingTicketCenterExcelModel
            {
                Data = dataBySheet,
                Option = new BettingTicketCenterOption
                {
                    ShowTotal = true,
                    ShowSumStake = true,
                    ShowLicenseeWl = true,
                    ShowWinLoss = true,
                    ShowExternalId = true,
                    ShowSystemId = true,
                    IsAllowedCustomerComm = metaModel.HasPlayerCom,
                    ShowTxId = metaModel.IsAllowTxIDColumn,
                    IsShowIpvn = metaModel.IsShowIpvn,
                    IsShowIp = metaModel.IsShowIp
                },
                Title = title,
                TotalText = totalText
            };
        }

        private void CleanDataFile(IList<string> dataPath)
        {
            foreach (var item in dataPath)
            {
                _tempFileService.Delete(item);
            }
        }

        private MemoryStream CreateZipFile(IList<string> excelFiles)
        {
            var mem = new MemoryStream();
            using (var zip = new ZipFile())
            {
                for (var i = 0; i < excelFiles.Count; i++)
                {
                    zip.AddEntry($"sheet{i}.xls", _tempFileService.ReadBytes(excelFiles[i]));
                }

                CleanDataFile(excelFiles);
                zip.ParallelDeflateThreshold = -1;
                zip.Save(mem);
            }

            return mem;
        }
    }
}