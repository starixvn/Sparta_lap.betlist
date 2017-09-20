import LangHelper from "../../lib/langHelper";
import locale from "./utils/locale";

export default {
    ranges: {
        [LangHelper.getSingleResource("Today")]: {
            startDate: (now) => {
                return now;
            },
            endDate: (now) => {
                return now;
            }
        },

        [LangHelper.getSingleResource("Yesterday")]: {
            startDate: (now) => {
                return now.add(-1, 'days');
            },
            endDate: (now) => {
                return now.add(-1, 'days');
            }
        },

        [LangHelper.getSingleResource("Currentweek")]: {
            startDate: (now) => {
                return now.startOf('week');
            },
            endDate: (now) => {
                return now;
            }
        },

        [LangHelper.getSingleResource("Lastweek")]: {
            startDate: (now) => {
                return now.subtract(1, 'week').startOf('week');
            },
            endDate: (now) => {
                return now.subtract(1, 'week').startOf('week').add(6, 'days');
            }
        },

        [LangHelper.getSingleResource("Currentmonth")]: {
            startDate: (now) => {
                return now.startOf('month');
            },
            endDate: (now) => {
                return now;
            }
        },

        [LangHelper.getSingleResource("Lastmonth")]: {
            startDate: (now) => {
                return now.subtract(1, 'month').startOf('month');
            },
            endDate: (now) => {
                return now.subtract(1, 'month').endOf('month');
            }
        },

        [LangHelper.getSingleResource("Sincelastmonth")]: {
            startDate: (now) => {
                return now.subtract(1, 'month').startOf('month');
            },
            endDate: (now) => {
                return now;
            }
        }
    },

    locale: {
        applyLabel: LangHelper.getSingleResource("OK"),
        cancelLabel: LangHelper.getSingleResource("Cancel"),
        ...locale
    }
}