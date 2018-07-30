import React, { Component } from 'react'

import { Table, Row, Cell } from '@jsroot/components/fixed-table-header'
import ReportTitle from '@jsroot/components/ReportTitle'
import Popup from '@jsroot/components/popup'
import LangHelper from '@jsroot/lib/langHelper'
import NoDataTitle from '@jsroot/components/NoDataTitle'

const DEFAULT_PAGE_SIZE = 50;

export default class BaseDataGrid extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const param = this.props.requestParam;
        param.PageSize = DEFAULT_PAGE_SIZE;
        this.props.setRequestParam(param);
    }
    
    get dataUrl(){
        // This property should be override in child class.
    }

    onPaging = (index, size) => {
        let param = this.props.requestParam;

        param.PageIndex = index;
        param.PageSize = size;

        this._getData(param);
    }
    
    _getData(param){
        this.props.toggleLoader(true);
        this.props.getGridData(this.dataUrl, param);
        this.props.setRequestParam(param);
    }

    _sort(sortBy){
        console.log("SORT: " + sortBy);
        let param = this.props.requestParam;

        if (!param.SortBy || param.SortBy != sortBy) {
            param.SortBy = sortBy;
            param.SortOrder = 'asc';
        }
        else {
            param.SortOrder = param.SortOrder == 'asc' ? 'desc' : 'asc';
        }

        this._getData(param);
    }

    _buildHeader() {
        // This method should be override in child class.
        return [];
    }

    _buildBody(){
        const data = this.props.gridData.Data;
        let rows = [];

        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            rows.push(
                   <Row key={`product-${i}`}>
                    <Cell className="center-align">{item.Index}</Cell>                  
                    <Cell>{item.ExternalId}</Cell>
                    <Cell>{item.SystemId}</Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.TransTime }}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Choice}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Odds}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Stake}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.WinLoss}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.LicenseeWL}}></Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.Status}}></Cell>
                    <Cell className="center-align">{item.Platform}</Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.Live}}></Cell>
                </Row>
            );
        }

        return rows;
    }

    _renderNodata() {
        return (
            <div className="result-content">
                <ReportTitle content={this.props.reportTitle} />
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

        const param = this.props.requestParam;
        const pageOption = {
            PageIndex: param.PageIndex,
            PageSize: param.PageSize,
            TotalItem: this.props.gridData.TotalRows,
            PageList: [DEFAULT_PAGE_SIZE, 100, 200, 300, 400, 500, 1000, 2000]
        };

        return (
            <div className="betlist-container result-content">
                <ReportTitle content={this.props.reportTitle} />
                <Table
                    minWidth={960}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader()}
                    body={this._buildBody()}
                    footer={this._buildFooter && this._buildFooter()}
                    isPaging={true}
                    pageOption={pageOption}
                    onPaging={this.onPaging}
                />
            </div>
        );
    }
}