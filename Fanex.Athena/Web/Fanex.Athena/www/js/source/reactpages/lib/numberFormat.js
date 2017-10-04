export default class NumberFormat {
    static formatDecimal(value) {
        return this.format(value, 2)
    }

    static format(value, digits = 0) {
        if (typeof value !== "number" || isNaN(value)) {
            return "";
        }

        const sign = value < 0 ? -1 : 1;
        const shift = Math.pow(10, digits);
        const rounded = Math.round(Math.abs(value) * shift) / shift;

        return (sign * rounded).toFixed(digits).replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,");
    }
}