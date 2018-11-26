"use strict";
define(["jquery", "common/site", "common/querystring"], function ($, Site, QueryString) {
    var me = {};
    var _settings = {}; // store multi setting instances by "elementcontainer" as key

    var defaultSetting = {
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

    function getByName($pagerContainer, name){
        return $("input[name='" + name + "']", $pagerContainer);
    }

    function goToPage($pagerContainer, method, index) {
        var $pageSize = getByName($pagerContainer, "PageSize");
        if (method === "POST") {
            $("#IsExportExcel").val(false);
            var formName = getByName($pagerContainer, "FormName").val();
            var form = $("#" + formName);
            var pageIndex = '<input type="hidden" value="' + index + '" name="PageIndex" />';
            var pageSize = '<input type="hidden" value="' + $pageSize.val() + '" name="PageSize" />';
            form.append(pageIndex);
            form.append(pageSize);
            Site.blockUI();
            form.submit();
        }
        else if (method === "AJAX") {
            var $pageIndex = getByName($pagerContainer, "PageIndex");
            $pageIndex.val(index);
            var fc = getByName($pagerContainer, "AjaxPagingFunction").val();
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
        var setting = $.extend({}, defaultSetting, option);
        _settings[setting.elementcontainer] = setting;
        var noOfPage = 1;
        var pagingVisible = setting.totalrow > setting.pagesize;

        if (pagingVisible) {
            noOfPage = parseInt(setting.totalrow / setting.pagesize
         + (setting.totalrow % setting.pagesize === 0 ? 0 : 1));
        }

        if (pagingVisible || setting.alwayshow) {
            var prevPageDisabledHtml = (setting.pageindex === 1 ? ' pagingDisable"' : '"') + '></a>';
            var pageControl = ['<div class="pagingContainer center-align">',
                          '<a href="javascript:;" class="icon pagingFirst' + prevPageDisabledHtml,
                          '<a href="javascript:;" class="icon pagingPrev' + prevPageDisabledHtml,
                          '<span class="pagingSeperator"></span>'];

            var pagingInfo = '<div class="paging-info">' + Site.getSingleResource("Page") +
                '<input  type="text" name="PageIndex" class="pagingCurrent paging-form" maxlength="4" size="2" value="' + setting.pageindex + '" />'
            + "&nbsp;" + Site.getSingleResource("of") + "&nbsp;" + noOfPage + '</div>';

            pageControl.push(pagingInfo);

            pageControl.push('<span class="pagingSeperator"></span>');

            var nextPageDisabled = (setting.pageindex === noOfPage ? ' pagingDisable"' : '"') + '></a>';
            var pageNext = '<a href="javascript:;" class="icon pagingNext' + nextPageDisabled;
            pageControl.push(pageNext);
            var pageLast = '<a href="javascript:;" class="icon pagingLast' + nextPageDisabled;
            pageControl.push(pageLast);

            var pageListLength = setting.pagelist.length;
            var pageSizeSelectHtml = '<select class="pageSizeSelected">';
            var pageSize = parseInt(setting.pagesize);
            for (var i = 0; i < pageListLength; i++) {
                pageSizeSelectHtml += '<option value="' + setting.pagelist[i]
                + '"' + (pageSize === setting.pagelist[i] ? ' selected="selected"' : '')
                + '>' + setting.pagelist[i] + '</option>';
            }
            pageSizeSelectHtml += '</select>';
            pageControl.push(pageSizeSelectHtml);

            pageControl.push('<input type="hidden" name="PageSize" value="' + setting.pagesize + '" />');
            pageControl.push('<input type="hidden" name="Method" value="' + setting.method + '" />');
            pageControl.push('<input type="hidden" name="FormName" value="' + setting.formId + '" />');
            pageControl.push('<input type="hidden" name="NoOfPage" value="' + noOfPage + '" />');
            pageControl.push('<input type="hidden" name="AjaxPagingFunction" value="' + setting.ajaxfunction + '" />');

            pageControl.push('</div>');
            pageHtml = pageControl.join("");
        }

        var $pagerContainer = $(setting.elementcontainer);
        $pagerContainer.html(pageHtml);
        me._bindEvent(setting);
    };
    me.getSetting = function (elementcontainer) {
        var elementcontainer = elementcontainer || Object.keys(_settings)[0];
        var setting = _settings[elementcontainer];
        var $pagerContainer = $(elementcontainer);
        var $pageIndex = getByName($pagerContainer, "PageIndex");
        var $pageSize = getByName($pagerContainer, "PageSize");
        setting.pageindex = parseInt($pageIndex.val(), 10);
        setting.pagesize = $pageSize.val();
        return setting;
    };

    // private methods
    me._bindEvent = function (setting) {
        var $pagerContainer = $(setting.elementcontainer);
        var $pageIndex = getByName($pagerContainer, "PageIndex");
        var method = getByName($pagerContainer, "Method").val();
        var noOfPage = parseInt($('input[name="NoOfPage"]', $pagerContainer).val(), 10);

        $(".pagingFirst", $pagerContainer).on("click", function () {
            if (isVisible(this)) {
                goToPage($pagerContainer, method, 1);
            }
        });
        $(".pagingPrev", $pagerContainer).on("click", function () {
            var currentPage = parseInt($pageIndex.val(), 10);
            if (isVisible(this) && currentPage > 1) {
                goToPage($pagerContainer, method, currentPage - 1);
            }
        });
        $(".pagingLast", $pagerContainer).on("click", function () {
            if (isVisible(this)) {
                goToPage($pagerContainer, method, noOfPage);
            }
        });
        $(".pagingNext", $pagerContainer).on("click", function () {
            if (isVisible(this)) {
                var currentPage = parseInt($pageIndex.val(), 10);
                goToPage($pagerContainer, method, currentPage + 1);
            }
        });

        $("select.pageSizeSelected", $pagerContainer).on("change", function () {
            var $pageSize = getByName($pagerContainer, "PageSize");
            var pageSize = $(this).val();
            $pageSize.val(pageSize);
            setting.pagesize = pageSize;
            goToPage($pagerContainer, method, 1);
        });

        $pageIndex.on("keyup", function (e) {
            var code = e.keyCode ? e.keyCode : e.which;
            if (code === 13) {
                var value = parseInt($(this).val());
                if (value > 0 && value <= noOfPage) {
                    $(this).val(value);
                    goToPage($pagerContainer, method, value);
                }
            }
        });
    };
    return me;
});