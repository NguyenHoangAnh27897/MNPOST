var app = angular.module('myApp', ['ui.bootstrap', 'myKeyPress']);

app.controller('myCtrl', function ($scope, $http, $rootScope) {

    $scope.numPages;
    $scope.itemPerPage;
    $scope.totalItems;
    $scope.currentPage = 1;
    $scope.maxSize = 5;
    $scope.showEdit = false;
    $scope.checkMailers = false;

    $scope.pageChanged = function () {
        $scope.GetData();
    };

    $scope.searchInfo = {
        "search": "",
        "fromDate": fromDate,
        "toDate": toDate,
        "status": -1,
        "page": $scope.currentPage,
        "postId": ""
    };

    $scope.mailers = [];
    $scope.GetData = function () {
        $scope.searchInfo.page = $scope.currentPage;
        $scope.searchInfo.postId = $scope.postHandle;
        $scope.searchInfo.fromDate = $('#fromDate').val();
        $scope.searchInfo.toDate = $('#toDate').val();

        showLoader(true);
        $http({
            method: "POST",
            url: "/mailer/getmailers",
            data: $scope.searchInfo
        }).then(function mySuccess(response) {
            showLoader(false);

            if (response.data.error === 0) {
                $scope.itemPerPage = response.data.pageSize;
                $scope.totalItems = response.data.toltalSize;
                $scope.numPages = Math.round($scope.totalItems / $scope.itemPerPage);

                $scope.mailers = response.data.data;
                console.log($scope.mailers);
            } else {
                alert(response.data.msg);
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    }

    $scope.checkAll = function () {
        for (var i = 0; i < $scope.mailers.length; i++) {
            $scope.mailers[i].isCheck = $scope.checkMailers;
        }
    };
    $scope.reportUrl = '#';
    $scope.printMailers = function () {

        var listMailers = '';
        for (var i = 0; i < $scope.mailers.length; i++) {
            if ($scope.mailers[i].isCheck) {
                listMailers = listMailers + ',' + $scope.mailers[i].MailerID;
            }
        }

        if (listMailers.charAt(0) === ',') {
            listMailers = listMailers.substr(1);
        }


        if (listMailers === '') {
            showNotify("Phải chọn vận đơn để in");
        } else {
            window.open("/Report/Viewer/MailerPrint.aspx?mailer=" + listMailers, "_blank");
        }
        
    };

    $scope.tracks = [];

    $scope.getTracking = function () {
        showLoader(true);
        var mailerId = $scope.mailer.MailerID;
        $http.get('/mailer/GetTracking?mailerId=' + mailerId).then(function (response) {
            showLoader(false);
            var result = response.data;
            if (result.error === 0) {
                $scope.tracks = result.data;
                showModel('showtracking');
            } else {
                showNotify("Không lấy được")
            }

        });

    };


    $scope.status = angular.copy(mailerStatusData);
    $scope.status.unshift({ "code": -1, "name": "TẤT CẢ" });
    $scope.findStatus = function (code) {
        for (var i = 0; i < $scope.status.length; i++) {
            if ($scope.status[i].code === code)
                return $scope.status[i].name;
        }

    };
    $scope.init = function () {
        $scope.postOffices = postOfficeDatas;

        $scope.postHandle = '';

        if ($scope.postOffices.length === 1) {
            $scope.postHandle = $scope.postOffices[0];
        } else {
            showModelFix('choosePostOfficeModal');
        }

    };


    $scope.choosePostHandle = function () {
        if ($scope.postHandle === '') {
            alert('Chọn bưu cục nếu không sẽ không thể thao tác');
        } else {
            $scope.getCustomerData();
            hideModel('choosePostOfficeModal');
        }
    };

    //
    $scope.mailer = {};
    $scope.showMailerDetail = function (idx) {
        $scope.showEdit = true;
        $('.nav-tabs a[href="#tab_chitiet"]').tab('show');
        $scope.mailer = $scope.mailers[idx];
    };

    $scope.init();
});