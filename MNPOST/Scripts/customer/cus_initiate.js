var app = angular.module('myApp', ['ngRoute', 'ngAnimate', 'ngSanitize', 'ui.bootstrap']);

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
app.controller("createCtrl", function ($scope, $log) {

    $scope.numPages = 12;
    $scope.itemPerPage = 20;
    $scope.totalItems = $scope.numPages * $scope.itemPerPage;
    $scope.currentPage = 1;
    $scope.maxSize = 5;
    $scope.pageChanged = function () {
        $log.log('Page changed to: ' + $scope.currentPage);
    };

});
