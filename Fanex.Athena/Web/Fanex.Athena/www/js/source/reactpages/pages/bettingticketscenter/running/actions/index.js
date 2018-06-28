import * as gridDataAction from '@jsroot/actions/gridDataAction'
import * as usernameAction from '@jsroot/actions/usernameAction'
import * as productAction from '@jsroot/actions/productAction'
import * as currencyAction from '@jsroot/actions/currencyAction'
import * as baseCurrencyAction from '@jsroot/actions/baseCurrencyAction'
import * as loaderAction from '@jsroot/actions/loaderAction'
import * as dateRangeAction from '@jsroot/actions/dateRangeAction'

export const ActionCreators = Object.assign({},
    gridDataAction,
    usernameAction,
    productAction,
    currencyAction,
    baseCurrencyAction,
    loaderAction,
    dateRangeAction
);