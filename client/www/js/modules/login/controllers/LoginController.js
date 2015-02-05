/*global define, console */

define([], function() {
    'use strict';

    var controller = function($scope, $state) {
        $scope.login = function () {
            console.log('........................... coooooooooooooooooool');
        };
    };
    controller.$inject = ['$scope', '$state'];

    return controller;
});