import React from 'react'
import renderer from 'react-test-renderer'

import CollapsibleRow from '../CollapsibleRow'

describe('CollapsibleRow ', () => {
    describe('render', () => {
        it('isExpand = true - component is expanded', () => { 
            const props = {
                isExpand: false,
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot(); 
        });

        it('isExpand = false - component is collapsed', () => { 
            const props = {
                isExpand: false,
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot(); 
        });

        it('subject is not Html - component render correctly - show raw content', () => {
            const props = {
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot(); 
        });

        it('subject is Html - component render correctly - show content with Html format', () => { 
            const props = {
                subject: { isHtml: true, text: '<p>Subject</p>' },
                content: { isHtml: false, text: 'Content' }
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot();
        });
        
        it('content is not Html - component render correctly - show raw content', () => { 
            const props = {
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot(); 
            });

        it('content is Html - component render correctly - show content with Html format', () => { 
            const props = {
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: true, text: '<p>Content</p>' }
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot();
        });
    });

    describe('componentWillReceiveProps', () => {
        it('props is not changed - state is not changed', () => { 
            const props = {
                isExpand: false,
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };
            let component = new CollapsibleRow(props);
            component.setState = jest.fn();

            const newProps = {
                isExpand: false,
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };
            component.componentWillReceiveProps(newProps);

            expect(component.setState).not.toHaveBeenCalled();
        });

        it('props is changed - state is changed', () => { 
            const props = {
                isExpand: false,
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };
            let component = new CollapsibleRow(props);
            component.setState = jest.fn();

            const newProps = {
                isExpand: true,
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };
            component.componentWillReceiveProps(newProps);

            expect(component.setState).toHaveBeenCalled();
        });
    });

    describe('onClick', () => {
        it('Class name is changed to show/hide content', () => {
            const props = {
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' }
            };

            const component = renderer.create(<CollapsibleRow {...props} />);
            
            let tree = component.toJSON();      
            expect(tree).toMatchSnapshot();

            // expand
            tree.children[0].children[1].props.onClick();           
            tree = component.toJSON();      
            expect(tree).toMatchSnapshot();

            // collapse
            tree.children[0].children[1].props.onClick();
            tree = component.toJSON();      
            expect(tree).toMatchSnapshot();
        });

        it('onExpand function is called when expanding', () => {
            const props = {
                subject: { isHtml: false, text: 'Subject' },
                content: { isHtml: false, text: 'Content' },
                onExpand: jest.fn(),
                isExpand: true
            };

            const component = renderer.create(<CollapsibleRow {...props} />);
            
            let tree = component.toJSON();      
            expect(tree).toMatchSnapshot();

            // collapse
            tree.children[0].children[1].props.onClick();
            expect(props.onExpand).not.toHaveBeenCalled();

            // expand
            tree.children[0].children[1].props.onClick();
            expect(props.onExpand).toHaveBeenCalled();
        });
    });
});