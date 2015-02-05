/*global define, require */

define(['app'], function(app) {
    'use strict';

    app.config(['$stateProvider', '$urlRouterProvider',
        function($stateProvider, $urlRouterProvider) {

            $stateProvider
                .state('login', {
                    url: "/login",
                    templateUrl: "js/modules/login/views/login.html",
                    controller: 'LoginController'
                });

            $urlRouterProvider.otherwise("/login");
        }
    ]);
});