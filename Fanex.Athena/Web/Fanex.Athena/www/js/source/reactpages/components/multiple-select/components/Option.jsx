import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';

export default class Option extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isChecked: props.itemData.checked
        }
    }

    static propTypes = {
        itemData: PropTypes.object.isRequired,
        onChange: PropTypes.func
    }

    onChange = () => {
        const { itemData, onChange } = this.props;
        
        const newItemData = {
            key: itemData.key,
            value: itemData.value,
            checked: !itemData.checked
        }
        onChange && onChange(newItemData);
    }

    render() {
        const { itemData, id } = this.props;

        return (
            <li className="multiple-select-item">
                <input
                    id={id}
                    type="checkbox"
                    className="option-checkbox"
                    checked={this.props.itemData.checked}
                    onChange={this.onChange}
                />
                <label className="option-label" htmlFor={id}>
                    {itemData.value}
                </label>
            </li>
        );
    }
}