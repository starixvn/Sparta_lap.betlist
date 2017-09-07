import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import PropTypes from 'prop-types'
import Option from './Option'

const LABEL = "All"

export default class OptionAll extends Component {
    constructor(props) {
        super(props);

        this.state = {
            checked: false
        }
    }

    static propTypes = {
        onChange: PropTypes.func,
        label: PropTypes.string
    }

    changeHandler = ({ checked }) => {
        this.props.onChange && this.props.onChange(checked);

        this.setState({ checked: checked });
    }

    render() {
        const itemData = {
            key: "All",
            value: LABEL,
            checked: this.state.checked
        }

        return (
            <ul className="multiple-select-options multiple-select-option-all">
                <Option
                    id="multipleSelectOptionItemAll"
                    itemData={itemData}
                    onChange={this.changeHandler} />
            </ul>
        )
    }
}