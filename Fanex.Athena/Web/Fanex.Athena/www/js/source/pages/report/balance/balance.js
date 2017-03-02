"use strict";
define(["jquery", "common/site"], function ($, Site) {
    var _me = {
        initialize: function () {
            Site.initialize();
            _me.initializeParam();
            _me.initEvent();
            _me.setHistoryDateConstraint();
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {
                _me.initializeParam();
                $('#IsExportExcel').val(true);
                $('#BalanceForm').submit();
            });

            $("#Year").change(function () {
                _me.setHistoryDateConstraint();
            });

            $('#SubmitButton').click(function () {
                _me.submit();
            });
        },

        setHistoryDateConstraint: function(){
            var historyDate = Site.getHistoryDate();
            var historyMonth = historyDate.getMonth();
            var $month = $("#Month");

            if (parseInt($("#Year").val()) === historyDate.getFullYear()) {
                if ($month.val() < historyMonth) {
                    $month.val(historyMonth);
                }

                $month.find("option").filter(function () {
                    return this.value < historyMonth;
                }).hide().prop("disabled", true);
            } else {
                $month.children().show().prop("disabled", false);
            }
        },

        initializeParam: function () {
            var month = $("#Month").val();
            var year = $("#Year").val();
            var fromDate = new Date(year, month, 1);
            var toDate = new Date(fromDate.getFullYear(), fromDate.getMonth() + 1, 0);
            $("#FromDate").val(fromDate.toDateString());
            $("#ToDate").val(toDate.toDateString());
        },
        submit: function () {
            Site.blockUI();
            _me.initializeParam();
            $('#IsExportExcel').val(false);
            $('#BalanceForm').submit();
        }
    };

    return _me;
});