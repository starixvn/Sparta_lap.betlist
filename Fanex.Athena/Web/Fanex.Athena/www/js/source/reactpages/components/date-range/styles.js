export const defaultClasses = {
    calendar: 'rdr-Calendar',
    dateRange: 'rdr-DateRange',
    predefinedRanges: 'rdr-PredefinedRanges',
    predefinedRangesItem: 'rdr-PredefinedRangesItem',
    predefinedRangesItemActive: 'rdr-PredefinedRangesItemActive',
    monthAndYear: 'rdr-MonthAndYear',
    weekDays: 'rdr-WeekDays',
    weekDay: 'rdr-WeekDay',
    days: 'rdr-Days',
    day: 'rdr-Day',
    dayActive: 'is-selected',
    dayPassive: 'is-passive',
    dayInRange: 'is-inRange',
    monthAndYearWrapper: 'rdr-MonthAndYear-innerWrapper',
    prevButton: 'rdr-MonthAndYear-button prev',
    nextButton: 'rdr-MonthAndYear-button next',
    month: 'rdr-MonthAndYear-month',
    monthAndYearDivider: 'rdr-MonthAndYear-divider',
    year: 'rdr-MonthAndYear-year',
    daySunday: 'rdr-Sunday',
    daySpecialDay: 'rdr-SpecialDay',
    rangeButtonGroup: 'range-button-group',
    applyButton: 'rdr-applyBtn',
    cancelButton: 'rdr-cancelBtn'
};

const defaultTheme = {
    DateRange: {
        display: 'none',
        boxSizing: 'border-box',
        background: '#ffffff',
        borderRadius: '2px',
        position: "absolute",
        width: 735,
        zIndex: 1,
        top: 60
    },
    Calendar: {
        width: 280,
        padding: 10,
        background: '#ffffff',
        borderRadius: '2px',
        display: 'inline-block',
        boxSizing: 'border-box',
        letterSpacing: 0,
        color: '#000000',
    },

    Day: {
        boxSizing: 'border-box',
        display: 'inline-block',
        letterSpacing: 'initial',
        textAlign: 'center',
        fontSize: 12,
        cursor: 'pointer',
        transition: 'transform .1s ease',
    },

    DayPassive: {
        opacity: 0.4,
        cursor: 'normal'
    },

    DayHover: {
        background: '#bdc3c7',
    },

    DayToday: {},

    DaySunday: {},

    DaySpecialDay: {},

    DayActive: {
        background: '#95a5a6',
        color: '#ffffff',
        transform: 'scale(0.9)',
    },

    DaySelected: {
        background: '#e74c3c',
        color: '#ffffff',
    },

    DayStartEdge: {},

    DayEndEdge: {},

    DayInRange: {
        background: '#34495e',
        color: '#95a5a6',
    },

    Weekday: {
        boxSizing: 'border-box',
        display: 'inline-block',
        letterSpacing: 'initial',
        textAlign: 'center',
        fontSize: 12,
        fontWeight: '600',
        marginBottom: 1
    },

    MonthAndYear: {
        textAlign: 'center',
        boxSizing: 'border-box',
        fontSize: 12,
        padding: '10px 0',
        height: 38,
        lineHeight: '18px'
    },

    MonthButton: {
        display: 'block',
        boxSizing: 'border-box',
        height: 18,
        width: 18,
        padding: 0,
        margin: '0 10px',
        border: 'none',
        background: '#bdc3c7',
        boxShadow: 'none',
        outline: 'none',
        borderRadius: '50%',
    },

    MonthArrow: {
        display: 'block',
        width: 0,
        height: 0,
        padding: 0,
        margin: 0,
        border: '4px solid transparent',
        textAlign: 'center'
    },

    MonthArrowPrev: {
        borderRightWidth: '6px',
        borderRightColor: '#34495e',
        marginLeft: 1,
    },

    MonthArrowNext: {
        borderLeftWidth: '6px',
        borderLeftColor: '#34495e',
        marginLeft: 7,
    },

    PredefinedRanges: {
        width: 160,
        display: 'inline-block',
        verticalAlign: 'top',
        marginLeft: 10,
        marginTop: 10
    },

    PredefinedRangesItem: {
        display: 'block',
        fontSize: 12,
        color: '#2c3e50',
        padding: '10px 14px',
        borderRadius: '4px',
        textDecoration: 'none',
        marginBottom: 6,
    },

    PredefinedRangesItemActive: {
        background: '#4c6375',
        color: '#fff',
    },

    RangeApplyButton: {
        marginRight: 'calc(4%)',
        border: 'none',
        borderRadius: '2px',
        boxShadow: '0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 3px 1px -2px rgba(0, 0, 0, 0.2), 0 1px 5px 0 rgba(0, 0, 0, 0.12)',
        height: 25,
        lineHeight: '25px',
        textAlign: 'center',
        letterSpacing: '0.5px',
        display: 'inline-block',
        outline: 0,
        verticalAlign: 'middle',
        width: '48%',
        color: '#fff',
        backgroundColor: '#f05b1f'
    },

    RangeCancelButton: {
        border: 'none',
        borderRadius: '2px',
        boxShadow: '0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 3px 1px -2px rgba(0, 0, 0, 0.2), 0 1px 5px 0 rgba(0, 0, 0, 0.12)',
        height: 25,
        lineHeight: '25px',
        textAlign: 'center',
        letterSpacing: '0.5px',
        display: 'inline-block',
        outline: 0,
        verticalAlign: 'middle',
        width: '48%',
        color: '#fff',
        backgroundColor: '#757985'
    }
};

export default (customTheme = {}) => {

    let calendarWidth = defaultTheme.Calendar.width;
    let calendarPadding = defaultTheme.Calendar.padding;
    let cellMargin = defaultTheme.Day.margin || 0;

    if (customTheme.Calendar && customTheme.Calendar.hasOwnProperty('width')) {
        calendarWidth = customTheme.Calendar.width;
    }

    if (customTheme.Calendar && customTheme.Calendar.hasOwnProperty('padding')) {
        calendarPadding = customTheme.Calendar.padding;
    }

    if (customTheme.Day && customTheme.Day.hasOwnProperty('margin')) {
        cellMargin = customTheme.Day.margin;
    }

    const cellSize = ((parseInt(calendarWidth) - parseInt(calendarPadding) * 2) / 7) - (parseInt(cellMargin) * 2);

    return {
        DateRange: {...defaultTheme.DateRange, ...customTheme.DateRange },

        Calendar: {...defaultTheme.Calendar, ...customTheme.Calendar },

        Day: {
            width: cellSize,
            height: cellSize,
            lineHeight: cellSize + 'px',
            ...defaultTheme.Day,
            ...customTheme.Day,
        },

        DayPassive: {...defaultTheme.DayPassive, ...customTheme.DayPassive },

        DayHover: {...defaultTheme.DayHover, ...customTheme.DayHover },

        DayToday: {...defaultTheme.DayToday, ...customTheme.DayToday },
        DaySunday: {...defaultTheme.DaySunday, ...customTheme.DaySunday },
        DaySpecialDay: {...defaultTheme.DaySpecialDay, ...customTheme.DaySpecialDay },

        DayActive: {...defaultTheme.DayActive, ...customTheme.DayActive },

        DaySelected: {...defaultTheme.DaySelected, ...customTheme.DaySelected },

        DayStartEdge: {...defaultTheme.DayStartEdge, ...customTheme.DayStartEdge },

        DayEndEdge: {...defaultTheme.DayEndEdge, ...customTheme.DayEndEdge },

        DayInRange: {...defaultTheme.DayInRange, ...customTheme.DayInRange },

        Weekday: {
            width: cellSize,
            height: cellSize / 2,
            lineHeight: cellSize / 2 + 'px',
            ...defaultTheme.Weekday,
            ...customTheme.Weekday,
        },

        MonthAndYear: {...defaultTheme.MonthAndYear, ...customTheme.MonthAndYear },

        MonthButton: {...defaultTheme.MonthButton, ...customTheme.MonthButton },

        MonthArrow: {...defaultTheme.MonthArrow, ...customTheme.MonthArrow },

        MonthArrowPrev: {...defaultTheme.MonthArrowPrev, ...customTheme.MonthArrowPrev },

        MonthArrowNext: {...defaultTheme.MonthArrowNext, ...customTheme.MonthArrowNext },

        PredefinedRanges: {...defaultTheme.PredefinedRanges, ...customTheme.PredefinedRanges },

        PredefinedRangesItem: {...defaultTheme.PredefinedRangesItem, ...customTheme.PredefinedRangesItem },

        PredefinedRangesItemActive: {...defaultTheme.PredefinedRangesItemActive, ...customTheme.PredefinedRangesItemActive },

        RangeApplyButton: {...defaultTheme.RangeApplyButton, ...customTheme.RangeApplyButton },

        RangeCancelButton: {...defaultTheme.RangeCancelButton, ...customTheme.RangeCancelButton }
    }
}