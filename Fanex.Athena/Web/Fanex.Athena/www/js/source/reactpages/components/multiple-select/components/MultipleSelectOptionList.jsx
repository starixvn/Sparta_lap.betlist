import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';

import Option from './Option'

export default class MultipleSelectOptionList extends Component {
    constructor(props) {
        super(props);
    }

    static propTypes = {
        dataSource: PropTypes.arrayOf(PropTypes.object).isRequired,
        onChange: PropTypes.func
    }

    _renderOptionList() {
        const { dataSource, onChange } = this.props;
        const optionList = dataSource.map((item, index) => {
            return (
                <Option
                    id={`optionItem${index}`}
                    key={`optionItem${index}`}
                    itemData={item}
                    onChange={onChange} />
            )
        });

        return optionList;
    }

    render() {
        return (
            <ul className="multiple-select-default multiple-select-options" style={{ display: this.props.show ? "block" : "none" }}>
                {this._renderOptionList()}
            </ul>
        )
    }
}