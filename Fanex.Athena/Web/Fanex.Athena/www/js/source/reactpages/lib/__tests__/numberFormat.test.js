import NumberFormat from '../numberFormat';

describe('NumberFormat ', () => {
    describe('format', () => {
        it('value is not a number - return empty string', () => {
            const expectedResult = "";

            const value = NaN;
            const result = NumberFormat.format(value);

            expect(result).toBe(expectedResult);
        })

        it('do not pass digit param - get default digit and return correct value', () => {
            const value = 2000;
            const expectedResult = "2,000";

            const result = NumberFormat.format(value);

            expect(result).toBe(expectedResult);
        })

        it('pass digit param - return correct formatted value', () => {
            const value = 2000;
            const digit = 1;
            const expectedResult = "2,000.0";

            const result = NumberFormat.format(value, digit);

            expect(result).toBe(expectedResult);
        })

        it('digit is 3 and value is positive decimal - return rounded value correctly', () => {
            const value = 2000.5555;
            const digit = 3;
            const expectedResult = "2,000.556";

            const result = NumberFormat.format(value, digit);

            expect(result).toBe(expectedResult);
        })

        it('digit is 3 and value is negative decimal - return rounded value correctly', () => {
            const value = -2000.5555;
            const digit = 3;
            const expectedResult = "-2,000.556";

            const result = NumberFormat.format(value, digit);

            expect(result).toBe(expectedResult);
        })

        it('digit is 3 with hundred place value - return correct formatted value', () => {
            const value = 200;
            const digit = 3;
            const expectedResult = "200.000";

            const result = NumberFormat.format(value, digit);

            expect(result).toBe(expectedResult);
        })
    })

    describe('formatDecimal', () => {
        it('value is positive - return correct formatted value', () => {
            const value = 2000.23;
            const expectedResult = "2,000.23";

            const result = NumberFormat.formatDecimal(value);

            expect(result).toBe(expectedResult);
        })

        it('value is negative - return correct formatted value', () => {
            const value = -2000;
            const expectedResult = "-2,000.00";

            const result = NumberFormat.formatDecimal(value);

            expect(result).toBe(expectedResult);
        })

        it('value is positive - return rounded value correctly', () => {
            const value = 2000.345;
            const expectedResult = "2,000.35";

            const result = NumberFormat.formatDecimal(value);

            expect(result).toBe(expectedResult);
        })

        it('value is zero - return correct formatted value', () => {
            const value = 0;
            const expectedResult = "0.00";

            const result = NumberFormat.formatDecimal(value);

            expect(result).toBe(expectedResult);
        })
    })
})