webpackJsonp([7],{

/***/ 10:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_RESULT__;/*! bignumber.js v2.4.0 https://github.com/MikeMcl/bignumber.js/LICENCE */

;(function (globalObj) {
    'use strict';

    /*
      bignumber.js v2.4.0
      A JavaScript library for arbitrary-precision arithmetic.
      https://github.com/MikeMcl/bignumber.js
      Copyright (c) 2016 Michael Mclaughlin <M8ch88l@gmail.com>
      MIT Expat Licence
    */


    var BigNumber, cryptoObj, parseNumeric,
        isNumeric = /^-?(\d+(\.\d*)?|\.\d+)(e[+-]?\d+)?$/i,
        mathceil = Math.ceil,
        mathfloor = Math.floor,
        notBool = ' not a boolean or binary digit',
        roundingMode = 'rounding mode',
        tooManyDigits = 'number type has more than 15 significant digits',
        ALPHABET = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ$_',
        BASE = 1e14,
        LOG_BASE = 14,
        MAX_SAFE_INTEGER = 0x1fffffffffffff,         // 2^53 - 1
        // MAX_INT32 = 0x7fffffff,                   // 2^31 - 1
        POWS_TEN = [1, 10, 100, 1e3, 1e4, 1e5, 1e6, 1e7, 1e8, 1e9, 1e10, 1e11, 1e12, 1e13],
        SQRT_BASE = 1e7,

        /*
         * The limit on the value of DECIMAL_PLACES, TO_EXP_NEG, TO_EXP_POS, MIN_EXP, MAX_EXP, and
         * the arguments to toExponential, toFixed, toFormat, and toPrecision, beyond which an
         * exception is thrown (if ERRORS is true).
         */
        MAX = 1E9;                                   // 0 to MAX_INT32

    if ( typeof crypto != 'undefined' ) cryptoObj = crypto;


    /*
     * Create and return a BigNumber constructor.
     */
    function constructorFactory(configObj) {
        var div,

            // id tracks the caller function, so its name can be included in error messages.
            id = 0,
            P = BigNumber.prototype,
            ONE = new BigNumber(1),


            /********************************* EDITABLE DEFAULTS **********************************/


            /*
             * The default values below must be integers within the inclusive ranges stated.
             * The values can also be changed at run-time using BigNumber.config.
             */

            // The maximum number of decimal places for operations involving division.
            DECIMAL_PLACES = 20,                     // 0 to MAX

            /*
             * The rounding mode used when rounding to the above decimal places, and when using
             * toExponential, toFixed, toFormat and toPrecision, and round (default value).
             * UP         0 Away from zero.
             * DOWN       1 Towards zero.
             * CEIL       2 Towards +Infinity.
             * FLOOR      3 Towards -Infinity.
             * HALF_UP    4 Towards nearest neighbour. If equidistant, up.
             * HALF_DOWN  5 Towards nearest neighbour. If equidistant, down.
             * HALF_EVEN  6 Towards nearest neighbour. If equidistant, towards even neighbour.
             * HALF_CEIL  7 Towards nearest neighbour. If equidistant, towards +Infinity.
             * HALF_FLOOR 8 Towards nearest neighbour. If equidistant, towards -Infinity.
             */
            ROUNDING_MODE = 4,                       // 0 to 8

            // EXPONENTIAL_AT : [TO_EXP_NEG , TO_EXP_POS]

            // The exponent value at and beneath which toString returns exponential notation.
            // Number type: -7
            TO_EXP_NEG = -7,                         // 0 to -MAX

            // The exponent value at and above which toString returns exponential notation.
            // Number type: 21
            TO_EXP_POS = 21,                         // 0 to MAX

            // RANGE : [MIN_EXP, MAX_EXP]

            // The minimum exponent value, beneath which underflow to zero occurs.
            // Number type: -324  (5e-324)
            MIN_EXP = -1e7,                          // -1 to -MAX

            // The maximum exponent value, above which overflow to Infinity occurs.
            // Number type:  308  (1.7976931348623157e+308)
            // For MAX_EXP > 1e7, e.g. new BigNumber('1e100000000').plus(1) may be slow.
            MAX_EXP = 1e7,                           // 1 to MAX

            // Whether BigNumber Errors are ever thrown.
            ERRORS = true,                           // true or false

            // Change to intValidatorNoErrors if ERRORS is false.
            isValidInt = intValidatorWithErrors,     // intValidatorWithErrors/intValidatorNoErrors

            // Whether to use cryptographically-secure random number generation, if available.
            CRYPTO = false,                          // true or false

            /*
             * The modulo mode used when calculating the modulus: a mod n.
             * The quotient (q = a / n) is calculated according to the corresponding rounding mode.
             * The remainder (r) is calculated as: r = a - n * q.
             *
             * UP        0 The remainder is positive if the dividend is negative, else is negative.
             * DOWN      1 The remainder has the same sign as the dividend.
             *             This modulo mode is commonly known as 'truncated division' and is
             *             equivalent to (a % n) in JavaScript.
             * FLOOR     3 The remainder has the same sign as the divisor (Python %).
             * HALF_EVEN 6 This modulo mode implements the IEEE 754 remainder function.
             * EUCLID    9 Euclidian division. q = sign(n) * floor(a / abs(n)).
             *             The remainder is always positive.
             *
             * The truncated division, floored division, Euclidian division and IEEE 754 remainder
             * modes are commonly used for the modulus operation.
             * Although the other rounding modes can also be used, they may not give useful results.
             */
            MODULO_MODE = 1,                         // 0 to 9

            // The maximum number of significant digits of the result of the toPower operation.
            // If POW_PRECISION is 0, there will be unlimited significant digits.
            POW_PRECISION = 100,                     // 0 to MAX

            // The format specification used by the BigNumber.prototype.toFormat method.
            FORMAT = {
                decimalSeparator: '.',
                groupSeparator: ',',
                groupSize: 3,
                secondaryGroupSize: 0,
                fractionGroupSeparator: '\xA0',      // non-breaking space
                fractionGroupSize: 0
            };


        /******************************************************************************************/


        // CONSTRUCTOR


        /*
         * The BigNumber constructor and exported function.
         * Create and return a new instance of a BigNumber object.
         *
         * n {number|string|BigNumber} A numeric value.
         * [b] {number} The base of n. Integer, 2 to 64 inclusive.
         */
        function BigNumber( n, b ) {
            var c, e, i, num, len, str,
                x = this;

            // Enable constructor usage without new.
            if ( !( x instanceof BigNumber ) ) {

                // 'BigNumber() constructor call without new: {n}'
                if (ERRORS) raise( 26, 'constructor call without new', n );
                return new BigNumber( n, b );
            }

            // 'new BigNumber() base not an integer: {b}'
            // 'new BigNumber() base out of range: {b}'
            if ( b == null || !isValidInt( b, 2, 64, id, 'base' ) ) {

                // Duplicate.
                if ( n instanceof BigNumber ) {
                    x.s = n.s;
                    x.e = n.e;
                    x.c = ( n = n.c ) ? n.slice() : n;
                    id = 0;
                    return;
                }

                if ( ( num = typeof n == 'number' ) && n * 0 == 0 ) {
                    x.s = 1 / n < 0 ? ( n = -n, -1 ) : 1;

                    // Fast path for integers.
                    if ( n === ~~n ) {
                        for ( e = 0, i = n; i >= 10; i /= 10, e++ );
                        x.e = e;
                        x.c = [n];
                        id = 0;
                        return;
                    }

                    str = n + '';
                } else {
                    if ( !isNumeric.test( str = n + '' ) ) return parseNumeric( x, str, num );
                    x.s = str.charCodeAt(0) === 45 ? ( str = str.slice(1), -1 ) : 1;
                }
            } else {
                b = b | 0;
                str = n + '';

                // Ensure return value is rounded to DECIMAL_PLACES as with other bases.
                // Allow exponential notation to be used with base 10 argument.
                if ( b == 10 ) {
                    x = new BigNumber( n instanceof BigNumber ? n : str );
                    return round( x, DECIMAL_PLACES + x.e + 1, ROUNDING_MODE );
                }

                // Avoid potential interpretation of Infinity and NaN as base 44+ values.
                // Any number in exponential form will fail due to the [Ee][+-].
                if ( ( num = typeof n == 'number' ) && n * 0 != 0 ||
                  !( new RegExp( '^-?' + ( c = '[' + ALPHABET.slice( 0, b ) + ']+' ) +
                    '(?:\\.' + c + ')?$',b < 37 ? 'i' : '' ) ).test(str) ) {
                    return parseNumeric( x, str, num, b );
                }

                if (num) {
                    x.s = 1 / n < 0 ? ( str = str.slice(1), -1 ) : 1;

                    if ( ERRORS && str.replace( /^0\.0*|\./, '' ).length > 15 ) {

                        // 'new BigNumber() number type has more than 15 significant digits: {n}'
                        raise( id, tooManyDigits, n );
                    }

                    // Prevent later check for length on converted number.
                    num = false;
                } else {
                    x.s = str.charCodeAt(0) === 45 ? ( str = str.slice(1), -1 ) : 1;
                }

                str = convertBase( str, 10, b, x.s );
            }

            // Decimal point?
            if ( ( e = str.indexOf('.') ) > -1 ) str = str.replace( '.', '' );

            // Exponential form?
            if ( ( i = str.search( /e/i ) ) > 0 ) {

                // Determine exponent.
                if ( e < 0 ) e = i;
                e += +str.slice( i + 1 );
                str = str.substring( 0, i );
            } else if ( e < 0 ) {

                // Integer.
                e = str.length;
            }

            // Determine leading zeros.
            for ( i = 0; str.charCodeAt(i) === 48; i++ );

            // Determine trailing zeros.
            for ( len = str.length; str.charCodeAt(--len) === 48; );
            str = str.slice( i, len + 1 );

            if (str) {
                len = str.length;

                // Disallow numbers with over 15 significant digits if number type.
                // 'new BigNumber() number type has more than 15 significant digits: {n}'
                if ( num && ERRORS && len > 15 && ( n > MAX_SAFE_INTEGER || n !== mathfloor(n) ) ) {
                    raise( id, tooManyDigits, x.s * n );
                }

                e = e - i - 1;

                 // Overflow?
                if ( e > MAX_EXP ) {

                    // Infinity.
                    x.c = x.e = null;

                // Underflow?
                } else if ( e < MIN_EXP ) {

                    // Zero.
                    x.c = [ x.e = 0 ];
                } else {
                    x.e = e;
                    x.c = [];

                    // Transform base

                    // e is the base 10 exponent.
                    // i is where to slice str to get the first element of the coefficient array.
                    i = ( e + 1 ) % LOG_BASE;
                    if ( e < 0 ) i += LOG_BASE;

                    if ( i < len ) {
                        if (i) x.c.push( +str.slice( 0, i ) );

                        for ( len -= LOG_BASE; i < len; ) {
                            x.c.push( +str.slice( i, i += LOG_BASE ) );
                        }

                        str = str.slice(i);
                        i = LOG_BASE - str.length;
                    } else {
                        i -= len;
                    }

                    for ( ; i--; str += '0' );
                    x.c.push( +str );
                }
            } else {

                // Zero.
                x.c = [ x.e = 0 ];
            }

            id = 0;
        }


        // CONSTRUCTOR PROPERTIES


        BigNumber.another = constructorFactory;

        BigNumber.ROUND_UP = 0;
        BigNumber.ROUND_DOWN = 1;
        BigNumber.ROUND_CEIL = 2;
        BigNumber.ROUND_FLOOR = 3;
        BigNumber.ROUND_HALF_UP = 4;
        BigNumber.ROUND_HALF_DOWN = 5;
        BigNumber.ROUND_HALF_EVEN = 6;
        BigNumber.ROUND_HALF_CEIL = 7;
        BigNumber.ROUND_HALF_FLOOR = 8;
        BigNumber.EUCLID = 9;


        /*
         * Configure infrequently-changing library-wide settings.
         *
         * Accept an object or an argument list, with one or many of the following properties or
         * parameters respectively:
         *
         *   DECIMAL_PLACES  {number}  Integer, 0 to MAX inclusive
         *   ROUNDING_MODE   {number}  Integer, 0 to 8 inclusive
         *   EXPONENTIAL_AT  {number|number[]}  Integer, -MAX to MAX inclusive or
         *                                      [integer -MAX to 0 incl., 0 to MAX incl.]
         *   RANGE           {number|number[]}  Non-zero integer, -MAX to MAX inclusive or
         *                                      [integer -MAX to -1 incl., integer 1 to MAX incl.]
         *   ERRORS          {boolean|number}   true, false, 1 or 0
         *   CRYPTO          {boolean|number}   true, false, 1 or 0
         *   MODULO_MODE     {number}           0 to 9 inclusive
         *   POW_PRECISION   {number}           0 to MAX inclusive
         *   FORMAT          {object}           See BigNumber.prototype.toFormat
         *      decimalSeparator       {string}
         *      groupSeparator         {string}
         *      groupSize              {number}
         *      secondaryGroupSize     {number}
         *      fractionGroupSeparator {string}
         *      fractionGroupSize      {number}
         *
         * (The values assigned to the above FORMAT object properties are not checked for validity.)
         *
         * E.g.
         * BigNumber.config(20, 4) is equivalent to
         * BigNumber.config({ DECIMAL_PLACES : 20, ROUNDING_MODE : 4 })
         *
         * Ignore properties/parameters set to null or undefined.
         * Return an object with the properties current values.
         */
        BigNumber.config = function () {
            var v, p,
                i = 0,
                r = {},
                a = arguments,
                o = a[0],
                has = o && typeof o == 'object'
                  ? function () { if ( o.hasOwnProperty(p) ) return ( v = o[p] ) != null; }
                  : function () { if ( a.length > i ) return ( v = a[i++] ) != null; };

            // DECIMAL_PLACES {number} Integer, 0 to MAX inclusive.
            // 'config() DECIMAL_PLACES not an integer: {v}'
            // 'config() DECIMAL_PLACES out of range: {v}'
            if ( has( p = 'DECIMAL_PLACES' ) && isValidInt( v, 0, MAX, 2, p ) ) {
                DECIMAL_PLACES = v | 0;
            }
            r[p] = DECIMAL_PLACES;

            // ROUNDING_MODE {number} Integer, 0 to 8 inclusive.
            // 'config() ROUNDING_MODE not an integer: {v}'
            // 'config() ROUNDING_MODE out of range: {v}'
            if ( has( p = 'ROUNDING_MODE' ) && isValidInt( v, 0, 8, 2, p ) ) {
                ROUNDING_MODE = v | 0;
            }
            r[p] = ROUNDING_MODE;

            // EXPONENTIAL_AT {number|number[]}
            // Integer, -MAX to MAX inclusive or [integer -MAX to 0 inclusive, 0 to MAX inclusive].
            // 'config() EXPONENTIAL_AT not an integer: {v}'
            // 'config() EXPONENTIAL_AT out of range: {v}'
            if ( has( p = 'EXPONENTIAL_AT' ) ) {

                if ( isArray(v) ) {
                    if ( isValidInt( v[0], -MAX, 0, 2, p ) && isValidInt( v[1], 0, MAX, 2, p ) ) {
                        TO_EXP_NEG = v[0] | 0;
                        TO_EXP_POS = v[1] | 0;
                    }
                } else if ( isValidInt( v, -MAX, MAX, 2, p ) ) {
                    TO_EXP_NEG = -( TO_EXP_POS = ( v < 0 ? -v : v ) | 0 );
                }
            }
            r[p] = [ TO_EXP_NEG, TO_EXP_POS ];

            // RANGE {number|number[]} Non-zero integer, -MAX to MAX inclusive or
            // [integer -MAX to -1 inclusive, integer 1 to MAX inclusive].
            // 'config() RANGE not an integer: {v}'
            // 'config() RANGE cannot be zero: {v}'
            // 'config() RANGE out of range: {v}'
            if ( has( p = 'RANGE' ) ) {

                if ( isArray(v) ) {
                    if ( isValidInt( v[0], -MAX, -1, 2, p ) && isValidInt( v[1], 1, MAX, 2, p ) ) {
                        MIN_EXP = v[0] | 0;
                        MAX_EXP = v[1] | 0;
                    }
                } else if ( isValidInt( v, -MAX, MAX, 2, p ) ) {
                    if ( v | 0 ) MIN_EXP = -( MAX_EXP = ( v < 0 ? -v : v ) | 0 );
                    else if (ERRORS) raise( 2, p + ' cannot be zero', v );
                }
            }
            r[p] = [ MIN_EXP, MAX_EXP ];

            // ERRORS {boolean|number} true, false, 1 or 0.
            // 'config() ERRORS not a boolean or binary digit: {v}'
            if ( has( p = 'ERRORS' ) ) {

                if ( v === !!v || v === 1 || v === 0 ) {
                    id = 0;
                    isValidInt = ( ERRORS = !!v ) ? intValidatorWithErrors : intValidatorNoErrors;
                } else if (ERRORS) {
                    raise( 2, p + notBool, v );
                }
            }
            r[p] = ERRORS;

            // CRYPTO {boolean|number} true, false, 1 or 0.
            // 'config() CRYPTO not a boolean or binary digit: {v}'
            // 'config() crypto unavailable: {crypto}'
            if ( has( p = 'CRYPTO' ) ) {

                if ( v === !!v || v === 1 || v === 0 ) {
                    CRYPTO = !!( v && cryptoObj );
                    if ( v && !CRYPTO && ERRORS ) raise( 2, 'crypto unavailable', cryptoObj );
                } else if (ERRORS) {
                    raise( 2, p + notBool, v );
                }
            }
            r[p] = CRYPTO;

            // MODULO_MODE {number} Integer, 0 to 9 inclusive.
            // 'config() MODULO_MODE not an integer: {v}'
            // 'config() MODULO_MODE out of range: {v}'
            if ( has( p = 'MODULO_MODE' ) && isValidInt( v, 0, 9, 2, p ) ) {
                MODULO_MODE = v | 0;
            }
            r[p] = MODULO_MODE;

            // POW_PRECISION {number} Integer, 0 to MAX inclusive.
            // 'config() POW_PRECISION not an integer: {v}'
            // 'config() POW_PRECISION out of range: {v}'
            if ( has( p = 'POW_PRECISION' ) && isValidInt( v, 0, MAX, 2, p ) ) {
                POW_PRECISION = v | 0;
            }
            r[p] = POW_PRECISION;

            // FORMAT {object}
            // 'config() FORMAT not an object: {v}'
            if ( has( p = 'FORMAT' ) ) {

                if ( typeof v == 'object' ) {
                    FORMAT = v;
                } else if (ERRORS) {
                    raise( 2, p + ' not an object', v );
                }
            }
            r[p] = FORMAT;

            return r;
        };


        /*
         * Return a new BigNumber whose value is the maximum of the arguments.
         *
         * arguments {number|string|BigNumber}
         */
        BigNumber.max = function () { return maxOrMin( arguments, P.lt ); };


        /*
         * Return a new BigNumber whose value is the minimum of the arguments.
         *
         * arguments {number|string|BigNumber}
         */
        BigNumber.min = function () { return maxOrMin( arguments, P.gt ); };


        /*
         * Return a new BigNumber with a random value equal to or greater than 0 and less than 1,
         * and with dp, or DECIMAL_PLACES if dp is omitted, decimal places (or less if trailing
         * zeros are produced).
         *
         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
         *
         * 'random() decimal places not an integer: {dp}'
         * 'random() decimal places out of range: {dp}'
         * 'random() crypto unavailable: {crypto}'
         */
        BigNumber.random = (function () {
            var pow2_53 = 0x20000000000000;

            // Return a 53 bit integer n, where 0 <= n < 9007199254740992.
            // Check if Math.random() produces more than 32 bits of randomness.
            // If it does, assume at least 53 bits are produced, otherwise assume at least 30 bits.
            // 0x40000000 is 2^30, 0x800000 is 2^23, 0x1fffff is 2^21 - 1.
            var random53bitInt = (Math.random() * pow2_53) & 0x1fffff
              ? function () { return mathfloor( Math.random() * pow2_53 ); }
              : function () { return ((Math.random() * 0x40000000 | 0) * 0x800000) +
                  (Math.random() * 0x800000 | 0); };

            return function (dp) {
                var a, b, e, k, v,
                    i = 0,
                    c = [],
                    rand = new BigNumber(ONE);

                dp = dp == null || !isValidInt( dp, 0, MAX, 14 ) ? DECIMAL_PLACES : dp | 0;
                k = mathceil( dp / LOG_BASE );

                if (CRYPTO) {

                    // Browsers supporting crypto.getRandomValues.
                    if ( cryptoObj && cryptoObj.getRandomValues ) {

                        a = cryptoObj.getRandomValues( new Uint32Array( k *= 2 ) );

                        for ( ; i < k; ) {

                            // 53 bits:
                            // ((Math.pow(2, 32) - 1) * Math.pow(2, 21)).toString(2)
                            // 11111 11111111 11111111 11111111 11100000 00000000 00000000
                            // ((Math.pow(2, 32) - 1) >>> 11).toString(2)
                            //                                     11111 11111111 11111111
                            // 0x20000 is 2^21.
                            v = a[i] * 0x20000 + (a[i + 1] >>> 11);

                            // Rejection sampling:
                            // 0 <= v < 9007199254740992
                            // Probability that v >= 9e15, is
                            // 7199254740992 / 9007199254740992 ~= 0.0008, i.e. 1 in 1251
                            if ( v >= 9e15 ) {
                                b = cryptoObj.getRandomValues( new Uint32Array(2) );
                                a[i] = b[0];
                                a[i + 1] = b[1];
                            } else {

                                // 0 <= v <= 8999999999999999
                                // 0 <= (v % 1e14) <= 99999999999999
                                c.push( v % 1e14 );
                                i += 2;
                            }
                        }
                        i = k / 2;

                    // Node.js supporting crypto.randomBytes.
                    } else if ( cryptoObj && cryptoObj.randomBytes ) {

                        // buffer
                        a = cryptoObj.randomBytes( k *= 7 );

                        for ( ; i < k; ) {

                            // 0x1000000000000 is 2^48, 0x10000000000 is 2^40
                            // 0x100000000 is 2^32, 0x1000000 is 2^24
                            // 11111 11111111 11111111 11111111 11111111 11111111 11111111
                            // 0 <= v < 9007199254740992
                            v = ( ( a[i] & 31 ) * 0x1000000000000 ) + ( a[i + 1] * 0x10000000000 ) +
                                  ( a[i + 2] * 0x100000000 ) + ( a[i + 3] * 0x1000000 ) +
                                  ( a[i + 4] << 16 ) + ( a[i + 5] << 8 ) + a[i + 6];

                            if ( v >= 9e15 ) {
                                cryptoObj.randomBytes(7).copy( a, i );
                            } else {

                                // 0 <= (v % 1e14) <= 99999999999999
                                c.push( v % 1e14 );
                                i += 7;
                            }
                        }
                        i = k / 7;
                    } else if (ERRORS) {
                        raise( 14, 'crypto unavailable', cryptoObj );
                    }
                }

                // Use Math.random: CRYPTO is false or crypto is unavailable and ERRORS is false.
                if (!i) {

                    for ( ; i < k; ) {
                        v = random53bitInt();
                        if ( v < 9e15 ) c[i++] = v % 1e14;
                    }
                }

                k = c[--i];
                dp %= LOG_BASE;

                // Convert trailing digits to zeros according to dp.
                if ( k && dp ) {
                    v = POWS_TEN[LOG_BASE - dp];
                    c[i] = mathfloor( k / v ) * v;
                }

                // Remove trailing elements which are zero.
                for ( ; c[i] === 0; c.pop(), i-- );

                // Zero?
                if ( i < 0 ) {
                    c = [ e = 0 ];
                } else {

                    // Remove leading elements which are zero and adjust exponent accordingly.
                    for ( e = -1 ; c[0] === 0; c.shift(), e -= LOG_BASE);

                    // Count the digits of the first element of c to determine leading zeros, and...
                    for ( i = 1, v = c[0]; v >= 10; v /= 10, i++);

                    // adjust the exponent accordingly.
                    if ( i < LOG_BASE ) e -= LOG_BASE - i;
                }

                rand.e = e;
                rand.c = c;
                return rand;
            };
        })();


        // PRIVATE FUNCTIONS


        // Convert a numeric string of baseIn to a numeric string of baseOut.
        function convertBase( str, baseOut, baseIn, sign ) {
            var d, e, k, r, x, xc, y,
                i = str.indexOf( '.' ),
                dp = DECIMAL_PLACES,
                rm = ROUNDING_MODE;

            if ( baseIn < 37 ) str = str.toLowerCase();

            // Non-integer.
            if ( i >= 0 ) {
                k = POW_PRECISION;

                // Unlimited precision.
                POW_PRECISION = 0;
                str = str.replace( '.', '' );
                y = new BigNumber(baseIn);
                x = y.pow( str.length - i );
                POW_PRECISION = k;

                // Convert str as if an integer, then restore the fraction part by dividing the
                // result by its base raised to a power.
                y.c = toBaseOut( toFixedPoint( coeffToString( x.c ), x.e ), 10, baseOut );
                y.e = y.c.length;
            }

            // Convert the number as integer.
            xc = toBaseOut( str, baseIn, baseOut );
            e = k = xc.length;

            // Remove trailing zeros.
            for ( ; xc[--k] == 0; xc.pop() );
            if ( !xc[0] ) return '0';

            if ( i < 0 ) {
                --e;
            } else {
                x.c = xc;
                x.e = e;

                // sign is needed for correct rounding.
                x.s = sign;
                x = div( x, y, dp, rm, baseOut );
                xc = x.c;
                r = x.r;
                e = x.e;
            }

            d = e + dp + 1;

            // The rounding digit, i.e. the digit to the right of the digit that may be rounded up.
            i = xc[d];
            k = baseOut / 2;
            r = r || d < 0 || xc[d + 1] != null;

            r = rm < 4 ? ( i != null || r ) && ( rm == 0 || rm == ( x.s < 0 ? 3 : 2 ) )
                       : i > k || i == k &&( rm == 4 || r || rm == 6 && xc[d - 1] & 1 ||
                         rm == ( x.s < 0 ? 8 : 7 ) );

            if ( d < 1 || !xc[0] ) {

                // 1^-dp or 0.
                str = r ? toFixedPoint( '1', -dp ) : '0';
            } else {
                xc.length = d;

                if (r) {

                    // Rounding up may mean the previous digit has to be rounded up and so on.
                    for ( --baseOut; ++xc[--d] > baseOut; ) {
                        xc[d] = 0;

                        if ( !d ) {
                            ++e;
                            xc.unshift(1);
                        }
                    }
                }

                // Determine trailing zeros.
                for ( k = xc.length; !xc[--k]; );

                // E.g. [4, 11, 15] becomes 4bf.
                for ( i = 0, str = ''; i <= k; str += ALPHABET.charAt( xc[i++] ) );
                str = toFixedPoint( str, e );
            }

            // The caller will add the sign.
            return str;
        }


        // Perform division in the specified base. Called by div and convertBase.
        div = (function () {

            // Assume non-zero x and k.
            function multiply( x, k, base ) {
                var m, temp, xlo, xhi,
                    carry = 0,
                    i = x.length,
                    klo = k % SQRT_BASE,
                    khi = k / SQRT_BASE | 0;

                for ( x = x.slice(); i--; ) {
                    xlo = x[i] % SQRT_BASE;
                    xhi = x[i] / SQRT_BASE | 0;
                    m = khi * xlo + xhi * klo;
                    temp = klo * xlo + ( ( m % SQRT_BASE ) * SQRT_BASE ) + carry;
                    carry = ( temp / base | 0 ) + ( m / SQRT_BASE | 0 ) + khi * xhi;
                    x[i] = temp % base;
                }

                if (carry) x.unshift(carry);

                return x;
            }

            function compare( a, b, aL, bL ) {
                var i, cmp;

                if ( aL != bL ) {
                    cmp = aL > bL ? 1 : -1;
                } else {

                    for ( i = cmp = 0; i < aL; i++ ) {

                        if ( a[i] != b[i] ) {
                            cmp = a[i] > b[i] ? 1 : -1;
                            break;
                        }
                    }
                }
                return cmp;
            }

            function subtract( a, b, aL, base ) {
                var i = 0;

                // Subtract b from a.
                for ( ; aL--; ) {
                    a[aL] -= i;
                    i = a[aL] < b[aL] ? 1 : 0;
                    a[aL] = i * base + a[aL] - b[aL];
                }

                // Remove leading zeros.
                for ( ; !a[0] && a.length > 1; a.shift() );
            }

            // x: dividend, y: divisor.
            return function ( x, y, dp, rm, base ) {
                var cmp, e, i, more, n, prod, prodL, q, qc, rem, remL, rem0, xi, xL, yc0,
                    yL, yz,
                    s = x.s == y.s ? 1 : -1,
                    xc = x.c,
                    yc = y.c;

                // Either NaN, Infinity or 0?
                if ( !xc || !xc[0] || !yc || !yc[0] ) {

                    return new BigNumber(

                      // Return NaN if either NaN, or both Infinity or 0.
                      !x.s || !y.s || ( xc ? yc && xc[0] == yc[0] : !yc ) ? NaN :

                        // Return ±0 if x is ±0 or y is ±Infinity, or return ±Infinity as y is ±0.
                        xc && xc[0] == 0 || !yc ? s * 0 : s / 0
                    );
                }

                q = new BigNumber(s);
                qc = q.c = [];
                e = x.e - y.e;
                s = dp + e + 1;

                if ( !base ) {
                    base = BASE;
                    e = bitFloor( x.e / LOG_BASE ) - bitFloor( y.e / LOG_BASE );
                    s = s / LOG_BASE | 0;
                }

                // Result exponent may be one less then the current value of e.
                // The coefficients of the BigNumbers from convertBase may have trailing zeros.
                for ( i = 0; yc[i] == ( xc[i] || 0 ); i++ );
                if ( yc[i] > ( xc[i] || 0 ) ) e--;

                if ( s < 0 ) {
                    qc.push(1);
                    more = true;
                } else {
                    xL = xc.length;
                    yL = yc.length;
                    i = 0;
                    s += 2;

                    // Normalise xc and yc so highest order digit of yc is >= base / 2.

                    n = mathfloor( base / ( yc[0] + 1 ) );

                    // Not necessary, but to handle odd bases where yc[0] == ( base / 2 ) - 1.
                    // if ( n > 1 || n++ == 1 && yc[0] < base / 2 ) {
                    if ( n > 1 ) {
                        yc = multiply( yc, n, base );
                        xc = multiply( xc, n, base );
                        yL = yc.length;
                        xL = xc.length;
                    }

                    xi = yL;
                    rem = xc.slice( 0, yL );
                    remL = rem.length;

                    // Add zeros to make remainder as long as divisor.
                    for ( ; remL < yL; rem[remL++] = 0 );
                    yz = yc.slice();
                    yz.unshift(0);
                    yc0 = yc[0];
                    if ( yc[1] >= base / 2 ) yc0++;
                    // Not necessary, but to prevent trial digit n > base, when using base 3.
                    // else if ( base == 3 && yc0 == 1 ) yc0 = 1 + 1e-15;

                    do {
                        n = 0;

                        // Compare divisor and remainder.
                        cmp = compare( yc, rem, yL, remL );

                        // If divisor < remainder.
                        if ( cmp < 0 ) {

                            // Calculate trial digit, n.

                            rem0 = rem[0];
                            if ( yL != remL ) rem0 = rem0 * base + ( rem[1] || 0 );

                            // n is how many times the divisor goes into the current remainder.
                            n = mathfloor( rem0 / yc0 );

                            //  Algorithm:
                            //  1. product = divisor * trial digit (n)
                            //  2. if product > remainder: product -= divisor, n--
                            //  3. remainder -= product
                            //  4. if product was < remainder at 2:
                            //    5. compare new remainder and divisor
                            //    6. If remainder > divisor: remainder -= divisor, n++

                            if ( n > 1 ) {

                                // n may be > base only when base is 3.
                                if (n >= base) n = base - 1;

                                // product = divisor * trial digit.
                                prod = multiply( yc, n, base );
                                prodL = prod.length;
                                remL = rem.length;

                                // Compare product and remainder.
                                // If product > remainder.
                                // Trial digit n too high.
                                // n is 1 too high about 5% of the time, and is not known to have
                                // ever been more than 1 too high.
                                while ( compare( prod, rem, prodL, remL ) == 1 ) {
                                    n--;

                                    // Subtract divisor from product.
                                    subtract( prod, yL < prodL ? yz : yc, prodL, base );
                                    prodL = prod.length;
                                    cmp = 1;
                                }
                            } else {

                                // n is 0 or 1, cmp is -1.
                                // If n is 0, there is no need to compare yc and rem again below,
                                // so change cmp to 1 to avoid it.
                                // If n is 1, leave cmp as -1, so yc and rem are compared again.
                                if ( n == 0 ) {

                                    // divisor < remainder, so n must be at least 1.
                                    cmp = n = 1;
                                }

                                // product = divisor
                                prod = yc.slice();
                                prodL = prod.length;
                            }

                            if ( prodL < remL ) prod.unshift(0);

                            // Subtract product from remainder.
                            subtract( rem, prod, remL, base );
                            remL = rem.length;

                             // If product was < remainder.
                            if ( cmp == -1 ) {

                                // Compare divisor and new remainder.
                                // If divisor < new remainder, subtract divisor from remainder.
                                // Trial digit n too low.
                                // n is 1 too low about 5% of the time, and very rarely 2 too low.
                                while ( compare( yc, rem, yL, remL ) < 1 ) {
                                    n++;

                                    // Subtract divisor from remainder.
                                    subtract( rem, yL < remL ? yz : yc, remL, base );
                                    remL = rem.length;
                                }
                            }
                        } else if ( cmp === 0 ) {
                            n++;
                            rem = [0];
                        } // else cmp === 1 and n will be 0

                        // Add the next digit, n, to the result array.
                        qc[i++] = n;

                        // Update the remainder.
                        if ( rem[0] ) {
                            rem[remL++] = xc[xi] || 0;
                        } else {
                            rem = [ xc[xi] ];
                            remL = 1;
                        }
                    } while ( ( xi++ < xL || rem[0] != null ) && s-- );

                    more = rem[0] != null;

                    // Leading zero?
                    if ( !qc[0] ) qc.shift();
                }

                if ( base == BASE ) {

                    // To calculate q.e, first get the number of digits of qc[0].
                    for ( i = 1, s = qc[0]; s >= 10; s /= 10, i++ );
                    round( q, dp + ( q.e = i + e * LOG_BASE - 1 ) + 1, rm, more );

                // Caller is convertBase.
                } else {
                    q.e = e;
                    q.r = +more;
                }

                return q;
            };
        })();


        /*
         * Return a string representing the value of BigNumber n in fixed-point or exponential
         * notation rounded to the specified decimal places or significant digits.
         *
         * n is a BigNumber.
         * i is the index of the last digit required (i.e. the digit that may be rounded up).
         * rm is the rounding mode.
         * caller is caller id: toExponential 19, toFixed 20, toFormat 21, toPrecision 24.
         */
        function format( n, i, rm, caller ) {
            var c0, e, ne, len, str;

            rm = rm != null && isValidInt( rm, 0, 8, caller, roundingMode )
              ? rm | 0 : ROUNDING_MODE;

            if ( !n.c ) return n.toString();
            c0 = n.c[0];
            ne = n.e;

            if ( i == null ) {
                str = coeffToString( n.c );
                str = caller == 19 || caller == 24 && ne <= TO_EXP_NEG
                  ? toExponential( str, ne )
                  : toFixedPoint( str, ne );
            } else {
                n = round( new BigNumber(n), i, rm );

                // n.e may have changed if the value was rounded up.
                e = n.e;

                str = coeffToString( n.c );
                len = str.length;

                // toPrecision returns exponential notation if the number of significant digits
                // specified is less than the number of digits necessary to represent the integer
                // part of the value in fixed-point notation.

                // Exponential notation.
                if ( caller == 19 || caller == 24 && ( i <= e || e <= TO_EXP_NEG ) ) {

                    // Append zeros?
                    for ( ; len < i; str += '0', len++ );
                    str = toExponential( str, e );

                // Fixed-point notation.
                } else {
                    i -= ne;
                    str = toFixedPoint( str, e );

                    // Append zeros?
                    if ( e + 1 > len ) {
                        if ( --i > 0 ) for ( str += '.'; i--; str += '0' );
                    } else {
                        i += e - len;
                        if ( i > 0 ) {
                            if ( e + 1 == len ) str += '.';
                            for ( ; i--; str += '0' );
                        }
                    }
                }
            }

            return n.s < 0 && c0 ? '-' + str : str;
        }


        // Handle BigNumber.max and BigNumber.min.
        function maxOrMin( args, method ) {
            var m, n,
                i = 0;

            if ( isArray( args[0] ) ) args = args[0];
            m = new BigNumber( args[0] );

            for ( ; ++i < args.length; ) {
                n = new BigNumber( args[i] );

                // If any number is NaN, return NaN.
                if ( !n.s ) {
                    m = n;
                    break;
                } else if ( method.call( m, n ) ) {
                    m = n;
                }
            }

            return m;
        }


        /*
         * Return true if n is an integer in range, otherwise throw.
         * Use for argument validation when ERRORS is true.
         */
        function intValidatorWithErrors( n, min, max, caller, name ) {
            if ( n < min || n > max || n != truncate(n) ) {
                raise( caller, ( name || 'decimal places' ) +
                  ( n < min || n > max ? ' out of range' : ' not an integer' ), n );
            }

            return true;
        }


        /*
         * Strip trailing zeros, calculate base 10 exponent and check against MIN_EXP and MAX_EXP.
         * Called by minus, plus and times.
         */
        function normalise( n, c, e ) {
            var i = 1,
                j = c.length;

             // Remove trailing zeros.
            for ( ; !c[--j]; c.pop() );

            // Calculate the base 10 exponent. First get the number of digits of c[0].
            for ( j = c[0]; j >= 10; j /= 10, i++ );

            // Overflow?
            if ( ( e = i + e * LOG_BASE - 1 ) > MAX_EXP ) {

                // Infinity.
                n.c = n.e = null;

            // Underflow?
            } else if ( e < MIN_EXP ) {

                // Zero.
                n.c = [ n.e = 0 ];
            } else {
                n.e = e;
                n.c = c;
            }

            return n;
        }


        // Handle values that fail the validity test in BigNumber.
        parseNumeric = (function () {
            var basePrefix = /^(-?)0([xbo])(?=\w[\w.]*$)/i,
                dotAfter = /^([^.]+)\.$/,
                dotBefore = /^\.([^.]+)$/,
                isInfinityOrNaN = /^-?(Infinity|NaN)$/,
                whitespaceOrPlus = /^\s*\+(?=[\w.])|^\s+|\s+$/g;

            return function ( x, str, num, b ) {
                var base,
                    s = num ? str : str.replace( whitespaceOrPlus, '' );

                // No exception on ±Infinity or NaN.
                if ( isInfinityOrNaN.test(s) ) {
                    x.s = isNaN(s) ? null : s < 0 ? -1 : 1;
                } else {
                    if ( !num ) {

                        // basePrefix = /^(-?)0([xbo])(?=\w[\w.]*$)/i
                        s = s.replace( basePrefix, function ( m, p1, p2 ) {
                            base = ( p2 = p2.toLowerCase() ) == 'x' ? 16 : p2 == 'b' ? 2 : 8;
                            return !b || b == base ? p1 : m;
                        });

                        if (b) {
                            base = b;

                            // E.g. '1.' to '1', '.1' to '0.1'
                            s = s.replace( dotAfter, '$1' ).replace( dotBefore, '0.$1' );
                        }

                        if ( str != s ) return new BigNumber( s, base );
                    }

                    // 'new BigNumber() not a number: {n}'
                    // 'new BigNumber() not a base {b} number: {n}'
                    if (ERRORS) raise( id, 'not a' + ( b ? ' base ' + b : '' ) + ' number', str );
                    x.s = null;
                }

                x.c = x.e = null;
                id = 0;
            }
        })();


        // Throw a BigNumber Error.
        function raise( caller, msg, val ) {
            var error = new Error( [
                'new BigNumber',     // 0
                'cmp',               // 1
                'config',            // 2
                'div',               // 3
                'divToInt',          // 4
                'eq',                // 5
                'gt',                // 6
                'gte',               // 7
                'lt',                // 8
                'lte',               // 9
                'minus',             // 10
                'mod',               // 11
                'plus',              // 12
                'precision',         // 13
                'random',            // 14
                'round',             // 15
                'shift',             // 16
                'times',             // 17
                'toDigits',          // 18
                'toExponential',     // 19
                'toFixed',           // 20
                'toFormat',          // 21
                'toFraction',        // 22
                'pow',               // 23
                'toPrecision',       // 24
                'toString',          // 25
                'BigNumber'          // 26
            ][caller] + '() ' + msg + ': ' + val );

            error.name = 'BigNumber Error';
            id = 0;
            throw error;
        }


        /*
         * Round x to sd significant digits using rounding mode rm. Check for over/under-flow.
         * If r is truthy, it is known that there are more digits after the rounding digit.
         */
        function round( x, sd, rm, r ) {
            var d, i, j, k, n, ni, rd,
                xc = x.c,
                pows10 = POWS_TEN;

            // if x is not Infinity or NaN...
            if (xc) {

                // rd is the rounding digit, i.e. the digit after the digit that may be rounded up.
                // n is a base 1e14 number, the value of the element of array x.c containing rd.
                // ni is the index of n within x.c.
                // d is the number of digits of n.
                // i is the index of rd within n including leading zeros.
                // j is the actual index of rd within n (if < 0, rd is a leading zero).
                out: {

                    // Get the number of digits of the first element of xc.
                    for ( d = 1, k = xc[0]; k >= 10; k /= 10, d++ );
                    i = sd - d;

                    // If the rounding digit is in the first element of xc...
                    if ( i < 0 ) {
                        i += LOG_BASE;
                        j = sd;
                        n = xc[ ni = 0 ];

                        // Get the rounding digit at index j of n.
                        rd = n / pows10[ d - j - 1 ] % 10 | 0;
                    } else {
                        ni = mathceil( ( i + 1 ) / LOG_BASE );

                        if ( ni >= xc.length ) {

                            if (r) {

                                // Needed by sqrt.
                                for ( ; xc.length <= ni; xc.push(0) );
                                n = rd = 0;
                                d = 1;
                                i %= LOG_BASE;
                                j = i - LOG_BASE + 1;
                            } else {
                                break out;
                            }
                        } else {
                            n = k = xc[ni];

                            // Get the number of digits of n.
                            for ( d = 1; k >= 10; k /= 10, d++ );

                            // Get the index of rd within n.
                            i %= LOG_BASE;

                            // Get the index of rd within n, adjusted for leading zeros.
                            // The number of leading zeros of n is given by LOG_BASE - d.
                            j = i - LOG_BASE + d;

                            // Get the rounding digit at index j of n.
                            rd = j < 0 ? 0 : n / pows10[ d - j - 1 ] % 10 | 0;
                        }
                    }

                    r = r || sd < 0 ||

                    // Are there any non-zero digits after the rounding digit?
                    // The expression  n % pows10[ d - j - 1 ]  returns all digits of n to the right
                    // of the digit at j, e.g. if n is 908714 and j is 2, the expression gives 714.
                      xc[ni + 1] != null || ( j < 0 ? n : n % pows10[ d - j - 1 ] );

                    r = rm < 4
                      ? ( rd || r ) && ( rm == 0 || rm == ( x.s < 0 ? 3 : 2 ) )
                      : rd > 5 || rd == 5 && ( rm == 4 || r || rm == 6 &&

                        // Check whether the digit to the left of the rounding digit is odd.
                        ( ( i > 0 ? j > 0 ? n / pows10[ d - j ] : 0 : xc[ni - 1] ) % 10 ) & 1 ||
                          rm == ( x.s < 0 ? 8 : 7 ) );

                    if ( sd < 1 || !xc[0] ) {
                        xc.length = 0;

                        if (r) {

                            // Convert sd to decimal places.
                            sd -= x.e + 1;

                            // 1, 0.1, 0.01, 0.001, 0.0001 etc.
                            xc[0] = pows10[ ( LOG_BASE - sd % LOG_BASE ) % LOG_BASE ];
                            x.e = -sd || 0;
                        } else {

                            // Zero.
                            xc[0] = x.e = 0;
                        }

                        return x;
                    }

                    // Remove excess digits.
                    if ( i == 0 ) {
                        xc.length = ni;
                        k = 1;
                        ni--;
                    } else {
                        xc.length = ni + 1;
                        k = pows10[ LOG_BASE - i ];

                        // E.g. 56700 becomes 56000 if 7 is the rounding digit.
                        // j > 0 means i > number of leading zeros of n.
                        xc[ni] = j > 0 ? mathfloor( n / pows10[ d - j ] % pows10[j] ) * k : 0;
                    }

                    // Round up?
                    if (r) {

                        for ( ; ; ) {

                            // If the digit to be rounded up is in the first element of xc...
                            if ( ni == 0 ) {

                                // i will be the length of xc[0] before k is added.
                                for ( i = 1, j = xc[0]; j >= 10; j /= 10, i++ );
                                j = xc[0] += k;
                                for ( k = 1; j >= 10; j /= 10, k++ );

                                // if i != k the length has increased.
                                if ( i != k ) {
                                    x.e++;
                                    if ( xc[0] == BASE ) xc[0] = 1;
                                }

                                break;
                            } else {
                                xc[ni] += k;
                                if ( xc[ni] != BASE ) break;
                                xc[ni--] = 0;
                                k = 1;
                            }
                        }
                    }

                    // Remove trailing zeros.
                    for ( i = xc.length; xc[--i] === 0; xc.pop() );
                }

                // Overflow? Infinity.
                if ( x.e > MAX_EXP ) {
                    x.c = x.e = null;

                // Underflow? Zero.
                } else if ( x.e < MIN_EXP ) {
                    x.c = [ x.e = 0 ];
                }
            }

            return x;
        }


        // PROTOTYPE/INSTANCE METHODS


        /*
         * Return a new BigNumber whose value is the absolute value of this BigNumber.
         */
        P.absoluteValue = P.abs = function () {
            var x = new BigNumber(this);
            if ( x.s < 0 ) x.s = 1;
            return x;
        };


        /*
         * Return a new BigNumber whose value is the value of this BigNumber rounded to a whole
         * number in the direction of Infinity.
         */
        P.ceil = function () {
            return round( new BigNumber(this), this.e + 1, 2 );
        };


        /*
         * Return
         * 1 if the value of this BigNumber is greater than the value of BigNumber(y, b),
         * -1 if the value of this BigNumber is less than the value of BigNumber(y, b),
         * 0 if they have the same value,
         * or null if the value of either is NaN.
         */
        P.comparedTo = P.cmp = function ( y, b ) {
            id = 1;
            return compare( this, new BigNumber( y, b ) );
        };


        /*
         * Return the number of decimal places of the value of this BigNumber, or null if the value
         * of this BigNumber is ±Infinity or NaN.
         */
        P.decimalPlaces = P.dp = function () {
            var n, v,
                c = this.c;

            if ( !c ) return null;
            n = ( ( v = c.length - 1 ) - bitFloor( this.e / LOG_BASE ) ) * LOG_BASE;

            // Subtract the number of trailing zeros of the last number.
            if ( v = c[v] ) for ( ; v % 10 == 0; v /= 10, n-- );
            if ( n < 0 ) n = 0;

            return n;
        };


        /*
         *  n / 0 = I
         *  n / N = N
         *  n / I = 0
         *  0 / n = 0
         *  0 / 0 = N
         *  0 / N = N
         *  0 / I = 0
         *  N / n = N
         *  N / 0 = N
         *  N / N = N
         *  N / I = N
         *  I / n = I
         *  I / 0 = I
         *  I / N = N
         *  I / I = N
         *
         * Return a new BigNumber whose value is the value of this BigNumber divided by the value of
         * BigNumber(y, b), rounded according to DECIMAL_PLACES and ROUNDING_MODE.
         */
        P.dividedBy = P.div = function ( y, b ) {
            id = 3;
            return div( this, new BigNumber( y, b ), DECIMAL_PLACES, ROUNDING_MODE );
        };


        /*
         * Return a new BigNumber whose value is the integer part of dividing the value of this
         * BigNumber by the value of BigNumber(y, b).
         */
        P.dividedToIntegerBy = P.divToInt = function ( y, b ) {
            id = 4;
            return div( this, new BigNumber( y, b ), 0, 1 );
        };


        /*
         * Return true if the value of this BigNumber is equal to the value of BigNumber(y, b),
         * otherwise returns false.
         */
        P.equals = P.eq = function ( y, b ) {
            id = 5;
            return compare( this, new BigNumber( y, b ) ) === 0;
        };


        /*
         * Return a new BigNumber whose value is the value of this BigNumber rounded to a whole
         * number in the direction of -Infinity.
         */
        P.floor = function () {
            return round( new BigNumber(this), this.e + 1, 3 );
        };


        /*
         * Return true if the value of this BigNumber is greater than the value of BigNumber(y, b),
         * otherwise returns false.
         */
        P.greaterThan = P.gt = function ( y, b ) {
            id = 6;
            return compare( this, new BigNumber( y, b ) ) > 0;
        };


        /*
         * Return true if the value of this BigNumber is greater than or equal to the value of
         * BigNumber(y, b), otherwise returns false.
         */
        P.greaterThanOrEqualTo = P.gte = function ( y, b ) {
            id = 7;
            return ( b = compare( this, new BigNumber( y, b ) ) ) === 1 || b === 0;

        };


        /*
         * Return true if the value of this BigNumber is a finite number, otherwise returns false.
         */
        P.isFinite = function () {
            return !!this.c;
        };


        /*
         * Return true if the value of this BigNumber is an integer, otherwise return false.
         */
        P.isInteger = P.isInt = function () {
            return !!this.c && bitFloor( this.e / LOG_BASE ) > this.c.length - 2;
        };


        /*
         * Return true if the value of this BigNumber is NaN, otherwise returns false.
         */
        P.isNaN = function () {
            return !this.s;
        };


        /*
         * Return true if the value of this BigNumber is negative, otherwise returns false.
         */
        P.isNegative = P.isNeg = function () {
            return this.s < 0;
        };


        /*
         * Return true if the value of this BigNumber is 0 or -0, otherwise returns false.
         */
        P.isZero = function () {
            return !!this.c && this.c[0] == 0;
        };


        /*
         * Return true if the value of this BigNumber is less than the value of BigNumber(y, b),
         * otherwise returns false.
         */
        P.lessThan = P.lt = function ( y, b ) {
            id = 8;
            return compare( this, new BigNumber( y, b ) ) < 0;
        };


        /*
         * Return true if the value of this BigNumber is less than or equal to the value of
         * BigNumber(y, b), otherwise returns false.
         */
        P.lessThanOrEqualTo = P.lte = function ( y, b ) {
            id = 9;
            return ( b = compare( this, new BigNumber( y, b ) ) ) === -1 || b === 0;
        };


        /*
         *  n - 0 = n
         *  n - N = N
         *  n - I = -I
         *  0 - n = -n
         *  0 - 0 = 0
         *  0 - N = N
         *  0 - I = -I
         *  N - n = N
         *  N - 0 = N
         *  N - N = N
         *  N - I = N
         *  I - n = I
         *  I - 0 = I
         *  I - N = N
         *  I - I = N
         *
         * Return a new BigNumber whose value is the value of this BigNumber minus the value of
         * BigNumber(y, b).
         */
        P.minus = P.sub = function ( y, b ) {
            var i, j, t, xLTy,
                x = this,
                a = x.s;

            id = 10;
            y = new BigNumber( y, b );
            b = y.s;

            // Either NaN?
            if ( !a || !b ) return new BigNumber(NaN);

            // Signs differ?
            if ( a != b ) {
                y.s = -b;
                return x.plus(y);
            }

            var xe = x.e / LOG_BASE,
                ye = y.e / LOG_BASE,
                xc = x.c,
                yc = y.c;

            if ( !xe || !ye ) {

                // Either Infinity?
                if ( !xc || !yc ) return xc ? ( y.s = -b, y ) : new BigNumber( yc ? x : NaN );

                // Either zero?
                if ( !xc[0] || !yc[0] ) {

                    // Return y if y is non-zero, x if x is non-zero, or zero if both are zero.
                    return yc[0] ? ( y.s = -b, y ) : new BigNumber( xc[0] ? x :

                      // IEEE 754 (2008) 6.3: n - n = -0 when rounding to -Infinity
                      ROUNDING_MODE == 3 ? -0 : 0 );
                }
            }

            xe = bitFloor(xe);
            ye = bitFloor(ye);
            xc = xc.slice();

            // Determine which is the bigger number.
            if ( a = xe - ye ) {

                if ( xLTy = a < 0 ) {
                    a = -a;
                    t = xc;
                } else {
                    ye = xe;
                    t = yc;
                }

                t.reverse();

                // Prepend zeros to equalise exponents.
                for ( b = a; b--; t.push(0) );
                t.reverse();
            } else {

                // Exponents equal. Check digit by digit.
                j = ( xLTy = ( a = xc.length ) < ( b = yc.length ) ) ? a : b;

                for ( a = b = 0; b < j; b++ ) {

                    if ( xc[b] != yc[b] ) {
                        xLTy = xc[b] < yc[b];
                        break;
                    }
                }
            }

            // x < y? Point xc to the array of the bigger number.
            if (xLTy) t = xc, xc = yc, yc = t, y.s = -y.s;

            b = ( j = yc.length ) - ( i = xc.length );

            // Append zeros to xc if shorter.
            // No need to add zeros to yc if shorter as subtract only needs to start at yc.length.
            if ( b > 0 ) for ( ; b--; xc[i++] = 0 );
            b = BASE - 1;

            // Subtract yc from xc.
            for ( ; j > a; ) {

                if ( xc[--j] < yc[j] ) {
                    for ( i = j; i && !xc[--i]; xc[i] = b );
                    --xc[i];
                    xc[j] += BASE;
                }

                xc[j] -= yc[j];
            }

            // Remove leading zeros and adjust exponent accordingly.
            for ( ; xc[0] == 0; xc.shift(), --ye );

            // Zero?
            if ( !xc[0] ) {

                // Following IEEE 754 (2008) 6.3,
                // n - n = +0  but  n - n = -0  when rounding towards -Infinity.
                y.s = ROUNDING_MODE == 3 ? -1 : 1;
                y.c = [ y.e = 0 ];
                return y;
            }

            // No need to check for Infinity as +x - +y != Infinity && -x - -y != Infinity
            // for finite x and y.
            return normalise( y, xc, ye );
        };


        /*
         *   n % 0 =  N
         *   n % N =  N
         *   n % I =  n
         *   0 % n =  0
         *  -0 % n = -0
         *   0 % 0 =  N
         *   0 % N =  N
         *   0 % I =  0
         *   N % n =  N
         *   N % 0 =  N
         *   N % N =  N
         *   N % I =  N
         *   I % n =  N
         *   I % 0 =  N
         *   I % N =  N
         *   I % I =  N
         *
         * Return a new BigNumber whose value is the value of this BigNumber modulo the value of
         * BigNumber(y, b). The result depends on the value of MODULO_MODE.
         */
        P.modulo = P.mod = function ( y, b ) {
            var q, s,
                x = this;

            id = 11;
            y = new BigNumber( y, b );

            // Return NaN if x is Infinity or NaN, or y is NaN or zero.
            if ( !x.c || !y.s || y.c && !y.c[0] ) {
                return new BigNumber(NaN);

            // Return x if y is Infinity or x is zero.
            } else if ( !y.c || x.c && !x.c[0] ) {
                return new BigNumber(x);
            }

            if ( MODULO_MODE == 9 ) {

                // Euclidian division: q = sign(y) * floor(x / abs(y))
                // r = x - qy    where  0 <= r < abs(y)
                s = y.s;
                y.s = 1;
                q = div( x, y, 0, 3 );
                y.s = s;
                q.s *= s;
            } else {
                q = div( x, y, 0, MODULO_MODE );
            }

            return x.minus( q.times(y) );
        };


        /*
         * Return a new BigNumber whose value is the value of this BigNumber negated,
         * i.e. multiplied by -1.
         */
        P.negated = P.neg = function () {
            var x = new BigNumber(this);
            x.s = -x.s || null;
            return x;
        };


        /*
         *  n + 0 = n
         *  n + N = N
         *  n + I = I
         *  0 + n = n
         *  0 + 0 = 0
         *  0 + N = N
         *  0 + I = I
         *  N + n = N
         *  N + 0 = N
         *  N + N = N
         *  N + I = N
         *  I + n = I
         *  I + 0 = I
         *  I + N = N
         *  I + I = I
         *
         * Return a new BigNumber whose value is the value of this BigNumber plus the value of
         * BigNumber(y, b).
         */
        P.plus = P.add = function ( y, b ) {
            var t,
                x = this,
                a = x.s;

            id = 12;
            y = new BigNumber( y, b );
            b = y.s;

            // Either NaN?
            if ( !a || !b ) return new BigNumber(NaN);

            // Signs differ?
             if ( a != b ) {
                y.s = -b;
                return x.minus(y);
            }

            var xe = x.e / LOG_BASE,
                ye = y.e / LOG_BASE,
                xc = x.c,
                yc = y.c;

            if ( !xe || !ye ) {

                // Return ±Infinity if either ±Infinity.
                if ( !xc || !yc ) return new BigNumber( a / 0 );

                // Either zero?
                // Return y if y is non-zero, x if x is non-zero, or zero if both are zero.
                if ( !xc[0] || !yc[0] ) return yc[0] ? y : new BigNumber( xc[0] ? x : a * 0 );
            }

            xe = bitFloor(xe);
            ye = bitFloor(ye);
            xc = xc.slice();

            // Prepend zeros to equalise exponents. Faster to use reverse then do unshifts.
            if ( a = xe - ye ) {
                if ( a > 0 ) {
                    ye = xe;
                    t = yc;
                } else {
                    a = -a;
                    t = xc;
                }

                t.reverse();
                for ( ; a--; t.push(0) );
                t.reverse();
            }

            a = xc.length;
            b = yc.length;

            // Point xc to the longer array, and b to the shorter length.
            if ( a - b < 0 ) t = yc, yc = xc, xc = t, b = a;

            // Only start adding at yc.length - 1 as the further digits of xc can be ignored.
            for ( a = 0; b; ) {
                a = ( xc[--b] = xc[b] + yc[b] + a ) / BASE | 0;
                xc[b] %= BASE;
            }

            if (a) {
                xc.unshift(a);
                ++ye;
            }

            // No need to check for zero, as +x + +y != 0 && -x + -y != 0
            // ye = MAX_EXP + 1 possible
            return normalise( y, xc, ye );
        };


        /*
         * Return the number of significant digits of the value of this BigNumber.
         *
         * [z] {boolean|number} Whether to count integer-part trailing zeros: true, false, 1 or 0.
         */
        P.precision = P.sd = function (z) {
            var n, v,
                x = this,
                c = x.c;

            // 'precision() argument not a boolean or binary digit: {z}'
            if ( z != null && z !== !!z && z !== 1 && z !== 0 ) {
                if (ERRORS) raise( 13, 'argument' + notBool, z );
                if ( z != !!z ) z = null;
            }

            if ( !c ) return null;
            v = c.length - 1;
            n = v * LOG_BASE + 1;

            if ( v = c[v] ) {

                // Subtract the number of trailing zeros of the last element.
                for ( ; v % 10 == 0; v /= 10, n-- );

                // Add the number of digits of the first element.
                for ( v = c[0]; v >= 10; v /= 10, n++ );
            }

            if ( z && x.e + 1 > n ) n = x.e + 1;

            return n;
        };


        /*
         * Return a new BigNumber whose value is the value of this BigNumber rounded to a maximum of
         * dp decimal places using rounding mode rm, or to 0 and ROUNDING_MODE respectively if
         * omitted.
         *
         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
         *
         * 'round() decimal places out of range: {dp}'
         * 'round() decimal places not an integer: {dp}'
         * 'round() rounding mode not an integer: {rm}'
         * 'round() rounding mode out of range: {rm}'
         */
        P.round = function ( dp, rm ) {
            var n = new BigNumber(this);

            if ( dp == null || isValidInt( dp, 0, MAX, 15 ) ) {
                round( n, ~~dp + this.e + 1, rm == null ||
                  !isValidInt( rm, 0, 8, 15, roundingMode ) ? ROUNDING_MODE : rm | 0 );
            }

            return n;
        };


        /*
         * Return a new BigNumber whose value is the value of this BigNumber shifted by k places
         * (powers of 10). Shift to the right if n > 0, and to the left if n < 0.
         *
         * k {number} Integer, -MAX_SAFE_INTEGER to MAX_SAFE_INTEGER inclusive.
         *
         * If k is out of range and ERRORS is false, the result will be ±0 if k < 0, or ±Infinity
         * otherwise.
         *
         * 'shift() argument not an integer: {k}'
         * 'shift() argument out of range: {k}'
         */
        P.shift = function (k) {
            var n = this;
            return isValidInt( k, -MAX_SAFE_INTEGER, MAX_SAFE_INTEGER, 16, 'argument' )

              // k < 1e+21, or truncate(k) will produce exponential notation.
              ? n.times( '1e' + truncate(k) )
              : new BigNumber( n.c && n.c[0] && ( k < -MAX_SAFE_INTEGER || k > MAX_SAFE_INTEGER )
                ? n.s * ( k < 0 ? 0 : 1 / 0 )
                : n );
        };


        /*
         *  sqrt(-n) =  N
         *  sqrt( N) =  N
         *  sqrt(-I) =  N
         *  sqrt( I) =  I
         *  sqrt( 0) =  0
         *  sqrt(-0) = -0
         *
         * Return a new BigNumber whose value is the square root of the value of this BigNumber,
         * rounded according to DECIMAL_PLACES and ROUNDING_MODE.
         */
        P.squareRoot = P.sqrt = function () {
            var m, n, r, rep, t,
                x = this,
                c = x.c,
                s = x.s,
                e = x.e,
                dp = DECIMAL_PLACES + 4,
                half = new BigNumber('0.5');

            // Negative/NaN/Infinity/zero?
            if ( s !== 1 || !c || !c[0] ) {
                return new BigNumber( !s || s < 0 && ( !c || c[0] ) ? NaN : c ? x : 1 / 0 );
            }

            // Initial estimate.
            s = Math.sqrt( +x );

            // Math.sqrt underflow/overflow?
            // Pass x to Math.sqrt as integer, then adjust the exponent of the result.
            if ( s == 0 || s == 1 / 0 ) {
                n = coeffToString(c);
                if ( ( n.length + e ) % 2 == 0 ) n += '0';
                s = Math.sqrt(n);
                e = bitFloor( ( e + 1 ) / 2 ) - ( e < 0 || e % 2 );

                if ( s == 1 / 0 ) {
                    n = '1e' + e;
                } else {
                    n = s.toExponential();
                    n = n.slice( 0, n.indexOf('e') + 1 ) + e;
                }

                r = new BigNumber(n);
            } else {
                r = new BigNumber( s + '' );
            }

            // Check for zero.
            // r could be zero if MIN_EXP is changed after the this value was created.
            // This would cause a division by zero (x/t) and hence Infinity below, which would cause
            // coeffToString to throw.
            if ( r.c[0] ) {
                e = r.e;
                s = e + dp;
                if ( s < 3 ) s = 0;

                // Newton-Raphson iteration.
                for ( ; ; ) {
                    t = r;
                    r = half.times( t.plus( div( x, t, dp, 1 ) ) );

                    if ( coeffToString( t.c   ).slice( 0, s ) === ( n =
                         coeffToString( r.c ) ).slice( 0, s ) ) {

                        // The exponent of r may here be one less than the final result exponent,
                        // e.g 0.0009999 (e-4) --> 0.001 (e-3), so adjust s so the rounding digits
                        // are indexed correctly.
                        if ( r.e < e ) --s;
                        n = n.slice( s - 3, s + 1 );

                        // The 4th rounding digit may be in error by -1 so if the 4 rounding digits
                        // are 9999 or 4999 (i.e. approaching a rounding boundary) continue the
                        // iteration.
                        if ( n == '9999' || !rep && n == '4999' ) {

                            // On the first iteration only, check to see if rounding up gives the
                            // exact result as the nines may infinitely repeat.
                            if ( !rep ) {
                                round( t, t.e + DECIMAL_PLACES + 2, 0 );

                                if ( t.times(t).eq(x) ) {
                                    r = t;
                                    break;
                                }
                            }

                            dp += 4;
                            s += 4;
                            rep = 1;
                        } else {

                            // If rounding digits are null, 0{0,4} or 50{0,3}, check for exact
                            // result. If not, then there are further digits and m will be truthy.
                            if ( !+n || !+n.slice(1) && n.charAt(0) == '5' ) {

                                // Truncate to the first rounding digit.
                                round( r, r.e + DECIMAL_PLACES + 2, 1 );
                                m = !r.times(r).eq(x);
                            }

                            break;
                        }
                    }
                }
            }

            return round( r, r.e + DECIMAL_PLACES + 1, ROUNDING_MODE, m );
        };


        /*
         *  n * 0 = 0
         *  n * N = N
         *  n * I = I
         *  0 * n = 0
         *  0 * 0 = 0
         *  0 * N = N
         *  0 * I = N
         *  N * n = N
         *  N * 0 = N
         *  N * N = N
         *  N * I = N
         *  I * n = I
         *  I * 0 = N
         *  I * N = N
         *  I * I = I
         *
         * Return a new BigNumber whose value is the value of this BigNumber times the value of
         * BigNumber(y, b).
         */
        P.times = P.mul = function ( y, b ) {
            var c, e, i, j, k, m, xcL, xlo, xhi, ycL, ylo, yhi, zc,
                base, sqrtBase,
                x = this,
                xc = x.c,
                yc = ( id = 17, y = new BigNumber( y, b ) ).c;

            // Either NaN, ±Infinity or ±0?
            if ( !xc || !yc || !xc[0] || !yc[0] ) {

                // Return NaN if either is NaN, or one is 0 and the other is Infinity.
                if ( !x.s || !y.s || xc && !xc[0] && !yc || yc && !yc[0] && !xc ) {
                    y.c = y.e = y.s = null;
                } else {
                    y.s *= x.s;

                    // Return ±Infinity if either is ±Infinity.
                    if ( !xc || !yc ) {
                        y.c = y.e = null;

                    // Return ±0 if either is ±0.
                    } else {
                        y.c = [0];
                        y.e = 0;
                    }
                }

                return y;
            }

            e = bitFloor( x.e / LOG_BASE ) + bitFloor( y.e / LOG_BASE );
            y.s *= x.s;
            xcL = xc.length;
            ycL = yc.length;

            // Ensure xc points to longer array and xcL to its length.
            if ( xcL < ycL ) zc = xc, xc = yc, yc = zc, i = xcL, xcL = ycL, ycL = i;

            // Initialise the result array with zeros.
            for ( i = xcL + ycL, zc = []; i--; zc.push(0) );

            base = BASE;
            sqrtBase = SQRT_BASE;

            for ( i = ycL; --i >= 0; ) {
                c = 0;
                ylo = yc[i] % sqrtBase;
                yhi = yc[i] / sqrtBase | 0;

                for ( k = xcL, j = i + k; j > i; ) {
                    xlo = xc[--k] % sqrtBase;
                    xhi = xc[k] / sqrtBase | 0;
                    m = yhi * xlo + xhi * ylo;
                    xlo = ylo * xlo + ( ( m % sqrtBase ) * sqrtBase ) + zc[j] + c;
                    c = ( xlo / base | 0 ) + ( m / sqrtBase | 0 ) + yhi * xhi;
                    zc[j--] = xlo % base;
                }

                zc[j] = c;
            }

            if (c) {
                ++e;
            } else {
                zc.shift();
            }

            return normalise( y, zc, e );
        };


        /*
         * Return a new BigNumber whose value is the value of this BigNumber rounded to a maximum of
         * sd significant digits using rounding mode rm, or ROUNDING_MODE if rm is omitted.
         *
         * [sd] {number} Significant digits. Integer, 1 to MAX inclusive.
         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
         *
         * 'toDigits() precision out of range: {sd}'
         * 'toDigits() precision not an integer: {sd}'
         * 'toDigits() rounding mode not an integer: {rm}'
         * 'toDigits() rounding mode out of range: {rm}'
         */
        P.toDigits = function ( sd, rm ) {
            var n = new BigNumber(this);
            sd = sd == null || !isValidInt( sd, 1, MAX, 18, 'precision' ) ? null : sd | 0;
            rm = rm == null || !isValidInt( rm, 0, 8, 18, roundingMode ) ? ROUNDING_MODE : rm | 0;
            return sd ? round( n, sd, rm ) : n;
        };


        /*
         * Return a string representing the value of this BigNumber in exponential notation and
         * rounded using ROUNDING_MODE to dp fixed decimal places.
         *
         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
         *
         * 'toExponential() decimal places not an integer: {dp}'
         * 'toExponential() decimal places out of range: {dp}'
         * 'toExponential() rounding mode not an integer: {rm}'
         * 'toExponential() rounding mode out of range: {rm}'
         */
        P.toExponential = function ( dp, rm ) {
            return format( this,
              dp != null && isValidInt( dp, 0, MAX, 19 ) ? ~~dp + 1 : null, rm, 19 );
        };


        /*
         * Return a string representing the value of this BigNumber in fixed-point notation rounding
         * to dp fixed decimal places using rounding mode rm, or ROUNDING_MODE if rm is omitted.
         *
         * Note: as with JavaScript's number type, (-0).toFixed(0) is '0',
         * but e.g. (-0.00001).toFixed(0) is '-0'.
         *
         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
         *
         * 'toFixed() decimal places not an integer: {dp}'
         * 'toFixed() decimal places out of range: {dp}'
         * 'toFixed() rounding mode not an integer: {rm}'
         * 'toFixed() rounding mode out of range: {rm}'
         */
        P.toFixed = function ( dp, rm ) {
            return format( this, dp != null && isValidInt( dp, 0, MAX, 20 )
              ? ~~dp + this.e + 1 : null, rm, 20 );
        };


        /*
         * Return a string representing the value of this BigNumber in fixed-point notation rounded
         * using rm or ROUNDING_MODE to dp decimal places, and formatted according to the properties
         * of the FORMAT object (see BigNumber.config).
         *
         * FORMAT = {
         *      decimalSeparator : '.',
         *      groupSeparator : ',',
         *      groupSize : 3,
         *      secondaryGroupSize : 0,
         *      fractionGroupSeparator : '\xA0',    // non-breaking space
         *      fractionGroupSize : 0
         * };
         *
         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
         *
         * 'toFormat() decimal places not an integer: {dp}'
         * 'toFormat() decimal places out of range: {dp}'
         * 'toFormat() rounding mode not an integer: {rm}'
         * 'toFormat() rounding mode out of range: {rm}'
         */
        P.toFormat = function ( dp, rm ) {
            var str = format( this, dp != null && isValidInt( dp, 0, MAX, 21 )
              ? ~~dp + this.e + 1 : null, rm, 21 );

            if ( this.c ) {
                var i,
                    arr = str.split('.'),
                    g1 = +FORMAT.groupSize,
                    g2 = +FORMAT.secondaryGroupSize,
                    groupSeparator = FORMAT.groupSeparator,
                    intPart = arr[0],
                    fractionPart = arr[1],
                    isNeg = this.s < 0,
                    intDigits = isNeg ? intPart.slice(1) : intPart,
                    len = intDigits.length;

                if (g2) i = g1, g1 = g2, g2 = i, len -= i;

                if ( g1 > 0 && len > 0 ) {
                    i = len % g1 || g1;
                    intPart = intDigits.substr( 0, i );

                    for ( ; i < len; i += g1 ) {
                        intPart += groupSeparator + intDigits.substr( i, g1 );
                    }

                    if ( g2 > 0 ) intPart += groupSeparator + intDigits.slice(i);
                    if (isNeg) intPart = '-' + intPart;
                }

                str = fractionPart
                  ? intPart + FORMAT.decimalSeparator + ( ( g2 = +FORMAT.fractionGroupSize )
                    ? fractionPart.replace( new RegExp( '\\d{' + g2 + '}\\B', 'g' ),
                      '$&' + FORMAT.fractionGroupSeparator )
                    : fractionPart )
                  : intPart;
            }

            return str;
        };


        /*
         * Return a string array representing the value of this BigNumber as a simple fraction with
         * an integer numerator and an integer denominator. The denominator will be a positive
         * non-zero value less than or equal to the specified maximum denominator. If a maximum
         * denominator is not specified, the denominator will be the lowest value necessary to
         * represent the number exactly.
         *
         * [md] {number|string|BigNumber} Integer >= 1 and < Infinity. The maximum denominator.
         *
         * 'toFraction() max denominator not an integer: {md}'
         * 'toFraction() max denominator out of range: {md}'
         */
        P.toFraction = function (md) {
            var arr, d0, d2, e, exp, n, n0, q, s,
                k = ERRORS,
                x = this,
                xc = x.c,
                d = new BigNumber(ONE),
                n1 = d0 = new BigNumber(ONE),
                d1 = n0 = new BigNumber(ONE);

            if ( md != null ) {
                ERRORS = false;
                n = new BigNumber(md);
                ERRORS = k;

                if ( !( k = n.isInt() ) || n.lt(ONE) ) {

                    if (ERRORS) {
                        raise( 22,
                          'max denominator ' + ( k ? 'out of range' : 'not an integer' ), md );
                    }

                    // ERRORS is false:
                    // If md is a finite non-integer >= 1, round it to an integer and use it.
                    md = !k && n.c && round( n, n.e + 1, 1 ).gte(ONE) ? n : null;
                }
            }

            if ( !xc ) return x.toString();
            s = coeffToString(xc);

            // Determine initial denominator.
            // d is a power of 10 and the minimum max denominator that specifies the value exactly.
            e = d.e = s.length - x.e - 1;
            d.c[0] = POWS_TEN[ ( exp = e % LOG_BASE ) < 0 ? LOG_BASE + exp : exp ];
            md = !md || n.cmp(d) > 0 ? ( e > 0 ? d : n1 ) : n;

            exp = MAX_EXP;
            MAX_EXP = 1 / 0;
            n = new BigNumber(s);

            // n0 = d1 = 0
            n0.c[0] = 0;

            for ( ; ; )  {
                q = div( n, d, 0, 1 );
                d2 = d0.plus( q.times(d1) );
                if ( d2.cmp(md) == 1 ) break;
                d0 = d1;
                d1 = d2;
                n1 = n0.plus( q.times( d2 = n1 ) );
                n0 = d2;
                d = n.minus( q.times( d2 = d ) );
                n = d2;
            }

            d2 = div( md.minus(d0), d1, 0, 1 );
            n0 = n0.plus( d2.times(n1) );
            d0 = d0.plus( d2.times(d1) );
            n0.s = n1.s = x.s;
            e *= 2;

            // Determine which fraction is closer to x, n0/d0 or n1/d1
            arr = div( n1, d1, e, ROUNDING_MODE ).minus(x).abs().cmp(
                  div( n0, d0, e, ROUNDING_MODE ).minus(x).abs() ) < 1
                    ? [ n1.toString(), d1.toString() ]
                    : [ n0.toString(), d0.toString() ];

            MAX_EXP = exp;
            return arr;
        };


        /*
         * Return the value of this BigNumber converted to a number primitive.
         */
        P.toNumber = function () {
            return +this;
        };


        /*
         * Return a BigNumber whose value is the value of this BigNumber raised to the power n.
         * If m is present, return the result modulo m.
         * If n is negative round according to DECIMAL_PLACES and ROUNDING_MODE.
         * If POW_PRECISION is non-zero and m is not present, round to POW_PRECISION using
         * ROUNDING_MODE.
         *
         * The modular power operation works efficiently when x, n, and m are positive integers,
         * otherwise it is equivalent to calculating x.toPower(n).modulo(m) (with POW_PRECISION 0).
         *
         * n {number} Integer, -MAX_SAFE_INTEGER to MAX_SAFE_INTEGER inclusive.
         * [m] {number|string|BigNumber} The modulus.
         *
         * 'pow() exponent not an integer: {n}'
         * 'pow() exponent out of range: {n}'
         *
         * Performs 54 loop iterations for n of 9007199254740991.
         */
        P.toPower = P.pow = function ( n, m ) {
            var k, y, z,
                i = mathfloor( n < 0 ? -n : +n ),
                x = this;

            if ( m != null ) {
                id = 23;
                m = new BigNumber(m);
            }

            // Pass ±Infinity to Math.pow if exponent is out of range.
            if ( !isValidInt( n, -MAX_SAFE_INTEGER, MAX_SAFE_INTEGER, 23, 'exponent' ) &&
              ( !isFinite(n) || i > MAX_SAFE_INTEGER && ( n /= 0 ) ||
                parseFloat(n) != n && !( n = NaN ) ) || n == 0 ) {
                k = Math.pow( +x, n );
                return new BigNumber( m ? k % m : k );
            }

            if (m) {
                if ( n > 1 && x.gt(ONE) && x.isInt() && m.gt(ONE) && m.isInt() ) {
                    x = x.mod(m);
                } else {
                    z = m;

                    // Nullify m so only a single mod operation is performed at the end.
                    m = null;
                }
            } else if (POW_PRECISION) {

                // Truncating each coefficient array to a length of k after each multiplication
                // equates to truncating significant digits to POW_PRECISION + [28, 41],
                // i.e. there will be a minimum of 28 guard digits retained.
                // (Using + 1.5 would give [9, 21] guard digits.)
                k = mathceil( POW_PRECISION / LOG_BASE + 2 );
            }

            y = new BigNumber(ONE);

            for ( ; ; ) {
                if ( i % 2 ) {
                    y = y.times(x);
                    if ( !y.c ) break;
                    if (k) {
                        if ( y.c.length > k ) y.c.length = k;
                    } else if (m) {
                        y = y.mod(m);
                    }
                }

                i = mathfloor( i / 2 );
                if ( !i ) break;
                x = x.times(x);
                if (k) {
                    if ( x.c && x.c.length > k ) x.c.length = k;
                } else if (m) {
                    x = x.mod(m);
                }
            }

            if (m) return y;
            if ( n < 0 ) y = ONE.div(y);

            return z ? y.mod(z) : k ? round( y, POW_PRECISION, ROUNDING_MODE ) : y;
        };


        /*
         * Return a string representing the value of this BigNumber rounded to sd significant digits
         * using rounding mode rm or ROUNDING_MODE. If sd is less than the number of digits
         * necessary to represent the integer part of the value in fixed-point notation, then use
         * exponential notation.
         *
         * [sd] {number} Significant digits. Integer, 1 to MAX inclusive.
         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
         *
         * 'toPrecision() precision not an integer: {sd}'
         * 'toPrecision() precision out of range: {sd}'
         * 'toPrecision() rounding mode not an integer: {rm}'
         * 'toPrecision() rounding mode out of range: {rm}'
         */
        P.toPrecision = function ( sd, rm ) {
            return format( this, sd != null && isValidInt( sd, 1, MAX, 24, 'precision' )
              ? sd | 0 : null, rm, 24 );
        };


        /*
         * Return a string representing the value of this BigNumber in base b, or base 10 if b is
         * omitted. If a base is specified, including base 10, round according to DECIMAL_PLACES and
         * ROUNDING_MODE. If a base is not specified, and this BigNumber has a positive exponent
         * that is equal to or greater than TO_EXP_POS, or a negative exponent equal to or less than
         * TO_EXP_NEG, return exponential notation.
         *
         * [b] {number} Integer, 2 to 64 inclusive.
         *
         * 'toString() base not an integer: {b}'
         * 'toString() base out of range: {b}'
         */
        P.toString = function (b) {
            var str,
                n = this,
                s = n.s,
                e = n.e;

            // Infinity or NaN?
            if ( e === null ) {

                if (s) {
                    str = 'Infinity';
                    if ( s < 0 ) str = '-' + str;
                } else {
                    str = 'NaN';
                }
            } else {
                str = coeffToString( n.c );

                if ( b == null || !isValidInt( b, 2, 64, 25, 'base' ) ) {
                    str = e <= TO_EXP_NEG || e >= TO_EXP_POS
                      ? toExponential( str, e )
                      : toFixedPoint( str, e );
                } else {
                    str = convertBase( toFixedPoint( str, e ), b | 0, 10, s );
                }

                if ( s < 0 && n.c[0] ) str = '-' + str;
            }

            return str;
        };


        /*
         * Return a new BigNumber whose value is the value of this BigNumber truncated to a whole
         * number.
         */
        P.truncated = P.trunc = function () {
            return round( new BigNumber(this), this.e + 1, 1 );
        };



        /*
         * Return as toString, but do not accept a base argument, and include the minus sign for
         * negative zero.
         */
        P.valueOf = P.toJSON = function () {
            var str,
                n = this,
                e = n.e;

            if ( e === null ) return n.toString();

            str = coeffToString( n.c );

            str = e <= TO_EXP_NEG || e >= TO_EXP_POS
                ? toExponential( str, e )
                : toFixedPoint( str, e );

            return n.s < 0 ? '-' + str : str;
        };


        // Aliases for BigDecimal methods.
        //P.add = P.plus;         // P.add included above
        //P.subtract = P.minus;   // P.sub included above
        //P.multiply = P.times;   // P.mul included above
        //P.divide = P.div;
        //P.remainder = P.mod;
        //P.compareTo = P.cmp;
        //P.negate = P.neg;


        if ( configObj != null ) BigNumber.config(configObj);

        return BigNumber;
    }


    // PRIVATE HELPER FUNCTIONS


    function bitFloor(n) {
        var i = n | 0;
        return n > 0 || n === i ? i : i - 1;
    }


    // Return a coefficient array as a string of base 10 digits.
    function coeffToString(a) {
        var s, z,
            i = 1,
            j = a.length,
            r = a[0] + '';

        for ( ; i < j; ) {
            s = a[i++] + '';
            z = LOG_BASE - s.length;
            for ( ; z--; s = '0' + s );
            r += s;
        }

        // Determine trailing zeros.
        for ( j = r.length; r.charCodeAt(--j) === 48; );
        return r.slice( 0, j + 1 || 1 );
    }


    // Compare the value of BigNumbers x and y.
    function compare( x, y ) {
        var a, b,
            xc = x.c,
            yc = y.c,
            i = x.s,
            j = y.s,
            k = x.e,
            l = y.e;

        // Either NaN?
        if ( !i || !j ) return null;

        a = xc && !xc[0];
        b = yc && !yc[0];

        // Either zero?
        if ( a || b ) return a ? b ? 0 : -j : i;

        // Signs differ?
        if ( i != j ) return i;

        a = i < 0;
        b = k == l;

        // Either Infinity?
        if ( !xc || !yc ) return b ? 0 : !xc ^ a ? 1 : -1;

        // Compare exponents.
        if ( !b ) return k > l ^ a ? 1 : -1;

        j = ( k = xc.length ) < ( l = yc.length ) ? k : l;

        // Compare digit by digit.
        for ( i = 0; i < j; i++ ) if ( xc[i] != yc[i] ) return xc[i] > yc[i] ^ a ? 1 : -1;

        // Compare lengths.
        return k == l ? 0 : k > l ^ a ? 1 : -1;
    }


    /*
     * Return true if n is a valid number in range, otherwise false.
     * Use for argument validation when ERRORS is false.
     * Note: parseInt('1e+1') == 1 but parseFloat('1e+1') == 10.
     */
    function intValidatorNoErrors( n, min, max ) {
        return ( n = truncate(n) ) >= min && n <= max;
    }


    function isArray(obj) {
        return Object.prototype.toString.call(obj) == '[object Array]';
    }


    /*
     * Convert string of baseIn to an array of numbers of baseOut.
     * Eg. convertBase('255', 10, 16) returns [15, 15].
     * Eg. convertBase('ff', 16, 10) returns [2, 5, 5].
     */
    function toBaseOut( str, baseIn, baseOut ) {
        var j,
            arr = [0],
            arrL,
            i = 0,
            len = str.length;

        for ( ; i < len; ) {
            for ( arrL = arr.length; arrL--; arr[arrL] *= baseIn );
            arr[ j = 0 ] += ALPHABET.indexOf( str.charAt( i++ ) );

            for ( ; j < arr.length; j++ ) {

                if ( arr[j] > baseOut - 1 ) {
                    if ( arr[j + 1] == null ) arr[j + 1] = 0;
                    arr[j + 1] += arr[j] / baseOut | 0;
                    arr[j] %= baseOut;
                }
            }
        }

        return arr.reverse();
    }


    function toExponential( str, e ) {
        return ( str.length > 1 ? str.charAt(0) + '.' + str.slice(1) : str ) +
          ( e < 0 ? 'e' : 'e+' ) + e;
    }


    function toFixedPoint( str, e ) {
        var len, z;

        // Negative exponent?
        if ( e < 0 ) {

            // Prepend zeros.
            for ( z = '0.'; ++e; z += '0' );
            str = z + str;

        // Positive exponent
        } else {
            len = str.length;

            // Append zeros.
            if ( ++e > len ) {
                for ( z = '0', e -= len; --e; z += '0' );
                str += z;
            } else if ( e < len ) {
                str = str.slice( 0, e ) + '.' + str.slice(e);
            }
        }

        return str;
    }


    function truncate(n) {
        n = parseFloat(n);
        return n < 0 ? mathceil(n) : mathfloor(n);
    }


    // EXPORT


    BigNumber = constructorFactory();
    BigNumber.default = BigNumber.BigNumber = BigNumber;


    // AMD.
    if ( true ) {
        !(__WEBPACK_AMD_DEFINE_RESULT__ = function () { return BigNumber; }.call(exports, __webpack_require__, exports, module),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

    // Node.js and other environments that support module.exports.
    } else if ( typeof module != 'undefined' && module.exports ) {
        module.exports = BigNumber;

        // Split string stops browserify adding crypto shim.
        if ( !cryptoObj ) try { cryptoObj = require('cry' + 'pto'); } catch (e) {}

    // Browser.
    } else {
        if ( !globalObj ) globalObj = typeof self != 'undefined' ? self : Function('return this')();
        globalObj.BigNumber = BigNumber;
    }
})(this);


/***/ }),

/***/ 11:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1), __webpack_require__(2), __webpack_require__(12)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, moment) {
        "use strict";

        var _daysOfWeek, _monthNames;

        function initCommonTextResource() {
            if (!_daysOfWeek || !_monthNames) {
                _daysOfWeek = [
                    Site.getSingleResource('Su'), Site.getSingleResource('Mo'), Site.getSingleResource('Tu'), Site.getSingleResource('We'),
                    Site.getSingleResource('Th'), Site.getSingleResource('Fr'), Site.getSingleResource('Sa')
                ];

                _monthNames = [
                    Site.getSingleResource('Jan'), Site.getSingleResource('Feb'), Site.getSingleResource('Mar'), Site.getSingleResource('Apr'),
                    Site.getSingleResource('May'), Site.getSingleResource('Jun'), Site.getSingleResource('Jul'), Site.getSingleResource('Aug'),
                    Site.getSingleResource('Sep'), Site.getSingleResource('Oct'), Site.getSingleResource('Nov'), Site.getSingleResource('Dec')
                ];
            }
        }

        function initDateRangePicker(firstDayOfWeekIsMonday, rangeOption) {
            var $dateRange = $('#DateRange');

            var $from = $('#FromDate');
            var $to = $('#ToDate');
            var start = moment($from.attr('data-original-from'), "MM/DD/YYYY");
            var end = moment($to.attr('data-original-to'), "MM/DD/YYYY");

            function setRangeDate(start, end) {
                $from.val(start.format('MM/DD/YYYY'));
                $to.val(end.format('MM/DD/YYYY'));
                $dateRange.val(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
            }

            var now = moment($dateRange.attr('data-today'), "MM/DD/YYYY");
            var isoDayOfWeek = firstDayOfWeekIsMonday ? 1 : 7; // 1.Monday  7.Sunday
            var startDayOfWeek = now.clone().startOf('week').isoWeekday(isoDayOfWeek);
            var startDayOfLastWeek = now.clone().subtract(1, 'week').startOf('week').isoWeekday(isoDayOfWeek);

            var ranges = {};
            ranges[Site.getSingleResource('Today')] = [now, now];
            ranges[Site.getSingleResource('Yesterday')] = [now.clone().subtract(1, 'days'), now.clone().subtract(1, 'days')];
            ranges[Site.getSingleResource('Currentweek')] = [startDayOfWeek, now];
            ranges[Site.getSingleResource('Lastweek')] = [startDayOfLastWeek, startDayOfLastWeek.clone().add(6, 'days')];
            ranges[Site.getSingleResource('Currentmonth')] = [now.clone().startOf('month'), now];
            ranges[Site.getSingleResource('Lastmonth')] = [now.clone().subtract(1, 'month').startOf('month'), now.clone().subtract(1, 'month').endOf('month')];
            ranges[Site.getSingleResource('Sincelastmonth')] = [now.clone().subtract(1, 'month').startOf('month'), now];

            var customRanges = {};
            var hasRangeOption = rangeOption && rangeOption.length > 0;
            if (hasRangeOption) {
                for (var i = 0; i < rangeOption.length; i++) {
                    var rangeLabel = Site.getSingleResource(rangeOption[i]);
                    customRanges[rangeLabel] = ranges[rangeLabel];
                }
            }

            initCommonTextResource();
            var locale = {
                format: 'MM/DD/YYYY',
                applyLabel: Site.getSingleResource('OK'),
                cancelLabel: Site.getSingleResource('Cancel'),
                daysOfWeek: _daysOfWeek,
                monthNames: _monthNames,
                firstDay: firstDayOfWeekIsMonday ? 1 : 0
            };

            $dateRange.daterangepicker({
                startDate: start,
                endDate: end,
                minDate: moment($dateRange.attr('data-mindate'), "MM/DD/YYYY"),
                maxDate: moment($dateRange.attr('data-maxdate'), "MM/DD/YYYY"),
                ranges: hasRangeOption ? customRanges : ranges,
                locale: locale,
                showDropdowns: true,
                linkedCalendars: false,
                alwaysShowCalendars: true,
                showCustomRangeLabel: false
            }, setRangeDate);

            $dateRange.on('apply.daterangepicker', function (ev, picker) {
                setRangeDate(picker.startDate, picker.endDate);
            });

            $dateRange.on('show.daterangepicker', function (ev, picker) {
                var toDate = picker.endDate || new Date();
                var toMoment = moment(toDate);

                picker.rightCalendar.month = toMoment;
                picker.renderCalendar("right");
            });

            $('.date-range').on("click", ".icon-calendar", function () {
                $dateRange.data('daterangepicker').toggle();
            });

            setRangeDate(start, end);
        }

        function initSingleDatePicker(idElement, parentElement) {
            var $picker = $("#" + idElement);
            initCommonTextResource();
            var locale = {
                format: 'MM/DD/YYYY',
                daysOfWeek: _daysOfWeek,
                monthNames: _monthNames
            };

            $picker.daterangepicker({
                startDate: moment($picker.attr('data-original'), "MM/DD/YYYY"),
                endDate: moment($picker.attr('data-original'), "MM/DD/YYYY"),
                minDate: moment($picker.attr('data-mindate'), "MM/DD/YYYY"),
                maxDate: moment($picker.attr('data-maxdate'), "MM/DD/YYYY"),
                locale: locale,
                singleDatePicker: true,
                showDropdowns: true,
                parentEl: parentElement && parentElement !== "" ? parentElement : ".single-date"
            });

            $('.single-date').on("click", ".icon-calendar", function () {
                $picker.data('daterangepicker').toggle();
            });
        }

        function setValueForDateRange(startDate, endDate) {
            if (startDate) {
                $('#DateRange').data('daterangepicker').setStartDate(startDate);
                $('#FromDate').val(startDate);
            }

            if (endDate) {
                $('#DateRange').data('daterangepicker').setEndDate(endDate);
                $('#ToDate').val(startDate);
            }
        }

        function setValueForSingleDate($element, dateValue) {
            $element.val(dateValue);
        }

        return {
            initDateRangePicker: initDateRangePicker,
            initSingleDatePicker: initSingleDatePicker,
            setValueForDateRange: setValueForDateRange,
            setValueForSingleDate: setValueForSingleDate
        };
    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 12:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/**
* @version: 2.1.25
* @author: Dan Grossman http://www.dangrossman.info/
* @copyright: Copyright (c) 2012-2017 Dan Grossman. All rights reserved.
* @license: Licensed under the MIT license. See http://www.opensource.org/licenses/mit-license.php
* @website: https://www.daterangepicker.com/
*/
// Follow the UMD template https://github.com/umdjs/umd/blob/master/templates/returnExportsGlobal.js
(function (root, factory) {
    if (true) {
        // AMD. Make globaly available as well
        !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(2), __webpack_require__(0)], __WEBPACK_AMD_DEFINE_RESULT__ = function (moment, jquery) {
            return (root.daterangepicker = factory(moment, jquery));
        }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
    } else if (typeof module === 'object' && module.exports) {
        // Node / Browserify
        //isomorphic issue
        var jQuery = (typeof window != 'undefined') ? window.jQuery : undefined;
        if (!jQuery) {
            jQuery = require('jquery');
            if (!jQuery.fn) jQuery.fn = {};
        }
        module.exports = factory(require('moment'), jQuery);
    } else {
        // Browser globals
        root.daterangepicker = factory(root.moment, root.jQuery);
    }
}(this, function(moment, $) {
    var DateRangePicker = function(element, options, cb) {

        //default settings for options
        this.parentEl = 'body';
        this.element = $(element);
        this.startDate = moment().startOf('day');
        this.endDate = moment().endOf('day');
        this.minDate = false;
        this.maxDate = false;
        this.dateLimit = false;
        this.autoApply = false;
        this.singleDatePicker = false;
        this.showDropdowns = false;
        this.showWeekNumbers = false;
        this.showISOWeekNumbers = false;
        this.showCustomRangeLabel = true;
        this.timePicker = false;
        this.timePicker24Hour = false;
        this.timePickerIncrement = 1;
        this.timePickerSeconds = false;
        this.linkedCalendars = true;
        this.autoUpdateInput = true;
        this.alwaysShowCalendars = false;
        this.ranges = {};

        this.opens = 'right';
        if (this.element.hasClass('pull-right'))
            this.opens = 'left';

        this.drops = 'down';
        if (this.element.hasClass('dropup'))
            this.drops = 'up';

        this.buttonClasses = 'btn btn-sm';
        this.applyClass = 'btn-success';
        this.cancelClass = 'btn-default';

        this.locale = {
            direction: 'ltr',
            format: moment.localeData().longDateFormat('L'),
            separator: ' - ',
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            weekLabel: 'W',
            customRangeLabel: 'Custom Range',
            daysOfWeek: moment.weekdaysMin(),
            monthNames: moment.monthsShort(),
            firstDay: moment.localeData().firstDayOfWeek()
        };

        this.callback = function() { };

        //some state information
        this.isShowing = false;
        this.leftCalendar = {};
        this.rightCalendar = {};

        //custom options from user
        if (typeof options !== 'object' || options === null)
            options = {};

        //allow setting options with data attributes
        //data-api options will be overwritten with custom javascript options
        options = $.extend(this.element.data(), options);

        //html template for the picker UI
        if (typeof options.template !== 'string' && !(options.template instanceof $))
            options.template = '<div class="daterangepicker dropdown-menu">' +
                '<div class="calendar left">' +
                    '<div class="daterangepicker_input">' +
                      '<input class="input-mini form-control" type="text" name="daterangepicker_start" value="" />' +
                      '<i class="fa fa-calendar glyphicon glyphicon-calendar"></i>' +
                      '<div class="calendar-time">' +
                        '<div></div>' +
                        '<i class="fa fa-clock-o glyphicon glyphicon-time"></i>' +
                      '</div>' +
                    '</div>' +
                    '<div class="calendar-table"></div>' +
                '</div>' +
                '<div class="calendar right">' +
                    '<div class="daterangepicker_input">' +
                      '<input class="input-mini form-control" type="text" name="daterangepicker_end" value="" />' +
                      '<i class="fa fa-calendar glyphicon glyphicon-calendar"></i>' +
                      '<div class="calendar-time">' +
                        '<div></div>' +
                        '<i class="fa fa-clock-o glyphicon glyphicon-time"></i>' +
                      '</div>' +
                    '</div>' +
                    '<div class="calendar-table"></div>' +
                '</div>' +
                '<div class="ranges">' +
                    '<div class="range_inputs">' +
                        '<button class="applyBtn" disabled="disabled" type="button"></button> ' +
                        '<button class="cancelBtn" type="button"></button>' +
                    '</div>' +
                '</div>' +
            '</div>';

        this.parentEl = (options.parentEl && $(options.parentEl).length) ? $(options.parentEl) : $(this.parentEl);
        this.container = $(options.template).appendTo(this.parentEl);

        //
        // handle all the possible options overriding defaults
        //

        if (typeof options.locale === 'object') {

            if (typeof options.locale.direction === 'string')
                this.locale.direction = options.locale.direction;

            if (typeof options.locale.format === 'string')
                this.locale.format = options.locale.format;

            if (typeof options.locale.separator === 'string')
                this.locale.separator = options.locale.separator;

            if (typeof options.locale.daysOfWeek === 'object')
                this.locale.daysOfWeek = options.locale.daysOfWeek.slice();

            if (typeof options.locale.monthNames === 'object')
              this.locale.monthNames = options.locale.monthNames.slice();

            if (typeof options.locale.firstDay === 'number')
              this.locale.firstDay = options.locale.firstDay;

            if (typeof options.locale.applyLabel === 'string')
              this.locale.applyLabel = options.locale.applyLabel;

            if (typeof options.locale.cancelLabel === 'string')
              this.locale.cancelLabel = options.locale.cancelLabel;

            if (typeof options.locale.weekLabel === 'string')
              this.locale.weekLabel = options.locale.weekLabel;

            if (typeof options.locale.customRangeLabel === 'string'){
                //Support unicode chars in the custom range name.
                var elem = document.createElement('textarea');
                elem.innerHTML = options.locale.customRangeLabel;
                var rangeHtml = elem.value;
                this.locale.customRangeLabel = rangeHtml;
            }
        }
        this.container.addClass(this.locale.direction);

        if (typeof options.startDate === 'string')
            this.startDate = moment(options.startDate, this.locale.format);

        if (typeof options.endDate === 'string')
            this.endDate = moment(options.endDate, this.locale.format);

        if (typeof options.minDate === 'string')
            this.minDate = moment(options.minDate, this.locale.format);

        if (typeof options.maxDate === 'string')
            this.maxDate = moment(options.maxDate, this.locale.format);

        if (typeof options.startDate === 'object')
            this.startDate = moment(options.startDate);

        if (typeof options.endDate === 'object')
            this.endDate = moment(options.endDate);

        if (typeof options.minDate === 'object')
            this.minDate = moment(options.minDate);

        if (typeof options.maxDate === 'object')
            this.maxDate = moment(options.maxDate);

        // sanity check for bad options
        if (this.minDate && this.startDate.isBefore(this.minDate))
            this.startDate = this.minDate.clone();

        // sanity check for bad options
        if (this.maxDate && this.endDate.isAfter(this.maxDate))
            this.endDate = this.maxDate.clone();

        if (typeof options.applyClass === 'string')
            this.applyClass = options.applyClass;

        if (typeof options.cancelClass === 'string')
            this.cancelClass = options.cancelClass;

        if (typeof options.dateLimit === 'object')
            this.dateLimit = options.dateLimit;

        if (typeof options.opens === 'string')
            this.opens = options.opens;

        if (typeof options.drops === 'string')
            this.drops = options.drops;

        if (typeof options.showWeekNumbers === 'boolean')
            this.showWeekNumbers = options.showWeekNumbers;

        if (typeof options.showISOWeekNumbers === 'boolean')
            this.showISOWeekNumbers = options.showISOWeekNumbers;

        if (typeof options.buttonClasses === 'string')
            this.buttonClasses = options.buttonClasses;

        if (typeof options.buttonClasses === 'object')
            this.buttonClasses = options.buttonClasses.join(' ');

        if (typeof options.showDropdowns === 'boolean')
            this.showDropdowns = options.showDropdowns;

        if (typeof options.showCustomRangeLabel === 'boolean')
            this.showCustomRangeLabel = options.showCustomRangeLabel;

        if (typeof options.singleDatePicker === 'boolean') {
            this.singleDatePicker = options.singleDatePicker;
            if (this.singleDatePicker)
                this.endDate = this.startDate.clone();
        }

        if (typeof options.timePicker === 'boolean')
            this.timePicker = options.timePicker;

        if (typeof options.timePickerSeconds === 'boolean')
            this.timePickerSeconds = options.timePickerSeconds;

        if (typeof options.timePickerIncrement === 'number')
            this.timePickerIncrement = options.timePickerIncrement;

        if (typeof options.timePicker24Hour === 'boolean')
            this.timePicker24Hour = options.timePicker24Hour;

        if (typeof options.autoApply === 'boolean')
            this.autoApply = options.autoApply;

        if (typeof options.autoUpdateInput === 'boolean')
            this.autoUpdateInput = options.autoUpdateInput;

        if (typeof options.linkedCalendars === 'boolean')
            this.linkedCalendars = options.linkedCalendars;

        if (typeof options.isInvalidDate === 'function')
            this.isInvalidDate = options.isInvalidDate;

        if (typeof options.isCustomDate === 'function')
            this.isCustomDate = options.isCustomDate;

        if (typeof options.alwaysShowCalendars === 'boolean')
            this.alwaysShowCalendars = options.alwaysShowCalendars;

        // update day names order to firstDay
        if (this.locale.firstDay != 0) {
            var iterator = this.locale.firstDay;
            while (iterator > 0) {
                this.locale.daysOfWeek.push(this.locale.daysOfWeek.shift());
                iterator--;
            }
        }

        var start, end, range;

        //if no start/end dates set, check if an input element contains initial values
        if (typeof options.startDate === 'undefined' && typeof options.endDate === 'undefined') {
            if ($(this.element).is('input[type=text]')) {
                var val = $(this.element).val(),
                    split = val.split(this.locale.separator);

                start = end = null;

                if (split.length == 2) {
                    start = moment(split[0], this.locale.format);
                    end = moment(split[1], this.locale.format);
                } else if (this.singleDatePicker && val !== "") {
                    start = moment(val, this.locale.format);
                    end = moment(val, this.locale.format);
                }
                if (start !== null && end !== null) {
                    this.setStartDate(start);
                    this.setEndDate(end);
                }
            }
        }

        if (typeof options.ranges === 'object') {
            for (range in options.ranges) {

                if (typeof options.ranges[range][0] === 'string')
                    start = moment(options.ranges[range][0], this.locale.format);
                else
                    start = moment(options.ranges[range][0]);

                if (typeof options.ranges[range][1] === 'string')
                    end = moment(options.ranges[range][1], this.locale.format);
                else
                    end = moment(options.ranges[range][1]);

                // If the start or end date exceed those allowed by the minDate or dateLimit
                // options, shorten the range to the allowable period.
                if (this.minDate && start.isBefore(this.minDate))
                    start = this.minDate.clone();

                var maxDate = this.maxDate;
                if (this.dateLimit && maxDate && start.clone().add(this.dateLimit).isAfter(maxDate))
                    maxDate = start.clone().add(this.dateLimit);
                if (maxDate && end.isAfter(maxDate))
                    end = maxDate.clone();

                // If the end of the range is before the minimum or the start of the range is
                // after the maximum, don't display this range option at all.
                if ((this.minDate && end.isBefore(this.minDate, this.timepicker ? 'minute' : 'day')) 
                  || (maxDate && start.isAfter(maxDate, this.timepicker ? 'minute' : 'day')))
                    continue;

                //Support unicode chars in the range names.
                var elem = document.createElement('textarea');
                elem.innerHTML = range;
                var rangeHtml = elem.value;

                this.ranges[rangeHtml] = [start, end];
            }

            var list = '<ul>';
            for (range in this.ranges) {
                list += '<li data-range-key="' + range + '">' + range + '</li>';
            }
            if (this.showCustomRangeLabel) {
                list += '<li data-range-key="' + this.locale.customRangeLabel + '">' + this.locale.customRangeLabel + '</li>';
            }
            list += '</ul>';
            this.container.find('.ranges').prepend(list);
        }

        if (typeof cb === 'function') {
            this.callback = cb;
        }

        if (!this.timePicker) {
            this.startDate = this.startDate.startOf('day');
            this.endDate = this.endDate.endOf('day');
            this.container.find('.calendar-time').hide();
        }

        //can't be used together for now
        if (this.timePicker && this.autoApply)
            this.autoApply = false;

        if (this.autoApply && typeof options.ranges !== 'object') {
            this.container.find('.ranges').hide();
        } else if (this.autoApply) {
            this.container.find('.applyBtn, .cancelBtn').addClass('hide');
        }

        if (this.singleDatePicker) {
            this.container.addClass('single');
            this.container.find('.calendar.left').addClass('single');
            this.container.find('.calendar.left').show();
            this.container.find('.calendar.right').hide();
            this.container.find('.daterangepicker_input input, .daterangepicker_input > i').hide();
            if (this.timePicker) {
                this.container.find('.ranges ul').hide();
            } else {
                this.container.find('.ranges').hide();
            }
        }

        if ((typeof options.ranges === 'undefined' && !this.singleDatePicker) || this.alwaysShowCalendars) {
            this.container.addClass('show-calendar');
        }

        this.container.addClass('opens' + this.opens);

        //swap the position of the predefined ranges if opens right
        if (typeof options.ranges !== 'undefined' && this.opens == 'right') {
            this.container.find('.ranges').prependTo( this.container.find('.calendar.left').parent() );
        }

        //apply CSS classes and labels to buttons
        this.container.find('.applyBtn, .cancelBtn').addClass(this.buttonClasses);
        if (this.applyClass.length)
            this.container.find('.applyBtn').addClass(this.applyClass);
        if (this.cancelClass.length)
            this.container.find('.cancelBtn').addClass(this.cancelClass);
        this.container.find('.applyBtn').html(this.locale.applyLabel);
        this.container.find('.cancelBtn').html(this.locale.cancelLabel);

        //
        // event listeners
        //

        this.container.find('.calendar')
            .on('click.daterangepicker', '.prev', $.proxy(this.clickPrev, this))
            .on('click.daterangepicker', '.next', $.proxy(this.clickNext, this))
            .on('mousedown.daterangepicker', 'td.available', $.proxy(this.clickDate, this))
            .on('mouseenter.daterangepicker', 'td.available', $.proxy(this.hoverDate, this))
            .on('mouseleave.daterangepicker', 'td.available', $.proxy(this.updateFormInputs, this))
            .on('change.daterangepicker', 'select.yearselect', $.proxy(this.monthOrYearChanged, this))
            .on('change.daterangepicker', 'select.monthselect', $.proxy(this.monthOrYearChanged, this))
            .on('change.daterangepicker', 'select.hourselect,select.minuteselect,select.secondselect,select.ampmselect', $.proxy(this.timeChanged, this))
            .on('click.daterangepicker', '.daterangepicker_input input', $.proxy(this.showCalendars, this))
            .on('focus.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsFocused, this))
            .on('blur.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsBlurred, this))
            .on('change.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsChanged, this));

        this.container.find('.ranges')
            .on('click.daterangepicker', 'button.applyBtn', $.proxy(this.clickApply, this))
            .on('click.daterangepicker', 'button.cancelBtn', $.proxy(this.clickCancel, this))
            .on('click.daterangepicker', 'li', $.proxy(this.clickRange, this))
            .on('mouseenter.daterangepicker', 'li', $.proxy(this.hoverRange, this))
            .on('mouseleave.daterangepicker', 'li', $.proxy(this.updateFormInputs, this));

        if (this.element.is('input') || this.element.is('button')) {
            this.element.on({
                'click.daterangepicker': $.proxy(this.show, this),
                'focus.daterangepicker': $.proxy(this.show, this),
                'keyup.daterangepicker': $.proxy(this.elementChanged, this),
                'keydown.daterangepicker': $.proxy(this.keydown, this)
            });
        } else {
            this.element.on('click.daterangepicker', $.proxy(this.toggle, this));
        }

        //
        // if attached to a text input, set the initial value
        //

        if (this.element.is('input') && !this.singleDatePicker && this.autoUpdateInput) {
            this.element.val(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format));
            this.element.trigger('change');
        } else if (this.element.is('input') && this.autoUpdateInput) {
            this.element.val(this.startDate.format(this.locale.format));
            this.element.trigger('change');
        }

    };

    DateRangePicker.prototype = {

        constructor: DateRangePicker,

        setStartDate: function(startDate) {
            if (typeof startDate === 'string')
                this.startDate = moment(startDate, this.locale.format);

            if (typeof startDate === 'object')
                this.startDate = moment(startDate);

            if (!this.timePicker)
                this.startDate = this.startDate.startOf('day');

            if (this.timePicker && this.timePickerIncrement)
                this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);

            if (this.minDate && this.startDate.isBefore(this.minDate)) {
                this.startDate = this.minDate.clone();
                if (this.timePicker && this.timePickerIncrement)
                    this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);
            }

            if (this.maxDate && this.startDate.isAfter(this.maxDate)) {
                this.startDate = this.maxDate.clone();
                if (this.timePicker && this.timePickerIncrement)
                    this.startDate.minute(Math.floor(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);
            }

            if (!this.isShowing)
                this.updateElement();

            this.updateMonthsInView();
        },

        setEndDate: function(endDate) {
            if (typeof endDate === 'string')
                this.endDate = moment(endDate, this.locale.format);

            if (typeof endDate === 'object')
                this.endDate = moment(endDate);

            if (!this.timePicker)
                this.endDate = this.endDate.endOf('day');

            if (this.timePicker && this.timePickerIncrement)
                this.endDate.minute(Math.round(this.endDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);

            if (this.endDate.isBefore(this.startDate))
                this.endDate = this.startDate.clone();

            if (this.maxDate && this.endDate.isAfter(this.maxDate))
                this.endDate = this.maxDate.clone();

            if (this.dateLimit && this.startDate.clone().add(this.dateLimit).isBefore(this.endDate))
                this.endDate = this.startDate.clone().add(this.dateLimit);

            this.previousRightTime = this.endDate.clone();

            if (!this.isShowing)
                this.updateElement();

            this.updateMonthsInView();
        },

        isInvalidDate: function() {
            return false;
        },

        isCustomDate: function() {
            return false;
        },

        updateView: function() {
            if (this.timePicker) {
                this.renderTimePicker('left');
                this.renderTimePicker('right');
                if (!this.endDate) {
                    this.container.find('.right .calendar-time select').attr('disabled', 'disabled').addClass('disabled');
                } else {
                    this.container.find('.right .calendar-time select').removeAttr('disabled').removeClass('disabled');
                }
            }
            if (this.endDate) {
                this.container.find('input[name="daterangepicker_end"]').removeClass('active');
                this.container.find('input[name="daterangepicker_start"]').addClass('active');
            } else {
                this.container.find('input[name="daterangepicker_end"]').addClass('active');
                this.container.find('input[name="daterangepicker_start"]').removeClass('active');
            }
            this.updateMonthsInView();
            this.updateCalendars();
            this.updateFormInputs();
        },

        updateMonthsInView: function() {
            if (this.endDate) {

                //if both dates are visible already, do nothing
                if (!this.singleDatePicker && this.leftCalendar.month && this.rightCalendar.month &&
                    (this.startDate.format('YYYY-MM') == this.leftCalendar.month.format('YYYY-MM') || this.startDate.format('YYYY-MM') == this.rightCalendar.month.format('YYYY-MM'))
                    &&
                    (this.endDate.format('YYYY-MM') == this.leftCalendar.month.format('YYYY-MM') || this.endDate.format('YYYY-MM') == this.rightCalendar.month.format('YYYY-MM'))
                    ) {
                    return;
                }

                this.leftCalendar.month = this.startDate.clone().date(2);
                if (!this.linkedCalendars && (this.endDate.month() != this.startDate.month() || this.endDate.year() != this.startDate.year())) {
                    this.rightCalendar.month = this.endDate.clone().date(2);
                } else {
                    this.rightCalendar.month = this.startDate.clone().date(2).add(1, 'month');
                }

            } else {
                if (this.leftCalendar.month.format('YYYY-MM') != this.startDate.format('YYYY-MM') && this.rightCalendar.month.format('YYYY-MM') != this.startDate.format('YYYY-MM')) {
                    this.leftCalendar.month = this.startDate.clone().date(2);
                    this.rightCalendar.month = this.startDate.clone().date(2).add(1, 'month');
                }
            }
            if (this.maxDate && this.linkedCalendars && !this.singleDatePicker && this.rightCalendar.month > this.maxDate) {
              this.rightCalendar.month = this.maxDate.clone().date(2);
              this.leftCalendar.month = this.maxDate.clone().date(2).subtract(1, 'month');
            }
        },

        updateCalendars: function() {

            if (this.timePicker) {
                var hour, minute, second;
                if (this.endDate) {
                    hour = parseInt(this.container.find('.left .hourselect').val(), 10);
                    minute = parseInt(this.container.find('.left .minuteselect').val(), 10);
                    second = this.timePickerSeconds ? parseInt(this.container.find('.left .secondselect').val(), 10) : 0;
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.left .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                } else {
                    hour = parseInt(this.container.find('.right .hourselect').val(), 10);
                    minute = parseInt(this.container.find('.right .minuteselect').val(), 10);
                    second = this.timePickerSeconds ? parseInt(this.container.find('.right .secondselect').val(), 10) : 0;
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.right .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                }
                this.leftCalendar.month.hour(hour).minute(minute).second(second);
                this.rightCalendar.month.hour(hour).minute(minute).second(second);
            }

            this.renderCalendar('left');
            this.renderCalendar('right');

            //highlight any predefined range matching the current start and end dates
            this.container.find('.ranges li').removeClass('active');
            if (this.endDate == null) return;

            this.calculateChosenLabel();
        },

        renderCalendar: function(side) {

            //
            // Build the matrix of dates that will populate the calendar
            //

            var calendar = side == 'left' ? this.leftCalendar : this.rightCalendar;
            var month = calendar.month.month();
            var year = calendar.month.year();
            var hour = calendar.month.hour();
            var minute = calendar.month.minute();
            var second = calendar.month.second();
            var daysInMonth = moment([year, month]).daysInMonth();
            var firstDay = moment([year, month, 1]);
            var lastDay = moment([year, month, daysInMonth]);
            var lastMonth = moment(firstDay).subtract(1, 'month').month();
            var lastYear = moment(firstDay).subtract(1, 'month').year();
            var daysInLastMonth = moment([lastYear, lastMonth]).daysInMonth();
            var dayOfWeek = firstDay.day();

            //initialize a 6 rows x 7 columns array for the calendar
            var calendar = [];
            calendar.firstDay = firstDay;
            calendar.lastDay = lastDay;

            for (var i = 0; i < 6; i++) {
                calendar[i] = [];
            }

            //populate the calendar with date objects
            var startDay = daysInLastMonth - dayOfWeek + this.locale.firstDay + 1;
            if (startDay > daysInLastMonth)
                startDay -= 7;

            if (dayOfWeek == this.locale.firstDay)
                startDay = daysInLastMonth - 6;

            var curDate = moment([lastYear, lastMonth, startDay, 12, minute, second]);

            var col, row;
            for (var i = 0, col = 0, row = 0; i < 42; i++, col++, curDate = moment(curDate).add(24, 'hour')) {
                if (i > 0 && col % 7 === 0) {
                    col = 0;
                    row++;
                }
                calendar[row][col] = curDate.clone().hour(hour).minute(minute).second(second);
                curDate.hour(12);

                if (this.minDate && calendar[row][col].format('YYYY-MM-DD') == this.minDate.format('YYYY-MM-DD') && calendar[row][col].isBefore(this.minDate) && side == 'left') {
                    calendar[row][col] = this.minDate.clone();
                }

                if (this.maxDate && calendar[row][col].format('YYYY-MM-DD') == this.maxDate.format('YYYY-MM-DD') && calendar[row][col].isAfter(this.maxDate) && side == 'right') {
                    calendar[row][col] = this.maxDate.clone();
                }

            }

            //make the calendar object available to hoverDate/clickDate
            if (side == 'left') {
                this.leftCalendar.calendar = calendar;
            } else {
                this.rightCalendar.calendar = calendar;
            }

            //
            // Display the calendar
            //

            var minDate = side == 'left' ? this.minDate : this.startDate;
            var maxDate = this.maxDate;
            var selected = side == 'left' ? this.startDate : this.endDate;
            var arrow = this.locale.direction == 'ltr' ? {left: 'chevron-left', right: 'chevron-right'} : {left: 'chevron-right', right: 'chevron-left'};

            var html = '<table class="table-condensed">';
            html += '<thead>';
            html += '<tr>';

            // add empty cell for week number
            if (this.showWeekNumbers || this.showISOWeekNumbers)
                html += '<th></th>';

            if ((!minDate || minDate.isBefore(calendar.firstDay)) && (!this.linkedCalendars || side == 'left')) {
                html += '<th class="prev available"><i class="fa fa-' + arrow.left + ' glyphicon glyphicon-' + arrow.left + '"></i></th>';
            } else {
                html += '<th></th>';
            }

            var dateHtml = this.locale.monthNames[calendar[1][1].month()] + calendar[1][1].format(" YYYY");

            if (this.showDropdowns) {
                var currentMonth = calendar[1][1].month();
                var currentYear = calendar[1][1].year();
                var maxYear = (maxDate && maxDate.year()) || (currentYear + 5);
                var minYear = (minDate && minDate.year()) || (currentYear - 50);
                var inMinYear = currentYear == minYear;
                var inMaxYear = currentYear == maxYear;

                var monthHtml = '<select class="monthselect">';
                for (var m = 0; m < 12; m++) {
                    if ((!inMinYear || m >= minDate.month()) && (!inMaxYear || m <= maxDate.month())) {
                        monthHtml += "<option value='" + m + "'" +
                            (m === currentMonth ? " selected='selected'" : "") +
                            ">" + this.locale.monthNames[m] + "</option>";
                    } else {
                        monthHtml += "<option value='" + m + "'" +
                            (m === currentMonth ? " selected='selected'" : "") +
                            " disabled='disabled'>" + this.locale.monthNames[m] + "</option>";
                    }
                }
                monthHtml += "</select>";

                var yearHtml = '<select class="yearselect">';
                for (var y = minYear; y <= maxYear; y++) {
                    yearHtml += '<option value="' + y + '"' +
                        (y === currentYear ? ' selected="selected"' : '') +
                        '>' + y + '</option>';
                }
                yearHtml += '</select>';

                dateHtml = monthHtml + yearHtml;
            }

            html += '<th colspan="5" class="month">' + dateHtml + '</th>';
            if ((!maxDate || maxDate.isAfter(calendar.lastDay)) && (!this.linkedCalendars || side == 'right' || this.singleDatePicker)) {
                html += '<th class="next available"><i class="fa fa-' + arrow.right + ' glyphicon glyphicon-' + arrow.right + '"></i></th>';
            } else {
                html += '<th></th>';
            }

            html += '</tr>';
            html += '<tr>';

            // add week number label
            if (this.showWeekNumbers || this.showISOWeekNumbers)
                html += '<th class="week">' + this.locale.weekLabel + '</th>';

            $.each(this.locale.daysOfWeek, function(index, dayOfWeek) {
                html += '<th>' + dayOfWeek + '</th>';
            });

            html += '</tr>';
            html += '</thead>';
            html += '<tbody>';

            //adjust maxDate to reflect the dateLimit setting in order to
            //grey out end dates beyond the dateLimit
            if (this.endDate == null && this.dateLimit) {
                var maxLimit = this.startDate.clone().add(this.dateLimit).endOf('day');
                if (!maxDate || maxLimit.isBefore(maxDate)) {
                    maxDate = maxLimit;
                }
            }

            for (var row = 0; row < 6; row++) {
                html += '<tr>';

                // add week number
                if (this.showWeekNumbers)
                    html += '<td class="week">' + calendar[row][0].week() + '</td>';
                else if (this.showISOWeekNumbers)
                    html += '<td class="week">' + calendar[row][0].isoWeek() + '</td>';

                for (var col = 0; col < 7; col++) {

                    var classes = [];

                    //highlight today's date
                    if (calendar[row][col].isSame(new Date(), "day"))
                        classes.push('today');

                    //highlight weekends
                    if (calendar[row][col].isoWeekday() > 5)
                        classes.push('weekend');

                    //grey out the dates in other months displayed at beginning and end of this calendar
                    if (calendar[row][col].month() != calendar[1][1].month())
                        classes.push('off');

                    //don't allow selection of dates before the minimum date
                    if (this.minDate && calendar[row][col].isBefore(this.minDate, 'day'))
                        classes.push('off', 'disabled');

                    //don't allow selection of dates after the maximum date
                    if (maxDate && calendar[row][col].isAfter(maxDate, 'day'))
                        classes.push('off', 'disabled');

                    //don't allow selection of date if a custom function decides it's invalid
                    if (this.isInvalidDate(calendar[row][col]))
                        classes.push('off', 'disabled');

                    //highlight the currently selected start date
                    if (calendar[row][col].format('YYYY-MM-DD') == this.startDate.format('YYYY-MM-DD'))
                        classes.push('active', 'start-date');

                    //highlight the currently selected end date
                    if (this.endDate != null && calendar[row][col].format('YYYY-MM-DD') == this.endDate.format('YYYY-MM-DD'))
                        classes.push('active', 'end-date');

                    //highlight dates in-between the selected dates
                    if (this.endDate != null && calendar[row][col] > this.startDate && calendar[row][col] < this.endDate)
                        classes.push('in-range');

                    //apply custom classes for this date
                    var isCustom = this.isCustomDate(calendar[row][col]);
                    if (isCustom !== false) {
                        if (typeof isCustom === 'string')
                            classes.push(isCustom);
                        else
                            Array.prototype.push.apply(classes, isCustom);
                    }

                    var cname = '', disabled = false;
                    for (var i = 0; i < classes.length; i++) {
                        cname += classes[i] + ' ';
                        if (classes[i] == 'disabled')
                            disabled = true;
                    }
                    if (!disabled)
                        cname += 'available';

                    html += '<td class="' + cname.replace(/^\s+|\s+$/g, '') + '" data-title="' + 'r' + row + 'c' + col + '">' + calendar[row][col].date() + '</td>';

                }
                html += '</tr>';
            }

            html += '</tbody>';
            html += '</table>';

            this.container.find('.calendar.' + side + ' .calendar-table').html(html);

        },

        renderTimePicker: function(side) {

            // Don't bother updating the time picker if it's currently disabled
            // because an end date hasn't been clicked yet
            if (side == 'right' && !this.endDate) return;

            var html, selected, minDate, maxDate = this.maxDate;

            if (this.dateLimit && (!this.maxDate || this.startDate.clone().add(this.dateLimit).isAfter(this.maxDate)))
                maxDate = this.startDate.clone().add(this.dateLimit);

            if (side == 'left') {
                selected = this.startDate.clone();
                minDate = this.minDate;
            } else if (side == 'right') {
                selected = this.endDate.clone();
                minDate = this.startDate;

                //Preserve the time already selected
                var timeSelector = this.container.find('.calendar.right .calendar-time div');
                if (timeSelector.html() != '') {

                    selected.hour(timeSelector.find('.hourselect option:selected').val() || selected.hour());
                    selected.minute(timeSelector.find('.minuteselect option:selected').val() || selected.minute());
                    selected.second(timeSelector.find('.secondselect option:selected').val() || selected.second());

                    if (!this.timePicker24Hour) {
                        var ampm = timeSelector.find('.ampmselect option:selected').val();
                        if (ampm === 'PM' && selected.hour() < 12)
                            selected.hour(selected.hour() + 12);
                        if (ampm === 'AM' && selected.hour() === 12)
                            selected.hour(0);
                    }

                }

                if (selected.isBefore(this.startDate))
                    selected = this.startDate.clone();

                if (maxDate && selected.isAfter(maxDate))
                    selected = maxDate.clone();

            }

            //
            // hours
            //

            html = '<select class="hourselect">';

            var start = this.timePicker24Hour ? 0 : 1;
            var end = this.timePicker24Hour ? 23 : 12;

            for (var i = start; i <= end; i++) {
                var i_in_24 = i;
                if (!this.timePicker24Hour)
                    i_in_24 = selected.hour() >= 12 ? (i == 12 ? 12 : i + 12) : (i == 12 ? 0 : i);

                var time = selected.clone().hour(i_in_24);
                var disabled = false;
                if (minDate && time.minute(59).isBefore(minDate))
                    disabled = true;
                if (maxDate && time.minute(0).isAfter(maxDate))
                    disabled = true;

                if (i_in_24 == selected.hour() && !disabled) {
                    html += '<option value="' + i + '" selected="selected">' + i + '</option>';
                } else if (disabled) {
                    html += '<option value="' + i + '" disabled="disabled" class="disabled">' + i + '</option>';
                } else {
                    html += '<option value="' + i + '">' + i + '</option>';
                }
            }

            html += '</select> ';

            //
            // minutes
            //

            html += ': <select class="minuteselect">';

            for (var i = 0; i < 60; i += this.timePickerIncrement) {
                var padded = i < 10 ? '0' + i : i;
                var time = selected.clone().minute(i);

                var disabled = false;
                if (minDate && time.second(59).isBefore(minDate))
                    disabled = true;
                if (maxDate && time.second(0).isAfter(maxDate))
                    disabled = true;

                if (selected.minute() == i && !disabled) {
                    html += '<option value="' + i + '" selected="selected">' + padded + '</option>';
                } else if (disabled) {
                    html += '<option value="' + i + '" disabled="disabled" class="disabled">' + padded + '</option>';
                } else {
                    html += '<option value="' + i + '">' + padded + '</option>';
                }
            }

            html += '</select> ';

            //
            // seconds
            //

            if (this.timePickerSeconds) {
                html += ': <select class="secondselect">';

                for (var i = 0; i < 60; i++) {
                    var padded = i < 10 ? '0' + i : i;
                    var time = selected.clone().second(i);

                    var disabled = false;
                    if (minDate && time.isBefore(minDate))
                        disabled = true;
                    if (maxDate && time.isAfter(maxDate))
                        disabled = true;

                    if (selected.second() == i && !disabled) {
                        html += '<option value="' + i + '" selected="selected">' + padded + '</option>';
                    } else if (disabled) {
                        html += '<option value="' + i + '" disabled="disabled" class="disabled">' + padded + '</option>';
                    } else {
                        html += '<option value="' + i + '">' + padded + '</option>';
                    }
                }

                html += '</select> ';
            }

            //
            // AM/PM
            //

            if (!this.timePicker24Hour) {
                html += '<select class="ampmselect">';

                var am_html = '';
                var pm_html = '';

                if (minDate && selected.clone().hour(12).minute(0).second(0).isBefore(minDate))
                    am_html = ' disabled="disabled" class="disabled"';

                if (maxDate && selected.clone().hour(0).minute(0).second(0).isAfter(maxDate))
                    pm_html = ' disabled="disabled" class="disabled"';

                if (selected.hour() >= 12) {
                    html += '<option value="AM"' + am_html + '>AM</option><option value="PM" selected="selected"' + pm_html + '>PM</option>';
                } else {
                    html += '<option value="AM" selected="selected"' + am_html + '>AM</option><option value="PM"' + pm_html + '>PM</option>';
                }

                html += '</select>';
            }

            this.container.find('.calendar.' + side + ' .calendar-time div').html(html);

        },

        updateFormInputs: function() {

            //ignore mouse movements while an above-calendar text input has focus
            if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
                return;

            this.container.find('input[name=daterangepicker_start]').val(this.startDate.format(this.locale.format));
            if (this.endDate)
                this.container.find('input[name=daterangepicker_end]').val(this.endDate.format(this.locale.format));

            if (this.singleDatePicker || (this.endDate && (this.startDate.isBefore(this.endDate) || this.startDate.isSame(this.endDate)))) {
                this.container.find('button.applyBtn').removeAttr('disabled');
            } else {
                this.container.find('button.applyBtn').attr('disabled', 'disabled');
            }

        },

        move: function() {
            var parentOffset = { top: 0, left: 0 },
                containerTop;
            var parentRightEdge = $(window).width();
            if (!this.parentEl.is('body')) {
                parentOffset = {
                    top: this.parentEl.offset().top - this.parentEl.scrollTop(),
                    left: this.parentEl.offset().left - this.parentEl.scrollLeft()
                };
                parentRightEdge = this.parentEl[0].clientWidth + this.parentEl.offset().left;
            }

            if (this.drops == 'up')
                containerTop = this.element.offset().top - this.container.outerHeight() - parentOffset.top;
            else
                containerTop = this.element.offset().top + this.element.outerHeight() - parentOffset.top;
            this.container[this.drops == 'up' ? 'addClass' : 'removeClass']('dropup');

            if (this.opens == 'left') {
                this.container.css({
                    top: containerTop,
                    right: parentRightEdge - this.element.offset().left - this.element.outerWidth(),
                    left: 'auto'
                });
                if (this.container.offset().left < 0) {
                    this.container.css({
                        right: 'auto',
                        left: 9
                    });
                }
            } else if (this.opens == 'center') {
                this.container.css({
                    top: containerTop,
                    left: this.element.offset().left - parentOffset.left + this.element.outerWidth() / 2
                            - this.container.outerWidth() / 2,
                    right: 'auto'
                });
                if (this.container.offset().left < 0) {
                    this.container.css({
                        right: 'auto',
                        left: 9
                    });
                }
            } else {
                this.container.css({
                    top: containerTop,
                    left: this.element.offset().left - parentOffset.left,
                    right: 'auto'
                });
                if (this.container.offset().left + this.container.outerWidth() > $(window).width()) {
                    this.container.css({
                        left: 'auto',
                        right: 0
                    });
                }
            }
        },

        show: function(e) {
            if (this.isShowing) return;

            // Create a click proxy that is private to this instance of datepicker, for unbinding
            this._outsideClickProxy = $.proxy(function(e) { this.outsideClick(e); }, this);

            // Bind global datepicker mousedown for hiding and
            $(document)
              .on('mousedown.daterangepicker', this._outsideClickProxy)
              // also support mobile devices
              .on('touchend.daterangepicker', this._outsideClickProxy)
              // also explicitly play nice with Bootstrap dropdowns, which stopPropagation when clicking them
              .on('click.daterangepicker', '[data-toggle=dropdown]', this._outsideClickProxy)
              // and also close when focus changes to outside the picker (eg. tabbing between controls)
              .on('focusin.daterangepicker', this._outsideClickProxy);

            // Reposition the picker if the window is resized while it's open
            $(window).on('resize.daterangepicker', $.proxy(function(e) { this.move(e); }, this));

            this.oldStartDate = this.startDate.clone();
            this.oldEndDate = this.endDate.clone();
            this.previousRightTime = this.endDate.clone();

            this.updateView();
            this.container.show();
            this.move();
            this.element.trigger('show.daterangepicker', this);
            this.isShowing = true;
        },

        hide: function(e) {
            if (!this.isShowing) return;

            //incomplete date selection, revert to last values
            if (!this.endDate) {
                this.startDate = this.oldStartDate.clone();
                this.endDate = this.oldEndDate.clone();
            }

            //if a new date range was selected, invoke the user callback function
            if (!this.startDate.isSame(this.oldStartDate) || !this.endDate.isSame(this.oldEndDate))
                this.callback(this.startDate, this.endDate, this.chosenLabel);

            //if picker is attached to a text input, update it
            this.updateElement();

            $(document).off('.daterangepicker');
            $(window).off('.daterangepicker');
            this.container.hide();
            this.element.trigger('hide.daterangepicker', this);
            this.isShowing = false;
        },

        toggle: function(e) {
            if (this.isShowing) {
                this.hide();
            } else {
                this.show();
            }
        },

        outsideClick: function(e) {
            var target = $(e.target);
            // if the page is clicked anywhere except within the daterangerpicker/button
            // itself then call this.hide()
            if (
                // ie modal dialog fix
                e.type == "focusin" ||
                target.closest(this.element).length ||
                target.closest(this.container).length ||
                target.closest('.calendar-table').length
                ) return;
            this.hide();
            this.element.trigger('outsideClick.daterangepicker', this);
        },

        showCalendars: function() {
            this.container.addClass('show-calendar');
            this.move();
            this.element.trigger('showCalendar.daterangepicker', this);
        },

        hideCalendars: function() {
            this.container.removeClass('show-calendar');
            this.element.trigger('hideCalendar.daterangepicker', this);
        },

        hoverRange: function(e) {

            //ignore mouse movements while an above-calendar text input has focus
            if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
                return;

            var label = e.target.getAttribute('data-range-key');

            if (label == this.locale.customRangeLabel) {
                this.updateView();
            } else {
                var dates = this.ranges[label];
                this.container.find('input[name=daterangepicker_start]').val(dates[0].format(this.locale.format));
                this.container.find('input[name=daterangepicker_end]').val(dates[1].format(this.locale.format));
            }

        },

        clickRange: function(e) {
            var label = e.target.getAttribute('data-range-key');
            this.chosenLabel = label;
            if (label == this.locale.customRangeLabel) {
                this.showCalendars();
            } else {
                var dates = this.ranges[label];
                this.startDate = dates[0];
                this.endDate = dates[1];

                if (!this.timePicker) {
                    this.startDate.startOf('day');
                    this.endDate.endOf('day');
                }

                if (!this.alwaysShowCalendars)
                    this.hideCalendars();
                this.clickApply();
            }
        },

        clickPrev: function(e) {
            var cal = $(e.target).parents('.calendar');
            if (cal.hasClass('left')) {
                this.leftCalendar.month.subtract(1, 'month');
                if (this.linkedCalendars)
                    this.rightCalendar.month.subtract(1, 'month');
            } else {
                this.rightCalendar.month.subtract(1, 'month');
            }
            this.updateCalendars();
        },

        clickNext: function(e) {
            var cal = $(e.target).parents('.calendar');
            if (cal.hasClass('left')) {
                this.leftCalendar.month.add(1, 'month');
            } else {
                this.rightCalendar.month.add(1, 'month');
                if (this.linkedCalendars)
                    this.leftCalendar.month.add(1, 'month');
            }
            this.updateCalendars();
        },

        hoverDate: function(e) {

            //ignore mouse movements while an above-calendar text input has focus
            //if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
            //    return;

            //ignore dates that can't be selected
            if (!$(e.target).hasClass('available')) return;

            //have the text inputs above calendars reflect the date being hovered over
            var title = $(e.target).attr('data-title');
            var row = title.substr(1, 1);
            var col = title.substr(3, 1);
            var cal = $(e.target).parents('.calendar');
            var date = cal.hasClass('left') ? this.leftCalendar.calendar[row][col] : this.rightCalendar.calendar[row][col];

            if (this.endDate && !this.container.find('input[name=daterangepicker_start]').is(":focus")) {
                this.container.find('input[name=daterangepicker_start]').val(date.format(this.locale.format));
            } else if (!this.endDate && !this.container.find('input[name=daterangepicker_end]').is(":focus")) {
                this.container.find('input[name=daterangepicker_end]').val(date.format(this.locale.format));
            }

            //highlight the dates between the start date and the date being hovered as a potential end date
            var leftCalendar = this.leftCalendar;
            var rightCalendar = this.rightCalendar;
            var startDate = this.startDate;
            if (!this.endDate) {
                this.container.find('.calendar tbody td').each(function(index, el) {

                    //skip week numbers, only look at dates
                    if ($(el).hasClass('week')) return;

                    var title = $(el).attr('data-title');
                    var row = title.substr(1, 1);
                    var col = title.substr(3, 1);
                    var cal = $(el).parents('.calendar');
                    var dt = cal.hasClass('left') ? leftCalendar.calendar[row][col] : rightCalendar.calendar[row][col];

                    if ((dt.isAfter(startDate) && dt.isBefore(date)) || dt.isSame(date, 'day')) {
                        $(el).addClass('in-range');
                    } else {
                        $(el).removeClass('in-range');
                    }

                });
            }

        },

        clickDate: function(e) {

            if (!$(e.target).hasClass('available')) return;

            var title = $(e.target).attr('data-title');
            var row = title.substr(1, 1);
            var col = title.substr(3, 1);
            var cal = $(e.target).parents('.calendar');
            var date = cal.hasClass('left') ? this.leftCalendar.calendar[row][col] : this.rightCalendar.calendar[row][col];

            //
            // this function needs to do a few things:
            // * alternate between selecting a start and end date for the range,
            // * if the time picker is enabled, apply the hour/minute/second from the select boxes to the clicked date
            // * if autoapply is enabled, and an end date was chosen, apply the selection
            // * if single date picker mode, and time picker isn't enabled, apply the selection immediately
            // * if one of the inputs above the calendars was focused, cancel that manual input
            //

            if (this.endDate || date.isBefore(this.startDate, 'day')) { //picking start
                if (this.timePicker) {
                    var hour = parseInt(this.container.find('.left .hourselect').val(), 10);
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.left .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                    var minute = parseInt(this.container.find('.left .minuteselect').val(), 10);
                    var second = this.timePickerSeconds ? parseInt(this.container.find('.left .secondselect').val(), 10) : 0;
                    date = date.clone().hour(hour).minute(minute).second(second);
                }
                this.endDate = null;
                this.setStartDate(date.clone());
            } else if (!this.endDate && date.isBefore(this.startDate)) {
                //special case: clicking the same date for start/end,
                //but the time of the end date is before the start date
                this.setEndDate(this.startDate.clone());
            } else { // picking end
                if (this.timePicker) {
                    var hour = parseInt(this.container.find('.right .hourselect').val(), 10);
                    if (!this.timePicker24Hour) {
                        var ampm = this.container.find('.right .ampmselect').val();
                        if (ampm === 'PM' && hour < 12)
                            hour += 12;
                        if (ampm === 'AM' && hour === 12)
                            hour = 0;
                    }
                    var minute = parseInt(this.container.find('.right .minuteselect').val(), 10);
                    var second = this.timePickerSeconds ? parseInt(this.container.find('.right .secondselect').val(), 10) : 0;
                    date = date.clone().hour(hour).minute(minute).second(second);
                }
                this.setEndDate(date.clone());
                if (this.autoApply) {
                  this.calculateChosenLabel();
                  this.clickApply();
                }
            }

            if (this.singleDatePicker) {
                this.setEndDate(this.startDate);
                if (!this.timePicker)
                    this.clickApply();
            }

            this.updateView();

            //This is to cancel the blur event handler if the mouse was in one of the inputs
            e.stopPropagation();

        },

        calculateChosenLabel: function () {
            var customRange = true;
            var i = 0;
            for (var range in this.ranges) {
                if (this.timePicker) {
                    if (this.startDate.isSame(this.ranges[range][0]) && this.endDate.isSame(this.ranges[range][1])) {
                        customRange = false;
                        this.chosenLabel = this.container.find('.ranges li:eq(' + i + ')').addClass('active').html();
                        break;
                    }
                } else {
                    //ignore times when comparing dates if time picker is not enabled
                    if (this.startDate.format('YYYY-MM-DD') == this.ranges[range][0].format('YYYY-MM-DD') && this.endDate.format('YYYY-MM-DD') == this.ranges[range][1].format('YYYY-MM-DD')) {
                        customRange = false;
                        this.chosenLabel = this.container.find('.ranges li:eq(' + i + ')').addClass('active').html();
                        break;
                    }
                }
                i++;
            }
            if (customRange) {
                if (this.showCustomRangeLabel) {
                    this.chosenLabel = this.container.find('.ranges li:last').addClass('active').html();
                } else {
                    this.chosenLabel = null;
                }
                this.showCalendars();
            }
        },

        clickApply: function(e) {
            this.hide();
            this.element.trigger('apply.daterangepicker', this);
        },

        clickCancel: function(e) {
            this.startDate = this.oldStartDate;
            this.endDate = this.oldEndDate;
            this.hide();
            this.element.trigger('cancel.daterangepicker', this);
        },

        monthOrYearChanged: function(e) {
            var isLeft = $(e.target).closest('.calendar').hasClass('left'),
                leftOrRight = isLeft ? 'left' : 'right',
                cal = this.container.find('.calendar.'+leftOrRight);

            // Month must be Number for new moment versions
            var month = parseInt(cal.find('.monthselect').val(), 10);
            var year = cal.find('.yearselect').val();

            if (!isLeft) {
                if (year < this.startDate.year() || (year == this.startDate.year() && month < this.startDate.month())) {
                    month = this.startDate.month();
                    year = this.startDate.year();
                }
            }

            if (this.minDate) {
                if (year < this.minDate.year() || (year == this.minDate.year() && month < this.minDate.month())) {
                    month = this.minDate.month();
                    year = this.minDate.year();
                }
            }

            if (this.maxDate) {
                if (year > this.maxDate.year() || (year == this.maxDate.year() && month > this.maxDate.month())) {
                    month = this.maxDate.month();
                    year = this.maxDate.year();
                }
            }

            if (isLeft) {
                this.leftCalendar.month.month(month).year(year);
                if (this.linkedCalendars)
                    this.rightCalendar.month = this.leftCalendar.month.clone().add(1, 'month');
            } else {
                this.rightCalendar.month.month(month).year(year);
                if (this.linkedCalendars)
                    this.leftCalendar.month = this.rightCalendar.month.clone().subtract(1, 'month');
            }
            this.updateCalendars();
        },

        timeChanged: function(e) {

            var cal = $(e.target).closest('.calendar'),
                isLeft = cal.hasClass('left');

            var hour = parseInt(cal.find('.hourselect').val(), 10);
            var minute = parseInt(cal.find('.minuteselect').val(), 10);
            var second = this.timePickerSeconds ? parseInt(cal.find('.secondselect').val(), 10) : 0;

            if (!this.timePicker24Hour) {
                var ampm = cal.find('.ampmselect').val();
                if (ampm === 'PM' && hour < 12)
                    hour += 12;
                if (ampm === 'AM' && hour === 12)
                    hour = 0;
            }

            if (isLeft) {
                var start = this.startDate.clone();
                start.hour(hour);
                start.minute(minute);
                start.second(second);
                this.setStartDate(start);
                if (this.singleDatePicker) {
                    this.endDate = this.startDate.clone();
                } else if (this.endDate && this.endDate.format('YYYY-MM-DD') == start.format('YYYY-MM-DD') && this.endDate.isBefore(start)) {
                    this.setEndDate(start.clone());
                }
            } else if (this.endDate) {
                var end = this.endDate.clone();
                end.hour(hour);
                end.minute(minute);
                end.second(second);
                this.setEndDate(end);
            }

            //update the calendars so all clickable dates reflect the new time component
            this.updateCalendars();

            //update the form inputs above the calendars with the new time
            this.updateFormInputs();

            //re-render the time pickers because changing one selection can affect what's enabled in another
            this.renderTimePicker('left');
            this.renderTimePicker('right');

        },

        formInputsChanged: function(e) {
            var isRight = $(e.target).closest('.calendar').hasClass('right');
            var start = moment(this.container.find('input[name="daterangepicker_start"]').val(), this.locale.format);
            var end = moment(this.container.find('input[name="daterangepicker_end"]').val(), this.locale.format);

            if (start.isValid() && end.isValid()) {

                if (isRight && end.isBefore(start))
                    start = end.clone();

                this.setStartDate(start);
                this.setEndDate(end);

                if (isRight) {
                    this.container.find('input[name="daterangepicker_start"]').val(this.startDate.format(this.locale.format));
                } else {
                    this.container.find('input[name="daterangepicker_end"]').val(this.endDate.format(this.locale.format));
                }

            }

            this.updateView();
        },

        formInputsFocused: function(e) {

            // Highlight the focused input
            this.container.find('input[name="daterangepicker_start"], input[name="daterangepicker_end"]').removeClass('active');
            $(e.target).addClass('active');

            // Set the state such that if the user goes back to using a mouse, 
            // the calendars are aware we're selecting the end of the range, not
            // the start. This allows someone to edit the end of a date range without
            // re-selecting the beginning, by clicking on the end date input then
            // using the calendar.
            var isRight = $(e.target).closest('.calendar').hasClass('right');
            if (isRight) {
                this.endDate = null;
                this.setStartDate(this.startDate.clone());
                this.updateView();
            }

        },

        formInputsBlurred: function(e) {

            // this function has one purpose right now: if you tab from the first
            // text input to the second in the UI, the endDate is nulled so that
            // you can click another, but if you tab out without clicking anything
            // or changing the input value, the old endDate should be retained

            if (!this.endDate) {
                var val = this.container.find('input[name="daterangepicker_end"]').val();
                var end = moment(val, this.locale.format);
                if (end.isValid()) {
                    this.setEndDate(end);
                    this.updateView();
                }
            }

        },

        elementChanged: function() {
            if (!this.element.is('input')) return;
            if (!this.element.val().length) return;
            if (this.element.val().length < this.locale.format.length) return;

            var dateString = this.element.val().split(this.locale.separator),
                start = null,
                end = null;

            if (dateString.length === 2) {
                start = moment(dateString[0], this.locale.format);
                end = moment(dateString[1], this.locale.format);
            }

            if (this.singleDatePicker || start === null || end === null) {
                start = moment(this.element.val(), this.locale.format);
                end = start;
            }

            if (!start.isValid() || !end.isValid()) return;

            this.setStartDate(start);
            this.setEndDate(end);
            this.updateView();
        },

        keydown: function(e) {
            //hide on tab or enter
            if ((e.keyCode === 9) || (e.keyCode === 13)) {
                this.hide();
            }
        },

        updateElement: function() {
            if (this.element.is('input') && !this.singleDatePicker && this.autoUpdateInput) {
                this.element.val(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format));
                this.element.trigger('change');
            } else if (this.element.is('input') && this.autoUpdateInput) {
                this.element.val(this.startDate.format(this.locale.format));
                this.element.trigger('change');
            }
        },

        remove: function() {
            this.container.remove();
            this.element.off('.daterangepicker');
            this.element.removeData();
        }

    };

    $.fn.daterangepicker = function(options, callback) {
        this.each(function() {
            var el = $(this);
            if (el.data('daterangepicker'))
                el.data('daterangepicker').remove();
            el.data('daterangepicker', new DateRangePicker(el, options, callback));
        });
        return this;
    };

    return DateRangePicker;

}));


/***/ }),

/***/ 17:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1), __webpack_require__(5)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup) {
    "use strict";
    var REPORT_BY_MONTHLY = 2;
    var LICENSEE_12BET = 1;
    function lastDateOfMonth(year, month) {
        return new Date(year, month + 1, 0);
    }

    var _me = {
        GetDrilldownDateRange: function (reportBy, fromDate, toDate, drilldownDate) {
            var fdate = typeof drilldownDate === "string" ? new Date(drilldownDate) : drilldownDate;
            var tdate = fdate;
            if (Number(reportBy) === REPORT_BY_MONTHLY) {
                var fday = 1;
                var month = fdate.getMonth();
                var year = fdate.getFullYear();
                var orignalFDate = new Date(fromDate);
                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                fdate = new Date(year, month, fday, 0, 0, 0, 0);
                var originalTDate = new Date(toDate);

                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    tdate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    tdate = lastDateOfMonth(year, month);
                }
            }

            return {
                FromDate: fdate,
                ToDate: tdate
            };
        },
        ValidProducts: function () {
            var temp = true;
            if ($(".childCheckbox:checked").length === 0) {
                temp = false;
                var title = Site.getSingleResource("LrfNotification");
                var message = Site.getSingleResource("AlertChooseProductMsg");
                var $dialog = $('<div title="' + title + '"><p class="notification">' + message + '</p></div>');

                Popup.openPopup($dialog, {
                    width: 380,
                    height: 150,
                    title: title,
                    id: "product-invalid",
                    className: "popup-notification",
                    onOpen: function () {
                        $('#SubmitButton').prop('disabled', true);
                    },
                    onClose: function () {
                        $('#SubmitButton').removeAttr('disabled');
                    }
                });
            }

            return temp;
        },

        setOriginalProduct: function () {
            $(".childCheckbox[type='checkbox']", "#ProductList").each(function (index, element) {
                var isCheck = $(this).is(":checked");
                $(element).attr({ 'data-original': isCheck });
            });
        },

        bindSummaryTotal: function (columnModel, summaryTotal) {
            if (!summaryTotal || summaryTotal.length === 0) {
                return;
            }

            var mainFooter = $(".ui-jqgrid-ftable tr:last-child");
            var rowCount = 0;
            var currencyList = [];

            function processColumnModel(dataItem) {
                var row = mainFooter.clone();
                var cellList = $("td", row);

                $(columnModel).each(function (cellIndex, modelElement) {
                    var isCustomFormater = typeof modelElement.formatter === "function";
                    var rawValue = dataItem[modelElement.name];
                    var value = isCustomFormater ? modelElement.formatter(rawValue, modelElement.formatoptions, row) :
                                $.fn.fmatter(modelElement.formatter, rawValue, modelElement.formatoptions);

                    var $cell = $(cellList.get(cellIndex));
                    if (cellIndex === 0) {
                        var content = "Total in " + currency;
                        $cell.html(content)
                            .addClass("group-" + currency)
                            .attr("title", content);
                    }
                    else {
                        var title = $.isNumeric(rawValue) && rawValue < 0 ? undefined : value;
                        $cell.html(value).attr("title", title);
                    }
                });

                row.insertAfter(mainFooter);
                rowCount++;
            }

            for (var currency in summaryTotal) {
                currencyList.push(currency);
                var summaryData = summaryTotal[currency];
                if (Array.isArray(summaryData)) {
                    summaryData.forEach(processColumnModel);
                }
                else {
                    processColumnModel(summaryData);
                }
            }

            $(".ui-jqgrid-ftable").attr("data-total-currency-list", currencyList.join());
            mainFooter.remove();

            return rowCount;
        },

        getCurrencyNameForReportTitle: function (currencyId) {
            var viewinKey = Site.getSingleResource("Viewin");
            var currencyTextList = {
                "-3": viewinKey.replace("{0}", "GBP"),
                "-2": viewinKey.replace("{0}", "USD"),
                "-1": viewinKey.replace("{0}", "EUR"),
                "0": Site.getSingleResource('AllCurrencies')
            };

            return currencyTextList[currencyId] === undefined
                ? viewinKey.replace("{0}", $('#CurrencyId option[value="' + currencyId + '"]').text())
                : currencyTextList[currencyId];
        },

        renderBreadCrumb: function (level, licenseeId) {
            level = Number(level);
            if (level === 0) {
                $(".breadCrumbHolder").hide();
            }
            else {
                $(".breadCrumbHolder").show();
                $("#level-link-container .level0").show();

                var isLevel2 = level === 2;
                $("#level-link-container .level2").toggle(isLevel2).toggleClass("active", isLevel2);

                var allowShowLevel1 = licenseeId ? licenseeId === LICENSEE_12BET : true;
                $("#level-link-container .level1").toggle(allowShowLevel1);
                $("#level-link-container .level1").toggleClass("active", allowShowLevel1 && !isLevel2);
            }
        }
    };

    return _me;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 18:
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function(jQuery) {﻿/// <reference path="/@JSense.js" />
(function ($) {
    var currentTriggerEvents = [];
    var methods = {
        createMultiselect: function (opts) {
            opts = $.extend({}, $.fn.multiSelect.defaults, opts);
            return this.each(function () {
                return new MultiSelect(this, opts);
            });
        },
        appendItem: function (item) {
        },
        removeItem: function (item) {
        },
        updateItems: function (opts) {
            opts = $.extend({}, $.fn.multiSelect.defaults, opts);
            return this.each(function () {
                return new MultiSelect(this, opts);
            });
        },
        checkAllItems: function () {
            var $inputs = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').find('input');
            $inputs.not(':disabled').attr('checked', 'checked');
            var numChecked = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark').find('input').not("#" + $(this).attr('id') + "HeaderCheckBox").filter('[checked]').length;
            if (o.isUsedCustomCheckAllText) {
                $(this).find('input').val(o.customCheckAllText);
            }
            else {
                $(this).find('input').val(numChecked + " selected");
            }
        },
        unCheckAllItems: function () {
            var $inputs = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').find('input');
            $inputs.not(':disabled').removeAttr('checked');
            var numChecked = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').find('input').filter('[checked]').length;

            $(this).find('input').val($.fn.multiSelect.defaults.noneSelectedText);
        },
        getUncheckItems: function () {
            var listChecked = $(this).next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(:checked)').not("#" + $(this).attr('id') + "HeaderCheckBox");
            var tempArray = [];
            for (var i = 0; i < listChecked.length; i++) {
                tempArray.push(listChecked[i].value);
            }
            return tempArray;
        },
        getCheckedItems: function () {
            var listChecked = $(this).next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $(this).attr('id') + "HeaderCheckBox)").filter('[checked]');
            var tempArray = [];
            for (var i = 0; i < listChecked.length; i++) {
                tempArray.push(listChecked[i].value);
            }
            return tempArray;
        },
        getCheckedItemsByText: function () {
            var listChecked = $(this).next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $(this).attr('id') + "HeaderCheckBox)").filter('[checked]');
            var tempArray = [];
            for (var i = 0; i < listChecked.length; i++) {
                tempArray.push(listChecked[i].title);
            }
            return tempArray;
        },
        hideMe: function () {
            $(this).next('div.ui-multiselect-options').trigger('close', true, false);
        },
        openMe: function (waiting) {
            //$(this).find('#ui-multiselect-options-' + $(this).find(":first-child").attr("id")).trigger('open', false);
            $(this).next('div.ui-multiselect-options').trigger('open', false);
        }
    };

    $.fn.multiSelect = function (opts) {
        if (methods[opts]) {
            return methods[opts].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof opts === 'object' || !opts) {
            // Default to "createMultiselect"
            return methods.createMultiselect.apply(this, arguments);
        }
        else {
            //throw error
        }
    };
    //// Detect Browser
    //// Grid Scroller
    var matchedBrowser, browserDectect;

    // Use of jQuery.browser is frowned upon.
    // More details: http://api.jquery.com/jQuery.browser
    // jQuery.uaMatch maintained for back-compat
    function ubMatch(ua) {
        ua = ua.toLowerCase();

        var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
        /(webkit)[ \/]([\w.]+)/.exec(ua) ||
        /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
        /(msie) ([\w.]+)/.exec(ua) ||
        ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
        [];

        return {
            browserDectect: match[1] || "",
            version: match[2] || "0"
        };
    };

    matchedBrowser = ubMatch(navigator.userAgent);
    browserDectect = {};

    if (matchedBrowser.browserDectect) {
        browserDectect[matchedBrowser.browserDectect] = true;
        browserDectect.version = matchedBrowser.version;
    }

    // Chrome is Webkit, but Webkit is also Safari.
    if (browserDectect.chrome) {
        browserDectect.webkit = true;
    } else if (browserDectect.webkit) {
        browserDectect.safari = true;
    }

    // counter to dynamically generate label/option IDs if they don't exist
    var multiselectID = 0;
    // built up a multiselect
    var MultiSelect = function (select, o) {
        //global variables
        var $original = $(select);
        var $select = $original,
            $options, $header, $labels, $footer, $listGroupCheckBox, $body, $listItem, $listCollapse, $divHover,
            $allLabel, $inputHeaderCheckBox, $selectinput, $headerCheckbox, $checkedInput, $dropdownlist,
            html = [],
            optgroups = [],
            isDisabled = $select.is(':disabled'),
            id = select.id || 'ui-multiselect-' + multiselectID++, // unique ID for the label & option tags
            listData = o.listData,
            isShowGroup = false,
            isOpenning = false,
            numGroup = 0;
        //render MultiSleclect Interface
        var RenderInterface = function (id, idDisable, noneSelectedText, title) {
            //remove old
            if ($select.prev().is('div.ui-multiselect-options')) {
                $select.prev().remove();
            }
            if ($select.next().is('div.ui-multiselect-options')) {
                $select.next().remove();
            }

            //render new
            var tempHtml = [];
            tempHtml.push('<a id="' + id + '" class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default ' + (idDisable ? ' ui-multiselect-state-disabled' : '') + '">');
            tempHtml.push('<input readonly="readonly" type="text" class="ui-multiselect-state-default" value="' + noneSelectedText + '" title="' + select.title + '" /><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
            return tempHtml.join('');
        };
        //render Header
        var checkShouldShowGroupOrNo = function () {
            for (var i = 0; i < o.listData.length; i++) {
                if (typeof o.listData[i].Items !== 'undefined' && o.listData[i].Items.length > 0) {
                    isShowGroup = true;
                    break;
                }
            }
        }
        var RenderHeader = function (id, checkAllText) {
            var tempHtml = [];
            //html.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-corner-all ui-multiselect-header">');
            //html.push('<ul class="ui-multiselect-helper-reset ui-multiselect-checkboxes" style="overflow-y:hidden;">');
            tempHtml.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-header custom-ui-widget-header">');
            tempHtml.push('<ul class="ui-multiselect-helper-reset ui-multiselect-checkboxes-head" style="overflow-y:hidden;">');
            tempHtml.push('<li style="display:inline;">');
            tempHtml.push('<label class="ui-multiselect-corner-all" style="color: #000;" for="' + id + "HeaderCheckBox" + '">');
            tempHtml.push('<input type="checkbox" id="' + id + "HeaderCheckBox" + '">' + checkAllText + '</label></li>');
            if (isShowGroup) {
                tempHtml.push('<li class="ui-multiselect-icon-dropdown ui-multiselect-icon-collapse" title="Collapse/Expand" style="display:inline; float:right; cursor:pointer;" id="' + id + "CollapseButton" + '" >&nbsp;</li>');
            }
            //tempHtml.push('</li>');
            tempHtml.push('</ul>');
            tempHtml.push('</div>');
            return tempHtml.join('');
        };
        var RenderNoItemHeader = function (id, noItemText) {
            var tempHtml = [];
            tempHtml.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-corner-all ui-multiselect-header custom-ui-widget-header">');
            tempHtml.push('<ul class="ui-multiselect-helper-reset ui-multiselect-checkboxes-head" style="overflow-y:hidden;">');
            tempHtml.push('<li style="display:inline;">');
            tempHtml.push('<label class="ui-multiselect-corner-all" style="color: #000000" >' + noItemText + '</label>');
            tempHtml.push('</ul>');
            tempHtml.push('</div>');
            return tempHtml.join('');
        };
        //render Body
        var InsertRow = function (tempData, index, isChild, isLastChildItem, isNoGroup, siblingGroup, currentParentGroupId, currentGroupId) {
            var tempHtml = [];
            var title = tempData.Text || "",
            value = tempData.Value || "",
            inputID = tempData.Id || "ui-multiselect-" + id + "-option-" + index,
            isDisabled = !tempData.Enable,
            labelClasses = tempData.CssClass || "",
            currentGroupStatus = (tempData.Show == false ? false : true),
            liClasses = "",
            divClasses = "ui-multiselect-div ui-multiselect-corner-all",
            divHtml = "";
            var groupIdAttr = "ui-multiselect-option-group-" + currentGroupId;
            if (typeof value != 'undefined') {
                if (isDisabled) {
                    labelClasses += ' ui-multiselect-state-disabled';
                }
                if (typeof tempData.Items !== 'undefined' && (typeof isChild === 'undefined' || isChild === false) && siblingGroup) {
                    labelClasses += ' ui-multiselect-group-mark';
                }

                liClasses += (isNoGroup == false ? 'rtLI ' : ' rtLI');
                liClasses += (isDisabled ? ' ui-multiselect-disabled ' : '');
                liClasses += (isLastChildItem == true ? " rtLast" : "");
                liClasses += (index === 0 && isNoGroup == false) ? " rtFirst" : "";

                if (!isShowGroup && o.columnNumber > 0) {
                    var style = "float: left; width:" + (100 / o.columnNumber).toFixed(0) + "%;";
                    tempHtml.push('<li class="' + liClasses + '" style="' + style + '">');
                }
                else {
                    tempHtml.push('<li class="' + liClasses + '">');
                }

                if (siblingGroup == false) {
                    if (isNoGroup == true && isShowGroup) {
                        divClasses += (isShowGroup == false ? "" : ((isLastChildItem == true) ? ' rtBot' : ' rtMid'));
                        tempHtml.push('<div class="' + divClasses + '">');
                    }
                    else {
                        tempHtml.push('<div class="' + divClasses + '" style="margin-left:10px;">');
                    }
                }
                else {
                    divClasses += (index === 0) ? " rtMid" : " rtMid rtSelected";
                    divClasses += (isLastChildItem === true ? " rtBot" : "");
                    tempHtml.push('<div class="' + divClasses + '">');
                    if (tempData.Items != null && tempData.Items.length !== 0) {
                        tempHtml.push('<span class="collapseClass rtMinus" showstatus="' + currentGroupStatus + '"></span>');
                    }
                }

                labelClasses += " ui-multiselect-corner-all";
                tempHtml.push('<label for="' + inputID + '" class="' + labelClasses + '" style="' + tempData.CssClass + '">');
                tempHtml.push('<input id="' + inputID + '" type="' + (o.multiple ? 'checkbox' : 'radio') + '" name="' + value + '" value="' + value + '" title="' + title + '" childgroupid="' + currentParentGroupId + '"');

                if (o.isCheckAll) {
                    tempHtml.push(' checked="checked"');
                }
                else if (tempData.Checked) {
                    tempHtml.push(' checked="checked"');
                }
                if (isDisabled) {
                    tempHtml.push(' disabled="disabled"');
                }
                tempHtml.push(' />' + title + '</label>');
                tempHtml.push('</div>');

                if (typeof tempData.Items !== 'undefined' && tempData.Items.length > 0) {
                    //var nextGroupIdAtt = "ui-multiselect-option-group-" + currentGroupId + "-" + index.toString();
                    tempHtml.push('<ul class="ui-multiselect-ul-group custom-multi-select-rtUL" childgroupid="' + groupIdAttr + '">');
                    for (var j = 0; j < tempData.Items.length; j++) {
                        if (typeof tempData.Items[j].Items === 'undefined'
                        || tempData.Items[j].Items == null
                        || tempData.Items[j].Items.length === 0) {
                            tempHtml.push(InsertRow(tempData.Items[j], j,
                                true, (j === tempData.Items.length - 1 ? true : false), true, siblingGroup, groupIdAttr, currentGroupId));
                        }
                        else {
                            currentGroupId += "-" + j.toString();
                            tempHtml.push(InsertRow(tempData.Items[j], j,
                                false, (j === tempData.Items.length - 1 ? true : false), false, siblingGroup, groupIdAttr, currentGroupId));
                        }

                        //tempHtml.push(InsertRow(tempData.Items[j], j, true, (j === tempData.Items.length - 1 ? true : false), false, false));
                    }
                    tempHtml.push("</ul>");
                }

                tempHtml.push('</li>');
            }
            return tempHtml.join('');
        };
        var RenderBody = function () {
            var tempHtml = [];
            var groupIdAttr = "ui-multiselect-option-group-0";
            tempHtml.push('<ul class="ui-multiselect-checkboxes ui-multiselect-helper-reset rtUL ' + (isShowGroup ? "rtLines" : "") + ' ui-multiselect-ul-group" childgroupid="' + groupIdAttr + '">');
            for (var i = 0; i < listData.length; i++) {
                var tempGroup = listData[i];
                var isChild = tempGroup.Items == null || tempGroup.Items.length === 0;
                var isLastChildItem = (i === listData.length - 1 ? true : false);
                var isNoGroup = (typeof tempGroup.Items === 'undefined' || (typeof tempGroup.Items !== 'undefined' && tempGroup.Items.length === 0)) ? true : false;
                var siblingIsGroup = isShowGroup ? true : false;
                var currentGroupId = "0-" + i.toString();
                numGroup++;
                tempHtml.push(InsertRow(tempGroup, i, isChild, isLastChildItem, isNoGroup, siblingIsGroup, groupIdAttr, currentGroupId));
            }
            tempHtml.push('</ul>');
            return tempHtml.join('');
        };
        //render Footer
        var RenderFooter = function () {
            var tempHtml = [];
            tempHtml.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-corner-all ui-multiselect-footer custom-ui-widget-header">');
            tempHtml.push('<a class="ui-multiselect-update-result" href="">' + o.updateResult + '</a>');
            tempHtml.push('</div>');
            return tempHtml.join('');
        };
        //add property value
        var AddPropertyValue = function () {
            // calculate widths
            var iconWidth = $select.find('span.ui-multiselect-icon').width();
            var inputWidth = o.minWidth - iconWidth + 3;

            // set widths
            $select.width(o.minWidth).find('input').width(inputWidth);
        };
        // binding event
        var checkCheckAll = function () {
            if (numGroup == $options.find('input[childgroupid="ui-multiselect-option-group-0"]').filter('[checked]').length) {
                $headerCheckbox.prop('checked', true);
                $headerCheckbox.attr('checked', 'checked');
                $listGroupCheckBox.prop('checked', true);
                $listGroupCheckBox.attr('checked', 'checked');
            }
        };
        var UpdateSelected = function () {
            var $inputs = $inputHeaderCheckBox,
                    $checked = $inputs.filter('[checked]'),
                    value = '',
                    numChecked = $checked.length;

            if (numChecked === 0) {
                value = o.noneSelectedText;
            } else {
                if (o.isUsedCustomCheckAllText && $inputs.length === numChecked) {
                    value = o.customCheckAllText;
                }
                else {
                    if (isShowGroup == false) {
                        var arrayItemChecked = [];
                        for (var i = 0; i < numChecked; i++) {
                            arrayItemChecked.push($checked[i].title);
                        }
                        value = arrayItemChecked.join(', ');
                        if (value.length * 8 > o.minWidth) {
                            value = o.selectedText.replace('#', numChecked).replace('#', $inputs.length);
                        }
                    }
                    else {
                        value = o.selectedText.replace('#', numChecked).replace('#', $inputs.length);
                    }
                }
            }

            // Update tooltip
            if (o.showTooltip) {
                var tooltipValue = $checked.map(function () { return this.title; }).get().join(', ');
                if (tooltipValue == '') {
                    tooltipValue = o.tooltipNoItem;
                }
                $select.attr("original-title", tooltipValue);
            }

            $select.find('input').val(value);
            return value;
        };

        var UpdateSelectedFromChild = function (currentGroup) {
            var parentGroup = currentGroup.parent().parent();
            if (parentGroup.prop("tagName").toLowerCase() == 'ul' && parentGroup.length > 0) {
                var currentChildren = parentGroup.find("li > div > label > input");
                var currentChildSelected = currentChildren.filter('[checked]').length;
                if (currentChildren.length == currentChildSelected) {
                    parentGroup.prev().find('input').prop('checked', true);
                    parentGroup.prev().find('input').attr('checked', 'checked');
                }
                UpdateSelectedFromChild(parentGroup);
            }
        };

        var UpdateRemoveParentSelected = function (currentGroup) {
            if (currentGroup.hasClass('ui-multiselect-ul-group') && currentGroup.length > 0) {
                if (currentGroup.prev().find('input').prop('checked') == true) {//true) {
                    currentGroup.prev().find('input').prop('checked', false);
                    currentGroup.prev().find('input').removeAttr('checked');
                    $headerCheckbox.prop('checked', false);
                    $headerCheckbox.removeAttr('checked');
                }
                UpdateRemoveParentSelected(currentGroup.parent().parent());
            }
        };

        var BindingEvent = function () {
            // build header links
            if (o.showHeader) {
                $header.find('input').click(function (e) {
                    var $this = $(this);
                    if ($this.prop('checked') == true) {
                        $this.attr('checked', 'checked');
                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').prop('checked', true);
                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').attr('checked', 'checked');
                    }
                    else {
                        $this.removeAttr('checked');
                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').prop('checked', false);
                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').removeAttr('checked');
                    }
                    UpdateSelected();
                });
            }
            //build footer link
            if (o.showFooter) {
                $footer.find('a').click(function (e) {
                    o.onUpdateResult.call(this);
                    e.preventDefault();
                });
            }

            if (o.showTooltip) {
                try {
                    $select.tipsy(
                        {
                            html: true,
                            opacity: 0.9,
                            gravity: 'sw'
                        }
                    );
                }
                catch (e) { };
            }
            //add handler event collapse
            $listCollapse.each(function () {
                var $this = $(this);
                $this.click(function () {
                    if ($(this).hasClass('rtMinus')) {
                        //alert("aaa");
                        $(this).removeClass('rtMinus').addClass('rtPlus');
                    }
                    else {
                        $(this).removeClass('rtPlus').addClass('rtMinus');
                    }
                    $(this).parent().next().toggle();
                });

                var currentStatus = $this.attr('showstatus');
                if (currentStatus == 'false') {
                    $this.removeClass('rtMinus').addClass('rtPlus');
                    $this.parent().next().hide();
                    //$this.trigger('click');
                }
            });
            // the select box events
            $select.bind({
                click: function () {
                    $options.trigger('toggle');
                },
                keypress: function (e) {
                    switch (e.keyCode) {
                        case 27: // esc
                        case 38: // up
                            $options.trigger('close');
                            break;
                        case 40: // down
                        case 0: // space
                            $options.trigger('toggle');
                            break;
                    }
                },
                mouseenter: function () {
                    if (!$select.hasClass('ui-multiselect-state-disabled')) {
                        $(this).addClass('ui-multiselect-state-hover');
                    };
                },
                mouseleave: function () {
                    $(this).removeClass('ui-multiselect-state-hover');
                },
                focus: function () {
                    if (!$select.hasClass('ui-multiselect-state-disabled')) {
                        $(this).addClass('ui-multiselect-state-focus');
                    }
                },
                blur: function () {
                    $(this).removeClass('ui-multiselect-state-focus');
                }
            });
            // bind custom events to the options div
            $options.bind({
                'close': function (e, others, isRetrieveData, targetId) {
                    if ($options.css('display') != 'none') {
                        others = others || false;

                        // hides all other options but the one clicked
                        if (others === true) {
                            $('div.ui-multiselect-options')
                                .filter(':visible')
                                .fadeOut(o.fadeSpeed)
                                .prev('a.ui-multiselect')
                                .removeClass('ui-multiselect-state-active')
                                .trigger('mouseout');

                            // hides the clicked options
                        } else {
                            $select.removeClass('ui-multiselect-state-active').trigger('mouseout');
                            $options.fadeOut(o.fadeSpeed);
                        }
                        if (o.isRetrieveDataAfterClose) {
                            isRetrieveData = isRetrieveData || false;
                            if (isRetrieveData) {
                                $allLabel.find('input[ischeck="true"]').attr('checked', 'checked');
                                if (isOpenning == true) {
                                    $allLabel.find('input[ischeck!="true"]').removeAttr('checked');
                                }
                            }
                            $allLabel.find('input').removeAttr('isCheck');
                            isOpenning = false;
                        }
                        UpdateSelected();
                        //$.fn.multiSelect.defaults.onLeave();
                        if (typeof (targetId) != "undefined" && (targetId != $select.attr("id") || targetId == 'html')) {
                            o.onLeave.call($select);
                        }
                    }
                },
                'open': function (e, closeOthers) {
                    // bail if this widget is disabled
                    if ($select.hasClass('ui-multiselect-state-disabled')) {
                        return;
                    }

                    //currentTriggerEvents.push(e.)

                    // use position() if inside ui-widget-content, because offset() won't cut it.
                    var offset = $select.position(),
                            $container = $options.find('ul:last'),
                            top, width;

                    // calling select is active
                    $select.addClass('ui-multiselect-state-active');

                    // hide all other options
                    if (closeOthers || typeof closeOthers === 'undefined') {
                        //$options.trigger('close', [true, true]);
                        $('div.ui-multiselect-options').trigger('close', [true, true]);
                    }

                    // calculate positioning
                    if (o.position === 'middle') {
                        top = (offset.top + ($select.height() / 2) - ($options.outerHeight() / 2));
                    } else if (o.position === 'top') {
                        top = (offset.top - $options.outerHeight());
                    } else {
                        top = (offset.top + $select.outerHeight());
                    }
                    top += 3; // padding extra 3px to seperate options with multi select div

                    // calculate the width of the options menu
                    if (browserDectect.msie) {
                        width = $select.width() - 2 - parseInt($options.css('padding-left'), 10) - parseInt($options.css('padding-right'), 10);
                    } else {
                        width = $select.width() + 2 - parseInt($options.css('padding-left'), 10) - parseInt($options.css('padding-right'), 10);
                    }

                    // select the first option
                    //$labels.filter('label:first').trigger('mouseenter').trigger('focus');

                    // show the options div + position it
                    $options.css({
                        position: 'absolute',
                        top: top + 'px',
                        left: offset.left + 'px',
                        width: width + 'px'
                    }).show();

                    // set the scroll of the checkbox container
                    $container.scrollTop(0);

                    // set the height of the checkbox container

                    if (o.maxHeight) {
                        if (o.maxHeight <= $body.height()) {
                            $body.css('max-height', o.maxHeight);
                        }
                    }
                    if (o.isRetrieveDataAfterClose) {
                        $allLabel.find('input').filter('[checked]').attr("ischeck", "true");
                        isOpenning = true;
                    }
                    //o.onOpen.call($options[0]);
                },
                'toggle': function () {
                    var isHide = $(this).is(':hidden');
                    if (isHide) {
                        $options.trigger('open');
                    }
                    else {
                        $options.trigger('close', [true, true]);
                    }
                }
            });
            // checkbox click event handle
            $listItem.each(function () {
                var $this = $(this);
                //handle group checkbox click event
                $this.prev().children().not('span.collapseClass').find('input').click(function () {
                    if ($(this).prop('checked') == true) {
                        $(this).attr('checked', 'checked');
                        $(this).parent().parent().next().find('input:not(:disabled)').prop('checked', true);
                        UpdateSelectedFromChild($this);
                        checkCheckAll();
                    }
                    else {
                        $(this).parent().parent().next().find('input:not(:disabled)').removeAttr('checked');
                        $(this).parent().parent().next().find('input:not(:disabled)').prop('checked', false);
                        $headerCheckbox.removeAttr('checked');
                        $headerCheckbox.prop('checked', false);
                    }
                    o.onCheck.call(this);
                    UpdateSelected();
                });

                //handler chilren checkbox event
                var childrenNodes = $this.find('input[childgroupid="' + $this.attr("childgroupid") + '"]');
                childrenNodes.click(function () {
                    var parent = $this;
                    var current = $(this);
                    if (current.prop('checked') == false) {
                        current.removeAttr('checked');
                        UpdateRemoveParentSelected(parent);
                    }
                    else if (current.prop('checked') == true) {
                        current.attr('checked', 'checked');
                        var currentGroup = parent;
                        var currentChildSelected = childrenNodes.filter('[checked]').length;
                        var currentChildLength = childrenNodes.length;
                        if (currentChildLength == currentChildSelected) {
                            currentGroup.prev().find('input').prop('checked', true);
                            currentGroup.prev().find('input').attr('checked', 'checked');
                            UpdateSelectedFromChild(current.parent().parent().parent().parent());
                        }

                        //                        if (currentGroup.find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark').find('input').not("#" + currentGroup.attr('id') + "HeaderCheckBox").filter('[checked]').length
                        //                                === currentGroup.find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark').find('input').not("#" + currentGroup.attr('id') + "HeaderCheckBox").length) {
                        //                            currentGroup.prev().find('input').attr('checked', 'checked');
                        //                        }
                        //                        if (parent.find('input:not(:disabled)').length === (parent.find('input[checked]').length)) {
                        //                            parent.prev().find('input').attr('checked', 'checked');
                        //                        }

                        checkCheckAll();
                    }
                    o.onCheck.call(this);
                    UpdateSelected();
                });
            });
            //handle collapse button
            $('#' + id + "CollapseButton").click(function () {
                if ($(this).hasClass("ui-multiselect-icon-collapse")) {
                    $(this).removeClass("ui-multiselect-icon-collapse").addClass("ui-multiselect-icon-expand");
                    $options.find('ul.ui-multiselect-ul-group:not(.ui-multiselect-checkboxes):visible').hide();
                    $options.find('span.collapseClass').removeClass('rtMinus').addClass('rtPlus');;
                    //$(this).text('Expande All');
                }
                else {
                    $options.find('ul.ui-multiselect-ul-group:not(.ui-multiselect-checkboxes):hidden').show();
                    $options.find('span.collapseClass').removeClass('rtPlus').addClass('rtMinus');
                    $(this).removeClass("ui-multiselect-icon-expand").addClass("ui-multiselect-icon-collapse");
                }
            });
        };
        //initial setup
        var InitialSetup = function () {
            if (o.isCheckAll === true) {
                $headerCheckbox.attr('checked', 'checked');
            }
            // open by default?
            if (o.state === 'open') {
                $options.trigger('open', [false]);
            }

            // update the number of selected elements when the page initially loads, and use that as the defaultValue.  necessary for form resets when options are pre-selected.
            $select.find('input')[0].defaultValue = UpdateSelected();
            //$options.trigger('closed', [true]);
            //            $('div.ui-multiselect-options').trigger('close', [true]);
            $options.hide();
        };

        //Render All
        (function () {
            checkShouldShowGroupOrNo();
            html.push(RenderInterface(id, isDisabled || o.disabled, o.noneSelectedText));
            html.push('<div class="ui-multiselect-options RadTreeView RadTreeView_Office2010Silver' + (o.shadow ? ' ui-multiselect-shadow' : '') + ' ui-multiselect-widget ui-multiselect-widget-content ui-multiselect-corner-all" >');
            if (o.listData.length != 0) {
                html.push(RenderHeader(id, o.checkAllText));
                html.push(RenderBody());
            }
            else {
                html.push(RenderNoItemHeader(id, o.noItemText));
            }
            if (o.showFooter == true) {
                html.push(RenderFooter());
            }
            html.push('</div>');

            //add global variable

            $select = $select.after(html.join('')).next('a.ui-multiselect');
            $options = $select.next('div.ui-multiselect-options');
            $options.attr('id', 'ui-multiselect-options-' + id);
            $options = $('#ui-multiselect-options-' + id);
            $header = $options.find('div.ui-multiselect-header');
            $body = $options.find('ul.ui-multiselect-checkboxes');
            $footer = $options.find('div.ui-multiselect-footer');
            $labels = $options.find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark');
            $allLabel = $options.find('label');
            $listItem = $options.find('ul.ui-multiselect-ul-group');
            $listCollapse = $options.find('span.collapseClass');
            $listGroupCheckBox = $options.find('label.ui-multiselect-group-mark').find('input');
            $divHover = $options.find('div.ui-multiselect-div');

            AddPropertyValue();
            BindingEvent();

            $inputHeaderCheckBox = $labels.find('input').not("#" + id + "HeaderCheckBox");
            $selectinput = $select.find('input');
            $headerCheckbox = $options.find("#" + id + "HeaderCheckBox");
            $checkedInput = $labels.find('input[checked]');

            InitialSetup();
            // remove the original input element

            $original.remove();
        })();
        return $select;
    };
    // close each select when clicking on any other element/anywhere else on the page
    $(document).delegate('html', 'click', function (e, isInsideCall) {
        var $target = $(e.target);
        if (!$target.closest('div.ui-multiselect-options').length && !$target.parent().hasClass('ui-multiselect')) {
            if (!$target.hasClass('exeption-multiselect-click')) {
                $('div.ui-multiselect-options').trigger('close', [true, true, ($target.attr("id") || 'html')]);
            }
        }
    });
    // default options
    $.fn.multiSelect.defaults = {
        showHeader: true,
        showFooter: true,
        showTooltip: false,
        maxHeight: 175, /* max height of the checkbox container (scroll) in pixels */
        minWidth: 215, /* min width of the entire widget in pixels. setting to 'auto' will disable */
        checkAllText: 'Check All',
        noItemText: 'No item found.',
        unCheckAllText: 'Uncheck All',
        updateResult: 'Update Result Table',
        noneSelectedText: 'Select options',
        selectedText: '# selected',
        tooltipNoItem: 'There is no selected item.',
        selectedList: 0,
        position: 'bottom', /* top|middle|bottom */
        shadow: false,
        fadeSpeed: 100,
        disabled: false,
        state: 'closed',
        multiple: true,
        isCheckAll: false,
        hasOptGroup: false,
        isRetrieveDataAfterClose: false,
        listData: [],
        buttons: [],
        isUsedCustomCheckAllText: false,
        customCheckAllText: '',
        columnNumber: 1,
        onLeave: function () { },
        onCheck: function () { }, /* when an individual checkbox is clicked */
        onOpen: function () { }, /* when the select menu is opened */
        onCheckAll: function () { }, /* when the check all link is clicked */
        onUncheckAll: function () { }, /* when the uncheck all link is clicked */
        onOptgroupToggle: function () { }, /* when the optgroup heading is clicked */
        onUpdateResult: function () { }
    };
})(jQuery);
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(0)))

/***/ }),

/***/ 19:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1), __webpack_require__(18)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
    var _me = {
        initProductList: function () {
            var selector = "#grid-products-multiselect";
            var data = $.parseJSON($("#productsData").val());
            var length = data.length;
            var dataUI = [];
            var checkedCount = 0;
            for (var i = 0; i < length; i++) {
                var item = data[i];
                var isChecked = item["Status"];
                if (isChecked) {
                    checkedCount++;
                }

                dataUI.push({
                    Id: item["Code"],
                    Value: item["Code"],
                    Text: Site.getSingleResource(item["Name"]),
                    Checked: isChecked,
                    Enable: true
                });
            }

            var isCheckAll = checkedCount === length;
            $(selector).multiSelect({
                minWidth: 200,
                maxHeight: 400,
                shadow: true,
                showHeader: true,
                isCheckAll: isCheckAll,
                checkAllText: Site.getSingleResource('All'),
                showFooter: false,
                listData: dataUI,
                onCheck: function () {
                    var checkedItems = $(selector).multiSelect("getCheckedItems");
                    $("#product-container input[type=checkbox]:checked").prop("checked", false);
                    var hasItemsChecked = checkedItems.length > 0;
                    if (hasItemsChecked) {
                        for (var i = 0; i < checkedItems.length; i++) {
                            $('#product-container #' + checkedItems[i]).prop("checked", true);
                        }
                    }
                    
                    $("#ProductList").toggleClass("invalid", !hasItemsChecked);
                    $("#SubmitButton").prop("disabled", !hasItemsChecked);
                }
            });
        }
    };

    return _me;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 2:
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function(module) {var require;//! moment.js
//! version : 2.20.1
//! authors : Tim Wood, Iskren Chernev, Moment.js contributors
//! license : MIT
//! momentjs.com

;(function (global, factory) {
     true ? module.exports = factory() :
    typeof define === 'function' && define.amd ? define(factory) :
    global.moment = factory()
}(this, (function () { 'use strict';

var hookCallback;

function hooks () {
    return hookCallback.apply(null, arguments);
}

// This is done to register the method called with moment()
// without creating circular dependencies.
function setHookCallback (callback) {
    hookCallback = callback;
}

function isArray(input) {
    return input instanceof Array || Object.prototype.toString.call(input) === '[object Array]';
}

function isObject(input) {
    // IE8 will treat undefined and null as object if it wasn't for
    // input != null
    return input != null && Object.prototype.toString.call(input) === '[object Object]';
}

function isObjectEmpty(obj) {
    if (Object.getOwnPropertyNames) {
        return (Object.getOwnPropertyNames(obj).length === 0);
    } else {
        var k;
        for (k in obj) {
            if (obj.hasOwnProperty(k)) {
                return false;
            }
        }
        return true;
    }
}

function isUndefined(input) {
    return input === void 0;
}

function isNumber(input) {
    return typeof input === 'number' || Object.prototype.toString.call(input) === '[object Number]';
}

function isDate(input) {
    return input instanceof Date || Object.prototype.toString.call(input) === '[object Date]';
}

function map(arr, fn) {
    var res = [], i;
    for (i = 0; i < arr.length; ++i) {
        res.push(fn(arr[i], i));
    }
    return res;
}

function hasOwnProp(a, b) {
    return Object.prototype.hasOwnProperty.call(a, b);
}

function extend(a, b) {
    for (var i in b) {
        if (hasOwnProp(b, i)) {
            a[i] = b[i];
        }
    }

    if (hasOwnProp(b, 'toString')) {
        a.toString = b.toString;
    }

    if (hasOwnProp(b, 'valueOf')) {
        a.valueOf = b.valueOf;
    }

    return a;
}

function createUTC (input, format, locale, strict) {
    return createLocalOrUTC(input, format, locale, strict, true).utc();
}

function defaultParsingFlags() {
    // We need to deep clone this object.
    return {
        empty           : false,
        unusedTokens    : [],
        unusedInput     : [],
        overflow        : -2,
        charsLeftOver   : 0,
        nullInput       : false,
        invalidMonth    : null,
        invalidFormat   : false,
        userInvalidated : false,
        iso             : false,
        parsedDateParts : [],
        meridiem        : null,
        rfc2822         : false,
        weekdayMismatch : false
    };
}

function getParsingFlags(m) {
    if (m._pf == null) {
        m._pf = defaultParsingFlags();
    }
    return m._pf;
}

var some;
if (Array.prototype.some) {
    some = Array.prototype.some;
} else {
    some = function (fun) {
        var t = Object(this);
        var len = t.length >>> 0;

        for (var i = 0; i < len; i++) {
            if (i in t && fun.call(this, t[i], i, t)) {
                return true;
            }
        }

        return false;
    };
}

function isValid(m) {
    if (m._isValid == null) {
        var flags = getParsingFlags(m);
        var parsedParts = some.call(flags.parsedDateParts, function (i) {
            return i != null;
        });
        var isNowValid = !isNaN(m._d.getTime()) &&
            flags.overflow < 0 &&
            !flags.empty &&
            !flags.invalidMonth &&
            !flags.invalidWeekday &&
            !flags.weekdayMismatch &&
            !flags.nullInput &&
            !flags.invalidFormat &&
            !flags.userInvalidated &&
            (!flags.meridiem || (flags.meridiem && parsedParts));

        if (m._strict) {
            isNowValid = isNowValid &&
                flags.charsLeftOver === 0 &&
                flags.unusedTokens.length === 0 &&
                flags.bigHour === undefined;
        }

        if (Object.isFrozen == null || !Object.isFrozen(m)) {
            m._isValid = isNowValid;
        }
        else {
            return isNowValid;
        }
    }
    return m._isValid;
}

function createInvalid (flags) {
    var m = createUTC(NaN);
    if (flags != null) {
        extend(getParsingFlags(m), flags);
    }
    else {
        getParsingFlags(m).userInvalidated = true;
    }

    return m;
}

// Plugins that add properties should also add the key here (null value),
// so we can properly clone ourselves.
var momentProperties = hooks.momentProperties = [];

function copyConfig(to, from) {
    var i, prop, val;

    if (!isUndefined(from._isAMomentObject)) {
        to._isAMomentObject = from._isAMomentObject;
    }
    if (!isUndefined(from._i)) {
        to._i = from._i;
    }
    if (!isUndefined(from._f)) {
        to._f = from._f;
    }
    if (!isUndefined(from._l)) {
        to._l = from._l;
    }
    if (!isUndefined(from._strict)) {
        to._strict = from._strict;
    }
    if (!isUndefined(from._tzm)) {
        to._tzm = from._tzm;
    }
    if (!isUndefined(from._isUTC)) {
        to._isUTC = from._isUTC;
    }
    if (!isUndefined(from._offset)) {
        to._offset = from._offset;
    }
    if (!isUndefined(from._pf)) {
        to._pf = getParsingFlags(from);
    }
    if (!isUndefined(from._locale)) {
        to._locale = from._locale;
    }

    if (momentProperties.length > 0) {
        for (i = 0; i < momentProperties.length; i++) {
            prop = momentProperties[i];
            val = from[prop];
            if (!isUndefined(val)) {
                to[prop] = val;
            }
        }
    }

    return to;
}

var updateInProgress = false;

// Moment prototype object
function Moment(config) {
    copyConfig(this, config);
    this._d = new Date(config._d != null ? config._d.getTime() : NaN);
    if (!this.isValid()) {
        this._d = new Date(NaN);
    }
    // Prevent infinite loop in case updateOffset creates new moment
    // objects.
    if (updateInProgress === false) {
        updateInProgress = true;
        hooks.updateOffset(this);
        updateInProgress = false;
    }
}

function isMoment (obj) {
    return obj instanceof Moment || (obj != null && obj._isAMomentObject != null);
}

function absFloor (number) {
    if (number < 0) {
        // -0 -> 0
        return Math.ceil(number) || 0;
    } else {
        return Math.floor(number);
    }
}

function toInt(argumentForCoercion) {
    var coercedNumber = +argumentForCoercion,
        value = 0;

    if (coercedNumber !== 0 && isFinite(coercedNumber)) {
        value = absFloor(coercedNumber);
    }

    return value;
}

// compare two arrays, return the number of differences
function compareArrays(array1, array2, dontConvert) {
    var len = Math.min(array1.length, array2.length),
        lengthDiff = Math.abs(array1.length - array2.length),
        diffs = 0,
        i;
    for (i = 0; i < len; i++) {
        if ((dontConvert && array1[i] !== array2[i]) ||
            (!dontConvert && toInt(array1[i]) !== toInt(array2[i]))) {
            diffs++;
        }
    }
    return diffs + lengthDiff;
}

function warn(msg) {
    if (hooks.suppressDeprecationWarnings === false &&
            (typeof console !==  'undefined') && console.warn) {
        console.warn('Deprecation warning: ' + msg);
    }
}

function deprecate(msg, fn) {
    var firstTime = true;

    return extend(function () {
        if (hooks.deprecationHandler != null) {
            hooks.deprecationHandler(null, msg);
        }
        if (firstTime) {
            var args = [];
            var arg;
            for (var i = 0; i < arguments.length; i++) {
                arg = '';
                if (typeof arguments[i] === 'object') {
                    arg += '\n[' + i + '] ';
                    for (var key in arguments[0]) {
                        arg += key + ': ' + arguments[0][key] + ', ';
                    }
                    arg = arg.slice(0, -2); // Remove trailing comma and space
                } else {
                    arg = arguments[i];
                }
                args.push(arg);
            }
            warn(msg + '\nArguments: ' + Array.prototype.slice.call(args).join('') + '\n' + (new Error()).stack);
            firstTime = false;
        }
        return fn.apply(this, arguments);
    }, fn);
}

var deprecations = {};

function deprecateSimple(name, msg) {
    if (hooks.deprecationHandler != null) {
        hooks.deprecationHandler(name, msg);
    }
    if (!deprecations[name]) {
        warn(msg);
        deprecations[name] = true;
    }
}

hooks.suppressDeprecationWarnings = false;
hooks.deprecationHandler = null;

function isFunction(input) {
    return input instanceof Function || Object.prototype.toString.call(input) === '[object Function]';
}

function set (config) {
    var prop, i;
    for (i in config) {
        prop = config[i];
        if (isFunction(prop)) {
            this[i] = prop;
        } else {
            this['_' + i] = prop;
        }
    }
    this._config = config;
    // Lenient ordinal parsing accepts just a number in addition to
    // number + (possibly) stuff coming from _dayOfMonthOrdinalParse.
    // TODO: Remove "ordinalParse" fallback in next major release.
    this._dayOfMonthOrdinalParseLenient = new RegExp(
        (this._dayOfMonthOrdinalParse.source || this._ordinalParse.source) +
            '|' + (/\d{1,2}/).source);
}

function mergeConfigs(parentConfig, childConfig) {
    var res = extend({}, parentConfig), prop;
    for (prop in childConfig) {
        if (hasOwnProp(childConfig, prop)) {
            if (isObject(parentConfig[prop]) && isObject(childConfig[prop])) {
                res[prop] = {};
                extend(res[prop], parentConfig[prop]);
                extend(res[prop], childConfig[prop]);
            } else if (childConfig[prop] != null) {
                res[prop] = childConfig[prop];
            } else {
                delete res[prop];
            }
        }
    }
    for (prop in parentConfig) {
        if (hasOwnProp(parentConfig, prop) &&
                !hasOwnProp(childConfig, prop) &&
                isObject(parentConfig[prop])) {
            // make sure changes to properties don't modify parent config
            res[prop] = extend({}, res[prop]);
        }
    }
    return res;
}

function Locale(config) {
    if (config != null) {
        this.set(config);
    }
}

var keys;

if (Object.keys) {
    keys = Object.keys;
} else {
    keys = function (obj) {
        var i, res = [];
        for (i in obj) {
            if (hasOwnProp(obj, i)) {
                res.push(i);
            }
        }
        return res;
    };
}

var defaultCalendar = {
    sameDay : '[Today at] LT',
    nextDay : '[Tomorrow at] LT',
    nextWeek : 'dddd [at] LT',
    lastDay : '[Yesterday at] LT',
    lastWeek : '[Last] dddd [at] LT',
    sameElse : 'L'
};

function calendar (key, mom, now) {
    var output = this._calendar[key] || this._calendar['sameElse'];
    return isFunction(output) ? output.call(mom, now) : output;
}

var defaultLongDateFormat = {
    LTS  : 'h:mm:ss A',
    LT   : 'h:mm A',
    L    : 'MM/DD/YYYY',
    LL   : 'MMMM D, YYYY',
    LLL  : 'MMMM D, YYYY h:mm A',
    LLLL : 'dddd, MMMM D, YYYY h:mm A'
};

function longDateFormat (key) {
    var format = this._longDateFormat[key],
        formatUpper = this._longDateFormat[key.toUpperCase()];

    if (format || !formatUpper) {
        return format;
    }

    this._longDateFormat[key] = formatUpper.replace(/MMMM|MM|DD|dddd/g, function (val) {
        return val.slice(1);
    });

    return this._longDateFormat[key];
}

var defaultInvalidDate = 'Invalid date';

function invalidDate () {
    return this._invalidDate;
}

var defaultOrdinal = '%d';
var defaultDayOfMonthOrdinalParse = /\d{1,2}/;

function ordinal (number) {
    return this._ordinal.replace('%d', number);
}

var defaultRelativeTime = {
    future : 'in %s',
    past   : '%s ago',
    s  : 'a few seconds',
    ss : '%d seconds',
    m  : 'a minute',
    mm : '%d minutes',
    h  : 'an hour',
    hh : '%d hours',
    d  : 'a day',
    dd : '%d days',
    M  : 'a month',
    MM : '%d months',
    y  : 'a year',
    yy : '%d years'
};

function relativeTime (number, withoutSuffix, string, isFuture) {
    var output = this._relativeTime[string];
    return (isFunction(output)) ?
        output(number, withoutSuffix, string, isFuture) :
        output.replace(/%d/i, number);
}

function pastFuture (diff, output) {
    var format = this._relativeTime[diff > 0 ? 'future' : 'past'];
    return isFunction(format) ? format(output) : format.replace(/%s/i, output);
}

var aliases = {};

function addUnitAlias (unit, shorthand) {
    var lowerCase = unit.toLowerCase();
    aliases[lowerCase] = aliases[lowerCase + 's'] = aliases[shorthand] = unit;
}

function normalizeUnits(units) {
    return typeof units === 'string' ? aliases[units] || aliases[units.toLowerCase()] : undefined;
}

function normalizeObjectUnits(inputObject) {
    var normalizedInput = {},
        normalizedProp,
        prop;

    for (prop in inputObject) {
        if (hasOwnProp(inputObject, prop)) {
            normalizedProp = normalizeUnits(prop);
            if (normalizedProp) {
                normalizedInput[normalizedProp] = inputObject[prop];
            }
        }
    }

    return normalizedInput;
}

var priorities = {};

function addUnitPriority(unit, priority) {
    priorities[unit] = priority;
}

function getPrioritizedUnits(unitsObj) {
    var units = [];
    for (var u in unitsObj) {
        units.push({unit: u, priority: priorities[u]});
    }
    units.sort(function (a, b) {
        return a.priority - b.priority;
    });
    return units;
}

function zeroFill(number, targetLength, forceSign) {
    var absNumber = '' + Math.abs(number),
        zerosToFill = targetLength - absNumber.length,
        sign = number >= 0;
    return (sign ? (forceSign ? '+' : '') : '-') +
        Math.pow(10, Math.max(0, zerosToFill)).toString().substr(1) + absNumber;
}

var formattingTokens = /(\[[^\[]*\])|(\\)?([Hh]mm(ss)?|Mo|MM?M?M?|Do|DDDo|DD?D?D?|ddd?d?|do?|w[o|w]?|W[o|W]?|Qo?|YYYYYY|YYYYY|YYYY|YY|gg(ggg?)?|GG(GGG?)?|e|E|a|A|hh?|HH?|kk?|mm?|ss?|S{1,9}|x|X|zz?|ZZ?|.)/g;

var localFormattingTokens = /(\[[^\[]*\])|(\\)?(LTS|LT|LL?L?L?|l{1,4})/g;

var formatFunctions = {};

var formatTokenFunctions = {};

// token:    'M'
// padded:   ['MM', 2]
// ordinal:  'Mo'
// callback: function () { this.month() + 1 }
function addFormatToken (token, padded, ordinal, callback) {
    var func = callback;
    if (typeof callback === 'string') {
        func = function () {
            return this[callback]();
        };
    }
    if (token) {
        formatTokenFunctions[token] = func;
    }
    if (padded) {
        formatTokenFunctions[padded[0]] = function () {
            return zeroFill(func.apply(this, arguments), padded[1], padded[2]);
        };
    }
    if (ordinal) {
        formatTokenFunctions[ordinal] = function () {
            return this.localeData().ordinal(func.apply(this, arguments), token);
        };
    }
}

function removeFormattingTokens(input) {
    if (input.match(/\[[\s\S]/)) {
        return input.replace(/^\[|\]$/g, '');
    }
    return input.replace(/\\/g, '');
}

function makeFormatFunction(format) {
    var array = format.match(formattingTokens), i, length;

    for (i = 0, length = array.length; i < length; i++) {
        if (formatTokenFunctions[array[i]]) {
            array[i] = formatTokenFunctions[array[i]];
        } else {
            array[i] = removeFormattingTokens(array[i]);
        }
    }

    return function (mom) {
        var output = '', i;
        for (i = 0; i < length; i++) {
            output += isFunction(array[i]) ? array[i].call(mom, format) : array[i];
        }
        return output;
    };
}

// format date using native date object
function formatMoment(m, format) {
    if (!m.isValid()) {
        return m.localeData().invalidDate();
    }

    format = expandFormat(format, m.localeData());
    formatFunctions[format] = formatFunctions[format] || makeFormatFunction(format);

    return formatFunctions[format](m);
}

function expandFormat(format, locale) {
    var i = 5;

    function replaceLongDateFormatTokens(input) {
        return locale.longDateFormat(input) || input;
    }

    localFormattingTokens.lastIndex = 0;
    while (i >= 0 && localFormattingTokens.test(format)) {
        format = format.replace(localFormattingTokens, replaceLongDateFormatTokens);
        localFormattingTokens.lastIndex = 0;
        i -= 1;
    }

    return format;
}

var match1         = /\d/;            //       0 - 9
var match2         = /\d\d/;          //      00 - 99
var match3         = /\d{3}/;         //     000 - 999
var match4         = /\d{4}/;         //    0000 - 9999
var match6         = /[+-]?\d{6}/;    // -999999 - 999999
var match1to2      = /\d\d?/;         //       0 - 99
var match3to4      = /\d\d\d\d?/;     //     999 - 9999
var match5to6      = /\d\d\d\d\d\d?/; //   99999 - 999999
var match1to3      = /\d{1,3}/;       //       0 - 999
var match1to4      = /\d{1,4}/;       //       0 - 9999
var match1to6      = /[+-]?\d{1,6}/;  // -999999 - 999999

var matchUnsigned  = /\d+/;           //       0 - inf
var matchSigned    = /[+-]?\d+/;      //    -inf - inf

var matchOffset    = /Z|[+-]\d\d:?\d\d/gi; // +00:00 -00:00 +0000 -0000 or Z
var matchShortOffset = /Z|[+-]\d\d(?::?\d\d)?/gi; // +00 -00 +00:00 -00:00 +0000 -0000 or Z

var matchTimestamp = /[+-]?\d+(\.\d{1,3})?/; // 123456789 123456789.123

// any word (or two) characters or numbers including two/three word month in arabic.
// includes scottish gaelic two word and hyphenated months
var matchWord = /[0-9]{0,256}['a-z\u00A0-\u05FF\u0700-\uD7FF\uF900-\uFDCF\uFDF0-\uFF07\uFF10-\uFFEF]{1,256}|[\u0600-\u06FF\/]{1,256}(\s*?[\u0600-\u06FF]{1,256}){1,2}/i;


var regexes = {};

function addRegexToken (token, regex, strictRegex) {
    regexes[token] = isFunction(regex) ? regex : function (isStrict, localeData) {
        return (isStrict && strictRegex) ? strictRegex : regex;
    };
}

function getParseRegexForToken (token, config) {
    if (!hasOwnProp(regexes, token)) {
        return new RegExp(unescapeFormat(token));
    }

    return regexes[token](config._strict, config._locale);
}

// Code from http://stackoverflow.com/questions/3561493/is-there-a-regexp-escape-function-in-javascript
function unescapeFormat(s) {
    return regexEscape(s.replace('\\', '').replace(/\\(\[)|\\(\])|\[([^\]\[]*)\]|\\(.)/g, function (matched, p1, p2, p3, p4) {
        return p1 || p2 || p3 || p4;
    }));
}

function regexEscape(s) {
    return s.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
}

var tokens = {};

function addParseToken (token, callback) {
    var i, func = callback;
    if (typeof token === 'string') {
        token = [token];
    }
    if (isNumber(callback)) {
        func = function (input, array) {
            array[callback] = toInt(input);
        };
    }
    for (i = 0; i < token.length; i++) {
        tokens[token[i]] = func;
    }
}

function addWeekParseToken (token, callback) {
    addParseToken(token, function (input, array, config, token) {
        config._w = config._w || {};
        callback(input, config._w, config, token);
    });
}

function addTimeToArrayFromToken(token, input, config) {
    if (input != null && hasOwnProp(tokens, token)) {
        tokens[token](input, config._a, config, token);
    }
}

var YEAR = 0;
var MONTH = 1;
var DATE = 2;
var HOUR = 3;
var MINUTE = 4;
var SECOND = 5;
var MILLISECOND = 6;
var WEEK = 7;
var WEEKDAY = 8;

// FORMATTING

addFormatToken('Y', 0, 0, function () {
    var y = this.year();
    return y <= 9999 ? '' + y : '+' + y;
});

addFormatToken(0, ['YY', 2], 0, function () {
    return this.year() % 100;
});

addFormatToken(0, ['YYYY',   4],       0, 'year');
addFormatToken(0, ['YYYYY',  5],       0, 'year');
addFormatToken(0, ['YYYYYY', 6, true], 0, 'year');

// ALIASES

addUnitAlias('year', 'y');

// PRIORITIES

addUnitPriority('year', 1);

// PARSING

addRegexToken('Y',      matchSigned);
addRegexToken('YY',     match1to2, match2);
addRegexToken('YYYY',   match1to4, match4);
addRegexToken('YYYYY',  match1to6, match6);
addRegexToken('YYYYYY', match1to6, match6);

addParseToken(['YYYYY', 'YYYYYY'], YEAR);
addParseToken('YYYY', function (input, array) {
    array[YEAR] = input.length === 2 ? hooks.parseTwoDigitYear(input) : toInt(input);
});
addParseToken('YY', function (input, array) {
    array[YEAR] = hooks.parseTwoDigitYear(input);
});
addParseToken('Y', function (input, array) {
    array[YEAR] = parseInt(input, 10);
});

// HELPERS

function daysInYear(year) {
    return isLeapYear(year) ? 366 : 365;
}

function isLeapYear(year) {
    return (year % 4 === 0 && year % 100 !== 0) || year % 400 === 0;
}

// HOOKS

hooks.parseTwoDigitYear = function (input) {
    return toInt(input) + (toInt(input) > 68 ? 1900 : 2000);
};

// MOMENTS

var getSetYear = makeGetSet('FullYear', true);

function getIsLeapYear () {
    return isLeapYear(this.year());
}

function makeGetSet (unit, keepTime) {
    return function (value) {
        if (value != null) {
            set$1(this, unit, value);
            hooks.updateOffset(this, keepTime);
            return this;
        } else {
            return get(this, unit);
        }
    };
}

function get (mom, unit) {
    return mom.isValid() ?
        mom._d['get' + (mom._isUTC ? 'UTC' : '') + unit]() : NaN;
}

function set$1 (mom, unit, value) {
    if (mom.isValid() && !isNaN(value)) {
        if (unit === 'FullYear' && isLeapYear(mom.year()) && mom.month() === 1 && mom.date() === 29) {
            mom._d['set' + (mom._isUTC ? 'UTC' : '') + unit](value, mom.month(), daysInMonth(value, mom.month()));
        }
        else {
            mom._d['set' + (mom._isUTC ? 'UTC' : '') + unit](value);
        }
    }
}

// MOMENTS

function stringGet (units) {
    units = normalizeUnits(units);
    if (isFunction(this[units])) {
        return this[units]();
    }
    return this;
}


function stringSet (units, value) {
    if (typeof units === 'object') {
        units = normalizeObjectUnits(units);
        var prioritized = getPrioritizedUnits(units);
        for (var i = 0; i < prioritized.length; i++) {
            this[prioritized[i].unit](units[prioritized[i].unit]);
        }
    } else {
        units = normalizeUnits(units);
        if (isFunction(this[units])) {
            return this[units](value);
        }
    }
    return this;
}

function mod(n, x) {
    return ((n % x) + x) % x;
}

var indexOf;

if (Array.prototype.indexOf) {
    indexOf = Array.prototype.indexOf;
} else {
    indexOf = function (o) {
        // I know
        var i;
        for (i = 0; i < this.length; ++i) {
            if (this[i] === o) {
                return i;
            }
        }
        return -1;
    };
}

function daysInMonth(year, month) {
    if (isNaN(year) || isNaN(month)) {
        return NaN;
    }
    var modMonth = mod(month, 12);
    year += (month - modMonth) / 12;
    return modMonth === 1 ? (isLeapYear(year) ? 29 : 28) : (31 - modMonth % 7 % 2);
}

// FORMATTING

addFormatToken('M', ['MM', 2], 'Mo', function () {
    return this.month() + 1;
});

addFormatToken('MMM', 0, 0, function (format) {
    return this.localeData().monthsShort(this, format);
});

addFormatToken('MMMM', 0, 0, function (format) {
    return this.localeData().months(this, format);
});

// ALIASES

addUnitAlias('month', 'M');

// PRIORITY

addUnitPriority('month', 8);

// PARSING

addRegexToken('M',    match1to2);
addRegexToken('MM',   match1to2, match2);
addRegexToken('MMM',  function (isStrict, locale) {
    return locale.monthsShortRegex(isStrict);
});
addRegexToken('MMMM', function (isStrict, locale) {
    return locale.monthsRegex(isStrict);
});

addParseToken(['M', 'MM'], function (input, array) {
    array[MONTH] = toInt(input) - 1;
});

addParseToken(['MMM', 'MMMM'], function (input, array, config, token) {
    var month = config._locale.monthsParse(input, token, config._strict);
    // if we didn't find a month name, mark the date as invalid.
    if (month != null) {
        array[MONTH] = month;
    } else {
        getParsingFlags(config).invalidMonth = input;
    }
});

// LOCALES

var MONTHS_IN_FORMAT = /D[oD]?(\[[^\[\]]*\]|\s)+MMMM?/;
var defaultLocaleMonths = 'January_February_March_April_May_June_July_August_September_October_November_December'.split('_');
function localeMonths (m, format) {
    if (!m) {
        return isArray(this._months) ? this._months :
            this._months['standalone'];
    }
    return isArray(this._months) ? this._months[m.month()] :
        this._months[(this._months.isFormat || MONTHS_IN_FORMAT).test(format) ? 'format' : 'standalone'][m.month()];
}

var defaultLocaleMonthsShort = 'Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec'.split('_');
function localeMonthsShort (m, format) {
    if (!m) {
        return isArray(this._monthsShort) ? this._monthsShort :
            this._monthsShort['standalone'];
    }
    return isArray(this._monthsShort) ? this._monthsShort[m.month()] :
        this._monthsShort[MONTHS_IN_FORMAT.test(format) ? 'format' : 'standalone'][m.month()];
}

function handleStrictParse(monthName, format, strict) {
    var i, ii, mom, llc = monthName.toLocaleLowerCase();
    if (!this._monthsParse) {
        // this is not used
        this._monthsParse = [];
        this._longMonthsParse = [];
        this._shortMonthsParse = [];
        for (i = 0; i < 12; ++i) {
            mom = createUTC([2000, i]);
            this._shortMonthsParse[i] = this.monthsShort(mom, '').toLocaleLowerCase();
            this._longMonthsParse[i] = this.months(mom, '').toLocaleLowerCase();
        }
    }

    if (strict) {
        if (format === 'MMM') {
            ii = indexOf.call(this._shortMonthsParse, llc);
            return ii !== -1 ? ii : null;
        } else {
            ii = indexOf.call(this._longMonthsParse, llc);
            return ii !== -1 ? ii : null;
        }
    } else {
        if (format === 'MMM') {
            ii = indexOf.call(this._shortMonthsParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._longMonthsParse, llc);
            return ii !== -1 ? ii : null;
        } else {
            ii = indexOf.call(this._longMonthsParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._shortMonthsParse, llc);
            return ii !== -1 ? ii : null;
        }
    }
}

function localeMonthsParse (monthName, format, strict) {
    var i, mom, regex;

    if (this._monthsParseExact) {
        return handleStrictParse.call(this, monthName, format, strict);
    }

    if (!this._monthsParse) {
        this._monthsParse = [];
        this._longMonthsParse = [];
        this._shortMonthsParse = [];
    }

    // TODO: add sorting
    // Sorting makes sure if one month (or abbr) is a prefix of another
    // see sorting in computeMonthsParse
    for (i = 0; i < 12; i++) {
        // make the regex if we don't have it already
        mom = createUTC([2000, i]);
        if (strict && !this._longMonthsParse[i]) {
            this._longMonthsParse[i] = new RegExp('^' + this.months(mom, '').replace('.', '') + '$', 'i');
            this._shortMonthsParse[i] = new RegExp('^' + this.monthsShort(mom, '').replace('.', '') + '$', 'i');
        }
        if (!strict && !this._monthsParse[i]) {
            regex = '^' + this.months(mom, '') + '|^' + this.monthsShort(mom, '');
            this._monthsParse[i] = new RegExp(regex.replace('.', ''), 'i');
        }
        // test the regex
        if (strict && format === 'MMMM' && this._longMonthsParse[i].test(monthName)) {
            return i;
        } else if (strict && format === 'MMM' && this._shortMonthsParse[i].test(monthName)) {
            return i;
        } else if (!strict && this._monthsParse[i].test(monthName)) {
            return i;
        }
    }
}

// MOMENTS

function setMonth (mom, value) {
    var dayOfMonth;

    if (!mom.isValid()) {
        // No op
        return mom;
    }

    if (typeof value === 'string') {
        if (/^\d+$/.test(value)) {
            value = toInt(value);
        } else {
            value = mom.localeData().monthsParse(value);
            // TODO: Another silent failure?
            if (!isNumber(value)) {
                return mom;
            }
        }
    }

    dayOfMonth = Math.min(mom.date(), daysInMonth(mom.year(), value));
    mom._d['set' + (mom._isUTC ? 'UTC' : '') + 'Month'](value, dayOfMonth);
    return mom;
}

function getSetMonth (value) {
    if (value != null) {
        setMonth(this, value);
        hooks.updateOffset(this, true);
        return this;
    } else {
        return get(this, 'Month');
    }
}

function getDaysInMonth () {
    return daysInMonth(this.year(), this.month());
}

var defaultMonthsShortRegex = matchWord;
function monthsShortRegex (isStrict) {
    if (this._monthsParseExact) {
        if (!hasOwnProp(this, '_monthsRegex')) {
            computeMonthsParse.call(this);
        }
        if (isStrict) {
            return this._monthsShortStrictRegex;
        } else {
            return this._monthsShortRegex;
        }
    } else {
        if (!hasOwnProp(this, '_monthsShortRegex')) {
            this._monthsShortRegex = defaultMonthsShortRegex;
        }
        return this._monthsShortStrictRegex && isStrict ?
            this._monthsShortStrictRegex : this._monthsShortRegex;
    }
}

var defaultMonthsRegex = matchWord;
function monthsRegex (isStrict) {
    if (this._monthsParseExact) {
        if (!hasOwnProp(this, '_monthsRegex')) {
            computeMonthsParse.call(this);
        }
        if (isStrict) {
            return this._monthsStrictRegex;
        } else {
            return this._monthsRegex;
        }
    } else {
        if (!hasOwnProp(this, '_monthsRegex')) {
            this._monthsRegex = defaultMonthsRegex;
        }
        return this._monthsStrictRegex && isStrict ?
            this._monthsStrictRegex : this._monthsRegex;
    }
}

function computeMonthsParse () {
    function cmpLenRev(a, b) {
        return b.length - a.length;
    }

    var shortPieces = [], longPieces = [], mixedPieces = [],
        i, mom;
    for (i = 0; i < 12; i++) {
        // make the regex if we don't have it already
        mom = createUTC([2000, i]);
        shortPieces.push(this.monthsShort(mom, ''));
        longPieces.push(this.months(mom, ''));
        mixedPieces.push(this.months(mom, ''));
        mixedPieces.push(this.monthsShort(mom, ''));
    }
    // Sorting makes sure if one month (or abbr) is a prefix of another it
    // will match the longer piece.
    shortPieces.sort(cmpLenRev);
    longPieces.sort(cmpLenRev);
    mixedPieces.sort(cmpLenRev);
    for (i = 0; i < 12; i++) {
        shortPieces[i] = regexEscape(shortPieces[i]);
        longPieces[i] = regexEscape(longPieces[i]);
    }
    for (i = 0; i < 24; i++) {
        mixedPieces[i] = regexEscape(mixedPieces[i]);
    }

    this._monthsRegex = new RegExp('^(' + mixedPieces.join('|') + ')', 'i');
    this._monthsShortRegex = this._monthsRegex;
    this._monthsStrictRegex = new RegExp('^(' + longPieces.join('|') + ')', 'i');
    this._monthsShortStrictRegex = new RegExp('^(' + shortPieces.join('|') + ')', 'i');
}

function createDate (y, m, d, h, M, s, ms) {
    // can't just apply() to create a date:
    // https://stackoverflow.com/q/181348
    var date = new Date(y, m, d, h, M, s, ms);

    // the date constructor remaps years 0-99 to 1900-1999
    if (y < 100 && y >= 0 && isFinite(date.getFullYear())) {
        date.setFullYear(y);
    }
    return date;
}

function createUTCDate (y) {
    var date = new Date(Date.UTC.apply(null, arguments));

    // the Date.UTC function remaps years 0-99 to 1900-1999
    if (y < 100 && y >= 0 && isFinite(date.getUTCFullYear())) {
        date.setUTCFullYear(y);
    }
    return date;
}

// start-of-first-week - start-of-year
function firstWeekOffset(year, dow, doy) {
    var // first-week day -- which january is always in the first week (4 for iso, 1 for other)
        fwd = 7 + dow - doy,
        // first-week day local weekday -- which local weekday is fwd
        fwdlw = (7 + createUTCDate(year, 0, fwd).getUTCDay() - dow) % 7;

    return -fwdlw + fwd - 1;
}

// https://en.wikipedia.org/wiki/ISO_week_date#Calculating_a_date_given_the_year.2C_week_number_and_weekday
function dayOfYearFromWeeks(year, week, weekday, dow, doy) {
    var localWeekday = (7 + weekday - dow) % 7,
        weekOffset = firstWeekOffset(year, dow, doy),
        dayOfYear = 1 + 7 * (week - 1) + localWeekday + weekOffset,
        resYear, resDayOfYear;

    if (dayOfYear <= 0) {
        resYear = year - 1;
        resDayOfYear = daysInYear(resYear) + dayOfYear;
    } else if (dayOfYear > daysInYear(year)) {
        resYear = year + 1;
        resDayOfYear = dayOfYear - daysInYear(year);
    } else {
        resYear = year;
        resDayOfYear = dayOfYear;
    }

    return {
        year: resYear,
        dayOfYear: resDayOfYear
    };
}

function weekOfYear(mom, dow, doy) {
    var weekOffset = firstWeekOffset(mom.year(), dow, doy),
        week = Math.floor((mom.dayOfYear() - weekOffset - 1) / 7) + 1,
        resWeek, resYear;

    if (week < 1) {
        resYear = mom.year() - 1;
        resWeek = week + weeksInYear(resYear, dow, doy);
    } else if (week > weeksInYear(mom.year(), dow, doy)) {
        resWeek = week - weeksInYear(mom.year(), dow, doy);
        resYear = mom.year() + 1;
    } else {
        resYear = mom.year();
        resWeek = week;
    }

    return {
        week: resWeek,
        year: resYear
    };
}

function weeksInYear(year, dow, doy) {
    var weekOffset = firstWeekOffset(year, dow, doy),
        weekOffsetNext = firstWeekOffset(year + 1, dow, doy);
    return (daysInYear(year) - weekOffset + weekOffsetNext) / 7;
}

// FORMATTING

addFormatToken('w', ['ww', 2], 'wo', 'week');
addFormatToken('W', ['WW', 2], 'Wo', 'isoWeek');

// ALIASES

addUnitAlias('week', 'w');
addUnitAlias('isoWeek', 'W');

// PRIORITIES

addUnitPriority('week', 5);
addUnitPriority('isoWeek', 5);

// PARSING

addRegexToken('w',  match1to2);
addRegexToken('ww', match1to2, match2);
addRegexToken('W',  match1to2);
addRegexToken('WW', match1to2, match2);

addWeekParseToken(['w', 'ww', 'W', 'WW'], function (input, week, config, token) {
    week[token.substr(0, 1)] = toInt(input);
});

// HELPERS

// LOCALES

function localeWeek (mom) {
    return weekOfYear(mom, this._week.dow, this._week.doy).week;
}

var defaultLocaleWeek = {
    dow : 0, // Sunday is the first day of the week.
    doy : 6  // The week that contains Jan 1st is the first week of the year.
};

function localeFirstDayOfWeek () {
    return this._week.dow;
}

function localeFirstDayOfYear () {
    return this._week.doy;
}

// MOMENTS

function getSetWeek (input) {
    var week = this.localeData().week(this);
    return input == null ? week : this.add((input - week) * 7, 'd');
}

function getSetISOWeek (input) {
    var week = weekOfYear(this, 1, 4).week;
    return input == null ? week : this.add((input - week) * 7, 'd');
}

// FORMATTING

addFormatToken('d', 0, 'do', 'day');

addFormatToken('dd', 0, 0, function (format) {
    return this.localeData().weekdaysMin(this, format);
});

addFormatToken('ddd', 0, 0, function (format) {
    return this.localeData().weekdaysShort(this, format);
});

addFormatToken('dddd', 0, 0, function (format) {
    return this.localeData().weekdays(this, format);
});

addFormatToken('e', 0, 0, 'weekday');
addFormatToken('E', 0, 0, 'isoWeekday');

// ALIASES

addUnitAlias('day', 'd');
addUnitAlias('weekday', 'e');
addUnitAlias('isoWeekday', 'E');

// PRIORITY
addUnitPriority('day', 11);
addUnitPriority('weekday', 11);
addUnitPriority('isoWeekday', 11);

// PARSING

addRegexToken('d',    match1to2);
addRegexToken('e',    match1to2);
addRegexToken('E',    match1to2);
addRegexToken('dd',   function (isStrict, locale) {
    return locale.weekdaysMinRegex(isStrict);
});
addRegexToken('ddd',   function (isStrict, locale) {
    return locale.weekdaysShortRegex(isStrict);
});
addRegexToken('dddd',   function (isStrict, locale) {
    return locale.weekdaysRegex(isStrict);
});

addWeekParseToken(['dd', 'ddd', 'dddd'], function (input, week, config, token) {
    var weekday = config._locale.weekdaysParse(input, token, config._strict);
    // if we didn't get a weekday name, mark the date as invalid
    if (weekday != null) {
        week.d = weekday;
    } else {
        getParsingFlags(config).invalidWeekday = input;
    }
});

addWeekParseToken(['d', 'e', 'E'], function (input, week, config, token) {
    week[token] = toInt(input);
});

// HELPERS

function parseWeekday(input, locale) {
    if (typeof input !== 'string') {
        return input;
    }

    if (!isNaN(input)) {
        return parseInt(input, 10);
    }

    input = locale.weekdaysParse(input);
    if (typeof input === 'number') {
        return input;
    }

    return null;
}

function parseIsoWeekday(input, locale) {
    if (typeof input === 'string') {
        return locale.weekdaysParse(input) % 7 || 7;
    }
    return isNaN(input) ? null : input;
}

// LOCALES

var defaultLocaleWeekdays = 'Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday'.split('_');
function localeWeekdays (m, format) {
    if (!m) {
        return isArray(this._weekdays) ? this._weekdays :
            this._weekdays['standalone'];
    }
    return isArray(this._weekdays) ? this._weekdays[m.day()] :
        this._weekdays[this._weekdays.isFormat.test(format) ? 'format' : 'standalone'][m.day()];
}

var defaultLocaleWeekdaysShort = 'Sun_Mon_Tue_Wed_Thu_Fri_Sat'.split('_');
function localeWeekdaysShort (m) {
    return (m) ? this._weekdaysShort[m.day()] : this._weekdaysShort;
}

var defaultLocaleWeekdaysMin = 'Su_Mo_Tu_We_Th_Fr_Sa'.split('_');
function localeWeekdaysMin (m) {
    return (m) ? this._weekdaysMin[m.day()] : this._weekdaysMin;
}

function handleStrictParse$1(weekdayName, format, strict) {
    var i, ii, mom, llc = weekdayName.toLocaleLowerCase();
    if (!this._weekdaysParse) {
        this._weekdaysParse = [];
        this._shortWeekdaysParse = [];
        this._minWeekdaysParse = [];

        for (i = 0; i < 7; ++i) {
            mom = createUTC([2000, 1]).day(i);
            this._minWeekdaysParse[i] = this.weekdaysMin(mom, '').toLocaleLowerCase();
            this._shortWeekdaysParse[i] = this.weekdaysShort(mom, '').toLocaleLowerCase();
            this._weekdaysParse[i] = this.weekdays(mom, '').toLocaleLowerCase();
        }
    }

    if (strict) {
        if (format === 'dddd') {
            ii = indexOf.call(this._weekdaysParse, llc);
            return ii !== -1 ? ii : null;
        } else if (format === 'ddd') {
            ii = indexOf.call(this._shortWeekdaysParse, llc);
            return ii !== -1 ? ii : null;
        } else {
            ii = indexOf.call(this._minWeekdaysParse, llc);
            return ii !== -1 ? ii : null;
        }
    } else {
        if (format === 'dddd') {
            ii = indexOf.call(this._weekdaysParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._shortWeekdaysParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._minWeekdaysParse, llc);
            return ii !== -1 ? ii : null;
        } else if (format === 'ddd') {
            ii = indexOf.call(this._shortWeekdaysParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._weekdaysParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._minWeekdaysParse, llc);
            return ii !== -1 ? ii : null;
        } else {
            ii = indexOf.call(this._minWeekdaysParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._weekdaysParse, llc);
            if (ii !== -1) {
                return ii;
            }
            ii = indexOf.call(this._shortWeekdaysParse, llc);
            return ii !== -1 ? ii : null;
        }
    }
}

function localeWeekdaysParse (weekdayName, format, strict) {
    var i, mom, regex;

    if (this._weekdaysParseExact) {
        return handleStrictParse$1.call(this, weekdayName, format, strict);
    }

    if (!this._weekdaysParse) {
        this._weekdaysParse = [];
        this._minWeekdaysParse = [];
        this._shortWeekdaysParse = [];
        this._fullWeekdaysParse = [];
    }

    for (i = 0; i < 7; i++) {
        // make the regex if we don't have it already

        mom = createUTC([2000, 1]).day(i);
        if (strict && !this._fullWeekdaysParse[i]) {
            this._fullWeekdaysParse[i] = new RegExp('^' + this.weekdays(mom, '').replace('.', '\.?') + '$', 'i');
            this._shortWeekdaysParse[i] = new RegExp('^' + this.weekdaysShort(mom, '').replace('.', '\.?') + '$', 'i');
            this._minWeekdaysParse[i] = new RegExp('^' + this.weekdaysMin(mom, '').replace('.', '\.?') + '$', 'i');
        }
        if (!this._weekdaysParse[i]) {
            regex = '^' + this.weekdays(mom, '') + '|^' + this.weekdaysShort(mom, '') + '|^' + this.weekdaysMin(mom, '');
            this._weekdaysParse[i] = new RegExp(regex.replace('.', ''), 'i');
        }
        // test the regex
        if (strict && format === 'dddd' && this._fullWeekdaysParse[i].test(weekdayName)) {
            return i;
        } else if (strict && format === 'ddd' && this._shortWeekdaysParse[i].test(weekdayName)) {
            return i;
        } else if (strict && format === 'dd' && this._minWeekdaysParse[i].test(weekdayName)) {
            return i;
        } else if (!strict && this._weekdaysParse[i].test(weekdayName)) {
            return i;
        }
    }
}

// MOMENTS

function getSetDayOfWeek (input) {
    if (!this.isValid()) {
        return input != null ? this : NaN;
    }
    var day = this._isUTC ? this._d.getUTCDay() : this._d.getDay();
    if (input != null) {
        input = parseWeekday(input, this.localeData());
        return this.add(input - day, 'd');
    } else {
        return day;
    }
}

function getSetLocaleDayOfWeek (input) {
    if (!this.isValid()) {
        return input != null ? this : NaN;
    }
    var weekday = (this.day() + 7 - this.localeData()._week.dow) % 7;
    return input == null ? weekday : this.add(input - weekday, 'd');
}

function getSetISODayOfWeek (input) {
    if (!this.isValid()) {
        return input != null ? this : NaN;
    }

    // behaves the same as moment#day except
    // as a getter, returns 7 instead of 0 (1-7 range instead of 0-6)
    // as a setter, sunday should belong to the previous week.

    if (input != null) {
        var weekday = parseIsoWeekday(input, this.localeData());
        return this.day(this.day() % 7 ? weekday : weekday - 7);
    } else {
        return this.day() || 7;
    }
}

var defaultWeekdaysRegex = matchWord;
function weekdaysRegex (isStrict) {
    if (this._weekdaysParseExact) {
        if (!hasOwnProp(this, '_weekdaysRegex')) {
            computeWeekdaysParse.call(this);
        }
        if (isStrict) {
            return this._weekdaysStrictRegex;
        } else {
            return this._weekdaysRegex;
        }
    } else {
        if (!hasOwnProp(this, '_weekdaysRegex')) {
            this._weekdaysRegex = defaultWeekdaysRegex;
        }
        return this._weekdaysStrictRegex && isStrict ?
            this._weekdaysStrictRegex : this._weekdaysRegex;
    }
}

var defaultWeekdaysShortRegex = matchWord;
function weekdaysShortRegex (isStrict) {
    if (this._weekdaysParseExact) {
        if (!hasOwnProp(this, '_weekdaysRegex')) {
            computeWeekdaysParse.call(this);
        }
        if (isStrict) {
            return this._weekdaysShortStrictRegex;
        } else {
            return this._weekdaysShortRegex;
        }
    } else {
        if (!hasOwnProp(this, '_weekdaysShortRegex')) {
            this._weekdaysShortRegex = defaultWeekdaysShortRegex;
        }
        return this._weekdaysShortStrictRegex && isStrict ?
            this._weekdaysShortStrictRegex : this._weekdaysShortRegex;
    }
}

var defaultWeekdaysMinRegex = matchWord;
function weekdaysMinRegex (isStrict) {
    if (this._weekdaysParseExact) {
        if (!hasOwnProp(this, '_weekdaysRegex')) {
            computeWeekdaysParse.call(this);
        }
        if (isStrict) {
            return this._weekdaysMinStrictRegex;
        } else {
            return this._weekdaysMinRegex;
        }
    } else {
        if (!hasOwnProp(this, '_weekdaysMinRegex')) {
            this._weekdaysMinRegex = defaultWeekdaysMinRegex;
        }
        return this._weekdaysMinStrictRegex && isStrict ?
            this._weekdaysMinStrictRegex : this._weekdaysMinRegex;
    }
}


function computeWeekdaysParse () {
    function cmpLenRev(a, b) {
        return b.length - a.length;
    }

    var minPieces = [], shortPieces = [], longPieces = [], mixedPieces = [],
        i, mom, minp, shortp, longp;
    for (i = 0; i < 7; i++) {
        // make the regex if we don't have it already
        mom = createUTC([2000, 1]).day(i);
        minp = this.weekdaysMin(mom, '');
        shortp = this.weekdaysShort(mom, '');
        longp = this.weekdays(mom, '');
        minPieces.push(minp);
        shortPieces.push(shortp);
        longPieces.push(longp);
        mixedPieces.push(minp);
        mixedPieces.push(shortp);
        mixedPieces.push(longp);
    }
    // Sorting makes sure if one weekday (or abbr) is a prefix of another it
    // will match the longer piece.
    minPieces.sort(cmpLenRev);
    shortPieces.sort(cmpLenRev);
    longPieces.sort(cmpLenRev);
    mixedPieces.sort(cmpLenRev);
    for (i = 0; i < 7; i++) {
        shortPieces[i] = regexEscape(shortPieces[i]);
        longPieces[i] = regexEscape(longPieces[i]);
        mixedPieces[i] = regexEscape(mixedPieces[i]);
    }

    this._weekdaysRegex = new RegExp('^(' + mixedPieces.join('|') + ')', 'i');
    this._weekdaysShortRegex = this._weekdaysRegex;
    this._weekdaysMinRegex = this._weekdaysRegex;

    this._weekdaysStrictRegex = new RegExp('^(' + longPieces.join('|') + ')', 'i');
    this._weekdaysShortStrictRegex = new RegExp('^(' + shortPieces.join('|') + ')', 'i');
    this._weekdaysMinStrictRegex = new RegExp('^(' + minPieces.join('|') + ')', 'i');
}

// FORMATTING

function hFormat() {
    return this.hours() % 12 || 12;
}

function kFormat() {
    return this.hours() || 24;
}

addFormatToken('H', ['HH', 2], 0, 'hour');
addFormatToken('h', ['hh', 2], 0, hFormat);
addFormatToken('k', ['kk', 2], 0, kFormat);

addFormatToken('hmm', 0, 0, function () {
    return '' + hFormat.apply(this) + zeroFill(this.minutes(), 2);
});

addFormatToken('hmmss', 0, 0, function () {
    return '' + hFormat.apply(this) + zeroFill(this.minutes(), 2) +
        zeroFill(this.seconds(), 2);
});

addFormatToken('Hmm', 0, 0, function () {
    return '' + this.hours() + zeroFill(this.minutes(), 2);
});

addFormatToken('Hmmss', 0, 0, function () {
    return '' + this.hours() + zeroFill(this.minutes(), 2) +
        zeroFill(this.seconds(), 2);
});

function meridiem (token, lowercase) {
    addFormatToken(token, 0, 0, function () {
        return this.localeData().meridiem(this.hours(), this.minutes(), lowercase);
    });
}

meridiem('a', true);
meridiem('A', false);

// ALIASES

addUnitAlias('hour', 'h');

// PRIORITY
addUnitPriority('hour', 13);

// PARSING

function matchMeridiem (isStrict, locale) {
    return locale._meridiemParse;
}

addRegexToken('a',  matchMeridiem);
addRegexToken('A',  matchMeridiem);
addRegexToken('H',  match1to2);
addRegexToken('h',  match1to2);
addRegexToken('k',  match1to2);
addRegexToken('HH', match1to2, match2);
addRegexToken('hh', match1to2, match2);
addRegexToken('kk', match1to2, match2);

addRegexToken('hmm', match3to4);
addRegexToken('hmmss', match5to6);
addRegexToken('Hmm', match3to4);
addRegexToken('Hmmss', match5to6);

addParseToken(['H', 'HH'], HOUR);
addParseToken(['k', 'kk'], function (input, array, config) {
    var kInput = toInt(input);
    array[HOUR] = kInput === 24 ? 0 : kInput;
});
addParseToken(['a', 'A'], function (input, array, config) {
    config._isPm = config._locale.isPM(input);
    config._meridiem = input;
});
addParseToken(['h', 'hh'], function (input, array, config) {
    array[HOUR] = toInt(input);
    getParsingFlags(config).bigHour = true;
});
addParseToken('hmm', function (input, array, config) {
    var pos = input.length - 2;
    array[HOUR] = toInt(input.substr(0, pos));
    array[MINUTE] = toInt(input.substr(pos));
    getParsingFlags(config).bigHour = true;
});
addParseToken('hmmss', function (input, array, config) {
    var pos1 = input.length - 4;
    var pos2 = input.length - 2;
    array[HOUR] = toInt(input.substr(0, pos1));
    array[MINUTE] = toInt(input.substr(pos1, 2));
    array[SECOND] = toInt(input.substr(pos2));
    getParsingFlags(config).bigHour = true;
});
addParseToken('Hmm', function (input, array, config) {
    var pos = input.length - 2;
    array[HOUR] = toInt(input.substr(0, pos));
    array[MINUTE] = toInt(input.substr(pos));
});
addParseToken('Hmmss', function (input, array, config) {
    var pos1 = input.length - 4;
    var pos2 = input.length - 2;
    array[HOUR] = toInt(input.substr(0, pos1));
    array[MINUTE] = toInt(input.substr(pos1, 2));
    array[SECOND] = toInt(input.substr(pos2));
});

// LOCALES

function localeIsPM (input) {
    // IE8 Quirks Mode & IE7 Standards Mode do not allow accessing strings like arrays
    // Using charAt should be more compatible.
    return ((input + '').toLowerCase().charAt(0) === 'p');
}

var defaultLocaleMeridiemParse = /[ap]\.?m?\.?/i;
function localeMeridiem (hours, minutes, isLower) {
    if (hours > 11) {
        return isLower ? 'pm' : 'PM';
    } else {
        return isLower ? 'am' : 'AM';
    }
}


// MOMENTS

// Setting the hour should keep the time, because the user explicitly
// specified which hour he wants. So trying to maintain the same hour (in
// a new timezone) makes sense. Adding/subtracting hours does not follow
// this rule.
var getSetHour = makeGetSet('Hours', true);

// months
// week
// weekdays
// meridiem
var baseConfig = {
    calendar: defaultCalendar,
    longDateFormat: defaultLongDateFormat,
    invalidDate: defaultInvalidDate,
    ordinal: defaultOrdinal,
    dayOfMonthOrdinalParse: defaultDayOfMonthOrdinalParse,
    relativeTime: defaultRelativeTime,

    months: defaultLocaleMonths,
    monthsShort: defaultLocaleMonthsShort,

    week: defaultLocaleWeek,

    weekdays: defaultLocaleWeekdays,
    weekdaysMin: defaultLocaleWeekdaysMin,
    weekdaysShort: defaultLocaleWeekdaysShort,

    meridiemParse: defaultLocaleMeridiemParse
};

// internal storage for locale config files
var locales = {};
var localeFamilies = {};
var globalLocale;

function normalizeLocale(key) {
    return key ? key.toLowerCase().replace('_', '-') : key;
}

// pick the locale from the array
// try ['en-au', 'en-gb'] as 'en-au', 'en-gb', 'en', as in move through the list trying each
// substring from most specific to least, but move to the next array item if it's a more specific variant than the current root
function chooseLocale(names) {
    var i = 0, j, next, locale, split;

    while (i < names.length) {
        split = normalizeLocale(names[i]).split('-');
        j = split.length;
        next = normalizeLocale(names[i + 1]);
        next = next ? next.split('-') : null;
        while (j > 0) {
            locale = loadLocale(split.slice(0, j).join('-'));
            if (locale) {
                return locale;
            }
            if (next && next.length >= j && compareArrays(split, next, true) >= j - 1) {
                //the next array item is better than a shallower substring of this one
                break;
            }
            j--;
        }
        i++;
    }
    return null;
}

function loadLocale(name) {
    var oldLocale = null;
    // TODO: Find a better way to register and load all the locales in Node
    if (!locales[name] && (typeof module !== 'undefined') &&
            module && module.exports) {
        try {
            oldLocale = globalLocale._abbr;
            var aliasedRequire = require;
            !(function webpackMissingModule() { var e = new Error("Cannot find module \"./locale\""); e.code = 'MODULE_NOT_FOUND'; throw e; }());
            getSetGlobalLocale(oldLocale);
        } catch (e) {}
    }
    return locales[name];
}

// This function will load locale and then set the global locale.  If
// no arguments are passed in, it will simply return the current global
// locale key.
function getSetGlobalLocale (key, values) {
    var data;
    if (key) {
        if (isUndefined(values)) {
            data = getLocale(key);
        }
        else {
            data = defineLocale(key, values);
        }

        if (data) {
            // moment.duration._locale = moment._locale = data;
            globalLocale = data;
        }
    }

    return globalLocale._abbr;
}

function defineLocale (name, config) {
    if (config !== null) {
        var parentConfig = baseConfig;
        config.abbr = name;
        if (locales[name] != null) {
            deprecateSimple('defineLocaleOverride',
                    'use moment.updateLocale(localeName, config) to change ' +
                    'an existing locale. moment.defineLocale(localeName, ' +
                    'config) should only be used for creating a new locale ' +
                    'See http://momentjs.com/guides/#/warnings/define-locale/ for more info.');
            parentConfig = locales[name]._config;
        } else if (config.parentLocale != null) {
            if (locales[config.parentLocale] != null) {
                parentConfig = locales[config.parentLocale]._config;
            } else {
                if (!localeFamilies[config.parentLocale]) {
                    localeFamilies[config.parentLocale] = [];
                }
                localeFamilies[config.parentLocale].push({
                    name: name,
                    config: config
                });
                return null;
            }
        }
        locales[name] = new Locale(mergeConfigs(parentConfig, config));

        if (localeFamilies[name]) {
            localeFamilies[name].forEach(function (x) {
                defineLocale(x.name, x.config);
            });
        }

        // backwards compat for now: also set the locale
        // make sure we set the locale AFTER all child locales have been
        // created, so we won't end up with the child locale set.
        getSetGlobalLocale(name);


        return locales[name];
    } else {
        // useful for testing
        delete locales[name];
        return null;
    }
}

function updateLocale(name, config) {
    if (config != null) {
        var locale, tmpLocale, parentConfig = baseConfig;
        // MERGE
        tmpLocale = loadLocale(name);
        if (tmpLocale != null) {
            parentConfig = tmpLocale._config;
        }
        config = mergeConfigs(parentConfig, config);
        locale = new Locale(config);
        locale.parentLocale = locales[name];
        locales[name] = locale;

        // backwards compat for now: also set the locale
        getSetGlobalLocale(name);
    } else {
        // pass null for config to unupdate, useful for tests
        if (locales[name] != null) {
            if (locales[name].parentLocale != null) {
                locales[name] = locales[name].parentLocale;
            } else if (locales[name] != null) {
                delete locales[name];
            }
        }
    }
    return locales[name];
}

// returns locale data
function getLocale (key) {
    var locale;

    if (key && key._locale && key._locale._abbr) {
        key = key._locale._abbr;
    }

    if (!key) {
        return globalLocale;
    }

    if (!isArray(key)) {
        //short-circuit everything else
        locale = loadLocale(key);
        if (locale) {
            return locale;
        }
        key = [key];
    }

    return chooseLocale(key);
}

function listLocales() {
    return keys(locales);
}

function checkOverflow (m) {
    var overflow;
    var a = m._a;

    if (a && getParsingFlags(m).overflow === -2) {
        overflow =
            a[MONTH]       < 0 || a[MONTH]       > 11  ? MONTH :
            a[DATE]        < 1 || a[DATE]        > daysInMonth(a[YEAR], a[MONTH]) ? DATE :
            a[HOUR]        < 0 || a[HOUR]        > 24 || (a[HOUR] === 24 && (a[MINUTE] !== 0 || a[SECOND] !== 0 || a[MILLISECOND] !== 0)) ? HOUR :
            a[MINUTE]      < 0 || a[MINUTE]      > 59  ? MINUTE :
            a[SECOND]      < 0 || a[SECOND]      > 59  ? SECOND :
            a[MILLISECOND] < 0 || a[MILLISECOND] > 999 ? MILLISECOND :
            -1;

        if (getParsingFlags(m)._overflowDayOfYear && (overflow < YEAR || overflow > DATE)) {
            overflow = DATE;
        }
        if (getParsingFlags(m)._overflowWeeks && overflow === -1) {
            overflow = WEEK;
        }
        if (getParsingFlags(m)._overflowWeekday && overflow === -1) {
            overflow = WEEKDAY;
        }

        getParsingFlags(m).overflow = overflow;
    }

    return m;
}

// Pick the first defined of two or three arguments.
function defaults(a, b, c) {
    if (a != null) {
        return a;
    }
    if (b != null) {
        return b;
    }
    return c;
}

function currentDateArray(config) {
    // hooks is actually the exported moment object
    var nowValue = new Date(hooks.now());
    if (config._useUTC) {
        return [nowValue.getUTCFullYear(), nowValue.getUTCMonth(), nowValue.getUTCDate()];
    }
    return [nowValue.getFullYear(), nowValue.getMonth(), nowValue.getDate()];
}

// convert an array to a date.
// the array should mirror the parameters below
// note: all values past the year are optional and will default to the lowest possible value.
// [year, month, day , hour, minute, second, millisecond]
function configFromArray (config) {
    var i, date, input = [], currentDate, expectedWeekday, yearToUse;

    if (config._d) {
        return;
    }

    currentDate = currentDateArray(config);

    //compute day of the year from weeks and weekdays
    if (config._w && config._a[DATE] == null && config._a[MONTH] == null) {
        dayOfYearFromWeekInfo(config);
    }

    //if the day of the year is set, figure out what it is
    if (config._dayOfYear != null) {
        yearToUse = defaults(config._a[YEAR], currentDate[YEAR]);

        if (config._dayOfYear > daysInYear(yearToUse) || config._dayOfYear === 0) {
            getParsingFlags(config)._overflowDayOfYear = true;
        }

        date = createUTCDate(yearToUse, 0, config._dayOfYear);
        config._a[MONTH] = date.getUTCMonth();
        config._a[DATE] = date.getUTCDate();
    }

    // Default to current date.
    // * if no year, month, day of month are given, default to today
    // * if day of month is given, default month and year
    // * if month is given, default only year
    // * if year is given, don't default anything
    for (i = 0; i < 3 && config._a[i] == null; ++i) {
        config._a[i] = input[i] = currentDate[i];
    }

    // Zero out whatever was not defaulted, including time
    for (; i < 7; i++) {
        config._a[i] = input[i] = (config._a[i] == null) ? (i === 2 ? 1 : 0) : config._a[i];
    }

    // Check for 24:00:00.000
    if (config._a[HOUR] === 24 &&
            config._a[MINUTE] === 0 &&
            config._a[SECOND] === 0 &&
            config._a[MILLISECOND] === 0) {
        config._nextDay = true;
        config._a[HOUR] = 0;
    }

    config._d = (config._useUTC ? createUTCDate : createDate).apply(null, input);
    expectedWeekday = config._useUTC ? config._d.getUTCDay() : config._d.getDay();

    // Apply timezone offset from input. The actual utcOffset can be changed
    // with parseZone.
    if (config._tzm != null) {
        config._d.setUTCMinutes(config._d.getUTCMinutes() - config._tzm);
    }

    if (config._nextDay) {
        config._a[HOUR] = 24;
    }

    // check for mismatching day of week
    if (config._w && typeof config._w.d !== 'undefined' && config._w.d !== expectedWeekday) {
        getParsingFlags(config).weekdayMismatch = true;
    }
}

function dayOfYearFromWeekInfo(config) {
    var w, weekYear, week, weekday, dow, doy, temp, weekdayOverflow;

    w = config._w;
    if (w.GG != null || w.W != null || w.E != null) {
        dow = 1;
        doy = 4;

        // TODO: We need to take the current isoWeekYear, but that depends on
        // how we interpret now (local, utc, fixed offset). So create
        // a now version of current config (take local/utc/offset flags, and
        // create now).
        weekYear = defaults(w.GG, config._a[YEAR], weekOfYear(createLocal(), 1, 4).year);
        week = defaults(w.W, 1);
        weekday = defaults(w.E, 1);
        if (weekday < 1 || weekday > 7) {
            weekdayOverflow = true;
        }
    } else {
        dow = config._locale._week.dow;
        doy = config._locale._week.doy;

        var curWeek = weekOfYear(createLocal(), dow, doy);

        weekYear = defaults(w.gg, config._a[YEAR], curWeek.year);

        // Default to current week.
        week = defaults(w.w, curWeek.week);

        if (w.d != null) {
            // weekday -- low day numbers are considered next week
            weekday = w.d;
            if (weekday < 0 || weekday > 6) {
                weekdayOverflow = true;
            }
        } else if (w.e != null) {
            // local weekday -- counting starts from begining of week
            weekday = w.e + dow;
            if (w.e < 0 || w.e > 6) {
                weekdayOverflow = true;
            }
        } else {
            // default to begining of week
            weekday = dow;
        }
    }
    if (week < 1 || week > weeksInYear(weekYear, dow, doy)) {
        getParsingFlags(config)._overflowWeeks = true;
    } else if (weekdayOverflow != null) {
        getParsingFlags(config)._overflowWeekday = true;
    } else {
        temp = dayOfYearFromWeeks(weekYear, week, weekday, dow, doy);
        config._a[YEAR] = temp.year;
        config._dayOfYear = temp.dayOfYear;
    }
}

// iso 8601 regex
// 0000-00-00 0000-W00 or 0000-W00-0 + T + 00 or 00:00 or 00:00:00 or 00:00:00.000 + +00:00 or +0000 or +00)
var extendedIsoRegex = /^\s*((?:[+-]\d{6}|\d{4})-(?:\d\d-\d\d|W\d\d-\d|W\d\d|\d\d\d|\d\d))(?:(T| )(\d\d(?::\d\d(?::\d\d(?:[.,]\d+)?)?)?)([\+\-]\d\d(?::?\d\d)?|\s*Z)?)?$/;
var basicIsoRegex = /^\s*((?:[+-]\d{6}|\d{4})(?:\d\d\d\d|W\d\d\d|W\d\d|\d\d\d|\d\d))(?:(T| )(\d\d(?:\d\d(?:\d\d(?:[.,]\d+)?)?)?)([\+\-]\d\d(?::?\d\d)?|\s*Z)?)?$/;

var tzRegex = /Z|[+-]\d\d(?::?\d\d)?/;

var isoDates = [
    ['YYYYYY-MM-DD', /[+-]\d{6}-\d\d-\d\d/],
    ['YYYY-MM-DD', /\d{4}-\d\d-\d\d/],
    ['GGGG-[W]WW-E', /\d{4}-W\d\d-\d/],
    ['GGGG-[W]WW', /\d{4}-W\d\d/, false],
    ['YYYY-DDD', /\d{4}-\d{3}/],
    ['YYYY-MM', /\d{4}-\d\d/, false],
    ['YYYYYYMMDD', /[+-]\d{10}/],
    ['YYYYMMDD', /\d{8}/],
    // YYYYMM is NOT allowed by the standard
    ['GGGG[W]WWE', /\d{4}W\d{3}/],
    ['GGGG[W]WW', /\d{4}W\d{2}/, false],
    ['YYYYDDD', /\d{7}/]
];

// iso time formats and regexes
var isoTimes = [
    ['HH:mm:ss.SSSS', /\d\d:\d\d:\d\d\.\d+/],
    ['HH:mm:ss,SSSS', /\d\d:\d\d:\d\d,\d+/],
    ['HH:mm:ss', /\d\d:\d\d:\d\d/],
    ['HH:mm', /\d\d:\d\d/],
    ['HHmmss.SSSS', /\d\d\d\d\d\d\.\d+/],
    ['HHmmss,SSSS', /\d\d\d\d\d\d,\d+/],
    ['HHmmss', /\d\d\d\d\d\d/],
    ['HHmm', /\d\d\d\d/],
    ['HH', /\d\d/]
];

var aspNetJsonRegex = /^\/?Date\((\-?\d+)/i;

// date from iso format
function configFromISO(config) {
    var i, l,
        string = config._i,
        match = extendedIsoRegex.exec(string) || basicIsoRegex.exec(string),
        allowTime, dateFormat, timeFormat, tzFormat;

    if (match) {
        getParsingFlags(config).iso = true;

        for (i = 0, l = isoDates.length; i < l; i++) {
            if (isoDates[i][1].exec(match[1])) {
                dateFormat = isoDates[i][0];
                allowTime = isoDates[i][2] !== false;
                break;
            }
        }
        if (dateFormat == null) {
            config._isValid = false;
            return;
        }
        if (match[3]) {
            for (i = 0, l = isoTimes.length; i < l; i++) {
                if (isoTimes[i][1].exec(match[3])) {
                    // match[2] should be 'T' or space
                    timeFormat = (match[2] || ' ') + isoTimes[i][0];
                    break;
                }
            }
            if (timeFormat == null) {
                config._isValid = false;
                return;
            }
        }
        if (!allowTime && timeFormat != null) {
            config._isValid = false;
            return;
        }
        if (match[4]) {
            if (tzRegex.exec(match[4])) {
                tzFormat = 'Z';
            } else {
                config._isValid = false;
                return;
            }
        }
        config._f = dateFormat + (timeFormat || '') + (tzFormat || '');
        configFromStringAndFormat(config);
    } else {
        config._isValid = false;
    }
}

// RFC 2822 regex: For details see https://tools.ietf.org/html/rfc2822#section-3.3
var rfc2822 = /^(?:(Mon|Tue|Wed|Thu|Fri|Sat|Sun),?\s)?(\d{1,2})\s(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s(\d{2,4})\s(\d\d):(\d\d)(?::(\d\d))?\s(?:(UT|GMT|[ECMP][SD]T)|([Zz])|([+-]\d{4}))$/;

function extractFromRFC2822Strings(yearStr, monthStr, dayStr, hourStr, minuteStr, secondStr) {
    var result = [
        untruncateYear(yearStr),
        defaultLocaleMonthsShort.indexOf(monthStr),
        parseInt(dayStr, 10),
        parseInt(hourStr, 10),
        parseInt(minuteStr, 10)
    ];

    if (secondStr) {
        result.push(parseInt(secondStr, 10));
    }

    return result;
}

function untruncateYear(yearStr) {
    var year = parseInt(yearStr, 10);
    if (year <= 49) {
        return 2000 + year;
    } else if (year <= 999) {
        return 1900 + year;
    }
    return year;
}

function preprocessRFC2822(s) {
    // Remove comments and folding whitespace and replace multiple-spaces with a single space
    return s.replace(/\([^)]*\)|[\n\t]/g, ' ').replace(/(\s\s+)/g, ' ').trim();
}

function checkWeekday(weekdayStr, parsedInput, config) {
    if (weekdayStr) {
        // TODO: Replace the vanilla JS Date object with an indepentent day-of-week check.
        var weekdayProvided = defaultLocaleWeekdaysShort.indexOf(weekdayStr),
            weekdayActual = new Date(parsedInput[0], parsedInput[1], parsedInput[2]).getDay();
        if (weekdayProvided !== weekdayActual) {
            getParsingFlags(config).weekdayMismatch = true;
            config._isValid = false;
            return false;
        }
    }
    return true;
}

var obsOffsets = {
    UT: 0,
    GMT: 0,
    EDT: -4 * 60,
    EST: -5 * 60,
    CDT: -5 * 60,
    CST: -6 * 60,
    MDT: -6 * 60,
    MST: -7 * 60,
    PDT: -7 * 60,
    PST: -8 * 60
};

function calculateOffset(obsOffset, militaryOffset, numOffset) {
    if (obsOffset) {
        return obsOffsets[obsOffset];
    } else if (militaryOffset) {
        // the only allowed military tz is Z
        return 0;
    } else {
        var hm = parseInt(numOffset, 10);
        var m = hm % 100, h = (hm - m) / 100;
        return h * 60 + m;
    }
}

// date and time from ref 2822 format
function configFromRFC2822(config) {
    var match = rfc2822.exec(preprocessRFC2822(config._i));
    if (match) {
        var parsedArray = extractFromRFC2822Strings(match[4], match[3], match[2], match[5], match[6], match[7]);
        if (!checkWeekday(match[1], parsedArray, config)) {
            return;
        }

        config._a = parsedArray;
        config._tzm = calculateOffset(match[8], match[9], match[10]);

        config._d = createUTCDate.apply(null, config._a);
        config._d.setUTCMinutes(config._d.getUTCMinutes() - config._tzm);

        getParsingFlags(config).rfc2822 = true;
    } else {
        config._isValid = false;
    }
}

// date from iso format or fallback
function configFromString(config) {
    var matched = aspNetJsonRegex.exec(config._i);

    if (matched !== null) {
        config._d = new Date(+matched[1]);
        return;
    }

    configFromISO(config);
    if (config._isValid === false) {
        delete config._isValid;
    } else {
        return;
    }

    configFromRFC2822(config);
    if (config._isValid === false) {
        delete config._isValid;
    } else {
        return;
    }

    // Final attempt, use Input Fallback
    hooks.createFromInputFallback(config);
}

hooks.createFromInputFallback = deprecate(
    'value provided is not in a recognized RFC2822 or ISO format. moment construction falls back to js Date(), ' +
    'which is not reliable across all browsers and versions. Non RFC2822/ISO date formats are ' +
    'discouraged and will be removed in an upcoming major release. Please refer to ' +
    'http://momentjs.com/guides/#/warnings/js-date/ for more info.',
    function (config) {
        config._d = new Date(config._i + (config._useUTC ? ' UTC' : ''));
    }
);

// constant that refers to the ISO standard
hooks.ISO_8601 = function () {};

// constant that refers to the RFC 2822 form
hooks.RFC_2822 = function () {};

// date from string and format string
function configFromStringAndFormat(config) {
    // TODO: Move this to another part of the creation flow to prevent circular deps
    if (config._f === hooks.ISO_8601) {
        configFromISO(config);
        return;
    }
    if (config._f === hooks.RFC_2822) {
        configFromRFC2822(config);
        return;
    }
    config._a = [];
    getParsingFlags(config).empty = true;

    // This array is used to make a Date, either with `new Date` or `Date.UTC`
    var string = '' + config._i,
        i, parsedInput, tokens, token, skipped,
        stringLength = string.length,
        totalParsedInputLength = 0;

    tokens = expandFormat(config._f, config._locale).match(formattingTokens) || [];

    for (i = 0; i < tokens.length; i++) {
        token = tokens[i];
        parsedInput = (string.match(getParseRegexForToken(token, config)) || [])[0];
        // console.log('token', token, 'parsedInput', parsedInput,
        //         'regex', getParseRegexForToken(token, config));
        if (parsedInput) {
            skipped = string.substr(0, string.indexOf(parsedInput));
            if (skipped.length > 0) {
                getParsingFlags(config).unusedInput.push(skipped);
            }
            string = string.slice(string.indexOf(parsedInput) + parsedInput.length);
            totalParsedInputLength += parsedInput.length;
        }
        // don't parse if it's not a known token
        if (formatTokenFunctions[token]) {
            if (parsedInput) {
                getParsingFlags(config).empty = false;
            }
            else {
                getParsingFlags(config).unusedTokens.push(token);
            }
            addTimeToArrayFromToken(token, parsedInput, config);
        }
        else if (config._strict && !parsedInput) {
            getParsingFlags(config).unusedTokens.push(token);
        }
    }

    // add remaining unparsed input length to the string
    getParsingFlags(config).charsLeftOver = stringLength - totalParsedInputLength;
    if (string.length > 0) {
        getParsingFlags(config).unusedInput.push(string);
    }

    // clear _12h flag if hour is <= 12
    if (config._a[HOUR] <= 12 &&
        getParsingFlags(config).bigHour === true &&
        config._a[HOUR] > 0) {
        getParsingFlags(config).bigHour = undefined;
    }

    getParsingFlags(config).parsedDateParts = config._a.slice(0);
    getParsingFlags(config).meridiem = config._meridiem;
    // handle meridiem
    config._a[HOUR] = meridiemFixWrap(config._locale, config._a[HOUR], config._meridiem);

    configFromArray(config);
    checkOverflow(config);
}


function meridiemFixWrap (locale, hour, meridiem) {
    var isPm;

    if (meridiem == null) {
        // nothing to do
        return hour;
    }
    if (locale.meridiemHour != null) {
        return locale.meridiemHour(hour, meridiem);
    } else if (locale.isPM != null) {
        // Fallback
        isPm = locale.isPM(meridiem);
        if (isPm && hour < 12) {
            hour += 12;
        }
        if (!isPm && hour === 12) {
            hour = 0;
        }
        return hour;
    } else {
        // this is not supposed to happen
        return hour;
    }
}

// date from string and array of format strings
function configFromStringAndArray(config) {
    var tempConfig,
        bestMoment,

        scoreToBeat,
        i,
        currentScore;

    if (config._f.length === 0) {
        getParsingFlags(config).invalidFormat = true;
        config._d = new Date(NaN);
        return;
    }

    for (i = 0; i < config._f.length; i++) {
        currentScore = 0;
        tempConfig = copyConfig({}, config);
        if (config._useUTC != null) {
            tempConfig._useUTC = config._useUTC;
        }
        tempConfig._f = config._f[i];
        configFromStringAndFormat(tempConfig);

        if (!isValid(tempConfig)) {
            continue;
        }

        // if there is any input that was not parsed add a penalty for that format
        currentScore += getParsingFlags(tempConfig).charsLeftOver;

        //or tokens
        currentScore += getParsingFlags(tempConfig).unusedTokens.length * 10;

        getParsingFlags(tempConfig).score = currentScore;

        if (scoreToBeat == null || currentScore < scoreToBeat) {
            scoreToBeat = currentScore;
            bestMoment = tempConfig;
        }
    }

    extend(config, bestMoment || tempConfig);
}

function configFromObject(config) {
    if (config._d) {
        return;
    }

    var i = normalizeObjectUnits(config._i);
    config._a = map([i.year, i.month, i.day || i.date, i.hour, i.minute, i.second, i.millisecond], function (obj) {
        return obj && parseInt(obj, 10);
    });

    configFromArray(config);
}

function createFromConfig (config) {
    var res = new Moment(checkOverflow(prepareConfig(config)));
    if (res._nextDay) {
        // Adding is smart enough around DST
        res.add(1, 'd');
        res._nextDay = undefined;
    }

    return res;
}

function prepareConfig (config) {
    var input = config._i,
        format = config._f;

    config._locale = config._locale || getLocale(config._l);

    if (input === null || (format === undefined && input === '')) {
        return createInvalid({nullInput: true});
    }

    if (typeof input === 'string') {
        config._i = input = config._locale.preparse(input);
    }

    if (isMoment(input)) {
        return new Moment(checkOverflow(input));
    } else if (isDate(input)) {
        config._d = input;
    } else if (isArray(format)) {
        configFromStringAndArray(config);
    } else if (format) {
        configFromStringAndFormat(config);
    }  else {
        configFromInput(config);
    }

    if (!isValid(config)) {
        config._d = null;
    }

    return config;
}

function configFromInput(config) {
    var input = config._i;
    if (isUndefined(input)) {
        config._d = new Date(hooks.now());
    } else if (isDate(input)) {
        config._d = new Date(input.valueOf());
    } else if (typeof input === 'string') {
        configFromString(config);
    } else if (isArray(input)) {
        config._a = map(input.slice(0), function (obj) {
            return parseInt(obj, 10);
        });
        configFromArray(config);
    } else if (isObject(input)) {
        configFromObject(config);
    } else if (isNumber(input)) {
        // from milliseconds
        config._d = new Date(input);
    } else {
        hooks.createFromInputFallback(config);
    }
}

function createLocalOrUTC (input, format, locale, strict, isUTC) {
    var c = {};

    if (locale === true || locale === false) {
        strict = locale;
        locale = undefined;
    }

    if ((isObject(input) && isObjectEmpty(input)) ||
            (isArray(input) && input.length === 0)) {
        input = undefined;
    }
    // object construction must be done this way.
    // https://github.com/moment/moment/issues/1423
    c._isAMomentObject = true;
    c._useUTC = c._isUTC = isUTC;
    c._l = locale;
    c._i = input;
    c._f = format;
    c._strict = strict;

    return createFromConfig(c);
}

function createLocal (input, format, locale, strict) {
    return createLocalOrUTC(input, format, locale, strict, false);
}

var prototypeMin = deprecate(
    'moment().min is deprecated, use moment.max instead. http://momentjs.com/guides/#/warnings/min-max/',
    function () {
        var other = createLocal.apply(null, arguments);
        if (this.isValid() && other.isValid()) {
            return other < this ? this : other;
        } else {
            return createInvalid();
        }
    }
);

var prototypeMax = deprecate(
    'moment().max is deprecated, use moment.min instead. http://momentjs.com/guides/#/warnings/min-max/',
    function () {
        var other = createLocal.apply(null, arguments);
        if (this.isValid() && other.isValid()) {
            return other > this ? this : other;
        } else {
            return createInvalid();
        }
    }
);

// Pick a moment m from moments so that m[fn](other) is true for all
// other. This relies on the function fn to be transitive.
//
// moments should either be an array of moment objects or an array, whose
// first element is an array of moment objects.
function pickBy(fn, moments) {
    var res, i;
    if (moments.length === 1 && isArray(moments[0])) {
        moments = moments[0];
    }
    if (!moments.length) {
        return createLocal();
    }
    res = moments[0];
    for (i = 1; i < moments.length; ++i) {
        if (!moments[i].isValid() || moments[i][fn](res)) {
            res = moments[i];
        }
    }
    return res;
}

// TODO: Use [].sort instead?
function min () {
    var args = [].slice.call(arguments, 0);

    return pickBy('isBefore', args);
}

function max () {
    var args = [].slice.call(arguments, 0);

    return pickBy('isAfter', args);
}

var now = function () {
    return Date.now ? Date.now() : +(new Date());
};

var ordering = ['year', 'quarter', 'month', 'week', 'day', 'hour', 'minute', 'second', 'millisecond'];

function isDurationValid(m) {
    for (var key in m) {
        if (!(indexOf.call(ordering, key) !== -1 && (m[key] == null || !isNaN(m[key])))) {
            return false;
        }
    }

    var unitHasDecimal = false;
    for (var i = 0; i < ordering.length; ++i) {
        if (m[ordering[i]]) {
            if (unitHasDecimal) {
                return false; // only allow non-integers for smallest unit
            }
            if (parseFloat(m[ordering[i]]) !== toInt(m[ordering[i]])) {
                unitHasDecimal = true;
            }
        }
    }

    return true;
}

function isValid$1() {
    return this._isValid;
}

function createInvalid$1() {
    return createDuration(NaN);
}

function Duration (duration) {
    var normalizedInput = normalizeObjectUnits(duration),
        years = normalizedInput.year || 0,
        quarters = normalizedInput.quarter || 0,
        months = normalizedInput.month || 0,
        weeks = normalizedInput.week || 0,
        days = normalizedInput.day || 0,
        hours = normalizedInput.hour || 0,
        minutes = normalizedInput.minute || 0,
        seconds = normalizedInput.second || 0,
        milliseconds = normalizedInput.millisecond || 0;

    this._isValid = isDurationValid(normalizedInput);

    // representation for dateAddRemove
    this._milliseconds = +milliseconds +
        seconds * 1e3 + // 1000
        minutes * 6e4 + // 1000 * 60
        hours * 1000 * 60 * 60; //using 1000 * 60 * 60 instead of 36e5 to avoid floating point rounding errors https://github.com/moment/moment/issues/2978
    // Because of dateAddRemove treats 24 hours as different from a
    // day when working around DST, we need to store them separately
    this._days = +days +
        weeks * 7;
    // It is impossible to translate months into days without knowing
    // which months you are are talking about, so we have to store
    // it separately.
    this._months = +months +
        quarters * 3 +
        years * 12;

    this._data = {};

    this._locale = getLocale();

    this._bubble();
}

function isDuration (obj) {
    return obj instanceof Duration;
}

function absRound (number) {
    if (number < 0) {
        return Math.round(-1 * number) * -1;
    } else {
        return Math.round(number);
    }
}

// FORMATTING

function offset (token, separator) {
    addFormatToken(token, 0, 0, function () {
        var offset = this.utcOffset();
        var sign = '+';
        if (offset < 0) {
            offset = -offset;
            sign = '-';
        }
        return sign + zeroFill(~~(offset / 60), 2) + separator + zeroFill(~~(offset) % 60, 2);
    });
}

offset('Z', ':');
offset('ZZ', '');

// PARSING

addRegexToken('Z',  matchShortOffset);
addRegexToken('ZZ', matchShortOffset);
addParseToken(['Z', 'ZZ'], function (input, array, config) {
    config._useUTC = true;
    config._tzm = offsetFromString(matchShortOffset, input);
});

// HELPERS

// timezone chunker
// '+10:00' > ['10',  '00']
// '-1530'  > ['-15', '30']
var chunkOffset = /([\+\-]|\d\d)/gi;

function offsetFromString(matcher, string) {
    var matches = (string || '').match(matcher);

    if (matches === null) {
        return null;
    }

    var chunk   = matches[matches.length - 1] || [];
    var parts   = (chunk + '').match(chunkOffset) || ['-', 0, 0];
    var minutes = +(parts[1] * 60) + toInt(parts[2]);

    return minutes === 0 ?
      0 :
      parts[0] === '+' ? minutes : -minutes;
}

// Return a moment from input, that is local/utc/zone equivalent to model.
function cloneWithOffset(input, model) {
    var res, diff;
    if (model._isUTC) {
        res = model.clone();
        diff = (isMoment(input) || isDate(input) ? input.valueOf() : createLocal(input).valueOf()) - res.valueOf();
        // Use low-level api, because this fn is low-level api.
        res._d.setTime(res._d.valueOf() + diff);
        hooks.updateOffset(res, false);
        return res;
    } else {
        return createLocal(input).local();
    }
}

function getDateOffset (m) {
    // On Firefox.24 Date#getTimezoneOffset returns a floating point.
    // https://github.com/moment/moment/pull/1871
    return -Math.round(m._d.getTimezoneOffset() / 15) * 15;
}

// HOOKS

// This function will be called whenever a moment is mutated.
// It is intended to keep the offset in sync with the timezone.
hooks.updateOffset = function () {};

// MOMENTS

// keepLocalTime = true means only change the timezone, without
// affecting the local hour. So 5:31:26 +0300 --[utcOffset(2, true)]-->
// 5:31:26 +0200 It is possible that 5:31:26 doesn't exist with offset
// +0200, so we adjust the time as needed, to be valid.
//
// Keeping the time actually adds/subtracts (one hour)
// from the actual represented time. That is why we call updateOffset
// a second time. In case it wants us to change the offset again
// _changeInProgress == true case, then we have to adjust, because
// there is no such time in the given timezone.
function getSetOffset (input, keepLocalTime, keepMinutes) {
    var offset = this._offset || 0,
        localAdjust;
    if (!this.isValid()) {
        return input != null ? this : NaN;
    }
    if (input != null) {
        if (typeof input === 'string') {
            input = offsetFromString(matchShortOffset, input);
            if (input === null) {
                return this;
            }
        } else if (Math.abs(input) < 16 && !keepMinutes) {
            input = input * 60;
        }
        if (!this._isUTC && keepLocalTime) {
            localAdjust = getDateOffset(this);
        }
        this._offset = input;
        this._isUTC = true;
        if (localAdjust != null) {
            this.add(localAdjust, 'm');
        }
        if (offset !== input) {
            if (!keepLocalTime || this._changeInProgress) {
                addSubtract(this, createDuration(input - offset, 'm'), 1, false);
            } else if (!this._changeInProgress) {
                this._changeInProgress = true;
                hooks.updateOffset(this, true);
                this._changeInProgress = null;
            }
        }
        return this;
    } else {
        return this._isUTC ? offset : getDateOffset(this);
    }
}

function getSetZone (input, keepLocalTime) {
    if (input != null) {
        if (typeof input !== 'string') {
            input = -input;
        }

        this.utcOffset(input, keepLocalTime);

        return this;
    } else {
        return -this.utcOffset();
    }
}

function setOffsetToUTC (keepLocalTime) {
    return this.utcOffset(0, keepLocalTime);
}

function setOffsetToLocal (keepLocalTime) {
    if (this._isUTC) {
        this.utcOffset(0, keepLocalTime);
        this._isUTC = false;

        if (keepLocalTime) {
            this.subtract(getDateOffset(this), 'm');
        }
    }
    return this;
}

function setOffsetToParsedOffset () {
    if (this._tzm != null) {
        this.utcOffset(this._tzm, false, true);
    } else if (typeof this._i === 'string') {
        var tZone = offsetFromString(matchOffset, this._i);
        if (tZone != null) {
            this.utcOffset(tZone);
        }
        else {
            this.utcOffset(0, true);
        }
    }
    return this;
}

function hasAlignedHourOffset (input) {
    if (!this.isValid()) {
        return false;
    }
    input = input ? createLocal(input).utcOffset() : 0;

    return (this.utcOffset() - input) % 60 === 0;
}

function isDaylightSavingTime () {
    return (
        this.utcOffset() > this.clone().month(0).utcOffset() ||
        this.utcOffset() > this.clone().month(5).utcOffset()
    );
}

function isDaylightSavingTimeShifted () {
    if (!isUndefined(this._isDSTShifted)) {
        return this._isDSTShifted;
    }

    var c = {};

    copyConfig(c, this);
    c = prepareConfig(c);

    if (c._a) {
        var other = c._isUTC ? createUTC(c._a) : createLocal(c._a);
        this._isDSTShifted = this.isValid() &&
            compareArrays(c._a, other.toArray()) > 0;
    } else {
        this._isDSTShifted = false;
    }

    return this._isDSTShifted;
}

function isLocal () {
    return this.isValid() ? !this._isUTC : false;
}

function isUtcOffset () {
    return this.isValid() ? this._isUTC : false;
}

function isUtc () {
    return this.isValid() ? this._isUTC && this._offset === 0 : false;
}

// ASP.NET json date format regex
var aspNetRegex = /^(\-|\+)?(?:(\d*)[. ])?(\d+)\:(\d+)(?:\:(\d+)(\.\d*)?)?$/;

// from http://docs.closure-library.googlecode.com/git/closure_goog_date_date.js.source.html
// somewhat more in line with 4.4.3.2 2004 spec, but allows decimal anywhere
// and further modified to allow for strings containing both week and day
var isoRegex = /^(-|\+)?P(?:([-+]?[0-9,.]*)Y)?(?:([-+]?[0-9,.]*)M)?(?:([-+]?[0-9,.]*)W)?(?:([-+]?[0-9,.]*)D)?(?:T(?:([-+]?[0-9,.]*)H)?(?:([-+]?[0-9,.]*)M)?(?:([-+]?[0-9,.]*)S)?)?$/;

function createDuration (input, key) {
    var duration = input,
        // matching against regexp is expensive, do it on demand
        match = null,
        sign,
        ret,
        diffRes;

    if (isDuration(input)) {
        duration = {
            ms : input._milliseconds,
            d  : input._days,
            M  : input._months
        };
    } else if (isNumber(input)) {
        duration = {};
        if (key) {
            duration[key] = input;
        } else {
            duration.milliseconds = input;
        }
    } else if (!!(match = aspNetRegex.exec(input))) {
        sign = (match[1] === '-') ? -1 : 1;
        duration = {
            y  : 0,
            d  : toInt(match[DATE])                         * sign,
            h  : toInt(match[HOUR])                         * sign,
            m  : toInt(match[MINUTE])                       * sign,
            s  : toInt(match[SECOND])                       * sign,
            ms : toInt(absRound(match[MILLISECOND] * 1000)) * sign // the millisecond decimal point is included in the match
        };
    } else if (!!(match = isoRegex.exec(input))) {
        sign = (match[1] === '-') ? -1 : (match[1] === '+') ? 1 : 1;
        duration = {
            y : parseIso(match[2], sign),
            M : parseIso(match[3], sign),
            w : parseIso(match[4], sign),
            d : parseIso(match[5], sign),
            h : parseIso(match[6], sign),
            m : parseIso(match[7], sign),
            s : parseIso(match[8], sign)
        };
    } else if (duration == null) {// checks for null or undefined
        duration = {};
    } else if (typeof duration === 'object' && ('from' in duration || 'to' in duration)) {
        diffRes = momentsDifference(createLocal(duration.from), createLocal(duration.to));

        duration = {};
        duration.ms = diffRes.milliseconds;
        duration.M = diffRes.months;
    }

    ret = new Duration(duration);

    if (isDuration(input) && hasOwnProp(input, '_locale')) {
        ret._locale = input._locale;
    }

    return ret;
}

createDuration.fn = Duration.prototype;
createDuration.invalid = createInvalid$1;

function parseIso (inp, sign) {
    // We'd normally use ~~inp for this, but unfortunately it also
    // converts floats to ints.
    // inp may be undefined, so careful calling replace on it.
    var res = inp && parseFloat(inp.replace(',', '.'));
    // apply sign while we're at it
    return (isNaN(res) ? 0 : res) * sign;
}

function positiveMomentsDifference(base, other) {
    var res = {milliseconds: 0, months: 0};

    res.months = other.month() - base.month() +
        (other.year() - base.year()) * 12;
    if (base.clone().add(res.months, 'M').isAfter(other)) {
        --res.months;
    }

    res.milliseconds = +other - +(base.clone().add(res.months, 'M'));

    return res;
}

function momentsDifference(base, other) {
    var res;
    if (!(base.isValid() && other.isValid())) {
        return {milliseconds: 0, months: 0};
    }

    other = cloneWithOffset(other, base);
    if (base.isBefore(other)) {
        res = positiveMomentsDifference(base, other);
    } else {
        res = positiveMomentsDifference(other, base);
        res.milliseconds = -res.milliseconds;
        res.months = -res.months;
    }

    return res;
}

// TODO: remove 'name' arg after deprecation is removed
function createAdder(direction, name) {
    return function (val, period) {
        var dur, tmp;
        //invert the arguments, but complain about it
        if (period !== null && !isNaN(+period)) {
            deprecateSimple(name, 'moment().' + name  + '(period, number) is deprecated. Please use moment().' + name + '(number, period). ' +
            'See http://momentjs.com/guides/#/warnings/add-inverted-param/ for more info.');
            tmp = val; val = period; period = tmp;
        }

        val = typeof val === 'string' ? +val : val;
        dur = createDuration(val, period);
        addSubtract(this, dur, direction);
        return this;
    };
}

function addSubtract (mom, duration, isAdding, updateOffset) {
    var milliseconds = duration._milliseconds,
        days = absRound(duration._days),
        months = absRound(duration._months);

    if (!mom.isValid()) {
        // No op
        return;
    }

    updateOffset = updateOffset == null ? true : updateOffset;

    if (months) {
        setMonth(mom, get(mom, 'Month') + months * isAdding);
    }
    if (days) {
        set$1(mom, 'Date', get(mom, 'Date') + days * isAdding);
    }
    if (milliseconds) {
        mom._d.setTime(mom._d.valueOf() + milliseconds * isAdding);
    }
    if (updateOffset) {
        hooks.updateOffset(mom, days || months);
    }
}

var add      = createAdder(1, 'add');
var subtract = createAdder(-1, 'subtract');

function getCalendarFormat(myMoment, now) {
    var diff = myMoment.diff(now, 'days', true);
    return diff < -6 ? 'sameElse' :
            diff < -1 ? 'lastWeek' :
            diff < 0 ? 'lastDay' :
            diff < 1 ? 'sameDay' :
            diff < 2 ? 'nextDay' :
            diff < 7 ? 'nextWeek' : 'sameElse';
}

function calendar$1 (time, formats) {
    // We want to compare the start of today, vs this.
    // Getting start-of-today depends on whether we're local/utc/offset or not.
    var now = time || createLocal(),
        sod = cloneWithOffset(now, this).startOf('day'),
        format = hooks.calendarFormat(this, sod) || 'sameElse';

    var output = formats && (isFunction(formats[format]) ? formats[format].call(this, now) : formats[format]);

    return this.format(output || this.localeData().calendar(format, this, createLocal(now)));
}

function clone () {
    return new Moment(this);
}

function isAfter (input, units) {
    var localInput = isMoment(input) ? input : createLocal(input);
    if (!(this.isValid() && localInput.isValid())) {
        return false;
    }
    units = normalizeUnits(!isUndefined(units) ? units : 'millisecond');
    if (units === 'millisecond') {
        return this.valueOf() > localInput.valueOf();
    } else {
        return localInput.valueOf() < this.clone().startOf(units).valueOf();
    }
}

function isBefore (input, units) {
    var localInput = isMoment(input) ? input : createLocal(input);
    if (!(this.isValid() && localInput.isValid())) {
        return false;
    }
    units = normalizeUnits(!isUndefined(units) ? units : 'millisecond');
    if (units === 'millisecond') {
        return this.valueOf() < localInput.valueOf();
    } else {
        return this.clone().endOf(units).valueOf() < localInput.valueOf();
    }
}

function isBetween (from, to, units, inclusivity) {
    inclusivity = inclusivity || '()';
    return (inclusivity[0] === '(' ? this.isAfter(from, units) : !this.isBefore(from, units)) &&
        (inclusivity[1] === ')' ? this.isBefore(to, units) : !this.isAfter(to, units));
}

function isSame (input, units) {
    var localInput = isMoment(input) ? input : createLocal(input),
        inputMs;
    if (!(this.isValid() && localInput.isValid())) {
        return false;
    }
    units = normalizeUnits(units || 'millisecond');
    if (units === 'millisecond') {
        return this.valueOf() === localInput.valueOf();
    } else {
        inputMs = localInput.valueOf();
        return this.clone().startOf(units).valueOf() <= inputMs && inputMs <= this.clone().endOf(units).valueOf();
    }
}

function isSameOrAfter (input, units) {
    return this.isSame(input, units) || this.isAfter(input,units);
}

function isSameOrBefore (input, units) {
    return this.isSame(input, units) || this.isBefore(input,units);
}

function diff (input, units, asFloat) {
    var that,
        zoneDelta,
        delta, output;

    if (!this.isValid()) {
        return NaN;
    }

    that = cloneWithOffset(input, this);

    if (!that.isValid()) {
        return NaN;
    }

    zoneDelta = (that.utcOffset() - this.utcOffset()) * 6e4;

    units = normalizeUnits(units);

    switch (units) {
        case 'year': output = monthDiff(this, that) / 12; break;
        case 'month': output = monthDiff(this, that); break;
        case 'quarter': output = monthDiff(this, that) / 3; break;
        case 'second': output = (this - that) / 1e3; break; // 1000
        case 'minute': output = (this - that) / 6e4; break; // 1000 * 60
        case 'hour': output = (this - that) / 36e5; break; // 1000 * 60 * 60
        case 'day': output = (this - that - zoneDelta) / 864e5; break; // 1000 * 60 * 60 * 24, negate dst
        case 'week': output = (this - that - zoneDelta) / 6048e5; break; // 1000 * 60 * 60 * 24 * 7, negate dst
        default: output = this - that;
    }

    return asFloat ? output : absFloor(output);
}

function monthDiff (a, b) {
    // difference in months
    var wholeMonthDiff = ((b.year() - a.year()) * 12) + (b.month() - a.month()),
        // b is in (anchor - 1 month, anchor + 1 month)
        anchor = a.clone().add(wholeMonthDiff, 'months'),
        anchor2, adjust;

    if (b - anchor < 0) {
        anchor2 = a.clone().add(wholeMonthDiff - 1, 'months');
        // linear across the month
        adjust = (b - anchor) / (anchor - anchor2);
    } else {
        anchor2 = a.clone().add(wholeMonthDiff + 1, 'months');
        // linear across the month
        adjust = (b - anchor) / (anchor2 - anchor);
    }

    //check for negative zero, return zero if negative zero
    return -(wholeMonthDiff + adjust) || 0;
}

hooks.defaultFormat = 'YYYY-MM-DDTHH:mm:ssZ';
hooks.defaultFormatUtc = 'YYYY-MM-DDTHH:mm:ss[Z]';

function toString () {
    return this.clone().locale('en').format('ddd MMM DD YYYY HH:mm:ss [GMT]ZZ');
}

function toISOString(keepOffset) {
    if (!this.isValid()) {
        return null;
    }
    var utc = keepOffset !== true;
    var m = utc ? this.clone().utc() : this;
    if (m.year() < 0 || m.year() > 9999) {
        return formatMoment(m, utc ? 'YYYYYY-MM-DD[T]HH:mm:ss.SSS[Z]' : 'YYYYYY-MM-DD[T]HH:mm:ss.SSSZ');
    }
    if (isFunction(Date.prototype.toISOString)) {
        // native implementation is ~50x faster, use it when we can
        if (utc) {
            return this.toDate().toISOString();
        } else {
            return new Date(this._d.valueOf()).toISOString().replace('Z', formatMoment(m, 'Z'));
        }
    }
    return formatMoment(m, utc ? 'YYYY-MM-DD[T]HH:mm:ss.SSS[Z]' : 'YYYY-MM-DD[T]HH:mm:ss.SSSZ');
}

/**
 * Return a human readable representation of a moment that can
 * also be evaluated to get a new moment which is the same
 *
 * @link https://nodejs.org/dist/latest/docs/api/util.html#util_custom_inspect_function_on_objects
 */
function inspect () {
    if (!this.isValid()) {
        return 'moment.invalid(/* ' + this._i + ' */)';
    }
    var func = 'moment';
    var zone = '';
    if (!this.isLocal()) {
        func = this.utcOffset() === 0 ? 'moment.utc' : 'moment.parseZone';
        zone = 'Z';
    }
    var prefix = '[' + func + '("]';
    var year = (0 <= this.year() && this.year() <= 9999) ? 'YYYY' : 'YYYYYY';
    var datetime = '-MM-DD[T]HH:mm:ss.SSS';
    var suffix = zone + '[")]';

    return this.format(prefix + year + datetime + suffix);
}

function format (inputString) {
    if (!inputString) {
        inputString = this.isUtc() ? hooks.defaultFormatUtc : hooks.defaultFormat;
    }
    var output = formatMoment(this, inputString);
    return this.localeData().postformat(output);
}

function from (time, withoutSuffix) {
    if (this.isValid() &&
            ((isMoment(time) && time.isValid()) ||
             createLocal(time).isValid())) {
        return createDuration({to: this, from: time}).locale(this.locale()).humanize(!withoutSuffix);
    } else {
        return this.localeData().invalidDate();
    }
}

function fromNow (withoutSuffix) {
    return this.from(createLocal(), withoutSuffix);
}

function to (time, withoutSuffix) {
    if (this.isValid() &&
            ((isMoment(time) && time.isValid()) ||
             createLocal(time).isValid())) {
        return createDuration({from: this, to: time}).locale(this.locale()).humanize(!withoutSuffix);
    } else {
        return this.localeData().invalidDate();
    }
}

function toNow (withoutSuffix) {
    return this.to(createLocal(), withoutSuffix);
}

// If passed a locale key, it will set the locale for this
// instance.  Otherwise, it will return the locale configuration
// variables for this instance.
function locale (key) {
    var newLocaleData;

    if (key === undefined) {
        return this._locale._abbr;
    } else {
        newLocaleData = getLocale(key);
        if (newLocaleData != null) {
            this._locale = newLocaleData;
        }
        return this;
    }
}

var lang = deprecate(
    'moment().lang() is deprecated. Instead, use moment().localeData() to get the language configuration. Use moment().locale() to change languages.',
    function (key) {
        if (key === undefined) {
            return this.localeData();
        } else {
            return this.locale(key);
        }
    }
);

function localeData () {
    return this._locale;
}

function startOf (units) {
    units = normalizeUnits(units);
    // the following switch intentionally omits break keywords
    // to utilize falling through the cases.
    switch (units) {
        case 'year':
            this.month(0);
            /* falls through */
        case 'quarter':
        case 'month':
            this.date(1);
            /* falls through */
        case 'week':
        case 'isoWeek':
        case 'day':
        case 'date':
            this.hours(0);
            /* falls through */
        case 'hour':
            this.minutes(0);
            /* falls through */
        case 'minute':
            this.seconds(0);
            /* falls through */
        case 'second':
            this.milliseconds(0);
    }

    // weeks are a special case
    if (units === 'week') {
        this.weekday(0);
    }
    if (units === 'isoWeek') {
        this.isoWeekday(1);
    }

    // quarters are also special
    if (units === 'quarter') {
        this.month(Math.floor(this.month() / 3) * 3);
    }

    return this;
}

function endOf (units) {
    units = normalizeUnits(units);
    if (units === undefined || units === 'millisecond') {
        return this;
    }

    // 'date' is an alias for 'day', so it should be considered as such.
    if (units === 'date') {
        units = 'day';
    }

    return this.startOf(units).add(1, (units === 'isoWeek' ? 'week' : units)).subtract(1, 'ms');
}

function valueOf () {
    return this._d.valueOf() - ((this._offset || 0) * 60000);
}

function unix () {
    return Math.floor(this.valueOf() / 1000);
}

function toDate () {
    return new Date(this.valueOf());
}

function toArray () {
    var m = this;
    return [m.year(), m.month(), m.date(), m.hour(), m.minute(), m.second(), m.millisecond()];
}

function toObject () {
    var m = this;
    return {
        years: m.year(),
        months: m.month(),
        date: m.date(),
        hours: m.hours(),
        minutes: m.minutes(),
        seconds: m.seconds(),
        milliseconds: m.milliseconds()
    };
}

function toJSON () {
    // new Date(NaN).toJSON() === null
    return this.isValid() ? this.toISOString() : null;
}

function isValid$2 () {
    return isValid(this);
}

function parsingFlags () {
    return extend({}, getParsingFlags(this));
}

function invalidAt () {
    return getParsingFlags(this).overflow;
}

function creationData() {
    return {
        input: this._i,
        format: this._f,
        locale: this._locale,
        isUTC: this._isUTC,
        strict: this._strict
    };
}

// FORMATTING

addFormatToken(0, ['gg', 2], 0, function () {
    return this.weekYear() % 100;
});

addFormatToken(0, ['GG', 2], 0, function () {
    return this.isoWeekYear() % 100;
});

function addWeekYearFormatToken (token, getter) {
    addFormatToken(0, [token, token.length], 0, getter);
}

addWeekYearFormatToken('gggg',     'weekYear');
addWeekYearFormatToken('ggggg',    'weekYear');
addWeekYearFormatToken('GGGG',  'isoWeekYear');
addWeekYearFormatToken('GGGGG', 'isoWeekYear');

// ALIASES

addUnitAlias('weekYear', 'gg');
addUnitAlias('isoWeekYear', 'GG');

// PRIORITY

addUnitPriority('weekYear', 1);
addUnitPriority('isoWeekYear', 1);


// PARSING

addRegexToken('G',      matchSigned);
addRegexToken('g',      matchSigned);
addRegexToken('GG',     match1to2, match2);
addRegexToken('gg',     match1to2, match2);
addRegexToken('GGGG',   match1to4, match4);
addRegexToken('gggg',   match1to4, match4);
addRegexToken('GGGGG',  match1to6, match6);
addRegexToken('ggggg',  match1to6, match6);

addWeekParseToken(['gggg', 'ggggg', 'GGGG', 'GGGGG'], function (input, week, config, token) {
    week[token.substr(0, 2)] = toInt(input);
});

addWeekParseToken(['gg', 'GG'], function (input, week, config, token) {
    week[token] = hooks.parseTwoDigitYear(input);
});

// MOMENTS

function getSetWeekYear (input) {
    return getSetWeekYearHelper.call(this,
            input,
            this.week(),
            this.weekday(),
            this.localeData()._week.dow,
            this.localeData()._week.doy);
}

function getSetISOWeekYear (input) {
    return getSetWeekYearHelper.call(this,
            input, this.isoWeek(), this.isoWeekday(), 1, 4);
}

function getISOWeeksInYear () {
    return weeksInYear(this.year(), 1, 4);
}

function getWeeksInYear () {
    var weekInfo = this.localeData()._week;
    return weeksInYear(this.year(), weekInfo.dow, weekInfo.doy);
}

function getSetWeekYearHelper(input, week, weekday, dow, doy) {
    var weeksTarget;
    if (input == null) {
        return weekOfYear(this, dow, doy).year;
    } else {
        weeksTarget = weeksInYear(input, dow, doy);
        if (week > weeksTarget) {
            week = weeksTarget;
        }
        return setWeekAll.call(this, input, week, weekday, dow, doy);
    }
}

function setWeekAll(weekYear, week, weekday, dow, doy) {
    var dayOfYearData = dayOfYearFromWeeks(weekYear, week, weekday, dow, doy),
        date = createUTCDate(dayOfYearData.year, 0, dayOfYearData.dayOfYear);

    this.year(date.getUTCFullYear());
    this.month(date.getUTCMonth());
    this.date(date.getUTCDate());
    return this;
}

// FORMATTING

addFormatToken('Q', 0, 'Qo', 'quarter');

// ALIASES

addUnitAlias('quarter', 'Q');

// PRIORITY

addUnitPriority('quarter', 7);

// PARSING

addRegexToken('Q', match1);
addParseToken('Q', function (input, array) {
    array[MONTH] = (toInt(input) - 1) * 3;
});

// MOMENTS

function getSetQuarter (input) {
    return input == null ? Math.ceil((this.month() + 1) / 3) : this.month((input - 1) * 3 + this.month() % 3);
}

// FORMATTING

addFormatToken('D', ['DD', 2], 'Do', 'date');

// ALIASES

addUnitAlias('date', 'D');

// PRIOROITY
addUnitPriority('date', 9);

// PARSING

addRegexToken('D',  match1to2);
addRegexToken('DD', match1to2, match2);
addRegexToken('Do', function (isStrict, locale) {
    // TODO: Remove "ordinalParse" fallback in next major release.
    return isStrict ?
      (locale._dayOfMonthOrdinalParse || locale._ordinalParse) :
      locale._dayOfMonthOrdinalParseLenient;
});

addParseToken(['D', 'DD'], DATE);
addParseToken('Do', function (input, array) {
    array[DATE] = toInt(input.match(match1to2)[0]);
});

// MOMENTS

var getSetDayOfMonth = makeGetSet('Date', true);

// FORMATTING

addFormatToken('DDD', ['DDDD', 3], 'DDDo', 'dayOfYear');

// ALIASES

addUnitAlias('dayOfYear', 'DDD');

// PRIORITY
addUnitPriority('dayOfYear', 4);

// PARSING

addRegexToken('DDD',  match1to3);
addRegexToken('DDDD', match3);
addParseToken(['DDD', 'DDDD'], function (input, array, config) {
    config._dayOfYear = toInt(input);
});

// HELPERS

// MOMENTS

function getSetDayOfYear (input) {
    var dayOfYear = Math.round((this.clone().startOf('day') - this.clone().startOf('year')) / 864e5) + 1;
    return input == null ? dayOfYear : this.add((input - dayOfYear), 'd');
}

// FORMATTING

addFormatToken('m', ['mm', 2], 0, 'minute');

// ALIASES

addUnitAlias('minute', 'm');

// PRIORITY

addUnitPriority('minute', 14);

// PARSING

addRegexToken('m',  match1to2);
addRegexToken('mm', match1to2, match2);
addParseToken(['m', 'mm'], MINUTE);

// MOMENTS

var getSetMinute = makeGetSet('Minutes', false);

// FORMATTING

addFormatToken('s', ['ss', 2], 0, 'second');

// ALIASES

addUnitAlias('second', 's');

// PRIORITY

addUnitPriority('second', 15);

// PARSING

addRegexToken('s',  match1to2);
addRegexToken('ss', match1to2, match2);
addParseToken(['s', 'ss'], SECOND);

// MOMENTS

var getSetSecond = makeGetSet('Seconds', false);

// FORMATTING

addFormatToken('S', 0, 0, function () {
    return ~~(this.millisecond() / 100);
});

addFormatToken(0, ['SS', 2], 0, function () {
    return ~~(this.millisecond() / 10);
});

addFormatToken(0, ['SSS', 3], 0, 'millisecond');
addFormatToken(0, ['SSSS', 4], 0, function () {
    return this.millisecond() * 10;
});
addFormatToken(0, ['SSSSS', 5], 0, function () {
    return this.millisecond() * 100;
});
addFormatToken(0, ['SSSSSS', 6], 0, function () {
    return this.millisecond() * 1000;
});
addFormatToken(0, ['SSSSSSS', 7], 0, function () {
    return this.millisecond() * 10000;
});
addFormatToken(0, ['SSSSSSSS', 8], 0, function () {
    return this.millisecond() * 100000;
});
addFormatToken(0, ['SSSSSSSSS', 9], 0, function () {
    return this.millisecond() * 1000000;
});


// ALIASES

addUnitAlias('millisecond', 'ms');

// PRIORITY

addUnitPriority('millisecond', 16);

// PARSING

addRegexToken('S',    match1to3, match1);
addRegexToken('SS',   match1to3, match2);
addRegexToken('SSS',  match1to3, match3);

var token;
for (token = 'SSSS'; token.length <= 9; token += 'S') {
    addRegexToken(token, matchUnsigned);
}

function parseMs(input, array) {
    array[MILLISECOND] = toInt(('0.' + input) * 1000);
}

for (token = 'S'; token.length <= 9; token += 'S') {
    addParseToken(token, parseMs);
}
// MOMENTS

var getSetMillisecond = makeGetSet('Milliseconds', false);

// FORMATTING

addFormatToken('z',  0, 0, 'zoneAbbr');
addFormatToken('zz', 0, 0, 'zoneName');

// MOMENTS

function getZoneAbbr () {
    return this._isUTC ? 'UTC' : '';
}

function getZoneName () {
    return this._isUTC ? 'Coordinated Universal Time' : '';
}

var proto = Moment.prototype;

proto.add               = add;
proto.calendar          = calendar$1;
proto.clone             = clone;
proto.diff              = diff;
proto.endOf             = endOf;
proto.format            = format;
proto.from              = from;
proto.fromNow           = fromNow;
proto.to                = to;
proto.toNow             = toNow;
proto.get               = stringGet;
proto.invalidAt         = invalidAt;
proto.isAfter           = isAfter;
proto.isBefore          = isBefore;
proto.isBetween         = isBetween;
proto.isSame            = isSame;
proto.isSameOrAfter     = isSameOrAfter;
proto.isSameOrBefore    = isSameOrBefore;
proto.isValid           = isValid$2;
proto.lang              = lang;
proto.locale            = locale;
proto.localeData        = localeData;
proto.max               = prototypeMax;
proto.min               = prototypeMin;
proto.parsingFlags      = parsingFlags;
proto.set               = stringSet;
proto.startOf           = startOf;
proto.subtract          = subtract;
proto.toArray           = toArray;
proto.toObject          = toObject;
proto.toDate            = toDate;
proto.toISOString       = toISOString;
proto.inspect           = inspect;
proto.toJSON            = toJSON;
proto.toString          = toString;
proto.unix              = unix;
proto.valueOf           = valueOf;
proto.creationData      = creationData;

// Year
proto.year       = getSetYear;
proto.isLeapYear = getIsLeapYear;

// Week Year
proto.weekYear    = getSetWeekYear;
proto.isoWeekYear = getSetISOWeekYear;

// Quarter
proto.quarter = proto.quarters = getSetQuarter;

// Month
proto.month       = getSetMonth;
proto.daysInMonth = getDaysInMonth;

// Week
proto.week           = proto.weeks        = getSetWeek;
proto.isoWeek        = proto.isoWeeks     = getSetISOWeek;
proto.weeksInYear    = getWeeksInYear;
proto.isoWeeksInYear = getISOWeeksInYear;

// Day
proto.date       = getSetDayOfMonth;
proto.day        = proto.days             = getSetDayOfWeek;
proto.weekday    = getSetLocaleDayOfWeek;
proto.isoWeekday = getSetISODayOfWeek;
proto.dayOfYear  = getSetDayOfYear;

// Hour
proto.hour = proto.hours = getSetHour;

// Minute
proto.minute = proto.minutes = getSetMinute;

// Second
proto.second = proto.seconds = getSetSecond;

// Millisecond
proto.millisecond = proto.milliseconds = getSetMillisecond;

// Offset
proto.utcOffset            = getSetOffset;
proto.utc                  = setOffsetToUTC;
proto.local                = setOffsetToLocal;
proto.parseZone            = setOffsetToParsedOffset;
proto.hasAlignedHourOffset = hasAlignedHourOffset;
proto.isDST                = isDaylightSavingTime;
proto.isLocal              = isLocal;
proto.isUtcOffset          = isUtcOffset;
proto.isUtc                = isUtc;
proto.isUTC                = isUtc;

// Timezone
proto.zoneAbbr = getZoneAbbr;
proto.zoneName = getZoneName;

// Deprecations
proto.dates  = deprecate('dates accessor is deprecated. Use date instead.', getSetDayOfMonth);
proto.months = deprecate('months accessor is deprecated. Use month instead', getSetMonth);
proto.years  = deprecate('years accessor is deprecated. Use year instead', getSetYear);
proto.zone   = deprecate('moment().zone is deprecated, use moment().utcOffset instead. http://momentjs.com/guides/#/warnings/zone/', getSetZone);
proto.isDSTShifted = deprecate('isDSTShifted is deprecated. See http://momentjs.com/guides/#/warnings/dst-shifted/ for more information', isDaylightSavingTimeShifted);

function createUnix (input) {
    return createLocal(input * 1000);
}

function createInZone () {
    return createLocal.apply(null, arguments).parseZone();
}

function preParsePostFormat (string) {
    return string;
}

var proto$1 = Locale.prototype;

proto$1.calendar        = calendar;
proto$1.longDateFormat  = longDateFormat;
proto$1.invalidDate     = invalidDate;
proto$1.ordinal         = ordinal;
proto$1.preparse        = preParsePostFormat;
proto$1.postformat      = preParsePostFormat;
proto$1.relativeTime    = relativeTime;
proto$1.pastFuture      = pastFuture;
proto$1.set             = set;

// Month
proto$1.months            =        localeMonths;
proto$1.monthsShort       =        localeMonthsShort;
proto$1.monthsParse       =        localeMonthsParse;
proto$1.monthsRegex       = monthsRegex;
proto$1.monthsShortRegex  = monthsShortRegex;

// Week
proto$1.week = localeWeek;
proto$1.firstDayOfYear = localeFirstDayOfYear;
proto$1.firstDayOfWeek = localeFirstDayOfWeek;

// Day of Week
proto$1.weekdays       =        localeWeekdays;
proto$1.weekdaysMin    =        localeWeekdaysMin;
proto$1.weekdaysShort  =        localeWeekdaysShort;
proto$1.weekdaysParse  =        localeWeekdaysParse;

proto$1.weekdaysRegex       =        weekdaysRegex;
proto$1.weekdaysShortRegex  =        weekdaysShortRegex;
proto$1.weekdaysMinRegex    =        weekdaysMinRegex;

// Hours
proto$1.isPM = localeIsPM;
proto$1.meridiem = localeMeridiem;

function get$1 (format, index, field, setter) {
    var locale = getLocale();
    var utc = createUTC().set(setter, index);
    return locale[field](utc, format);
}

function listMonthsImpl (format, index, field) {
    if (isNumber(format)) {
        index = format;
        format = undefined;
    }

    format = format || '';

    if (index != null) {
        return get$1(format, index, field, 'month');
    }

    var i;
    var out = [];
    for (i = 0; i < 12; i++) {
        out[i] = get$1(format, i, field, 'month');
    }
    return out;
}

// ()
// (5)
// (fmt, 5)
// (fmt)
// (true)
// (true, 5)
// (true, fmt, 5)
// (true, fmt)
function listWeekdaysImpl (localeSorted, format, index, field) {
    if (typeof localeSorted === 'boolean') {
        if (isNumber(format)) {
            index = format;
            format = undefined;
        }

        format = format || '';
    } else {
        format = localeSorted;
        index = format;
        localeSorted = false;

        if (isNumber(format)) {
            index = format;
            format = undefined;
        }

        format = format || '';
    }

    var locale = getLocale(),
        shift = localeSorted ? locale._week.dow : 0;

    if (index != null) {
        return get$1(format, (index + shift) % 7, field, 'day');
    }

    var i;
    var out = [];
    for (i = 0; i < 7; i++) {
        out[i] = get$1(format, (i + shift) % 7, field, 'day');
    }
    return out;
}

function listMonths (format, index) {
    return listMonthsImpl(format, index, 'months');
}

function listMonthsShort (format, index) {
    return listMonthsImpl(format, index, 'monthsShort');
}

function listWeekdays (localeSorted, format, index) {
    return listWeekdaysImpl(localeSorted, format, index, 'weekdays');
}

function listWeekdaysShort (localeSorted, format, index) {
    return listWeekdaysImpl(localeSorted, format, index, 'weekdaysShort');
}

function listWeekdaysMin (localeSorted, format, index) {
    return listWeekdaysImpl(localeSorted, format, index, 'weekdaysMin');
}

getSetGlobalLocale('en', {
    dayOfMonthOrdinalParse: /\d{1,2}(th|st|nd|rd)/,
    ordinal : function (number) {
        var b = number % 10,
            output = (toInt(number % 100 / 10) === 1) ? 'th' :
            (b === 1) ? 'st' :
            (b === 2) ? 'nd' :
            (b === 3) ? 'rd' : 'th';
        return number + output;
    }
});

// Side effect imports
hooks.lang = deprecate('moment.lang is deprecated. Use moment.locale instead.', getSetGlobalLocale);
hooks.langData = deprecate('moment.langData is deprecated. Use moment.localeData instead.', getLocale);

var mathAbs = Math.abs;

function abs () {
    var data           = this._data;

    this._milliseconds = mathAbs(this._milliseconds);
    this._days         = mathAbs(this._days);
    this._months       = mathAbs(this._months);

    data.milliseconds  = mathAbs(data.milliseconds);
    data.seconds       = mathAbs(data.seconds);
    data.minutes       = mathAbs(data.minutes);
    data.hours         = mathAbs(data.hours);
    data.months        = mathAbs(data.months);
    data.years         = mathAbs(data.years);

    return this;
}

function addSubtract$1 (duration, input, value, direction) {
    var other = createDuration(input, value);

    duration._milliseconds += direction * other._milliseconds;
    duration._days         += direction * other._days;
    duration._months       += direction * other._months;

    return duration._bubble();
}

// supports only 2.0-style add(1, 's') or add(duration)
function add$1 (input, value) {
    return addSubtract$1(this, input, value, 1);
}

// supports only 2.0-style subtract(1, 's') or subtract(duration)
function subtract$1 (input, value) {
    return addSubtract$1(this, input, value, -1);
}

function absCeil (number) {
    if (number < 0) {
        return Math.floor(number);
    } else {
        return Math.ceil(number);
    }
}

function bubble () {
    var milliseconds = this._milliseconds;
    var days         = this._days;
    var months       = this._months;
    var data         = this._data;
    var seconds, minutes, hours, years, monthsFromDays;

    // if we have a mix of positive and negative values, bubble down first
    // check: https://github.com/moment/moment/issues/2166
    if (!((milliseconds >= 0 && days >= 0 && months >= 0) ||
            (milliseconds <= 0 && days <= 0 && months <= 0))) {
        milliseconds += absCeil(monthsToDays(months) + days) * 864e5;
        days = 0;
        months = 0;
    }

    // The following code bubbles up values, see the tests for
    // examples of what that means.
    data.milliseconds = milliseconds % 1000;

    seconds           = absFloor(milliseconds / 1000);
    data.seconds      = seconds % 60;

    minutes           = absFloor(seconds / 60);
    data.minutes      = minutes % 60;

    hours             = absFloor(minutes / 60);
    data.hours        = hours % 24;

    days += absFloor(hours / 24);

    // convert days to months
    monthsFromDays = absFloor(daysToMonths(days));
    months += monthsFromDays;
    days -= absCeil(monthsToDays(monthsFromDays));

    // 12 months -> 1 year
    years = absFloor(months / 12);
    months %= 12;

    data.days   = days;
    data.months = months;
    data.years  = years;

    return this;
}

function daysToMonths (days) {
    // 400 years have 146097 days (taking into account leap year rules)
    // 400 years have 12 months === 4800
    return days * 4800 / 146097;
}

function monthsToDays (months) {
    // the reverse of daysToMonths
    return months * 146097 / 4800;
}

function as (units) {
    if (!this.isValid()) {
        return NaN;
    }
    var days;
    var months;
    var milliseconds = this._milliseconds;

    units = normalizeUnits(units);

    if (units === 'month' || units === 'year') {
        days   = this._days   + milliseconds / 864e5;
        months = this._months + daysToMonths(days);
        return units === 'month' ? months : months / 12;
    } else {
        // handle milliseconds separately because of floating point math errors (issue #1867)
        days = this._days + Math.round(monthsToDays(this._months));
        switch (units) {
            case 'week'   : return days / 7     + milliseconds / 6048e5;
            case 'day'    : return days         + milliseconds / 864e5;
            case 'hour'   : return days * 24    + milliseconds / 36e5;
            case 'minute' : return days * 1440  + milliseconds / 6e4;
            case 'second' : return days * 86400 + milliseconds / 1000;
            // Math.floor prevents floating point math errors here
            case 'millisecond': return Math.floor(days * 864e5) + milliseconds;
            default: throw new Error('Unknown unit ' + units);
        }
    }
}

// TODO: Use this.as('ms')?
function valueOf$1 () {
    if (!this.isValid()) {
        return NaN;
    }
    return (
        this._milliseconds +
        this._days * 864e5 +
        (this._months % 12) * 2592e6 +
        toInt(this._months / 12) * 31536e6
    );
}

function makeAs (alias) {
    return function () {
        return this.as(alias);
    };
}

var asMilliseconds = makeAs('ms');
var asSeconds      = makeAs('s');
var asMinutes      = makeAs('m');
var asHours        = makeAs('h');
var asDays         = makeAs('d');
var asWeeks        = makeAs('w');
var asMonths       = makeAs('M');
var asYears        = makeAs('y');

function clone$1 () {
    return createDuration(this);
}

function get$2 (units) {
    units = normalizeUnits(units);
    return this.isValid() ? this[units + 's']() : NaN;
}

function makeGetter(name) {
    return function () {
        return this.isValid() ? this._data[name] : NaN;
    };
}

var milliseconds = makeGetter('milliseconds');
var seconds      = makeGetter('seconds');
var minutes      = makeGetter('minutes');
var hours        = makeGetter('hours');
var days         = makeGetter('days');
var months       = makeGetter('months');
var years        = makeGetter('years');

function weeks () {
    return absFloor(this.days() / 7);
}

var round = Math.round;
var thresholds = {
    ss: 44,         // a few seconds to seconds
    s : 45,         // seconds to minute
    m : 45,         // minutes to hour
    h : 22,         // hours to day
    d : 26,         // days to month
    M : 11          // months to year
};

// helper function for moment.fn.from, moment.fn.fromNow, and moment.duration.fn.humanize
function substituteTimeAgo(string, number, withoutSuffix, isFuture, locale) {
    return locale.relativeTime(number || 1, !!withoutSuffix, string, isFuture);
}

function relativeTime$1 (posNegDuration, withoutSuffix, locale) {
    var duration = createDuration(posNegDuration).abs();
    var seconds  = round(duration.as('s'));
    var minutes  = round(duration.as('m'));
    var hours    = round(duration.as('h'));
    var days     = round(duration.as('d'));
    var months   = round(duration.as('M'));
    var years    = round(duration.as('y'));

    var a = seconds <= thresholds.ss && ['s', seconds]  ||
            seconds < thresholds.s   && ['ss', seconds] ||
            minutes <= 1             && ['m']           ||
            minutes < thresholds.m   && ['mm', minutes] ||
            hours   <= 1             && ['h']           ||
            hours   < thresholds.h   && ['hh', hours]   ||
            days    <= 1             && ['d']           ||
            days    < thresholds.d   && ['dd', days]    ||
            months  <= 1             && ['M']           ||
            months  < thresholds.M   && ['MM', months]  ||
            years   <= 1             && ['y']           || ['yy', years];

    a[2] = withoutSuffix;
    a[3] = +posNegDuration > 0;
    a[4] = locale;
    return substituteTimeAgo.apply(null, a);
}

// This function allows you to set the rounding function for relative time strings
function getSetRelativeTimeRounding (roundingFunction) {
    if (roundingFunction === undefined) {
        return round;
    }
    if (typeof(roundingFunction) === 'function') {
        round = roundingFunction;
        return true;
    }
    return false;
}

// This function allows you to set a threshold for relative time strings
function getSetRelativeTimeThreshold (threshold, limit) {
    if (thresholds[threshold] === undefined) {
        return false;
    }
    if (limit === undefined) {
        return thresholds[threshold];
    }
    thresholds[threshold] = limit;
    if (threshold === 's') {
        thresholds.ss = limit - 1;
    }
    return true;
}

function humanize (withSuffix) {
    if (!this.isValid()) {
        return this.localeData().invalidDate();
    }

    var locale = this.localeData();
    var output = relativeTime$1(this, !withSuffix, locale);

    if (withSuffix) {
        output = locale.pastFuture(+this, output);
    }

    return locale.postformat(output);
}

var abs$1 = Math.abs;

function sign(x) {
    return ((x > 0) - (x < 0)) || +x;
}

function toISOString$1() {
    // for ISO strings we do not use the normal bubbling rules:
    //  * milliseconds bubble up until they become hours
    //  * days do not bubble at all
    //  * months bubble up until they become years
    // This is because there is no context-free conversion between hours and days
    // (think of clock changes)
    // and also not between days and months (28-31 days per month)
    if (!this.isValid()) {
        return this.localeData().invalidDate();
    }

    var seconds = abs$1(this._milliseconds) / 1000;
    var days         = abs$1(this._days);
    var months       = abs$1(this._months);
    var minutes, hours, years;

    // 3600 seconds -> 60 minutes -> 1 hour
    minutes           = absFloor(seconds / 60);
    hours             = absFloor(minutes / 60);
    seconds %= 60;
    minutes %= 60;

    // 12 months -> 1 year
    years  = absFloor(months / 12);
    months %= 12;


    // inspired by https://github.com/dordille/moment-isoduration/blob/master/moment.isoduration.js
    var Y = years;
    var M = months;
    var D = days;
    var h = hours;
    var m = minutes;
    var s = seconds ? seconds.toFixed(3).replace(/\.?0+$/, '') : '';
    var total = this.asSeconds();

    if (!total) {
        // this is the same as C#'s (Noda) and python (isodate)...
        // but not other JS (goog.date)
        return 'P0D';
    }

    var totalSign = total < 0 ? '-' : '';
    var ymSign = sign(this._months) !== sign(total) ? '-' : '';
    var daysSign = sign(this._days) !== sign(total) ? '-' : '';
    var hmsSign = sign(this._milliseconds) !== sign(total) ? '-' : '';

    return totalSign + 'P' +
        (Y ? ymSign + Y + 'Y' : '') +
        (M ? ymSign + M + 'M' : '') +
        (D ? daysSign + D + 'D' : '') +
        ((h || m || s) ? 'T' : '') +
        (h ? hmsSign + h + 'H' : '') +
        (m ? hmsSign + m + 'M' : '') +
        (s ? hmsSign + s + 'S' : '');
}

var proto$2 = Duration.prototype;

proto$2.isValid        = isValid$1;
proto$2.abs            = abs;
proto$2.add            = add$1;
proto$2.subtract       = subtract$1;
proto$2.as             = as;
proto$2.asMilliseconds = asMilliseconds;
proto$2.asSeconds      = asSeconds;
proto$2.asMinutes      = asMinutes;
proto$2.asHours        = asHours;
proto$2.asDays         = asDays;
proto$2.asWeeks        = asWeeks;
proto$2.asMonths       = asMonths;
proto$2.asYears        = asYears;
proto$2.valueOf        = valueOf$1;
proto$2._bubble        = bubble;
proto$2.clone          = clone$1;
proto$2.get            = get$2;
proto$2.milliseconds   = milliseconds;
proto$2.seconds        = seconds;
proto$2.minutes        = minutes;
proto$2.hours          = hours;
proto$2.days           = days;
proto$2.weeks          = weeks;
proto$2.months         = months;
proto$2.years          = years;
proto$2.humanize       = humanize;
proto$2.toISOString    = toISOString$1;
proto$2.toString       = toISOString$1;
proto$2.toJSON         = toISOString$1;
proto$2.locale         = locale;
proto$2.localeData     = localeData;

// Deprecations
proto$2.toIsoString = deprecate('toIsoString() is deprecated. Please use toISOString() instead (notice the capitals)', toISOString$1);
proto$2.lang = lang;

// Side effect imports

// FORMATTING

addFormatToken('X', 0, 0, 'unix');
addFormatToken('x', 0, 0, 'valueOf');

// PARSING

addRegexToken('x', matchSigned);
addRegexToken('X', matchTimestamp);
addParseToken('X', function (input, array, config) {
    config._d = new Date(parseFloat(input, 10) * 1000);
});
addParseToken('x', function (input, array, config) {
    config._d = new Date(toInt(input));
});

// Side effect imports


hooks.version = '2.20.1';

setHookCallback(createLocal);

hooks.fn                    = proto;
hooks.min                   = min;
hooks.max                   = max;
hooks.now                   = now;
hooks.utc                   = createUTC;
hooks.unix                  = createUnix;
hooks.months                = listMonths;
hooks.isDate                = isDate;
hooks.locale                = getSetGlobalLocale;
hooks.invalid               = createInvalid;
hooks.duration              = createDuration;
hooks.isMoment              = isMoment;
hooks.weekdays              = listWeekdays;
hooks.parseZone             = createInZone;
hooks.localeData            = getLocale;
hooks.isDuration            = isDuration;
hooks.monthsShort           = listMonthsShort;
hooks.weekdaysMin           = listWeekdaysMin;
hooks.defineLocale          = defineLocale;
hooks.updateLocale          = updateLocale;
hooks.locales               = listLocales;
hooks.weekdaysShort         = listWeekdaysShort;
hooks.normalizeUnits        = normalizeUnits;
hooks.relativeTimeRounding  = getSetRelativeTimeRounding;
hooks.relativeTimeThreshold = getSetRelativeTimeThreshold;
hooks.calendarFormat        = getCalendarFormat;
hooks.prototype             = proto;

// currently HTML5 input type only supports 24-hour formats
hooks.HTML5_FMT = {
    DATETIME_LOCAL: 'YYYY-MM-DDTHH:mm',             // <input type="datetime-local" />
    DATETIME_LOCAL_SECONDS: 'YYYY-MM-DDTHH:mm:ss',  // <input type="datetime-local" step="1" />
    DATETIME_LOCAL_MS: 'YYYY-MM-DDTHH:mm:ss.SSS',   // <input type="datetime-local" step="0.001" />
    DATE: 'YYYY-MM-DD',                             // <input type="date" />
    TIME: 'HH:mm',                                  // <input type="time" />
    TIME_SECONDS: 'HH:mm:ss',                       // <input type="time" step="1" />
    TIME_MS: 'HH:mm:ss.SSS',                        // <input type="time" step="0.001" />
    WEEK: 'YYYY-[W]WW',                             // <input type="week" />
    MONTH: 'YYYY-MM'                                // <input type="month" />
};

return hooks;

})));

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(7)(module)))

/***/ }),

/***/ 22:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1), __webpack_require__(3), __webpack_require__(4), __webpack_require__(25), __webpack_require__(2), __webpack_require__(5),
        __webpack_require__(8), __webpack_require__(9)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString, jqGridAdditional, BetListOption, moment, Popup) {
        "use strict";
        var BETTYPE_CASHOUT = 911;
        var BETTYPE_MIXPARLAY = 9, BETTYPE_SYSTEMPARLAY = 29, BETTYPE_MIXPARLAY_BETRADAR = 2799;
        var MIN_BETTYPE_FASTMARKET = 221;
        var MAX_BETTYPE_FASTMARKET = 227;
        var SPORT_MICROGAMMING_LC = 203;
        var SPORT_MICROGAMMING_RNG = 205;
        var SPORT_RNG_GAME = 208;
        var SPORT_RNG_MINIGAME = 209;
        var SPORT_RNG_MOBILE = 210;
        var SPORT_ALLBET = 211;
        var SPORT_VOIDBRIDGE = 212;
        var SPORT_GLOBALGAMING = 219;
        var BETLIST_META_MODEL = "#BetListMetaModel";

        var _sortColumnName, _sortOrder;
        var _params, _url;
        var _isShowSumStake;
        var _onClientPaging = false;

        function showDetailPopUp(url, option) {
            option.id = "betlist-popup-detail";
            Popup.openPopupWithIframe(url, option);
        }

        function showResultPopUp(url, option) {
            option.id = "betlist-popup-result";
            Popup.openPopupWithIframe(url, option);
        }

        function isFastMarket(bettype) {
            return bettype >= MIN_BETTYPE_FASTMARKET && bettype <= MAX_BETTYPE_FASTMARKET;
        }

        var _me = {
            getSortParams: function () {
                if (!_sortColumnName) {
                    return null;
                }

                return {
                    SortBy: _sortColumnName,
                    SortOrder: _sortOrder
                };
            },
            submiting: true,
            initialize: function () {
                Site.initialize();

                if (_me.isMatchWLBetList() === true) {
                    _me.updateSortColumn();
                }

                _isShowSumStake = $("#IsShowSumStake").val() === "1";

                _sortColumnName = _sortColumnName || $("#SortBy").val();
                _sortOrder = _sortOrder || $("#SortOrder").val();

                _me.registerBetListGlobalFunctions();
            },

            updateSortColumn: function () {
                BetListOption.columnModel[1].sortable = true; // externalId
                BetListOption.columnModel[2].sortable = true; // systemId
            },

            isMatchWLBetList: function () {
                var regex = /MatchWinLossBetList/i;
                return regex.test($("#ApiUrl").val());
            },

            gridComplete: function (grid) {
                var $grid = $(grid);
                // add sort icon
                var sortCol = _sortColumnName === "Winlost" ? "WinLoss" : _sortColumnName;
                sortCol = _sortColumnName === "nickname" ? "ExternalId" : sortCol;
                sortCol = _sortColumnName === "UserName" ? "SystemId" : sortCol;
                if (sortCol && !_onClientPaging) {
                    jqGridAdditional.addSortableHeader($grid, sortCol, _sortOrder);
                }
                _onClientPaging = false;
                // Resize
                jqGridAdditional.gridComplete();
                // Remove unnecessary item
                if (_isShowSumStake) {
                    $(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Odds]").html(Site.getSingleResource("GrandTotal")).removeAttr("title");
                } else {
                    $(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Stake]").html(Site.getSingleResource("GrandTotal")).removeAttr("title");
                }

                $("#pager2").show();

                // fix cursor on non-sortable columns
                var cm = $grid[0].p.colModel;
                $.each($grid[0].grid.headers, function (index, value) {
                    var cmi = cm[index];
                    if (!cmi.sortable) {
                        $('.ui-jqgrid-sortable', value.el).css({ cursor: "default" });
                    }
                });

                $(".ui-jqgrid-bdiv").css({ "height": "auto" });
                jqGridAdditional.updateGridHeight();
                Site.unBlockUI();
            },

            getGridOption: function () {
                var gridOption = $.extend(true, {}, BetListOption.gridOption);

                gridOption.onSortCol = function (index) {
                    if (window.disableBetListSorting) {
                        return 'stop';
                    }
                    Site.blockUI();
                    index = index === "WinLoss" ? "Winlost" : index;
                    index = index === "ExternalId" ? "nickname" : index;
                    if (_sortColumnName === index) {
                        _sortOrder = _sortOrder === "asc" ? "desc" : "asc";
                    } else {
                        _sortOrder = "desc";
                        _sortColumnName = index;
                    }
                    if (window.CorrectScoreBetList || window.CancelledBetsBetList) {
                        $("#SortBy").val(_sortColumnName);
                        $("#SortOrder").val(_sortOrder);
                    }

                    if (window.serverPaging || $("#IsServerPaging").val() === "1") {
                        _me.loadDataPaging(_url);
                    }
                    else {
                        _me.loadData(_params, _url);
                    }

                    return 'stop';
                };

                // Specify the hidden column
                var hiddenColumnList = $("#HiddenColumnList").val();
                var hiddenColumnTokens = hiddenColumnList.split(",");
                var showSummary = $("#ShowSummary").val() === "0" ? false : hiddenColumnList.indexOf("licenseewl") < 0;

                $.each(hiddenColumnTokens, function (index, value) {
                    jqGridAdditional.hideColumn(BetListOption.columnModel, value);
                });
                if ($("#HasBetEffectiveColumn").val() !== "1") {
                    jqGridAdditional.hideColumn(BetListOption.columnModel, "BetEffective");
                }
                if (Site.isFakeLicensee()) {
                    jqGridAdditional.hideColumn(BetListOption.columnModel, "SystemId");
                }
                gridOption.colNames = BetListOption.columnNames;
                gridOption.colModel = BetListOption.columnModel;
                gridOption.footerrow = showSummary;
                var hasCustComm = $('#HasCustComm').val();
                if (hasCustComm === '1') {
                    gridOption.colNames = $.map(gridOption.colNames, function (val) {
                        if (val === 'WinLoss') {
                            return '@[WinLoss]/@[Comm]';
                        }
                        return val;
                    });
                }
                gridOption.colNames = Site.getResources(gridOption.colNames);

                gridOption.onPaging = function () {
                    _onClientPaging = true;
                };
                return gridOption;
            },

            loadData: function (params, url) {
                if (typeof params === "string") {
                    params = JSON.parse(params);
                }

                _params = params;
                _url = url;

                if (params && _sortColumnName) {
                    params.SortBy = _sortColumnName;
                    params.SortOrder = _sortOrder;
                }

                var paramString = typeof (params) === "object" ? JSON.stringify(params) : params;

                $.ajax({
                    type: "POST",
                    data: paramString,
                    contentType: 'application/json',
                    dataType: "json",
                    cache: false,
                    url: url,
                    success: function (response) {
                        if (response !== null) {
                            _me.bindData(response);
                            _me.setLeagueTitle(response.LeagueName);
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                });
            },

            loadDataPaging: function (url) {
                _url = url;
                Site.blockUI();
                if (url && _sortColumnName) {
                    url = QueryString.setParam(url, "IsServerPaging", true);
                    url = QueryString.setParam(url, "SortBy", _sortColumnName);
                    url = QueryString.setParam(url, "SortOrder", _sortOrder);
                }

                _me.submiting = true;

                var gridOption = _me.getGridOption();
                var rows = Number($("#BetListGrid").getGridParam("rowNum")) || gridOption.rowNum;
                gridOption.url = url;
                gridOption.rowNum = rows;
                gridOption.height = 0;
                gridOption.loadonce = false;
                gridOption.datatype = "json";
                gridOption.gridComplete = function () {
                    var rowCount = $(this).getGridParam("records");
                    if (rowCount === 0) {
                        $("#pager2").hide();
                        jqGridAdditional.setNoInfo();
                        Site.unBlockUI();
                        return;
                    }

                    $("#gbox_BetListGrid").show();
                    _me.gridComplete(this);
                    if (_me.submiting) {
                        $(".ui-pg-selbox").val(rows);
                    }
                    _me.submiting = false;
                };

                // Show the grid
                $("#BetListGrid").jqGrid('GridUnload');
                $("#BetListGrid").jqGrid(gridOption);
                $("#gbox_BetListGrid").hide();
            },

            bindData: function (response) {
                $("#BetListGrid").jqGrid('GridUnload');

                if (response === null || response.Main.length === 0) {
                    jqGridAdditional.setNoInfo();
                    return;
                }

                var rawData = response.Main;
                var summary = response.Total;
                var sumWinLoss = jqGridAdditional.formatDecimal(summary.SumWinLoss);
                var sumLicenseeWl = jqGridAdditional.formatLicenseeWl(summary.SumLicenseeWl);
                var sumBetEffective = jqGridAdditional.formatDecimal(summary.SumBetEffective);
                var gridOption = _me.getGridOption();

                var rowNum = Number($("#BetListGrid").getGridParam("rowNum")) || gridOption.rowNum;
                if (rowNum < gridOption.rowList[0]) {
                    rowNum = gridOption.rowList[0];
                }
                // Show the grid
                gridOption.rowNum = rowNum;
                gridOption.data = rawData;
                gridOption.userData = { WinLoss: sumWinLoss, LicenseeWL: sumLicenseeWl, BetEffective: sumBetEffective };

                if (_isShowSumStake) {
                    gridOption.userData.Stake = jqGridAdditional.formatDecimal(summary.SumStake);
                    gridOption.userDataOnFooter = true;
                    gridOption.footerrow = true;
                }
                else if (response.IsShowTotal === undefined || response.IsShowTotal) {
                    gridOption.userDataOnFooter = true;
                }
                else {
                    gridOption.userDataOnFooter = false;
                    gridOption.footerrow = false;
                }
                gridOption.gridComplete = function () {
                    _me.gridComplete(this);
                };

                $("#BetListGrid").jqGrid(gridOption);
            },

            getMixParlayResultUrl: function (refno, winlostdate, bettype) {
                var url = Site.resolveClientUrl('ResultReport/ResultPopup');
                url = QueryString.setParam(url, "refno", refno);
                url = QueryString.setParam(url, "winlossdate", winlostdate);
                url = QueryString.setParam(url, "bettype", bettype);
                url = QueryString.setParam(url, "matchid", 1);

                return url;
            },

            getSportBookResultUrl: function (matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
                var url = Site.resolveClientUrl('ResultReport/ResultPopup');
                url = QueryString.setParam(url, "matchid", matchid);
                url = QueryString.setParam(url, "bettype", bettype);
                url = QueryString.setParam(url, "raceno", raceNo);
                url = QueryString.setParam(url, "sporttype", sporttype);
                url = QueryString.setParam(url, "isoutright", isOutright);
                
                if (isFastMarket(bettype)) {
                    url = QueryString.setParam(url, "handicap1", extension1);
                    url = QueryString.setParam(url, "handicap2", extension2);
                }
                else {
                    url = QueryString.setParam(url, "betid", extension1);
                }

                return url;
            },

            getMicrogamingResultUrl: function (gameid, custid) {
                var url = Site.resolveClientUrl('ResultReport/MicrogamingResultPopup');
                url = QueryString.setParam(url, "gameid", gameid);
                url = QueryString.setParam(url, "custid", custid);

                return url;
            },

            viewRngCasinoResult: function (title, refno) {
                var url = Site.resolveClientUrl('ResultReport/ShowRngCasinoSingleWalletResult');

                var params = { "refNo": refno };
                $.ajax({
                    type: "POST",
                    contentType: 'application/json',
                    dataType: "json",
                    data: JSON.stringify(params),
                    cache: false,
                    url: url,
                    success: function (response) {
                        var result = response.Result;
                        if (response.Success) {
                            if (result === "") {
                                return;
                            }

                            showResultPopUp(result, { width: 800, height: 380, title: title });
                        }
                        else {
                            alert(Site.getSingleResource("ErrorProcessingMsg"));
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                });
            },

            viewVoidbridgeResult: function (custId, gamePlayId, title) {
                var url = Site.resolveClientUrl('ResultReport/VoidbridgeResult');
                Site.blockUI();
                $.get(url + '?custid=' + custId + '&gamePlayId=' + gamePlayId, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewAllBetResult: function (trasdesc, betcheck, bettype, title) {
                var url = Site.resolveClientUrl('ResultReport/AllBetResult?transdesc=' +
                    encodeURIComponent(trasdesc) + '&betcheck=' + betcheck + '&bettype=' + bettype);

                Site.blockUI();
                showResultPopUp(url, { width: 800, height: 380, title: title });
                Site.unBlockUI();
            },

            setLeagueTitle: function (leagueName) {
                var leagueNameText = leagueName === '' || leagueName === undefined ? Site.getSingleResource('BetList') : leagueName;
                $(".league-name-title").html(leagueNameText + " - ");
            },
            showMicrogamingResultPopup: function (title, gameid, custid) {
                var url = _me.getMicrogamingResultUrl(gameid, custid);

                showResultPopUp(url, { width: 750, height: 400, title: title });
            },
            viewGlobalGamingResult: function (custId, gameId, title) {
                var url = Site.resolveClientUrl('ResultReport/GlobalGamingResult');
                Site.blockUI();
                $.get(url + '?custid=' + custId + '&gameId=' + gameId, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },
            viewResult: function (matchid, raceNo, bettype, sporttype, refno,
                username, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs) {
                var title = Site.getSingleResource("Result");
                if (sporttype === SPORT_MICROGAMMING_RNG || sporttype === SPORT_MICROGAMMING_LC) {
                    _me.showMicrogamingResultPopup(title, extension2, custid);
                }
                else if (sporttype === SPORT_RNG_GAME || sporttype === SPORT_RNG_MINIGAME || sporttype === SPORT_RNG_MOBILE) {
                    _me.viewRngCasinoResult(title, refno);
                }
                else if (sporttype === SPORT_VOIDBRIDGE) {
                    _me.viewVoidbridgeResult(custid, extension2, title);
                }
                else if (sporttype === SPORT_ALLBET) {
                    _me.viewAllBetResult(transdecs, extension2, bettype, title);
                }
                else if (sporttype === SPORT_GLOBALGAMING) {
                    _me.viewGlobalGamingResult(custid, extension2, title);
                }
                else {
                    bettype = Number(bettype);
                    var isParlay = bettype === BETTYPE_MIXPARLAY || bettype === BETTYPE_SYSTEMPARLAY || bettype === BETTYPE_MIXPARLAY_BETRADAR;
                    var url = isParlay
                        ? _me.getMixParlayResultUrl(refnoMixParlay, winlostdate, bettype)
                        : _me.getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);

                    var height = isParlay ? 450 : (isFastMarket(bettype) ? 300 : 250);
                    var width = 750;

                    showResultPopUp(url, { width: width + 25, height: height + 70, title: title });
                }
            },

            showMixParlayDetail: function (transId, divEvent) {
                var url = Site.resolveClientUrl('api/MixParlayBetList/Detail?transId=' + transId);
                if (divEvent.css("display") === 'none') {
                    $.ajax({
                        type: "GET",
                        contentType: 'application/json',
                        dataType: "json",
                        cache: false,
                        url: url,
                        success: function (response) {
                            var choice = "";
                            var len = response.length;
                            $.each(response, function (index, item) {
                                if (index === len - 1) {
                                    choice += item.Choice;
                                }
                                else {
                                    choice += item.Choice + "<div class='line'></div>";
                                }
                            });
                            divEvent.html(choice);
                            divEvent.css("display", "");
                        },
                        beforeSend: function () {
                            Site.blockUI();
                        },
                        complete: function () {
                            Site.unBlockUI();
                        }
                    });
                }
                else {
                    divEvent.css("display", "none");
                    divEvent.html("");
                }
            },

            showNewParlayDetail: function (transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam) {
                var $divEvent = $('#divEvent_' + transId + "" + transIndex);
                var $nameDiv = $('#combinationOfferName_' + transId + transIndex);
                $divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket

                if ($divEvent.is(":hidden")) {
                    var url = Site.resolveClientUrl('api/MixParlayBetList/NewParlayDetail');
                    url = QueryString.setParam(url, "transId", transId);
                    url = QueryString.setParam(url, "refNo", refNo);
                    url = QueryString.setParam(url, "handicap1", handicap1);
                    url = QueryString.setParam(url, "handicap2", handicap2);
                    url = QueryString.setParam(url, "betcheck", betcheck);
                    url = QueryString.setParam(url, "betteam", betteam);

                    $.ajax({
                        type: "GET",
                        contentType: 'application/json',
                        dataType: "json",
                        cache: false,
                        url: url,
                        success: function (response) {
                            var length = response.Data === null ? 0 : response.Data.length;
                            if (length > 0) {
                                var choice = "";
                                $.each(response.Texts, function (index, item) {
                                    choice += item;
                                });

                                choice += "<div class='line'></div>";

                                $.each(response.Data, function (index, item) {
                                    if (index === length - 1) {
                                        choice += item.Choice;
                                    }
                                    else {
                                        choice += item.Choice + "<div class='line'></div>";
                                    }
                                });

                                $divEvent.html(choice).show();
                                $nameDiv.hide();
                            }
                        },
                        beforeSend: Site.blockUI,
                        complete: Site.unBlockUI
                    });
                }
                else {
                    $divEvent.hide().html("");
                    $nameDiv.show();
                    $(".ui-jqgrid-bdiv").css("height", "auto");
                }
            },

            showSystemParlayDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
                if (Number(transId) === 0) { return; }

                var url = Site.resolveClientUrl('SystemParlayBetList/Detail');
                url = QueryString.setParam(url, "refno", refNo);
                url = QueryString.setParam(url, "betteam", betTeam);
                url = QueryString.setParam(url, "basecurrency", baseCurrency);

                showDetailPopUp(url, { width: 775, height: 500, title: betTeam });
            },

            showMicroGamingRnGBetsDetails: function (transId, winlostDate, custId, transType) {
                var url = Site.resolveClientUrl('MicroGamingRngBetList/Detail');
                url = QueryString.setParam(url, "transid", transId);
                url = QueryString.setParam(url, "custid", custId);
                url = QueryString.setParam(url, "transType", transType);

                var popH = Math.min($(window).height() - 100, 550);
                var title = Site.getSingleResource("Microgaming") + " - " +
                    Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");

                showDetailPopUp(url, { width: 870, height: popH + 50, title: title });
            },

            showAGCasinoDetails: function (transId, winlostDate, custId, transType) {
                var url = Site.resolveClientUrl('AGCasinoBetList/Detail');
                url = QueryString.setParam(url, "transid", transId);
                url = QueryString.setParam(url, "custid", custId);
                url = QueryString.setParam(url, "transType", transType);

                var popH = Math.min($(window).height() - 100, 550);
                var title = Site.getSingleResource("AGCasino") + " - " +
                    Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");

                showDetailPopUp(url, { width: 870, height: popH + 50, title: title });
            },

            showRnGCasinoDetails: function (transId, winlostDate, custId, transType) {
                var url = Site.resolveClientUrl('RnGCasinoBetList/Detail');
                url = QueryString.setParam(url, "transId", transId);
                url = QueryString.setParam(url, "winlostDate", winlostDate);
                url = QueryString.setParam(url, "custId", custId);
                url = QueryString.setParam(url, "transType", transType);

                var popH = Math.min($(window).height() - 100, 550);
                var title = Site.getSingleResource("ProductCS") + " - " +
                    Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },

            showVoidBridgeDetails: function (transId, winlostDate, custId, transType) {
                var url = Site.resolveClientUrl('VoidbridgeBetList/Detail');
                url = QueryString.setParam(url, "transId", transId);
                url = QueryString.setParam(url, "winlostDate", winlostDate);
                url = QueryString.setParam(url, "custId", custId);
                url = QueryString.setParam(url, "transType", transType);

                var popH = Math.min($(window).height() - 100, 550);
                var title = Site.getSingleResource("ProductVB") + " - " +
                    Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showAllBetDetails: function (transId, winlostDate, custId, transType) {
                var url = Site.resolveClientUrl('AllBetBetList/Detail');
                url = QueryString.setParam(url, "transid", transId);
                url = QueryString.setParam(url, "custid", custId);
                url = QueryString.setParam(url, "transType", transType);
                url = QueryString.setParam(url, "date", winlostDate);

                var popH = Math.min($(window).height() - 100, 550);
                var title = Site.getSingleResource("ProductAB") + " - " +
                    Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");

                showDetailPopUp(url, { width: 870, height: popH + 50, title: title });
            },
            showColossusBetsDetails: function (refNo, winlostDate, transId, custId, betCheck) {
                var params = $.parseJSON($(BETLIST_META_MODEL).val());
                var url = Site.resolveClientUrl('ColossusBetBetList/Detail');
                url = QueryString.setParam(url, "refNo", refNo);
                url = QueryString.setParam(url, "date", winlostDate);
                url = QueryString.setParam(url, "transid", transId);
                url = QueryString.setParam(url, "custid", custId);
                url = QueryString.setParam(url, "betCheck", betCheck);
                url = QueryString.setParam(url, "BaseCurrency", params.BaseCurrency);

                var popH = Math.min($(window).height() - 100, 550);
                var title = Site.getSingleResource("TicketDetailOf").replace("{0}", refNo);

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showGlobalGamingDetails: function (transId, winlostDate, custId, transType) {
                var params = $.parseJSON($(BETLIST_META_MODEL).val());
                var url = Site.resolveClientUrl('GlobalGamingBetList/Detail');
                url = QueryString.setParam(url, "transId", transId);
                url = QueryString.setParam(url, "winlostDate", winlostDate);
                url = QueryString.setParam(url, "custId", custId);
                url = QueryString.setParam(url, "transType", transType);
                url = QueryString.setParam(url, "BaseCurrency", params.BaseCurrency);

                var popH = Math.min($(window).height() - 100, 550);
                var title = Site.getSingleResource("ProductFW") + " - " +
                    Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            ValidProducts: function () {
                var temp = true;
                if ($("#ProductList input:checked").length === 0) {
                    temp = false;
                    var title = Site.getSingleResource("LrfNotification");
                    var dialogForm = '<div title="' + title + '"><p class="notification">' + Site.getSingleResource("AlertChooseProductMsg") + '</p></div>';
                    Popup.openPopup(dialogForm, { width: 350, height: 120, title: title, className: "popup-notification" });
                }

                return temp;
            },

            ViewCasinoHandHistory: function (refNo) {
                var url = Site.resolveClientUrl('HandHistory/RNGCasino');
                var urlParam = $("#casino-hand-history-" + refNo).attr('url-param');

                url += urlParam;
                Site.redirect(url);
            },

            viewCashOutHistory: function (transId, refNo, betType, betdaqId) {
                var $baseCurrency = $("#BaseCurrency");
                var baseCurrency = $baseCurrency.length ? $baseCurrency.val() : $.parseJSON($(BETLIST_META_MODEL).val()).BaseCurrency;

                var isCashOut = betdaqId < 1 || betType === BETTYPE_CASHOUT;
                var url = Site.resolveClientUrl('CashOutBetList/Detail');
                url = QueryString.setParam(url, "transId", transId);
                url = QueryString.setParam(url, "refNo", refNo);
                url = QueryString.setParam(url, "betType", betType);
                url = QueryString.setParam(url, "betdaqId", betdaqId);
                url = QueryString.setParam(url, "BaseCurrency", baseCurrency);
                url = QueryString.setParam(url, "isCashout", isCashOut);

                var popH = Math.min($(window).height() - 100, 550);
                var title = isCashOut ? Site.getSingleResource("CashOutDetails") : Site.getSingleResource("BoughtDetails");

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },

            showBonusDetails: function (refCode, transId) {
                var $baseCurrency = $("#BaseCurrency");
                var baseCurrency = $baseCurrency.length ? $baseCurrency.val() : $.parseJSON($(BETLIST_META_MODEL).val()).BaseCurrency;

                var url = Site.resolveClientUrl('BonusBetList/Detail');
                url = QueryString.setParam(url, "RefCode", refCode);
                url = QueryString.setParam(url, "TransId", transId);
                url = QueryString.setParam(url, "BaseCurrency", baseCurrency);

                var popH = Math.min($(window).height() - 100, 550), popW = 840;
                var title = Site.getSingleResource("TransactionDetail");

                showDetailPopUp(url, { width: popW + 50, height: popH + 50, title: title });
            },

            registerBetListGlobalFunctions: function () {
                window.ViewResult = _me.viewResult;

                window.showMP = function (transId) {
                    var divEvent = $('#divEvent_' + transId);
                    divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
                    _me.showMixParlayDetail(transId, divEvent);
                };

                window.showCombMP = function (transId, index) {
                    var divEvent = $('#divEvent_' + transId + "" + index);
                    divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
                    _me.showMixParlayDetail(transId, divEvent);
                };

                window.showNewCombMP = _me.showNewParlayDetail;
                window.showCombMPDetail = _me.showSystemParlayDetail;
                window.showMicroGamingRnGBetsDetails = _me.showMicroGamingRnGBetsDetails;
                window.showAGCasinoDetails = _me.showAGCasinoDetails;
                window.showRnGCasinoDetails = _me.showRnGCasinoDetails;
                window.showVoidBridgeDetails = _me.showVoidBridgeDetails;
                window.showVoidbridgeResult = _me.viewVoidbridgeResult;
                window.showTodayRunningBetsDetails = _me.showAllBetDetails;
                window.showColossusBetsDetails = _me.showColossusBetsDetails;
                window.showGlobalGaming = _me.showGlobalGamingDetails;
                window.ViewCashOutHistory = _me.viewCashOutHistory;
                window.ShowBonusDetails = _me.showBonusDetails;

                var betList = {
                    ViewCasinoHandHistory: _me.ViewCasinoHandHistory
                };
                window.BetList = betList;
            },

            resizeGrid: function () {
                var grid = $("#BetListGrid");
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth(grid);
            }
        };

        return _me;
    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 25:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
    "use strict";
    var columnNames = ['#', 'ExternalId', 'SystemId', 'TransTime', 'Choice', 'Odds', 'Stake',
                    'WinLoss', 'EffectiveBettingValue', '@[LicenseeWL]/@[Comm]', 'Status'];
    var indexColumn =
    {
        name: 'Index',
        index: 'Index',
        width: 50,
        align: 'center',
        sortable: false,
        resizable: false
    };
    var externalId =
    {
        name: 'ExternalId',
        index: 'ExternalId',
        width: 100,
        align: 'left',
        sortable: false
    };
    var systemId =
    {
        name: 'SystemId',
        index: 'UserName',
        width: 120,
        align: 'left',
        sortable: false
    };
    var transTime =
    {
        name: 'TransTime',
        index: 'TransTime',
        width: 110,
        align: 'center',
        sortable: false,
        resizable: false,
        title: false,
        classes: 'transtime'
    };
    var choice =
    {
        name: 'Choice',
        index: 'Choice',
        width: 300,
        align: 'right',
        sortable: false,
        title: false
    };
    var odds =
    {
        name: 'Odds',
        index: 'Odds',
        width: 60,
        align: 'right',
        sortable: false,
        resizable: false,
        title: false
    };
    var stake =
    {
        name: 'Stake',
        index: 'Stake',
        width: 70,
        align: 'right',
        sortable: true,
        resizable: false
    };
    var winloss =
    {
        name: 'WinLoss',
        index: 'WinLoss',
        width: 75,
        align: 'right',
        sortable: true,
        resizable: false,
        title: false
    };
    var betEffective =
    {
        name: 'BetEffective',
        index: 'BetEffective',
        width: 115,
        align: 'right',
        sortable: true,
        resizable: false,
        title: false
    };
    var licenseeWl =
    {
        name: 'LicenseeWL',
        index: 'LicenseeWL',
        width: 85,
        align: 'right',
        sortable: true,
        resizable: false,
        title: false
    };
    var status =
    {
        name: 'Status',
        index: 'Status',
        width: 60,
        align: 'center',
        sortable: false,
        resizable: false,
        title: false
    };

    var columnModel = [indexColumn, externalId, systemId, transTime, choice, odds, stake, winloss, betEffective, licenseeWl, status];

    return {
        gridOption:
        {
            datatype: 'local',
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
            pager: '#pager2',
            loadtext: '',
            viewrecords: true,
            autowidth: true,
            shrinkToFit: true,
            scroll: false,
            loadonce: true,
            gridview: true,
            userDataOnFooter: true,
            jsonReader: { repeatitems: false },
            cmTemplate: { sortable: false },
            beforeSelectRow: function () {
                return false;
            },
            onRightClickRow: function () {
                return false;
            },
            onPaging: function (pgButton) {
                if ($("#" + pgButton).hasClass("ui-state-disabled") === false) {
                    Site.blockUI();
                }
            }
        },

        columnModel: columnModel,
        columnNames: columnNames,
        maxHeight: 720
    };
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 264:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(265);


/***/ }),

/***/ 265:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(266)], __WEBPACK_AMD_DEFINE_RESULT__ = function (CustomerOutStanding) {
    $(CustomerOutStanding.initialize);
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(0)))

/***/ }),

/***/ 266:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1), __webpack_require__(3), __webpack_require__(17), __webpack_require__(267), __webpack_require__(22),
            __webpack_require__(11), __webpack_require__(19)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString, Report, DailyStatement, BetList, DatePicker, MultiSelect) {
        var _viewName;
        var _viewOption;

        function getViewOption() {
            return {
                "betlistrunning": {
                        title: Site.getSingleResource("BetListRunning"),
                        hiddenColumn: "externalid,systemid,winloss,licenseewl",
                        submit: _me.submitRunningBetList
                    },
                "betlist": {
                        title: Site.getSingleResource("BetList"),
                        hiddenColumn: "externalid,systemid",
                        submit: _me.submitCompletedBetList
                    },
                "dailystatement": {
                        title: Site.getSingleResource("DailyStatement"),
                        hiddenColumn: "externalid,systemid",
                        submit: _me.submitDailyStatementBetList
                    }
            };
        }

        function setProductOptionParamList(params) {
            var productCodeList = $("#grid-products-multiselect").multiSelect("getCheckedItems");
            params.ProductCodeList = productCodeList.toString();
            return params;
        }

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initDateRangePicker();
                MultiSelect.initProductList();

                _viewName = $("#ViewOption").val();
                _viewOption = getViewOption();
                _me.updatePopupTitle();

                //show or hide products dropdown list by ViewOption
                _me.showHideProductsList();

                //Set columns need hide
                _me.setHiddenColumns();

                _me.initEvent();

                var params = _me.getParamsSubmit();
                if (params.ProductCodeList) {
                    _me.submit(params);
                }
                else {
                    $("#ProductList").addClass("invalid");
                    $("#SubmitButton").prop("disabled", true);
                }
            },
            initEvent: function () {
                $("#ViewOption").change(function () {
                    Site.blockUI();
                    var betlistName = $(this).val();
                    var url = Site.getCurrentUrl();
                    url = QueryString.setParam(url, "betlistname", betlistName);
                    url = Site.setProductOptionOriginalParamList(url);

                    Site.redirect(url);
                });

                $("#SubmitButton").click(function () {
                    Report.setOriginalProduct();
                    var params = _me.getParamsSubmit();
                    _me.submit(params);
                });

                $("#report-container").on("click", ".betlistDrilldown", function () {
                    var param = $(this).data("param");
                    _me.viewDailyStatementBetList(param);
                });
            },

            updatePopupTitle: function () {
                var title = _viewOption[_viewName].title;
                var $frame = parent.$("#popupFrame");
                var $popUp = $frame.closest(".flat-popup");
                $popUp.find(".txt-title").text(title);
            },

            setHiddenColumns: function () {
                //Set string columns need to hide
                var columsHidden = _viewOption[_viewName].hiddenColumn;
                $("#HiddenColumnList").val(columsHidden);
            },

            showHideProductsList: function () {
                var $productsListContainer = $("#ProductList");
                var isHideProductList = _viewName === "dailystatement";

                $productsListContainer.parent().toggleClass("hide", isHideProductList);
            },

            getParamsSubmit: function () {
                var params = $.parseJSON($("#BetListMetaModel").val());
                var url = Site.getCurrentUrl();
                params.CustId = QueryString.getParam(url, "custid");
                params.ExternalId = QueryString.getParam(url, "externalid");
                params.SystemId = QueryString.getParam(url, "systemid");
                params.FromDate = $("#FromDate").val();
                params.ToDate = $("#ToDate").val();

                return setProductOptionParamList(params);
            },

            submitCompletedBetList: function (params) {
                var url = Site.resolveClientUrl('api/OutstandingCompletedBetList/Index');
                BetList.loadData(params, url);
            },

            submitRunningBetList: function (params) {
                var url = Site.resolveClientUrl('api/OutstandingRunningBetList/Index');
                BetList.loadData(params, url);
            },

            submitDailyStatementBetList: function (params) {
                var url = Site.resolveClientUrl('api/OutstandingDailyStatementBetList/Index');
                DailyStatement.loadData(params, url);
            },

            submit: function (params) {
                BetList.ValidProducts();
                BetList.registerBetListGlobalFunctions();

                _viewOption[_viewName].submit(params);
            },

            viewDailyStatementBetList: function (param) {
                BetList.registerBetListGlobalFunctions();

                var url = Site.resolveClientUrl('api/DailyStatementBetList/Index');
                var postData = $.parseJSON($("#BetListMetaModel").val());

                postData.CustId = param.CustId;
                postData.FromDate = param.Date;
                postData.TransType = param.TransType;
                postData.ExternalId = param.ExternalId;
                postData.SystemId = param.SystemId;

                $('.page-form').remove();
                BetList.loadData(postData, url);
            }
        };

        return _me;
    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 267:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1), __webpack_require__(3), __webpack_require__(268), __webpack_require__(22),
    __webpack_require__(4), __webpack_require__(8), __webpack_require__(9)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString, DailyStatementOption, BetList, jqGridAdditional) {
        var _me = {
            loadData: function (params, url) {
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(params),
                    contentType: 'application/json',
                    dataType: "json",
                    cache: false,
                    url: url,
                    success: function (response) {
                        _me.bindData(response);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                });
            },

            getGridOption: function () {
                var gridOption = DailyStatementOption.gridOption;
                gridOption.colNames = DailyStatementOption.getColNames();

                return gridOption;
            },

            bindData: function (response) {
                jqGridAdditional.clearGrid("BetListGrid");
                var data = response.Main;
                if (data.length === 0) {
                    jqGridAdditional.setNoInfo();
                    return;
                }

                var gridOption = _me.getGridOption();
                gridOption.data = data;
                gridOption.gridComplete = function () {
                    jqGridAdditional.gridComplete();
                    jqGridAdditional.updateGridWidth($(this));
                    jqGridAdditional.updateGridHeight();
                };

                $("#BetListGrid").jqGrid(gridOption);
            }
        };

        return _me;
    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 268:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
    var setLink = function (rowObject) {
        var drilldownParam = {
            CustId: rowObject.CustId,
            Date: rowObject.Date,
            TransType: rowObject.TransType,
            ExternalId: rowObject.ExternalId,
            SystemId: rowObject.SystemId
        };

        return rowObject.TransTypeHasBetList ?
            '<a href="javascript:;" class="betlistDrilldown" data-param=\'' + JSON.stringify(drilldownParam) + '\'>' + rowObject.FullStatementDate + '</a>'
            : rowObject.FullStatementDate;
    };

    var indexColumn = {
        name: 'Index',
        index: 'Index',
        width: 30,
        align: 'center',
        sortable: false,
        resizable: false
    };
    var date = {
        name: 'Date',
        index: 'Date',
        width: 150,
        align: 'center',
        sortable: false,
        formatter: function (cellValue, option, rowObject) {
            return setLink(rowObject);
        }
    };
    var remark = {
        name: 'Remark',
        index: 'Remark',
        width: 205,
        align: 'right',
        sortable: false
    };
    var turnover = {
        name: 'Turnover',
        index: 'Turnover',
        width: 120,
        align: 'right',
        sortable: false
    };

    var creditDebit = {
        name: 'CreditDebit',
        index: 'CreditDebit',
        width: 120,
        align: 'right',
        sortable: false
    };

    var commission = {
        name: 'Commission',
        index: 'Commission',
        width: 120,
        align: 'right',
        sortable: false,
        resizable: false
    };
    var balance = {
        name: 'Balance',
        index: 'Balance',
        width: 120,
        align: 'right',
        sortable: false
    };

    var columnNames = ['#', 'Date', 'Remark', 'Turnover', 'CreditDebit', 'Commission', 'Balance'];
    var columnModel = [indexColumn, date, remark, turnover, creditDebit, commission, balance];

    var _me = {
        gridOption: {
            datatype: 'local',
            height: "auto",
            rowNum: 5000,
            colModel: columnModel,
            colNames: [],
            autowidth: false,
            shrinkToFit: true,
            gridview: true,
            footerrow: false,
            cmTemplate: { sortable: false }
        },
        getColNames: function () {
            return Site.getResources(columnNames);
        }
    };

    return _me;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 4:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿/// <reference path="/@JSense.js" />
///
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(0), __webpack_require__(1), __webpack_require__(10)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, BigNumber) {
    "use strict";
    var jqGridAdditional = (function () {
        return {
            DECIMAL_FORMAT_OPTIONS: {
                decimalSeparator: ".",
                thousandsSeparator: ",",
                decimalPlaces: 2
            },
            updateGridHeight: function () {
                var $bdiv = $(".ui-jqgrid-bdiv");
                if ($bdiv.length === 0) {
                    return;
                }

                var gridTop = $bdiv.offset().top;
                var pagingHeight = $(".paging:visible").length ? $(".paging:visible").height() : 0;
                pagingHeight = pagingHeight || ($("#paging:visible").length ? $("#paging:visible").height() : 0);
                var footerHeight = $(".ui-jqgrid-sdiv:visible").length ? $(".ui-jqgrid-sdiv:visible").height() : 0;
                var jqgridPagerHeight = $(".ui-jqgrid-pager:visible").length ? $(".ui-jqgrid-pager:visible").outerHeight(true) : 0;
                var footerSite = 18;

                var gridHeight = window.innerHeight - gridTop - footerHeight - jqgridPagerHeight - pagingHeight - footerSite - 15;
                gridHeight = Math.max(120, gridHeight);

                $bdiv.css("max-height", gridHeight);
            },
            updateGridWidth: function (grid) {
                if ($("div.ui-jqgrid-hbox").length === 0) {
                    return;
                }

                grid.setGridParam({
                    shrinkToFit: false
                });

                var parentWidth = grid.closest(".theme_jqGrid").width();
                grid.jqGrid('setGridWidth', parentWidth);

                var tableWidth = parentWidth - 20;
                grid.css("width", tableWidth + "px");
                $(".theme_jqGrid .ui-jqgrid-hdiv table.ui-jqgrid-htable").css("width", tableWidth + "px");
                $(".theme_jqGrid .ui-jqgrid-sdiv table.ui-jqgrid-ftable").css("width", tableWidth + "px");

                jqGridAdditional.updateFooterWidth();
            },

            updateFooterWidth: function () {
                $(".footer-firstrow").remove();
                var settingWidthRow = $(".ui-jqgrid-bdiv table.ui-jqgrid-btable tr.jqgfirstrow").clone();
                settingWidthRow.addClass("footer-firstrow").removeClass("jqgfirstrow")
                                .prependTo($("tbody", "table.ui-jqgrid-ftable"));
            },

            formatInteger: function (value) {
                if (value !== undefined && value !== null) {
                    if (value === "NaN") {
                        return "NaN";
                    }

                    var sRegExp = new RegExp('(-?[0-9]+)([0-9]{3})');
                    var resultValue = value + '';

                    while (sRegExp.test(resultValue)) {
                        resultValue = resultValue.replace(sRegExp, '$1,$2');
                    }

                    return resultValue;
                }
                return value;
            },

            formatDecimal: function (value) {
                if (value !== undefined && value !== null) {
                    if (isNaN(value)) {
                        return "NaN";
                    }

                    return new BigNumber(value.toString()).toFormat(2);
                }

                return value;
            },

            formatLicenseeWl: function (value) {
                var decimalValue = jqGridAdditional.formatDecimal(value);
                return value < 0 ? "<span class='negative-number'>" + decimalValue + "</span>" : decimalValue;
            },

            excludeCol: function (colName, colModel, excludedCol) {
                var result =
                {
                    colName: [],
                    colModel: []
                };
                for (var i = 0; i < colModel.length; i++) {
                    if ($.inArray(colModel[i].name, excludedCol) < 0) {
                        result.colName.push(colName[i]);
                        result.colModel.push(colModel[i]);
                    }
                }
                return result;
            },

            hideColumn: function (models, colName) {
                for (var i = 0; i < models.length; i++) {
                    if (models[i].name.toLowerCase() === colName.toLowerCase()) {
                        models[i].hidden = true;
                        break;
                    }
                }
            },

            setGroupCustomerWLColumns: function (grid) {
                grid.jqGrid('setGroupHeaders',
                {
                    useColSpanStyle: true,
                    groupHeaders: [{
                        startColumnName: 'CustomerWinLoss',
                        numberOfColumns: 3,
                        titleText: Site.getSingleResource('CustomerW_L')
                    }]
                });
            },

            resetHeaderHeight: function () {
                $(".ui-jqgrid-htable>thead>tr.ui-jqgrid-labels > th.ui-th-column div").addClass("header-two-line");
            },

            getNewLicenseeWLNote: function (productList, caption) {
                var temp = caption || Site.getSingleResource("LicenseeW_L");
                var productTemp = "";
                var productAds = ["SB", "LC", "NG", "VS", "CS", "BG", "KN", "BT", "MGCLC", "MGCRNG", "VB"];
                for (var i = 0; i < productList.length; i++) {
                    if ($.inArray(productList[i], productAds) > -1) {
                        productTemp += productTemp.length === 0 ? productList[i] : ", " + productList[i];
                    }
                }
                if (productTemp.length > 0) {
                    temp += "(" + productTemp + ")";
                }
                else {
                    return caption || "";
                }

                return temp;
            },
            // update getNewLicenseeWLNote() as well
            getLicenseeWLNote: function (productOptionJson, caption) {
                var productOptions = $.parseJSON(productOptionJson);
                var temp = caption || Site.getSingleResource("LicenseeW_L");
                var productTemp = "";
                var productAds = ["SB", "LC", "NG", "VS", "CS", "BG", "KN", "BT", "MGCLC", "MGCRNG", "VB"];
                for (var i = 0; i < productOptions.length; i++) {
                    if ($.inArray(productOptions[i].Code, productAds) > -1 && Site.asBool(productOptions[i].Status)) {
                        productTemp += productTemp.length === 0 ? productOptions[i].Code : ", " + productOptions[i].Code;
                    }
                }
                if (productTemp.length > 0) {
                    temp += "(" + productTemp + ")";
                }
                else {
                    return caption || "";
                }

                return temp;
            },

            gridComplete: function () {
                $(".ui-jqgrid-titlebar").hide();
                $(".ui-jqgrid-hdiv").css("clear", "both");

                $('#noInfoRow').remove();
            },

            clearGrid: function (reportId) {
                $('#' + reportId).jqGrid('GridUnload');
                $("div.theme_jqGrid > div").css('width', '');
            },

            rowSpanSetting: function (rowId, val, rawObject) {
                var rowSpan = rawObject.RowSpan, result;
                if (rowSpan > 1) {
                    result = ' rowspan=' + '"' + rowSpan + '"';
                }
                else if (rowSpan < 0) {
                    result = ' style="display:none;" ';
                }
                else {
                    result = '';
                }

                return result;
            },

            rowSpanSetting2: function (rowId, val, rawObject) {
                var rowSpan = rawObject.RowSpan2, result;
                if (rowSpan > 1) {
                    result = ' rowspan=' + '"' + rowSpan + '"';
                }
                else if (rowSpan < 0) {
                    result = ' style="display:none;" ';
                }
                else {
                    result = '';
                }

                return result;
            },

            assignTotal: function (totalData, columnModel) {
                var mainFooter = $(".ui-jqgrid-ftable tr:last-child");

                if (mainFooter.length >= 0) {
                    //add grand total
                    $(totalData).each(function (totalIndex, totalElement) {
                        var row = mainFooter.clone();
                        row.addClass("grandTotal");
                        var cellList = $("td", row);
                        $(columnModel).each(function (cellIndex, modelElement) {
                            var cell = cellList.get(cellIndex);
                            var isCustomFormater = typeof (modelElement.formatter) === "function";
                            var rawValue = totalElement[modelElement.name];
                            var value;
                            if (isCustomFormater) {
                                value = modelElement.formatter(rawValue, modelElement.formatoptions, row);
                            } else {
                                var formatOptions = {};
                                formatOptions[modelElement.formatter] = modelElement.formatoptions;
                                value = $.fn.fmatter(modelElement.formatter, rawValue, formatOptions);
                            }

                            var titleValue = value;
                            if ($.isNumeric(rawValue) && rawValue < 0) {
                                var wrapper = document.createElement('div');
                                wrapper.innerHTML = value;
                                titleValue = wrapper.outerHtml;
                            }
                            $(cell).html(value).attr("title", titleValue);
                        });
                        row.insertBefore(mainFooter);
                    });
                }
            },

            setFooterColumns: function (param, columnNames) {
                var isShow = "showCol" === param;
                var footers = $("table.ui-jqgrid-ftable tr");
                if (isShow) {
                    $.each(columnNames, function (index, value) {
                        $('td[aria-describedby="' + value + '"]', footers).show();
                    });
                }
                else {
                    $.each(columnNames, function (index, value) {
                        $('td[aria-describedby="' + value + '"]', footers).hide();
                    });
                }
            },

            setNoInfo: function (message) {
                var noInfoText = message || Site.getSingleResource("NoInfo");
                var htmlNoInfor = '<p id="noInfoRow" class="no-info-row">' + noInfoText + '</p>';

                var $noInforRow = $('#noInfoRow');
                if ($noInforRow.length) {
                    $noInforRow.html(noInfoText);
                }
                else {
                    $('.theme_jqGrid').append(htmlNoInfor);
                }
                $("div.ui-jqgrid-sdiv").hide();
            },

            setNoInfoWithColspan: function (colspan) {
                var mainFooter = $(".ui-jqgrid-ftable");
                var info = Site.getSingleResource("NoInfo");

                if (mainFooter.length > 0) {
                    mainFooter.remove();
                }
                var contentTable = $("table.ui-jqgrid-htable");
                if (contentTable.find(".no-info-row").length === 0) {
                    var temps = '<tr role="row" class="no-info-row  ui-widget-content jqgrow ui-row-ltr" style="height:auto;">' +
                        ' <td style="text-align:center;" role="role="gridcell" colspan="' + colspan + '">' + info + '</td></tr>';
                    contentTable.append(temps);
                }

                $("div.ui-jqgrid-sdiv").hide();
            },

            setGroupFooters: function (colspan, summaryColspan, summaryRowCount, allowScrolling) {
                /// <summary>
                /// set colspan for footer rows
                /// </summary>
                /// <param name="colspan"></param>
                /// <param name="summaryColspan"></param>
                /// <param name="summaryRowCount"></param>

                summaryRowCount = summaryRowCount === undefined ? 0 : summaryRowCount;
                var footerTable = $("table.ui-jqgrid-ftable");
                var footerRowList = $("tr", footerTable);
                var rowCount = footerRowList.length;
                var columnIndex = 0;
                if (rowCount > 0) {
                    // Hide other columns to match the colspan & rowspan
                    var processFooter = function ($td, colspan) {
                        $td.each(function (j, cell) {
                            if (columnIndex < colspan) {
                                if ($(cell).is(":visible")) {
                                    $(cell).hide();
                                    columnIndex++;
                                }
                            }
                        });
                    };

                    for (var i = 0; i < rowCount - summaryRowCount; i++) {
                        columnIndex = 0;
                        processFooter($("td ", footerRowList[i]), colspan);
                    }

                    //#region Render total eur section
                    columnIndex = 0;
                    // Hide other eur columns to match the colspan & rowspan
                    for (var j = rowCount - summaryRowCount; j < rowCount; j++) {
                        columnIndex = 0;
                        processFooter($("td ", footerRowList[j]), summaryColspan);
                    }
                    var totalInText = Site.getSingleResource("Totalin");
                    //#region Render total eur section

                    var totalText = Site.getSingleResource("Total");
                    // Set colspan for total section
                    if (rowCount > 1) {
                        // Show total text
                        $("td:first-child", footerRowList[0]).attr("colspan", colspan)
                                .attr("rowspan", rowCount - summaryRowCount).css("display", "")
                                .css("width", "").html(totalText);
                    }
                    else {
                        $("td:first-child", footerRowList[0]).css("display", "").attr("colspan", summaryColspan)
                            .css("width", "").html(totalInText + " EUR");
                        //remove border style
                        $(".ui-jqgrid-sdiv .ui-jqgrid-hbox").css("border-top", "none");
                    }

                    // If there are many total rows such as total eur, total usd, ...
                    var currencyListString = footerTable.attr("data-total-currency-list");
                    if (currencyListString) {
                        var currencyList = currencyListString.split(",");
                        var rowspan = summaryRowCount / currencyList.length;
                        $(currencyList).each(function (i, currency) {
                            $("td.group-" + currency + ":first-child", footerTable)
                                    .attr("rowspan", rowspan).css("display", "")
                                    .attr("colspan", summaryColspan).html(totalInText + " " + currency);
                            $("td.group-" + currency + ":gt(0)", footerTable).hide();
                            $("td.group-" + currency + ":first-child", footerTable).parent().addClass("summarytotal");
                        });
                    } else {
                        if (footerRowList.length > 1) {
                            $(footerRowList[footerRowList.length = 1]).hide();
                        }
                    }
                    /*set scrolling*/
                    if (allowScrolling) {
                        footerTable.closest(".ui-jqgrid-sdiv").addClass("jqgrid-footer-scroll-hacking");
                    }
                }
            },

            addSortableHeader: function ($grid, sortColumn, sortOrder, isLightText) {
                $grid.closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-sortable")
                        .each(function (index, element) {
                            var $this = $(element);
                            $this.find(".s-ico").remove();

                            if (element.id.indexOf(sortColumn) > -1) {
                                var sortClass = "sort-icon-" + sortOrder + (isLightText ? " light" : "");
                                var sortSpan = $("<span class='sort-icon'></span>");
                                sortSpan.addClass(sortClass);
                                $this.append(sortSpan);
                                $this.addClass("sortable sort-heading sorted-heading");
                            }
                        });
            },

            updateSortableHeader: function ($grid, sortColumn, sortOrder, isLightText) {
                var sortClass = "sort-icon-" + sortOrder + (isLightText ? " light" : "");
                sortColumn = "_" + sortColumn;

                $grid.closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-sortable")
                        .each(function (index, element) {
                            var $this = $(element);
                            $this.find(".s-ico").remove();

                            if (element.id.endsWith(sortColumn)) {
                                var sortSpan = $this.find(".sort-icon");
                                if (sortSpan.length) {
                                    sortSpan.removeClass().addClass("sort-icon " + sortClass);
                                }
                                else {
                                    sortSpan = $("<span class='sort-icon'></span>");
                                    sortSpan.addClass(sortClass);
                                    $this.append(sortSpan);
                                    $this.addClass("sortable sort-heading sorted-heading");
                                }
                            }
                            else {
                                $this.removeClass("sortable sort-heading sorted-heading");
                                $this.find(".sort-icon").remove();
                            }
                        });
            },

            removeCursorOnNonSortableHeader: function ($grid) {
                var cm = $grid[0].p.colModel;
                $.each($grid[0].grid.headers, function (index, value) {
                    var cmi = cm[index];
                    if (!cmi.sortable) {
                        $('.ui-jqgrid-sortable', value.el).css({ cursor: "default" });
                    }
                });
            },

            addSortableHeaders: function ($grid, sortColumn, sortOrder) {
                var cm = $grid[0].p.colModel;
                $.each($grid[0].grid.headers, function (index, value) {
                    var cmi = cm[index];
                    if (!cmi.sortable) {
                        $(".ui-jqgrid-sortable", value.el).css({ cursor: "default" });
                    }
                    if (cmi.index === sortColumn && sortOrder !== "") {
                        var header = $(".ui-jqgrid-sortable", value.el);
                        header.find(".s-ico").remove();
                        header.addClass("sortable sort-heading sorted-heading");
                        var sortClass = "sort-icon sort-icon-" + sortOrder;
                        header.append('<span class="' + sortClass + '"></span>');
                    }
                });
            }
        };
    })();

    return jqGridAdditional;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 7:
/***/ (function(module, exports) {

module.exports = function(module) {
	if(!module.webpackPolyfill) {
		module.deprecate = function() {};
		module.paths = [];
		// module.parent = undefined by default
		if(!module.children) module.children = [];
		Object.defineProperty(module, "loaded", {
			enumerable: true,
			get: function() {
				return module.l;
			}
		});
		Object.defineProperty(module, "id", {
			enumerable: true,
			get: function() {
				return module.i;
			}
		});
		module.webpackPolyfill = 1;
	}
	return module;
};


/***/ }),

/***/ 8:
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function(jQuery) {;(function($){
/**
 * jqGrid English Translation
 * Tony Tomov tony@trirand.com
 * http://trirand.com/blog/ 
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
**/
$.jgrid = $.jgrid || {};
$.extend($.jgrid,{
	defaults : {
		recordtext: "View {0} - {1} of {2}",
		emptyrecords: "No records to view",
		loadtext: "Loading...",
		pgtext : "Page {0} of {1}"
	},
	search : {
		caption: "Search...",
		Find: "Find",
		Reset: "Reset",
		odata: [{ oper:'eq', text:'equal'},{ oper:'ne', text:'not equal'},{ oper:'lt', text:'less'},{ oper:'le', text:'less or equal'},{ oper:'gt', text:'greater'},{ oper:'ge', text:'greater or equal'},{ oper:'bw', text:'begins with'},{ oper:'bn', text:'does not begin with'},{ oper:'in', text:'is in'},{ oper:'ni', text:'is not in'},{ oper:'ew', text:'ends with'},{ oper:'en', text:'does not end with'},{ oper:'cn', text:'contains'},{ oper:'nc', text:'does not contain'},{ oper:'nu', text:'is null'},{ oper:'nn', text:'is not null'}],
		groupOps: [{ op: "AND", text: "all" },{ op: "OR",  text: "any" }],
		operandTitle : "Click to select search operation.",
		resetTitle : "Reset Search Value"
	},
	edit : {
		addCaption: "Add Record",
		editCaption: "Edit Record",
		bSubmit: "Submit",
		bCancel: "Cancel",
		bClose: "Close",
		saveData: "Data has been changed! Save changes?",
		bYes : "Yes",
		bNo : "No",
		bExit : "Cancel",
		msg: {
			required:"Field is required",
			number:"Please, enter valid number",
			minValue:"value must be greater than or equal to ",
			maxValue:"value must be less than or equal to",
			email: "is not a valid e-mail",
			integer: "Please, enter valid integer value",
			date: "Please, enter valid date value",
			url: "is not a valid URL. Prefix required ('http://' or 'https://')",
			nodefined : " is not defined!",
			novalue : " return value is required!",
			customarray : "Custom function should return array!",
			customfcheck : "Custom function should be present in case of custom checking!"
			
		}
	},
	view : {
		caption: "View Record",
		bClose: "Close"
	},
	del : {
		caption: "Delete",
		msg: "Delete selected record(s)?",
		bSubmit: "Delete",
		bCancel: "Cancel"
	},
	nav : {
		edittext: "",
		edittitle: "Edit selected row",
		addtext:"",
		addtitle: "Add new row",
		deltext: "",
		deltitle: "Delete selected row",
		searchtext: "",
		searchtitle: "Find records",
		refreshtext: "",
		refreshtitle: "Reload Grid",
		alertcap: "Warning",
		alerttext: "Please, select row",
		viewtext: "",
		viewtitle: "View selected row"
	},
	col : {
		caption: "Select columns",
		bSubmit: "Ok",
		bCancel: "Cancel"
	},
	errors : {
		errcap : "Error",
		nourl : "No url is set",
		norecords: "No records to process",
		model : "Length of colNames <> colModel!"
	},
	formatter : {
		integer : {thousandsSeparator: ",", defaultValue: '0'},
		number : {decimalSeparator:".", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00'},
		currency : {decimalSeparator:".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix:"", defaultValue: '0.00'},
		date : {
			dayNames:   [
				"Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat",
				"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
			],
			monthNames: [
				"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
				"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
			],
			AmPm : ["am","pm","AM","PM"],
			S: function (j) {return j < 11 || j > 13 ? ['st', 'nd', 'rd', 'th'][Math.min((j - 1) % 10, 3)] : 'th';},
			srcformat: 'Y-m-d',
			newformat: 'n/j/Y',
			parseRe : /[#%\\\/:_;.,\t\s-]/,
			masks : {
				// see http://php.net/manual/en/function.date.php for PHP format used in jqGrid
				// and see http://docs.jquery.com/UI/Datepicker/formatDate
				// and https://github.com/jquery/globalize#dates for alternative formats used frequently
				// one can find on https://github.com/jquery/globalize/tree/master/lib/cultures many
				// information about date, time, numbers and currency formats used in different countries
				// one should just convert the information in PHP format
				ISO8601Long:"Y-m-d H:i:s",
				ISO8601Short:"Y-m-d",
				// short date:
				//    n - Numeric representation of a month, without leading zeros
				//    j - Day of the month without leading zeros
				//    Y - A full numeric representation of a year, 4 digits
				// example: 3/1/2012 which means 1 March 2012
				ShortDate: "n/j/Y", // in jQuery UI Datepicker: "M/d/yyyy"
				// long date:
				//    l - A full textual representation of the day of the week
				//    F - A full textual representation of a month
				//    d - Day of the month, 2 digits with leading zeros
				//    Y - A full numeric representation of a year, 4 digits
				LongDate: "l, F d, Y", // in jQuery UI Datepicker: "dddd, MMMM dd, yyyy"
				// long date with long time:
				//    l - A full textual representation of the day of the week
				//    F - A full textual representation of a month
				//    d - Day of the month, 2 digits with leading zeros
				//    Y - A full numeric representation of a year, 4 digits
				//    g - 12-hour format of an hour without leading zeros
				//    i - Minutes with leading zeros
				//    s - Seconds, with leading zeros
				//    A - Uppercase Ante meridiem and Post meridiem (AM or PM)
				FullDateTime: "l, F d, Y g:i:s A", // in jQuery UI Datepicker: "dddd, MMMM dd, yyyy h:mm:ss tt"
				// month day:
				//    F - A full textual representation of a month
				//    d - Day of the month, 2 digits with leading zeros
				MonthDay: "F d", // in jQuery UI Datepicker: "MMMM dd"
				// short time (without seconds)
				//    g - 12-hour format of an hour without leading zeros
				//    i - Minutes with leading zeros
				//    A - Uppercase Ante meridiem and Post meridiem (AM or PM)
				ShortTime: "g:i A", // in jQuery UI Datepicker: "h:mm tt"
				// long time (with seconds)
				//    g - 12-hour format of an hour without leading zeros
				//    i - Minutes with leading zeros
				//    s - Seconds, with leading zeros
				//    A - Uppercase Ante meridiem and Post meridiem (AM or PM)
				LongTime: "g:i:s A", // in jQuery UI Datepicker: "h:mm:ss tt"
				SortableDateTime: "Y-m-d\\TH:i:s",
				UniversalSortableDateTime: "Y-m-d H:i:sO",
				// month with year
				//    Y - A full numeric representation of a year, 4 digits
				//    F - A full textual representation of a month
				YearMonth: "F, Y" // in jQuery UI Datepicker: "MMMM, yyyy"
			},
			reformatAfterEdit : false
		},
		baseLinkUrl: '',
		showAction: '',
		target: '',
		checkbox : {disabled:true},
		idName : 'id'
	}
});
})(jQuery);

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(0)))

/***/ }),

/***/ 9:
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function(jQuery) {/* 
* jqGrid  4.6.0 - jQuery Grid 
* Copyright (c) 2008, Tony Tomov, tony@trirand.com 
* Dual licensed under the MIT or GPL licenses 
* http://www.opensource.org/licenses/mit-license.php 
* http://www.gnu.org/licenses/gpl-2.0.html 
* Date:2014-02-20 
* Modules: grid.base.js; jquery.fmatter.js; grid.custom.js; grid.common.js; grid.subgrid.js; grid.grouping.js; grid.treegrid.js; grid.pivot.js; JsonXml.js; grid.tbltogrid.js; 
*/
(function(b){b.jgrid=b.jgrid||{};b.extend(b.jgrid,{version:"4.6.0",htmlDecode:function(b){return b&&("&nbsp;"===b||"&#160;"===b||1===b.length&&160===b.charCodeAt(0))?"":b?String(b).replace(/&gt;/g,">").replace(/&lt;/g,"<").replace(/&quot;/g,'"').replace(/&amp;/g,"&"):b},htmlEncode:function(b){return b?String(b).replace(/&/g,"&amp;").replace(/\"/g,"&quot;").replace(/</g,"&lt;").replace(/>/g,"&gt;"):b},format:function(e){var f=b.makeArray(arguments).slice(1);null==e&&(e="");return e.replace(/\{(\d+)\}/g,
function(b,d){return f[d]})},msie:"Microsoft Internet Explorer"===navigator.appName,msiever:function(){var b=-1;null!=/MSIE ([0-9]{1,}[.0-9]{0,})/.exec(navigator.userAgent)&&(b=parseFloat(RegExp.$1));return b},getCellIndex:function(e){e=b(e);if(e.is("tr"))return-1;e=(e.is("td")||e.is("th")?e:e.closest("td,th"))[0];return b.jgrid.msie?b.inArray(e,e.parentNode.cells):e.cellIndex},stripHtml:function(b){b=String(b);var f=/<("[^"]*"|'[^']*'|[^'">])*>/gi;return b?(b=b.replace(f,""))&&"&nbsp;"!==b&&"&#160;"!==
b?b.replace(/\"/g,"'"):"":b},stripPref:function(e,f){var c=b.type(e);if("string"===c||"number"===c)e=String(e),f=""!==e?String(f).replace(String(e),""):f;return f},parse:function(e){"while(1);"===e.substr(0,9)&&(e=e.substr(9));"/*"===e.substr(0,2)&&(e=e.substr(2,e.length-4));e||(e="{}");return!0===b.jgrid.useJSON&&"object"===typeof JSON&&"function"===typeof JSON.parse?JSON.parse(e):eval("("+e+")")},parseDate:function(e,f,c,d){var a=/^\/Date\((([-+])?[0-9]+)(([-+])([0-9]{2})([0-9]{2}))?\)\/$/,l="string"===
typeof f?f.match(a):null,a=function(a,b){a=String(a);for(b=parseInt(b,10)||2;a.length<b;)a="0"+a;return a},g={m:1,d:1,y:1970,h:0,i:0,s:0,u:0},h=0,k,n,h=function(a,b){0===a?12===b&&(b=0):12!==b&&(b+=12);return b};void 0===d&&(d=b.jgrid.formatter.date);void 0===d.parseRe&&(d.parseRe=/[#%\\\/:_;.,\t\s-]/);d.masks.hasOwnProperty(e)&&(e=d.masks[e]);if(f&&null!=f)if(isNaN(f-0)||"u"!==String(e).toLowerCase())if(f.constructor===Date)h=f;else if(null!==l){if(h=new Date(parseInt(l[1],10)),l[3]){var m=60*Number(l[5])+
Number(l[6]),m=m*("-"===l[4]?1:-1),m=m-h.getTimezoneOffset();h.setTime(Number(Number(h)+6E4*m))}}else{m=0;"ISO8601Long"===d.srcformat&&"Z"===f.charAt(f.length-1)&&(m-=(new Date).getTimezoneOffset());f=String(f).replace(/\T/g,"#").replace(/\t/,"%").split(d.parseRe);e=e.replace(/\T/g,"#").replace(/\t/,"%").split(d.parseRe);k=0;for(n=e.length;k<n;k++)"M"===e[k]&&(l=b.inArray(f[k],d.monthNames),-1!==l&&12>l&&(f[k]=l+1,g.m=f[k])),"F"===e[k]&&(l=b.inArray(f[k],d.monthNames,12),-1!==l&&11<l&&(f[k]=l+1-12,
g.m=f[k])),"a"===e[k]&&(l=b.inArray(f[k],d.AmPm),-1!==l&&2>l&&f[k]===d.AmPm[l]&&(f[k]=l,g.h=h(f[k],g.h))),"A"===e[k]&&(l=b.inArray(f[k],d.AmPm),-1!==l&&1<l&&f[k]===d.AmPm[l]&&(f[k]=l-2,g.h=h(f[k],g.h))),"g"===e[k]&&(g.h=parseInt(f[k],10)),void 0!==f[k]&&(g[e[k].toLowerCase()]=parseInt(f[k],10));g.f&&(g.m=g.f);if(0===g.m&&0===g.y&&0===g.d)return"&#160;";g.m=parseInt(g.m,10)-1;h=g.y;70<=h&&99>=h?g.y=1900+g.y:0<=h&&69>=h&&(g.y=2E3+g.y);h=new Date(g.y,g.m,g.d,g.h,g.i,g.s,g.u);0<m&&h.setTime(Number(Number(h)+
6E4*m))}else h=new Date(1E3*parseFloat(f));else h=new Date(g.y,g.m,g.d,g.h,g.i,g.s,g.u);if(void 0===c)return h;d.masks.hasOwnProperty(c)?c=d.masks[c]:c||(c="Y-m-d");e=h.getHours();f=h.getMinutes();g=h.getDate();m=h.getMonth()+1;l=h.getTimezoneOffset();k=h.getSeconds();n=h.getMilliseconds();var r=h.getDay(),p=h.getFullYear(),q=(r+6)%7+1,x=(new Date(p,m-1,g)-new Date(p,0,1))/864E5,G={d:a(g),D:d.dayNames[r],j:g,l:d.dayNames[r+7],N:q,S:d.S(g),w:r,z:x,W:5>q?Math.floor((x+q-1)/7)+1:Math.floor((x+q-1)/7)||
(4>((new Date(p-1,0,1)).getDay()+6)%7?53:52),F:d.monthNames[m-1+12],m:a(m),M:d.monthNames[m-1],n:m,t:"?",L:"?",o:"?",Y:p,y:String(p).substring(2),a:12>e?d.AmPm[0]:d.AmPm[1],A:12>e?d.AmPm[2]:d.AmPm[3],B:"?",g:e%12||12,G:e,h:a(e%12||12),H:a(e),i:a(f),s:a(k),u:n,e:"?",I:"?",O:(0<l?"-":"+")+a(100*Math.floor(Math.abs(l)/60)+Math.abs(l)%60,4),P:"?",T:(String(h).match(/\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g)||
[""]).pop().replace(/[^-+\dA-Z]/g,""),Z:"?",c:"?",r:"?",U:Math.floor(h/1E3)};return c.replace(/\\.|[dDjlNSwzWFmMntLoYyaABgGhHisueIOPTZcrU]/g,function(a){return G.hasOwnProperty(a)?G[a]:a.substring(1)})},jqID:function(b){return String(b).replace(/[!"#$%&'()*+,.\/:; <=>?@\[\\\]\^`{|}~]/g,"\\$&")},guid:1,uidPref:"jqg",randId:function(e){return(e||b.jgrid.uidPref)+b.jgrid.guid++},getAccessor:function(b,f){var c,d,a=[],l;if("function"===typeof f)return f(b);c=b[f];if(void 0===c)try{if("string"===typeof f&&
(a=f.split(".")),l=a.length)for(c=b;c&&l--;)d=a.shift(),c=c[d]}catch(g){}return c},getXmlData:function(e,f,c){var d="string"===typeof f?f.match(/^(.*)\[(\w+)\]$/):null;if("function"===typeof f)return f(e);if(d&&d[2])return d[1]?b(d[1],e).attr(d[2]):b(e).attr(d[2]);e=b(f,e);return c?e:0<e.length?b(e).text():void 0},cellWidth:function(){var e=b("<div class='ui-jqgrid' style='left:10000px'><table class='ui-jqgrid-btable' style='width:5px;'><tr class='jqgrow'><td style='width:5px;display:block;'></td></tr></table></div>"),
f=e.appendTo("body").find("td").width();e.remove();return 0.1<Math.abs(f-5)},cell_width:!0,ajaxOptions:{},from:function(e){return new function(e,c){"string"===typeof e&&(e=b.data(e));var d=this,a=e,l=!0,g=!1,h=c,k=/[\$,%]/g,n=null,m=null,r=0,p=!1,q="",x=[],G=!0;if("object"===typeof e&&e.push)0<e.length&&(G="object"!==typeof e[0]?!1:!0);else throw"data provides is not an array";this._hasData=function(){return null===a?!1:0===a.length?!1:!0};this._getStr=function(a){var b=[];g&&b.push("jQuery.trim(");
b.push("String("+a+")");g&&b.push(")");l||b.push(".toLowerCase()");return b.join("")};this._strComp=function(a){return"string"===typeof a?".toString()":""};this._group=function(a,b){return{field:a.toString(),unique:b,items:[]}};this._toStr=function(a){g&&(a=b.trim(a));a=a.toString().replace(/\\/g,"\\\\").replace(/\"/g,'\\"');return l?a:a.toLowerCase()};this._funcLoop=function(d){var l=[];b.each(a,function(a,b){l.push(d(b))});return l};this._append=function(a){var b;h=null===h?"":h+(""===q?" && ":
q);for(b=0;b<r;b++)h+="(";p&&(h+="!");h+="("+a+")";p=!1;q="";r=0};this._setCommand=function(a,b){n=a;m=b};this._resetNegate=function(){p=!1};this._repeatCommand=function(a,b){return null===n?d:null!==a&&null!==b?n(a,b):null!==m&&G?n(m,a):n(a)};this._equals=function(a,b){return 0===d._compare(a,b,1)};this._compare=function(a,b,d){var e=Object.prototype.toString;void 0===d&&(d=1);void 0===a&&(a=null);void 0===b&&(b=null);if(null===a&&null===b)return 0;if(null===a&&null!==b)return 1;if(null!==a&&null===
b)return-1;if("[object Date]"===e.call(a)&&"[object Date]"===e.call(b))return a<b?-d:a>b?d:0;l||"number"===typeof a||"number"===typeof b||(a=String(a),b=String(b));return a<b?-d:a>b?d:0};this._performSort=function(){0!==x.length&&(a=d._doSort(a,0))};this._doSort=function(a,b){var l=x[b].by,e=x[b].dir,g=x[b].type,c=x[b].datefmt,f=x[b].sfunc;if(b===x.length-1)return d._getOrder(a,l,e,g,c,f);b++;l=d._getGroup(a,l,e,g,c);e=[];for(g=0;g<l.length;g++)for(f=d._doSort(l[g].items,b),c=0;c<f.length;c++)e.push(f[c]);
return e};this._getOrder=function(a,e,g,c,f,h){var m=[],n=[],r="a"===g?1:-1,p,x;void 0===c&&(c="text");x="float"===c||"number"===c||"currency"===c||"numeric"===c?function(a){a=parseFloat(String(a).replace(k,""));return isNaN(a)?0:a}:"int"===c||"integer"===c?function(a){return a?parseFloat(String(a).replace(k,"")):0}:"date"===c||"datetime"===c?function(a){return b.jgrid.parseDate(f,a).getTime()}:b.isFunction(c)?c:function(a){a=a?b.trim(String(a)):"";return l?a:a.toLowerCase()};b.each(a,function(a,
d){p=""!==e?b.jgrid.getAccessor(d,e):d;void 0===p&&(p="");p=x(p,d);n.push({vSort:p,index:a})});b.isFunction(h)?n.sort(function(a,b){a=a.vSort;b=b.vSort;return h.call(this,a,b,r)}):n.sort(function(a,b){a=a.vSort;b=b.vSort;return d._compare(a,b,r)});c=0;for(var q=a.length;c<q;)g=n[c].index,m.push(a[g]),c++;return m};this._getGroup=function(a,c,e,l,g){var f=[],h=null,k=null,m;b.each(d._getOrder(a,c,e,l,g),function(a,e){m=b.jgrid.getAccessor(e,c);null==m&&(m="");d._equals(k,m)||(k=m,null!==h&&f.push(h),
h=d._group(c,m));h.items.push(e)});null!==h&&f.push(h);return f};this.ignoreCase=function(){l=!1;return d};this.useCase=function(){l=!0;return d};this.trim=function(){g=!0;return d};this.noTrim=function(){g=!1;return d};this.execute=function(){var c=h,e=[];if(null===c)return d;b.each(a,function(){eval(c)&&e.push(this)});a=e;return d};this.data=function(){return a};this.select=function(c){d._performSort();if(!d._hasData())return[];d.execute();if(b.isFunction(c)){var e=[];b.each(a,function(a,b){e.push(c(b))});
return e}return a};this.hasMatch=function(){if(!d._hasData())return!1;d.execute();return 0<a.length};this.andNot=function(a,b,c){p=!p;return d.and(a,b,c)};this.orNot=function(a,b,c){p=!p;return d.or(a,b,c)};this.not=function(a,b,c){return d.andNot(a,b,c)};this.and=function(a,b,c){q=" && ";return void 0===a?d:d._repeatCommand(a,b,c)};this.or=function(a,b,c){q=" || ";return void 0===a?d:d._repeatCommand(a,b,c)};this.orBegin=function(){r++;return d};this.orEnd=function(){null!==h&&(h+=")");return d};
this.isNot=function(a){p=!p;return d.is(a)};this.is=function(a){d._append("this."+a);d._resetNegate();return d};this._compareValues=function(a,c,e,l,g){var f;f=G?"jQuery.jgrid.getAccessor(this,'"+c+"')":"this";void 0===e&&(e=null);var h=e,m=void 0===g.stype?"text":g.stype;if(null!==e)switch(m){case "int":case "integer":h=isNaN(Number(h))||""===h?"0":h;f="parseInt("+f+",10)";h="parseInt("+h+",10)";break;case "float":case "number":case "numeric":h=String(h).replace(k,"");h=isNaN(Number(h))||""===h?
"0":h;f="parseFloat("+f+")";h="parseFloat("+h+")";break;case "date":case "datetime":h=String(b.jgrid.parseDate(g.newfmt||"Y-m-d",h).getTime());f='jQuery.jgrid.parseDate("'+g.srcfmt+'",'+f+").getTime()";break;default:f=d._getStr(f),h=d._getStr('"'+d._toStr(h)+'"')}d._append(f+" "+l+" "+h);d._setCommand(a,c);d._resetNegate();return d};this.equals=function(a,b,c){return d._compareValues(d.equals,a,b,"==",c)};this.notEquals=function(a,b,c){return d._compareValues(d.equals,a,b,"!==",c)};this.isNull=function(a,
b,c){return d._compareValues(d.equals,a,null,"===",c)};this.greater=function(a,b,c){return d._compareValues(d.greater,a,b,">",c)};this.less=function(a,b,c){return d._compareValues(d.less,a,b,"<",c)};this.greaterOrEquals=function(a,b,c){return d._compareValues(d.greaterOrEquals,a,b,">=",c)};this.lessOrEquals=function(a,b,c){return d._compareValues(d.lessOrEquals,a,b,"<=",c)};this.startsWith=function(a,c){var e=null==c?a:c,e=g?b.trim(e.toString()).length:e.toString().length;G?d._append(d._getStr("jQuery.jgrid.getAccessor(this,'"+
a+"')")+".substr(0,"+e+") == "+d._getStr('"'+d._toStr(c)+'"')):(null!=c&&(e=g?b.trim(c.toString()).length:c.toString().length),d._append(d._getStr("this")+".substr(0,"+e+") == "+d._getStr('"'+d._toStr(a)+'"')));d._setCommand(d.startsWith,a);d._resetNegate();return d};this.endsWith=function(a,c){var e=null==c?a:c,e=g?b.trim(e.toString()).length:e.toString().length;G?d._append(d._getStr("jQuery.jgrid.getAccessor(this,'"+a+"')")+".substr("+d._getStr("jQuery.jgrid.getAccessor(this,'"+a+"')")+".length-"+
e+","+e+') == "'+d._toStr(c)+'"'):d._append(d._getStr("this")+".substr("+d._getStr("this")+'.length-"'+d._toStr(a)+'".length,"'+d._toStr(a)+'".length) == "'+d._toStr(a)+'"');d._setCommand(d.endsWith,a);d._resetNegate();return d};this.contains=function(a,b){G?d._append(d._getStr("jQuery.jgrid.getAccessor(this,'"+a+"')")+'.indexOf("'+d._toStr(b)+'",0) > -1'):d._append(d._getStr("this")+'.indexOf("'+d._toStr(a)+'",0) > -1');d._setCommand(d.contains,a);d._resetNegate();return d};this.groupBy=function(b,
c,e,l){return d._hasData()?d._getGroup(a,b,c,e,l):null};this.orderBy=function(a,c,e,l,g){c=null==c?"a":b.trim(c.toString().toLowerCase());null==e&&(e="text");null==l&&(l="Y-m-d");null==g&&(g=!1);if("desc"===c||"descending"===c)c="d";if("asc"===c||"ascending"===c)c="a";x.push({by:a,dir:c,type:e,datefmt:l,sfunc:g});return d};return d}(e,null)},getMethod:function(e){return this.getAccessor(b.fn.jqGrid,e)},extend:function(e){b.extend(b.fn.jqGrid,e);this.no_legacy_api||b.fn.extend(e)}});b.fn.jqGrid=function(e){if("string"===
typeof e){var f=b.jgrid.getMethod(e);if(!f)throw"jqGrid - No such method: "+e;var c=b.makeArray(arguments).slice(1);return f.apply(this,c)}return this.each(function(){if(!this.grid){var d=b.extend(!0,{url:"",height:150,page:1,rowNum:20,rowTotal:null,records:0,pager:"",pgbuttons:!0,pginput:!0,colModel:[],rowList:[],colNames:[],sortorder:"asc",sortname:"",datatype:"xml",mtype:"GET",altRows:!1,selarrrow:[],savedRow:[],shrinkToFit:!0,xmlReader:{},jsonReader:{},subGrid:!1,subGridModel:[],reccount:0,lastpage:0,
lastsort:0,selrow:null,beforeSelectRow:null,onSelectRow:null,onSortCol:null,ondblClickRow:null,onRightClickRow:null,onPaging:null,onSelectAll:null,onInitGrid:null,loadComplete:null,gridComplete:null,loadError:null,loadBeforeSend:null,afterInsertRow:null,beforeRequest:null,beforeProcessing:null,onHeaderClick:null,viewrecords:!1,loadonce:!1,multiselect:!1,multikey:!1,editurl:null,search:!1,caption:"",hidegrid:!0,hiddengrid:!1,postData:{},userData:{},treeGrid:!1,treeGridModel:"nested",treeReader:{},
treeANode:-1,ExpandColumn:null,tree_root_level:0,prmNames:{page:"page",rows:"rows",sort:"sidx",order:"sord",search:"_search",nd:"nd",id:"id",oper:"oper",editoper:"edit",addoper:"add",deloper:"del",subgridid:"id",npage:null,totalrows:"totalrows"},forceFit:!1,gridstate:"visible",cellEdit:!1,cellsubmit:"remote",nv:0,loadui:"enable",toolbar:[!1,""],scroll:!1,multiboxonly:!1,deselectAfterSort:!0,scrollrows:!1,autowidth:!1,scrollOffset:18,cellLayout:5,subGridWidth:20,multiselectWidth:20,gridview:!1,rownumWidth:25,
rownumbers:!1,pagerpos:"center",recordpos:"right",footerrow:!1,userDataOnFooter:!1,hoverrows:!0,altclass:"ui-priority-secondary",viewsortcols:[!1,"vertical",!0],resizeclass:"",autoencode:!1,remapColumns:[],ajaxGridOptions:{},direction:"ltr",toppager:!1,headertitles:!1,scrollTimeout:40,data:[],_index:{},grouping:!1,groupingView:{groupField:[],groupOrder:[],groupText:[],groupColumnShow:[],groupSummary:[],showSummaryOnHide:!1,sortitems:[],sortnames:[],summary:[],summaryval:[],plusicon:"ui-icon-circlesmall-plus",
minusicon:"ui-icon-circlesmall-minus",displayField:[],groupSummaryPos:[],formatDisplayField:[],_locgr:!1},ignoreCase:!1,cmTemplate:{},idPrefix:"",multiSort:!1},b.jgrid.defaults,e||{}),a=this,c={headers:[],cols:[],footers:[],dragStart:function(c,e,g){var f=b(this.bDiv).offset().left;this.resizing={idx:c,startX:e.clientX,sOL:e.clientX-f};this.hDiv.style.cursor="col-resize";this.curGbox=b("#rs_m"+b.jgrid.jqID(d.id),"#gbox_"+b.jgrid.jqID(d.id));this.curGbox.css({display:"block",left:e.clientX-f,top:g[1],
height:g[2]});b(a).triggerHandler("jqGridResizeStart",[e,c]);b.isFunction(d.resizeStart)&&d.resizeStart.call(a,e,c);document.onselectstart=function(){return!1}},dragMove:function(a){if(this.resizing){var b=a.clientX-this.resizing.startX;a=this.headers[this.resizing.idx];var c="ltr"===d.direction?a.width+b:a.width-b,e;33<c&&(this.curGbox.css({left:this.resizing.sOL+b}),!0===d.forceFit?(e=this.headers[this.resizing.idx+d.nv],b="ltr"===d.direction?e.width-b:e.width+b,33<b&&(a.newWidth=c,e.newWidth=b)):
(this.newWidth="ltr"===d.direction?d.tblwidth+b:d.tblwidth-b,a.newWidth=c))}},dragEnd:function(){this.hDiv.style.cursor="default";if(this.resizing){var c=this.resizing.idx,e=this.headers[c].newWidth||this.headers[c].width,e=parseInt(e,10);this.resizing=!1;b("#rs_m"+b.jgrid.jqID(d.id)).css("display","none");d.colModel[c].width=e;this.headers[c].width=e;this.headers[c].el.style.width=e+"px";this.cols[c].style.width=e+"px";0<this.footers.length&&(this.footers[c].style.width=e+"px");!0===d.forceFit?(e=
this.headers[c+d.nv].newWidth||this.headers[c+d.nv].width,this.headers[c+d.nv].width=e,this.headers[c+d.nv].el.style.width=e+"px",this.cols[c+d.nv].style.width=e+"px",0<this.footers.length&&(this.footers[c+d.nv].style.width=e+"px"),d.colModel[c+d.nv].width=e):(d.tblwidth=this.newWidth||d.tblwidth,b("table:first",this.bDiv).css("width",d.tblwidth+"px"),b("table:first",this.hDiv).css("width",d.tblwidth+"px"),this.hDiv.scrollLeft=this.bDiv.scrollLeft,d.footerrow&&(b("table:first",this.sDiv).css("width",
d.tblwidth+"px"),this.sDiv.scrollLeft=this.bDiv.scrollLeft));b(a).triggerHandler("jqGridResizeStop",[e,c]);b.isFunction(d.resizeStop)&&d.resizeStop.call(a,e,c)}this.curGbox=null;document.onselectstart=function(){return!0}},populateVisible:function(){c.timer&&clearTimeout(c.timer);c.timer=null;var a=b(c.bDiv).height();if(a){var e=b("table:first",c.bDiv),g,f;if(e[0].rows.length)try{f=(g=e[0].rows[1])?b(g).outerHeight()||c.prevRowHeight:c.prevRowHeight}catch(pa){f=c.prevRowHeight}if(f){c.prevRowHeight=
f;var h=d.rowNum;g=c.scrollTop=c.bDiv.scrollTop;var k=Math.round(e.position().top)-g,m=k+e.height();f*=h;var E,n,C;m<a&&0>=k&&(void 0===d.lastpage||parseInt((m+g+f-1)/f,10)<=d.lastpage)&&(n=parseInt((a-m+f-1)/f,10),0<=m||2>n||!0===d.scroll?(E=Math.round((m+g)/f)+1,k=-1):k=1);0<k&&(E=parseInt(g/f,10)+1,n=parseInt((g+a)/f,10)+2-E,C=!0);!n||d.lastpage&&(E>d.lastpage||1===d.lastpage||E===d.page&&E===d.lastpage)||(c.hDiv.loading?c.timer=setTimeout(c.populateVisible,d.scrollTimeout):(d.page=E,C&&(c.selectionPreserver(e[0]),
c.emptyRows.call(e[0],!1,!1)),c.populate(n)))}}},scrollGrid:function(a){if(d.scroll){var b=c.bDiv.scrollTop;void 0===c.scrollTop&&(c.scrollTop=0);b!==c.scrollTop&&(c.scrollTop=b,c.timer&&clearTimeout(c.timer),c.timer=setTimeout(c.populateVisible,d.scrollTimeout))}c.hDiv.scrollLeft=c.bDiv.scrollLeft;d.footerrow&&(c.sDiv.scrollLeft=c.bDiv.scrollLeft);a&&a.stopPropagation()},selectionPreserver:function(a){var c=a.p,d=c.selrow,e=c.selarrrow?b.makeArray(c.selarrrow):null,f=a.grid.bDiv.scrollLeft,g=function(){var h;
c.selrow=null;c.selarrrow=[];if(c.multiselect&&e&&0<e.length)for(h=0;h<e.length;h++)e[h]!==d&&b(a).jqGrid("setSelection",e[h],!1,null);d&&b(a).jqGrid("setSelection",d,!1,null);a.grid.bDiv.scrollLeft=f;b(a).unbind(".selectionPreserver",g)};b(a).bind("jqGridGridComplete.selectionPreserver",g)}};if("TABLE"!==this.tagName.toUpperCase())alert("Element is not a table");else if(void 0!==document.documentMode&&5>=document.documentMode)alert("Grid can not be used in this ('quirks') mode!");else{b(this).empty().attr("tabindex",
"0");this.p=d;this.p.useProp=!!b.fn.prop;var g,f;if(0===this.p.colNames.length)for(g=0;g<this.p.colModel.length;g++)this.p.colNames[g]=this.p.colModel[g].label||this.p.colModel[g].name;if(this.p.colNames.length!==this.p.colModel.length)alert(b.jgrid.errors.model);else{var k=b("<div class='ui-jqgrid-view'></div>"),n=b.jgrid.msie;a.p.direction=b.trim(a.p.direction.toLowerCase());-1===b.inArray(a.p.direction,["ltr","rtl"])&&(a.p.direction="ltr");f=a.p.direction;b(k).insertBefore(this);b(this).removeClass("scroll").appendTo(k);
var m=b("<div class='ui-jqgrid ui-widget ui-widget-content ui-corner-all'></div>");b(m).attr({id:"gbox_"+this.id,dir:f}).insertBefore(k);b(k).attr("id","gview_"+this.id).appendTo(m);b("<div class='ui-widget-overlay jqgrid-overlay' id='lui_"+this.id+"'></div>").insertBefore(k);b("<div class='loading ui-state-default ui-state-active' id='load_"+this.id+"'>"+this.p.loadtext+"</div>").insertBefore(k);b(this).attr({cellspacing:"0",cellpadding:"0",border:"0",role:"grid","aria-multiselectable":!!this.p.multiselect,
"aria-labelledby":"gbox_"+this.id});var r=function(a,b){a=parseInt(a,10);return isNaN(a)?b||0:a},p=function(d,e,f,g,pa,h){var k=a.p.colModel[d],m=k.align,E='style="',n=k.classes,C=k.name,A=[];m&&(E+="text-align:"+m+";");!0===k.hidden&&(E+="display:none;");if(0===e)E+="width: "+c.headers[d].width+"px;";else if(k.cellattr&&b.isFunction(k.cellattr)&&(d=k.cellattr.call(a,pa,f,g,k,h))&&"string"===typeof d)if(d=d.replace(/style/i,"style").replace(/title/i,"title"),-1<d.indexOf("title")&&(k.title=!1),-1<
d.indexOf("class")&&(n=void 0),A=d.replace("-style","-sti").split(/style/),2===A.length){A[1]=b.trim(A[1].replace("-sti","-style").replace("=",""));if(0===A[1].indexOf("'")||0===A[1].indexOf('"'))A[1]=A[1].substring(1);E+=A[1].replace(/'/gi,'"')}else E+='"';A.length||(A[0]="",E+='"');E+=(void 0!==n?' class="'+n+'"':"")+(k.title&&f?' title="'+b.jgrid.stripHtml(f)+'"':"");E+=' aria-describedby="'+a.p.id+"_"+C+'"';return E+A[0]},q=function(c){return null==c||""===c?"&#160;":a.p.autoencode?b.jgrid.htmlEncode(c):
String(c)},x=function(c,d,e,f,g){var h=a.p.colModel[e];void 0!==h.formatter?(c=""!==String(a.p.idPrefix)?b.jgrid.stripPref(a.p.idPrefix,c):c,c={rowId:c,colModel:h,gid:a.p.id,pos:e},d=b.isFunction(h.formatter)?h.formatter.call(a,d,c,f,g):b.fmatter?b.fn.fmatter.call(a,h.formatter,d,c,f,g):q(d)):d=q(d);return d},G=function(a,b,c,d,e,f){b=x(a,b,c,e,"add");return'<td role="gridcell" '+p(c,d,b,e,a,f)+">"+b+"</td>"},U=function(b,c,d,e){e='<input role="checkbox" type="checkbox" id="jqg_'+a.p.id+"_"+b+'" class="cbox" name="jqg_'+
a.p.id+"_"+b+'"'+(e?'checked="checked"':"")+"/>";return'<td role="gridcell" '+p(c,d,"",null,b,!0)+">"+e+"</td>"},M=function(a,b,c,d){c=(parseInt(c,10)-1)*parseInt(d,10)+1+b;return'<td role="gridcell" class="ui-state-default jqgrid-rownum" '+p(a,b,c,null,b,!0)+">"+c+"</td>"},ea=function(b){var c,d=[],e=0,f;for(f=0;f<a.p.colModel.length;f++)c=a.p.colModel[f],"cb"!==c.name&&"subgrid"!==c.name&&"rn"!==c.name&&(d[e]="local"===b?c.name:"xml"===b||"xmlstring"===b?c.xmlmap||c.name:c.jsonmap||c.name,!1!==
a.p.keyIndex&&!0===c.key&&(a.p.keyName=d[e]),e++);return d},W=function(c){var d=a.p.remapColumns;d&&d.length||(d=b.map(a.p.colModel,function(a,b){return b}));c&&(d=b.map(d,function(a){return a<c?null:a-c}));return d},X=function(a,c){var d;this.p.deepempty?b(this.rows).slice(1).remove():(d=0<this.rows.length?this.rows[0]:null,b(this.firstChild).empty().append(d));a&&this.p.scroll&&(b(this.grid.bDiv.firstChild).css({height:"auto"}),b(this.grid.bDiv.firstChild.firstChild).css({height:0,display:"none"}),
0!==this.grid.bDiv.scrollTop&&(this.grid.bDiv.scrollTop=0));!0===c&&this.p.treeGrid&&(this.p.data=[],this.p._index={})},O=function(){var c=a.p.data.length,d,e,f;d=!0===a.p.rownumbers?1:0;e=!0===a.p.multiselect?1:0;f=!0===a.p.subGrid?1:0;d=!1===a.p.keyIndex||!0===a.p.loadonce?a.p.localReader.id:a.p.colModel[a.p.keyIndex+e+f+d].name;for(e=0;e<c;e++)f=b.jgrid.getAccessor(a.p.data[e],d),void 0===f&&(f=String(e+1)),a.p._index[f]=e},$=function(c,d,e,f,g,h){var l="-1",k="",m;d=d?"display:none;":"";e="ui-widget-content jqgrow ui-row-"+
a.p.direction+(e?" "+e:"")+(h?" ui-state-highlight":"");h=b(a).triggerHandler("jqGridRowAttr",[f,g,c]);"object"!==typeof h&&(h=b.isFunction(a.p.rowattr)?a.p.rowattr.call(a,f,g,c):{});if(!b.isEmptyObject(h)){h.hasOwnProperty("id")&&(c=h.id,delete h.id);h.hasOwnProperty("tabindex")&&(l=h.tabindex,delete h.tabindex);h.hasOwnProperty("style")&&(d+=h.style,delete h.style);h.hasOwnProperty("class")&&(e+=" "+h["class"],delete h["class"]);try{delete h.role}catch(n){}for(m in h)h.hasOwnProperty(m)&&(k+=" "+
m+"="+h[m])}return'<tr role="row" id="'+c+'" tabindex="'+l+'" class="'+e+'"'+(""===d?"":' style="'+d+'"')+k+">"},K=function(c,d,e,f,g){var h=new Date,l="local"!==a.p.datatype&&a.p.loadonce||"xmlstring"===a.p.datatype,k=a.p.xmlReader,m="local"===a.p.datatype?"local":"xml";l&&(a.p.data=[],a.p._index={},a.p.localReader.id="_id_");a.p.reccount=0;if(b.isXMLDoc(c)){-1!==a.p.treeANode||a.p.scroll?e=1<e?e:1:(X.call(a,!1,!0),e=1);var n=b(a),C,A,R=0,p,u=!0===a.p.multiselect?1:0,z=0,x,q=!0===a.p.rownumbers?
1:0,t,Z=[],aa,v={},w,H,s=[],L=!0===a.p.altRows?a.p.altclass:"",ia;!0===a.p.subGrid&&(z=1,x=b.jgrid.getMethod("addSubGridCell"));k.repeatitems||(Z=ea(m));t=!1===a.p.keyIndex?b.isFunction(k.id)?k.id.call(a,c):k.id:a.p.keyIndex;0<Z.length&&!isNaN(t)&&(t=a.p.keyName);m=-1===String(t).indexOf("[")?Z.length?function(a,c){return b(t,a).text()||c}:function(a,c){return b(k.cell,a).eq(t).text()||c}:function(a,b){return a.getAttribute(t.replace(/[\[\]]/g,""))||b};a.p.userData={};a.p.page=r(b.jgrid.getXmlData(c,
k.page),a.p.page);a.p.lastpage=r(b.jgrid.getXmlData(c,k.total),1);a.p.records=r(b.jgrid.getXmlData(c,k.records));b.isFunction(k.userdata)?a.p.userData=k.userdata.call(a,c)||{}:b.jgrid.getXmlData(c,k.userdata,!0).each(function(){a.p.userData[this.getAttribute("name")]=b(this).text()});c=b.jgrid.getXmlData(c,k.root,!0);(c=b.jgrid.getXmlData(c,k.row,!0))||(c=[]);var S=c.length,I=0,y=[],D=parseInt(a.p.rowNum,10),B=a.p.scroll?b.jgrid.randId():1;0<S&&0>=a.p.page&&(a.p.page=1);if(c&&S){g&&(D*=g+1);g=b.isFunction(a.p.afterInsertRow);
var F=!1,J;a.p.grouping&&(F=!0===a.p.groupingView.groupCollapse,J=b.jgrid.getMethod("groupingPrepare"));for(;I<S;){w=c[I];H=m(w,B+I);H=a.p.idPrefix+H;C=0===e?0:e+1;ia=1===(C+I)%2?L:"";var K=s.length;s.push("");q&&s.push(M(0,I,a.p.page,a.p.rowNum));u&&s.push(U(H,q,I,!1));z&&s.push(x.call(n,u+q,I+e));if(k.repeatitems){aa||(aa=W(u+z+q));var N=b.jgrid.getXmlData(w,k.cell,!0);b.each(aa,function(b){var c=N[this];if(!c)return!1;p=c.textContent||c.text;v[a.p.colModel[b+u+z+q].name]=p;s.push(G(H,p,b+u+z+q,
I+e,w,v))})}else for(C=0;C<Z.length;C++)p=b.jgrid.getXmlData(w,Z[C]),v[a.p.colModel[C+u+z+q].name]=p,s.push(G(H,p,C+u+z+q,I+e,w,v));s[K]=$(H,F,ia,v,w,!1);s.push("</tr>");a.p.grouping&&(y.push(s),a.p.groupingView._locgr||J.call(n,v,I),s=[]);if(l||!0===a.p.treeGrid)v._id_=b.jgrid.stripPref(a.p.idPrefix,H),a.p.data.push(v),a.p._index[v._id_]=a.p.data.length-1;!1===a.p.gridview&&(b("tbody:first",d).append(s.join("")),n.triggerHandler("jqGridAfterInsertRow",[H,v,w]),g&&a.p.afterInsertRow.call(a,H,v,w),
s=[]);v={};R++;I++;if(R===D)break}}!0===a.p.gridview&&(A=-1<a.p.treeANode?a.p.treeANode:0,a.p.grouping?(l||n.jqGrid("groupingRender",y,a.p.colModel.length,a.p.page,D),y=null):!0===a.p.treeGrid&&0<A?b(a.rows[A]).after(s.join("")):b("tbody:first",d).append(s.join("")));if(!0===a.p.subGrid)try{n.jqGrid("addSubGrid",u+q)}catch(Q){}a.p.totaltime=new Date-h;0<R&&0===a.p.records&&(a.p.records=S);s=null;if(!0===a.p.treeGrid)try{n.jqGrid("setTreeNode",A+1,R+A+1)}catch(O){}a.p.treeGrid||a.p.scroll||(a.grid.bDiv.scrollTop=
0);a.p.reccount=R;a.p.treeANode=-1;a.p.userDataOnFooter&&n.jqGrid("footerData","set",a.p.userData,!0);l&&(a.p.records=S,a.p.lastpage=Math.ceil(S/D));f||a.updatepager(!1,!0);if(l){for(;R<S;){w=c[R];H=m(w,R+B);H=a.p.idPrefix+H;if(k.repeatitems){aa||(aa=W(u+z+q));var P=b.jgrid.getXmlData(w,k.cell,!0);b.each(aa,function(b){var c=P[this];if(!c)return!1;p=c.textContent||c.text;v[a.p.colModel[b+u+z+q].name]=p})}else for(C=0;C<Z.length;C++)p=b.jgrid.getXmlData(w,Z[C]),v[a.p.colModel[C+u+z+q].name]=p;v._id_=
b.jgrid.stripPref(a.p.idPrefix,H);a.p.grouping&&J.call(n,v,R);a.p.data.push(v);a.p._index[v._id_]=a.p.data.length-1;v={};R++}a.p.grouping&&(a.p.groupingView._locgr=!0,n.jqGrid("groupingRender",y,a.p.colModel.length,a.p.page,D),y=null)}}},Y=function(c,d,e,f,g){var h=new Date;if(c){-1!==a.p.treeANode||a.p.scroll?e=1<e?e:1:(X.call(a,!1,!0),e=1);var k,l="local"!==a.p.datatype&&a.p.loadonce||"jsonstring"===a.p.datatype;l&&(a.p.data=[],a.p._index={},a.p.localReader.id="_id_");a.p.reccount=0;"local"===a.p.datatype?
(d=a.p.localReader,k="local"):(d=a.p.jsonReader,k="json");var m=b(a),n=0,C,A,p,q=[],u=a.p.multiselect?1:0,z=!0===a.p.subGrid?1:0,x,t=!0===a.p.rownumbers?1:0,D=W(u+z+t);k=ea(k);var y,B,v,w={},H,s,L=[],ia=!0===a.p.altRows?a.p.altclass:"",S;a.p.page=r(b.jgrid.getAccessor(c,d.page),a.p.page);a.p.lastpage=r(b.jgrid.getAccessor(c,d.total),1);a.p.records=r(b.jgrid.getAccessor(c,d.records));a.p.userData=b.jgrid.getAccessor(c,d.userdata)||{};z&&(x=b.jgrid.getMethod("addSubGridCell"));v=!1===a.p.keyIndex?b.isFunction(d.id)?
d.id.call(a,c):d.id:a.p.keyIndex;d.repeatitems||(q=k,0<q.length&&!isNaN(v)&&(v=a.p.keyName));B=b.jgrid.getAccessor(c,d.root);null==B&&b.isArray(c)&&(B=c);B||(B=[]);c=B.length;A=0;0<c&&0>=a.p.page&&(a.p.page=1);var I=parseInt(a.p.rowNum,10),F=a.p.scroll?b.jgrid.randId():1,J=!1,K;g&&(I*=g+1);"local"!==a.p.datatype||a.p.deselectAfterSort||(J=!0);var N=b.isFunction(a.p.afterInsertRow),P=[],Q=!1,O;a.p.grouping&&(Q=!0===a.p.groupingView.groupCollapse,O=b.jgrid.getMethod("groupingPrepare"));for(;A<c;){g=
B[A];s=b.jgrid.getAccessor(g,v);void 0===s&&("number"===typeof v&&null!=a.p.colModel[v+u+z+t]&&(s=b.jgrid.getAccessor(g,a.p.colModel[v+u+z+t].name)),void 0===s&&(s=F+A,0===q.length&&d.cell&&(C=b.jgrid.getAccessor(g,d.cell)||g,s=null!=C&&void 0!==C[v]?C[v]:s)));s=a.p.idPrefix+s;C=1===e?0:e;S=1===(C+A)%2?ia:"";J&&(K=a.p.multiselect?-1!==b.inArray(s,a.p.selarrrow):s===a.p.selrow);var T=L.length;L.push("");t&&L.push(M(0,A,a.p.page,a.p.rowNum));u&&L.push(U(s,t,A,K));z&&L.push(x.call(m,u+t,A+e));y=k;d.repeatitems&&
(d.cell&&(g=b.jgrid.getAccessor(g,d.cell)||g),b.isArray(g)&&(y=D));for(p=0;p<y.length;p++)C=b.jgrid.getAccessor(g,y[p]),w[a.p.colModel[p+u+z+t].name]=C,L.push(G(s,C,p+u+z+t,A+e,g,w));L[T]=$(s,Q,S,w,g,K);L.push("</tr>");a.p.grouping&&(P.push(L),a.p.groupingView._locgr||O.call(m,w,A),L=[]);if(l||!0===a.p.treeGrid)w._id_=b.jgrid.stripPref(a.p.idPrefix,s),a.p.data.push(w),a.p._index[w._id_]=a.p.data.length-1;!1===a.p.gridview&&(b("#"+b.jgrid.jqID(a.p.id)+" tbody:first").append(L.join("")),m.triggerHandler("jqGridAfterInsertRow",
[s,w,g]),N&&a.p.afterInsertRow.call(a,s,w,g),L=[]);w={};n++;A++;if(n===I)break}!0===a.p.gridview&&(H=-1<a.p.treeANode?a.p.treeANode:0,a.p.grouping?l||(m.jqGrid("groupingRender",P,a.p.colModel.length,a.p.page,I),P=null):!0===a.p.treeGrid&&0<H?b(a.rows[H]).after(L.join("")):b("#"+b.jgrid.jqID(a.p.id)+" tbody:first").append(L.join("")));if(!0===a.p.subGrid)try{m.jqGrid("addSubGrid",u+t)}catch(V){}a.p.totaltime=new Date-h;0<n&&0===a.p.records&&(a.p.records=c);if(!0===a.p.treeGrid)try{m.jqGrid("setTreeNode",
H+1,n+H+1)}catch(Y){}a.p.treeGrid||a.p.scroll||(a.grid.bDiv.scrollTop=0);a.p.reccount=n;a.p.treeANode=-1;a.p.userDataOnFooter&&m.jqGrid("footerData","set",a.p.userData,!0);l&&(a.p.records=c,a.p.lastpage=Math.ceil(c/I));f||a.updatepager(!1,!0);if(l){for(;n<c&&B[n];){g=B[n];s=b.jgrid.getAccessor(g,v);void 0===s&&("number"===typeof v&&null!=a.p.colModel[v+u+z+t]&&(s=b.jgrid.getAccessor(g,a.p.colModel[v+u+z+t].name)),void 0===s&&(s=F+n,0===q.length&&d.cell&&(e=b.jgrid.getAccessor(g,d.cell)||g,s=null!=
e&&void 0!==e[v]?e[v]:s)));if(g){s=a.p.idPrefix+s;y=k;d.repeatitems&&(d.cell&&(g=b.jgrid.getAccessor(g,d.cell)||g),b.isArray(g)&&(y=D));for(p=0;p<y.length;p++)w[a.p.colModel[p+u+z+t].name]=b.jgrid.getAccessor(g,y[p]);w._id_=b.jgrid.stripPref(a.p.idPrefix,s);a.p.grouping&&O.call(m,w,n);a.p.data.push(w);a.p._index[w._id_]=a.p.data.length-1;w={}}n++}a.p.grouping&&(a.p.groupingView._locgr=!0,m.jqGrid("groupingRender",P,a.p.colModel.length,a.p.page,I))}}},oa=function(){function c(a){var b=0,d,e,g,h,k;
if(null!=a.groups){(e=a.groups.length&&"OR"===a.groupOp.toString().toUpperCase())&&u.orBegin();for(d=0;d<a.groups.length;d++){0<b&&e&&u.or();try{c(a.groups[d])}catch(l){alert(l)}b++}e&&u.orEnd()}if(null!=a.rules)try{(g=a.rules.length&&"OR"===a.groupOp.toString().toUpperCase())&&u.orBegin();for(d=0;d<a.rules.length;d++)k=a.rules[d],h=a.groupOp.toString().toUpperCase(),q[k.op]&&k.field&&(0<b&&h&&"OR"===h&&(u=u.or()),u=q[k.op](u,h)(k.field,k.data,f[k.field])),b++;g&&u.orEnd()}catch(m){alert(m)}}var d=
a.p.multiSort?[]:"",e=[],g=!1,f={},h=[],k=[],l,m,n;if(b.isArray(a.p.data)){var p=a.p.grouping?a.p.groupingView:!1,A,r;b.each(a.p.colModel,function(){m=this.sorttype||"text";"date"===m||"datetime"===m?(this.formatter&&"string"===typeof this.formatter&&"date"===this.formatter?(l=this.formatoptions&&this.formatoptions.srcformat?this.formatoptions.srcformat:b.jgrid.formatter.date.srcformat,n=this.formatoptions&&this.formatoptions.newformat?this.formatoptions.newformat:b.jgrid.formatter.date.newformat):
l=n=this.datefmt||"Y-m-d",f[this.name]={stype:m,srcfmt:l,newfmt:n,sfunc:this.sortfunc||null}):f[this.name]={stype:m,srcfmt:"",newfmt:"",sfunc:this.sortfunc||null};if(a.p.grouping)for(r=0,A=p.groupField.length;r<A;r++)if(this.name===p.groupField[r]){var c=this.name;this.index&&(c=this.index);h[r]=f[c];k[r]=c}a.p.multiSort?this.lso&&(d.push(this.name),c=this.lso.split("-"),e.push(c[c.length-1])):g||this.index!==a.p.sortname&&this.name!==a.p.sortname||(d=this.name,g=!0)});if(a.p.treeGrid)b(a).jqGrid("SortTree",
d,a.p.sortorder,f[d].stype||"text",f[d].srcfmt||"");else{var q={eq:function(a){return a.equals},ne:function(a){return a.notEquals},lt:function(a){return a.less},le:function(a){return a.lessOrEquals},gt:function(a){return a.greater},ge:function(a){return a.greaterOrEquals},cn:function(a){return a.contains},nc:function(a,b){return"OR"===b?a.orNot().contains:a.andNot().contains},bw:function(a){return a.startsWith},bn:function(a,b){return"OR"===b?a.orNot().startsWith:a.andNot().startsWith},en:function(a,
b){return"OR"===b?a.orNot().endsWith:a.andNot().endsWith},ew:function(a){return a.endsWith},ni:function(a,b){return"OR"===b?a.orNot().equals:a.andNot().equals},"in":function(a){return a.equals},nu:function(a){return a.isNull},nn:function(a,b){return"OR"===b?a.orNot().isNull:a.andNot().isNull}},u=b.jgrid.from(a.p.data);a.p.ignoreCase&&(u=u.ignoreCase());if(!0===a.p.search){var z=a.p.postData.filters;if(z)"string"===typeof z&&(z=b.jgrid.parse(z)),c(z);else try{u=q[a.p.postData.searchOper](u)(a.p.postData.searchField,
a.p.postData.searchString,f[a.p.postData.searchField])}catch(t){}}if(a.p.grouping)for(r=0;r<A;r++)u.orderBy(k[r],p.groupOrder[r],h[r].stype,h[r].srcfmt);a.p.multiSort?b.each(d,function(a){u.orderBy(this,e[a],f[this].stype,f[this].srcfmt,f[this].sfunc)}):d&&a.p.sortorder&&g&&("DESC"===a.p.sortorder.toUpperCase()?u.orderBy(a.p.sortname,"d",f[d].stype,f[d].srcfmt,f[d].sfunc):u.orderBy(a.p.sortname,"a",f[d].stype,f[d].srcfmt,f[d].sfunc));var z=u.select(),x=parseInt(a.p.rowNum,10),y=z.length,B=parseInt(a.p.page,
10),D=Math.ceil(y/x),v={};if((a.p.search||a.p.resetsearch)&&a.p.grouping&&a.p.groupingView._locgr){a.p.groupingView.groups=[];var w,G=b.jgrid.getMethod("groupingPrepare"),s,F;if(a.p.footerrow&&a.p.userDataOnFooter){for(s in a.p.userData)a.p.userData.hasOwnProperty(s)&&(a.p.userData[s]=0);F=!0}for(w=0;w<y;w++){if(F)for(s in a.p.userData)a.p.userData[s]+=parseFloat(z[w][s]||0);G.call(b(a),z[w],w,x)}}z=z.slice((B-1)*x,B*x);f=u=null;v[a.p.localReader.total]=D;v[a.p.localReader.page]=B;v[a.p.localReader.records]=
y;v[a.p.localReader.root]=z;v[a.p.localReader.userdata]=a.p.userData;z=null;return v}}},P=function(){a.grid.hDiv.loading=!0;if(!a.p.hiddengrid)switch(a.p.loadui){case "enable":b("#load_"+b.jgrid.jqID(a.p.id)).show();break;case "block":b("#lui_"+b.jgrid.jqID(a.p.id)).show(),b("#load_"+b.jgrid.jqID(a.p.id)).show()}},T=function(){a.grid.hDiv.loading=!1;switch(a.p.loadui){case "enable":b("#load_"+b.jgrid.jqID(a.p.id)).hide();break;case "block":b("#lui_"+b.jgrid.jqID(a.p.id)).hide(),b("#load_"+b.jgrid.jqID(a.p.id)).hide()}},
Q=function(c){if(!a.grid.hDiv.loading){var d=a.p.scroll&&!1===c,e={},g,f=a.p.prmNames;0>=a.p.page&&(a.p.page=Math.min(1,a.p.lastpage));null!==f.search&&(e[f.search]=a.p.search);null!==f.nd&&(e[f.nd]=(new Date).getTime());null!==f.rows&&(e[f.rows]=a.p.rowNum);null!==f.page&&(e[f.page]=a.p.page);null!==f.sort&&(e[f.sort]=a.p.sortname);null!==f.order&&(e[f.order]=a.p.sortorder);null!==a.p.rowTotal&&null!==f.totalrows&&(e[f.totalrows]=a.p.rowTotal);var h=b.isFunction(a.p.loadComplete),k=h?a.p.loadComplete:
null,l=0;c=c||1;1<c?null!==f.npage?(e[f.npage]=c,l=c-1,c=1):k=function(b){a.p.page++;a.grid.hDiv.loading=!1;h&&a.p.loadComplete.call(a,b);Q(c-1)}:null!==f.npage&&delete a.p.postData[f.npage];if(a.p.grouping){b(a).jqGrid("groupingSetup");var m=a.p.groupingView,n,p="";for(n=0;n<m.groupField.length;n++){var r=m.groupField[n];b.each(a.p.colModel,function(a,b){b.name===r&&b.index&&(r=b.index)});p+=r+" "+m.groupOrder[n]+", "}e[f.sort]=p+e[f.sort]}b.extend(a.p.postData,e);var q=a.p.scroll?a.rows.length-
1:1,e=b(a).triggerHandler("jqGridBeforeRequest");if(!1!==e&&"stop"!==e)if(b.isFunction(a.p.datatype))a.p.datatype.call(a,a.p.postData,"load_"+a.p.id,q,c,l);else{if(b.isFunction(a.p.beforeRequest)&&(e=a.p.beforeRequest.call(a),void 0===e&&(e=!0),!1===e))return;g=a.p.datatype.toLowerCase();switch(g){case "json":case "jsonp":case "xml":case "script":b.ajax(b.extend({url:a.p.url,type:a.p.mtype,dataType:g,data:b.isFunction(a.p.serializeGridData)?a.p.serializeGridData.call(a,a.p.postData):a.p.postData,
success:function(e,f,h){if(b.isFunction(a.p.beforeProcessing)&&!1===a.p.beforeProcessing.call(a,e,f,h))T();else{"xml"===g?K(e,a.grid.bDiv,q,1<c,l):Y(e,a.grid.bDiv,q,1<c,l);b(a).triggerHandler("jqGridLoadComplete",[e]);k&&k.call(a,e);b(a).triggerHandler("jqGridAfterLoadComplete",[e]);d&&a.grid.populateVisible();if(a.p.loadonce||a.p.treeGrid)a.p.datatype="local";1===c&&T()}},error:function(d,e,f){b.isFunction(a.p.loadError)&&a.p.loadError.call(a,d,e,f);1===c&&T()},beforeSend:function(c,d){var e=!0;
b.isFunction(a.p.loadBeforeSend)&&(e=a.p.loadBeforeSend.call(a,c,d));void 0===e&&(e=!0);if(!1===e)return!1;P()}},b.jgrid.ajaxOptions,a.p.ajaxGridOptions));break;case "xmlstring":P();e="string"!==typeof a.p.datastr?a.p.datastr:b.parseXML(a.p.datastr);K(e,a.grid.bDiv);b(a).triggerHandler("jqGridLoadComplete",[e]);h&&a.p.loadComplete.call(a,e);b(a).triggerHandler("jqGridAfterLoadComplete",[e]);a.p.datatype="local";a.p.datastr=null;T();break;case "jsonstring":P();e="string"===typeof a.p.datastr?b.jgrid.parse(a.p.datastr):
a.p.datastr;Y(e,a.grid.bDiv);b(a).triggerHandler("jqGridLoadComplete",[e]);h&&a.p.loadComplete.call(a,e);b(a).triggerHandler("jqGridAfterLoadComplete",[e]);a.p.datatype="local";a.p.datastr=null;T();break;case "local":case "clientside":P(),a.p.datatype="local",e=oa(),Y(e,a.grid.bDiv,q,1<c,l),b(a).triggerHandler("jqGridLoadComplete",[e]),k&&k.call(a,e),b(a).triggerHandler("jqGridAfterLoadComplete",[e]),d&&a.grid.populateVisible(),T()}}}},ha=function(c){b("#cb_"+b.jgrid.jqID(a.p.id),a.grid.hDiv)[a.p.useProp?
"prop":"attr"]("checked",c);if(a.p.frozenColumns&&a.p.id+"_frozen")b("#cb_"+b.jgrid.jqID(a.p.id),a.grid.fhDiv)[a.p.useProp?"prop":"attr"]("checked",c)},qa=function(c,e){var d="",g="<table cellspacing='0' cellpadding='0' border='0' style='table-layout:auto;' class='ui-pg-table'><tbody><tr>",k="",l,m,n,p,q=function(c){var e;b.isFunction(a.p.onPaging)&&(e=a.p.onPaging.call(a,c));if("stop"===e)return!1;a.p.selrow=null;a.p.multiselect&&(a.p.selarrrow=[],ha(!1));a.p.savedRow=[];return!0};c=c.substr(1);
e+="_"+c;l="pg_"+c;m=c+"_left";n=c+"_center";p=c+"_right";b("#"+b.jgrid.jqID(c)).append("<div id='"+l+"' class='ui-pager-control' role='group'><table cellspacing='0' cellpadding='0' border='0' class='ui-pg-table' style='width:100%;table-layout:fixed;height:100%;' role='row'><tbody><tr><td id='"+m+"' align='left'></td><td id='"+n+"' align='center' style='white-space:pre;'></td><td id='"+p+"' align='right'></td></tr></tbody></table></div>").attr("dir","ltr");if(0<a.p.rowList.length){k="<td dir='"+f+
"'>";k+="<select class='ui-pg-selbox' role='listbox'>";for(m=0;m<a.p.rowList.length;m++)k+='<option role="option" value="'+a.p.rowList[m]+'"'+(a.p.rowNum===a.p.rowList[m]?' selected="selected"':"")+">"+a.p.rowList[m]+"</option>";k+="</select></td>"}"rtl"===f&&(g+=k);!0===a.p.pginput&&(d="<td dir='"+f+"'>"+b.jgrid.format(a.p.pgtext||"","<input class='ui-pg-input' type='text' size='2' maxlength='7' value='0' role='textbox'/>","<span id='sp_1_"+b.jgrid.jqID(c)+"'></span>")+"</td>");!0===a.p.pgbuttons?
(m=["first"+e,"prev"+e,"next"+e,"last"+e],"rtl"===f&&m.reverse(),g+="<td id='"+m[0]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-first'></span></td>",g+="<td id='"+m[1]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-prev'></span></td>",g=g+(""!==d?"<td class='ui-pg-button ui-state-disabled' style='width:4px;'><span class='ui-separator'></span></td>"+d+"<td class='ui-pg-button ui-state-disabled' style='width:4px;'><span class='ui-separator'></span></td>":
"")+("<td id='"+m[2]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-next'></span></td>"),g+="<td id='"+m[3]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-end'></span></td>"):""!==d&&(g+=d);"ltr"===f&&(g+=k);g+="</tr></tbody></table>";!0===a.p.viewrecords&&b("td#"+c+"_"+a.p.recordpos,"#"+l).append("<div dir='"+f+"' style='text-align:"+a.p.recordpos+"' class='ui-paging-info'></div>");b("td#"+c+"_"+a.p.pagerpos,"#"+l).append(g);k=b(".ui-jqgrid").css("font-size")||
"11px";b(document.body).append("<div id='testpg' class='ui-jqgrid ui-widget ui-widget-content' style='font-size:"+k+";visibility:hidden;' ></div>");g=b(g).clone().appendTo("#testpg").width();b("#testpg").remove();0<g&&(""!==d&&(g+=50),b("td#"+c+"_"+a.p.pagerpos,"#"+l).width(g));a.p._nvtd=[];a.p._nvtd[0]=g?Math.floor((a.p.width-g)/2):Math.floor(a.p.width/3);a.p._nvtd[1]=0;g=null;b(".ui-pg-selbox","#"+l).bind("change",function(){if(!q("records"))return!1;a.p.page=Math.round(a.p.rowNum*(a.p.page-1)/
this.value-0.5)+1;a.p.rowNum=this.value;a.p.pager&&b(".ui-pg-selbox",a.p.pager).val(this.value);a.p.toppager&&b(".ui-pg-selbox",a.p.toppager).val(this.value);Q();return!1});!0===a.p.pgbuttons&&(b(".ui-pg-button","#"+l).hover(function(){b(this).hasClass("ui-state-disabled")?this.style.cursor="default":(b(this).addClass("ui-state-hover"),this.style.cursor="pointer")},function(){b(this).hasClass("ui-state-disabled")||(b(this).removeClass("ui-state-hover"),this.style.cursor="default")}),b("#first"+b.jgrid.jqID(e)+
", #prev"+b.jgrid.jqID(e)+", #next"+b.jgrid.jqID(e)+", #last"+b.jgrid.jqID(e)).click(function(){if(b(this).hasClass("ui-state-disabled"))return!1;var c=r(a.p.page,1),d=r(a.p.lastpage,1),g=!1,f=!0,h=!0,k=!0,l=!0;0===d||1===d?l=k=h=f=!1:1<d&&1<=c?1===c?h=f=!1:c===d&&(l=k=!1):1<d&&0===c&&(l=k=!1,c=d-1);if(!q(this.id))return!1;this.id==="first"+e&&f&&(a.p.page=1,g=!0);this.id==="prev"+e&&h&&(a.p.page=c-1,g=!0);this.id==="next"+e&&k&&(a.p.page=c+1,g=!0);this.id==="last"+e&&l&&(a.p.page=d,g=!0);g&&Q();
return!1}));!0===a.p.pginput&&b("input.ui-pg-input","#"+l).keypress(function(c){if(13===(c.charCode||c.keyCode||0)){if(!q("user"))return!1;b(this).val(r(b(this).val(),1));a.p.page=0<b(this).val()?b(this).val():a.p.page;Q();return!1}return this})},wa=function(c,e){var d,g="",f=a.p.colModel,h=!1,k;k=a.p.frozenColumns?e:a.grid.headers[c].el;var l="";b("span.ui-grid-ico-sort",k).addClass("ui-state-disabled");b(k).attr("aria-selected","false");if(f[c].lso)if("asc"===f[c].lso)f[c].lso+="-desc",l="desc";
else if("desc"===f[c].lso)f[c].lso+="-asc",l="asc";else{if("asc-desc"===f[c].lso||"desc-asc"===f[c].lso)f[c].lso=""}else f[c].lso=l=f[c].firstsortorder||"asc";l?(b("span.s-ico",k).show(),b("span.ui-icon-"+l,k).removeClass("ui-state-disabled"),b(k).attr("aria-selected","true")):a.p.viewsortcols[0]||b("span.s-ico",k).hide();a.p.sortorder="";b.each(f,function(b){this.lso&&(0<b&&h&&(g+=", "),d=this.lso.split("-"),g+=f[b].index||f[b].name,g+=" "+d[d.length-1],h=!0,a.p.sortorder=d[d.length-1])});k=g.lastIndexOf(a.p.sortorder);
g=g.substring(0,k);a.p.sortname=g},ra=function(c,d,e,g,f){if(a.p.colModel[d].sortable&&!(0<a.p.savedRow.length)){e||(a.p.lastsort===d?"asc"===a.p.sortorder?a.p.sortorder="desc":"desc"===a.p.sortorder&&(a.p.sortorder="asc"):a.p.sortorder=a.p.colModel[d].firstsortorder||"asc",a.p.page=1);if(a.p.multiSort)wa(d,f);else{if(g){if(a.p.lastsort===d&&a.p.sortorder===g&&!e)return;a.p.sortorder=g}e=a.grid.headers[a.p.lastsort].el;f=a.p.frozenColumns?f:a.grid.headers[d].el;b("span.ui-grid-ico-sort",e).addClass("ui-state-disabled");
b(e).attr("aria-selected","false");a.p.frozenColumns&&(a.grid.fhDiv.find("span.ui-grid-ico-sort").addClass("ui-state-disabled"),a.grid.fhDiv.find("th").attr("aria-selected","false"));b("span.ui-icon-"+a.p.sortorder,f).removeClass("ui-state-disabled");b(f).attr("aria-selected","true");a.p.viewsortcols[0]||a.p.lastsort===d||(a.p.frozenColumns&&a.grid.fhDiv.find("span.s-ico").hide(),b("span.s-ico",e).hide(),b("span.s-ico",f).show());c=c.substring(5+a.p.id.length+1);a.p.sortname=a.p.colModel[d].index||
c}"stop"===b(a).triggerHandler("jqGridSortCol",[a.p.sortname,d,a.p.sortorder])?a.p.lastsort=d:b.isFunction(a.p.onSortCol)&&"stop"===a.p.onSortCol.call(a,a.p.sortname,d,a.p.sortorder)?a.p.lastsort=d:("local"===a.p.datatype?a.p.deselectAfterSort&&b(a).jqGrid("resetSelection"):(a.p.selrow=null,a.p.multiselect&&ha(!1),a.p.selarrrow=[],a.p.savedRow=[]),a.p.scroll&&(f=a.grid.bDiv.scrollLeft,X.call(a,!0,!1),a.grid.hDiv.scrollLeft=f),a.p.subGrid&&"local"===a.p.datatype&&b("td.sgexpanded","#"+b.jgrid.jqID(a.p.id)).each(function(){b(this).trigger("click")}),
Q(),a.p.lastsort=d,a.p.sortname!==c&&d&&(a.p.lastsort=d))}},xa=function(c){c=b(a.grid.headers[c].el);c=[c.position().left+c.outerWidth()];"rtl"===a.p.direction&&(c[0]=a.p.width-c[0]);c[0]-=a.grid.bDiv.scrollLeft;c.push(b(a.grid.hDiv).position().top);c.push(b(a.grid.bDiv).offset().top-b(a.grid.hDiv).offset().top+b(a.grid.bDiv).height());return c},sa=function(c){var d,e=a.grid.headers,g=b.jgrid.getCellIndex(c);for(d=0;d<e.length;d++)if(c===e[d].el){g=d;break}return g};this.p.id=this.id;-1===b.inArray(a.p.multikey,
["shiftKey","altKey","ctrlKey"])&&(a.p.multikey=!1);a.p.keyIndex=!1;a.p.keyName=!1;for(g=0;g<a.p.colModel.length;g++)a.p.colModel[g]=b.extend(!0,{},a.p.cmTemplate,a.p.colModel[g].template||{},a.p.colModel[g]),!1===a.p.keyIndex&&!0===a.p.colModel[g].key&&(a.p.keyIndex=g);a.p.sortorder=a.p.sortorder.toLowerCase();b.jgrid.cell_width=b.jgrid.cellWidth();!0===a.p.grouping&&(a.p.scroll=!1,a.p.rownumbers=!1,a.p.treeGrid=!1,a.p.gridview=!0);if(!0===this.p.treeGrid){try{b(this).jqGrid("setTreeGrid")}catch(za){}"local"!==
a.p.datatype&&(a.p.localReader={id:"_id_"})}if(this.p.subGrid)try{b(a).jqGrid("setSubGrid")}catch(Aa){}this.p.multiselect&&(this.p.colNames.unshift("<input role='checkbox' id='cb_"+this.p.id+"' class='cbox' type='checkbox'/>"),this.p.colModel.unshift({name:"cb",width:b.jgrid.cell_width?a.p.multiselectWidth+a.p.cellLayout:a.p.multiselectWidth,sortable:!1,resizable:!1,hidedlg:!0,search:!1,align:"center",fixed:!0}));this.p.rownumbers&&(this.p.colNames.unshift(""),this.p.colModel.unshift({name:"rn",width:a.p.rownumWidth,
sortable:!1,resizable:!1,hidedlg:!0,search:!1,align:"center",fixed:!0}));a.p.xmlReader=b.extend(!0,{root:"rows",row:"row",page:"rows>page",total:"rows>total",records:"rows>records",repeatitems:!0,cell:"cell",id:"[id]",userdata:"userdata",subgrid:{root:"rows",row:"row",repeatitems:!0,cell:"cell"}},a.p.xmlReader);a.p.jsonReader=b.extend(!0,{root:"rows",page:"page",total:"total",records:"records",repeatitems:!0,cell:"cell",id:"id",userdata:"userdata",subgrid:{root:"rows",repeatitems:!0,cell:"cell"}},
a.p.jsonReader);a.p.localReader=b.extend(!0,{root:"rows",page:"page",total:"total",records:"records",repeatitems:!1,cell:"cell",id:"id",userdata:"userdata",subgrid:{root:"rows",repeatitems:!0,cell:"cell"}},a.p.localReader);a.p.scroll&&(a.p.pgbuttons=!1,a.p.pginput=!1,a.p.rowList=[]);a.p.data.length&&O();var D="<thead><tr class='ui-jqgrid-labels' role='rowheader'>",ta,F,ja,fa,ka,y,t,ba,ua=ba="",ga=[],va=[];F=[];if(!0===a.p.shrinkToFit&&!0===a.p.forceFit)for(g=a.p.colModel.length-1;0<=g;g--)if(!a.p.colModel[g].hidden){a.p.colModel[g].resizable=
!1;break}"horizontal"===a.p.viewsortcols[1]&&(ba=" ui-i-asc",ua=" ui-i-desc");ta=n?"class='ui-th-div-ie'":"";ba="<span class='s-ico' style='display:none'><span sort='asc' class='ui-grid-ico-sort ui-icon-asc"+ba+" ui-state-disabled ui-icon ui-icon-triangle-1-n ui-sort-"+f+"'></span>"+("<span sort='desc' class='ui-grid-ico-sort ui-icon-desc"+ua+" ui-state-disabled ui-icon ui-icon-triangle-1-s ui-sort-"+f+"'></span></span>");if(a.p.multiSort)for(ga=a.p.sortname.split(","),g=0;g<ga.length;g++)F=b.trim(ga[g]).split(" "),
ga[g]=b.trim(F[0]),va[g]=F[1]?b.trim(F[1]):a.p.sortorder||"asc";for(g=0;g<this.p.colNames.length;g++)F=a.p.headertitles?' title="'+b.jgrid.stripHtml(a.p.colNames[g])+'"':"",D+="<th id='"+a.p.id+"_"+a.p.colModel[g].name+"' role='columnheader' class='ui-state-default ui-th-column ui-th-"+f+"'"+F+">",F=a.p.colModel[g].index||a.p.colModel[g].name,D+="<div id='jqgh_"+a.p.id+"_"+a.p.colModel[g].name+"' "+ta+">"+a.p.colNames[g],a.p.colModel[g].width=a.p.colModel[g].width?parseInt(a.p.colModel[g].width,10):
150,"boolean"!==typeof a.p.colModel[g].title&&(a.p.colModel[g].title=!0),a.p.colModel[g].lso="",F===a.p.sortname&&(a.p.lastsort=g),a.p.multiSort&&(F=b.inArray(F,ga),-1!==F&&(a.p.colModel[g].lso=va[F])),D+=ba+"</div></th>";D+="</tr></thead>";ba=null;b(this).append(D);b("thead tr:first th",this).hover(function(){b(this).addClass("ui-state-hover")},function(){b(this).removeClass("ui-state-hover")});if(this.p.multiselect){var la=[],ca;b("#cb_"+b.jgrid.jqID(a.p.id),this).bind("click",function(){a.p.selarrrow=
[];var c=!0===a.p.frozenColumns?a.p.id+"_frozen":"";this.checked?(b(a.rows).each(function(d){0<d&&!b(this).hasClass("ui-subgrid")&&!b(this).hasClass("jqgroup")&&!b(this).hasClass("ui-state-disabled")&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id))[a.p.useProp?"prop":"attr"]("checked",!0),b(this).addClass("ui-state-highlight").attr("aria-selected","true"),a.p.selarrrow.push(this.id),a.p.selrow=this.id,c&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id),a.grid.fbDiv)[a.p.useProp?
"prop":"attr"]("checked",!0),b("#"+b.jgrid.jqID(this.id),a.grid.fbDiv).addClass("ui-state-highlight")))}),ca=!0,la=[]):(b(a.rows).each(function(d){0<d&&!b(this).hasClass("ui-subgrid")&&!b(this).hasClass("ui-state-disabled")&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id))[a.p.useProp?"prop":"attr"]("checked",!1),b(this).removeClass("ui-state-highlight").attr("aria-selected","false"),la.push(this.id),c&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id),a.grid.fbDiv)[a.p.useProp?
"prop":"attr"]("checked",!1),b("#"+b.jgrid.jqID(this.id),a.grid.fbDiv).removeClass("ui-state-highlight")))}),a.p.selrow=null,ca=!1);b(a).triggerHandler("jqGridSelectAll",[ca?a.p.selarrrow:la,ca]);b.isFunction(a.p.onSelectAll)&&a.p.onSelectAll.call(a,ca?a.p.selarrrow:la,ca)})}!0===a.p.autowidth&&(D=b(m).innerWidth(),a.p.width=0<D?D:"nw");(function(){var d=0,e=b.jgrid.cell_width?0:r(a.p.cellLayout,0),g=0,f,h=r(a.p.scrollOffset,0),k,m=!1,n,p=0,q;b.each(a.p.colModel,function(){void 0===this.hidden&&(this.hidden=
!1);if(a.p.grouping&&a.p.autowidth){var c=b.inArray(this.name,a.p.groupingView.groupField);0<=c&&a.p.groupingView.groupColumnShow.length>c&&(this.hidden=!a.p.groupingView.groupColumnShow[c])}this.widthOrg=k=r(this.width,0);!1===this.hidden&&(d+=k+e,this.fixed?p+=k+e:g++)});isNaN(a.p.width)&&(a.p.width=d+(!1!==a.p.shrinkToFit||isNaN(a.p.height)?0:h));c.width=a.p.width;a.p.tblwidth=d;!1===a.p.shrinkToFit&&!0===a.p.forceFit&&(a.p.forceFit=!1);!0===a.p.shrinkToFit&&0<g&&(n=c.width-e*g-p,isNaN(a.p.height)||
(n-=h,m=!0),d=0,b.each(a.p.colModel,function(b){!1!==this.hidden||this.fixed||(this.width=k=Math.round(n*this.width/(a.p.tblwidth-e*g-p)),d+=k,f=b)}),q=0,m?c.width-p-(d+e*g)!==h&&(q=c.width-p-(d+e*g)-h):m||1===Math.abs(c.width-p-(d+e*g))||(q=c.width-p-(d+e*g)),a.p.colModel[f].width+=q,a.p.tblwidth=d+q+e*g+p,a.p.tblwidth>a.p.width&&(a.p.colModel[f].width-=a.p.tblwidth-parseInt(a.p.width,10),a.p.tblwidth=a.p.width))})();b(m).css("width",c.width+"px").append("<div class='ui-jqgrid-resize-mark' id='rs_m"+
a.p.id+"'>&#160;</div>");b(k).css("width",c.width+"px");var D=b("thead:first",a).get(0),V="";a.p.footerrow&&(V+="<table role='grid' style='width:"+a.p.tblwidth+"px' class='ui-jqgrid-ftable' cellspacing='0' cellpadding='0' border='0'><tbody><tr role='row' class='ui-widget-content footrow footrow-"+f+"'>");var k=b("tr:first",D),da="<tr class='jqgfirstrow' role='row' style='height:auto'>";a.p.disableClick=!1;b("th",k).each(function(d){ja=a.p.colModel[d].width;void 0===a.p.colModel[d].resizable&&(a.p.colModel[d].resizable=
!0);a.p.colModel[d].resizable?(fa=document.createElement("span"),b(fa).html("&#160;").addClass("ui-jqgrid-resize ui-jqgrid-resize-"+f).css("cursor","col-resize"),b(this).addClass(a.p.resizeclass)):fa="";b(this).css("width",ja+"px").prepend(fa);fa=null;var e="";a.p.colModel[d].hidden&&(b(this).css("display","none"),e="display:none;");da+="<td role='gridcell' style='height:0px;width:"+ja+"px;"+e+"'></td>";c.headers[d]={width:ja,el:this};ka=a.p.colModel[d].sortable;"boolean"!==typeof ka&&(ka=a.p.colModel[d].sortable=
!0);e=a.p.colModel[d].name;"cb"!==e&&"subgrid"!==e&&"rn"!==e&&a.p.viewsortcols[2]&&b(">div",this).addClass("ui-jqgrid-sortable");ka&&(a.p.multiSort?a.p.viewsortcols[0]?(b("div span.s-ico",this).show(),a.p.colModel[d].lso&&b("div span.ui-icon-"+a.p.colModel[d].lso,this).removeClass("ui-state-disabled")):a.p.colModel[d].lso&&(b("div span.s-ico",this).show(),b("div span.ui-icon-"+a.p.colModel[d].lso,this).removeClass("ui-state-disabled")):a.p.viewsortcols[0]?(b("div span.s-ico",this).show(),d===a.p.lastsort&&
b("div span.ui-icon-"+a.p.sortorder,this).removeClass("ui-state-disabled")):d===a.p.lastsort&&(b("div span.s-ico",this).show(),b("div span.ui-icon-"+a.p.sortorder,this).removeClass("ui-state-disabled")));a.p.footerrow&&(V+="<td role='gridcell' "+p(d,0,"",null,"",!1)+">&#160;</td>")}).mousedown(function(d){if(1===b(d.target).closest("th>span.ui-jqgrid-resize").length){var e=sa(this);if(!0===a.p.forceFit){var g=a.p,f=e,h;for(h=e+1;h<a.p.colModel.length;h++)if(!0!==a.p.colModel[h].hidden){f=h;break}g.nv=
f-e}c.dragStart(e,d,xa(e));return!1}}).click(function(c){if(a.p.disableClick)return a.p.disableClick=!1;var d="th>div.ui-jqgrid-sortable",e,g;a.p.viewsortcols[2]||(d="th>div>span>span.ui-grid-ico-sort");c=b(c.target).closest(d);if(1===c.length){var f;if(a.p.frozenColumns){var h=b(this)[0].id.substring(a.p.id.length+1);b(a.p.colModel).each(function(a){if(this.name===h)return f=a,!1})}else f=sa(this);a.p.viewsortcols[2]||(e=!0,g=c.attr("sort"));null!=f&&ra(b("div",this)[0].id,f,e,g,this);return!1}});
if(a.p.sortable&&b.fn.sortable)try{b(a).jqGrid("sortableColumns",k)}catch(Ba){}a.p.footerrow&&(V+="</tr></tbody></table>");da+="</tr>";k=document.createElement("tbody");this.appendChild(k);b(this).addClass("ui-jqgrid-btable").append(da);var da=null,k=b("<table class='ui-jqgrid-htable' style='width:"+a.p.tblwidth+"px' role='grid' aria-labelledby='gbox_"+this.id+"' cellspacing='0' cellpadding='0' border='0'></table>").append(D),J=a.p.caption&&!0===a.p.hiddengrid?!0:!1;g=b("<div class='ui-jqgrid-hbox"+
("rtl"===f?"-rtl":"")+"'></div>");D=null;c.hDiv=document.createElement("div");b(c.hDiv).css({width:c.width+"px"}).addClass("ui-state-default ui-jqgrid-hdiv").append(g);b(g).append(k);k=null;J&&b(c.hDiv).hide();a.p.pager&&("string"===typeof a.p.pager?"#"!==a.p.pager.substr(0,1)&&(a.p.pager="#"+a.p.pager):a.p.pager="#"+b(a.p.pager).attr("id"),b(a.p.pager).css({width:c.width+"px"}).addClass("ui-state-default ui-jqgrid-pager ui-corner-bottom").appendTo(m),J&&b(a.p.pager).hide(),qa(a.p.pager,""));!1===
a.p.cellEdit&&!0===a.p.hoverrows&&b(a).bind("mouseover",function(a){t=b(a.target).closest("tr.jqgrow");"ui-subgrid"!==b(t).attr("class")&&b(t).addClass("ui-state-hover")}).bind("mouseout",function(a){t=b(a.target).closest("tr.jqgrow");b(t).removeClass("ui-state-hover")});var B,N,ma;b(a).before(c.hDiv).click(function(c){y=c.target;t=b(y,a.rows).closest("tr.jqgrow");if(0===b(t).length||-1<t[0].className.indexOf("ui-state-disabled")||(b(y,a).closest("table.ui-jqgrid-btable").attr("id")||"").replace("_frozen",
"")!==a.id)return this;var d=b(y).hasClass("cbox"),e=b(a).triggerHandler("jqGridBeforeSelectRow",[t[0].id,c]);(e=!1===e||"stop"===e?!1:!0)&&b.isFunction(a.p.beforeSelectRow)&&(e=a.p.beforeSelectRow.call(a,t[0].id,c));if("A"!==y.tagName&&("INPUT"!==y.tagName&&"TEXTAREA"!==y.tagName&&"OPTION"!==y.tagName&&"SELECT"!==y.tagName||d)&&!0===e)if(B=t[0].id,N=b.jgrid.getCellIndex(y),ma=b(y).closest("td,th").html(),b(a).triggerHandler("jqGridCellSelect",[B,N,ma,c]),b.isFunction(a.p.onCellSelect)&&a.p.onCellSelect.call(a,
B,N,ma,c),!0===a.p.cellEdit)if(a.p.multiselect&&d)b(a).jqGrid("setSelection",B,!0,c);else{B=t[0].rowIndex;try{b(a).jqGrid("editCell",B,N,!0)}catch(g){}}else if(a.p.multikey)c[a.p.multikey]?b(a).jqGrid("setSelection",B,!0,c):a.p.multiselect&&d&&(d=b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+B).is(":checked"),b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+B)[a.p.useProp?"prop":"attr"]("checked",d));else{if(a.p.multiselect&&a.p.multiboxonly&&!d){var f=a.p.frozenColumns?a.p.id+"_frozen":"";b(a.p.selarrrow).each(function(c,
d){var e=b(a).jqGrid("getGridRowById",d);b(e).removeClass("ui-state-highlight");b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(d))[a.p.useProp?"prop":"attr"]("checked",!1);f&&(b("#"+b.jgrid.jqID(d),"#"+b.jgrid.jqID(f)).removeClass("ui-state-highlight"),b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(d),"#"+b.jgrid.jqID(f))[a.p.useProp?"prop":"attr"]("checked",!1))});a.p.selarrrow=[]}b(a).jqGrid("setSelection",B,!0,c)}}).bind("reloadGrid",function(c,d){!0===a.p.treeGrid&&(a.p.datatype=a.p.treedatatype);
d&&d.current&&a.grid.selectionPreserver(a);"local"===a.p.datatype?(b(a).jqGrid("resetSelection"),a.p.data.length&&O()):a.p.treeGrid||(a.p.selrow=null,a.p.multiselect&&(a.p.selarrrow=[],ha(!1)),a.p.savedRow=[]);a.p.scroll&&X.call(a,!0,!1);if(d&&d.page){var e=d.page;e>a.p.lastpage&&(e=a.p.lastpage);1>e&&(e=1);a.p.page=e;a.grid.bDiv.scrollTop=a.grid.prevRowHeight?(e-1)*a.grid.prevRowHeight*a.p.rowNum:0}a.grid.prevRowHeight&&a.p.scroll?(delete a.p.lastpage,a.grid.populateVisible()):a.grid.populate();
!0===a.p._inlinenav&&b(a).jqGrid("showAddEditButtons");return!1}).dblclick(function(c){y=c.target;t=b(y,a.rows).closest("tr.jqgrow");0!==b(t).length&&(B=t[0].rowIndex,N=b.jgrid.getCellIndex(y),b(a).triggerHandler("jqGridDblClickRow",[b(t).attr("id"),B,N,c]),b.isFunction(a.p.ondblClickRow)&&a.p.ondblClickRow.call(a,b(t).attr("id"),B,N,c))}).bind("contextmenu",function(c){y=c.target;t=b(y,a.rows).closest("tr.jqgrow");0!==b(t).length&&(a.p.multiselect||b(a).jqGrid("setSelection",t[0].id,!0,c),B=t[0].rowIndex,
N=b.jgrid.getCellIndex(y),b(a).triggerHandler("jqGridRightClickRow",[b(t).attr("id"),B,N,c]),b.isFunction(a.p.onRightClickRow)&&a.p.onRightClickRow.call(a,b(t).attr("id"),B,N,c))});c.bDiv=document.createElement("div");n&&"auto"===String(a.p.height).toLowerCase()&&(a.p.height="100%");b(c.bDiv).append(b('<div style="position:relative;'+(n&&8>b.jgrid.msiever()?"height:0.01%;":"")+'"></div>').append("<div></div>").append(this)).addClass("ui-jqgrid-bdiv").css({height:a.p.height+(isNaN(a.p.height)?"":"px"),
width:c.width+"px"}).scroll(c.scrollGrid);b("table:first",c.bDiv).css({width:a.p.tblwidth+"px"});b.support.tbody||2===b("tbody",this).length&&b("tbody:gt(0)",this).remove();a.p.multikey&&(b.jgrid.msie?b(c.bDiv).bind("selectstart",function(){return!1}):b(c.bDiv).bind("mousedown",function(){return!1}));J&&b(c.bDiv).hide();c.cDiv=document.createElement("div");var na=!0===a.p.hidegrid?b("<a role='link' class='ui-jqgrid-titlebar-close ui-corner-all HeaderButton' />").hover(function(){na.addClass("ui-state-hover")},
function(){na.removeClass("ui-state-hover")}).append("<span class='ui-icon ui-icon-circle-triangle-n'></span>").css("rtl"===f?"left":"right","0px"):"";b(c.cDiv).append(na).append("<span class='ui-jqgrid-title'>"+a.p.caption+"</span>").addClass("ui-jqgrid-titlebar ui-jqgrid-caption"+("rtl"===f?"-rtl":"")+" ui-widget-header ui-corner-top ui-helper-clearfix");b(c.cDiv).insertBefore(c.hDiv);a.p.toolbar[0]&&(c.uDiv=document.createElement("div"),"top"===a.p.toolbar[1]?b(c.uDiv).insertBefore(c.hDiv):"bottom"===
a.p.toolbar[1]&&b(c.uDiv).insertAfter(c.hDiv),"both"===a.p.toolbar[1]?(c.ubDiv=document.createElement("div"),b(c.uDiv).addClass("ui-userdata ui-state-default").attr("id","t_"+this.id).insertBefore(c.hDiv),b(c.ubDiv).addClass("ui-userdata ui-state-default").attr("id","tb_"+this.id).insertAfter(c.hDiv),J&&b(c.ubDiv).hide()):b(c.uDiv).width(c.width).addClass("ui-userdata ui-state-default").attr("id","t_"+this.id),J&&b(c.uDiv).hide());a.p.toppager&&(a.p.toppager=b.jgrid.jqID(a.p.id)+"_toppager",c.topDiv=
b("<div id='"+a.p.toppager+"'></div>")[0],a.p.toppager="#"+a.p.toppager,b(c.topDiv).addClass("ui-state-default ui-jqgrid-toppager").width(c.width).insertBefore(c.hDiv),qa(a.p.toppager,"_t"));a.p.footerrow&&(c.sDiv=b("<div class='ui-jqgrid-sdiv'></div>")[0],g=b("<div class='ui-jqgrid-hbox"+("rtl"===f?"-rtl":"")+"'></div>"),b(c.sDiv).append(g).width(c.width).insertAfter(c.hDiv),b(g).append(V),c.footers=b(".ui-jqgrid-ftable",c.sDiv)[0].rows[0].cells,a.p.rownumbers&&(c.footers[0].className="ui-state-default jqgrid-rownum"),
J&&b(c.sDiv).hide());g=null;if(a.p.caption){var ya=a.p.datatype;!0===a.p.hidegrid&&(b(".ui-jqgrid-titlebar-close",c.cDiv).click(function(d){var e=b.isFunction(a.p.onHeaderClick),g=".ui-jqgrid-bdiv, .ui-jqgrid-hdiv, .ui-jqgrid-pager, .ui-jqgrid-sdiv",f,h=this;!0===a.p.toolbar[0]&&("both"===a.p.toolbar[1]&&(g+=", #"+b(c.ubDiv).attr("id")),g+=", #"+b(c.uDiv).attr("id"));f=b(g,"#gview_"+b.jgrid.jqID(a.p.id)).length;"visible"===a.p.gridstate?b(g,"#gbox_"+b.jgrid.jqID(a.p.id)).slideUp("fast",function(){f--;
0===f&&(b("span",h).removeClass("ui-icon-circle-triangle-n").addClass("ui-icon-circle-triangle-s"),a.p.gridstate="hidden",b("#gbox_"+b.jgrid.jqID(a.p.id)).hasClass("ui-resizable")&&b(".ui-resizable-handle","#gbox_"+b.jgrid.jqID(a.p.id)).hide(),b(a).triggerHandler("jqGridHeaderClick",[a.p.gridstate,d]),e&&(J||a.p.onHeaderClick.call(a,a.p.gridstate,d)))}):"hidden"===a.p.gridstate&&b(g,"#gbox_"+b.jgrid.jqID(a.p.id)).slideDown("fast",function(){f--;0===f&&(b("span",h).removeClass("ui-icon-circle-triangle-s").addClass("ui-icon-circle-triangle-n"),
J&&(a.p.datatype=ya,Q(),J=!1),a.p.gridstate="visible",b("#gbox_"+b.jgrid.jqID(a.p.id)).hasClass("ui-resizable")&&b(".ui-resizable-handle","#gbox_"+b.jgrid.jqID(a.p.id)).show(),b(a).triggerHandler("jqGridHeaderClick",[a.p.gridstate,d]),e&&(J||a.p.onHeaderClick.call(a,a.p.gridstate,d)))});return!1}),J&&(a.p.datatype="local",b(".ui-jqgrid-titlebar-close",c.cDiv).trigger("click")))}else b(c.cDiv).hide();b(c.hDiv).after(c.bDiv).mousemove(function(a){if(c.resizing)return c.dragMove(a),!1});b(".ui-jqgrid-labels",
c.hDiv).bind("selectstart",function(){return!1});b(document).bind("mouseup.jqGrid"+a.p.id,function(){return c.resizing?(c.dragEnd(),!1):!0});a.formatCol=p;a.sortData=ra;a.updatepager=function(c,d){var e,g,f,h,k,l,m,n="",p=a.p.pager?"_"+b.jgrid.jqID(a.p.pager.substr(1)):"",q=a.p.toppager?"_"+a.p.toppager.substr(1):"";f=parseInt(a.p.page,10)-1;0>f&&(f=0);f*=parseInt(a.p.rowNum,10);k=f+a.p.reccount;if(a.p.scroll){e=b("tbody:first > tr:gt(0)",a.grid.bDiv);f=k-e.length;a.p.reccount=e.length;if(e=e.outerHeight()||
a.grid.prevRowHeight)g=f*e,m=parseInt(a.p.records,10)*e,b(">div:first",a.grid.bDiv).css({height:m}).children("div:first").css({height:g,display:g?"":"none"}),0==a.grid.bDiv.scrollTop&&1<a.p.page&&(a.grid.bDiv.scrollTop=a.p.rowNum*(a.p.page-1)*e);a.grid.bDiv.scrollLeft=a.grid.hDiv.scrollLeft}n=a.p.pager||"";if(n+=a.p.toppager?n?","+a.p.toppager:a.p.toppager:"")m=b.jgrid.formatter.integer||{},e=r(a.p.page),g=r(a.p.lastpage),b(".selbox",n)[this.p.useProp?"prop":"attr"]("disabled",!1),!0===a.p.pginput&&
(b(".ui-pg-input",n).val(a.p.page),h=a.p.toppager?"#sp_1"+p+",#sp_1"+q:"#sp_1"+p,b(h).html(b.fmatter?b.fmatter.util.NumberFormat(a.p.lastpage,m):a.p.lastpage)),a.p.viewrecords&&(0===a.p.reccount?b(".ui-paging-info",n).html(a.p.emptyrecords):(h=f+1,l=a.p.records,b.fmatter&&(h=b.fmatter.util.NumberFormat(h,m),k=b.fmatter.util.NumberFormat(k,m),l=b.fmatter.util.NumberFormat(l,m)),b(".ui-paging-info",n).html(b.jgrid.format(a.p.recordtext,h,k,l)))),!0===a.p.pgbuttons&&(0>=e&&(e=g=0),1===e||0===e?(b("#first"+
p+", #prev"+p).addClass("ui-state-disabled").removeClass("ui-state-hover"),a.p.toppager&&b("#first_t"+q+", #prev_t"+q).addClass("ui-state-disabled").removeClass("ui-state-hover")):(b("#first"+p+", #prev"+p).removeClass("ui-state-disabled"),a.p.toppager&&b("#first_t"+q+", #prev_t"+q).removeClass("ui-state-disabled")),e===g||0===e?(b("#next"+p+", #last"+p).addClass("ui-state-disabled").removeClass("ui-state-hover"),a.p.toppager&&b("#next_t"+q+", #last_t"+q).addClass("ui-state-disabled").removeClass("ui-state-hover")):
(b("#next"+p+", #last"+p).removeClass("ui-state-disabled"),a.p.toppager&&b("#next_t"+q+", #last_t"+q).removeClass("ui-state-disabled")));!0===c&&!0===a.p.rownumbers&&b(">td.jqgrid-rownum",a.rows).each(function(a){b(this).html(f+1+a)});d&&a.p.jqgdnd&&b(a).jqGrid("gridDnD","updateDnD");b(a).triggerHandler("jqGridGridComplete");b.isFunction(a.p.gridComplete)&&a.p.gridComplete.call(a);b(a).triggerHandler("jqGridAfterGridComplete")};a.refreshIndex=O;a.setHeadCheckBox=ha;a.constructTr=$;a.formatter=function(a,
b,c,d,e){return x(a,b,c,d,e)};b.extend(c,{populate:Q,emptyRows:X,beginReq:P,endReq:T});this.grid=c;a.addXmlData=function(b){K(b,a.grid.bDiv)};a.addJSONData=function(b){Y(b,a.grid.bDiv)};this.grid.cols=this.rows[0].cells;b(a).triggerHandler("jqGridInitGrid");b.isFunction(a.p.onInitGrid)&&a.p.onInitGrid.call(a);Q();a.p.hiddengrid=!1}}}})};b.jgrid.extend({getGridParam:function(b){var f=this[0];if(f&&f.grid)return b?void 0!==f.p[b]?f.p[b]:null:f.p},setGridParam:function(e){return this.each(function(){this.grid&&
"object"===typeof e&&b.extend(!0,this.p,e)})},getGridRowById:function(e){var f;this.each(function(){try{for(var c=this.rows.length;c--;)if(e.toString()===this.rows[c].id){f=this.rows[c];break}}catch(d){f=b(this.grid.bDiv).find("#"+b.jgrid.jqID(e))}});return f},getDataIDs:function(){var e=[],f=0,c,d=0;this.each(function(){if((c=this.rows.length)&&0<c)for(;f<c;)b(this.rows[f]).hasClass("jqgrow")&&(e[d]=this.rows[f].id,d++),f++});return e},setSelection:function(e,f,c){return this.each(function(){var d,
a,l,g,h,k;void 0!==e&&(f=!1===f?!1:!0,!(a=b(this).jqGrid("getGridRowById",e))||!a.className||-1<a.className.indexOf("ui-state-disabled")||(!0===this.p.scrollrows&&(l=b(this).jqGrid("getGridRowById",e).rowIndex,0<=l&&(d=b(this.grid.bDiv)[0].clientHeight,g=b(this.grid.bDiv)[0].scrollTop,h=b(this.rows[l]).position().top,l=this.rows[l].clientHeight,h+l>=d+g?b(this.grid.bDiv)[0].scrollTop=h-(d+g)+l+g:h<d+g&&h<g&&(b(this.grid.bDiv)[0].scrollTop=h))),!0===this.p.frozenColumns&&(k=this.p.id+"_frozen"),this.p.multiselect?
(this.setHeadCheckBox(!1),this.p.selrow=a.id,g=b.inArray(this.p.selrow,this.p.selarrrow),-1===g?("ui-subgrid"!==a.className&&b(a).addClass("ui-state-highlight").attr("aria-selected","true"),d=!0,this.p.selarrrow.push(this.p.selrow)):("ui-subgrid"!==a.className&&b(a).removeClass("ui-state-highlight").attr("aria-selected","false"),d=!1,this.p.selarrrow.splice(g,1),h=this.p.selarrrow[0],this.p.selrow=void 0===h?null:h),b("#jqg_"+b.jgrid.jqID(this.p.id)+"_"+b.jgrid.jqID(a.id))[this.p.useProp?"prop":"attr"]("checked",
d),k&&(-1===g?b("#"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k)).addClass("ui-state-highlight"):b("#"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k)).removeClass("ui-state-highlight"),b("#jqg_"+b.jgrid.jqID(this.p.id)+"_"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k))[this.p.useProp?"prop":"attr"]("checked",d)),f&&(b(this).triggerHandler("jqGridSelectRow",[a.id,d,c]),this.p.onSelectRow&&this.p.onSelectRow.call(this,a.id,d,c))):"ui-subgrid"!==a.className&&(this.p.selrow!==a.id?(b(b(this).jqGrid("getGridRowById",this.p.selrow)).removeClass("ui-state-highlight").attr({"aria-selected":"false",
tabindex:"-1"}),b(a).addClass("ui-state-highlight").attr({"aria-selected":"true",tabindex:"0"}),k&&(b("#"+b.jgrid.jqID(this.p.selrow),"#"+b.jgrid.jqID(k)).removeClass("ui-state-highlight"),b("#"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k)).addClass("ui-state-highlight")),d=!0):d=!1,this.p.selrow=a.id,f&&(b(this).triggerHandler("jqGridSelectRow",[a.id,d,c]),this.p.onSelectRow&&this.p.onSelectRow.call(this,a.id,d,c)))))})},resetSelection:function(e){return this.each(function(){var f=this,c,d;!0===f.p.frozenColumns&&
(d=f.p.id+"_frozen");if(void 0!==e){c=e===f.p.selrow?f.p.selrow:e;b("#"+b.jgrid.jqID(f.p.id)+" tbody:first tr#"+b.jgrid.jqID(c)).removeClass("ui-state-highlight").attr("aria-selected","false");d&&b("#"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d)).removeClass("ui-state-highlight");if(f.p.multiselect){b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(f.p.id))[f.p.useProp?"prop":"attr"]("checked",!1);if(d)b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d))[f.p.useProp?"prop":
"attr"]("checked",!1);f.setHeadCheckBox(!1)}c=null}else f.p.multiselect?(b(f.p.selarrrow).each(function(a,c){b(b(f).jqGrid("getGridRowById",c)).removeClass("ui-state-highlight").attr("aria-selected","false");b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c))[f.p.useProp?"prop":"attr"]("checked",!1);d&&(b("#"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d)).removeClass("ui-state-highlight"),b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d))[f.p.useProp?"prop":"attr"]("checked",!1))}),f.setHeadCheckBox(!1),
f.p.selarrrow=[],f.p.selrow=null):f.p.selrow&&(b("#"+b.jgrid.jqID(f.p.id)+" tbody:first tr#"+b.jgrid.jqID(f.p.selrow)).removeClass("ui-state-highlight").attr("aria-selected","false"),d&&b("#"+b.jgrid.jqID(f.p.selrow),"#"+b.jgrid.jqID(d)).removeClass("ui-state-highlight"),f.p.selrow=null);!0===f.p.cellEdit&&0<=parseInt(f.p.iCol,10)&&0<=parseInt(f.p.iRow,10)&&(b("td:eq("+f.p.iCol+")",f.rows[f.p.iRow]).removeClass("edit-cell ui-state-highlight"),b(f.rows[f.p.iRow]).removeClass("selected-row ui-state-hover"));
f.p.savedRow=[]})},getRowData:function(e){var f={},c,d=!1,a,l=0;this.each(function(){var g=this,h,k;if(void 0===e)d=!0,c=[],a=g.rows.length;else{k=b(g).jqGrid("getGridRowById",e);if(!k)return f;a=2}for(;l<a;)d&&(k=g.rows[l]),b(k).hasClass("jqgrow")&&(b('td[role="gridcell"]',k).each(function(a){h=g.p.colModel[a].name;if("cb"!==h&&"subgrid"!==h&&"rn"!==h)if(!0===g.p.treeGrid&&h===g.p.ExpandColumn)f[h]=b.jgrid.htmlDecode(b("span:first",this).html());else try{f[h]=b.unformat.call(g,this,{rowId:k.id,colModel:g.p.colModel[a]},
a)}catch(c){f[h]=b.jgrid.htmlDecode(b(this).html())}}),d&&(c.push(f),f={})),l++});return c||f},delRowData:function(e){var f=!1,c,d;this.each(function(){c=b(this).jqGrid("getGridRowById",e);if(!c)return!1;b(c).remove();this.p.records--;this.p.reccount--;this.updatepager(!0,!1);f=!0;this.p.multiselect&&(d=b.inArray(e,this.p.selarrrow),-1!==d&&this.p.selarrrow.splice(d,1));this.p.selrow=this.p.multiselect&&0<this.p.selarrrow.length?this.p.selarrrow[this.p.selarrrow.length-1]:null;if("local"===this.p.datatype){var a=
b.jgrid.stripPref(this.p.idPrefix,e),a=this.p._index[a];void 0!==a&&(this.p.data.splice(a,1),this.refreshIndex())}if(!0===this.p.altRows&&f){var l=this.p.altclass;b(this.rows).each(function(a){1===a%2?b(this).addClass(l):b(this).removeClass(l)})}});return f},setRowData:function(e,f,c){var d,a=!0,l;this.each(function(){if(!this.grid)return!1;var g=this,h,k,n=typeof c,m={};k=b(this).jqGrid("getGridRowById",e);if(!k)return!1;if(f)try{if(b(this.p.colModel).each(function(a){d=this.name;var c=b.jgrid.getAccessor(f,
d);void 0!==c&&(m[d]=this.formatter&&"string"===typeof this.formatter&&"date"===this.formatter?b.unformat.date.call(g,c,this):c,h=g.formatter(e,c,a,f,"edit"),l=this.title?{title:b.jgrid.stripHtml(h)}:{},!0===g.p.treeGrid&&d===g.p.ExpandColumn?b("td[role='gridcell']:eq("+a+") > span:first",k).html(h).attr(l):b("td[role='gridcell']:eq("+a+")",k).html(h).attr(l))}),"local"===g.p.datatype){var r=b.jgrid.stripPref(g.p.idPrefix,e),p=g.p._index[r],q;if(g.p.treeGrid)for(q in g.p.treeReader)g.p.treeReader.hasOwnProperty(q)&&
delete m[g.p.treeReader[q]];void 0!==p&&(g.p.data[p]=b.extend(!0,g.p.data[p],m));m=null}}catch(x){a=!1}a&&("string"===n?b(k).addClass(c):null!==c&&"object"===n&&b(k).css(c),b(g).triggerHandler("jqGridAfterGridComplete"))});return a},addRowData:function(e,f,c,d){c||(c="last");var a=!1,l,g,h,k,n,m,r,p,q="",x,G,U,M,ea,W;f&&(b.isArray(f)?(x=!0,c="last",G=e):(f=[f],x=!1),this.each(function(){var X=f.length;n=!0===this.p.rownumbers?1:0;h=!0===this.p.multiselect?1:0;k=!0===this.p.subGrid?1:0;x||(void 0!==
e?e=String(e):(e=b.jgrid.randId(),!1!==this.p.keyIndex&&(G=this.p.colModel[this.p.keyIndex+h+k+n].name,void 0!==f[0][G]&&(e=f[0][G]))));U=this.p.altclass;for(var O=0,$="",K={},Y=b.isFunction(this.p.afterInsertRow)?!0:!1;O<X;){M=f[O];g=[];if(x){try{e=M[G],void 0===e&&(e=b.jgrid.randId())}catch(oa){e=b.jgrid.randId()}$=!0===this.p.altRows?0===(this.rows.length-1)%2?U:"":""}W=e;e=this.p.idPrefix+e;n&&(q=this.formatCol(0,1,"",null,e,!0),g[g.length]='<td role="gridcell" class="ui-state-default jqgrid-rownum" '+
q+">0</td>");h&&(p='<input role="checkbox" type="checkbox" id="jqg_'+this.p.id+"_"+e+'" class="cbox"/>',q=this.formatCol(n,1,"",null,e,!0),g[g.length]='<td role="gridcell" '+q+">"+p+"</td>");k&&(g[g.length]=b(this).jqGrid("addSubGridCell",h+n,1));for(r=h+k+n;r<this.p.colModel.length;r++)ea=this.p.colModel[r],l=ea.name,K[l]=M[l],p=this.formatter(e,b.jgrid.getAccessor(M,l),r,M),q=this.formatCol(r,1,p,M,e,K),g[g.length]='<td role="gridcell" '+q+">"+p+"</td>";g.unshift(this.constructTr(e,!1,$,K,M,!1));
g[g.length]="</tr>";if(0===this.rows.length)b("table:first",this.grid.bDiv).append(g.join(""));else switch(c){case "last":b(this.rows[this.rows.length-1]).after(g.join(""));m=this.rows.length-1;break;case "first":b(this.rows[0]).after(g.join(""));m=1;break;case "after":if(m=b(this).jqGrid("getGridRowById",d))b(this.rows[m.rowIndex+1]).hasClass("ui-subgrid")?b(this.rows[m.rowIndex+1]).after(g):b(m).after(g.join("")),m=m.rowIndex+1;break;case "before":if(m=b(this).jqGrid("getGridRowById",d))b(m).before(g.join("")),
m=m.rowIndex-1}!0===this.p.subGrid&&b(this).jqGrid("addSubGrid",h+n,m);this.p.records++;this.p.reccount++;b(this).triggerHandler("jqGridAfterInsertRow",[e,M,M]);Y&&this.p.afterInsertRow.call(this,e,M,M);O++;"local"===this.p.datatype&&(K[this.p.localReader.id]=W,this.p._index[W]=this.p.data.length,this.p.data.push(K),K={})}!0!==this.p.altRows||x||("last"===c?1===(this.rows.length-1)%2&&b(this.rows[this.rows.length-1]).addClass(U):b(this.rows).each(function(a){1===a%2?b(this).addClass(U):b(this).removeClass(U)}));
this.updatepager(!0,!0);a=!0}));return a},footerData:function(e,f,c){function d(a){for(var b in a)if(a.hasOwnProperty(b))return!1;return!0}var a,l=!1,g={},h;void 0==e&&(e="get");"boolean"!==typeof c&&(c=!0);e=e.toLowerCase();this.each(function(){var k=this,n;if(!k.grid||!k.p.footerrow||"set"===e&&d(f))return!1;l=!0;b(this.p.colModel).each(function(d){a=this.name;"set"===e?void 0!==f[a]&&(n=c?k.formatter("",f[a],d,f,"edit"):f[a],h=this.title?{title:b.jgrid.stripHtml(n)}:{},b("tr.footrow td:eq("+d+
")",k.grid.sDiv).html(n).attr(h),l=!0):"get"===e&&(g[a]=b("tr.footrow td:eq("+d+")",k.grid.sDiv).html())})});return"get"===e?g:l},showHideCol:function(e,f){return this.each(function(){var c=this,d=!1,a=b.jgrid.cell_width?0:c.p.cellLayout,l;if(c.grid){"string"===typeof e&&(e=[e]);f="none"!==f?"":"none";var g=""===f?!0:!1,h=c.p.groupHeader&&("object"===typeof c.p.groupHeader||b.isFunction(c.p.groupHeader));h&&b(c).jqGrid("destroyGroupHeader",!1);b(this.p.colModel).each(function(h){if(-1!==b.inArray(this.name,
e)&&this.hidden===g){if(!0===c.p.frozenColumns&&!0===this.frozen)return!0;b("tr[role=rowheader]",c.grid.hDiv).each(function(){b(this.cells[h]).css("display",f)});b(c.rows).each(function(){b(this).hasClass("jqgroup")||b(this.cells[h]).css("display",f)});c.p.footerrow&&b("tr.footrow td:eq("+h+")",c.grid.sDiv).css("display",f);l=parseInt(this.width,10);c.p.tblwidth="none"===f?c.p.tblwidth-(l+a):c.p.tblwidth+(l+a);this.hidden=!g;d=!0;b(c).triggerHandler("jqGridShowHideCol",[g,this.name,h])}});!0===d&&
(!0!==c.p.shrinkToFit||isNaN(c.p.height)||(c.p.tblwidth+=parseInt(c.p.scrollOffset,10)),b(c).jqGrid("setGridWidth",!0===c.p.shrinkToFit?c.p.tblwidth:c.p.width));h&&b(c).jqGrid("setGroupHeaders",c.p.groupHeader)}})},hideCol:function(e){return this.each(function(){b(this).jqGrid("showHideCol",e,"none")})},showCol:function(e){return this.each(function(){b(this).jqGrid("showHideCol",e,"")})},remapColumns:function(e,f,c){function d(a){var c;c=a.length?b.makeArray(a):b.extend({},a);b.each(e,function(b){a[b]=
c[this]})}function a(a,c){b(">tr"+(c||""),a).each(function(){var a=this,c=b.makeArray(a.cells);b.each(e,function(){var b=c[this];b&&a.appendChild(b)})})}var l=this.get(0);d(l.p.colModel);d(l.p.colNames);d(l.grid.headers);a(b("thead:first",l.grid.hDiv),c&&":not(.ui-jqgrid-labels)");f&&a(b("#"+b.jgrid.jqID(l.p.id)+" tbody:first"),".jqgfirstrow, tr.jqgrow, tr.jqfoot");l.p.footerrow&&a(b("tbody:first",l.grid.sDiv));l.p.remapColumns&&(l.p.remapColumns.length?d(l.p.remapColumns):l.p.remapColumns=b.makeArray(e));
l.p.lastsort=b.inArray(l.p.lastsort,e);l.p.treeGrid&&(l.p.expColInd=b.inArray(l.p.expColInd,e));b(l).triggerHandler("jqGridRemapColumns",[e,f,c])},setGridWidth:function(e,f){return this.each(function(){if(this.grid){var c=this,d,a=0,l=b.jgrid.cell_width?0:c.p.cellLayout,g,h=0,k=!1,n=c.p.scrollOffset,m,r=0,p;"boolean"!==typeof f&&(f=c.p.shrinkToFit);if(!isNaN(e)){e=parseInt(e,10);c.grid.width=c.p.width=e;b("#gbox_"+b.jgrid.jqID(c.p.id)).css("width",e+"px");b("#gview_"+b.jgrid.jqID(c.p.id)).css("width",
e+"px");b(c.grid.bDiv).css("width",e+"px");b(c.grid.hDiv).css("width",e+"px");c.p.pager&&b(c.p.pager).css("width",e+"px");c.p.toppager&&b(c.p.toppager).css("width",e+"px");!0===c.p.toolbar[0]&&(b(c.grid.uDiv).css("width",e+"px"),"both"===c.p.toolbar[1]&&b(c.grid.ubDiv).css("width",e+"px"));c.p.footerrow&&b(c.grid.sDiv).css("width",e+"px");!1===f&&!0===c.p.forceFit&&(c.p.forceFit=!1);if(!0===f){b.each(c.p.colModel,function(){!1===this.hidden&&(d=this.widthOrg,a+=d+l,this.fixed?r+=d+l:h++)});if(0===
h)return;c.p.tblwidth=a;m=e-l*h-r;!isNaN(c.p.height)&&(b(c.grid.bDiv)[0].clientHeight<b(c.grid.bDiv)[0].scrollHeight||1===c.rows.length)&&(k=!0,m-=n);var a=0,q=0<c.grid.cols.length;b.each(c.p.colModel,function(b){!1!==this.hidden||this.fixed||(d=this.widthOrg,d=Math.round(m*d/(c.p.tblwidth-l*h-r)),0>d||(this.width=d,a+=d,c.grid.headers[b].width=d,c.grid.headers[b].el.style.width=d+"px",c.p.footerrow&&(c.grid.footers[b].style.width=d+"px"),q&&(c.grid.cols[b].style.width=d+"px"),g=b))});if(!g)return;
p=0;k?e-r-(a+l*h)!==n&&(p=e-r-(a+l*h)-n):1!==Math.abs(e-r-(a+l*h))&&(p=e-r-(a+l*h));c.p.colModel[g].width+=p;c.p.tblwidth=a+p+l*h+r;c.p.tblwidth>e?(k=c.p.tblwidth-parseInt(e,10),c.p.tblwidth=e,d=c.p.colModel[g].width-=k):d=c.p.colModel[g].width;c.grid.headers[g].width=d;c.grid.headers[g].el.style.width=d+"px";q&&(c.grid.cols[g].style.width=d+"px");c.p.footerrow&&(c.grid.footers[g].style.width=d+"px")}c.p.tblwidth&&(b("table:first",c.grid.bDiv).css("width",c.p.tblwidth+"px"),b("table:first",c.grid.hDiv).css("width",
c.p.tblwidth+"px"),c.grid.hDiv.scrollLeft=c.grid.bDiv.scrollLeft,c.p.footerrow&&b("table:first",c.grid.sDiv).css("width",c.p.tblwidth+"px"))}}})},setGridHeight:function(e){return this.each(function(){if(this.grid){var f=b(this.grid.bDiv);f.css({height:e+(isNaN(e)?"":"px")});!0===this.p.frozenColumns&&b("#"+b.jgrid.jqID(this.p.id)+"_frozen").parent().height(f.height()-16);this.p.height=e;this.p.scroll&&this.grid.populateVisible()}})},setCaption:function(e){return this.each(function(){this.p.caption=
e;b("span.ui-jqgrid-title, span.ui-jqgrid-title-rtl",this.grid.cDiv).html(e);b(this.grid.cDiv).show()})},setLabel:function(e,f,c,d){return this.each(function(){var a=-1;if(this.grid&&void 0!==e&&(b(this.p.colModel).each(function(b){if(this.name===e)return a=b,!1}),0<=a)){var l=b("tr.ui-jqgrid-labels th:eq("+a+")",this.grid.hDiv);if(f){var g=b(".s-ico",l);b("[id^=jqgh_]",l).empty().html(f).append(g);this.p.colNames[a]=f}c&&("string"===typeof c?b(l).addClass(c):b(l).css(c));"object"===typeof d&&b(l).attr(d)}})},
setCell:function(e,f,c,d,a,l){return this.each(function(){var g=-1,h,k;if(this.grid&&(isNaN(f)?b(this.p.colModel).each(function(a){if(this.name===f)return g=a,!1}):g=parseInt(f,10),0<=g&&(h=b(this).jqGrid("getGridRowById",e)))){var n=b("td:eq("+g+")",h);if(""!==c||!0===l)h=this.formatter(e,c,g,h,"edit"),k=this.p.colModel[g].title?{title:b.jgrid.stripHtml(h)}:{},this.p.treeGrid&&0<b(".tree-wrap",b(n)).length?b("span",b(n)).html(h).attr(k):b(n).html(h).attr(k),"local"===this.p.datatype&&(h=this.p.colModel[g],
c=h.formatter&&"string"===typeof h.formatter&&"date"===h.formatter?b.unformat.date.call(this,c,h):c,k=this.p._index[b.jgrid.stripPref(this.p.idPrefix,e)],void 0!==k&&(this.p.data[k][h.name]=c));"string"===typeof d?b(n).addClass(d):d&&b(n).css(d);"object"===typeof a&&b(n).attr(a)}})},getCell:function(e,f){var c=!1;this.each(function(){var d=-1;if(this.grid&&(isNaN(f)?b(this.p.colModel).each(function(a){if(this.name===f)return d=a,!1}):d=parseInt(f,10),0<=d)){var a=b(this).jqGrid("getGridRowById",e);
if(a)try{c=b.unformat.call(this,b("td:eq("+d+")",a),{rowId:a.id,colModel:this.p.colModel[d]},d)}catch(l){c=b.jgrid.htmlDecode(b("td:eq("+d+")",a).html())}}});return c},getCol:function(e,f,c){var d=[],a,l=0,g,h,k;f="boolean"!==typeof f?!1:f;void 0===c&&(c=!1);this.each(function(){var n=-1;if(this.grid&&(isNaN(e)?b(this.p.colModel).each(function(a){if(this.name===e)return n=a,!1}):n=parseInt(e,10),0<=n)){var m=this.rows.length,r=0,p=0;if(m&&0<m){for(;r<m;){if(b(this.rows[r]).hasClass("jqgrow")){try{a=
b.unformat.call(this,b(this.rows[r].cells[n]),{rowId:this.rows[r].id,colModel:this.p.colModel[n]},n)}catch(q){a=b.jgrid.htmlDecode(this.rows[r].cells[n].innerHTML)}c?(k=parseFloat(a),isNaN(k)||(l+=k,void 0===h&&(h=g=k),g=Math.min(g,k),h=Math.max(h,k),p++)):f?d.push({id:this.rows[r].id,value:a}):d.push(a)}r++}if(c)switch(c.toLowerCase()){case "sum":d=l;break;case "avg":d=l/p;break;case "count":d=m-1;break;case "min":d=g;break;case "max":d=h}}}});return d},clearGridData:function(e){return this.each(function(){if(this.grid){"boolean"!==
typeof e&&(e=!1);if(this.p.deepempty)b("#"+b.jgrid.jqID(this.p.id)+" tbody:first tr:gt(0)").remove();else{var f=b("#"+b.jgrid.jqID(this.p.id)+" tbody:first tr:first")[0];b("#"+b.jgrid.jqID(this.p.id)+" tbody:first").empty().append(f)}this.p.footerrow&&e&&b(".ui-jqgrid-ftable td",this.grid.sDiv).html("&#160;");this.p.selrow=null;this.p.selarrrow=[];this.p.savedRow=[];this.p.records=0;this.p.page=1;this.p.lastpage=0;this.p.reccount=0;this.p.data=[];this.p._index={};this.updatepager(!0,!1)}})},getInd:function(e,
f){var c=!1,d;this.each(function(){(d=b(this).jqGrid("getGridRowById",e))&&(c=!0===f?d:d.rowIndex)});return c},bindKeys:function(e){var f=b.extend({onEnter:null,onSpace:null,onLeftKey:null,onRightKey:null,scrollingRows:!0},e||{});return this.each(function(){var c=this;b("body").is("[role]")||b("body").attr("role","application");c.p.scrollrows=f.scrollingRows;b(c).keydown(function(d){var a=b(c).find("tr[tabindex=0]")[0],e,g,h,k=c.p.treeReader.expanded_field;if(a)if(h=c.p._index[b.jgrid.stripPref(c.p.idPrefix,
a.id)],37===d.keyCode||38===d.keyCode||39===d.keyCode||40===d.keyCode){if(38===d.keyCode){g=a.previousSibling;e="";if(g)if(b(g).is(":hidden"))for(;g;){if(g=g.previousSibling,!b(g).is(":hidden")&&b(g).hasClass("jqgrow")){e=g.id;break}}else e=g.id;b(c).jqGrid("setSelection",e,!0,d);d.preventDefault()}if(40===d.keyCode){g=a.nextSibling;e="";if(g)if(b(g).is(":hidden"))for(;g;){if(g=g.nextSibling,!b(g).is(":hidden")&&b(g).hasClass("jqgrow")){e=g.id;break}}else e=g.id;b(c).jqGrid("setSelection",e,!0,d);
d.preventDefault()}37===d.keyCode&&(c.p.treeGrid&&c.p.data[h][k]&&b(a).find("div.treeclick").trigger("click"),b(c).triggerHandler("jqGridKeyLeft",[c.p.selrow]),b.isFunction(f.onLeftKey)&&f.onLeftKey.call(c,c.p.selrow));39===d.keyCode&&(c.p.treeGrid&&!c.p.data[h][k]&&b(a).find("div.treeclick").trigger("click"),b(c).triggerHandler("jqGridKeyRight",[c.p.selrow]),b.isFunction(f.onRightKey)&&f.onRightKey.call(c,c.p.selrow))}else 13===d.keyCode?(b(c).triggerHandler("jqGridKeyEnter",[c.p.selrow]),b.isFunction(f.onEnter)&&
f.onEnter.call(c,c.p.selrow)):32===d.keyCode&&(b(c).triggerHandler("jqGridKeySpace",[c.p.selrow]),b.isFunction(f.onSpace)&&f.onSpace.call(c,c.p.selrow))})})},unbindKeys:function(){return this.each(function(){b(this).unbind("keydown")})},getLocalRow:function(e){var f=!1,c;this.each(function(){void 0!==e&&(c=this.p._index[b.jgrid.stripPref(this.p.idPrefix,e)],0<=c&&(f=this.p.data[c]))});return f}})})(jQuery);
(function(a){a.fmatter={};a.extend(a.fmatter,{isBoolean:function(a){return"boolean"===typeof a},isObject:function(c){return c&&("object"===typeof c||a.isFunction(c))||!1},isString:function(a){return"string"===typeof a},isNumber:function(a){return"number"===typeof a&&isFinite(a)},isValue:function(a){return this.isObject(a)||this.isString(a)||this.isNumber(a)||this.isBoolean(a)},isEmpty:function(c){if(!this.isString(c)&&this.isValue(c))return!1;if(!this.isValue(c))return!0;c=a.trim(c).replace(/\&nbsp\;/ig,
"").replace(/\&#160\;/ig,"");return""===c}});a.fn.fmatter=function(c,b,d,e,f){var g=b;d=a.extend({},a.jgrid.formatter,d);try{g=a.fn.fmatter[c].call(this,b,d,e,f)}catch(h){}return g};a.fmatter.util={NumberFormat:function(c,b){a.fmatter.isNumber(c)||(c*=1);if(a.fmatter.isNumber(c)){var d=0>c,e=String(c),f=b.decimalSeparator||".",g;if(a.fmatter.isNumber(b.decimalPlaces)){var h=b.decimalPlaces,e=Math.pow(10,h),e=String(Math.round(c*e)/e);g=e.lastIndexOf(".");if(0<h)for(0>g?(e+=f,g=e.length-1):"."!==f&&
(e=e.replace(".",f));e.length-1-g<h;)e+="0"}if(b.thousandsSeparator){h=b.thousandsSeparator;g=e.lastIndexOf(f);g=-1<g?g:e.length;var f=e.substring(g),l=-1,k;for(k=g;0<k;k--)l++,0===l%3&&k!==g&&(!d||1<k)&&(f=h+f),f=e.charAt(k-1)+f;e=f}e=b.prefix?b.prefix+e:e;return e=b.suffix?e+b.suffix:e}return c}};a.fn.fmatter.defaultFormat=function(c,b){return a.fmatter.isValue(c)&&""!==c?c:b.defaultValue||"&#160;"};a.fn.fmatter.email=function(c,b){return a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):'<a href="mailto:'+
c+'">'+c+"</a>"};a.fn.fmatter.checkbox=function(c,b){var d=a.extend({},b.checkbox),e;void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));e=!0===d.disabled?'disabled="disabled"':"";if(a.fmatter.isEmpty(c)||void 0===c)c=a.fn.fmatter.defaultFormat(c,d);c=String(c);c=(c+"").toLowerCase();return'<input type="checkbox" '+(0>c.search(/(false|f|0|no|n|off|undefined)/i)?" checked='checked' ":"")+' value="'+c+'" offval="no" '+e+"/>"};a.fn.fmatter.link=function(c,
b){var d={target:b.target},e="";void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));d.target&&(e="target="+d.target);return a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):"<a "+e+' href="'+c+'">'+c+"</a>"};a.fn.fmatter.showlink=function(c,b){var d={baseLinkUrl:b.baseLinkUrl,showAction:b.showAction,addParam:b.addParam||"",target:b.target,idName:b.idName},e="";void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));
d.target&&(e="target="+d.target);d=d.baseLinkUrl+d.showAction+"?"+d.idName+"="+b.rowId+d.addParam;return a.fmatter.isString(c)||a.fmatter.isNumber(c)?"<a "+e+' href="'+d+'">'+c+"</a>":a.fn.fmatter.defaultFormat(c,b)};a.fn.fmatter.integer=function(c,b){var d=a.extend({},b.integer);void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return a.fmatter.isEmpty(c)?d.defaultValue:a.fmatter.util.NumberFormat(c,d)};a.fn.fmatter.number=function(c,b){var d=a.extend({},
b.number);void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return a.fmatter.isEmpty(c)?d.defaultValue:a.fmatter.util.NumberFormat(c,d)};a.fn.fmatter.currency=function(c,b){var d=a.extend({},b.currency);void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return a.fmatter.isEmpty(c)?d.defaultValue:a.fmatter.util.NumberFormat(c,d)};a.fn.fmatter.date=function(c,b,d,e){d=a.extend({},b.date);void 0!==b.colModel&&
void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return d.reformatAfterEdit||"edit"!==e?a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):a.jgrid.parseDate(d.srcformat,c,d.newformat,d):a.fn.fmatter.defaultFormat(c,b)};a.fn.fmatter.select=function(c,b){c=String(c);var d=!1,e=[],f,g;void 0!==b.colModel.formatoptions?(d=b.colModel.formatoptions.value,f=void 0===b.colModel.formatoptions.separator?":":b.colModel.formatoptions.separator,g=void 0===b.colModel.formatoptions.delimiter?
";":b.colModel.formatoptions.delimiter):void 0!==b.colModel.editoptions&&(d=b.colModel.editoptions.value,f=void 0===b.colModel.editoptions.separator?":":b.colModel.editoptions.separator,g=void 0===b.colModel.editoptions.delimiter?";":b.colModel.editoptions.delimiter);if(d){var h=!0===b.colModel.editoptions.multiple?!0:!1,l=[];h&&(l=c.split(","),l=a.map(l,function(b){return a.trim(b)}));if(a.fmatter.isString(d)){var k=d.split(g),m=0,n;for(n=0;n<k.length;n++)if(g=k[n].split(f),2<g.length&&(g[1]=a.map(g,
function(a,b){if(0<b)return a}).join(f)),h)-1<a.inArray(g[0],l)&&(e[m]=g[1],m++);else if(a.trim(g[0])===a.trim(c)){e[0]=g[1];break}}else a.fmatter.isObject(d)&&(h?e=a.map(l,function(a){return d[a]}):e[0]=d[c]||"")}c=e.join(", ");return""===c?a.fn.fmatter.defaultFormat(c,b):c};a.fn.fmatter.rowactions=function(c){var b=a(this).closest("tr.jqgrow"),d=b.attr("id"),e=a(this).closest("table.ui-jqgrid-btable").attr("id").replace(/_frozen([^_]*)$/,"$1"),e=a("#"+e),f=e[0],g=f.p,h=g.colModel[a.jgrid.getCellIndex(this)],
l=h.frozen?a("tr#"+d+" td:eq("+a.jgrid.getCellIndex(this)+") > div",e):a(this).parent(),k={extraparam:{}},m=function(b){a.isFunction(k.afterRestore)&&k.afterRestore.call(f,b);l.find("div.ui-inline-edit,div.ui-inline-del").show();l.find("div.ui-inline-save,div.ui-inline-cancel").hide()};void 0!==h.formatoptions&&(k=a.extend(k,h.formatoptions));void 0!==g.editOptions&&(k.editOptions=g.editOptions);void 0!==g.delOptions&&(k.delOptions=g.delOptions);b.hasClass("jqgrid-new-row")&&(k.extraparam[g.prmNames.oper]=
g.prmNames.addoper);b={keys:k.keys,oneditfunc:k.onEdit,successfunc:k.onSuccess,url:k.url,extraparam:k.extraparam,aftersavefunc:function(b,c){a.isFunction(k.afterSave)&&k.afterSave.call(f,b,c);l.find("div.ui-inline-edit,div.ui-inline-del").show();l.find("div.ui-inline-save,div.ui-inline-cancel").hide()},errorfunc:k.onError,afterrestorefunc:m,restoreAfterError:k.restoreAfterError,mtype:k.mtype};switch(c){case "edit":e.jqGrid("editRow",d,b);l.find("div.ui-inline-edit,div.ui-inline-del").hide();l.find("div.ui-inline-save,div.ui-inline-cancel").show();
e.triggerHandler("jqGridAfterGridComplete");break;case "save":e.jqGrid("saveRow",d,b)&&(l.find("div.ui-inline-edit,div.ui-inline-del").show(),l.find("div.ui-inline-save,div.ui-inline-cancel").hide(),e.triggerHandler("jqGridAfterGridComplete"));break;case "cancel":e.jqGrid("restoreRow",d,m);l.find("div.ui-inline-edit,div.ui-inline-del").show();l.find("div.ui-inline-save,div.ui-inline-cancel").hide();e.triggerHandler("jqGridAfterGridComplete");break;case "del":e.jqGrid("delGridRow",d,k.delOptions);
break;case "formedit":e.jqGrid("setSelection",d),e.jqGrid("editGridRow",d,k.editOptions)}};a.fn.fmatter.actions=function(c,b){var d={keys:!1,editbutton:!0,delbutton:!0,editformbutton:!1},e=b.rowId,f="";void 0!==b.colModel.formatoptions&&(d=a.extend(d,b.colModel.formatoptions));if(void 0===e||a.fmatter.isEmpty(e))return"";d.editformbutton?f+="<div title='"+a.jgrid.nav.edittitle+"' style='float:left;cursor:pointer;' class='ui-pg-div ui-inline-edit' "+("id='jEditButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'formedit'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+
"><span class='ui-icon ui-icon-pencil'></span></div>":d.editbutton&&(f+="<div title='"+a.jgrid.nav.edittitle+"' style='float:left;cursor:pointer;' class='ui-pg-div ui-inline-edit' "+("id='jEditButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'edit'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover') ")+"><span class='ui-icon ui-icon-pencil'></span></div>");d.delbutton&&(f+="<div title='"+a.jgrid.nav.deltitle+"' style='float:left;margin-left:5px;' class='ui-pg-div ui-inline-del' "+
("id='jDeleteButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'del'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+"><span class='ui-icon ui-icon-trash'></span></div>");f+="<div title='"+a.jgrid.edit.bSubmit+"' style='float:left;display:none' class='ui-pg-div ui-inline-save' "+("id='jSaveButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'save'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+
"><span class='ui-icon ui-icon-disk'></span></div>";f+="<div title='"+a.jgrid.edit.bCancel+"' style='float:left;display:none;margin-left:5px;' class='ui-pg-div ui-inline-cancel' "+("id='jCancelButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'cancel'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+"><span class='ui-icon ui-icon-cancel'></span></div>";return"<div style='margin-left:8px;'>"+f+"</div>"};a.unformat=function(c,
b,d,e){var f,g=b.colModel.formatter,h=b.colModel.formatoptions||{},l=/([\.\*\_\'\(\)\{\}\+\?\\])/g,k=b.colModel.unformat||a.fn.fmatter[g]&&a.fn.fmatter[g].unformat;if(void 0!==k&&a.isFunction(k))f=k.call(this,a(c).text(),b,c);else if(void 0!==g&&a.fmatter.isString(g))switch(f=a.jgrid.formatter||{},g){case "integer":h=a.extend({},f.integer,h);b=h.thousandsSeparator.replace(l,"\\$1");b=RegExp(b,"g");f=a(c).text().replace(b,"");break;case "number":h=a.extend({},f.number,h);b=h.thousandsSeparator.replace(l,
"\\$1");b=RegExp(b,"g");f=a(c).text().replace(b,"").replace(h.decimalSeparator,".");break;case "currency":h=a.extend({},f.currency,h);b=h.thousandsSeparator.replace(l,"\\$1");b=RegExp(b,"g");f=a(c).text();h.prefix&&h.prefix.length&&(f=f.substr(h.prefix.length));h.suffix&&h.suffix.length&&(f=f.substr(0,f.length-h.suffix.length));f=f.replace(b,"").replace(h.decimalSeparator,".");break;case "checkbox":h=b.colModel.editoptions?b.colModel.editoptions.value.split(":"):["Yes","No"];f=a("input",c).is(":checked")?
h[0]:h[1];break;case "select":f=a.unformat.select(c,b,d,e);break;case "actions":return"";default:f=a(c).text()}return void 0!==f?f:!0===e?a(c).text():a.jgrid.htmlDecode(a(c).html())};a.unformat.select=function(c,b,d,e){d=[];c=a(c).text();if(!0===e)return c;e=a.extend({},void 0!==b.colModel.formatoptions?b.colModel.formatoptions:b.colModel.editoptions);b=void 0===e.separator?":":e.separator;var f=void 0===e.delimiter?";":e.delimiter;if(e.value){var g=e.value;e=!0===e.multiple?!0:!1;var h=[];e&&(h=
c.split(","),h=a.map(h,function(b){return a.trim(b)}));if(a.fmatter.isString(g)){var l=g.split(f),k=0,m;for(m=0;m<l.length;m++)if(f=l[m].split(b),2<f.length&&(f[1]=a.map(f,function(a,b){if(0<b)return a}).join(b)),e)-1<a.inArray(f[1],h)&&(d[k]=f[0],k++);else if(a.trim(f[1])===a.trim(c)){d[0]=f[0];break}}else if(a.fmatter.isObject(g)||a.isArray(g))e||(h[0]=c),d=a.map(h,function(b){var c;a.each(g,function(a,d){if(d===b)return c=a,!1});if(void 0!==c)return c});return d.join(", ")}return c||""};a.unformat.date=
function(c,b){var d=a.jgrid.formatter.date||{};void 0!==b.formatoptions&&(d=a.extend({},d,b.formatoptions));return a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):a.jgrid.parseDate(d.newformat,c,d.srcformat,d)}})(jQuery);
(function(a){a.jgrid.extend({getColProp:function(a){var c={},d=this[0];if(!d.grid)return!1;var d=d.p.colModel,h;for(h=0;h<d.length;h++)if(d[h].name===a){c=d[h];break}return c},setColProp:function(b,c){return this.each(function(){if(this.grid&&c){var d=this.p.colModel,h;for(h=0;h<d.length;h++)if(d[h].name===b){a.extend(!0,this.p.colModel[h],c);break}}})},sortGrid:function(a,c,d){return this.each(function(){var h=-1,k,e=!1;if(this.grid){a||(a=this.p.sortname);for(k=0;k<this.p.colModel.length;k++)if(this.p.colModel[k].index===
a||this.p.colModel[k].name===a){h=k;!0===this.p.frozenColumns&&!0===this.p.colModel[k].frozen&&(e=this.grid.fhDiv.find("#"+this.p.id+"_"+a));break}-1!==h&&(k=this.p.colModel[h].sortable,e||(e=this.grid.headers[h].el),"boolean"!==typeof k&&(k=!0),"boolean"!==typeof c&&(c=!1),k&&this.sortData("jqgh_"+this.p.id+"_"+a,h,c,d,e))}})},clearBeforeUnload:function(){return this.each(function(){var b=this.grid;a.isFunction(b.emptyRows)&&b.emptyRows.call(this,!0,!0);a(document).unbind("mouseup.jqGrid"+this.p.id);
a(b.hDiv).unbind("mousemove");a(this).unbind();b.dragEnd=null;b.dragMove=null;b.dragStart=null;b.emptyRows=null;b.populate=null;b.populateVisible=null;b.scrollGrid=null;b.selectionPreserver=null;b.bDiv=null;b.cDiv=null;b.hDiv=null;b.cols=null;var c,d=b.headers.length;for(c=0;c<d;c++)b.headers[c].el=null;this.grid=this.addJSONData=this.addXmlData=this.formatter=this.constructTr=this.setHeadCheckBox=this.refreshIndex=this.updatepager=this.sortData=this.formatCol=null})},GridDestroy:function(){return this.each(function(){if(this.grid){this.p.pager&&
a(this.p.pager).remove();try{a(this).jqGrid("clearBeforeUnload"),a("#gbox_"+a.jgrid.jqID(this.id)).remove()}catch(b){}}})},GridUnload:function(){return this.each(function(){if(this.grid){var b=a(this).attr("id"),c=a(this).attr("class");this.p.pager&&a(this.p.pager).empty().removeClass("ui-state-default ui-jqgrid-pager ui-corner-bottom");var d=document.createElement("table");a(d).attr({id:b});d.className=c;b=a.jgrid.jqID(this.id);a(d).removeClass("ui-jqgrid-btable");1===a(this.p.pager).parents("#gbox_"+
b).length?(a(d).insertBefore("#gbox_"+b).show(),a(this.p.pager).insertBefore("#gbox_"+b)):a(d).insertBefore("#gbox_"+b).show();a(this).jqGrid("clearBeforeUnload");a("#gbox_"+b).remove()}})},setGridState:function(b){return this.each(function(){this.grid&&("hidden"===b?(a(".ui-jqgrid-bdiv, .ui-jqgrid-hdiv","#gview_"+a.jgrid.jqID(this.p.id)).slideUp("fast"),this.p.pager&&a(this.p.pager).slideUp("fast"),this.p.toppager&&a(this.p.toppager).slideUp("fast"),!0===this.p.toolbar[0]&&("both"===this.p.toolbar[1]&&
a(this.grid.ubDiv).slideUp("fast"),a(this.grid.uDiv).slideUp("fast")),this.p.footerrow&&a(".ui-jqgrid-sdiv","#gbox_"+a.jgrid.jqID(this.p.id)).slideUp("fast"),a(".ui-jqgrid-titlebar-close span",this.grid.cDiv).removeClass("ui-icon-circle-triangle-n").addClass("ui-icon-circle-triangle-s"),this.p.gridstate="hidden"):"visible"===b&&(a(".ui-jqgrid-hdiv, .ui-jqgrid-bdiv","#gview_"+a.jgrid.jqID(this.p.id)).slideDown("fast"),this.p.pager&&a(this.p.pager).slideDown("fast"),this.p.toppager&&a(this.p.toppager).slideDown("fast"),
!0===this.p.toolbar[0]&&("both"===this.p.toolbar[1]&&a(this.grid.ubDiv).slideDown("fast"),a(this.grid.uDiv).slideDown("fast")),this.p.footerrow&&a(".ui-jqgrid-sdiv","#gbox_"+a.jgrid.jqID(this.p.id)).slideDown("fast"),a(".ui-jqgrid-titlebar-close span",this.grid.cDiv).removeClass("ui-icon-circle-triangle-s").addClass("ui-icon-circle-triangle-n"),this.p.gridstate="visible"))})},filterToolbar:function(b){b=a.extend({autosearch:!0,searchOnEnter:!0,beforeSearch:null,afterSearch:null,beforeClear:null,afterClear:null,
searchurl:"",stringResult:!1,groupOp:"AND",defaultSearch:"bw",searchOperators:!1,resetIcon:"x",operands:{eq:"==",ne:"!",lt:"<",le:"<=",gt:">",ge:">=",bw:"^",bn:"!^","in":"=",ni:"!=",ew:"|",en:"!@",cn:"~",nc:"!~",nu:"#",nn:"!#"}},a.jgrid.search,b||{});return this.each(function(){var c=this;if(!this.ftoolbar){var d=function(){var d={},f=0,g,m,e={},q;a.each(c.p.colModel,function(){var l=a("#gs_"+a.jgrid.jqID(this.name),!0===this.frozen&&!0===c.p.frozenColumns?c.grid.fhDiv:c.grid.hDiv);m=this.index||
this.name;q=b.searchOperators?l.parent().prev().children("a").attr("soper")||b.defaultSearch:this.searchoptions&&this.searchoptions.sopt?this.searchoptions.sopt[0]:"select"===this.stype?"eq":b.defaultSearch;if((g="custom"===this.stype&&a.isFunction(this.searchoptions.custom_value)&&0<l.length&&"SPAN"===l[0].nodeName.toUpperCase()?this.searchoptions.custom_value.call(c,l.children(".customelement:first"),"get"):l.val())||"nu"===q||"nn"===q)d[m]=g,e[m]=q,f++;else try{delete c.p.postData[m]}catch(k){}});
var k=0<f?!0:!1;if(!0===b.stringResult||"local"===c.p.datatype){var l='{"groupOp":"'+b.groupOp+'","rules":[',n=0;a.each(d,function(a,b){0<n&&(l+=",");l+='{"field":"'+a+'",';l+='"op":"'+e[a]+'",';l+='"data":"'+(b+"").replace(/\\/g,"\\\\").replace(/\"/g,'\\"')+'"}';n++});l+="]}";a.extend(c.p.postData,{filters:l});a.each(["searchField","searchString","searchOper"],function(a,b){c.p.postData.hasOwnProperty(b)&&delete c.p.postData[b]})}else a.extend(c.p.postData,d);var r;c.p.searchurl&&(r=c.p.url,a(c).jqGrid("setGridParam",
{url:c.p.searchurl}));var h="stop"===a(c).triggerHandler("jqGridToolbarBeforeSearch")?!0:!1;!h&&a.isFunction(b.beforeSearch)&&(h=b.beforeSearch.call(c));h||a(c).jqGrid("setGridParam",{search:k}).trigger("reloadGrid",[{page:1}]);r&&a(c).jqGrid("setGridParam",{url:r});a(c).triggerHandler("jqGridToolbarAfterSearch");a.isFunction(b.afterSearch)&&b.afterSearch.call(c)},h=function(e,f,g){a("#sopt_menu").remove();f=parseInt(f,10);g=parseInt(g,10)+18;f='<ul id="sopt_menu" class="ui-search-menu" role="menu" tabindex="0" style="font-size:'+
(a(".ui-jqgrid-view").css("font-size")||"11px")+";left:"+f+"px;top:"+g+'px;">';g=a(e).attr("soper");var k,h=[],q,p=0,l=a(e).attr("colname");for(k=c.p.colModel.length;p<k&&c.p.colModel[p].name!==l;)p++;p=c.p.colModel[p];l=a.extend({},p.searchoptions);l.sopt||(l.sopt=[],l.sopt[0]="select"===p.stype?"eq":b.defaultSearch);a.each(b.odata,function(){h.push(this.oper)});for(p=0;p<l.sopt.length;p++)q=a.inArray(l.sopt[p],h),-1!==q&&(k=g===b.odata[q].oper?"ui-state-highlight":"",f+='<li class="ui-menu-item '+
k+'" role="presentation"><a class="ui-corner-all g-menu-item" tabindex="0" role="menuitem" value="'+b.odata[q].oper+'" oper="'+b.operands[b.odata[q].oper]+'"><table cellspacing="0" cellpadding="0" border="0"><tr><td width="25px">'+b.operands[b.odata[q].oper]+"</td><td>"+b.odata[q].text+"</td></tr></table></a></li>");f+="</ul>";a("body").append(f);a("#sopt_menu").addClass("ui-menu ui-widget ui-widget-content ui-corner-all");a("#sopt_menu > li > a").hover(function(){a(this).addClass("ui-state-hover")},
function(){a(this).removeClass("ui-state-hover")}).click(function(f){f=a(this).attr("value");var g=a(this).attr("oper");a(c).triggerHandler("jqGridToolbarSelectOper",[f,g,e]);a("#sopt_menu").hide();a(e).text(g).attr("soper",f);!0===b.autosearch&&(g=a(e).parent().next().children()[0],(a(g).val()||"nu"===f||"nn"===f)&&d())})},k=a("<tr class='ui-search-toolbar' role='rowheader'></tr>"),e;a.each(c.p.colModel,function(h){var f=this,g,m;m="";var x="=",q,p=a("<th role='columnheader' class='ui-state-default ui-th-column ui-th-"+
c.p.direction+"'></th>"),l=a("<div style='position:relative;height:100%;padding-right:0.3em;padding-left:0.3em;'></div>"),n=a("<table class='ui-search-table' cellspacing='0'><tr><td class='ui-search-oper'></td><td class='ui-search-input'></td><td class='ui-search-clear'></td></tr></table>");!0===this.hidden&&a(p).css("display","none");this.search=!1===this.search?!1:!0;void 0===this.stype&&(this.stype="text");g=a.extend({},this.searchoptions||{});if(this.search){if(b.searchOperators){m=g.sopt?g.sopt[0]:
"select"===f.stype?"eq":b.defaultSearch;for(q=0;q<b.odata.length;q++)if(b.odata[q].oper===m){x=b.operands[m]||"";break}m="<a title='"+(null!=g.searchtitle?g.searchtitle:b.operandTitle)+"' style='padding-right: 0.5em;' soper='"+m+"' class='soptclass' colname='"+this.name+"'>"+x+"</a>"}a("td:eq(0)",n).attr("colindex",h).append(m);void 0===g.clearSearch&&(g.clearSearch=!0);g.clearSearch?(m=b.resetTitle||"Clear Search Value",a("td:eq(2)",n).append("<a title='"+m+"' style='padding-right: 0.3em;padding-left: 0.3em;' class='clearsearchclass'>"+
b.resetIcon+"</a>")):a("td:eq(2)",n).hide();switch(this.stype){case "select":if(m=this.surl||g.dataUrl)a(l).append(n),a.ajax(a.extend({url:m,dataType:"html",success:function(e){void 0!==g.buildSelect?(e=g.buildSelect(e))&&a("td:eq(1)",n).append(e):a("td:eq(1)",n).append(e);void 0!==g.defaultValue&&a("select",l).val(g.defaultValue);a("select",l).attr({name:f.index||f.name,id:"gs_"+f.name});g.attr&&a("select",l).attr(g.attr);a("select",l).css({width:"100%"});a.jgrid.bindEv.call(c,a("select",l)[0],g);
!0===b.autosearch&&a("select",l).change(function(){d();return!1});e=null}},a.jgrid.ajaxOptions,c.p.ajaxSelectOptions||{}));else{var r,w,u;f.searchoptions?(r=void 0===f.searchoptions.value?"":f.searchoptions.value,w=void 0===f.searchoptions.separator?":":f.searchoptions.separator,u=void 0===f.searchoptions.delimiter?";":f.searchoptions.delimiter):f.editoptions&&(r=void 0===f.editoptions.value?"":f.editoptions.value,w=void 0===f.editoptions.separator?":":f.editoptions.separator,u=void 0===f.editoptions.delimiter?
";":f.editoptions.delimiter);if(r){var t=document.createElement("select");t.style.width="100%";a(t).attr({name:f.index||f.name,id:"gs_"+f.name});var v;if("string"===typeof r)for(m=r.split(u),v=0;v<m.length;v++)r=m[v].split(w),u=document.createElement("option"),u.value=r[0],u.innerHTML=r[1],t.appendChild(u);else if("object"===typeof r)for(v in r)r.hasOwnProperty(v)&&(u=document.createElement("option"),u.value=v,u.innerHTML=r[v],t.appendChild(u));void 0!==g.defaultValue&&a(t).val(g.defaultValue);g.attr&&
a(t).attr(g.attr);a(l).append(n);a.jgrid.bindEv.call(c,t,g);a("td:eq(1)",n).append(t);!0===b.autosearch&&a(t).change(function(){d();return!1})}}break;case "text":w=void 0!==g.defaultValue?g.defaultValue:"";a("td:eq(1)",n).append("<input type='text' style='width:100%;padding:0px;' name='"+(f.index||f.name)+"' id='gs_"+f.name+"' value='"+w+"'/>");a(l).append(n);g.attr&&a("input",l).attr(g.attr);a.jgrid.bindEv.call(c,a("input",l)[0],g);!0===b.autosearch&&(b.searchOnEnter?a("input",l).keypress(function(a){return 13===
(a.charCode||a.keyCode||0)?(d(),!1):this}):a("input",l).keydown(function(a){switch(a.which){case 13:return!1;case 9:case 16:case 37:case 38:case 39:case 40:case 27:break;default:e&&clearTimeout(e),e=setTimeout(function(){d()},500)}}));break;case "custom":a("td:eq(1)",n).append("<span style='width:95%;padding:0px;' name='"+(f.index||f.name)+"' id='gs_"+f.name+"'/>");a(l).append(n);try{if(a.isFunction(g.custom_element))if(t=g.custom_element.call(c,void 0!==g.defaultValue?g.defaultValue:"",g))t=a(t).addClass("customelement"),
a(l).find(">span").append(t);else throw"e2";else throw"e1";}catch(y){"e1"===y&&a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.nodefined,a.jgrid.edit.bClose),"e2"===y?a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.novalue,a.jgrid.edit.bClose):a.jgrid.info_dialog(a.jgrid.errors.errcap,"string"===typeof y?y:y.message,a.jgrid.edit.bClose)}}}a(p).append(l);a(k).append(p);b.searchOperators||a("td:eq(0)",n).hide()});a("table thead",
c.grid.hDiv).append(k);b.searchOperators&&(a(".soptclass",k).click(function(b){var c=a(this).offset();h(this,c.left,c.top);b.stopPropagation()}),a("body").on("click",function(b){"soptclass"!==b.target.className&&a("#sopt_menu").hide()}));a(".clearsearchclass",k).click(function(e){e=a(this).parents("tr:first");var f=parseInt(a("td.ui-search-oper",e).attr("colindex"),10),g=a.extend({},c.p.colModel[f].searchoptions||{}),g=g.defaultValue?g.defaultValue:"";"select"===c.p.colModel[f].stype?g?a("td.ui-search-input select",
e).val(g):a("td.ui-search-input select",e)[0].selectedIndex=0:a("td.ui-search-input input",e).val(g);!0===b.autosearch&&d()});this.ftoolbar=!0;this.triggerToolbar=d;this.clearToolbar=function(d){var f={},g=0,e;d="boolean"!==typeof d?!0:d;a.each(c.p.colModel,function(){var b,d=a("#gs_"+a.jgrid.jqID(this.name),!0===this.frozen&&!0===c.p.frozenColumns?c.grid.fhDiv:c.grid.hDiv);this.searchoptions&&void 0!==this.searchoptions.defaultValue&&(b=this.searchoptions.defaultValue);e=this.index||this.name;switch(this.stype){case "select":d.find("option").each(function(c){0===
c&&(this.selected=!0);if(a(this).val()===b)return this.selected=!0,!1});if(void 0!==b)f[e]=b,g++;else try{delete c.p.postData[e]}catch(h){}break;case "text":d.val(b||"");if(void 0!==b)f[e]=b,g++;else try{delete c.p.postData[e]}catch(k){}break;case "custom":a.isFunction(this.searchoptions.custom_value)&&0<d.length&&"SPAN"===d[0].nodeName.toUpperCase()&&this.searchoptions.custom_value.call(c,d.children(".customelement:first"),"set",b||"")}});var k=0<g?!0:!1;c.p.resetsearch=!0;if(!0===b.stringResult||
"local"===c.p.datatype){var h='{"groupOp":"'+b.groupOp+'","rules":[',p=0;a.each(f,function(a,b){0<p&&(h+=",");h+='{"field":"'+a+'",';h+='"op":"eq",';h+='"data":"'+(b+"").replace(/\\/g,"\\\\").replace(/\"/g,'\\"')+'"}';p++});h+="]}";a.extend(c.p.postData,{filters:h});a.each(["searchField","searchString","searchOper"],function(a,b){c.p.postData.hasOwnProperty(b)&&delete c.p.postData[b]})}else a.extend(c.p.postData,f);var l;c.p.searchurl&&(l=c.p.url,a(c).jqGrid("setGridParam",{url:c.p.searchurl}));var n=
"stop"===a(c).triggerHandler("jqGridToolbarBeforeClear")?!0:!1;!n&&a.isFunction(b.beforeClear)&&(n=b.beforeClear.call(c));n||d&&a(c).jqGrid("setGridParam",{search:k}).trigger("reloadGrid",[{page:1}]);l&&a(c).jqGrid("setGridParam",{url:l});a(c).triggerHandler("jqGridToolbarAfterClear");a.isFunction(b.afterClear)&&b.afterClear()};this.toggleToolbar=function(){var b=a("tr.ui-search-toolbar",c.grid.hDiv),d=!0===c.p.frozenColumns?a("tr.ui-search-toolbar",c.grid.fhDiv):!1;"none"===b.css("display")?(b.show(),
d&&d.show()):(b.hide(),d&&d.hide())}}})},destroyFilterToolbar:function(){return this.each(function(){this.ftoolbar&&(this.toggleToolbar=this.clearToolbar=this.triggerToolbar=null,this.ftoolbar=!1,a(this.grid.hDiv).find("table thead tr.ui-search-toolbar").remove())})},destroyGroupHeader:function(b){void 0===b&&(b=!0);return this.each(function(){var c,d,h,k,e,s;d=this.grid;var f=a("table.ui-jqgrid-htable thead",d.hDiv),g=this.p.colModel;if(d){a(this).unbind(".setGroupHeaders");c=a("<tr>",{role:"rowheader"}).addClass("ui-jqgrid-labels");
k=d.headers;d=0;for(h=k.length;d<h;d++){e=g[d].hidden?"none":"";e=a(k[d].el).width(k[d].width).css("display",e);try{e.removeAttr("rowSpan")}catch(m){e.attr("rowSpan",1)}c.append(e);s=e.children("span.ui-jqgrid-resize");0<s.length&&(s[0].style.height="");e.children("div")[0].style.top=""}a(f).children("tr.ui-jqgrid-labels").remove();a(f).prepend(c);!0===b&&a(this).jqGrid("setGridParam",{groupHeader:null})}})},setGroupHeaders:function(b){b=a.extend({useColSpanStyle:!1,groupHeaders:[]},b||{});return this.each(function(){this.p.groupHeader=
b;var c,d,h=0,k,e,s,f,g,m=this.p.colModel,x=m.length,q=this.grid.headers,p=a("table.ui-jqgrid-htable",this.grid.hDiv),l=p.children("thead").children("tr.ui-jqgrid-labels:last").addClass("jqg-second-row-header");k=p.children("thead");var n=p.find(".jqg-first-row-header");void 0===n[0]?n=a("<tr>",{role:"row","aria-hidden":"true"}).addClass("jqg-first-row-header").css("height","auto"):n.empty();var r,w=function(a,b){var c=b.length,d;for(d=0;d<c;d++)if(b[d].startColumnName===a)return d;return-1};a(this).prepend(k);
k=a("<tr>",{role:"rowheader"}).addClass("ui-jqgrid-labels jqg-third-row-header");for(c=0;c<x;c++)if(s=q[c].el,f=a(s),d=m[c],e={height:"0px",width:q[c].width+"px",display:d.hidden?"none":""},a("<th>",{role:"gridcell"}).css(e).addClass("ui-first-th-"+this.p.direction).appendTo(n),s.style.width="",e=w(d.name,b.groupHeaders),0<=e){e=b.groupHeaders[e];h=e.numberOfColumns;g=e.titleText;for(e=d=0;e<h&&c+e<x;e++)m[c+e].hidden||d++;e=a("<th>").attr({role:"columnheader"}).addClass("ui-state-default ui-th-column-header ui-th-"+
this.p.direction).css({height:"22px","border-top":"0 none"}).html(g);0<d&&e.attr("colspan",String(d));this.p.headertitles&&e.attr("title",e.text());0===d&&e.hide();f.before(e);k.append(s);h-=1}else 0===h?b.useColSpanStyle?f.attr("rowspan","2"):(a("<th>",{role:"columnheader"}).addClass("ui-state-default ui-th-column-header ui-th-"+this.p.direction).css({display:d.hidden?"none":"","border-top":"0 none"}).insertBefore(f),k.append(s)):(k.append(s),h--);m=a(this).children("thead");m.prepend(n);k.insertAfter(l);
p.append(m);b.useColSpanStyle&&(p.find("span.ui-jqgrid-resize").each(function(){var b=a(this).parent();b.is(":visible")&&(this.style.cssText="height: "+b.height()+"px !important; cursor: col-resize;")}),p.find("div.ui-jqgrid-sortable").each(function(){var b=a(this),c=b.parent();c.is(":visible")&&c.is(":has(span.ui-jqgrid-resize)")&&b.css("top",(c.height()-b.outerHeight())/2+"px")}));r=m.find("tr.jqg-first-row-header");a(this).bind("jqGridResizeStop.setGroupHeaders",function(a,b,c){r.find("th").eq(c).width(b)})})},
setFrozenColumns:function(){return this.each(function(){if(this.grid){var b=this,c=b.p.colModel,d=0,h=c.length,k=-1,e=!1;if(!0!==b.p.subGrid&&!0!==b.p.treeGrid&&!0!==b.p.cellEdit&&!b.p.sortable&&!b.p.scroll){b.p.rownumbers&&d++;for(b.p.multiselect&&d++;d<h;){if(!0===c[d].frozen)e=!0,k=d;else break;d++}if(0<=k&&e){c=b.p.caption?a(b.grid.cDiv).outerHeight():0;d=a(".ui-jqgrid-htable","#gview_"+a.jgrid.jqID(b.p.id)).height();b.p.toppager&&(c+=a(b.grid.topDiv).outerHeight());!0===b.p.toolbar[0]&&"bottom"!==
b.p.toolbar[1]&&(c+=a(b.grid.uDiv).outerHeight());b.grid.fhDiv=a('<div style="position:absolute;left:0px;top:'+c+"px;height:"+d+'px;" class="frozen-div ui-state-default ui-jqgrid-hdiv"></div>');b.grid.fbDiv=a('<div style="position:absolute;left:0px;top:'+(parseInt(c,10)+parseInt(d,10)+1)+'px;overflow-y:hidden" class="frozen-bdiv ui-jqgrid-bdiv"></div>');a("#gview_"+a.jgrid.jqID(b.p.id)).append(b.grid.fhDiv);c=a(".ui-jqgrid-htable","#gview_"+a.jgrid.jqID(b.p.id)).clone(!0);if(b.p.groupHeader){a("tr.jqg-first-row-header, tr.jqg-third-row-header",
c).each(function(){a("th:gt("+k+")",this).remove()});var s=-1,f=-1,g,m;a("tr.jqg-second-row-header th",c).each(function(){g=parseInt(a(this).attr("colspan"),10);if(m=parseInt(a(this).attr("rowspan"),10))s++,f++;g&&(s+=g,f++);if(s===k)return!1});s!==k&&(f=k);a("tr.jqg-second-row-header",c).each(function(){a("th:gt("+f+")",this).remove()})}else a("tr",c).each(function(){a("th:gt("+k+")",this).remove()});a(c).width(1);a(b.grid.fhDiv).append(c).mousemove(function(a){if(b.grid.resizing)return b.grid.dragMove(a),
!1});a(b).bind("jqGridResizeStop.setFrozenColumns",function(c,d,e){c=a(".ui-jqgrid-htable",b.grid.fhDiv);a("th:eq("+e+")",c).width(d);c=a(".ui-jqgrid-btable",b.grid.fbDiv);a("tr:first td:eq("+e+")",c).width(d)});a(b).bind("jqGridSortCol.setFrozenColumns",function(c,d,e){c=a("tr.ui-jqgrid-labels:last th:eq("+b.p.lastsort+")",b.grid.fhDiv);d=a("tr.ui-jqgrid-labels:last th:eq("+e+")",b.grid.fhDiv);a("span.ui-grid-ico-sort",c).addClass("ui-state-disabled");a(c).attr("aria-selected","false");a("span.ui-icon-"+
b.p.sortorder,d).removeClass("ui-state-disabled");a(d).attr("aria-selected","true");b.p.viewsortcols[0]||b.p.lastsort===e||(a("span.s-ico",c).hide(),a("span.s-ico",d).show())});a("#gview_"+a.jgrid.jqID(b.p.id)).append(b.grid.fbDiv);a(b.grid.bDiv).scroll(function(){a(b.grid.fbDiv).scrollTop(a(this).scrollTop())});!0===b.p.hoverrows&&a("#"+a.jgrid.jqID(b.p.id)).unbind("mouseover").unbind("mouseout");a(b).bind("jqGridAfterGridComplete.setFrozenColumns",function(){a("#"+a.jgrid.jqID(b.p.id)+"_frozen").remove();
a(b.grid.fbDiv).height(a(b.grid.bDiv).height()-16);var c=a("#"+a.jgrid.jqID(b.p.id)).clone(!0);a("tr[role=row]",c).each(function(){a("td[role=gridcell]:gt("+k+")",this).remove()});a(c).width(1).attr("id",b.p.id+"_frozen");a(b.grid.fbDiv).append(c);!0===b.p.hoverrows&&(a("tr.jqgrow",c).hover(function(){a(this).addClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+a.jgrid.jqID(b.p.id)).addClass("ui-state-hover")},function(){a(this).removeClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+
a.jgrid.jqID(b.p.id)).removeClass("ui-state-hover")}),a("tr.jqgrow","#"+a.jgrid.jqID(b.p.id)).hover(function(){a(this).addClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+a.jgrid.jqID(b.p.id)+"_frozen").addClass("ui-state-hover")},function(){a(this).removeClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+a.jgrid.jqID(b.p.id)+"_frozen").removeClass("ui-state-hover")}));c=null});b.grid.hDiv.loading||a(b).triggerHandler("jqGridAfterGridComplete");b.p.frozenColumns=!0}}}})},destroyFrozenColumns:function(){return this.each(function(){if(this.grid&&
!0===this.p.frozenColumns){a(this.grid.fhDiv).remove();a(this.grid.fbDiv).remove();this.grid.fhDiv=null;this.grid.fbDiv=null;a(this).unbind(".setFrozenColumns");if(!0===this.p.hoverrows){var b;a("#"+a.jgrid.jqID(this.p.id)).bind("mouseover",function(c){b=a(c.target).closest("tr.jqgrow");"ui-subgrid"!==a(b).attr("class")&&a(b).addClass("ui-state-hover")}).bind("mouseout",function(c){b=a(c.target).closest("tr.jqgrow");a(b).removeClass("ui-state-hover")})}this.p.frozenColumns=!1}})}})})(jQuery);
(function(a){a.extend(a.jgrid,{showModal:function(a){a.w.show()},closeModal:function(a){a.w.hide().attr("aria-hidden","true");a.o&&a.o.remove()},hideModal:function(d,b){b=a.extend({jqm:!0,gb:""},b||{});if(b.onClose){var c=b.gb&&"string"===typeof b.gb&&"#gbox_"===b.gb.substr(0,6)?b.onClose.call(a("#"+b.gb.substr(6))[0],d):b.onClose(d);if("boolean"===typeof c&&!c)return}if(a.fn.jqm&&!0===b.jqm)a(d).attr("aria-hidden","true").jqmHide();else{if(""!==b.gb)try{a(".jqgrid-overlay:first",b.gb).hide()}catch(g){}a(d).hide().attr("aria-hidden",
"true")}},findPos:function(a){var b=0,c=0;if(a.offsetParent){do b+=a.offsetLeft,c+=a.offsetTop;while(a=a.offsetParent)}return[b,c]},createModal:function(d,b,c,g,e,h,f){c=a.extend(!0,{},a.jgrid.jqModal||{},c);var k=document.createElement("div"),l,m=this;f=a.extend({},f||{});l="rtl"===a(c.gbox).attr("dir")?!0:!1;k.className="ui-widget ui-widget-content ui-corner-all ui-jqdialog";k.id=d.themodal;var n=document.createElement("div");n.className="ui-jqdialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix";
n.id=d.modalhead;a(n).append("<span class='ui-jqdialog-title'>"+c.caption+"</span>");var q=a("<a class='ui-jqdialog-titlebar-close ui-corner-all'></a>").hover(function(){q.addClass("ui-state-hover")},function(){q.removeClass("ui-state-hover")}).append("<span class='ui-icon ui-icon-closethick'></span>");a(n).append(q);l?(k.dir="rtl",a(".ui-jqdialog-title",n).css("float","right"),a(".ui-jqdialog-titlebar-close",n).css("left","0.3em")):(k.dir="ltr",a(".ui-jqdialog-title",n).css("float","left"),a(".ui-jqdialog-titlebar-close",
n).css("right","0.3em"));var p=document.createElement("div");a(p).addClass("ui-jqdialog-content ui-widget-content").attr("id",d.modalcontent);a(p).append(b);k.appendChild(p);a(k).prepend(n);!0===h?a("body").append(k):"string"===typeof h?a(h).append(k):a(k).insertBefore(g);a(k).css(f);void 0===c.jqModal&&(c.jqModal=!0);b={};if(a.fn.jqm&&!0===c.jqModal)0===c.left&&0===c.top&&c.overlay&&(f=[],f=a.jgrid.findPos(e),c.left=f[0]+4,c.top=f[1]+4),b.top=c.top+"px",b.left=c.left;else if(0!==c.left||0!==c.top)b.left=
c.left,b.top=c.top+"px";a("a.ui-jqdialog-titlebar-close",n).click(function(){var b=a("#"+a.jgrid.jqID(d.themodal)).data("onClose")||c.onClose,e=a("#"+a.jgrid.jqID(d.themodal)).data("gbox")||c.gbox;m.hideModal("#"+a.jgrid.jqID(d.themodal),{gb:e,jqm:c.jqModal,onClose:b});return!1});0!==c.width&&c.width||(c.width=300);0!==c.height&&c.height||(c.height=200);c.zIndex||(g=a(g).parents("*[role=dialog]").filter(":first").css("z-index"),c.zIndex=g?parseInt(g,10)+2:950);g=0;l&&b.left&&!h&&(g=a(c.gbox).width()-
(isNaN(c.width)?0:parseInt(c.width,10))-8,b.left=parseInt(b.left,10)+parseInt(g,10));b.left&&(b.left+="px");a(k).css(a.extend({width:isNaN(c.width)?"auto":c.width+"px",height:isNaN(c.height)?"auto":c.height+"px",zIndex:c.zIndex,overflow:"hidden"},b)).attr({tabIndex:"-1",role:"dialog","aria-labelledby":d.modalhead,"aria-hidden":"true"});void 0===c.drag&&(c.drag=!0);void 0===c.resize&&(c.resize=!0);if(c.drag)if(a(n).css("cursor","move"),a.fn.jqDrag)a(k).jqDrag(n);else try{a(k).draggable({handle:a("#"+
a.jgrid.jqID(n.id))})}catch(r){}if(c.resize)if(a.fn.jqResize)a(k).append("<div class='jqResize ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se'></div>"),a("#"+a.jgrid.jqID(d.themodal)).jqResize(".jqResize",d.scrollelm?"#"+a.jgrid.jqID(d.scrollelm):!1);else try{a(k).resizable({handles:"se, sw",alsoResize:d.scrollelm?"#"+a.jgrid.jqID(d.scrollelm):!1})}catch(s){}!0===c.closeOnEscape&&a(k).keydown(function(b){27==b.which&&(b=a("#"+a.jgrid.jqID(d.themodal)).data("onClose")||c.onClose,
m.hideModal("#"+a.jgrid.jqID(d.themodal),{gb:c.gbox,jqm:c.jqModal,onClose:b}))})},viewModal:function(d,b){b=a.extend({toTop:!0,overlay:10,modal:!1,overlayClass:"ui-widget-overlay",onShow:a.jgrid.showModal,onHide:a.jgrid.closeModal,gbox:"",jqm:!0,jqM:!0},b||{});if(a.fn.jqm&&!0===b.jqm)b.jqM?a(d).attr("aria-hidden","false").jqm(b).jqmShow():a(d).attr("aria-hidden","false").jqmShow();else{""!==b.gbox&&(a(".jqgrid-overlay:first",b.gbox).show(),a(d).data("gbox",b.gbox));a(d).show().attr("aria-hidden",
"false");try{a(":input:visible",d)[0].focus()}catch(c){}}},info_dialog:function(d,b,c,g){var e={width:290,height:"auto",dataheight:"auto",drag:!0,resize:!1,left:250,top:170,zIndex:1E3,jqModal:!0,modal:!1,closeOnEscape:!0,align:"center",buttonalign:"center",buttons:[]};a.extend(!0,e,a.jgrid.jqModal||{},{caption:"<b>"+d+"</b>"},g||{});var h=e.jqModal,f=this;a.fn.jqm&&!h&&(h=!1);d="";if(0<e.buttons.length)for(g=0;g<e.buttons.length;g++)void 0===e.buttons[g].id&&(e.buttons[g].id="info_button_"+g),d+=
"<a id='"+e.buttons[g].id+"' class='fm-button ui-state-default ui-corner-all'>"+e.buttons[g].text+"</a>";g=isNaN(e.dataheight)?e.dataheight:e.dataheight+"px";b="<div id='info_id'>"+("<div id='infocnt' style='margin:0px;padding-bottom:1em;width:100%;overflow:auto;position:relative;height:"+g+";"+("text-align:"+e.align+";")+"'>"+b+"</div>");b+=c?"<div class='ui-widget-content ui-helper-clearfix' style='text-align:"+e.buttonalign+";padding-bottom:0.8em;padding-top:0.5em;background-image: none;border-width: 1px 0 0 0;'><a id='closedialog' class='fm-button ui-state-default ui-corner-all'>"+
c+"</a>"+d+"</div>":""!==d?"<div class='ui-widget-content ui-helper-clearfix' style='text-align:"+e.buttonalign+";padding-bottom:0.8em;padding-top:0.5em;background-image: none;border-width: 1px 0 0 0;'>"+d+"</div>":"";b+="</div>";try{"false"===a("#info_dialog").attr("aria-hidden")&&a.jgrid.hideModal("#info_dialog",{jqm:h}),a("#info_dialog").remove()}catch(k){}a.jgrid.createModal({themodal:"info_dialog",modalhead:"info_head",modalcontent:"info_content",scrollelm:"infocnt"},b,e,"","",!0);d&&a.each(e.buttons,
function(b){a("#"+a.jgrid.jqID(this.id),"#info_id").bind("click",function(){e.buttons[b].onClick.call(a("#info_dialog"));return!1})});a("#closedialog","#info_id").click(function(){f.hideModal("#info_dialog",{jqm:h,onClose:a("#info_dialog").data("onClose")||e.onClose,gb:a("#info_dialog").data("gbox")||e.gbox});return!1});a(".fm-button","#info_dialog").hover(function(){a(this).addClass("ui-state-hover")},function(){a(this).removeClass("ui-state-hover")});a.isFunction(e.beforeOpen)&&e.beforeOpen();a.jgrid.viewModal("#info_dialog",
{onHide:function(a){a.w.hide().remove();a.o&&a.o.remove()},modal:e.modal,jqm:h});a.isFunction(e.afterOpen)&&e.afterOpen();try{a("#info_dialog").focus()}catch(l){}},bindEv:function(d,b){a.isFunction(b.dataInit)&&b.dataInit.call(this,d,b);b.dataEvents&&a.each(b.dataEvents,function(){void 0!==this.data?a(d).bind(this.type,this.data,this.fn):a(d).bind(this.type,this.fn)})},createEl:function(d,b,c,g,e){function h(b,d,c){var e="dataInit dataEvents dataUrl buildSelect sopt searchhidden defaultValue attr custom_element custom_value".split(" ");
void 0!==c&&a.isArray(c)&&a.merge(e,c);a.each(d,function(d,c){-1===a.inArray(d,e)&&a(b).attr(d,c)});d.hasOwnProperty("id")||a(b).attr("id",a.jgrid.randId())}var f="",k=this;switch(d){case "textarea":f=document.createElement("textarea");g?b.cols||a(f).css({width:"98%"}):b.cols||(b.cols=20);b.rows||(b.rows=2);if("&nbsp;"===c||"&#160;"===c||1===c.length&&160===c.charCodeAt(0))c="";f.value=c;h(f,b);a(f).attr({role:"textbox",multiline:"true"});break;case "checkbox":f=document.createElement("input");f.type=
"checkbox";b.value?(d=b.value.split(":"),c===d[0]&&(f.checked=!0,f.defaultChecked=!0),f.value=d[0],a(f).attr("offval",d[1])):(d=(c+"").toLowerCase(),0>d.search(/(false|f|0|no|n|off|undefined)/i)&&""!==d?(f.checked=!0,f.defaultChecked=!0,f.value=c):f.value="on",a(f).attr("offval","off"));h(f,b,["value"]);a(f).attr("role","checkbox");break;case "select":f=document.createElement("select");f.setAttribute("role","select");g=[];!0===b.multiple?(d=!0,f.multiple="multiple",a(f).attr("aria-multiselectable",
"true")):d=!1;if(void 0!==b.dataUrl){d=b.name?String(b.id).substring(0,String(b.id).length-String(b.name).length-1):String(b.id);var l=b.postData||e.postData;k.p&&k.p.idPrefix&&(d=a.jgrid.stripPref(k.p.idPrefix,d));a.ajax(a.extend({url:a.isFunction(b.dataUrl)?b.dataUrl.call(k,d,c,String(b.name)):b.dataUrl,type:"GET",dataType:"html",data:a.isFunction(l)?l.call(k,d,c,String(b.name)):l,context:{elem:f,options:b,vl:c},success:function(b){var d=[],c=this.elem,e=this.vl,f=a.extend({},this.options),g=!0===
f.multiple;b=a.isFunction(f.buildSelect)?f.buildSelect.call(k,b):b;"string"===typeof b&&(b=a(a.trim(b)).html());b&&(a(c).append(b),h(c,f,l?["postData"]:void 0),void 0===f.size&&(f.size=g?3:1),g?(d=e.split(","),d=a.map(d,function(b){return a.trim(b)})):d[0]=a.trim(e),setTimeout(function(){a("option",c).each(function(b){0===b&&c.multiple&&(this.selected=!1);a(this).attr("role","option");if(-1<a.inArray(a.trim(a(this).text()),d)||-1<a.inArray(a.trim(a(this).val()),d))this.selected="selected"})},0))}},
e||{}))}else if(b.value){var m;void 0===b.size&&(b.size=d?3:1);d&&(g=c.split(","),g=a.map(g,function(b){return a.trim(b)}));"function"===typeof b.value&&(b.value=b.value());var n,q,p=void 0===b.separator?":":b.separator;e=void 0===b.delimiter?";":b.delimiter;if("string"===typeof b.value)for(n=b.value.split(e),m=0;m<n.length;m++)q=n[m].split(p),2<q.length&&(q[1]=a.map(q,function(a,b){if(0<b)return a}).join(p)),e=document.createElement("option"),e.setAttribute("role","option"),e.value=q[0],e.innerHTML=
q[1],f.appendChild(e),d||a.trim(q[0])!==a.trim(c)&&a.trim(q[1])!==a.trim(c)||(e.selected="selected"),d&&(-1<a.inArray(a.trim(q[1]),g)||-1<a.inArray(a.trim(q[0]),g))&&(e.selected="selected");else if("object"===typeof b.value)for(m in p=b.value,p)p.hasOwnProperty(m)&&(e=document.createElement("option"),e.setAttribute("role","option"),e.value=m,e.innerHTML=p[m],f.appendChild(e),d||a.trim(m)!==a.trim(c)&&a.trim(p[m])!==a.trim(c)||(e.selected="selected"),d&&(-1<a.inArray(a.trim(p[m]),g)||-1<a.inArray(a.trim(m),
g))&&(e.selected="selected"));h(f,b,["value"])}break;case "text":case "password":case "button":m="button"===d?"button":"textbox";f=document.createElement("input");f.type=d;f.value=c;h(f,b);"button"!==d&&(g?b.size||a(f).css({width:"98%"}):b.size||(b.size=20));a(f).attr("role",m);break;case "image":case "file":f=document.createElement("input");f.type=d;h(f,b);break;case "custom":f=document.createElement("span");try{if(a.isFunction(b.custom_element))if(p=b.custom_element.call(k,c,b))p=a(p).addClass("customelement").attr({id:b.id,
name:b.name}),a(f).empty().append(p);else throw"e2";else throw"e1";}catch(r){"e1"===r&&a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.nodefined,a.jgrid.edit.bClose),"e2"===r?a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.novalue,a.jgrid.edit.bClose):a.jgrid.info_dialog(a.jgrid.errors.errcap,"string"===typeof r?r:r.message,a.jgrid.edit.bClose)}}return f},checkDate:function(a,b){var c={},g;a=a.toLowerCase();g=-1!==a.indexOf("/")?
"/":-1!==a.indexOf("-")?"-":-1!==a.indexOf(".")?".":"/";a=a.split(g);b=b.split(g);if(3!==b.length)return!1;var e=-1,h,f=g=-1,k;for(k=0;k<a.length;k++)h=isNaN(b[k])?0:parseInt(b[k],10),c[a[k]]=h,h=a[k],-1!==h.indexOf("y")&&(e=k),-1!==h.indexOf("m")&&(f=k),-1!==h.indexOf("d")&&(g=k);h="y"===a[e]||"yyyy"===a[e]?4:"yy"===a[e]?2:-1;k=[0,31,29,31,30,31,30,31,31,30,31,30,31];var l;if(-1===e)return!1;l=c[a[e]].toString();2===h&&1===l.length&&(h=1);if(l.length!==h||0===c[a[e]]&&"00"!==b[e]||-1===f)return!1;
l=c[a[f]].toString();if(1>l.length||1>c[a[f]]||12<c[a[f]]||-1===g)return!1;l=c[a[g]].toString();if(!(h=1>l.length)&&!(h=1>c[a[g]])&&!(h=31<c[a[g]])){if(h=2===c[a[f]])e=c[a[e]],h=c[a[g]]>(0!==e%4||0===e%100&&0!==e%400?28:29);h=h||c[a[g]]>k[c[a[f]]]}return h?!1:!0},isEmpty:function(a){return a.match(/^\s+$/)||""===a?!0:!1},checkTime:function(d){var b=/^(\d{1,2}):(\d{2})([apAP][Mm])?$/;if(!a.jgrid.isEmpty(d))if(d=d.match(b)){if(d[3]){if(1>d[1]||12<d[1])return!1}else if(23<d[1])return!1;if(59<d[2])return!1}else return!1;
return!0},checkValues:function(d,b,c,g){var e,h,f;f=this.p.colModel;if(void 0===c)if("string"===typeof b)for(c=0,g=f.length;c<g;c++){if(f[c].name===b){e=f[c].editrules;b=c;null!=f[c].formoptions&&(h=f[c].formoptions.label);break}}else 0<=b&&(e=f[b].editrules);else e=c,h=void 0===g?"_":g;if(e){h||(h=null!=this.p.colNames?this.p.colNames[b]:f[b].label);if(!0===e.required&&a.jgrid.isEmpty(d))return[!1,h+": "+a.jgrid.edit.msg.required,""];c=!1===e.required?!1:!0;if(!0===e.number&&(!1!==c||!a.jgrid.isEmpty(d))&&
isNaN(d))return[!1,h+": "+a.jgrid.edit.msg.number,""];if(void 0!==e.minValue&&!isNaN(e.minValue)&&parseFloat(d)<parseFloat(e.minValue))return[!1,h+": "+a.jgrid.edit.msg.minValue+" "+e.minValue,""];if(void 0!==e.maxValue&&!isNaN(e.maxValue)&&parseFloat(d)>parseFloat(e.maxValue))return[!1,h+": "+a.jgrid.edit.msg.maxValue+" "+e.maxValue,""];if(!(!0!==e.email||!1===c&&a.jgrid.isEmpty(d)||(g=/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i,
g.test(d))))return[!1,h+": "+a.jgrid.edit.msg.email,""];if(!(!0!==e.integer||!1===c&&a.jgrid.isEmpty(d)||!isNaN(d)&&0===d%1&&-1===d.indexOf(".")))return[!1,h+": "+a.jgrid.edit.msg.integer,""];if(!(!0!==e.date||!1===c&&a.jgrid.isEmpty(d)||(f[b].formatoptions&&f[b].formatoptions.newformat?(f=f[b].formatoptions.newformat,a.jgrid.formatter.date.masks.hasOwnProperty(f)&&(f=a.jgrid.formatter.date.masks[f])):f=f[b].datefmt||"Y-m-d",a.jgrid.checkDate(f,d))))return[!1,h+": "+a.jgrid.edit.msg.date+" - "+f,
""];if(!0===e.time&&!(!1===c&&a.jgrid.isEmpty(d)||a.jgrid.checkTime(d)))return[!1,h+": "+a.jgrid.edit.msg.date+" - hh:mm (am/pm)",""];if(!(!0!==e.url||!1===c&&a.jgrid.isEmpty(d)||(g=/^(((https?)|(ftp)):\/\/([\-\w]+\.)+\w{2,3}(\/[%\-\w]+(\.\w{2,})?)*(([\w\-\.\?\\\/+@&#;`~=%!]*)(\.\w{2,})?)*\/?)/i,g.test(d))))return[!1,h+": "+a.jgrid.edit.msg.url,""];if(!0===e.custom&&(!1!==c||!a.jgrid.isEmpty(d)))return a.isFunction(e.custom_func)?(d=e.custom_func.call(this,d,h,b),a.isArray(d)?d:[!1,a.jgrid.edit.msg.customarray,
""]):[!1,a.jgrid.edit.msg.customfcheck,""]}return[!0,"",""]}})})(jQuery);
(function(b){b.jgrid.extend({setSubGrid:function(){return this.each(function(){var d,c;this.p.subGridOptions=b.extend({plusicon:"ui-icon-plus",minusicon:"ui-icon-minus",openicon:"ui-icon-carat-1-sw",expandOnLoad:!1,delayOnLoad:50,selectOnExpand:!1,selectOnCollapse:!1,reloadOnExpand:!0},this.p.subGridOptions||{});this.p.colNames.unshift("");this.p.colModel.unshift({name:"subgrid",width:b.jgrid.cell_width?this.p.subGridWidth+this.p.cellLayout:this.p.subGridWidth,sortable:!1,resizable:!1,hidedlg:!0,
search:!1,fixed:!0});d=this.p.subGridModel;if(d[0])for(d[0].align=b.extend([],d[0].align||[]),c=0;c<d[0].name.length;c++)d[0].align[c]=d[0].align[c]||"left"})},addSubGridCell:function(b,c){var a="",p,n;this.each(function(){a=this.formatCol(b,c);n=this.p.id;p=this.p.subGridOptions.plusicon});return'<td role="gridcell" aria-describedby="'+n+'_subgrid" class="ui-sgcollapsed sgcollapsed" '+a+"><a style='cursor:pointer;'><span class='ui-icon "+p+"'></span></a></td>"},addSubGrid:function(d,c){return this.each(function(){var a=
this;if(a.grid){var p=function(c,d,h){d=b("<td align='"+a.p.subGridModel[0].align[h]+"'></td>").html(d);b(c).append(d)},n=function(c,d){var h,f,e,g=b("<table cellspacing='0' cellpadding='0' border='0'><tbody></tbody></table>"),k=b("<tr></tr>");for(f=0;f<a.p.subGridModel[0].name.length;f++)h=b("<th class='ui-state-default ui-th-subgrid ui-th-column ui-th-"+a.p.direction+"'></th>"),b(h).html(a.p.subGridModel[0].name[f]),b(h).width(a.p.subGridModel[0].width[f]),b(k).append(h);b(g).append(k);c&&(e=a.p.xmlReader.subgrid,
b(e.root+" "+e.row,c).each(function(){k=b("<tr class='ui-widget-content ui-subtblcell'></tr>");if(!0===e.repeatitems)b(e.cell,this).each(function(a){p(k,b(this).text()||"&#160;",a)});else{var c=a.p.subGridModel[0].mapping||a.p.subGridModel[0].name;if(c)for(f=0;f<c.length;f++)p(k,b(c[f],this).text()||"&#160;",f)}b(g).append(k)}));h=b("table:first",a.grid.bDiv).attr("id")+"_";b("#"+b.jgrid.jqID(h+d)).append(g);a.grid.hDiv.loading=!1;b("#load_"+b.jgrid.jqID(a.p.id)).hide();return!1},r=function(c,d){var h,
f,e,g,k,m=b("<table cellspacing='0' cellpadding='0' border='0'><tbody></tbody></table>"),l=b("<tr></tr>");for(f=0;f<a.p.subGridModel[0].name.length;f++)h=b("<th class='ui-state-default ui-th-subgrid ui-th-column ui-th-"+a.p.direction+"'></th>"),b(h).html(a.p.subGridModel[0].name[f]),b(h).width(a.p.subGridModel[0].width[f]),b(l).append(h);b(m).append(l);if(c&&(g=a.p.jsonReader.subgrid,h=b.jgrid.getAccessor(c,g.root),void 0!==h))for(f=0;f<h.length;f++){e=h[f];l=b("<tr class='ui-widget-content ui-subtblcell'></tr>");
if(!0===g.repeatitems)for(g.cell&&(e=e[g.cell]),k=0;k<e.length;k++)p(l,e[k]||"&#160;",k);else{var n=a.p.subGridModel[0].mapping||a.p.subGridModel[0].name;if(n.length)for(k=0;k<n.length;k++)p(l,e[n[k]]||"&#160;",k)}b(m).append(l)}f=b("table:first",a.grid.bDiv).attr("id")+"_";b("#"+b.jgrid.jqID(f+d)).append(m);a.grid.hDiv.loading=!1;b("#load_"+b.jgrid.jqID(a.p.id)).hide();return!1},v=function(c){var e,d,f,g;e=b(c).attr("id");d={nd_:(new Date).getTime()};d[a.p.prmNames.subgridid]=e;if(!a.p.subGridModel[0])return!1;
if(a.p.subGridModel[0].params)for(g=0;g<a.p.subGridModel[0].params.length;g++)for(f=0;f<a.p.colModel.length;f++)a.p.colModel[f].name===a.p.subGridModel[0].params[g]&&(d[a.p.colModel[f].name]=b("td:eq("+f+")",c).text().replace(/\&#160\;/ig,""));if(!a.grid.hDiv.loading)switch(a.grid.hDiv.loading=!0,b("#load_"+b.jgrid.jqID(a.p.id)).show(),a.p.subgridtype||(a.p.subgridtype=a.p.datatype),b.isFunction(a.p.subgridtype)?a.p.subgridtype.call(a,d):a.p.subgridtype=a.p.subgridtype.toLowerCase(),a.p.subgridtype){case "xml":case "json":b.ajax(b.extend({type:a.p.mtype,
url:a.p.subGridUrl,dataType:a.p.subgridtype,data:b.isFunction(a.p.serializeSubGridData)?a.p.serializeSubGridData.call(a,d):d,complete:function(c){"xml"===a.p.subgridtype?n(c.responseXML,e):r(b.jgrid.parse(c.responseText),e)}},b.jgrid.ajaxOptions,a.p.ajaxSubgridOptions||{}))}return!1},e,m,s,t=0,g,l;b.each(a.p.colModel,function(){!0!==this.hidden&&"rn"!==this.name&&"cb"!==this.name||t++});var u=a.rows.length,q=1;void 0!==c&&0<c&&(q=c,u=c+1);for(;q<u;)b(a.rows[q]).hasClass("jqgrow")&&b(a.rows[q].cells[d]).bind("click",
function(){var c=b(this).parent("tr")[0];l=c.nextSibling;if(b(this).hasClass("sgcollapsed")){m=a.p.id;e=c.id;if(!0===a.p.subGridOptions.reloadOnExpand||!1===a.p.subGridOptions.reloadOnExpand&&!b(l).hasClass("ui-subgrid")){s=1<=d?"<td colspan='"+d+"'>&#160;</td>":"";g=b(a).triggerHandler("jqGridSubGridBeforeExpand",[m+"_"+e,e]);(g=!1===g||"stop"===g?!1:!0)&&b.isFunction(a.p.subGridBeforeExpand)&&(g=a.p.subGridBeforeExpand.call(a,m+"_"+e,e));if(!1===g)return!1;b(c).after("<tr role='row' class='ui-subgrid'>"+
s+"<td class='ui-widget-content subgrid-cell'><span class='ui-icon "+a.p.subGridOptions.openicon+"'></span></td><td colspan='"+parseInt(a.p.colNames.length-1-t,10)+"' class='ui-widget-content subgrid-data'><div id="+m+"_"+e+" class='tablediv'></div></td></tr>");b(a).triggerHandler("jqGridSubGridRowExpanded",[m+"_"+e,e]);b.isFunction(a.p.subGridRowExpanded)?a.p.subGridRowExpanded.call(a,m+"_"+e,e):v(c)}else b(l).show();b(this).html("<a style='cursor:pointer;'><span class='ui-icon "+a.p.subGridOptions.minusicon+
"'></span></a>").removeClass("sgcollapsed").addClass("sgexpanded");a.p.subGridOptions.selectOnExpand&&b(a).jqGrid("setSelection",e)}else if(b(this).hasClass("sgexpanded")){g=b(a).triggerHandler("jqGridSubGridRowColapsed",[m+"_"+e,e]);g=!1===g||"stop"===g?!1:!0;e=c.id;g&&b.isFunction(a.p.subGridRowColapsed)&&(g=a.p.subGridRowColapsed.call(a,m+"_"+e,e));if(!1===g)return!1;!0===a.p.subGridOptions.reloadOnExpand?b(l).remove(".ui-subgrid"):b(l).hasClass("ui-subgrid")&&b(l).hide();b(this).html("<a style='cursor:pointer;'><span class='ui-icon "+
a.p.subGridOptions.plusicon+"'></span></a>").removeClass("sgexpanded").addClass("sgcollapsed");a.p.subGridOptions.selectOnCollapse&&b(a).jqGrid("setSelection",e)}return!1}),q++;!0===a.p.subGridOptions.expandOnLoad&&b(a.rows).filter(".jqgrow").each(function(a,c){b(c.cells[0]).click()});a.subGridXml=function(a,b){n(a,b)};a.subGridJson=function(a,b){r(a,b)}}})},expandSubGridRow:function(d){return this.each(function(){if((this.grid||d)&&!0===this.p.subGrid){var c=b(this).jqGrid("getInd",d,!0);c&&(c=b("td.sgcollapsed",
c)[0])&&b(c).trigger("click")}})},collapseSubGridRow:function(d){return this.each(function(){if((this.grid||d)&&!0===this.p.subGrid){var c=b(this).jqGrid("getInd",d,!0);c&&(c=b("td.sgexpanded",c)[0])&&b(c).trigger("click")}})},toggleSubGridRow:function(d){return this.each(function(){if((this.grid||d)&&!0===this.p.subGrid){var c=b(this).jqGrid("getInd",d,!0);if(c){var a=b("td.sgcollapsed",c)[0];a?b(a).trigger("click"):(a=b("td.sgexpanded",c)[0])&&b(a).trigger("click")}}})}})})(jQuery);
(function(d){d.extend(d.jgrid,{template:function(b){var k=d.makeArray(arguments).slice(1),a,c=k.length;null==b&&(b="");return b.replace(/\{([\w\-]+)(?:\:([\w\.]*)(?:\((.*?)?\))?)?\}/g,function(b,m){if(!isNaN(parseInt(m,10)))return k[parseInt(m,10)];for(a=0;a<c;a++)if(d.isArray(k[a]))for(var e=k[a],f=e.length;f--;)if(m===e[f].nm)return e[f].v})}});d.jgrid.extend({groupingSetup:function(){return this.each(function(){var b,k,a=this.p.colModel,c=this.p.groupingView;if(null===c||"object"!==typeof c&&!d.isFunction(c))this.p.grouping=
!1;else if(c.groupField.length){void 0===c.visibiltyOnNextGrouping&&(c.visibiltyOnNextGrouping=[]);c.lastvalues=[];c._locgr||(c.groups=[]);c.counters=[];for(b=0;b<c.groupField.length;b++)c.groupOrder[b]||(c.groupOrder[b]="asc"),c.groupText[b]||(c.groupText[b]="{0}"),"boolean"!==typeof c.groupColumnShow[b]&&(c.groupColumnShow[b]=!0),"boolean"!==typeof c.groupSummary[b]&&(c.groupSummary[b]=!1),c.groupSummaryPos[b]||(c.groupSummaryPos[b]="footer"),!0===c.groupColumnShow[b]?(c.visibiltyOnNextGrouping[b]=
!0,d(this).jqGrid("showCol",c.groupField[b])):(c.visibiltyOnNextGrouping[b]=d("#"+d.jgrid.jqID(this.p.id+"_"+c.groupField[b])).is(":visible"),d(this).jqGrid("hideCol",c.groupField[b]));c.summary=[];c.hideFirstGroupCol&&(c.formatDisplayField[0]=function(a){return a});b=0;for(k=a.length;b<k;b++)c.hideFirstGroupCol&&!a[b].hidden&&c.groupField[0]===a[b].name&&(a[b].formatter=function(){return""}),a[b].summaryType&&(a[b].summaryDivider?c.summary.push({nm:a[b].name,st:a[b].summaryType,v:"",sd:a[b].summaryDivider,
vd:"",sr:a[b].summaryRound,srt:a[b].summaryRoundType||"round"}):c.summary.push({nm:a[b].name,st:a[b].summaryType,v:"",sr:a[b].summaryRound,srt:a[b].summaryRoundType||"round"}))}else this.p.grouping=!1})},groupingPrepare:function(b,k){this.each(function(){var a=this.p.groupingView,c=this,g,m=a.groupField.length,e,f,h,p=0;for(g=0;g<m;g++)e=a.groupField[g],h=a.displayField[g],f=b[e],h=null==h?null:b[h],null==h&&(h=f),void 0!==f&&(0===k?(a.groups.push({idx:g,dataIndex:e,value:f,displayValue:h,startRow:k,
cnt:1,summary:[]}),a.lastvalues[g]=f,a.counters[g]={cnt:1,pos:a.groups.length-1,summary:d.extend(!0,[],a.summary)}):"object"===typeof f||(d.isArray(a.isInTheSameGroup)&&d.isFunction(a.isInTheSameGroup[g])?a.isInTheSameGroup[g].call(c,a.lastvalues[g],f,g,a):a.lastvalues[g]===f)?1===p?(a.groups.push({idx:g,dataIndex:e,value:f,displayValue:h,startRow:k,cnt:1,summary:[]}),a.lastvalues[g]=f,a.counters[g]={cnt:1,pos:a.groups.length-1,summary:d.extend(!0,[],a.summary)}):(a.counters[g].cnt+=1,a.groups[a.counters[g].pos].cnt=
a.counters[g].cnt):(a.groups.push({idx:g,dataIndex:e,value:f,displayValue:h,startRow:k,cnt:1,summary:[]}),a.lastvalues[g]=f,p=1,a.counters[g]={cnt:1,pos:a.groups.length-1,summary:d.extend(!0,[],a.summary)}),d.each(a.counters[g].summary,function(){d.isFunction(this.st)?this.v=this.st.call(c,this.v,this.nm,b):(this.v=d(c).jqGrid("groupingCalculations.handler",this.st,this.v,this.nm,this.sr,this.srt,b),"avg"===this.st.toLowerCase()&&this.sd&&(this.vd=d(c).jqGrid("groupingCalculations.handler",this.st,
this.vd,this.sd,this.sr,this.srt,b)))}),a.groups[a.counters[g].pos].summary=a.counters[g].summary)});return this},groupingToggle:function(b){this.each(function(){var k=this.p.groupingView,a=b.split("_"),c=parseInt(a[a.length-2],10);a.splice(a.length-2,2);var g=a.join("_"),a=k.minusicon,m=k.plusicon,e=d("#"+d.jgrid.jqID(b)),e=e.length?e[0].nextSibling:null,f=d("#"+d.jgrid.jqID(b)+" span.tree-wrap-"+this.p.direction),h=function(a){a=d.map(a.split(" "),function(a){if(a.substring(0,g.length+1)===g+"_")return parseInt(a.substring(g.length+
1),10)});return 0<a.length?a[0]:void 0},p,r=!1,q=this.p.frozenColumns?this.p.id+"_frozen":!1,n=q?d("#"+d.jgrid.jqID(b),"#"+d.jgrid.jqID(q)):!1,n=n&&n.length?n[0].nextSibling:null;if(f.hasClass(a)){if(k.showSummaryOnHide){if(e)for(;e&&!(d(e).hasClass("jqfoot")&&parseInt(d(e).attr("jqfootlevel"),10)<=c);)d(e).hide(),e=e.nextSibling,q&&(d(n).hide(),n=n.nextSibling)}else if(e)for(;e;){k=h(e.className);if(void 0!==k&&k<=c)break;d(e).hide();e=e.nextSibling;q&&(d(n).hide(),n=n.nextSibling)}f.removeClass(a).addClass(m);
r=!0}else{if(e)for(p=void 0;e;){k=h(e.className);void 0===p&&(p=void 0===k);if(void 0!==k){if(k<=c)break;k===c+1&&(d(e).show().find(">td>span.tree-wrap-"+this.p.direction).removeClass(a).addClass(m),q&&d(n).show().find(">td>span.tree-wrap-"+this.p.direction).removeClass(a).addClass(m))}else p&&(d(e).show(),q&&d(n).show());e=e.nextSibling;q&&(n=n.nextSibling)}f.removeClass(m).addClass(a)}d(this).triggerHandler("jqGridGroupingClickGroup",[b,r]);d.isFunction(this.p.onClickGroup)&&this.p.onClickGroup.call(this,
b,r)});return!1},groupingRender:function(b,k,a,c){return this.each(function(){function g(a,b,c){var d=!1;if(0===b)d=c[a];else{var e=c[a].idx;if(0===e)d=c[a];else for(;0<=a;a--)if(c[a].idx===e-b){d=c[a];break}}return d}function m(a,b,c,f){var h=g(a,b,c),m=e.p.colModel,n,q=h.cnt;a="";var p;for(p=f;p<k;p++){var r="<td "+e.formatCol(p,1,"")+">&#160;</td>",t="{0}";d.each(h.summary,function(){if(this.nm===m[p].name){m[p].summaryTpl&&(t=m[p].summaryTpl);"string"===typeof this.st&&"avg"===this.st.toLowerCase()&&
(this.sd&&this.vd?this.v/=this.vd:this.v&&0<q&&(this.v/=q));try{this.groupCount=h.cnt,this.groupIndex=h.dataIndex,this.groupValue=h.value,n=e.formatter("",this.v,p,this)}catch(a){n=this.v}r="<td "+e.formatCol(p,1,"")+">"+d.jgrid.format(t,n)+"</td>";return!1}});a+=r}return a}var e=this,f=e.p.groupingView,h="",p="",r,q,n=f.groupCollapse?f.plusicon:f.minusicon,t,y=[],z=f.groupField.length,n=n+(" tree-wrap-"+e.p.direction);d.each(e.p.colModel,function(a,b){var c;for(c=0;c<z;c++)if(f.groupField[c]===b.name){y[c]=
a;break}});var x=0,A=d.makeArray(f.groupSummary);A.reverse();d.each(f.groups,function(g,l){if(f._locgr&&!(l.startRow+l.cnt>(a-1)*c&&l.startRow<a*c))return!0;x++;q=e.p.id+"ghead_"+l.idx;r=q+"_"+g;p="<span style='cursor:pointer;' class='ui-icon "+n+"' onclick=\"jQuery('#"+d.jgrid.jqID(e.p.id)+"').jqGrid('groupingToggle','"+r+"');return false;\"></span>";try{d.isArray(f.formatDisplayField)&&d.isFunction(f.formatDisplayField[l.idx])?(l.displayValue=f.formatDisplayField[l.idx].call(e,l.displayValue,l.value,
e.p.colModel[y[l.idx]],l.idx,f),t=l.displayValue):t=e.formatter(r,l.displayValue,y[l.idx],l.value)}catch(C){t=l.displayValue}"header"===f.groupSummaryPos[l.idx]?(h+='<tr id="'+r+'"'+(f.groupCollapse&&0<l.idx?' style="display:none;" ':" ")+'role="row" class= "ui-widget-content jqgroup ui-row-'+e.p.direction+" "+q+'"><td style="padding-left:'+12*l.idx+'px;">'+p+d.jgrid.template(f.groupText[l.idx],t,l.cnt,l.summary)+"</td>",h+=m(g,l.idx-1,f.groups,1),h+="</tr>"):h+='<tr id="'+r+'"'+(f.groupCollapse&&
0<l.idx?' style="display:none;" ':" ")+'role="row" class= "ui-widget-content jqgroup ui-row-'+e.p.direction+" "+q+'"><td style="padding-left:'+12*l.idx+'px;" colspan="'+k+'">'+p+d.jgrid.template(f.groupText[l.idx],t,l.cnt,l.summary)+"</td></tr>";if(z-1===l.idx){var s=f.groups[g+1],v,u=0;v=l.startRow;var B=void 0!==s?f.groups[g+1].startRow:b.length;f._locgr&&(u=(a-1)*c,u>l.startRow&&(v=u));for(;v<B&&b[v-u];v++)h+=b[v-u].join("");if("header"!==f.groupSummaryPos[l.idx]){var w;if(void 0!==s){for(w=0;w<
f.groupField.length&&s.dataIndex!==f.groupField[w];w++);x=f.groupField.length-w}for(s=0;s<x;s++)A[s]&&(u="",f.groupCollapse&&!f.showSummaryOnHide&&(u=' style="display:none;"'),h+="<tr"+u+' jqfootlevel="'+(l.idx-s)+'" role="row" class="ui-widget-content jqfoot ui-row-'+e.p.direction+'">',h+=m(g,s,f.groups,0),h+="</tr>");x=w}}});d("#"+d.jgrid.jqID(e.p.id)+" tbody:first").append(h);h=null})},groupingGroupBy:function(b,k){return this.each(function(){"string"===typeof b&&(b=[b]);var a=this.p.groupingView;
this.p.grouping=!0;void 0===a.visibiltyOnNextGrouping&&(a.visibiltyOnNextGrouping=[]);var c;for(c=0;c<a.groupField.length;c++)!a.groupColumnShow[c]&&a.visibiltyOnNextGrouping[c]&&d(this).jqGrid("showCol",a.groupField[c]);for(c=0;c<b.length;c++)a.visibiltyOnNextGrouping[c]=d("#"+d.jgrid.jqID(this.p.id)+"_"+d.jgrid.jqID(b[c])).is(":visible");this.p.groupingView=d.extend(this.p.groupingView,k||{});a.groupField=b;d(this).trigger("reloadGrid")})},groupingRemove:function(b){return this.each(function(){void 0===
b&&(b=!0);this.p.grouping=!1;if(!0===b){var k=this.p.groupingView,a;for(a=0;a<k.groupField.length;a++)!k.groupColumnShow[a]&&k.visibiltyOnNextGrouping[a]&&d(this).jqGrid("showCol",k.groupField);d("tr.jqgroup, tr.jqfoot","#"+d.jgrid.jqID(this.p.id)+" tbody:first").remove();d("tr.jqgrow:hidden","#"+d.jgrid.jqID(this.p.id)+" tbody:first").show()}else d(this).trigger("reloadGrid")})},groupingCalculations:{handler:function(b,d,a,c,g,m){var e={sum:function(){return parseFloat(d||0)+parseFloat(m[a]||0)},
min:function(){return""===d?parseFloat(m[a]||0):Math.min(parseFloat(d),parseFloat(m[a]||0))},max:function(){return""===d?parseFloat(m[a]||0):Math.max(parseFloat(d),parseFloat(m[a]||0))},count:function(){""===d&&(d=0);return m.hasOwnProperty(a)?d+1:0},avg:function(){return e.sum()}};if(!e[b])throw"jqGrid Grouping No such method: "+b;b=e[b]();null!=c&&("fixed"===g?b=b.toFixed(c):(c=Math.pow(10,c),b=Math.round(b*c)/c));return b}}})})(jQuery);
(function(d){d.jgrid.extend({setTreeNode:function(b,c){return this.each(function(){var a=this;if(a.grid&&a.p.treeGrid)for(var h=a.p.expColInd,e=a.p.treeReader.expanded_field,k=a.p.treeReader.leaf_field,g=a.p.treeReader.level_field,f=a.p.treeReader.icon_field,n=a.p.treeReader.loaded,m,p,q,l;b<c;)l=d.jgrid.stripPref(a.p.idPrefix,a.rows[b].id),l=a.p.data[a.p._index[l]],"nested"!==a.p.treeGridModel||l[k]||(m=parseInt(l[a.p.treeReader.left_field],10),p=parseInt(l[a.p.treeReader.right_field],10),l[k]=p===
m+1?"true":"false",a.rows[b].cells[a.p._treeleafpos].innerHTML=l[k]),m=parseInt(l[g],10),0===a.p.tree_root_level?(q=m+1,p=m):(q=m,p=m-1),q="<div class='tree-wrap tree-wrap-"+a.p.direction+"' style='width:"+18*q+"px;'>",q+="<div style='"+("rtl"===a.p.direction?"right:":"left:")+18*p+"px;' class='ui-icon ",void 0!==l[n]&&(l[n]="true"===l[n]||!0===l[n]?!0:!1),"true"===l[k]||!0===l[k]?(q+=(void 0!==l[f]&&""!==l[f]?l[f]:a.p.treeIcons.leaf)+" tree-leaf treeclick",l[k]=!0,p="leaf"):(l[k]=!1,p=""),l[e]=("true"===
l[e]||!0===l[e]?!0:!1)&&(l[n]||void 0===l[n]),q=!1===l[e]?q+(!0===l[k]?"'":a.p.treeIcons.plus+" tree-plus treeclick'"):q+(!0===l[k]?"'":a.p.treeIcons.minus+" tree-minus treeclick'"),q+="></div></div>",d(a.rows[b].cells[h]).wrapInner("<span class='cell-wrapper"+p+"'></span>").prepend(q),m!==parseInt(a.p.tree_root_level,10)&&((l=(l=d(a).jqGrid("getNodeParent",l))&&l.hasOwnProperty(e)?l[e]:!0)||d(a.rows[b]).css("display","none")),d(a.rows[b].cells[h]).find("div.treeclick").bind("click",function(b){b=
d.jgrid.stripPref(a.p.idPrefix,d(b.target||b.srcElement,a.rows).closest("tr.jqgrow")[0].id);b=a.p._index[b];a.p.data[b][k]||(a.p.data[b][e]?(d(a).jqGrid("collapseRow",a.p.data[b]),d(a).jqGrid("collapseNode",a.p.data[b])):(d(a).jqGrid("expandRow",a.p.data[b]),d(a).jqGrid("expandNode",a.p.data[b])));return!1}),!0===a.p.ExpandColClick&&d(a.rows[b].cells[h]).find("span.cell-wrapper").css("cursor","pointer").bind("click",function(b){b=d.jgrid.stripPref(a.p.idPrefix,d(b.target||b.srcElement,a.rows).closest("tr.jqgrow")[0].id);
var c=a.p._index[b];a.p.data[c][k]||(a.p.data[c][e]?(d(a).jqGrid("collapseRow",a.p.data[c]),d(a).jqGrid("collapseNode",a.p.data[c])):(d(a).jqGrid("expandRow",a.p.data[c]),d(a).jqGrid("expandNode",a.p.data[c])));d(a).jqGrid("setSelection",b);return!1}),b++})},setTreeGrid:function(){return this.each(function(){var b=this,c=0,a,h=!1,e,k,g=[];if(b.p.treeGrid){b.p.treedatatype||d.extend(b.p,{treedatatype:b.p.datatype});b.p.subGrid=!1;b.p.altRows=!1;b.p.pgbuttons=!1;b.p.pginput=!1;b.p.gridview=!0;null===
b.p.rowTotal&&(b.p.rowNum=1E4);b.p.multiselect=!1;b.p.rowList=[];b.p.expColInd=0;a="ui-icon-triangle-1-"+("rtl"===b.p.direction?"w":"e");b.p.treeIcons=d.extend({plus:a,minus:"ui-icon-triangle-1-s",leaf:"ui-icon-radio-off"},b.p.treeIcons||{});"nested"===b.p.treeGridModel?b.p.treeReader=d.extend({level_field:"level",left_field:"lft",right_field:"rgt",leaf_field:"isLeaf",expanded_field:"expanded",loaded:"loaded",icon_field:"icon"},b.p.treeReader):"adjacency"===b.p.treeGridModel&&(b.p.treeReader=d.extend({level_field:"level",
parent_id_field:"parent",leaf_field:"isLeaf",expanded_field:"expanded",loaded:"loaded",icon_field:"icon"},b.p.treeReader));for(e in b.p.colModel)if(b.p.colModel.hasOwnProperty(e))for(k in a=b.p.colModel[e].name,a!==b.p.ExpandColumn||h||(h=!0,b.p.expColInd=c),c++,b.p.treeReader)b.p.treeReader.hasOwnProperty(k)&&b.p.treeReader[k]===a&&g.push(a);d.each(b.p.treeReader,function(a,e){e&&-1===d.inArray(e,g)&&("leaf_field"===a&&(b.p._treeleafpos=c),c++,b.p.colNames.push(e),b.p.colModel.push({name:e,width:1,
hidden:!0,sortable:!1,resizable:!1,hidedlg:!0,editable:!0,search:!1}))})}})},expandRow:function(b){this.each(function(){var c=this;if(c.grid&&c.p.treeGrid){var a=d(c).jqGrid("getNodeChildren",b),h=c.p.treeReader.expanded_field;d(a).each(function(){var a=c.p.idPrefix+d.jgrid.getAccessor(this,c.p.localReader.id);d(d(c).jqGrid("getGridRowById",a)).css("display","");this[h]&&d(c).jqGrid("expandRow",this)})}})},collapseRow:function(b){this.each(function(){var c=this;if(c.grid&&c.p.treeGrid){var a=d(c).jqGrid("getNodeChildren",
b),h=c.p.treeReader.expanded_field;d(a).each(function(){var a=c.p.idPrefix+d.jgrid.getAccessor(this,c.p.localReader.id);d(d(c).jqGrid("getGridRowById",a)).css("display","none");this[h]&&d(c).jqGrid("collapseRow",this)})}})},getRootNodes:function(){var b=[];this.each(function(){var c=this;if(c.grid&&c.p.treeGrid)switch(c.p.treeGridModel){case "nested":var a=c.p.treeReader.level_field;d(c.p.data).each(function(){parseInt(this[a],10)===parseInt(c.p.tree_root_level,10)&&b.push(this)});break;case "adjacency":var h=
c.p.treeReader.parent_id_field;d(c.p.data).each(function(){null!==this[h]&&"null"!==String(this[h]).toLowerCase()||b.push(this)})}});return b},getNodeDepth:function(b){var c=null;this.each(function(){if(this.grid&&this.p.treeGrid)switch(this.p.treeGridModel){case "nested":c=parseInt(b[this.p.treeReader.level_field],10)-parseInt(this.p.tree_root_level,10);break;case "adjacency":c=d(this).jqGrid("getNodeAncestors",b).length}});return c},getNodeParent:function(b){var c=null;this.each(function(){var a=
this;if(a.grid&&a.p.treeGrid)switch(a.p.treeGridModel){case "nested":var h=a.p.treeReader.left_field,e=a.p.treeReader.right_field,k=a.p.treeReader.level_field,g=parseInt(b[h],10),f=parseInt(b[e],10),n=parseInt(b[k],10);d(this.p.data).each(function(){if(parseInt(this[k],10)===n-1&&parseInt(this[h],10)<g&&parseInt(this[e],10)>f)return c=this,!1});break;case "adjacency":var m=a.p.treeReader.parent_id_field,p=a.p.localReader.id;d(this.p.data).each(function(){if(this[p]===d.jgrid.stripPref(a.p.idPrefix,
b[m]))return c=this,!1})}});return c},getNodeChildren:function(b){var c=[];this.each(function(){var a=this;if(a.grid&&a.p.treeGrid)switch(a.p.treeGridModel){case "nested":var h=a.p.treeReader.left_field,e=a.p.treeReader.right_field,k=a.p.treeReader.level_field,g=parseInt(b[h],10),f=parseInt(b[e],10),n=parseInt(b[k],10);d(this.p.data).each(function(){parseInt(this[k],10)===n+1&&parseInt(this[h],10)>g&&parseInt(this[e],10)<f&&c.push(this)});break;case "adjacency":var m=a.p.treeReader.parent_id_field,
p=a.p.localReader.id;d(this.p.data).each(function(){this[m]==d.jgrid.stripPref(a.p.idPrefix,b[p])&&c.push(this)})}});return c},getFullTreeNode:function(b){var c=[];this.each(function(){var a=this,h;if(a.grid&&a.p.treeGrid)switch(a.p.treeGridModel){case "nested":var e=a.p.treeReader.left_field,k=a.p.treeReader.right_field,g=a.p.treeReader.level_field,f=parseInt(b[e],10),n=parseInt(b[k],10),m=parseInt(b[g],10);d(this.p.data).each(function(){parseInt(this[g],10)>=m&&parseInt(this[e],10)>=f&&parseInt(this[e],
10)<=n&&c.push(this)});break;case "adjacency":if(b){c.push(b);var p=a.p.treeReader.parent_id_field,q=a.p.localReader.id;d(this.p.data).each(function(b){h=c.length;for(b=0;b<h;b++)if(d.jgrid.stripPref(a.p.idPrefix,c[b][q])===this[p]){c.push(this);break}})}}});return c},getNodeAncestors:function(b){var c=[];this.each(function(){if(this.grid&&this.p.treeGrid)for(var a=d(this).jqGrid("getNodeParent",b);a;)c.push(a),a=d(this).jqGrid("getNodeParent",a)});return c},isVisibleNode:function(b){var c=!0;this.each(function(){if(this.grid&&
this.p.treeGrid){var a=d(this).jqGrid("getNodeAncestors",b),h=this.p.treeReader.expanded_field;d(a).each(function(){c=c&&this[h];if(!c)return!1})}});return c},isNodeLoaded:function(b){var c;this.each(function(){if(this.grid&&this.p.treeGrid){var a=this.p.treeReader.leaf_field,h=this.p.treeReader.loaded;c=void 0!==b?void 0!==b[h]?b[h]:b[a]||0<d(this).jqGrid("getNodeChildren",b).length?!0:!1:!1}});return c},expandNode:function(b){return this.each(function(){if(this.grid&&this.p.treeGrid){var c=this.p.treeReader.expanded_field,
a=this.p.treeReader.parent_id_field,h=this.p.treeReader.loaded,e=this.p.treeReader.level_field,k=this.p.treeReader.left_field,g=this.p.treeReader.right_field;if(!b[c]){var f=d.jgrid.getAccessor(b,this.p.localReader.id),n=d("#"+this.p.idPrefix+d.jgrid.jqID(f),this.grid.bDiv)[0],m=this.p._index[f];d(this).jqGrid("isNodeLoaded",this.p.data[m])?(b[c]=!0,d("div.treeclick",n).removeClass(this.p.treeIcons.plus+" tree-plus").addClass(this.p.treeIcons.minus+" tree-minus")):this.grid.hDiv.loading||(b[c]=!0,
d("div.treeclick",n).removeClass(this.p.treeIcons.plus+" tree-plus").addClass(this.p.treeIcons.minus+" tree-minus"),this.p.treeANode=n.rowIndex,this.p.datatype=this.p.treedatatype,"nested"===this.p.treeGridModel?d(this).jqGrid("setGridParam",{postData:{nodeid:f,n_left:b[k],n_right:b[g],n_level:b[e]}}):d(this).jqGrid("setGridParam",{postData:{nodeid:f,parentid:b[a],n_level:b[e]}}),d(this).trigger("reloadGrid"),b[h]=!0,"nested"===this.p.treeGridModel?d(this).jqGrid("setGridParam",{postData:{nodeid:"",
n_left:"",n_right:"",n_level:""}}):d(this).jqGrid("setGridParam",{postData:{nodeid:"",parentid:"",n_level:""}}))}}})},collapseNode:function(b){return this.each(function(){if(this.grid&&this.p.treeGrid){var c=this.p.treeReader.expanded_field;b[c]&&(b[c]=!1,c=d.jgrid.getAccessor(b,this.p.localReader.id),c=d("#"+this.p.idPrefix+d.jgrid.jqID(c),this.grid.bDiv)[0],d("div.treeclick",c).removeClass(this.p.treeIcons.minus+" tree-minus").addClass(this.p.treeIcons.plus+" tree-plus"))}})},SortTree:function(b,
c,a,h){return this.each(function(){if(this.grid&&this.p.treeGrid){var e,k,g,f=[],n=this,m;e=d(this).jqGrid("getRootNodes");e=d.jgrid.from(e);e.orderBy(b,c,a,h);m=e.select();e=0;for(k=m.length;e<k;e++)g=m[e],f.push(g),d(this).jqGrid("collectChildrenSortTree",f,g,b,c,a,h);d.each(f,function(a){var b=d.jgrid.getAccessor(this,n.p.localReader.id);d("#"+d.jgrid.jqID(n.p.id)+" tbody tr:eq("+a+")").after(d("tr#"+d.jgrid.jqID(b),n.grid.bDiv))});f=m=e=null}})},collectChildrenSortTree:function(b,c,a,h,e,k){return this.each(function(){if(this.grid&&
this.p.treeGrid){var g,f,n,m;g=d(this).jqGrid("getNodeChildren",c);g=d.jgrid.from(g);g.orderBy(a,h,e,k);m=g.select();g=0;for(f=m.length;g<f;g++)n=m[g],b.push(n),d(this).jqGrid("collectChildrenSortTree",b,n,a,h,e,k)}})},setTreeRow:function(b,c){var a=!1;this.each(function(){this.grid&&this.p.treeGrid&&(a=d(this).jqGrid("setRowData",b,c))});return a},delTreeNode:function(b){return this.each(function(){var c=this.p.localReader.id,a,h=this.p.treeReader.left_field,e=this.p.treeReader.right_field,k,g,f;
if(this.grid&&this.p.treeGrid&&(a=this.p._index[b],void 0!==a)){k=parseInt(this.p.data[a][e],10);g=k-parseInt(this.p.data[a][h],10)+1;var n=d(this).jqGrid("getFullTreeNode",this.p.data[a]);if(0<n.length)for(a=0;a<n.length;a++)d(this).jqGrid("delRowData",n[a][c]);if("nested"===this.p.treeGridModel){c=d.jgrid.from(this.p.data).greater(h,k,{stype:"integer"}).select();if(c.length)for(f in c)c.hasOwnProperty(f)&&(c[f][h]=parseInt(c[f][h],10)-g);c=d.jgrid.from(this.p.data).greater(e,k,{stype:"integer"}).select();
if(c.length)for(f in c)c.hasOwnProperty(f)&&(c[f][e]=parseInt(c[f][e],10)-g)}}})},addChildNode:function(b,c,a,h){var e=this[0];if(a){var k=e.p.treeReader.expanded_field,g=e.p.treeReader.leaf_field,f=e.p.treeReader.level_field,n=e.p.treeReader.parent_id_field,m=e.p.treeReader.left_field,p=e.p.treeReader.right_field,q=e.p.treeReader.loaded,l,u,t,w,s;l=0;var v=c,x;void 0===h&&(h=!1);if(void 0===b||null===b){s=e.p.data.length-1;if(0<=s)for(;0<=s;)l=Math.max(l,parseInt(e.p.data[s][e.p.localReader.id],
10)),s--;b=l+1}var y=d(e).jqGrid("getInd",c);x=!1;void 0===c||null===c||""===c?(v=c=null,l="last",w=e.p.tree_root_level,s=e.p.data.length+1):(l="after",u=e.p._index[c],t=e.p.data[u],c=t[e.p.localReader.id],w=parseInt(t[f],10)+1,s=d(e).jqGrid("getFullTreeNode",t),s.length?(v=s=s[s.length-1][e.p.localReader.id],s=d(e).jqGrid("getInd",v)+1):s=d(e).jqGrid("getInd",c)+1,t[g]&&(x=!0,t[k]=!0,d(e.rows[y]).find("span.cell-wrapperleaf").removeClass("cell-wrapperleaf").addClass("cell-wrapper").end().find("div.tree-leaf").removeClass(e.p.treeIcons.leaf+
" tree-leaf").addClass(e.p.treeIcons.minus+" tree-minus"),e.p.data[u][g]=!1,t[q]=!0));u=s+1;void 0===a[k]&&(a[k]=!1);void 0===a[q]&&(a[q]=!1);a[f]=w;void 0===a[g]&&(a[g]=!0);"adjacency"===e.p.treeGridModel&&(a[n]=c);if("nested"===e.p.treeGridModel){var r;if(null!==c){g=parseInt(t[p],10);f=d.jgrid.from(e.p.data);f=f.greaterOrEquals(p,g,{stype:"integer"});f=f.select();if(f.length)for(r in f)f.hasOwnProperty(r)&&(f[r][m]=f[r][m]>g?parseInt(f[r][m],10)+2:f[r][m],f[r][p]=f[r][p]>=g?parseInt(f[r][p],10)+
2:f[r][p]);a[m]=g;a[p]=g+1}else{g=parseInt(d(e).jqGrid("getCol",p,!1,"max"),10);f=d.jgrid.from(e.p.data).greater(m,g,{stype:"integer"}).select();if(f.length)for(r in f)f.hasOwnProperty(r)&&(f[r][m]=parseInt(f[r][m],10)+2);f=d.jgrid.from(e.p.data).greater(p,g,{stype:"integer"}).select();if(f.length)for(r in f)f.hasOwnProperty(r)&&(f[r][p]=parseInt(f[r][p],10)+2);a[m]=g+1;a[p]=g+2}}if(null===c||d(e).jqGrid("isNodeLoaded",t)||x)d(e).jqGrid("addRowData",b,a,l,v),d(e).jqGrid("setTreeNode",s,u);t&&!t[k]&&
h&&d(e.rows[y]).find("div.treeclick").click()}}})})(jQuery);
(function(d){function I(d,n){var h,e,v=[],r;if(!this||"function"!==typeof d||d instanceof RegExp)throw new TypeError;r=this.length;for(h=0;h<r;h++)if(this.hasOwnProperty(h)&&(e=this[h],d.call(n,e,h,this))){v.push(e);break}return v}d.assocArraySize=function(d){var n=0,h;for(h in d)d.hasOwnProperty(h)&&n++;return n};d.jgrid.extend({pivotSetup:function(q,n){var h=[],e=[],v=[],r=[],b={grouping:!0,groupingView:{groupField:[],groupSummary:[],groupSummaryPos:[]}},f=[],c=d.extend({rowTotals:!1,rowTotalsText:"Total",
colTotals:!1,groupSummary:!0,groupSummaryPos:"header",frozenStaticCols:!1},n||{});this.each(function(){function n(C,c,a){C=I.call(C,c,a);return 0<C.length?C[0]:null}function J(c,a){var d=0,f=!0,h;for(h in c){if(c[h]!=this[d]){f=!1;break}d++;if(d>=this.length)break}f&&(D=a);return f}function E(c,a,f,h){var g=a.length,b,k,e,l;l=d.isArray(f)?f.length:1;r=[];for(e=r.root=0;e<l;e++){var n=[],m;for(b=0;b<g;b++){if(null==f)m=k=d.trim(a[b].member)+"_"+a[b].aggregator;else{m=f[e].replace(/\s+/g,"");try{k=
1===g?m:m+"_"+a[b].aggregator+"_"+b}catch(v){}}var t=h,u=k,x=n,y=k,w=h[k],p=a[b].member,q=c,s=void 0;switch(a[b].aggregator){case "sum":s=parseFloat(w||0)+parseFloat(q[p]||0);break;case "count":if(""===w||null==w)w=0;s=q.hasOwnProperty(p)?w+1:0;break;case "min":s=""===w||null==w?parseFloat(q[p]||0):Math.min(parseFloat(w),parseFloat(q[p]||0));break;case "max":s=""===w||null==w?parseFloat(q[p]||0):Math.max(parseFloat(w),parseFloat(q[p]||0))}t[u]=x[y]=s}r[m]=n}return h}function H(a){var d,b,g,k,e;for(g in a)if(a.hasOwnProperty(g)){if("object"!==
typeof a[g]&&("level"===g&&(void 0===F[a.level]&&(F[a.level]="",0<a.level&&"_r_Totals"!==a.text&&(f[a.level-1]={useColSpanStyle:!1,groupHeaders:[]})),F[a.level]!==a.text&&a.children.length&&"_r_Totals"!==a.text&&0<a.level&&(f[a.level-1].groupHeaders.push({titleText:a.text}),b=f[a.level-1].groupHeaders.length,e=1===b?K:G+(b-1)*z,f[a.level-1].groupHeaders[b-1].startColumnName=h[e].name,f[a.level-1].groupHeaders[b-1].numberOfColumns=h.length-e,G=h.length),F[a.level]=a.text),a.level===l&&"level"===g&&
0<l))if(1<z){b=1;for(d in a.fields)1===b&&f[l-1].groupHeaders.push({startColumnName:d,numberOfColumns:1,titleText:a.text}),b++;f[l-1].groupHeaders[f[l-1].groupHeaders.length-1].numberOfColumns=b-1}else f.splice(l-1,1);null!=a[g]&&"object"===typeof a[g]&&H(a[g]);if("level"===g&&0<a.level)for(d in b=0,a.fields){e={};for(k in c.aggregates[b])if(c.aggregates[b].hasOwnProperty(k))switch(k){case "member":case "label":case "aggregator":break;default:e[k]=c.aggregates[b][k]}1<z?(e.name=d,e.label=c.aggregates[b].label||
d):(e.name=a.text,e.label="_r_Totals"===a.text?c.rowTotalsText:a.text);h.push(e);b++}}}var m,D,a,y=q.length,s,l,z,k,p=0;c.rowTotals&&0<c.yDimension.length&&(c.yDimension.splice(0,0,{dataName:c.yDimension[0].dataName}),c.yDimension[0].converter=function(){return"_r_Totals"});s=d.isArray(c.xDimension)?c.xDimension.length:0;l=c.yDimension.length;z=d.isArray(c.aggregates)?c.aggregates.length:0;if(0===s||0===z)throw"xDimension or aggregates optiona are not set!";var x;for(a=0;a<s;a++)x={name:c.xDimension[a].dataName,
frozen:c.frozenStaticCols},x=d.extend(!0,x,c.xDimension[a]),h.push(x);x=s-1;for(var A={};p<y;){m=q[p];var t=[],u=[];k={};a=0;do t[a]=d.trim(m[c.xDimension[a].dataName]),k[c.xDimension[a].dataName]=t[a],a++;while(a<s);var g=0;D=-1;a=n(e,J,t);if(!a){g=0;if(1<=l){for(g=0;g<l;g++)u[g]=d.trim(m[c.yDimension[g].dataName]),c.yDimension[g].converter&&d.isFunction(c.yDimension[g].converter)&&(u[g]=c.yDimension[g].converter.call(this,u[g],t,u));k=E(m,c.aggregates,u,k)}else 0===l&&(k=E(m,c.aggregates,null,k));
e.push(k)}else if(0<=D){g=0;if(1<=l){for(g=0;g<l;g++)u[g]=d.trim(m[c.yDimension[g].dataName]),c.yDimension[g].converter&&d.isFunction(c.yDimension[g].converter)&&(u[g]=c.yDimension[g].converter.call(this,u[g],t,u));a=E(m,c.aggregates,u,a)}else 0===l&&(a=E(m,c.aggregates,null,a));e[D]=a}m=0;var t=k=null,B;for(B in r){if(0===m)A.children&&void 0!==A.children||(A={text:B,level:0,children:[]}),k=A.children;else{t=null;for(a=0;a<k.length;a++)if(k[a].text===B){t=k[a];break}t?k=t.children:(k.push({children:[],
text:B,level:m,fields:r[B]}),k=k[k.length-1].children)}m++}p++}var F=[],G=h.length,K=G;0<l&&(f[l-1]={useColSpanStyle:!1,groupHeaders:[]});H(A,0);if(c.colTotals)for(p=e.length;p--;)for(a=s;a<h.length;a++)y=h[a].name,v[y]=v[y]?v[y]+parseFloat(e[p][y]||0):parseFloat(e[p][y]||0);if(0<x)for(a=0;a<x;a++)b.groupingView.groupField[a]=h[a].name,b.groupingView.groupSummary[a]=c.groupSummary,b.groupingView.groupSummaryPos[a]=c.groupSummaryPos;else b.grouping=!1;b.sortname=h[x].name;b.groupingView.hideFirstGroupCol=
!0});return{colModel:h,rows:e,groupOptions:b,groupHeaders:f,summary:v}},jqPivot:function(q,n,h,e){return this.each(function(){function v(b){var f=jQuery(r).jqGrid("pivotSetup",b,n),c=0<d.assocArraySize(f.summary)?!0:!1,e=d.jgrid.from(f.rows);for(b=0;b<f.groupOptions.groupingView.groupField.length;b++)e.orderBy(f.groupOptions.groupingView.groupField[b],"a","text","");jQuery(r).jqGrid(d.extend({datastr:d.extend(e.select(),c?{userdata:f.summary}:{}),datatype:"jsonstring",footerrow:c,userDataOnFooter:c,
colModel:f.colModel,viewrecords:!0,sortname:n.xDimension[0].dataName},h||{},f.groupOptions));f=f.groupHeaders;if(f.length)for(b=0;b<f.length;b++)f[b]&&f[b].groupHeaders.length&&jQuery(r).jqGrid("setGroupHeaders",f[b]);n.frozenStaticCols&&jQuery(r).jqGrid("setFrozenColumns")}var r=this;"string"===typeof q?d.ajax(d.extend({url:q,dataType:"json",success:function(b){v(d.jgrid.getAccessor(b,e&&e.reader?e.reader:"rows"))}},e||{})):v(q)})}})})(jQuery);
var xmlJsonClass={xml2json:function(a,b){9===a.nodeType&&(a=a.documentElement);var g=this.removeWhite(a),g=this.toObj(g),g=this.toJson(g,a.nodeName,"\t");return"{\n"+b+(b?g.replace(/\t/g,b):g.replace(/\t|\n/g,""))+"\n}"},json2xml:function(a,b){var g=function(a,b,e){var d="",f,k;if(a instanceof Array)if(0===a.length)d+=e+"<"+b+">__EMPTY_ARRAY_</"+b+">\n";else for(f=0,k=a.length;f<k;f+=1)var n=e+g(a[f],b,e+"\t")+"\n",d=d+n;else if("object"===typeof a){f=!1;d+=e+"<"+b;for(k in a)a.hasOwnProperty(k)&&
("@"===k.charAt(0)?d+=" "+k.substr(1)+'="'+a[k].toString()+'"':f=!0);d+=f?">":"/>";if(f){for(k in a)a.hasOwnProperty(k)&&("#text"===k?d+=a[k]:"#cdata"===k?d+="<![CDATA["+a[k]+"]]\x3e":"@"!==k.charAt(0)&&(d+=g(a[k],k,e+"\t")));d+=("\n"===d.charAt(d.length-1)?e:"")+"</"+b+">"}}else"function"===typeof a?d+=e+"<"+b+"><![CDATA["+a+"]]\x3e</"+b+">":(void 0===a&&(a=""),d='""'===a.toString()||0===a.toString().length?d+(e+"<"+b+">__EMPTY_STRING_</"+b+">"):d+(e+"<"+b+">"+a.toString()+"</"+b+">"));return d},
f="",e;for(e in a)a.hasOwnProperty(e)&&(f+=g(a[e],e,""));return b?f.replace(/\t/g,b):f.replace(/\t|\n/g,"")},toObj:function(a){var b={},g=/function/i;if(1===a.nodeType){if(a.attributes.length){var f;for(f=0;f<a.attributes.length;f+=1)b["@"+a.attributes[f].nodeName]=(a.attributes[f].nodeValue||"").toString()}if(a.firstChild){var e=f=0,h=!1,c;for(c=a.firstChild;c;c=c.nextSibling)1===c.nodeType?h=!0:3===c.nodeType&&c.nodeValue.match(/[^ \f\n\r\t\v]/)?f+=1:4===c.nodeType&&(e+=1);if(h)if(2>f&&2>e)for(this.removeWhite(a),
c=a.firstChild;c;c=c.nextSibling)3===c.nodeType?b["#text"]=this.escape(c.nodeValue):4===c.nodeType?g.test(c.nodeValue)?b[c.nodeName]=[b[c.nodeName],c.nodeValue]:b["#cdata"]=this.escape(c.nodeValue):b[c.nodeName]?b[c.nodeName]instanceof Array?b[c.nodeName][b[c.nodeName].length]=this.toObj(c):b[c.nodeName]=[b[c.nodeName],this.toObj(c)]:b[c.nodeName]=this.toObj(c);else a.attributes.length?b["#text"]=this.escape(this.innerXml(a)):b=this.escape(this.innerXml(a));else if(f)a.attributes.length?b["#text"]=
this.escape(this.innerXml(a)):(b=this.escape(this.innerXml(a)),"__EMPTY_ARRAY_"===b?b="[]":"__EMPTY_STRING_"===b&&(b=""));else if(e)if(1<e)b=this.escape(this.innerXml(a));else for(c=a.firstChild;c;c=c.nextSibling)if(g.test(a.firstChild.nodeValue)){b=a.firstChild.nodeValue;break}else b["#cdata"]=this.escape(c.nodeValue)}a.attributes.length||a.firstChild||(b=null)}else 9===a.nodeType?b=this.toObj(a.documentElement):alert("unhandled node type: "+a.nodeType);return b},toJson:function(a,b,g,f){void 0===
f&&(f=!0);var e=b?'"'+b+'"':"",h="\t",c="\n";f||(c=h="");if("[]"===a)e+=b?":[]":"[]";else if(a instanceof Array){var l,d,m=[];d=0;for(l=a.length;d<l;d+=1)m[d]=this.toJson(a[d],"",g+h,f);e+=(b?":[":"[")+(1<m.length?c+g+h+m.join(","+c+g+h)+c+g:m.join(""))+"]"}else if(null===a)e+=(b&&":")+"null";else if("object"===typeof a){l=[];for(d in a)a.hasOwnProperty(d)&&(l[l.length]=this.toJson(a[d],d,g+h,f));e+=(b?":{":"{")+(1<l.length?c+g+h+l.join(","+c+g+h)+c+g:l.join(""))+"}"}else e="string"===typeof a?e+
((b&&":")+'"'+a.replace(/\\/g,"\\\\").replace(/\"/g,'\\"')+'"'):e+((b&&":")+a.toString());return e},innerXml:function(a){var b="";if("innerHTML"in a)b=a.innerHTML;else{var g=function(a){var b="",h;if(1===a.nodeType){b+="<"+a.nodeName;for(h=0;h<a.attributes.length;h+=1)b+=" "+a.attributes[h].nodeName+'="'+(a.attributes[h].nodeValue||"").toString()+'"';if(a.firstChild){b+=">";for(h=a.firstChild;h;h=h.nextSibling)b+=g(h);b+="</"+a.nodeName+">"}else b+="/>"}else 3===a.nodeType?b+=a.nodeValue:4===a.nodeType&&
(b+="<![CDATA["+a.nodeValue+"]]\x3e");return b};for(a=a.firstChild;a;a=a.nextSibling)b+=g(a)}return b},escape:function(a){return a.replace(/[\\]/g,"\\\\").replace(/[\"]/g,'\\"').replace(/[\n]/g,"\\n").replace(/[\r]/g,"\\r")},removeWhite:function(a){a.normalize();var b;for(b=a.firstChild;b;)if(3===b.nodeType)if(b.nodeValue.match(/[^ \f\n\r\t\v]/))b=b.nextSibling;else{var g=b.nextSibling;a.removeChild(b);b=g}else 1===b.nodeType&&this.removeWhite(b),b=b.nextSibling;return a}};
function tableToGrid(l,m){jQuery(l).each(function(){if(!this.grid){jQuery(this).width("99%");var b=jQuery(this).width(),c=jQuery("tr td:first-child input[type=checkbox]:first",jQuery(this)),a=jQuery("tr td:first-child input[type=radio]:first",jQuery(this)),c=0<c.length,a=!c&&0<a.length,k=c||a,d=[],e=[];jQuery("th",jQuery(this)).each(function(){0===d.length&&k?(d.push({name:"__selection__",index:"__selection__",width:0,hidden:!0}),e.push("__selection__")):(d.push({name:jQuery(this).attr("id")||jQuery.trim(jQuery.jgrid.stripHtml(jQuery(this).html())).split(" ").join("_"),
index:jQuery(this).attr("id")||jQuery.trim(jQuery.jgrid.stripHtml(jQuery(this).html())).split(" ").join("_"),width:jQuery(this).width()||150}),e.push(jQuery(this).html()))});var f=[],g=[],h=[];jQuery("tbody > tr",jQuery(this)).each(function(){var b={},a=0;jQuery("td",jQuery(this)).each(function(){if(0===a&&k){var c=jQuery("input",jQuery(this)),e=c.attr("value");g.push(e||f.length);c.is(":checked")&&h.push(e);b[d[a].name]=c.attr("value")}else b[d[a].name]=jQuery(this).html();a++});0<a&&f.push(b)});
jQuery(this).empty();jQuery(this).addClass("scroll");jQuery(this).jqGrid(jQuery.extend({datatype:"local",width:b,colNames:e,colModel:d,multiselect:c},m||{}));for(b=0;b<f.length;b++)a=null,0<g.length&&(a=g[b])&&a.replace&&(a=encodeURIComponent(a).replace(/[.\-%]/g,"_")),null===a&&(a=b+1),jQuery(this).jqGrid("addRowData",a,f[b]);for(b=0;b<h.length;b++)jQuery(this).jqGrid("setSelection",h[b])}})};

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(0)))

/***/ })

},[264]);
//# sourceMappingURL=customeroutstanding.chunk.js.map