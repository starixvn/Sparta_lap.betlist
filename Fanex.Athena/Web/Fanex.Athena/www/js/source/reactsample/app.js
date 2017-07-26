import React from 'react';
import ReactDOM from 'react-dom';
import FilterableProductTable from './components/FilterableProductTable.jsx';
import Data from './util/data';

const PRODUCTS = Data.products;
ReactDOM.render(
    <FilterableProductTable products={PRODUCTS} />,
    document.getElementById('app')
);