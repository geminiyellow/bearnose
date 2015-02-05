/*global define, require */

define(function(require) {
    'use strict';

    var angular = require('angular');
    var login = angular.module('app.login', []);

    // Controller
    login.controller('LoginController', require('./controllers/LoginController'));

    return login;
});