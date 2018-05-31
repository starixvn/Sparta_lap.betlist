import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'

import Loader from '../../../components/Loader'
import AppLayout from '../../../layouts/AppLayout'
import { Tabs, Tab } from '@9softstudio/simple-react-tab'
import LangHelper from '../../../lib/langHelper'

import SingleDay from './singleday'

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        window.ViewResult = function(){
            alert(1);
        };
    }

    render() {
        return (
            <AppLayout>
                <Loader isLoading={this.props.isLoading} />
                <Tabs>
                    <Tab isActive={true} label={LangHelper.getSingleResource("SingleDay")}>
                        <SingleDay />
                    </Tab>
                    <Tab isActive={false} label={LangHelper.getSingleResource("RefNo")}>
                        
                    </Tab>
                    <Tab isActive={false} label={LangHelper.getSingleResource("CasinoDetailed")}>
                        
                    </Tab>
                </Tabs>
            </AppLayout>
        );
    }
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: false });
}

export default connect(mapStateToProps)(AppContainer);