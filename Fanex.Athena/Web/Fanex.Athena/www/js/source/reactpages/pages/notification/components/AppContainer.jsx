import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../actions/index'
import NotificationTab from '../notificationTab/components/notificationTab'
import ResultAmendmentTab from '../resultAmendmentTab/components/resultAmendmentTab'
import Loader from '../../../components/Loader'
import AppLayout from '../../../layouts/AppLayout'
import Tab from '../../../components/Tab'
import Tabs from '../../../components/Tabs'
import LangHelper from '../../../lib/langHelper'

export class AppContainer extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
          <AppLayout>
            <Loader isLoading={this.props.isLoading} />
            <Tabs>
                <Tab isActive={true} label={LangHelper.getSingleResource("Notification")}>
                    <NotificationTab />
                </Tab>
                <Tab isActive={false} label={LangHelper.getSingleResource("ResultAmendment")}>
                    <ResultAmendmentTab />
                </Tab>
            </Tabs>
          </AppLayout>
    );
}
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: state.isLoading });
}

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);