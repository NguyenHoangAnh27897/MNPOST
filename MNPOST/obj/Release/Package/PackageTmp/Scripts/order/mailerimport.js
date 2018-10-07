var app = angular.module('myApp', ['ui.bootstrap']);

app.controller('myCtrl', function ($scope, $http, $rootScope, $interval) {

    $scope.mailers = [];

    $scope.customers = angular.copy(customerDatas);
    $scope.customers.unshift({
        name: 'Tất cả',
        code: ''
    });

    $scope.customerSearch = { SenderID: '' };

    $scope.sendTakeInfo = {};

    $scope.checkMailers = false;

    $scope.mailerId = '';

    $scope.checkAll = function () {
        for (var i = 0; i < $scope.mailers.length; i++) {

            if ($scope.customerSearch.SenderID === '') {
                $scope.mailers[i].isCheck = $scope.checkMailers;
            } else {

                if ($scope.mailers[i].SenderID === $scope.customerSearch.SenderID) {
                    $scope.mailers[i].isCheck = $scope.checkMailers;
                } else {
                    if ($scope.checkMailers) {
                        $scope.mailers[i].isCheck = !$scope.checkMailers;
                    }
                }
            }

        }
    };
    $scope.removeCheck = function () {
        $scope.checkMailers = false;
        for (var i = 0; i < $scope.mailers.length; i++) {

            $scope.mailers[i].isCheck = $scope.checkMailers;

        }
    };

    function getSelectedIndex(mailerId) {
        for (var i = 0; i < $scope.mailers.length; i++)
            if ($scope.mailers[i].MailerID === mailerId)
                return i;
        return -1;
    };

    $scope.getData = function () {
        var url = '/mailerimport/getdata?postId=' + $scope.postHandle;
        $http.get(url).then(function (response) {

            $scope.checkMailers = false;
            $scope.mailers = angular.copy(response.data.data);

        });

    };



    $scope.sendImport = function () {


        var listSends = [];
        for (i = 0; i < $scope.mailers.length; i++) {
            if ($scope.mailers[i].isCheck) {
                listSends.push($scope.mailers[i].MailerID);
            }
        }

        if (listSends.length > 0) {

            showLoader(true);

            $http({
                method: 'POST',
                url: '/mailerimport/addMailers',
                data: {
                    mailers: listSends,
                    postId: $scope.postHandle
                }
            }).then(function sucess(response) {
                showLoader(false);
                for (i = 0; i < response.data.data.length; i++) {
                    var findIndex = getSelectedIndex(listSends[i]);

                    $scope.mailers.shift(findIndex);

                }
                showNotify("Đã nhập kho");
            }, function error(response, error) {
                showNotify("connect has disconnect");
                showLoader(false);

            });

        }



    };

    $scope.addMailerImport = function (isValid) {

        if (isValid) {
            var findIndex = getSelectedIndex($scope.mailerId);

            if (findIndex === -1) {
                $scope.mailerId = '';
                showNotify("Mã này không có trong danh sách");
            } else {
                var listSends = [];
                listSends.push($scope.mailerId);
                $http({
                    method: 'POST',
                    url: '/mailerimport/addMailers',
                    data: {
                        mailers: listSends,
                        postId: $scope.postHandle
                    }
                }).then(function sucess(response) {
                    $scope.mailers.shift(findIndex);
                    $scope.mailerId = '';
                }, function error(response, error) {
                    showNotify("connect has disconnect");
                });

            }



        }

    };
    $scope.status = angular.copy(mailerStatusData);
    $scope.init = function () {
        $scope.postOffices = postOfficeDatas;

        $scope.postHandle = '';

        if ($scope.postOffices.length === 1) {
            $scope.postHandle = $scope.postOffices[0];
            $scope.getData();
            $scope.sendGetEmployees();
            $scope.sendGetTakeMailers();
            $interval(function () { $scope.getData(); }, 1000*60);
        } else {
            showModelFix('choosePostOfficeModal');
        }

    };
    $scope.sendGetEmployees = function () {

        $http.get("/MailerImport/GetEmployee?postId=" + $scope.postHandle).then(function (response) {

            $scope.employees = response.data;
        });

    };

    $scope.sendGetTakeMailers = function () {

        $http.get("/MailerImport/GetTakeMailers?postId=" + $scope.postHandle).then(function (response) {

            $scope.takeMailerDatas = response.data;
        });

    };
    $scope.choosePostHandle = function () {
        if ($scope.postHandle === '') {
            alert('Chọn bưu cục nếu không sẽ không thể thao tác');
        } else {

            $scope.sendGetEmployees();
            $scope.getData();
            $scope.sendGetTakeMailers();
            $interval(function () { $scope.getData(); }, 1000 * 60);
            hideModel('choosePostOfficeModal');
        }
    };

    $scope.init();




    function findCustomerIndex(code) {
        for (i = 0; i < $scope.customers.length; i++) {
            if ($scope.customers[i].code === code) {
                return i;
            }
        }

        return -1;
    }

    $scope.takeMailers = function () {

        if ($scope.customerSearch.SenderID === '') {
            showNotify('Chỉ giao đi lấy cho khách hàng cố định');
        } else {

            var listSends = [];
            for (i = 0; i < $scope.mailers.length; i++) {
                if ($scope.mailers[i].isCheck) {
                    listSends.push($scope.mailers[i].MailerID);
                }
            }

            if (listSends.length == 0) {
                showNotify('Chọn các đơn hàng đi lấy');
            } else {

                var idx = findCustomerIndex($scope.customerSearch.SenderID);

                $scope.sendTakeInfo = { code: $scope.customers[idx].code, name: $scope.customers[idx].name, address: $scope.customers[idx].address, phone: $scope.customers[idx].phone };
                showModel('takemailers');
            }


        }



    };

    $scope.sendTake = function (valid) {

        if (valid) {
            var listSends = [];
            for (i = 0; i < $scope.mailers.length; i++) {
                if ($scope.mailers[i].isCheck) {
                    listSends.push($scope.mailers[i].MailerID);
                }
            }
            $http({

                method: 'POST',
                url: '/MailerImport/CreateTakeMailer',
                data: {

                    cusCode: $scope.sendTakeInfo.code,
                    cusName: $scope.sendTakeInfo.name,
                    cusAddress: $scope.sendTakeInfo.address,
                    cusPhone: $scope.sendTakeInfo.phone,
                    content: $scope.sendTakeInfo.content,
                    employeeId: $scope.sendTakeInfo.employeeId,
                    mailers: listSends,
                    postId: $scope.postHandle
                }
            }).then(function success(response) {

                hideModel('takemailers');
                showNotify("Đã tạo xong");
                $scope.getData();
                $scope.sendGetTakeMailers();
                $('.nav-tabs a[href="#tab_layhang"]').tab('show');


            }, function error(response) {
                showNotify("disconect internet")
            });

        } else {

            showNotify('missing form');
        }

    };

    $scope.checkMailerTake = false;
    $scope.checkTakeAll = function () {
        for (var i = 0; i < $scope.takeDetailDatas.length; i++) {
            $scope.takeDetailDatas[i].isCheck = $scope.checkMailerTake;
        }
    };

    $scope.showTakeDetail = function (documentID, idx) {
        $('.nav-tabs a[href="#tab_chitiet"]').tab('show');

        $scope.takeInfo = $scope.takeMailerDatas[idx];

        $http.get('/mailerimport/showtakedetail?documentID=' + documentID).then(function (response) {
             $scope.updateTake = true;
            $scope.takeDetailDatas = response.data;

        });


    };


    $scope.sendUpdateTake= function () {

        var listSends = [];
        for (i = 0; i < $scope.takeDetailDatas.length; i++) {
            if ($scope.takeDetailDatas[i].isCheck) {
                listSends.push($scope.takeDetailDatas[i].MailerID);
            }
        }


        if (listSends.length === 0) {
            showNotify('Chọn vận đơn cần cập nhật');
        } else {
            $http({

                method: 'POST',
                url: '/mailerimport/updatetakedetails',
                data: {

                    documentID: $scope.takeInfo.DocumentID,
                    mailers: listSends
                }

            }).then(function success(response) {

                if (response.data.error === 0) {
                    $scope.getData();
                    $scope.sendGetTakeMailers();
                    $http.get('/mailerimport/showtakedetail?documentID=' + $scope.takeInfo.DocumentID).then(function (response) {

                        $scope.takeDetailDatas = response.data;

                    });
                } else {
                    showNotify(response.data.msg);
                }

            }, function error(response) {
                showNotify('disconnected internet')
            });

        }

    };

});