/*global define, require */

define(['angular', 'uiRouter', 'ionicAngular', './config', '../modules/login/login', '../modules/signup/signup'],
  function(angular, uiRouter) {
    'use strict';

    var inject = ['ionic', 'ui.router', 'app.config', 'app.login', 'app.signup'];
    var bearnose = angular.module('bearnose', inject);

    // **** Route **************************************************************
    var config = function($urlRouterProvider) {
      $urlRouterProvider.otherwise("/login");
    };
    config.$inject = ['$urlRouterProvider'];
    bearnose.config(config);

    // ****       **************************************************************
    return bearnose;
  }
);