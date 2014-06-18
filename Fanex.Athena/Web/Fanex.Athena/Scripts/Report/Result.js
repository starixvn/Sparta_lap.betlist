/*global Site*/
/// <reference path="/@JSense.js" />
var result = (function () {
    "use strict";

    var api = "Report/Result";
    var apiExcel = "Report/ResultExcel?";
    var apiSport = "Report/GetSports";
    var kenoResultApi = "Report/KenoResult";
    var ajaxRequest = null;
    var ajaxFilterRequest = null;
    var kenoAjaxRequest = null;
    var kenoIntervalId = 0;
    var kenoUserId = 0;
    var param =
        {
            FromDate: "",
            ToDate: "",
            IsOutRight: false,
            IsExportExcel: false,
            SportId: 0,
            LeagueId: 0
        };
    var showOutRight = function (isShow) {
        if (isShow) {
            $(".for-out-right").show();
            $("#submit-outright").attr("disabled", "disabled");
            $("#submit-normal").removeAttr("disabled");
        } else {
            $(".for-out-right").hide();
            $("#submit-outright").removeAttr("disabled");
            $("#submit-normal").attr("disabled", "disabled");
        }
        $("#submit-outright").attr("data-isOutright", isShow ? "1" : "0");
    };
    var showKeno = function () {
        if ($("#submit-keno").length) {
            $("#submit-keno").click(function () {
                var kenoWindow = window.open("", '_blank', 'left=20,top=20,width=950,height=650,toolbar=1,resizable=1,scrollbars,status=1');
                var option = {
                    type: "GET",
                    cache: false,
                    url: Site.resolveClientUrl(kenoResultApi),
                    error: function () { },
                    success: function (response) {
                        kenoUserId = response.UserId;
                        kenoWindow.location = response.Url;
                        clearInterval(kenoIntervalId);
                        extendKenoSession();
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (kenoAjaxRequest != null) {
                    kenoAjaxRequest.abort();
                }
                kenoAjaxRequest = $.ajax(option);
            });
        }
    };
    var extendKenoSession = function () {
        kenoIntervalId = setInterval(function () {
            var option = {
                type: "POST",
                cache: false,
                data: "userid=" + kenoUserId,
                url: Site.resolveClientUrl(kenoResultApi)
            };
            if (kenoAjaxRequest != null) {
                kenoAjaxRequest.abort();
            }
            kenoAjaxRequest = $.ajax(option);
        }, 6000);
    };
    var getData = function () {
        if (param.IsExportExcel) {
            exportExcel();
        } else {
            getFilter();
        }
    };
    var clearResult = function () {
        var defaultLeague = $('#league option[value="0"]').clone();
        $('#league option').remove();
        $('#league').append(defaultLeague);
        $("#sport option").remove();
        $("#result-page").html("");
        $("#filter-by-sport-league").hide();
    };
    var renderFilter = function (data) {
        var defaultLeague = $('#league option[value="0"]').clone();
        $('#league option').remove();
        $('#league').append(defaultLeague);
        if (data != null && data.toString().length > 1) {
            if (param.SportId === 0 && data.Sports.length > 0) {
                var firtSport = data.Sports[0].SportId;
                param.SportId = firtSport;
                $("#sport option").remove();
                //render sport
                $.each(data.Sports, function (index, value) {
                    $("#sport").append('<option value="' + value.SportId + '">' + value.SportName + '</option>');
                });
            }
            if (data.Leagues != null && data.Leagues.length > 0) {
                $.each(data.Leagues, function (index, value) {
                    $("#league").append('<option value="' + value.LeagueId + '">' + value.LeagueName + '</option>');
                });
                $("#league").val(param.LeagueId);
            }
            $("#filter-by-sport-league").show();
            $("#no-info").hide();
        } else {
            $("#sport option").remove();
            $("#filter-by-sport-league").hide();
        }
    };
    var getFilter = function () {
        var url = Site.resolveClientUrl(apiSport);
        var option = {
            type: "POST",
            dataType: "json",
            data: JSON.stringify(param),
            contentType: "application/json",
            cache: false,
            url: url,
            error: function () { },
            success: function (response) {
                renderFilter(response);
                if (response != null && response.toString().length > 1) {
                    getReportData();
                } else {
                    $("#no-info").show();
                    Site.unBlockUI();
                }
            },
            beforeSend: function () {
                Site.blockUI();
            },
            complete: function () {
            }
        };
        if (ajaxFilterRequest != null) {
            ajaxFilterRequest.abort();
        }
        if (ajaxRequest != null) {
            ajaxRequest.abort();
        }
        ajaxFilterRequest = $.ajax(option);
    };
    var exportExcel = function () {
        var url = Site.resolveClientUrl(apiExcel);
        url += "FromDate=" + param.FromDate;
        url += "&ToDate=" + param.ToDate;
        url += "&IsOutRight=" + param.IsOutRight;
        url += "&SportId=" + param.SportId;
        url += "&LeagueId=" + param.LeagueId;
        url += "&IsExportExcel=" + param.IsExportExcel;
        $('#iframe').attr('src', url);
        $('#iframe').load();
    };
    var clearExcelExport = function () {
        param.IsExportExcel = false;
        $('#iframe').attr('src', "");
    };
    var getReportData = function () {
        var url = Site.resolveClientUrl(api);
        var data = "FromDate=" + param.FromDate;
        data += "&ToDate=" + param.ToDate;
        data += "&IsOutRight=" + param.IsOutRight;
        data += "&SportId=" + param.SportId;
        data += "&LeagueId=" + param.LeagueId;
        data += "&IsExportExcel=" + param.IsExportExcel;
        var option = {
            type: "POST",
            cache: false,
            data: data,
            url: url,
            error: function () { },
            success: function (response) {
                if (response != null && response.length > 0) {
                    $("#no-info").hide();
                    $("#result-page").html("");
                    $("#result-page").html(response);
                } else {
                    $("#no-info").show();
                }
            },
            beforeSend: function () {
                Site.blockUI();
            },
            complete: function () {
                Site.unBlockUI();
            }
        };
        if (ajaxRequest != null) {
            ajaxRequest.abort();
        }
        ajaxRequest = $.ajax(option);
    };

    return {
        init: function () {
            var isShowedRight = self.location.hash.substr(1) === "outright";
            showOutRight(isShowedRight);
            showKeno();
            $("#submit-outright").click(function () {
                clearExcelExport();
                clearResult();
                var data = $(this).attr("data-isoutright");
                showOutRight(data === "0");
                $("#SubmitButton").trigger("click");
            });
            $("#submit-normal").click(function () {
                clearExcelExport();
                clearResult();
                showOutRight(false);
                $("#SubmitButton").trigger("click");
            });
            $("#SubmitButton").click(function () {
                clearExcelExport();
                clearResult();
                var data = $("#submit-outright").attr("data-isoutright");
                param.FromDate = $("#FromDate").val();
                param.ToDate = $("#ToDate").val();
                param.IsExportExcel = false;
                param.IsOutRight = data === "1";
                param.SportId = 0;
                param.LeagueId = 0;
                getData();
            });
            $("#submit-todate").click(function () {
                clearExcelExport();
                clearResult();
                var date = $(this).attr("data-date");
                var outright = $("#submit-outright").attr("data-isoutright");
                param.FromDate = date;
                param.ToDate = date;
                param.IsExportExcel = false;
                param.IsOutRight = outright === "1";
                param.SportId = 0;
                param.LeagueId = 0;

                $('#FromDate').datepicker('setDate', date);
                $('#ToDate').datepicker('setDate', date);
                getData();
            });
            $("#submit-yesterdate").click(function () {
                clearExcelExport();
                clearResult();
                var date = $(this).attr("data-date");
                var outright = $("#submit-outright").attr("data-isoutright");
                param.FromDate = date;
                param.ToDate = date;
                param.IsExportExcel = false;
                param.IsOutRight = outright === "1";
                param.SportId = 0;
                param.LeagueId = 0;

                $('#FromDate').datepicker('setDate', date);
                $('#ToDate').datepicker('setDate', date);

                getData();
            });
            $("#ExportToExcel").click(function (e) {
                e.preventDefault();

                var data = $("#submit-outright").attr("data-isoutright");
                param.FromDate = $("#FromDate").val();
                param.ToDate = $("#ToDate").val();
                param.IsExportExcel = true;
                param.IsOutRight = data === "1";
                param.SportId = $("#sport option").length > 0 ? $("#sport").val() : 0;
                param.LeagueId = $("#league").val();
                getData();
            });
            $("#sport").change(function () {
                clearExcelExport();
                param.SportId = $(this).val();
                param.LeagueId = 0;
                getData();
            });
            $("#league").change(function () {
                clearExcelExport();
                param.LeagueId = $(this).val();
                getReportData();
            });
            param.FromDate = $("#hiddenToDate").val();
            param.ToDate = $("#hiddenToDate").val();
        }
    };
} ());
$(document).ready(function () {
    "use strict";
    result.init();
});