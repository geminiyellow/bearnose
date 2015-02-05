/*global define, console */

define([], function() {
    'use strict';

    var controller = function($scope, $state) {
        $scope.name = 'Sign Up';
    };
    controller.$inject = ['$scope', '$state'];

    return controller;
});