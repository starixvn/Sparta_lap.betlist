import moment from 'moment';
import LangHelper from 'lrflib/langHelper';

export default function updateLocaleByLanguage(langKey) {
    moment.updateLocale(langKey, {
        months: [
            LangHelper.getSingleResource('Jan'), LangHelper.getSingleResource('Feb'),
            LangHelper.getSingleResource('Mar'), LangHelper.getSingleResource('Apr'),
            LangHelper.getSingleResource('May'), LangHelper.getSingleResource('Jun'),
            LangHelper.getSingleResource('Jul'), LangHelper.getSingleResource('Aug'),
            LangHelper.getSingleResource('Sep'), LangHelper.getSingleResource('Oct'),
            LangHelper.getSingleResource('Nov'), LangHelper.getSingleResource('Dec')
        ]
    });

    moment.locale(langKey);
}