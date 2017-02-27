"use strict";
define(["jquery", "common/site", "components/pluginhelper/daterangepicker", "common/popup", "components/scrollablegrid", "extensions/jquery.extension"],
    function ($, Site, DatePicker, Popup, GridHelper) {
        var api = "ResultReport/Index";
        var apiExcel = "ResultReport/ResultExcel?";
        var apiSport = "ResultReport/GetSports";
        var kenoResultApi = "ResultReport/KenoResult";
        var ajaxRequest = null;
        var ajaxFilterRequest = null;
        var kenoAjaxRequest = null;
        var kenoIntervalId = 0;
        var kenoUserId = 0;
        var param = {
            FromDate: "",
            ToDate: "",
            IsOutRight: false,
            IsExportExcel: false,
            SportId: 0,
            LeagueId: 0
        };

        var ACTIVE_CLASS = "active";

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initSingleDatePicker("Date");
                DatePicker.initDateRangePicker();
                _me.initEvent();
            },

            initEvent: function () {
                var $submitOutright = $("#submitOutright");

                $("#resultContent").on("click", ".detail-link", function () {
                    var url = Site.resolveClientUrl("ResultReport/ResultDetail")
                     + "?matchid=" + $(this).attr("data-match-id") + "&SportType=" + param.SportId;

                    var popupOption = { id: "result-popup", width: 775, height: 520, title: Site.getSingleResource("Result") };
                    Popup.openPopupWithIframe(url, popupOption);
                });

                $submitOutright.click(function () {
                    if (_me.canSubmit(this)) {
                        _me.showOutRight(true);
                        var startDate = $("#Date").val();
                        DatePicker.setValueForDateRange(startDate);
                        $("#SubmitButton").trigger("click");
                    }
                });

                $("#submitNormal").click(function () {
                    if (_me.canSubmit(this)) {
                        _me.showOutRight(false);
                        DatePicker.setValueForSingleDate($("#Date"), $('#FromDate').val());
                        $("#SubmitButton").trigger("click");
                    }
                });

                $("#SubmitButton").click(function () {
                    _me.clearExcelExport();
                    _me.clearResult();
                    var isShowOutright = $submitOutright.attr("data-isoutright") === "1";
                    _me.setDateParam(isShowOutright);

                    param.IsExportExcel = false;
                    param.IsOutRight = isShowOutright;
                    param.SportId = 0;
                    param.LeagueId = 0;
                    _me.getData();
                });

                $("#submitToday").click(function () {
                    _me.clearExcelExport();
                    _me.clearResult();
                    var date = $(this).attr("data-date");
                    var isShowOutright = $submitOutright.attr("data-isoutright") === "1";
                    param.FromDate = date;
                    param.ToDate = date;
                    param.IsExportExcel = false;
                    param.IsOutRight = isShowOutright;
                    param.SportId = 0;
                    param.LeagueId = 0;

                    _me.getData();
                });

                $("#submitYesterday").click(function () {
                    _me.clearExcelExport();
                    _me.clearResult();
                    var date = $(this).attr("data-date");
                    var isShowOutright = $submitOutright.attr("data-isoutright") === "1";
                    param.FromDate = date;
                    param.ToDate = date;
                    param.IsExportExcel = false;
                    param.IsOutRight = isShowOutright;
                    param.SportId = 0;
                    param.LeagueId = 0;

                    _me.getData();
                });

                $("#ExportToExcel").click(function (e) {
                    e.preventDefault();
                    var isShowOutright = $submitOutright.attr("data-isoutright") === "1";
                    _me.setDateParam(isShowOutright);

                    param.IsExportExcel = true;
                    param.IsOutRight = isShowOutright;
                    param.SportId = $("#ddlSport option").length > 0 ? $("#ddlSport").val() : 0;
                    param.LeagueId = $("#ddlLeague").val();
                    _me.getData();
                });

                $("#ddlSport").change(function () {
                    _me.clearExcelExport();
                    _me.clearLeagueFilter();

                    param.SportId = $(this).val();
                    param.LeagueId = 0;
                    _me.getData();
                });

                $("#ddlLeague").change(function () {
                    _me.clearExcelExport();
                    param.LeagueId = $(this).val();
                    _me.getReportData();
                });

                if ($("#submitKeno").length) {
                    $("#submitKeno").click(function () {
                        _me.showKeno();
                    });
                }
            },

            showKeno: function () {
                var kenoWindow = window.open("", '_blank', 'left=20,top=20,width=950,height=650,toolbar=1,resizable=1,scrollbars,status=1');
                var option = {
                    type: "GET",
                    cache: false,
                    url: Site.resolveClientUrl(kenoResultApi),
                    success: function (response) {
                        kenoUserId = response.UserId;
                        kenoWindow.location = response.Url;
                        clearInterval(kenoIntervalId);
                        _me.extendKenoSession();
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (kenoAjaxRequest !== null) {
                    kenoAjaxRequest.abort();
                }
                kenoAjaxRequest = $.ajax(option);
            },

            extendKenoSession: function () {
                kenoIntervalId = setInterval(function () {
                    var option = {
                        type: "POST",
                        cache: false,
                        data: "userid=" + kenoUserId,
                        url: Site.resolveClientUrl(kenoResultApi)
                    };
                    if (kenoAjaxRequest !== null) {
                        kenoAjaxRequest.abort();
                    }
                    kenoAjaxRequest = $.ajax(option);
                }, 6000);
            },

            getData: function () {
                if (param.IsExportExcel) {
                    _me.exportExcel();
                } else {
                    _me.getFilter();
                }
            },

            exportExcel: function () {
                var url = Site.resolveClientUrl(apiExcel);
                url += "FromDate=" + param.FromDate;
                url += "&ToDate=" + param.ToDate;
                url += "&IsOutRight=" + param.IsOutRight;
                url += "&SportId=" + param.SportId;
                url += "&LeagueId=" + param.LeagueId;
                url += "&IsExportExcel=" + param.IsExportExcel;

                $('#iframeForExportExcel').attr('src', url);
            },

            getFilter: function () {
                var url = Site.resolveClientUrl(apiSport);
                var option = {
                    type: "POST",
                    dataType: "json",
                    data: param,
                    cache: false,
                    url: url,
                    success: function (response) {
                        _me.renderFilter(response);
                        if (response !== null && response.toString().length > 1) {
                            _me.getReportData();
                        } else {
                            $("#noInfoRow").show();
                            Site.unBlockUI();
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    }
                };
                if (ajaxFilterRequest !== null) {
                    ajaxFilterRequest.abort();
                }
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxFilterRequest = $.ajax(option);
            },

            getReportData: function () {
                var url = Site.resolveClientUrl(api);
                var option = {
                    type: "POST",
                    cache: false,
                    data: param,
                    url: url,
                    success: function (response) {
                        if (response !== null && response.length > 0) {
                            $("#noInfoRow").hide();
                            $("#resultContent").html(response);

                            if ($(".fn-rs-content-scroll").length > 0) {
                                GridHelper.updateHeight($(".fn-rs-content-scroll"));
                            }
                        } else {
                            $("#noInfoRow").show();
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);
            },

            renderFilter: function (data) {
                if (data) {
                    _me.renderSportFilter(data.Sports);
                    _me.renderLeagueFilter(data.Leagues);

                    $("#sportAndLeagueFilter").css("visibility", "visible");
                    $("#noInfoRow").hide();
                }
                else {
                    $("#sportAndLeagueFilter").css("visibility", "hidden");
                }
            },

            renderSportFilter: function (sportList) {
                if (param.SportId === 0 && sportList.length > 0) {
                    var firstSport = sportList[0].SportId;
                    param.SportId = firstSport;
                    $.each(sportList, function (index, value) {
                        $("#ddlSport").append('<option value="' + value.SportId + '">' + value.SportName + '</option>');
                    });
                }
            },

            renderLeagueFilter: function (leagueList) {
                if (leagueList !== null && leagueList.length > 0) {
                    $.each(leagueList, function (index, value) {
                        $("#ddlLeague").append('<option value="' + value.LeagueId + '">' + value.LeagueName + '</option>');
                    });
                    $("#ddlLeague").val(param.LeagueId);
                }
            },

            clearResult: function () {
                _me.clearLeagueFilter();
                $("#ddlSport option").remove();
                $("#resultContent").html("");
                $("#sportAndLeagueFilter").css("visibility", "hidden");
            },

            clearLeagueFilter: function () {
                var defaultLeague = $('#ddlLeague option[value="0"]').clone();
                $('#ddlLeague option').remove();
                $('#ddlLeague').append(defaultLeague);
            },

            clearExcelExport: function () {
                param.IsExportExcel = false;
                $('#iframeForExportExcel').attr('src', "");
            },

            showOutRight: function (isShow) {
                if (isShow) {
                    $("#singleDate").hide();
                    $("#dateRange").show();
                    $("#submitOutright").addClass(ACTIVE_CLASS);
                    $("#submitNormal").removeClass(ACTIVE_CLASS);
                } else {
                    $("#dateRange").hide();
                    $("#singleDate").show();
                    $("#submitOutright").removeClass(ACTIVE_CLASS);
                    $("#submitNormal").addClass(ACTIVE_CLASS);
                }
                $("#submitOutright").attr("data-isOutright", isShow ? "1" : "0");
            },

            setDateParam: function (isShowOutright) {
                if (isShowOutright) {
                    param.FromDate = $("#FromDate").val();
                    param.ToDate = $("#ToDate").val();
                }
                else {
                    param.FromDate = $("#Date").val();
                    param.ToDate = $("#Date").val();
                }
            },

            canSubmit: function (element) {
                return !$(element).hasClass(ACTIVE_CLASS);
            }
        };

        return _me;
    });