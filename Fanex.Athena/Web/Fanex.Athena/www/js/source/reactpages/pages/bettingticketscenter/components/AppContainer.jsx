import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'

import Loader from '../../../components/Loader'
import AppLayout from '../../../layouts/AppLayout'
import { Tabs, Tab } from '../../../components/tab'
import LangHelper from '../../../lib/langHelper'

import Product from './products'
import RefNo from './refno'
import CasinoDetailed from './casinodetailed'

export class AppContainer extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <AppLayout>
                <Loader isLoading={this.props.isLoading} />
                <Tabs>
                    <Tab isActive={true} label={LangHelper.getSingleResource("Products")}>
                        <Product />
                    </Tab>
                    <Tab isActive={false} label={LangHelper.getSingleResource("RefNo")}>
                        <RefNo />
                    </Tab>
                    <Tab isActive={false} label={LangHelper.getSingleResource("CasinoDetailed")}>
                        <CasinoDetailed />
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