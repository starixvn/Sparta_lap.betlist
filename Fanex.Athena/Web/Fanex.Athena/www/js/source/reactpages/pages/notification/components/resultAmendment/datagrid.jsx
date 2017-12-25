import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../../actions/resultAmendment'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../../components/fixed-table-header'
import LangHelper from '../../../../lib/langHelper'
import ReportTitle from '../../../../components/ReportTitle'
import NoDataTitle from '../../../../components/NoDataTitle'
import service from '../../lib/resultAmendmentService'
import Site from '../../../../lib/site'
import CollapsibleRow from '../../../../components/CollapsibleRow'
import moment from 'moment'
import Excel from '../../../../lib/excel'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = "";
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const paramData = this.props.param;
            if(paramData) {
                this.reportTitle = service.getReportTitle({
                    fromDate: paramData.FromDate,
                    toDate: paramData.ToDate
                });
            }
            this.props.toggleLoader(false);
        }
    }

    onPaging = (index, size) => {
        let paramData = this.props.param;

        paramData.PageIndex = index;
        paramData.PageSize = size;

        const params = this._buildParams({
            data: paramData
        });

        this.props.toggleLoader(true);
        this.props.getGridData(params);
        this.props.setParam(paramData);
    }

    onExpand(index, item) {
        const { Data, Total, DownloadFileApi } = this.props.gridData;
        Data[index].IsExpand = true;

        if(!item.IsRead) {
            this.props.markItemAsRead(this._buildParams({
                Index: index,
                Id: item.Id,
                Type: item.Type,
                Data: Data,
                Total: Total,
                DownloadFileApi: DownloadFileApi
            }));
        }
        else {
            this.props.setGridData({ Data: Data, Total: Total, DownloadFileApi: DownloadFileApi });
        }
    }

    exportExcel(item) {
        const params = this._buildParams({
            api: this.props.gridData.DownloadFileApi,
            fileName: item.DataFilePath
        });
        const fileName = this.props.licenseeName;

        this.props.toggleLoader(true);
        this.props.exportAttachment(params).then(response => {
            if(response.status === 204) {
                alert(LangHelper.getSingleResource('FileIsNotReady'));
            }
            else{
                const blob = response.data;
                Excel.downloadFromBolb(blob, fileName);
            }
            this.props.toggleLoader(false);
        });
    }

    _buildParams(params) {
        params.xsrfToken = this.props.xsrfToken;
        return params;
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={30} header={true} title={headerText.index}>{headerText.index}</Cell>
                <Cell colWidth={120} header={true} title={headerText.dateTime}>{headerText.dateTime}</Cell>
                <Cell colWidth={350} header={true} title={headerText.content}>{headerText.content}</Cell>
                <Cell colWidth={30} header={true} title={headerText.ticketList}>{headerText.ticketList}</Cell>
            </Row>
        );
    }

    _buildBody() {
        const data = this.props.gridData.Data;
        const exportToExcelTitle = LangHelper.getSingleResource("ExportToExcel");

        let rows = [];
        let index = ((this.props.param.PageIndex - 1) * this.props.param.PageSize) + 1;
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const datetimeString = moment(item.DateTime).format("MM/DD/YYYY hh:mm:ss A");
            rows.push(
                <Row key={`tr${i}`} className={ item.IsRead ? "" : "unread" }>
                    <Cell title={index} className="center-align">{index}</Cell>
                    <Cell title={datetimeString} className="center-align">{datetimeString}</Cell>
                    <Cell className="notification-content">
                        <CollapsibleRow 
                            subject={item.Subject} 
                            content={item.Content} 
                            onExpand={() => this.onExpand(i, item)} 
                            isExpand={item.IsExpand} 
                            isHtml={true} />
                    </Cell>
                    {
                        Site.asNull(item.DataFilePath) === null 
                        ? <Cell></Cell>
                        : <Cell title={exportToExcelTitle} className="center-align"><a onClick={() => this.exportExcel(item)} className="icon-excel"></a></Cell>
                    }
                </Row>)
            index++;
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
        const gridData = this.props.gridData;
        if (!gridData.Data) {
            return null;
        }

        if (!gridData.Data || !gridData.Data.length) {
            return this._renderNodata();
        }

        const param = this.props.param;
        const pageOption = {
            PageIndex: param.PageIndex,
            PageSize: param.PageSize,
            TotalItem: gridData.Total,
            PageList: [10, 20, 50, 100, 500]
        };

        return (
            <div className="result-content">
                <ReportTitle content={this.reportTitle} />

                <Table minWidth={650}
                       autoWidth={true}
                       adjustedHeight={32}
                       header={this._buildHeader()}
                       body={this._buildBody()}
                       isPaging={true}
                       pageOption={pageOption}
                       onPaging={this.onPaging} />
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
    const { gridData, dateRange, param, licenseeName } = state.resultAmendment;
    return Object.assign({}, { gridData, dateRange, param, xsrfToken: state.xsrfToken, licenseeName });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);