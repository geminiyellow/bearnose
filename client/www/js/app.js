/*global define, require */

define(['angular', 'uiRouter', 'config', 'ionicAngular', 'modules/login/login'],
  function(angular, uiRouter) {
    'use strict';

    var inject = ['ionic', 'ui.router', 'app.config', 'app.login'];
    return angular.module('app', inject);
  }
);