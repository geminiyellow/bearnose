/*global define, require */

define(function(require) {
    'use strict';

    var angular = require('angular');
    var signup = angular.module('app.signup', []);

    // **** Route **************************************************************
    var config = function($stateProvider) {
        $stateProvider
            .state('signup', {
                url: "/signup",
                templateUrl: "js/modules/signup/views/signup.html",
                controller: 'SignUpController'
            });
    };
    config.$inject = ['$stateProvider'];
    signup.config(config);

    // **** Controller *********************************************************
    signup.controller('SignUpController', require('./controllers/SignUpController'));

    // **** Service ************************************************************
    
    // ****       **************************************************************
    return signup;
});