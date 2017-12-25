import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class Page extends Component {
    constructor(props) {
        super(props);

        this.state = {
            index: this.props.pageOption.PageIndex,
            isShow: this.props.pageOption.TotalItem > this.props.pageOption.PageList[0],
            totalPage: Math.ceil(this.props.pageOption.TotalItem/this.props.pageOption.PageSize),
            pageSize: this.props.pageOption.PageSize,
            inputValue: this.props.pageOption.PageIndex
        };
    }

    static propTypes = {
        pageOption: PropTypes.shape({
            PageIndex: PropTypes.number,
            PageSize: PropTypes.number,
            PageList: PropTypes.arrayOf(PropTypes.number),
            TotalItem: PropTypes.number
        }),
        onPaging: PropTypes.func
    };

    static defaultProps = {
        pageOption: {
            PageIndex: 1,
            PageSize: 50,
            PageList: [50, 100, 200],
            TotalItem: 0
        }
    };

    componentWillReceiveProps(nextProps) {
        if (this.props.pageOption !== nextProps.pageOption) {
            this._updateState(
                nextProps.pageOption.PageIndex,
                nextProps.pageOption.TotalItem > nextProps.pageOption.PageList[0],
                Math.ceil(nextProps.pageOption.TotalItem/nextProps.pageOption.PageSize),
                nextProps.pageOption.PageSize,
                nextProps.pageOption.PageIndex
            );
        }
    }

    goToPage(index, pageSize) {
        this._updateState(index, null, Math.ceil(this.props.pageOption.TotalItem/pageSize), pageSize, index);
        this.props.onPaging && this.props.onPaging(index, pageSize);
    }

    onKeyUpHandle = (e) => {
        const code =  e.keyCode ? e.keyCode : e.which;
        if (code === 13) {
            var value = parseInt(this.state.inputValue);
            if (value > 0 && value <= this.state.totalPage) {
                this.goToPage(value, this.state.pageSize);
            }
        }
    }

    onClickHandle(isDisable, handleFunc) {
        if(!isDisable) {
            handleFunc();
        }
    }


    _updateState(index, isShow, totalPage, pageSize, input) {
        this.setState({
            index: index !== null ? index : this.state.index,
            isShow: isShow !== null ? isShow : this.state.isShow,
            totalPage: totalPage ? totalPage : this.state.totalPage,
            pageSize: pageSize ? pageSize : this.state.pageSize,
            inputValue: input !== null ? input : this.state.inputValue
        });
    }

    _renderPageSizeSelection(isDisable, pageIndex, pageSize) {
        return (
            <select className="pagesize-select" disabled={isDisable} value={pageSize} onChange={(e) => this.goToPage(1, Number(e.target.value))}>
                {this._renderPageSizeOption(this.props.pageOption.PageList)}
	        </select>
        );
    }

    _renderPageSizeOption(pageList) {
        return pageList.map((item, index) => {
            return (<option key={`pagesize${index}`} value={item}>{item}</option>);
        });
    }

    _renderPageIcon(iconClass, isDisable, onClickFunc) {
        return (<a href="javascript:;" className={`icon ${iconClass} ${isDisable ? "disable" : ""}`} onClick={() => this.onClickHandle(isDisable, onClickFunc)}></a>);
    }

    _renderPageInput(index) {
        return (
            <div className="page-input"><span>Page </span><input type="text" className="page-form" value={index} maxLength="4" size="2"
                        onChange={(e) => this._updateState(null, null, null, null, e.target.value) }
                        onKeyUp={this.onKeyUpHandle} />
            <span>{` of ${this.state.totalPage}`} </span>
            </div>);
    }

    _renderPageInfo(index, pageSize, totalItem) {
        const viewFrom = ((index - 1) * pageSize) + 1;
        const viewTo = index * pageSize;
        return (<span className="page-info">{`View ${viewFrom}-${viewTo > totalItem ? totalItem : viewTo} of ${totalItem}`}</span>);
    }    

    render() {
        const { index, totalPage, pageSize, inputValue } = this.state;
        const isDisable = isNaN(parseInt(inputValue));

        return (
            <div className="page-container center-align" style={{ display: `${this.state.isShow ? 'block' : 'none'}` }}>
                {this._renderPageIcon("page-first", index === 1, () => this.goToPage(1, pageSize))}
                {this._renderPageIcon("page-prev", isDisable || index <= 1 || index > totalPage, () => this.goToPage(index - 1, pageSize))}
                {this._renderPageInput(inputValue)}
                {this._renderPageIcon("page-next", isDisable || index >= totalPage || index < 1, () => this.goToPage(index + 1, pageSize))}
                {this._renderPageIcon("page-last", index === totalPage, () => this.goToPage(totalPage, pageSize))}
                {this._renderPageSizeSelection(isDisable, index, pageSize)}
                {this._renderPageInfo(index, pageSize, this.props.pageOption.TotalItem)}
            </div>);
    }
}