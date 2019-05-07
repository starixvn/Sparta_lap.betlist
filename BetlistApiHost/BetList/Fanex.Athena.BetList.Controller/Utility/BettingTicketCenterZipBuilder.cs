using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Excel.Builders;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Ionic.Zip;
using Newtonsoft.Json;
using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Controller.Utility
{
    public class BettingTicketCenterZipBuilder : IBettingTicketCenterZipBuilder
    {
        private readonly IAppSettings _appSettings;

        private readonly ITempFileService _tempFileService;

        public BettingTicketCenterZipBuilder(
            IAppSettings appSettings,
            ITempFileService tempFileService)
        {
            _appSettings = appSettings;
            _tempFileService = tempFileService;
        }

        public ZipFile BuildZip(HttpResponseBase response, GroupTicketResult ticketResult, string title, bool hasPlayerComm, string totalText)
        {
            var excelFiles = new List<string>();

            for (int i = 0; i < ticketResult.DataPath.Count; i++)
            {
                if (response.IsClientConnected)
                {
                    using (var excelService = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>())
                    {
                        var contentData = _tempFileService.ReadString(ticketResult.DataPath[i]);
                        var excelModel = CreateExcelModel(ticketResult, contentData, title, hasPlayerComm, totalText);
                        if (excelModel != null)
                        {
                            var workbook = excelService.Export(excelModel);
                            var dataFileName = ticketResult.DataPath[i] + ".dat";
                            var dataFilePath = Path.Combine(_appSettings.TempFilePath, dataFileName);

                            SaveFile(workbook, dataFilePath);

                            excelFiles.Add(dataFileName);
                            BetListHelper.ClearData(excelModel.Data);
                            excelModel.Data = null;
                        }
                    }
                }
            }

            CleanDataFile(ticketResult.DataPath);
            return CreateZipFile(excelFiles, response);
        }

        private static BettingTicketCenterExcelModel CreateExcelModel(
            GroupTicketResult groupData,
            string contentData,
            string title,
            bool hasPlayerComm,
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
                Option = new BettingTicketCenterExcelOption
                {
                    ShowTotal = true,
                    ShowSumStake = true,
                    ShowLicenseeWl = true,
                    ShowWinLoss = true,
                    TotalText = totalText
                },
                CustomerCommVisible = hasPlayerComm,
                Title = title
            };
        }

        protected internal virtual void SaveFile(IWorkbook workbook, string dataFilePath)
        {
            using (var stream = new FileStream(dataFilePath, FileMode.CreateNew, FileAccess.Write))
            using (var bufferStream = new BufferedStream(stream))
            {
                workbook.Write(bufferStream);
                workbook.Clear();
            }
        }

        private void CleanDataFile(IList<string> dataPath)
        {
            foreach (var item in dataPath)
            {
                _tempFileService.Delete(item);
            }
        }

        private ZipFile CreateZipFile(IList<string> excelFiles, HttpResponseBase response)
        {
            using (var zip = new ZipFile())
            {
                for (var i = 0; i < excelFiles.Count; i++)
                {
                    if (response.IsClientConnected)
                    {
                        zip.AddEntry($"sheet{i}.xls", _tempFileService.ReadBytes(excelFiles[i]));
                    }
                }

                CleanDataFile(excelFiles);
                return response.IsClientConnected ? zip : null;
            }
        }
    }
}