﻿var app = angular.module('myApp', ['ui.bootstrap', 'myKeyPress', 'myDirective', 'ui.mask', 'ui.select2']);
app.controller('myCtrl', function ($scope, $http, $rootScope, $interval) {

    $scope.select2Options = {
        width: '100%'
    };

    // phan trang
    $scope.numPages;
    $scope.itemPerPage;
    $scope.totalItems;
    $scope.currentPage = 1;
    $scope.maxSize = 5;
    $scope.showEdit = false;
    $scope.showDeliveries = false;
    $scope.showUpdate = false;
    $scope.provinces = provinceData;
    $scope.districts = [];

    $scope.deliveryStatus = angular.copy(deliveryStatusData);
    $scope.mailerStatus = angular.copy(mailerStatusData);

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
        "fromDate": fromDate,
        "toDate": toDate,
        "page": $scope.currentPage,
        "postId": ""
    };

    $scope.allDeliveries = [];

    $scope.GetData = function () {
        $scope.searchInfo.page = $scope.currentPage;
        $scope.searchInfo.postId = $scope.postHandle;

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
    };

    $scope.title = '';
    //nhan vien
    $scope.employeeFilter = { EmployeeID: '' };
    $scope.employees = [];
    $scope.licensePlates = [];
    $scope.listEmployeeMonitor = [];

    var findEmployeeIndex = function (code) {
        for (var i = 0; i < $scope.employees.length; i++) {
            if ($scope.employees[i].code === code) {
                return i;
            }
        }

        return -1;
    };

    $scope.addEmployeeMonitor = function () {

        var chek = true;

        for (var i = 0; i < $scope.listEmployeeMonitor.length; i++) {
            if ($scope.listEmployeeMonitor[i].code === $scope.monitorEmployeeChoose) {
                chek = false;
                break;
            }
        }

        if (chek) {
            $scope.listEmployeeMonitor.push($scope.employees[findEmployeeIndex($scope.monitorEmployeeChoose)]);
        } else {
            showNotify("Đã thêm");
        }

    };
    $scope.deliveryReports = [];
    $scope.getReportEmployeeDelivery = function () {

        var listTemps = [];

        for (var i = 0; i < $scope.listEmployeeMonitor.length; i++) {
            listTemps.push($scope.listEmployeeMonitor[i].code);
        }

        $http({
            method: "POST",
            url: "/mailerdelivery/GetReportEmployeeDelivery",
            data: {
                postId: $scope.postHandle,
                employees: listTemps
            }
        }).then(function sucess(response) {

            $scope.deliveryReports = response.data.data;

        }, function error(reponse) {

        });

    };

    $scope.removeMonitor = function (idx) {
        $scope.listEmployeeMonitor.splice(idx, 1);
    };

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
            $scope.getReportEmployeeDelivery();
            $interval(function () { $scope.GetData(); $scope.getReportEmployeeDelivery() }, 1000 * 30);
            hideModel('choosePostOfficeModal');
        }
    };
    $scope.init = function () {
        $scope.postOffices = postOfficeDatas;

        $scope.postHandle = '';

        if ($scope.postOffices.length === 1) {
            $scope.postHandle = $scope.postOffices[0];
            $scope.GetData();
            $scope.getFirstData();
            $scope.getReportEmployeeDelivery();
            $interval(function () { $scope.GetData(); $scope.getReportEmployeeDelivery() }, 1000 * 30);
        } else {
            showModelFix('choosePostOfficeModal');
        }

    };

    // xư ly bang ke
    $scope.createDelivery = function () {

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
        $scope.showDeliveries = false;
        $scope.showUpdate = false;
        $scope.currentDocument = angular.copy($scope.allDeliveries[idx]);

        $('.nav-tabs a[href="#tab_chitiet"]').tab('show');
        $scope.getDocumentDetail();

    };

    $scope.getDocumentDetail = function () {
        $http.get("/mailerdelivery/GetDeliveryMailerDetail?documentID=" + $scope.currentDocument.DocumentID).then(
            function (response) {
                console.log(response.data);
                $scope.mailers = angular.copy(response.data);
            }
        );
    };

    $scope.addMailer = function (valid) {
        if ($scope.currentDocument.DocumentID === '') {
            alert("Không thể thêm");
        } else {
            showLoader(true);
            $http({
                method: "POST",
                url: "/mailerdelivery/AddMailer",
                data: {
                    documentId: $scope.currentDocument.DocumentID,
                    mailerId: $scope.mailerId
                }
            }).then(function sucess(response) {
                var result = response.data;

                if (result.error === 1) {
                    showNotifyWarm(result.msg);
                } else {
                    showNotify('Đã thêm : ' + $scope.mailerId);
                    $scope.mailers.push(result.data);
                }
                $scope.mailerId = "";

                showLoader(false);

            }, function error() {
                alert("connect error");
                showLoader(false);
            });
        }
    };

    // huy phat
    $scope.detroyMailerDelivery = function (mailerId) {
        showLoader(true);
        $http({
            method: 'POST',
            url: '/mailerdelivery/DetroyMailerDelivery',
            data: {
                mailerId: mailerId,
                documentId: $scope.currentDocument.DocumentID
            }
        }).then(function success(response) {
            showLoader(false);
            $scope.getDocumentDetail();
            showNotify('Đã xóa');

        }, function error(response) {
            alert("connect error");
            showLoader(false);

        });
    };

    //  cap nhat mailer
    $scope.mailerUpdates = [];
    $scope.mailerIdUpdate = '';
    $scope.returnReasons = angular.copy(reasonReturnDatas);
    $scope.confirmAllMailers = function () {

        $scope.showEdit = false;
        $scope.showDeliveries = false;
        $scope.showUpdate = true;

        $('.nav-tabs a[href="#tab_capnhatphat"]').tab('show');

        $http.get("/mailerdelivery/GetDeliveryMailerDetailNotUpdate?documentID=" + $scope.currentDocument.DocumentID).then(
            function (response) {
                for (var i = 0; i < response.data.length; i++) {
                    var mailerId = response.data[i].MailerID;
                    if (findMailerUpdatesIndex(mailerId) === -1) {
                        response.data[i].DeliveryStatus = 4;
                        $scope.mailerUpdates.push(response.data[i]);
                    }

                }

            }
        );

    };

    function findMailerUpdatesIndex(mailerId) {
        for (var i = 0; i < $scope.mailerUpdates.length; i++) {
            if ($scope.mailerUpdates[i].MailerID === mailerId)
                return i;
        }

        return -1;
    }

    $scope.addMailerUpdate = function (isvalid) {

        $http.get('/mailerdelivery/GetMailerForReUpdate?mailerID=' + $scope.mailerIdUpdate).then(function (response) {

            var result = response.data;

            if (result.error === 1) {
                showNotifyWarm(result.msg);
            } else {

                if (findMailerUpdatesIndex(result.data.MailerID) === -1) {
                    if (result.data.DeliveryStatus === 3) {
                        result.data.DeliveryStatus = 4;
                    }

                    $scope.mailerUpdates.push(result.data);



                }
                showNotify('Đã thêm ' + $scope.mailerIdUpdate);
                $scope.mailerIdUpdate = '';
            }


        });

    };


    $scope.updateDeliveryMailer = function () {

        var sendUpdate = true;

        for (var i = 0; i < $scope.mailerUpdates.length; i++) {
            // check dieu kien update
            var mailer = $scope.mailerUpdates[i];

            if (mailer.DeliveryDate === "" || mailer.DeliveryTime === "") {
                showNotify(mailer.MailerID + ' chưa nhập ngày giờ');
                sendUpdate = false;
                break;
            }

            if (mailer.DeliveryStatus === 4) {
                // da phat
                if (mailer.DeliveryTo === '') {
                    showNotify(mailer.MailerID + ' chưa nhập người nhận');
                    sendUpdate = false;
                    break;
                }
            }

            if (mailer.DeliveryStatus === 5) {
                // chuyen hoan
                if (mailer.ReturnReasonID === '') {
                    showNotify(mailer.MailerID + ' chưa nhập lý do');
                    sendUpdate = false;
                    break;
                }
            }
            if (mailer.DeliveryStatus === 6) {
                // chuyen hoan
                if (mailer.DeliveryNotes === '') {
                    showNotify(mailer.MailerID + ' chưa nhập ghi chú');
                    sendUpdate = false;
                    break;
                }
            }
        }

        if (sendUpdate) {
            showLoader(true);
            $http({
                method: 'POST',
                url: '/mailerdelivery/ConfirmDeliveyMailer',
                data: {
                    detail: $scope.mailerUpdates
                }
            }).then(function sucess(response) {

                showLoader(false);
                $scope.mailerUpdates = [];
                showNotify('Đã cập nhật phát');
                $scope.mailers = [];
                $scope.currentDocument = {};
                $scope.GetData();

            }, function error(response) {
                showLoader(false);
                alert("connect error");
            });
        }

    };

    $scope.init();

    // tao tu dong
    $scope.autoRoutes = [];
    $scope.countMailers = 0;
    $scope.getAutoRoutes = function () {

        showLoader(true);
        $http.get('/mailerdelivery/AutoGetRouteEmployees?postId=' + $scope.postHandle).then(function (response) {

            showLoader(false);
            $scope.autoRoutes = angular.copy(response.data.routes);
            $scope.countMailers = response.data.coutMailer;
            showModel('autoRoutes');

        });

    };

    $scope.createAutoOneEmployee = function (idx) {

        var routesSend = [];
        routesSend.push($scope.autoRoutes[idx]);
        var date = $('#deliveryRouteDate').val();
        showLoader(true);
        $http({
            method: "POsT",
            url: "/mailerdelivery/CreateFromRoutes",
            data: {
                routes: routesSend,
                postId: $scope.postHandle,
                deliveryDate: date
            }
        }).then(function sucess(response) {

            for (var i = 0; i < response.data.length; i++) {
                $scope.allDeliveries.unshift(response.data[i]);

            }
            showLoader(false);
            hideModel('autoRoutes');
        }, function error(response) {
            showLoader(false);
            alert("connect error");
        });

    };


    $scope.createAutoAllEmployee = function () {

        var date = $('#deliveryRouteDate').val();
        showLoader(true);
        $http({
            method: "POsT",
            url: "/mailerdelivery/CreateFromRoutes",
            data: {
                routes: $scope.autoRoutes,
                postId: $scope.postHandle,
                deliveryDate: date
            }
        }).then(function sucess(response) {

            for (var i = 0; i < response.data.length; i++) {
                $scope.allDeliveries.unshift(response.data[i]);

            }
            showLoader(false);
            hideModel('autoRoutes');
        }, function error(response) {
            showLoader(false);
            alert("connect error");
        });

    };


    // do danh sach tu dong
    $scope.fillMailerForEmployee = function () {


        showLoader(true);

        $http.get('/mailerdelivery/GetAutoMailerFromEmployeeRoute?postId=' + $scope.postHandle + '&employeeId=' + $scope.currentDocument.EmployeeID).then(function (response) {

            $scope.mailerEmployeeFinds = [];

            showLoader(false);

            $scope.employeeMailerFromRoutes = angular.copy(response.data.routes);
            $scope.countMailers = response.data.countMailer;

            showModel('getmailerdelivery');

        });



    };
    $scope.provincesearch = '';
    $scope.districtsearch = '';

    $scope.changeProvince = function () {
        var url = '/mailerinit/GetProvinces?';

        url = url + "parentId=" + $scope.provincesearch + "&type=district";

        $http.get(url).then(function (response) {



            $scope.districts = angular.copy(response.data);

        });
    };

    $scope.changeCheckAllMailerEmplpoyeeAuto = function () {

        for (var i = 0; i < $scope.employeeMailerFromRoutes.Mailers.length; i++) {
            $scope.employeeMailerFromRoutes.Mailers[i].IsCheck = $scope.checkAllMailerAuto;
        }

    };

    $scope.mailerEmployeeFinds = [];
    $scope.findAllMailerForEmployee = function () {

        showLoader(true);

        $http.get('/mailerdelivery/GetMailerForEmployee?postId=' + $scope.postHandle + '&province=' + $scope.provincesearch + '&district=' + $scope.districtsearch).then(function (response) {

            showLoader(false);

            $scope.mailerEmployeeFinds = angular.copy(response.data);

        });

    };

    $scope.checkAllMailerForEmployeeChange = function () {
        for (var i = 0; i < $scope.mailerEmployeeFinds.length; i++) {
            $scope.mailerEmployeeFinds[i].IsCheck = $scope.checkAllMailerForEmployee;
        }
    };

    // do danh sach tu danh sach lay tuyen tu donng
    // type : route --> lay tu employeeMailerFromRoutes
    // type : province --> lay tu mailerEmployeeFinds
    $scope.addMailerAutoFromRoutes = function (type) {
        showLoader(true);

        var listMailer = [];
        var listTemp = [];
        if (type === 'route') {
            listTemp = $scope.employeeMailerFromRoutes.Mailers;
        } else {
            listTemp = $scope.mailerEmployeeFinds;
        }

        for (var i = 0; i < listTemp.length; i++) {
            if (listTemp[i].IsCheck) {
                listMailer.push(listTemp[i].MailerID);
            }
        }

        $http(
            {
                method: 'POST',
                url: '/mailerdelivery/AddListMailer',
                data: {
                    documentId: $scope.currentDocument.DocumentID,
                    mailers: listMailer
                }
            }
        ).then(function sucess(response) {

            var result = response.data;
            hideModel("getmailerdelivery");
            showLoader(false);

            if (result.error === 1) {
                showNotifyWarm(result.msg);
            } else {
                $scope.getDocumentDetail();
                $scope.GetData();
            }

        }, function error(response) {
            showLoader(false);
            showNotify('internet disconnect');
        });

    };

    //


});