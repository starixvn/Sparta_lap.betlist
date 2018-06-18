import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import DataGrid from './Datagrid'
import MultiCustomerFormFilter from './formfilter/MultiCustomerFormFilter'
import SingleCustomerFormFilter from './formfilter/SingleCustomerFormFilter'
import RefNoFormFilter from './formfilter/RefNoFormFilter'
import Loader from '../../../components/Loader'
import AppLayout from '../../../layouts/AppLayout'
import { Tabs, Tab } from '@9softstudio/simple-react-tab'
import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

export class AppContainer extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <AppLayout>
                <Loader isLoading={this.props.isLoading} />
                <Tabs>
                    <Tab isActive={true} label={LangHelper.getSingleResource("MultipleCustomers111")}>
                        <MultiCustomerFormFilter />
                    </Tab>
                    <Tab isActive={false} label={LangHelper.getSingleResource("RefNo")}>
                        <SingleCustomerFormFilter />
                    </Tab>
                    <Tab isActive={false} label={LangHelper.getSingleResource("CasinoDetailed")}>
                        <RefNoFormFilter />
                    </Tab>
                </Tabs>
                <div>
                    <DataGrid />
                </div>
            </AppLayout>
        );
    }
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: false });
}

export default connect(mapStateToProps)(AppContainer);