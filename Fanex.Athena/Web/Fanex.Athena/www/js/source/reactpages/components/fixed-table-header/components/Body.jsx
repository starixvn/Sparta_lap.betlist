import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import TableSection from './TableSection';

class Body extends Component {
    constructor(props) {
        super(props);
        this.id = props.id;
    }

    componentDidMount() {
        if (!this.id) return;

        const container = document.getElementById(this.id);
        if (this._hasVerticalScrollbar(container)) {
            container.style.paddingRight = 0;
        }
    }

    _hasVerticalScrollbar(element) {
        return element && element.scrollHeight && element.scrollHeight > element.offsetHeight;
    }

    render() {
        return (
            <TableSection className="body-content" {...this.props}>
                <tbody>
                    {this.props.children}
                </tbody>
            </TableSection>
        );
    }
}

export default Body;