var app = angular.module('myApp', ['ui.bootstrap', 'myKeyPress', 'myDirective', 'ui.mask']);
app.controller('myCtrl', function ($scope, $http, $rootScope) {

    // phan trang
    $scope.numPages;
    $scope.itemPerPage;
    $scope.totalItems;
    $scope.currentPage = 1;
    $scope.maxSize = 5;
    $scope.postOffices = '';
    $scope.mailerPartners = [];
    $scope.mailerPartnerStatus = [{ code: 0, name: 'Mới tạo' }, { code: 1, name: 'Đã gửi' }, { code: 2, name: 'Đã hủy' }];
    $scope.mailerStatus = angular.copy(mailerStatusData);
    $scope.partners = partnerDatas;
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
            url: "/mailerpartner/GetMailerPartner",
            data: $scope.searchInfo
        }).then(function mySuccess(response) {
            showLoader(false);

            if (response.data.error === 0) {
                $scope.itemPerPage = response.data.pageSize;
                $scope.totalItems = response.data.toltalSize;
                $scope.numPages = Math.round($scope.totalItems / $scope.itemPerPage);
                $scope.mailerPartners = response.data.data;

            } else {
                alert(response.data.msg);
            }

        }, function myError(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    };

    $scope.myAddress = {};
    $scope.provinceSend = provinceSendGet;
    $scope.districtSend = [];
    $scope.wardSend = [];
    $scope.provinceChange = function (pType, type) {

        var url = '/mailerinit/GetProvinces?';

        if (pType === "district") {
            url = url + "parentId=" + $scope.myAddress.province + "&type=district";
        }

        if (pType === "ward") {
            url = url + "parentId=" + $scope.myAddress.district + "&type=ward";
        }

        $http.get(url).then(function (response) {

            if (pType === "district") {
                $scope.districtSend = angular.copy(response.data);
            }

            if (pType === "ward") {
                $scope.wardSend = angular.copy(response.data);
            }

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
    $scope.init();

    $scope.addMailerPartner = function () {
        $scope.mailerPartnerSend = { postId: $scope.postHandle };
        showModel('createPartnerMailer');
    };

    $scope.finishMailerPartnerSend = function (valid) {

        if (valid) {
            showLoader(true);
            $http({
                method: 'POST',
                url: '/mailerpartner/createmailerpartner',
                data: $scope.mailerPartnerSend
            }).then(function sucess(response) {
                showLoader(false);
                hideModel('createPartnerMailer');
                $scope.GetData();

            }, function error(response) {
                showLoader(false);
                showNotify('Connect error');
            });


        } else {
            showNotify("Thiếu một số thông tin");
        }

    };

    $scope.document = {};
    $scope.mailers = [];
    $scope.showDetail = function (idx) {
        $('.nav-tabs a[href="#tab_detail"]').tab('show');
        $scope.document = $scope.mailerPartners[idx];
        $scope.getDocumentDetail();
    };
    $scope.getDocumentDetail = function () {

        $http.get("/mailerpartner/GetMailerPartnerDetail?documentId=" + $scope.document.DocumentID).then(function (response) {
            $scope.mailers = response.data.data;

        });

    };

    $scope.addMailer = function () {
        showLoader(true);

        $http({
            method: 'POST',
            url: '/mailerpartner/AddMailer',
            data: {
                documentId: $scope.document.DocumentID,
                mailerId: $scope.mailerId
            }
        }).then(function sucess(response) {

            var result = response.data;
            if (result.error === 1) {
                showNotify(result.msg);
            } else {
                $scope.mailerId = '';
                $scope.mailers.unshift(result.data);
                showNotify("Đã add");
            }

            showLoader(false);
        }, function erorr(response) {
            showLoader(false);
            showNotify('Connect error');
        });

    };

    $scope.updateDetails = function () {

        showLoader(true);

        $http({
            method: 'POST',
            url: '/mailerpartner/UpdateDetails',
            data: {
                mailers: $scope.mailers,
                documentId: $scope.document.DocumentID
            }
        }).then(function sucess(response) {
            $scope.getDocumentDetail();
            showLoader(false);
        }, function erorr(response) {
            showLoader(false);
            showNotify('Connect error');
        });

    };

    $scope.deleteDetail = function (idx) {
        showLoader(true);

        $http({
            method: 'POST',
            url: '/mailerpartner/DeleteDetail',
            data: {
                documentId: $scope.document.DocumentID,
                mailerId: $scope.mailers[idx].MailerID
            }
        }).then(function sucess(response) {

            var result = response.data;
            if (result.error === 1) {
                showNotify(result.msg);
            } else {
                $scope.mailers.splice(idx, 1);
                showNotify("Đã xóa");
            }

            showLoader(false);
        }, function erorr(response) {
            showLoader(false);
            showNotify('Connect error');
        });
    };

    $scope.showModalAddressInfo = function () {
        showModel('sendpartner');
    };

    $scope.sendToPartner = function (valid) {

        if (valid) {
            showLoader(true);

            $http({
                method: "POST",
                url: "/mailerpartner/SendPartner",
                data: {
                    documentId: $scope.document.DocumentID,
                    address: $scope.myAddress
                }
            }).then(function sucess(response) {
                showLoader(false);
                $scope.document.StatusID = 1;
                $scope.getDocumentDetail();
                hideModel('sendpartner');
            }, function error(response) {

                showLoader(false);
                showNotify('Connect error');
            });
        } else {
            showNotify("thiếu một số thông tin");
        }

    };

});