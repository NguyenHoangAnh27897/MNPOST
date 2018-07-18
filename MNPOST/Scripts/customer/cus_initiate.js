var app = angular.module('myApp', ['ngRoute']);

app.config(function ($routeProvider) {
    $routeProvider.
    when('/', {
        templateUrl: '/CustomerInitiate/Show',
        controller: 'showCtrl'
    }).
    when('/create', {
        templateUrl: '/CustomerInitiate/Create',
        controller: 'createCtrl'
    })
})
//--------------------------- show controller
app.controller("showCtrl", function ($scope) {



});
//--------------------------- create controller
app.controller("createCtrl", function ($scope) {



});
