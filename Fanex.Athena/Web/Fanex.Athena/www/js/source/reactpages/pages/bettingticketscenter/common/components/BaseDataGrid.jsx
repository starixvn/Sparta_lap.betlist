import React, { Component } from 'react'

import { Table, Row, Cell } from '../../../../components/fixed-table-header'
import Service from '../lib/Service'
import ReportTitle from '../../../../components/ReportTitle'
import Popup from '../../../../components/popup'
import LangHelper from '../../../../lib/langHelper'
import BetList from '../lib/BetList'
import NoDataTitle from '../../../../components/NoDataTitle'

export default class BaseDataGrid extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isShowResultPopup : false,
            resultUrl: null
        };
        
        window.ViewResult = (...args) => {
            let betlist = new BetList();
            var url = betlist.getResultUrl.apply(betlist, args);
            this.setState({ isShowResultPopup: true, resultUrl: url});
        }
    }
    closeBetListPopup = () => {
        this.setState({ isShowResultPopup: false });
    }

    _buildHeader() {
        var headerText = Service.getHeaderText();
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={40} header={true} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Platform}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.ExternalId}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.SystemId}</Cell>
                <Cell colWidth={180} header={true} title={headerText.Index}>{headerText.TransTime}</Cell>
                <Cell colWidth={250} header={true} title={headerText.Index}>{headerText.Choice}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Odds}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Stake}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.WinLoss}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.LicenseeWL}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Status}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Live}</Cell>
            </Row>
        )
    }

    _buildBody() {
                    const data = this.props.gridData.Data;
                    let rows = [];

                    for (let i = 0; i < data.length; i++) {
                        const item = data[i];
                        rows.push(
                   <Row key={`product-${i}`}>
                    <Cell className="center-align">{item.Index}</Cell>
                    <Cell>{item.Platform}</Cell>
                    <Cell>{item.ExternalId}</Cell>
                    <Cell>{item.SystemId}</Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.TransTime }}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Choice}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Odds}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Stake}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.WinLoss}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.LicenseeWL}}></Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.Status}}></Cell>
                    <Cell className="center-align"dangerouslySetInnerHTML={{ __html: item.Live}}></Cell>
                </Row>
            );
                }

        return rows;
                }

    _renderNodata() {
        return (
            <div className="result-content">
                <ReportTitle content={this.reportTitle} />
                <NoDataTitle />
            </div>
        )
                }

    render() {
        if (!this.props.gridData || !this.props.gridData.Data) {
            return null;
                }

        const { Data } = this.props.gridData;
        
        if (!Data.length) {
            return this._renderNodata();
        }

        return (
            <div className="betlist-container result-content">
                <ReportTitle content={this.reportTitle} />
                <Table
                    minWidth={600}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader()}
                    body={this._buildBody()}
                />
                <Popup isShow={this.state.isShowResultPopup} 
                        iframeUrl={this.state.resultUrl}
                        title={LangHelper.getSingleResource("Result")} 
                        width={900} height={600}
                        onClose={this.closeBetListPopup}
                        >
                </Popup>
            </div>
        );
}
}