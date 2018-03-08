import moment from 'moment';
import LangHelper from "../../lib/langHelper";
import locale from "./utils/locale";
import { MomentFormat } from './constants';

export default {
    ranges: (today, max) => {
        if (today && !moment.isMoment(today)) {
            today = moment(today, MomentFormat.default)
        }

        if (!today) {
            today = moment();
        }

        if (max && !moment.isMoment(max)) {
            max = moment(max, MomentFormat.default)
        }

        if (!max) {
            max = moment();
        }

        const range = {};
        if (today <= max) {
            range[LangHelper.getSingleResource("Today")] = { startDate: today, endDate: today };
        }

        range[LangHelper.getSingleResource("Yesterday")] = {
            startDate: today.clone().add(-1, 'days'),
            endDate: today.clone().add(-1, 'days')
        };

        today = today > max ? max : today;

        range[LangHelper.getSingleResource("Currentweek")] = {
            startDate: today.clone().startOf('week'),
            endDate: today
        };
        range[LangHelper.getSingleResource("Lastweek")] = {
            startDate: today.clone().subtract(1, 'week').startOf('week'),
            endDate: today.clone().subtract(1, 'week').startOf('week').add(6, 'days')
        };
        range[LangHelper.getSingleResource("Currentmonth")] = {
            startDate: today.clone().startOf('month'),
            endDate: today
        };
        range[LangHelper.getSingleResource("Lastmonth")] = {
            startDate: today.clone().subtract(1, 'month').startOf('month'),
            endDate: today.clone().subtract(1, 'month').endOf('month')
        };
        range[LangHelper.getSingleResource("Sincelastmonth")] = {
            startDate: today.clone().subtract(1, 'month').startOf('month'),
            endDate: today
        };    

        return range;
    },

    locale: {
        applyLabel: LangHelper.getSingleResource("OK"),
        cancelLabel: LangHelper.getSingleResource("Cancel"),
        ...locale
    }
}