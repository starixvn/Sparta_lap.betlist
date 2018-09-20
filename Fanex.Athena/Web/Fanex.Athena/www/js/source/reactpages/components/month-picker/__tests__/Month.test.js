import React from 'react'
import renderer from 'react-test-renderer'

import Month from '../Month'

describe('Month', () => {
    const defaultProps = {
        label: undefined,
        value: {
            year: 1999
        },
        min: {},
        max: {},
        onChange: undefined
    }

    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('constructor', () => {
        it('Initial state should be correct', () => {
            const component = new Month(defaultProps);
            const expectedState = {expandYear: 1999, showMonthDrop: false }

            expect(component.state).toEqual(expectedState);
        });
    })

    describe('_onChange', () => {
        const value = {year: 2000};
        let component;
        const props = Object.assign({}, defaultProps);

        beforeEach(() => {
            props.onChange = jest.fn();
            component = new Month(props);
            component.setState = jest.fn();
            component._onChange(value);
        });

        it('props.onChange() should be called with correct param', () => {
            expect(props.onChange).toHaveBeenCalledWith(value);
        })

        it('setState() should be called with correct param', () => {
            expect(component.setState).toHaveBeenCalledWith({showMonthDrop: false})
        })
    });

    describe('render', () => {
        it('_renderOption() renders correctly', () => {
            const props = Object.assign({}, defaultProps);
            const component = new Month(props);

            const renderedComponent = 
                renderer.create(<div>{component._renderOption({year: 1999, month: 1}, {year: 2004, month: 2}, {year: 2004, month: 1})}</div>);
            const tree = renderedComponent.toJSON();

            expect(tree).toMatchSnapshot();
        })

        it('showMonthDrop = true: render correct content', () => {
            const props = Object.assign({}, defaultProps);
            const component = renderer.create(<Month {...props} />);
        component.getInstance().setState({showMonthDrop: true});
            const tree = component.toJSON();

            expect(tree).toMatchSnapshot();
        })

        it('showMonthDrop = false: render correct content', () => {
            const props = Object.assign({}, defaultProps);
            const component = renderer.create(<Month {...props} />);
            component.getInstance().setState({showMonthDrop: false});
            const tree = component.toJSON();

            expect(tree).toMatchSnapshot();
        })
    });
});