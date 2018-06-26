import React, { Component } from 'react'
import axios from 'axios'

import { Table, Row, Cell } from '@jsroot/components/fixed-table-header'
import ReportTitle from '@jsroot/components/ReportTitle'
import Popup from '@jsroot/components/popup'
import LangHelper from '@jsroot/lib/langHelper'
import NoDataTitle from '@jsroot/components/NoDataTitle'
import BetList from '../lib/BetList'
import Service from '../lib/Service'
import AppData from '@jsroot/lib/AppData'

export default class BaseDataGrid extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isShowPopup : false,
            popupUrl: null
        };
        
        window.ViewResult = (...args) => {
            var url = BetList.getResultUrl.apply(BetList, args);
            this.setState({ isShowPopup: true, popupUrl: url});
        };

        // expand choice content
        window.showNewCombMP = (...args) => {
            this._showNewParlayDetail.apply(this, args);
        };

        // show detail popup (in choice column)
        window.showCombMPDetail = (...args) => {
            var url = BetList.getParlayDetailUrl.apply(BetList, args);
            this.setState({ isShowPopup: true, popupUrl: url});
        };
    }

    _closeResultPopup = () => {
        this.setState({ isShowPopup: false });
    }

    _showNewParlayDetail (transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam) {
        let divEvent = document.querySelector('#divEvent_' + transId + "" + transIndex);
        let nameDiv = document.querySelector('#combinationOfferName_' + transId + transIndex);
        divEvent.classList.add("mx-parlay"); //to make line-through for void-parlay-ticket
        
        if (divEvent.style.display == "none") {
            let url = BetList.getChoiceParlayDetailUrl(transId, refNo, handicap1, handicap2, betcheck, betteam);

            this.props.toggleLoader(true);
             axios.get(url)
                .then((result) => {
                    let choice = BetList.buildChoiceParlayDetail(result.data);
                    divEvent.innerHTML = choice;
                    divEvent.style.display = 'block';
                    nameDiv.style.display = 'none';
                    this.props.toggleLoader(false);
                }).catch(function (error) {
                    console.log(error);
                });
        }
        else {
            divEvent.style.display = 'none';
            divEvent.innerHTML = "";
            nameDiv.style.display = 'block';
        }
    }

    _buildHeader() {
        var headerText = Service.getHeaderText();
        const licenseeWLText = `${headerText.LicenseeWL}/${headerText.Comm}`;
        const winlossText = AppData.isAllowedCustomerComm ? `${headerText.WinLoss}/${headerText.Comm}` : headerText.WinLoss;
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={40} header={true} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Platform}>{headerText.Platform}</Cell>
                <Cell colWidth={120} header={true} title={headerText.ExternalId}>{headerText.ExternalId}</Cell>
                <Cell colWidth={120} header={true} title={headerText.SystemId}>{headerText.SystemId}</Cell>
                <Cell colWidth={180} header={true} title={headerText.TransTime}>{headerText.TransTime}</Cell>
                <Cell colWidth={250} header={true} title={headerText.Choice}>{headerText.Choice}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Odds}>{headerText.Odds}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Stake}>{headerText.Stake}</Cell>
                <Cell colWidth={120} header={true} title={winlossText}>{winlossText}</Cell>
                <Cell colWidth={120} header={true} title={licenseeWLText}>{licenseeWLText}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Status}>{headerText.Status}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Live}>{headerText.Live}</Cell>
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
                    <Cell className="center-align">{item.Platform}</Cell>
                    <Cell>{item.ExternalId}</Cell>
                    <Cell>{item.SystemId}</Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.TransTime }}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Choice}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Odds}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Stake}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.WinLoss}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.LicenseeWL}}></Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.Status}}></Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.Live}}></Cell>
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
                {this.state.isShowPopup &&
                <Popup isShow={true} 
                        iframeUrl={this.state.popupUrl}
                        title={LangHelper.getSingleResource("Result")} 
                        width={900} height={600}
                        onClose={this._closeResultPopup}
                        >
                </Popup>}
            </div>
        );
    }
}