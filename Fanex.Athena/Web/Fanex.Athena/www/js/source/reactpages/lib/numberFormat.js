export default class NumberFormat {
    static formatDecimal(value) {
        return this.format(value, 2)
    }

    static format(value, digits = 0) {
        return value.toFixed(digits).replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,")
    }
}