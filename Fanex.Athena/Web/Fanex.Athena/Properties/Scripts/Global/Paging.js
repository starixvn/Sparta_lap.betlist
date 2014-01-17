/// <reference path="/@JSense.js" />
$(window).load(function () {
    var pagingControl = new paging();
    var method = $('div.pagingContainer input[name="Method"]').val();
    var currentPage = parseInt($("div.pagingContainer input.pagingCurrent").val(), 10);

    $("div.pagingContainer input.pagingFirst").click(function () {
        pagingControl.pageGo(method, 1);
    });
    $("div.pagingContainer input.pagingPrev").click(function () {
        if (currentPage > 1) {

            pagingControl.pageGo(method, currentPage - 1);
        }

    });
    $("div.pagingContainer input.pagingLast").click(function () {

        pagingControl.pageGo(method, parseInt($('div.pagingContainer input[name="NoOfPage"]').val(), 10));

    });
    $("div.pagingContainer input.pagingNext").click(function () {

        pagingControl.pageGo(method, currentPage + 1);

    });

    $("div.pagingContainer select.pageSizeSelected").change(function () {
        $("div.pagingContainer input[name='PageSize']").val($(this).val());
        pagingControl.pageGo(method, 1);
    });
    $("div.pagingContainer input.pagingCurrent").keyup(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            var value = parseInt($(this).val())
            var limit = parseInt($('div.pagingContainer input[name="NoOfPage"]').val(), 10);
            if (value > 0 && value <= limit) {
                $(this).val(value);
                pagingControl.pageGo(method, value);

            }
        }
    });
});
var paging = function () {
    this.pageGo = function (method, index) {

        if (method == "POST") {
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
        else {
            var pageSize = $("div.pagingContainer input[name='PageSize']").val();
            var url = location.href;
            url = QueryString.setParam(url, "PageSize", pageSize);
            url = QueryString.setParam(url, "PageIndex", index);

            location.href = url;
        }
    }
}