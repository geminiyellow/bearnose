/*global define, require */

define(function(require) {
    'use strict';

    var angular = require('angular');
    var login = angular.module('app.login', []);

    // **** Route **************************************************************
    var config = function($stateProvider) {
        $stateProvider
            .state('login', {
                url: "/login",
                templateUrl: "js/modules/login/views/login.html",
                controller: 'LoginController'
            });
    };
    config.$inject = ['$stateProvider'];
    login.config(config);

    // **** Controller *********************************************************
    login.controller('LoginController', require('./controllers/LoginController'));

    // **** Service ************************************************************
    
    // ****       **************************************************************
    return login;
});