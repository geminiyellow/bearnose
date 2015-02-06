/*global define, console */

define([], function() {
    'use strict';

    var controller = function($scope, $state) {
        
        // **** Properties *****************************************************
        $scope.form = {
            email : '',
            password : ''
        };
        
        // **** Events *********************************************************
        var fnLogin = function() {
            console.log('Login ........................... coooooooooooooooooool');
        };
        
        var fnAuthenticate = function(provider) {
            console.log('Authenticate ........................... coooooooooooooooooool');
        };
        
        $scope.events = {};
        $scope.events.login = fnLogin;
        $scope.events.authenticate = fnAuthenticate;
    };
    controller.$inject = ['$scope', '$state'];

    return controller;
});