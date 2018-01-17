import moment from 'moment'
import { MomentFormat } from '../constants'

import defaultOptions from '../defaultOptions'

describe('defaultOptions', () => {
    describe('ranges', () => {
        it('today is undefined - get today value in system', () => {
            const ranges = defaultOptions.ranges();
            const expectedResult = moment().format(MomentFormat.default);

            const startDate = ranges.Today.startDate.format(MomentFormat.default);
            const endDate = ranges.Today.endDate.format(MomentFormat.default);

            expect(startDate).toEqual(expectedResult);
            expect(endDate).toEqual(expectedResult);
        })

        it('today is string - parse today value to moment object - Today value should be correct', () => {
            const today = "11/24/2017";
            const ranges = defaultOptions.ranges(today);
            const expectedResult = moment(today, MomentFormat.default);

            expect(ranges.Today.startDate).toEqual(expectedResult);
            expect(ranges.Today.endDate).toEqual(expectedResult);
        })

        it('today is moment obejct - Today value should be correct', () => {
            const today = "11/24/2017";
            const todayMoment = moment(today, MomentFormat.default);
            const ranges = defaultOptions.ranges(todayMoment);

            expect(ranges.Today.startDate).toEqual(todayMoment);
            expect(ranges.Today.endDate).toEqual(todayMoment);
        })
    })
})