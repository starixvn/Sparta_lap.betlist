import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import DataGrid from './Datagrid'
import FormFilter from './FormFilter'
import Loader from '../../../../components/Loader'
import AppLayout from '../../../../layouts/AppLayout'
import { Tabs, Tab } from '@9softstudio/simple-react-tab'
import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'

export class AppContainer extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <AppLayout>
                <Loader isLoading={this.props.isLoading} />
                <Tabs>
                    <Tab isActive={false} label={LangHelper.getSingleResource("MultipleCustomers")}
                        url={Site.resolveClientUrl("BettingTicketsCenter/MultipleCustomers")} >
                    </Tab>
                    <Tab isActive={false} label={LangHelper.getSingleResource("SingleCustomer")}
                        url={Site.resolveClientUrl("BettingTicketsCenter/SingleCustomer")}>
                    </Tab>
                    <Tab isActive={true} label={LangHelper.getSingleResource("RefNo")}
                        url={Site.resolveClientUrl("BettingTicketsCenter/RefNo")}>
                    </Tab>
                </Tabs>
                <div>
                    <FormFilter />
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