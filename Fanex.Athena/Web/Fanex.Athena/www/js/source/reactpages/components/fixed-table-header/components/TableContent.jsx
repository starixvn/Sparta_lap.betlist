import React, { Component } from 'react';
import Row from './Row';
import Cell from './Cell';
import RowLayout from './RowLayout'

class TableContent extends Component {
    constructor(props) {
        super(props);
    }
    _renderRows() {
        return this.props.data.map((item, index) => {
            return (
                <Row key={`row${index}`}>{this._renderCells(item)}</Row>
            );

        });
    }
    _renderCells(row) {
        return Object.keys(row).map((key, index) => {
            return <Cell key={`cell${index}`}>{row[key]}</Cell>
        });
    }
    render() {
        return (
            <div className="body-content noScroll">
                <table className="table">
                    <tbody >
                        <RowLayout layoutConfig={this.props.layoutConfig} />
                        {this._renderRows()}
                    </tbody>
                </table>
            </div>
        );
    }
}

export default TableContent;