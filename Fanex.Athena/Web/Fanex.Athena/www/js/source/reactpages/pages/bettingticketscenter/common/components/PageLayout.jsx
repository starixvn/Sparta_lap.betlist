import React, { Component } from 'react';
import AppLayout from '@jsroot/layouts/AppLayout'
import Loader from '@jsroot/components/Loader'
import { Tabs, Tab } from '@9softstudio/simple-react-tab'
import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'

export default class PageLayout extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <AppLayout>
                <Loader isLoading={this.props.isLoading} />
                <Tabs>
                    <Tab isActive={this.props.selectedTabIndex == 0} label={LangHelper.getSingleResource("MultipleCustomers")}
                        url={Site.resolveClientUrl("BettingTicketCenter/Index")} >
                    </Tab>
                    <Tab isActive={this.props.selectedTabIndex == 1} label={LangHelper.getSingleResource("SingleCustomer")}
                        url={Site.resolveClientUrl("BettingTicketCenter/SingleCustomer")}>
                    </Tab>
                    <Tab isActive={this.props.selectedTabIndex == 2} label={LangHelper.getSingleResource("RefNo")}
                        url={Site.resolveClientUrl("BettingTicketCenter/RefNo")}>
                    </Tab>
                    <Tab isActive={this.props.selectedTabIndex == 3} label={LangHelper.getSingleResource("Running")}
                        url={Site.resolveClientUrl("BettingTicketCenter/Running")}>
                    </Tab>
                </Tabs>
                <div>
                    {this.props.children}
                </div>
            </AppLayout>
        );
    }
}