/*global define, require */

define(['angular', 'uiRouter', 'angularMessage',
    'ionicAngular', 'angularAria', 'angularMaterial',
    './config', '../modules/login/login', '../modules/signup/signup'
  ],
  function(angular, uiRouter) {
    'use strict';

    var inject = ['ionic', 'ngMessages', 'ui.router', 'ngMaterial', 'app.config', 'app.login', 'app.signup'];
    var bearnose = angular.module('bearnose', inject);

    // **** Config *************************************************************
    var config = function($urlRouterProvider, $mdThemingProvider) {
      // ---- Route ------------------------------------------------------------
      $urlRouterProvider.otherwise("/login");

      // ---- Theme ------------------------------------------------------------
      // $mdThemingProvider.theme('default').primaryPalette('blue').accentPalette('orange').warnPalette('red').backgroundPalette('grey');
    };
    config.$inject = ['$urlRouterProvider', '$mdThemingProvider'];
    bearnose.config(config);

    // ****       **************************************************************
    return bearnose;
  }
);