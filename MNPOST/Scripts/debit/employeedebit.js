var app = angular.module('myApp', ['ui.bootstrap', 'ui.select2', 'ui.mask']);

app.controller('myCtrl', function ($scope, $http, $interval) {

    $scope.select2Options = {
        width: 'element'
    };

    // phan trang
    $scope.numPages;
    $scope.itemPerPage;
    $scope.totalItems;
    $scope.currentPage = 1;
    $scope.maxSize = 5;

    $scope.pageChanged = function () {
        $scope.GetData();
    };

    $scope.searchInfo = {
        "fromDate": fromDate,
        "toDate": toDate,
        "page": $scope.currentPage,
        "postId": ""
    };

    $scope.GetData = function () {
        $scope.searchInfo.page = $scope.currentPage;
        $scope.searchInfo.postId = $scope.postHandle;

        showLoader(true);
        $http({
            method: "POST",
            url: "/employeedebit/getdocuments",
            data: $scope.searchInfo
        }).then(function mySuccess(response) {
            showLoader(false);

            if (response.data.error === 0) {
                $scope.itemPerPage = response.data.pageSize;
                $scope.totalItems = response.data.toltalSize;
                $scope.numPages = Math.round($scope.totalItems / $scope.itemPerPage);


            } else {
                alert(response.data.msg);
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    };

    $scope.choosePostHandle = function () {
        if ($scope.postHandle === '') {
            alert('Chọn bưu cục nếu không sẽ không thể thao tác');
        } else {

            $scope.GetData();
            hideModel('choosePostOfficeModal');
        }
    };
    $scope.init = function () {
        $scope.postOffices = postOfficeDatas;

        $scope.postHandle = '';

        if ($scope.postOffices.length === 1) {
            $scope.postHandle = $scope.postOffices[0];
            $scope.GetData();
        } else {
            showModelFix('choosePostOfficeModal');
        }

    };
});

