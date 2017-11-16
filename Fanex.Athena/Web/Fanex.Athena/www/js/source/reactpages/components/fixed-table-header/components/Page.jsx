import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class Page extends Component {
    constructor(props) {
        super(props);

        this.state = {
            index: this.props.pageOption.PageIndex,
            isShow: this.props.pageOption.TotalItem > this.props.pageOption.PageList[0],
            totalPage: Math.ceil(this.props.pageOption.TotalItem/this.props.pageOption.PageSize)
        };
    }

    static propTypes = {
        pageOption: PropTypes.object,
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
                Math.ceil(nextProps.pageOption.TotalItem/nextProps.pageOption.PageSize)
            );
        }
    }

    goToPage(index, size) {
        this._updateState(index, null, this.props.pageOption.TotalItem/size);
        this.props.onPaging && this.props.onPaging(index, size);
    }

    onKeyUpHandle = (e) => {
        const code =  e.keyCode ? e.keyCode : e.which;
        if (code === 13) {
            var value = parseInt(this.state.index);
            if (value > 0 && value <= this.state.totalPage) {
                this.goToPage(value, this.props.pageOption.PageSize);
            }
        }
    }

    _updateState(index, isShow, totalPage) {
        this.setState({
            index: index ? index : this.state.index,
            isShow: isShow ? isShow : this.state.isShow,
            totalPage: totalPage ? totalPage : this.state.totalPage
        });
    }

    _renderPageSizeSelection(pageIndex, pageSize, pageList) {
        return (
            <select className="pagesize-select" value={pageSize} onChange={(e) => this.goToPage(1, e.target.value)}>
                {this._renderPageSizeOption(pageList)}
	        </select>
        );
    }

    _renderPageSizeOption(pageList) {
        return pageList.map((item, index) => {
            return (<option key={`pagesize${index}`} value={item}>{item}</option>);
        });
    }

    _renderPageIcon(iconClass, isDisable, onClickFunc) {
        return (<a href="javascript:;" className={`icon ${iconClass} ${isDisable ? "disable" : ""}`} onClick={onClickFunc}></a>);
    }

    _renderPageSizeInput() {
    return (
        <div className="page-info">Page <input type="text" className="page-form" value={this.state.index} maxLength="4" size="2"
                    onChange={(e) => { this._updateState(e.target.value, null, null); }}
                    onKeyUp={this.onKeyUpHandle} />
        {` of ${this.state.totalPage}`}
        </div>);
    }

    render() {
    const { PageIndex, PageSize, PageList } = this.props.pageOption;

    return (
        <div className="page-container center-align" style={{ display: `${this.state.isShow ? 'block' : 'none'}` }}>
            {this._renderPageIcon("page-first", PageIndex <= 1, () => this.goToPage(1, PageSize))}
            {this._renderPageIcon("page-prev", PageIndex <= 1, () => this.goToPage(PageIndex - 1, PageSize))}
            {this._renderPageSizeInput()}
            {this._renderPageIcon("page-next", PageIndex >= this.state.totalPage, () => this.goToPage(PageIndex + 1, PageSize))}
            {this._renderPageIcon("page-last", PageIndex >= this.state.totalPage, () => this.goToPage(this.state.totalPage, PageSize))}
            {this._renderPageSizeSelection(PageIndex, PageSize, PageList)}
        </div>);
    }
}