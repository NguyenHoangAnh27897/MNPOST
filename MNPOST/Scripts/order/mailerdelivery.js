﻿var app = angular.module('myApp', ['ui.bootstrap', 'myKeyPress']);
app.controller('myCtrl', function ($scope, $http, $rootScope) {

    // phan trang
    $scope.numPages;
    $scope.itemPerPage;
    $scope.totalItems;
    $scope.currentPage = 1;
    $scope.maxSize = 5;
    $scope.showEdit = false;

    $scope.optionSeaches = [
        {
            "code": "deliverycode",
            "name": "Mã bảng kê"
        },
        {
            "code": "mailer",
            "name": "Mã vận đơn"
        },
        {
            "code": "employee",
            "name": "Nhân viên"
        },
        {
            "code": "licenseplate",
            "name": "Số xe"
        }
    ];

    $scope.pageChanged = function () {
        $scope.GetData();
    };

    $scope.searchInfo = {
        "search": "",
        "fromDate": fromDate,
        "toDate": toDate,
        "optionSeach": "deliverycode",
        "page": $scope.currentPage,
        "postId": ""
    };

    $scope.allDeliveries = [];

    $scope.GetData = function () {
        $scope.searchInfo.page = $scope.currentPage;
        $scope.searchInfo.postId = $scope.postHandle;
        $scope.searchInfo.fromDate = $('#fromDate').val();
        $scope.searchInfo.toDate = $('#toDate').val();
        showLoader(true);
        $http({
            method: "POST",
            url: "/mailerdelivery/getmailerdelivery",
            data: $scope.searchInfo
        }).then(function mySuccess(response) {
            showLoader(false);

            if (response.data.error === 0) {
                $scope.itemPerPage = response.data.pageSize;
                $scope.totalItems = response.data.toltalSize;
                $scope.numPages = Math.round($scope.totalItems / $scope.itemPerPage);

                $scope.allDeliveries = response.data.data;
                console.log($scope.allDeliveries);
            } else {
                alert(response.data.msg);
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    }

    $scope.postOffices = postOfficeDatas;

    $scope.postHandle = '';

    $scope.title = '';
    //nhan vien
    $scope.employees = [];
    $scope.licensePlates = [];
    $scope.getFirstData = function () {


        $http.get("/mailerdelivery/GetDataHandle?postId=" + $scope.postHandle).then(function (response) {

            $scope.employees = angular.copy(response.data.employees);
            $scope.licensePlates = angular.copy(response.data.licensePlates);

            console.log($scope.employees);

        });

    };

    $scope.choosePostHandle = function () {
        if ($scope.postHandle === '') {
            alert('Chọn bưu cục nếu không sẽ không thể thao tác');
        } else {

            $scope.GetData();
            $scope.getFirstData();
            hideModel('choosePostOfficeModal');
        }
    };


    // xư ly bang ke
    $scope.createDelivery = function () {
        $('.selectpicker').selectpicker('refresh');
        showModel('createDelivery');

    };

    $scope.finishCreateDelivery = function () {
        var employeeId = $('#chooseEmployeeSelect').val();
        var licensePlateCode = $('#licenseplateChoose').val();
        var notes = $('#deliveryNotes').val();
        var date = $('#deliveryDate').val();
        showLoader(true);
        $http({
            method: "POST",
            url: "/mailerdelivery/create",
            data: {
                employeeId: employeeId,
                deliveryDate: date,
                licensePlate: licensePlateCode,
                notes: notes,
                postId: $scope.postHandle
            }
        }).then(function sucess(response) {
            showLoader(false);
            if (response.data.error === 0) {
                $scope.allDeliveries.unshift(response.data.data);
                hideModel('createDelivery');
            } else {
                alert(response.data.msg);
            }


        }, function error(response) {
            showLoader(false);
            alert("connect error");
        });

    };


    // xu ly chi tiet
    $scope.currentDocument = {};
    $scope.mailers = [];
    $scope.mailerId = '';
    $scope.showDocumentDetail = function (idx) {
        $scope.mailers = [];
        $scope.showEdit = true;
        $scope.currentDocument = angular.copy($scope.allDeliveries[idx]);

        $('.nav-tabs a[href="#tab_chitiet"]').tab('show');

        $http.get("/mailerdelivery/GetDeliveryMailerDetail?documentID=" + $scope.currentDocument.DocumentID).then(
            function (response) {
                console.log(response.data);
                $scope.mailers = anglar.copy(response.data);
            }
        );
    };

    $scope.addMailer = function (valid) {
        if ($scope.currentDocument.DocumentID === '') {
            alert("Không thể thêm");
        } else {
            $http({
                method: "POST",
                url: "/mailerdelivery/AddMailer",
                data: {
                    documentId: $scope.currentDocument.DocumentID,
                    mailerId: $scope.mailerId
                }
            }).then(function sucess(response) {
                    
                }, function error() {

                });
        }
    };
});