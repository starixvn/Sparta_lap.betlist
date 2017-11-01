import * as notificationDataAction from '../notificationTab/actions/dataHandlerAction.js'
import * as notificationFilterAction from '../notificationTab/actions/filterHandlerAction.js'
import * as resultAmendmentDataAction from '../resultAmendmentTab/actions/dataHandlerAction.js'
import * as resultAmendmentFilterAction from '../resultAmendmentTab/actions/filterHandlerAction.js'
import * as loaderAction from '../../../actions/loaderAction'

export const ActionCreators = Object.assign({},
    notificationDataAction,
    notificationFilterAction,
    resultAmendmentDataAction,
    resultAmendmentFilterAction,
    loaderAction
);