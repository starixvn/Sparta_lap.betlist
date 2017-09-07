import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';

import MultipleSelectLabel from './MultipleSelectLabel'
import MultipleSelectOptionList from './MultipleSelectOptionList'

const KEY_NAME = "key";
const VALUE_NAME = "value";
const STATUS_NAME = "checked";

export default class MultipleSelect extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showOptionList: false,
            dataSource: this._convertDataSourceToState(props)
        }
    }

    static propTypes = {
        dataSource: PropTypes.arrayOf(PropTypes.object).isRequired,
        keyField: PropTypes.string,
        valueField: PropTypes.string,
        statusField: PropTypes.string,
        noneSelectedLabel: PropTypes.string,
        maxDisplayItemCount: PropTypes.number,
        onChange: PropTypes.func
    }

    static defaultProps = {
        keyField: KEY_NAME,
        valueField: VALUE_NAME,
        statusField: STATUS_NAME
    }

    _convertDataSourceToState({ keyField, valueField, statusField, dataSource }) {
        return dataSource.map(item => {
            return {
                [KEY_NAME]: item[keyField],
                [VALUE_NAME]: item[valueField],
                [STATUS_NAME]: item[statusField]
            }
        })
    }

    get selectedItems() {
        return this.state.dataSource.filter(item => item.checked);
    }

    onChangeHandler = (item) => {
        const { keyField, valueField, statusField, onChange } = this.props;

        const selectedItem = {
            [keyField]: item.key,
            [valueField]: item.value,
            [statusField]: item.checked
        }

        const newDataSource = this.state.dataSource.slice(0);
        const itemToUpdate = newDataSource.find(x => x.key === item.key);
        itemToUpdate.checked = item.checked;

        const selectedItemsKey = this._getSelectedItemKey(newDataSource);
        onChange && onChange(selectedItem, selectedItemsKey);

        this.setState(Object.assign({}, { dataSource: newDataSource }));
    }

    onToggle = () => {
        this.setState((prevState) => { return { showOptionList: !prevState.showOptionList } })
    }

    _getSelectedItemKey(dataSource) {
        const selectedItemKey = [];

        for (let i = 0; i < dataSource.length; i++) {
            const item = dataSource[i];

            if (item.checked) {
                selectedItemKey.push(item.key)
            }
        }

        return selectedItemKey.join(",");
    }

    _close = () => {
        this.setState((prevState) => { return { showOptionList: false } })
    }

    componentDidMount() {
        document.addEventListener('click', this._handleDocumentClick, true)
    }

    componentWillUnmount() {
        document.removeEventListener('click', this._handleDocumentClick, true)
    }

    _handleDocumentClick = (e) => {
        const node = ReactDOM.findDOMNode(this);

        const clickOutside = node && node !== e.target && !node.contains(e.target);

        if (clickOutside) {
            this._close();
        }
    }

    render() {
        const { noneSelectedLabel, maxDisplayItemCount } = this.props;

        return (
            <div className="multiple-select-container">
                <MultipleSelectLabel
                    selectedItems={this.selectedItems}
                    onToggle={this.onToggle}
                    noneSelectedLabel={noneSelectedLabel}
                    maxDisplayItemCount={maxDisplayItemCount} />

                <MultipleSelectOptionList
                    show={this.state.showOptionList}
                    dataSource={this.state.dataSource}
                    onChange={this.onChangeHandler} />
            </div>
        )
    }
}