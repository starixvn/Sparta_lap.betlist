export default class NumberFormat {
    static formatDecimal(value) {
        return this.format(value, 2)
    }

    static format(value, digits = 0) {
        const roundedValue = Number(Math.round(value + 'e' + digits) + 'e-' + digits).toFixed(digits);
        return roundedValue.replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,")
    }
}