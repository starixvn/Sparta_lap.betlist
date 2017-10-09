import React from 'react'
import renderer from 'react-test-renderer'

import MultipleSelect from '../MultipleSelect'

const defaultProps = {
    dataSource: [],
    keyField: "key",
    valueField: "value",
    statusField: "checked",
    noneSelectedLabel: "Select Options",
    optionAllLabel: "All",
    maxDisplayItemCount: 3,
    onChange: jest.fn(),
    hasAllOption: true
}

const defaultOption = { code: "SB", name: "Sportsbook", status: true };
const defaultDataSource = [defaultOption];

describe('MultipleSelect ', () => {
    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('Initial State', () => {
        it('has id prop - id value should be correct', () => {
            let defaultId = 1;

            const testIdProp = (idValue) => {
                const currentId = idValue || defaultId++;
                const expectedResult = `multiple-select-${currentId}`;
                const multipleSelect = renderer.create(<MultipleSelect id={idValue} {...defaultProps} />);
                const instance = multipleSelect.getInstance();

                const result = instance.id;

                expect(result).toBe(expectedResult);
            }

            const idValues = ["multipleSelectId", undefined, undefined, 100];

            idValues.forEach(testIdProp);
        })

        it('showOptionList should be false', () => {
            const multipleSelect = new MultipleSelect(defaultProps);

            const result = multipleSelect.state.showOptionList;

            expect(result).toBeFalsy();
        })

        it('dataSource prop should convert correctly', () => {
            const expectedResult = [
                { key: defaultOption.code, value: defaultOption.name, checked: defaultOption.status }
            ]

            const newProp = {
                dataSource: defaultDataSource,
                keyField: "code",
                valueField: "name",
                statusField: "status"
            }

            const props = Object.assign({}, defaultProps, newProp);
            const multipleSelect = new MultipleSelect(props);

            const result = multipleSelect.state.dataSource;

            expect(result).toEqual(expectedResult);
        })
    })

    describe('selectedItems', () => {
        it('selectedItems should return correct data', () => {
            const sportsbookOption = { key: "SB", value: "Sportsbook", checked: true };
            const expectedResult = [sportsbookOption];
            const dataSource = [sportsbookOption, { key: "BA", value: "BA", checked: false }];

            const props = Object.assign({}, defaultProps, { dataSource });
            const multipleSelect = new MultipleSelect(props);

            const result = multipleSelect.selectedItems;

            expect(result).toEqual(expectedResult);
        })
    })

    describe('onChangeHandler', () => {
        let multipleSelect;

        const sportsbookOption = { key: "SB", value: "Sportsbook", checked: true };
        const changedItem = { key: "BA", value: "BA", checked: true };

        const dataSource = [sportsbookOption, Object.assign({}, changedItem, { checked: false })];
        const newDataSource = [sportsbookOption, changedItem];

        beforeEach(() => {
            const props = Object.assign({}, defaultProps, { dataSource });
            multipleSelect = new MultipleSelect(props);

            multipleSelect._callBackToParent = jest.fn();
            multipleSelect.setState = jest.fn();

            multipleSelect.onChangeHandler(changedItem);
        })

        it('_callBackToParent should be called with correct parameter', () => {
            expect(multipleSelect._callBackToParent).toHaveBeenCalledWith(changedItem, newDataSource);
        })

        it('setState should be called with correct parameter', () => {
            const newState = { dataSource: newDataSource };

            expect(multipleSelect.setState).toHaveBeenCalledWith(newState);
        })
    })

    describe('onToggle', () => {
        it('showOptionList should be correct', () => {
            const multipleSelect = new MultipleSelect(defaultProps);
            multipleSelect.setState = jest.fn();

            multipleSelect.onToggle();

            expect(multipleSelect.setState).toHaveBeenCalled();
        })
    })

    describe('checkAllHandler', () => {
        let multipleSelect;
        const sportsbookOption = { key: "SB", value: "Sportsbook", checked: true };
        const baOption = { key: "BA", value: "BA", checked: true };

        const testCheckAll = (checked) => {
            const dataSource = [
                Object.assign({}, sportsbookOption, { checked: !checked }),
                Object.assign({}, baOption, { checked: !checked })
            ];

            const props = Object.assign({}, defaultProps, { dataSource });
            multipleSelect = new MultipleSelect(props);

            multipleSelect._callBackToParent = jest.fn();
            multipleSelect.setState = jest.fn();

            multipleSelect.checkAllHandler(checked);
        }

        it('_callBackToParent should be called with correct parameter', () => {
            const checked = true;
            const newDataSource = [
                Object.assign({}, sportsbookOption, { checked }),
                Object.assign({}, baOption, { checked })
            ];

            testCheckAll(checked);

            expect(multipleSelect._callBackToParent).toHaveBeenCalledWith(null, newDataSource);
        })

        it('uncheck All - setState should be called with correct parameter', () => {
            const checked = false;
            const newDataSource = [
                Object.assign({}, sportsbookOption, { checked }),
                Object.assign({}, baOption, { checked })
            ];

            const newState = { dataSource: newDataSource };

            testCheckAll(checked);

            expect(multipleSelect.setState).toHaveBeenCalledWith(newState);
        })

        it('check All - setState should be called with correct parameter', () => {
            const checked = true;
            const newDataSource = [
                Object.assign({}, sportsbookOption, { checked }),
                Object.assign({}, baOption, { checked })
            ];

            const newState = { dataSource: newDataSource };

            testCheckAll(checked);

            expect(multipleSelect.setState).toHaveBeenCalledWith(newState);
        })
    })

    describe('_callBackToParent', () => {
        it('call onChange function in props with correct parameters', () => {
            const expectedSelectedItemKey = "SB,BA";
            const expectedSelectedItem = { key: "Key", value: "Value", checked: true };
            const multipleSelect = new MultipleSelect(defaultProps);

            multipleSelect._getSelectedItemKey = jest.fn().mockReturnValueOnce(expectedSelectedItemKey);
            multipleSelect.props.onChange = jest.fn();

            const dataSource = [];
            multipleSelect._callBackToParent(expectedSelectedItem, dataSource);

            expect(multipleSelect.props.onChange).toHaveBeenCalledWith(expectedSelectedItem, expectedSelectedItemKey);
        })
    })
})