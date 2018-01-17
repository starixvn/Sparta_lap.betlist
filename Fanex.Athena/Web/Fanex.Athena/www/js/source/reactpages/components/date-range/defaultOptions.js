import moment from 'moment';
import LangHelper from "../../lib/langHelper";
import locale from "./utils/locale";
import { MomentFormat } from './constants';

export default {
    ranges: (today) => {
        if (today && !moment.isMoment(today)) {
            today = moment(today, MomentFormat.default)
        }

        if (!today) {
            today = moment();
        }

        return {
            [LangHelper.getSingleResource("Today")]: {
                startDate: today,
                endDate: today
            },

            [LangHelper.getSingleResource("Yesterday")]: {
                startDate: today.clone().add(-1, 'days'),
                endDate: today.clone().add(-1, 'days')
            },

            [LangHelper.getSingleResource("Currentweek")]: {
                startDate: today.clone().startOf('week'),
                endDate: today
            },

            [LangHelper.getSingleResource("Lastweek")]: {
                startDate: today.clone().subtract(1, 'week').startOf('week'),
                endDate: today.clone().subtract(1, 'week').startOf('week').add(6, 'days')
            },

            [LangHelper.getSingleResource("Currentmonth")]: {
                startDate: today.clone().startOf('month'),
                endDate: today
            },

            [LangHelper.getSingleResource("Lastmonth")]: {
                startDate: today.clone().subtract(1, 'month').startOf('month'),
                endDate: today.clone().subtract(1, 'month').endOf('month')
            },

            [LangHelper.getSingleResource("Sincelastmonth")]: {
                startDate: today.clone().subtract(1, 'month').startOf('month'),
                endDate: today
            }
        }
    },

    locale: {
        applyLabel: LangHelper.getSingleResource("OK"),
        cancelLabel: LangHelper.getSingleResource("Cancel"),
        ...locale
    }
}