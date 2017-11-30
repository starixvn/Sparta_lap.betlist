import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../../actions/general'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../../components/fixed-table-header'
import ReportTitle from '../../../../components/ReportTitle'
import NoDataTitle from '../../../../components/NoDataTitle'
import service from '../../lib/generalService'
import CollapsibleRow from '../../../../components/CollapsibleRow'
import Site from '../../../../lib/site'
import moment from 'moment'

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

        const params = {
            xsrfToken: this.props.xsrfToken,
            data: paramData
        };

        this.props.toggleLoader(true);
        this.props.getGridData(params);
        this.props.setParam(paramData);
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={30} header={true} title={headerText.index}>{headerText.index}</Cell>
                <Cell colWidth={150} header={true} title={headerText.type}>{headerText.type}</Cell>
                <Cell colWidth={150} header={true} title={headerText.dateTime}>{headerText.dateTime}</Cell>
                <Cell colWidth={300} header={true} title={headerText.content}>{headerText.content}</Cell>
            </Row>
        )
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
                    <Cell title={item.Type}>{item.Type}</Cell>
                    <Cell title={datetimeString} className="center-align">{datetimeString}</Cell>
                    <Cell title={item.Subject}>
                        <CollapsibleRow subject={item.Subject} content={item.Content} isHTML={true} />
                    </Cell>
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

                <Table
                    minWidth={650}
                    autoWidth={true}
                    bodyHeight={540}
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
    const { gridData, dateRange, param } = state.general;
    return Object.assign({}, { gridData, dateRange, param, xsrfToken: state.xsrfToken });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);