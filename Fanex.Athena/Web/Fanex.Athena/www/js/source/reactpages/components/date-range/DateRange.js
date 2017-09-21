import React, { Component } from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
import parseInput from './utils/parseInput.js';
import Calendar from './Calendar.js';
import PredefinedRanges from './PredefinedRanges.js';
import getTheme, { defaultClasses } from './styles.js';
import joiningClassNames from 'classnames';
import { MomentFormat } from './constants';
import defaultOptions from './defaultOptions.js';

class DateRange extends Component {

  constructor(props, context) {
    super(props, context);

    const { format, linkedCalendars, theme, locale } = props;

    const startDate = parseInput(props.startDate, format, 'startOf');
    const endDate = parseInput(props.endDate, format, 'endOf');
    this.styles = getTheme(theme);
    this.step = 0;
    this.locale = { ...defaultOptions.locale, ...props.locale };
    this.ranges = { ...defaultOptions.ranges, ...props.ranges };

    this.currentRange = { startDate, endDate };
    this.isValid = true;

    this.state = {
      range: { startDate, endDate },
      link: linkedCalendars && endDate,
      show: false
    }
  }

  get dateRangeText() {
    const date = this.currentRange;
    if (date.startDate && date.endDate) {
      return `${date.startDate.format(MomentFormat.default)} - ${date.endDate.format(MomentFormat.default)}`;
    }

    return "";
  }

  show() {
    this.step = 0;
    this.setState({ show: true });
  }

  hide() {
    this.setState({ show: false });
  }

  componentDidMount() {
    const { onInit } = this.props;
    onInit && onInit(this.state.range);
  }

  orderRange(range) {
    const { startDate, endDate } = range;
    const swap = startDate.isAfter(endDate);

    if (!swap) return range;

    return {
      startDate: endDate,
      endDate: startDate
    }
  }

  setRange(range, triggerChange, show) {
    const { onChange } = this.props
    range = this.orderRange(range);
    this.isValid = this.isValidMonthRange(range);

    const callback = () => {
      triggerChange && onChange && onChange(range, this.isValid);
    }

    if (this.isValid) {
      const newState = show === undefined ? { range } : { range, show };
      this.setState(newState, callback);
    }
    else {
      onChange && onChange(range, this.isValid);
    }
  }

  handleSelect(date) {
    if (date.startDate && date.endDate) { //Select in range
      this.step = 0;
      const showDaterange = false, triggerChange = true;
      this.currentRange = Object.assign({}, date);
      return this.setRange(date, triggerChange, showDaterange);
    }

    const { startDate, endDate } = this.state.range;

    const range = {
      startDate: startDate,
      endDate: endDate
    };

    switch (this.step) {
      case 0:
        range['startDate'] = date;
        range['endDate'] = date;
        this.step = 1;
        break;

      case 1:
        range['endDate'] = date;
        this.step = 0;
        break;
    }

    const triggerChange = !this.props.twoStepChange || this.step === 0 && this.props.twoStepChange;
    this.setRange(range, triggerChange);
  }

  isValidMonthRange(date) {
    const { limitedMonthRange } = this.props;

    if (limitedMonthRange) {
      const range = date.endDate.clone().subtract(date.startDate.month(), "month");
      if (range.month() > limitedMonthRange - 1) {
        return false;
      }
    }

    return true;
  }

  handleLinkChange(newDate) {
    this.setState({
      link: newDate
    });
  }

  handleApply() {
    const { onClickApplyButton } = this.props;
    const range = this.state.range;
    onClickApplyButton && onClickApplyButton(range, this.isValid);

    this.currentRange = Object.assign({}, range);
    this.hide();
  }

  componentWillReceiveProps(newProps) {
    // Whenever date props changes, update state with parsed variant
    if (newProps.startDate || newProps.endDate) {
      const format = newProps.format || this.props.format;
      const startDate = newProps.startDate && parseInput(newProps.startDate, format, 'startOf');
      const endDate = newProps.endDate && parseInput(newProps.endDate, format, 'endOf');
      const oldStartDate = this.props.startDate && parseInput(this.props.startDate, format, 'startOf');
      const oldEndDate = this.props.endDate && parseInput(this.props.endDate, format, 'endOf');

      if (!startDate.isSame(oldStartDate) || !endDate.isSame(oldEndDate)) {
        const range = {
          startDate: startDate || oldStartDate,
          endDate: endDate || oldEndDate
        };

        this.setRange(range);
      }
    }
  }

  render() {
    const { showRange, format, linkedCalendars, style, calendars, firstDayOfWeek, minDate, maxDate, classNames, onlyClasses, specialDays, lang, disableDaysBeforeToday, offsetPositive, shownDate, showMonthArrow, rangedCalendars } = this.props;
    const { range, link } = this.state;
    const { styles } = this;

    const classes = { ...defaultClasses, ...classNames };
    const yearsDiff = range.endDate.year() - range.startDate.year();
    const monthsDiff = range.endDate.month() - range.startDate.month();
    const diff = yearsDiff * 12 + monthsDiff;
    const calendarsCount = Number(calendars) - 1;

    const dateRangeClass = joiningClassNames({
      [classes.dateRange]: true,
      [classes.show]: this.state.show,
      [classes.hide]: !this.state.show
    });

    const dateRangeStyle = {
      display: this.state.show ? "block" : "none",
    }

    const buttonHandlers = {
      apply: this.handleApply.bind(this),
      cancel: this.hide.bind(this)
    }

    return (
      <div className="daterange-container">
        <div className="daterange-label" onClick={() => this.show()}>
          <input readOnly name="DateRange" id="DateRange" className="input-text datepicker" type="text" autoComplete="off" value={this.dateRangeText} />
          <i className="icon icon-calendar"></i>
        </div>

        <div style={onlyClasses ? undefined : { ...styles['DateRange'], ...style, ...dateRangeStyle }} className={dateRangeClass}>
          {showRange && (
            <PredefinedRanges
              format={format}
              ranges={this.ranges}
              range={range}
              theme={styles}
              onSelect={this.handleSelect.bind(this)}
              onlyClasses={onlyClasses}
              classNames={classes}
              locale={this.locale}
              buttonHandlers={buttonHandlers} />
          )}

          {(() => {
            const _calendars = [];
            const _method = offsetPositive ? 'unshift' : 'push';
            for (var i = calendarsCount; i >= 0; i--) {
              const offset = offsetPositive ? i : -i;
              const realDiff = offsetPositive ? diff : -diff;
              const realOffset = (rangedCalendars && i == calendarsCount && diff != 0) ? realDiff : offset;
              _calendars[_method](
                <Calendar
                  showMonthArrow={showMonthArrow}
                  shownDate={shownDate}
                  disableDaysBeforeToday={disableDaysBeforeToday}
                  lang={lang}
                  key={i}
                  offset={realOffset}
                  link={linkedCalendars && link}
                  linkCB={this.handleLinkChange.bind(this)}
                  range={range}
                  format={format}
                  firstDayOfWeek={firstDayOfWeek}
                  theme={styles}
                  minDate={minDate}
                  maxDate={maxDate}
                  onlyClasses={onlyClasses}
                  specialDays={specialDays}
                  classNames={classes}
                  onChange={this.handleSelect.bind(this)}
                  locale={this.locale} />
              );
            }
            return _calendars;
          })()}
        </div>
      </div>
    );
  }
}

DateRange.defaultProps = {
  linkedCalendars: false,
  theme: {},
  format: MomentFormat.default,
  calendars: 2,
  onlyClasses: true,
  offsetPositive: false,
  classNames: {},
  specialDays: [],
  rangedCalendars: false,
  twoStepChange: false,
  showRange: true
}

DateRange.propTypes = {
  format: PropTypes.string,
  firstDayOfWeek: PropTypes.number,
  calendars: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  startDate: PropTypes.oneOfType([PropTypes.object, PropTypes.func, PropTypes.string]),
  endDate: PropTypes.oneOfType([PropTypes.object, PropTypes.func, PropTypes.string]),
  minDate: PropTypes.oneOfType([PropTypes.object, PropTypes.func, PropTypes.string]),
  maxDate: PropTypes.oneOfType([PropTypes.object, PropTypes.func, PropTypes.string]),
  dateLimit: PropTypes.func,
  ranges: PropTypes.object,
  linkedCalendars: PropTypes.bool,
  twoStepChange: PropTypes.bool,
  theme: PropTypes.object,
  onInit: PropTypes.func,
  onChange: PropTypes.func,
  onlyClasses: PropTypes.bool,
  specialDays: PropTypes.array,
  offsetPositive: PropTypes.bool,
  classNames: PropTypes.object,
  rangedCalendars: PropTypes.bool,
  onClickApplyButton: PropTypes.func,
  showRange: PropTypes.bool,
  limitedMonthRange: PropTypes.number
}

export default DateRange;
