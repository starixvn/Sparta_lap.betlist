define(["moment", "bootstrap-daterangepicker", "lib/jquery.monthpicker", "vendor-materialize", "malihu-custom-scrollbar-plugin"], function (moment) {
    var _historyDate = new Date($("#HistoryDate").val());

    function initMaterialSelect() {
        $('select.select').not('.disabled').material_select();
    }

    function initDateRangePicker() {
        var $dateRange = $('#DateRange');

        var $from = $('#FromDate');
        var $to = $('#ToDate');
        var start = moment($from.attr('data-original-from'), "MM/DD/YYYY");
        var end = moment($to.attr('data-original-to'), "MM/DD/YYYY");

        function setRangeDate(start, end) {
            $from.val(start.format('MM/DD/YYYY'));
            $to.val(end.format('MM/DD/YYYY'));
            $dateRange.val(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
        }

        var now = moment($dateRange.attr('data-today'), "MM/DD/YYYY");

        $dateRange.daterangepicker({
            startDate: start,
            endDate: end,
            minDate: moment($dateRange.attr('data-mindate'), "MM/DD/YYYY"),
            maxDate: moment($dateRange.attr('data-maxdate'), "MM/DD/YYYY"),
            ranges: {
                'Today': [now, now],
                'Yesterday': [now.clone().subtract(1, 'days'), now.clone().subtract(1, 'days')],
                'Current week': [now.clone().startOf('week'), now],
                'Last week': [now.clone().subtract(1, 'week').startOf('week'), now.clone().subtract(1, 'week').endOf('week')],
                'Current month': [now.clone().startOf('month'), now],
                'Last Month': [now.clone().subtract(1, 'month').startOf('month'), now.clone().subtract(1, 'month').endOf('month')],
                'Since last month': [now.clone().subtract(1, 'month').startOf('month'), now]
            },
            locale: {
                format: 'MM/DD/YYYY'
            },
            showDropdowns: true,
            linkedCalendars: false,
            alwaysShowCalendars: true
        }, setRangeDate);

        $dateRange.on('apply.daterangepicker', function (ev, picker) {
            setRangeDate(picker.startDate, picker.endDate);
        });

        setRangeDate(start, end);
    }

    function initMonthPicker() {
        var fromdate = new Date($('#FromDate').val());
        var todate = new Date($('#ToDate').val());

        $('#month-picker').monPickr({
            minDate: _historyDate,
            showDateRange: true,
            fromDayId: 'fDay',
            toDayId: 'tDay',
            format: 'MM/dd/yyyy',
            month: fromdate,
            fromDay: fromdate.getDate(),
            toDay: todate.getDate(),
            afterChangeMonth: function () {
                $('#FromDate').val($("#month-picker").monPickr("getFromDate", "MM/dd/yyyy"));
                $('#ToDate').val($("#month-picker").monPickr("getToDate", "MM/dd/yyyy"));
            },
            afterChangeDay: function () {
                $('#FromDate').val($("#month-picker").monPickr("getFromDate", "MM/dd/yyyy"));
                $('#ToDate').val($("#month-picker").monPickr("getToDate", "MM/dd/yyyy"));
            }
        });
    }

    function initCustomScrollbar($target, options) {
        return options ? $target.mCustomScrollbar(options) : $target.mCustomScrollbar({ theme: 'light' });
    }

    return {
        initMaterialSelect: initMaterialSelect,
        initDateRangePicker: initDateRangePicker,
        initMonthPicker: initMonthPicker,
        initCustomScrollbar: initCustomScrollbar
    };
});