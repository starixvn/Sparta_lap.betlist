/*global Site, QueryString*/
/// <reference path="/@JSense.js" />
"use strict";

var paging = function () {
    this.pageGo = function (method, index) {
        /// <summary>
        ///
        /// </summary>
        /// <param name="method">POST or GET or AJAX</param>
        /// <param name="index"></param>
        if (method === "POST") {
            $("#IsExportExcel").val(false);
            var form = $("#" + $("div.pagingContainer input[name='FormName']").val());
            var size = parseInt($("div.pagingContainer input[name='PageSize']").val(), 10);
            var pageIndex = '<input type="hidden" value="' + index + '" name="PageIndex" />';
            var pageSize = '<input type="hidden" value="' + size + '" name="PageSize" />';
            form.append(pageIndex);
            form.append(pageSize);
            Site.blockUI();
            form.submit();
        }
        else if (method === "AJAX") {
            $("div.pagingContainer input[name='PageIndex']").val(index);
            var fc = $("div.pagingContainer input[name='AjaxPagingFunction']").val();
            eval(fc); // jshint ignore:line
        } else {
            var pagesize = $("div.pagingContainer input[name='PageSize']").val();
            var url = location.href;
            url = QueryString.setParam(url, "PageSize", pagesize);
            url = QueryString.setParam(url, "PageIndex", index);

            location.href = url;
        }
    };
};
var pagingHeper = (function () {
    var me = {};
    var setting =
        {
            pageindex: 1,
            pagesize: 50,
            pagelist: [50, 100, 1000],
            totalrow: 1,
            formId: "formreport",
            elementcontainer: "#page",
            method: "AJAX",
            ajaxfunction: "",
            alwayshow: false
        };
    me.render = function (option) {
        /// <summary>
        ///
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="totalrow"></param>
        /// <param name="elementcontainer">ex: "#page"</param>
        /// <param name="method">POST or GET or AJAX</param>
        var pageHtml = "";
        $.extend(setting, option);
        var noOfPage = 1;
        if (setting.totalrow > setting.pagesize) {
            noOfPage = parseInt(setting.totalrow / setting.pagesize
         + (setting.totalrow % setting.pagesize === 0 ? 0 : 1));
        }
        var pagingVisible = setting.totalrow > setting.pagesize;
        if (pagingVisible || setting.alwayshow) {
            var prevPageDisabledHtml = (setting.pageindex === 1 ? ' pagingDisable" disabled="disabled"' : '"') + '/>';
            var pageControl = ['<div class="pagingContainer">',
                          '<input type="button" class="icon pagingFirst' + prevPageDisabledHtml,
                          '<input type="button" class="icon pagingPrev' + prevPageDisabledHtml,
                          '<span class="pagingSeperator"></span>' + Site.getSingleResource("Page")];

            var pageIndexStr = '<input  type="text" name="PageIndex"'
            + ' class="pagingCurrent" maxlength="4" size="2" value="' + setting.pageindex + '" />'
            + Site.getSingleResource("of") + "&nbsp;" + noOfPage;

            pageControl.push(pageIndexStr);
            pageControl.push('<input type="hidden" name="PageSize" value="' + setting.pagesize + '" />');
            pageControl.push('<input type="hidden" name="Method" value="' + setting.method + '" />');
            pageControl.push('<input type="hidden" name="FormName" value="' + setting.formId + '" />');
            pageControl.push('<input type="hidden" name="NoOfPage" value="' + noOfPage + '" />');
            pageControl.push('<input type="hidden" name="AjaxPagingFunction" value="' + setting.ajaxfunction + '" />');
            pageControl.push('<span class="pagingSeperator"></span>');
            var pageNext = '<input type="button" class="icon pagingNext'
            + (setting.pageindex === noOfPage ? ' pagingDisable" disabled="disabled"' : '"') + '/>';
            pageControl.push(pageNext);
            var pageLast = '<input type="button" class="icon pagingLast'
            + (setting.pageindex === noOfPage ? ' pagingDisable" disabled="disabled"' : '"') + '/>';
            pageControl.push(pageLast);
            pageControl.push('<select class="pageSizeSelected">');
            var pageListLength = setting.pagelist.length;
            for (var i = 0; i < pageListLength; i++) {
                var optionHtml = '<option value="' + setting.pagelist[i]
                + '"' + (setting.pagesize === setting.pagelist[i] ? ' selected="selected"' : '')
                + '>' + setting.pagelist[i] + '</option>';
                pageControl.push(optionHtml);
            }
            pageControl.push('</select> </div>');
            pageHtml = pageControl.join("");
        }
        $(setting.elementcontainer).html(pageHtml);
        me.bindEvent();
    };
    me.getSetting = function () {
        setting.pageindex = parseInt($("div.pagingContainer input[name='PageIndex']").val(), 10);
        setting.pagesize = parseInt($("div.pagingContainer input[name='PageSize']").val(), 10);
        return setting;
    };
    me.bindEvent = function () {
        var pagingControl = new paging();

        $("div.pagingContainer input.pagingFirst").on("click", function () {
            var method = $('div.pagingContainer input[name="Method"]').val();
            pagingControl.pageGo(method, 1);
        });
        $("div.pagingContainer input.pagingPrev").on("click", function () {
            var method = $('div.pagingContainer input[name="Method"]').val();
            var currentPage = parseInt($("div.pagingContainer input.pagingCurrent").val(), 10);
            if (currentPage > 1) {
                pagingControl.pageGo(method, currentPage - 1);
            }
        });
        $("div.pagingContainer input.pagingLast").on("click", function () {
            var method = $('div.pagingContainer input[name="Method"]').val();
            pagingControl.pageGo(method, parseInt($('div.pagingContainer input[name="NoOfPage"]').val(), 10));
        });
        $("div.pagingContainer input.pagingNext").on("click", function () {
            var method = $('div.pagingContainer input[name="Method"]').val();
            var currentPage = parseInt($("div.pagingContainer input.pagingCurrent").val(), 10);
            pagingControl.pageGo(method, currentPage + 1);
        });

        $("div.pagingContainer select.pageSizeSelected").on("change", function () {
            var method = $('div.pagingContainer input[name="Method"]').val();
            $("div.pagingContainer input[name='PageSize']").val($(this).val());
            pagingControl.pageGo(method, 1);
        });
        $("div.pagingContainer input.pagingCurrent").on("keyup", function (e) {
            var method = $('div.pagingContainer input[name="Method"]').val();
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code === 13) {
                var value = parseInt($(this).val());
                var limit = parseInt($('div.pagingContainer input[name="NoOfPage"]').val(), 10);
                if (value > 0 && value <= limit) {
                    $(this).val(value);
                    pagingControl.pageGo(method, value);
                }
            }
        });
    };

    return me;
})();

$(document).ready(function () {
    pagingHeper.bindEvent();
});