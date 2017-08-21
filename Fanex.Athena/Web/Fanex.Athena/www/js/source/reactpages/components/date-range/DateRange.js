import React, { Component } from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
import parseInput from './utils/parseInput.js';
import Calendar from './Calendar.js';
import PredefinedRanges from './PredefinedRanges.js';
import getTheme, { defaultClasses } from './styles.js';
import { MomentFormat } from './constants';

class DateRange extends Component {

  constructor(props, context) {
    super(props, context);

    const { format, linkedCalendars, theme } = props;

    const startDate = parseInput(props.startDate, format, 'startOf');
    const endDate = parseInput(props.endDate, format, 'endOf');
    this.styles = getTheme(theme);
    this.step = 0;

    this.state = {
      range: { startDate, endDate },
      link: linkedCalendars && endDate,
      styles: this.styles['DateRange']
    }
  }

  get dateRangeText() {
    const date = this.state.range;
    if (date.startDate && date.endDate) {
      return `${date.startDate.format(MomentFormat.default)} - ${date.endDate.format(MomentFormat.default)}`;
    }

    return "";
  }

  show() {
    let newStyles = Object.assign({}, this.state.styles, { display: "block" });
    this.setState({ styles: newStyles });
  }

  hide() {
    let newStyles = Object.assign({}, this.state.styles, { display: "none" });
    this.setState({ styles: newStyles });
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

  setRange(range, source, triggerChange) {
    const { onChange } = this.props
    range = this.orderRange(range);

    this.setState({ range }, () => triggerChange && onChange && onChange(range, source));
  }

  handleSelect(date, source) {
    if (date.startDate && date.endDate) {
      this.step = 0;
      return this.setRange(date, source, true);
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

    this.setRange(range, source, triggerChange);
  }


  handleLinkChange(newDate) {
    this.setState({
      link: newDate
    });
  }

  handleApply() {
    const { onClickApplyButton } = this.props;
    onClickApplyButton && onClickApplyButton(this.state.range);

    console.log("Apply");
    console.log(`From: ${this.state.range.startDate.format("MM/DD/YYYY")}`);
    console.log(`To: ${this.state.range.endDate.format("MM/DD/YYYY")}`);
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
        this.setRange({
          startDate: startDate || oldStartDate,
          endDate: endDate || oldEndDate
        });
      }
    }
  }

  render() {
    const { ranges, format, linkedCalendars, style, calendars, firstDayOfWeek, minDate, maxDate, classNames, onlyClasses, specialDays, lang, disableDaysBeforeToday, offsetPositive, shownDate, showMonthArrow, rangedCalendars, locale } = this.props;
    const { range, link } = this.state;
    const { styles } = this;

    const classes = { ...defaultClasses, ...classNames };
    const yearsDiff = range.endDate.year() - range.startDate.year();
    const monthsDiff = range.endDate.month() - range.startDate.month();
    const diff = yearsDiff * 12 + monthsDiff;
    const calendarsCount = Number(calendars) - 1;


    return (
      <div>
        <input onClick={() => this.show()} onFocus={() => this.show()} readOnly
          name="DateRange" id="DateRange" className="input-text datepicker" type="text" autoComplete="off" value={this.dateRangeText} />
        <div style={onlyClasses ? undefined : { ...this.state.styles, ...style }} className={classes.dateRange}>
          {ranges && (
            <PredefinedRanges
              format={format}
              ranges={ranges}
              range={range}
              theme={styles}
              onSelect={this.handleSelect.bind(this)}
              onlyClasses={onlyClasses}
              classNames={classes}
              locale={locale}
              onApply={() => this.handleApply()} />
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
                  onChange={this.handleSelect.bind(this)} />
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
  onlyClasses: false,
  offsetPositive: false,
  classNames: {},
  specialDays: [],
  rangedCalendars: false,
  twoStepChange: false,
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
}

export default DateRange;
