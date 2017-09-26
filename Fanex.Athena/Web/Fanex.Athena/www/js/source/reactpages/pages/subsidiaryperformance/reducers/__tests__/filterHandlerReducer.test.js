import * as filterHandlerReducer from '../filterHandlerReducer'
import * as types from '../../actions/types'

const emptyState = {};

const assertReducer = (stateName, actionType, expectedResult) => {
    const action = {
        type: actionType,
        data: expectedResult
    }

    const result = filterHandlerReducer[stateName](emptyState, action);

    expect(result).toEqual(expectedResult);
}

const reducerList = [
    { name: "subsidiaryList", actionType: types.GET_SUBSIDIARY, expected: "1,2,3" },
    { name: "selectedSubsidiary", actionType: types.SET_SUBSIDIARY, expected: "1,2,3,4,5" },
    { name: "dateRange", actionType: types.SET_DATE_RANGE, expected: { fromDate: "09/01/2017", toDate: "09/26/2017" } },
    { name: "convertAll", actionType: types.CHANGE_CONVERT_ALL, expected: true },
    { name: "xsrfToken", actionType: types.GET_XSRF_TOKEN, expected: "token" },
    { name: "dateConstraint", actionType: types.GET_DATE_CONSTRAINT, expected: { fromDate: "01/01/2017", toDate: "09/30/2017" } },
]

describe('[filterHandlerReducer] ', () => {
    reducerList.forEach(({ name, actionType, expected }) => {
        it(name, () => {
            const action = {
                type: actionType,
                data: expected
            }

            const result = filterHandlerReducer[name](emptyState, action);

            expect(result).toEqual(expected);
        })
    })

    it("return default state when no match action type", () => {
        const initialState = { subsidiaryList: "1", convertAll: true };
        const action = {
            type: "Action no match",
            data: "1,2,3"
        }

        const result = filterHandlerReducer.subsidiaryList(initialState, action)

        expect(result).toEqual(initialState);
    })
})