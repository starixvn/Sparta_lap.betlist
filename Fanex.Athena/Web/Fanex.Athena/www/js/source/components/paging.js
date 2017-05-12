"use strict";
define(["jquery", "common/site", "common/querystring"], function ($, Site, QueryString) {
    var $pagingContainer, $pageIndex, $pageSize;

    var me = {};

    var setting = {
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

    function goToPage(method, index) {
        if (method === "POST") {
            $("#IsExportExcel").val(false);
            var formName = $("input[name='FormName']", $pagingContainer).val();
            var form = $("#" + formName);
            var pageIndex = '<input type="hidden" value="' + index + '" name="PageIndex" />';
            var pageSize = '<input type="hidden" value="' + $pageSize.val() + '" name="PageSize" />';
            form.append(pageIndex);
            form.append(pageSize);
            Site.blockUI();
            form.submit();
        }
        else if (method === "AJAX") {
            $pageIndex.val(index);
            var fc = $("input[name='AjaxPagingFunction']", $pagingContainer).val();
            eval(fc); // jshint ignore:line
        } else {
            var url = Site.getCurrentUrl();
            url = QueryString.setParam(url, "PageSize", $pageSize.val());
            url = QueryString.setParam(url, "PageIndex", index);

            Site.redirect(url);
        }
    }

    function isVisible(element) {
        return !$(element).hasClass("pagingDisable");
    }

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
        var pagingVisible = setting.totalrow > setting.pagesize;

        if (pagingVisible) {
            noOfPage = parseInt(setting.totalrow / setting.pagesize
         + (setting.totalrow % setting.pagesize === 0 ? 0 : 1));
        }

        if (pagingVisible || setting.alwayshow) {
            var prevPageDisabledHtml = (setting.pageindex === 1 ? ' pagingDisable"' : '"') + '</a>';
            var pageControl = ['<div class="pagingContainer center-align">',
                          '<a href="javascript:;" class="icon pagingFirst' + prevPageDisabledHtml,
                          '<a href="javascript:;" class="icon pagingPrev' + prevPageDisabledHtml,
                          '<span class="pagingSeperator"></span>'];

            var pagingInfo = '<div class="paging-info">' + Site.getSingleResource("Page") +
                '<input  type="text" name="PageIndex" class="pagingCurrent paging-form" maxlength="4" size="2" value="' + setting.pageindex + '" />'
            + Site.getSingleResource("of") + "&nbsp;" + noOfPage;

            pageControl.push(pagingInfo);

            pageControl.push('<span class="pagingSeperator"></span>');

            var nextPageDisabled = (setting.pageindex === noOfPage ? ' pagingDisable"' : '"') + '</a>';
            var pageNext = '<a href="javascript:;" class="icon pagingNext' + nextPageDisabled;
            pageControl.push(pageNext);
            var pageLast = '<a href="javascript:;" class="icon pagingLast' + nextPageDisabled;
            pageControl.push(pageLast);

            var pageListLength = setting.pagelist.length;
            var pageSizeSelectHtml = '<select class="pageSizeSelected">';
            for (var i = 0; i < pageListLength; i++) {
                pageSizeSelectHtml += '<option value="' + setting.pagelist[i]
                + '"' + (setting.pagesize === setting.pagelist[i] ? ' selected="selected"' : '')
                + '>' + setting.pagelist[i] + '</option>';
            }
            pageSizeSelectHtml += '</select></div>';
            pageControl.push(pageSizeSelectHtml);

            pageControl.push('<input type="hidden" name="PageSize" value="' + setting.pagesize + '" />');
            pageControl.push('<input type="hidden" name="Method" value="' + setting.method + '" />');
            pageControl.push('<input type="hidden" name="FormName" value="' + setting.formId + '" />');
            pageControl.push('<input type="hidden" name="NoOfPage" value="' + noOfPage + '" />');
            pageControl.push('<input type="hidden" name="AjaxPagingFunction" value="' + setting.ajaxfunction + '" />');
            pageHtml = pageControl.join("");
        }

        $(setting.elementcontainer).html(pageHtml);
        me.initialize();
    };
    me.getSetting = function () {
        setting.pageindex = parseInt($pageIndex.val(), 10);
        setting.pagesize = $pageSize.val();
        return setting;
    };
    me.bindEvent = function () {
        var method = $('input[name="Method"]', $pagingContainer).val();
        var noOfPage = parseInt($('input[name="NoOfPage"]', $pagingContainer).val(), 10);

        $(".pagingFirst", $pagingContainer).on("click", function () {
            if (isVisible(this)) {
                goToPage(method, 1);
            }
        });
        $(".pagingPrev", $pagingContainer).on("click", function () {
            var currentPage = parseInt($pageIndex.val(), 10);
            if (isVisible(this) && currentPage > 1) {
                goToPage(method, currentPage - 1);
            }
        });
        $(".pagingLast", $pagingContainer).on("click", function () {
            if (isVisible(this)) {
                goToPage(method, noOfPage);
            }
        });
        $(".pagingNext", $pagingContainer).on("click", function () {
            if (isVisible(this)) {
                var currentPage = parseInt($pageIndex.val(), 10);
                goToPage(method, currentPage + 1);
            }
        });

        $("select.pageSizeSelected", $pagingContainer).on("change", function () {
            $pageSize.val($(this).val());
            goToPage(method, 1);
        });

        $pageIndex.on("keyup", function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code === 13) {
                var value = parseInt($(this).val());
                if (value > 0 && value <= noOfPage) {
                    $(this).val(value);
                    goToPage(method, value);
                }
            }
        });
    };

    me.initialize = function () {
        $pagingContainer = $(".pagingContainer");
        $pageIndex = $("input[name='PageIndex']", $pagingContainer);
        $pageSize = $("input[name='PageSize']", $pagingContainer);

        me.bindEvent();
    };

    return me;
});