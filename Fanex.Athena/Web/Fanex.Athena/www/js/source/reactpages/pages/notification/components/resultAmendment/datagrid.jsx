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

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = "";
    }

    componentWillMount() {
        const param = this.props.param;
        if(param && param.hasOwnProperty("FromDate")) {
            const dateRange = {
                fromDate: param.FromDate,
                toDate: param.ToDate
            };

            this.reportTitle = service.getReportTitle(dateRange);
        }
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const { dateRange } = nextProps;
            this.reportTitle = service.getReportTitle(dateRange);
            this.props.toggleLoader(false);
        }
    }

    onPaging = (index, size) => {
        let paramData = this.props.param;

        paramData.PageIndex = index;
        paramData.PageSize = size;

        const params = {
            xsrfToken: this.props.xsrfToken,
            data: paramData
        };

        this.props.toggleLoader(true);
        this.props.getGridData(params);
        this.props.setParam(paramData);
    }

    onExpand(index, item) {
        const { Data, Total } = this.props.gridData;
        Data[index].IsExpand = true;

        if(!item.IsRead) {
            this.props.markItemAsRead({
                Index: index,
                Id: item.Id,
                Type: item.Type,
                Data: Data,
                Total: Total
            });
        }
        else {
            this.props.setGridData({ Data: Data, Total: Total });
        }        
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={30} header={true} title={headerText.index}>{headerText.index}</Cell>
                <Cell colWidth={150} header={true} title={headerText.dateTime}>{headerText.dateTime}</Cell>
                <Cell colWidth={340} header={true} title={headerText.content}>{headerText.content}</Cell>
                <Cell colWidth={30} header={true} title={headerText.ticketList}>{headerText.ticketList}</Cell>
            </Row>
        );
    }

    _buildBody() {
        const data = this.props.gridData.Data;       
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const datetimeString = moment(item.DateTime).format("MM/DD/YYYY hh:mm:ss A");
            rows.push(
                <Row key={`tr${i}`} className={ item.IsRead ? "" : "unread" }>
                    <Cell title={i + 1} className="center-align">{i + 1}</Cell>
                    <Cell title={datetimeString} className="center-align">{datetimeString}</Cell>
                    <Cell title={item.Subject}>
                        <CollapsibleRow subject={item.Subject} content={item.Content} onExpand={() => this.onExpand(i, item)} isExpand={item.IsExpand} isHtml={true} />
                    </Cell>
                    {
                        Site.asNull(item.DataFilePath) === null 
                        ? <Cell></Cell>
                        : <Cell title={LangHelper.getSingleResource("ExportToExcel")} className="center-align"><a href={item.DataFilePath} className="icon-excel"></a></Cell>
                    }
                </Row>)
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
                       bodyHeight={600}
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
    const { gridData, dateRange, param } = state.resultAmendment;
    return Object.assign({}, { gridData, dateRange, param, xsrfToken: state.xsrfToken });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);