/// <reference path="/@JSense.js" />
"use strict";
define(
    ['jquery', 'common/site', 'common/querystring', './betlist'],
    function ($, Site, QueryString, BetList) {

        var _me = {
            initialize: function () {
                _me.initEvent();
                _me.submit();
                Site.addUserGuide({
                    resourceName: $('#userGuideName').val(),
                    title: Site.getSingleResource("TotalBets") + " - " + $('#title').val()
                });
            },

            initEvent: function () {
                $('#SubmitButton').click(_me.submit);
                $('#mixparlay_link').click(function () {
                    Site.redirect(Site.resolveClientUrl('RunningBettypesBetList/MixParlay'));
                });
                $('#systemparlay_link').click(function () {
                    Site.redirect(Site.resolveClientUrl('RunningBettypesBetList/SystemParlay'));
                });
            },

            submit: function () {
                BetList.initialize();
                var actionName = window.location.pathname.split('/').pop();
                var params = Site.setLicenseeOptionParam();
                var url = Site.resolveClientUrl('api/RunningBettypesBetList/' + actionName);

                BetList.loadData(params, url);
            },

            showCombMPDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
                if (Number(custId) === 0) {
                    return;
                }
                var url = Site.resolveClientUrl("_BetList/Popup/ViewSystemParlay.aspx");
                url = QueryString.setParam(url, "transid", transId);
                url = QueryString.setParam(url, "refno", refNo);
                url = QueryString.setParam(url, "betteam", betTeam);
                url = QueryString.setParam(url, "betid", betId);
                url = QueryString.setParam(url, "custid", custId);
                url = QueryString.setParam(url, "basecurrency", baseCurrency);

                Site.blockUI();
                $("#popupSystemParlay").load(url).dialog();
            },

            exportToExcel: function () {
                $("#ExportToExcel").click(function () {
                    var url = window.location.href;
                    url = Site.setLicenseeOptionParam(url);
                    url = QueryString.setParam(url, "IsExportExcel", true);
                    var sortParams = BetList.getSortParams();
                    if (sortParams) {
                        url = QueryString.setParam(url, "SortBy", sortParams.SortBy);
                        url = QueryString.setParam(url, "SortOrder", sortParams.SortOrder);
                    }
                    Site.redirect(url);
                });
            }
        };

        return _me;
    });