/*global requirejs, document, cordova, window, navigator, console */

requirejs.config({
    paths: {
        angular: '../../lib/angular/angular.min',
        angularAnimate: '../../lib/angular-animate/angular-animate.min',
        angularSanitize: '../../lib/angular-sanitize/angular-sanitize.min',
        uiRouter: '../../lib/angular-ui-router/release/angular-ui-router.min',
        ionic: '../../lib/ionic/js/ionic.min',
        ionicAngular: '../../lib/ionic/js/ionic-angular.min',
        angularAria : '../../lib/angular-aria/angular-aria',
        angularMaterial : '../../lib/angular-material/angular-material'
    },
    shim: {
        angular: { exports: 'angular' },
        angularAnimate: { deps: ['angular'] },
        angularSanitize: { deps: ['angular'] },
        uiRouter: { deps: ['angular'] },
        ionic: { deps: ['angular'], exports: 'ionic' },
        ionicAngular: { deps: ['angular', 'ionic', 'uiRouter', 'angularAnimate', 'angularSanitize'] },
        angularAria : { deps: ['angular'] },
        angularMaterial: { deps: ['angular'] }
    },
    priority: [
        'angular',
        'ionic'
    ],
    deps: [
        'bootstrap'
    ]
});