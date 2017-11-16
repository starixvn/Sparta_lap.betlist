import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import ResultAmendment from './resultAmendment'
import Loader from '../../../components/Loader'
import AppLayout from '../../../layouts/AppLayout'
import { Tabs, Tab } from '../../../components/tab'
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
                    <Tab isActive={true} label={LangHelper.getSingleResource("Amendment")}>
                        <ResultAmendment />
                    </Tab>
                </Tabs>
            </AppLayout>
        );
    }
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: state.isLoading });
}

export default connect(mapStateToProps)(AppContainer);