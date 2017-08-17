import React, { Component } from 'react';
import Row from './Row';
import Header from './Header';
import RowLayout from './RowLayout'

class TableHeader extends Component {
    constructor(props){
        super(props);
    }

    _renderHeader() {
        const headers = this.props.headers.map(item => {
            return <Header>{item}</Header>
        });

        return (
            <Row>
                {headers}
            </Row>
        );
    }

    render() {
        return (
            <div className="header-content">
                <table className="table">
                    <thead>
                        <RowLayout layoutConfig={this.props.layoutConfig} />
                        {this._renderHeader()}
                    </thead>
                </table>
            </div>
        );
    }
}

export default TableHeader;