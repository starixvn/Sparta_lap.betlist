import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js';
import { bindActionCreators } from 'redux';
import FixedTableHeader from 'components/fixed-table-header/FixedTableHeader'

const headers = ["Col 1", "Col 2", "Col 3"];
const data = [{Id: 0, Name: "A", Age: 20}, {Id: 1, Name: "B", Age: 210}, {Id: 2, Name: "C", Age: 201}];
const layoutConfig = [100, 200, 300];

const config = {headers, data, layoutConfig};

class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <FixedTableHeader dataConfig={config} />
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    return {

    };
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);