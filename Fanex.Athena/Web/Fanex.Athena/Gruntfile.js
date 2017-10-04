var webpack = require("webpack");
var webpackDevelopment = require("./www/webpackconfig/build/webpack.dev.conf.js");
var webpackProduction = require("./www/webpackconfig/build/webpack.pro.conf.js");

var reactDevelopment = require("./www/webpackconfig/build/webpack.react.dev.conf.js");
var reactProduction = require("./www/webpackconfig/build/webpack.react.pro.conf.js");

module.exports = function (grunt) {
    grunt.file.defaultEncoding = "utf-8";
    grunt.file.preserveBOM = true;

    var configs = grunt.file.readJSON('GruntFilters.json');
    var jsFilters = configs.jsFilters;

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        karma: {
            unit: {
                configFile: 'karma.conf.js'
                /*port: 9999,
                singleRun: true,
                browsers: ['PhantomJS'],
                logLevel: 'ERROR'*/
            },
            unitdev: {
                configFile: 'karma.conf-dev.js'
            },
            unit2: {
                configFile: 'karma-new.conf.js'
                /*port: 9999,
                singleRun: true,
                browsers: ['PhantomJS'],
                logLevel: 'ERROR'*/
            },
            unit2dev: {
                configFile: 'karma-new-dev.conf.js'
            }
        },
        jshint: {
            options: grunt.file.readJSON('jshint.json'),
            beforeconcat: jsFilters,
            afterconcat: ['dist/output.js']
        },
        webpack: {
            dev: webpackDevelopment,
            pro: webpackProduction,
            both: [webpackDevelopment, webpackProduction],
            reactdev: reactDevelopment,
            reactpro: reactProduction,
            reactboth: [reactDevelopment, reactProduction]
        }
    });

    grunt.loadNpmTasks('grunt-karma');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-webpack');
};