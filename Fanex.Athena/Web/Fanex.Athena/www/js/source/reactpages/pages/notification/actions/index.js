import * as notificationDataAction from '../components/notification/actions/dataHandlerAction.js'
import * as notificationFilterAction from '../components/notification/actions/filterHandlerAction.js'
import * as resultAmendmentDataAction from '../components/resultAmendment/actions/dataHandlerAction.js'
import * as resultAmendmentFilterAction from '../components/resultAmendment/actions/filterHandlerAction.js'
import * as loaderAction from '../../../actions/loaderAction'

export const ActionCreators = Object.assign({},
    notificationDataAction,
    notificationFilterAction,
    resultAmendmentDataAction,
    resultAmendmentFilterAction,
    loaderAction
);