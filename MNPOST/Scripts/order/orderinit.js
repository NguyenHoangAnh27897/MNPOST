
// tao controller
var app = angular.module('myApp', ['ui.bootstrap', 'myKeyPress', 'ui.uploader']);

app.service('mailerService', function () {
    var mailerList = [];

    var merchandise = [{ 'code': 'H', 'name': 'Hàng hóa' }, { 'code': 'T', 'name': 'Thư từ' }];

    var customers = customersGet;

    var mailerTypes = mailerTypesGet;

    var payments = paymentsGet;


    var postchoose = '';

    var setPost = function (post) {
        postchoose = post;
    };

    var getPost = function () {
        return postchoose;
    };

    var actionEdit = true;

    var addMailer = function (newObj) {
        mailerList.unshift(newObj);
    };

    var addNewMailer = function () {
        mailerList.unshift(getNewMailer());

    };


    var getNewMailer = function () {
        return {
            'MailerID': '', 'SenderID': '', 'SenderName': '', 'SenderAddress': '', 'SenderWardID': '', 'SenderDistrictID': '',
            'SenderProvinceID': '', 'SenderPhone': '', 'RecieverName': ''
            , 'RecieverAddress': '', 'RecieverWardID': '', 'RecieverDistrictID': '', 'RecieverProvinceID': '',
            'RecieverPhone': '', 'Weight': 0.01, 'Quantity': 1, 'PaymentMethodID': 'NGTT', 'MailerTypeID': 'SN',
            'PriceService': 0, 'MerchandiseID': 'H', 'Services': [], 'MailerDescription': '', 'Notes': '', 'COD': 0, 'LengthSize': 0, 'WidthSize': 0, 'HeightSize': 0, 'PriceMain': 0, 'CODPrice': 0,
            'PriceDefault': 0
        };
    };


    var resetMailers = function () {
        mailerList = [];
    };

    var getServices = function () {
        return services;
    };

    var getMailerCurrent = function () {
        return mailer;
    };
    var getActionEdit = function () {
        return actionEdit;
    };

    var getMailers = function () {
        return mailerList;
    };

    var removeMailer = function (idx) {
        mailerList.splice(idx, 1);
    };


    var getCustomers = function () {
        return customers;
    };

    var getMailerTypes = function () {
        return mailerTypes;
    };

    var getPayments = function () {
        return payments;
    };

    var findCustomer = function (code) {
        for (var i = 0; i < customers.length; i++)
            if (customers[i].code === code)
                return customers[i];
        return null;
    };

    var getMerchandises = function () {

        return merchandise;
    };

    return {
        addMailer: addMailer,
        getMailers: getMailers,
        addNewMailer: addNewMailer,
        removeMailer: removeMailer,
        getCustomers: getCustomers,
        getMailerTypes: getMailerTypes,
        getPayments: getPayments,
        findCustomer: findCustomer,
        getActionEdit: getActionEdit,
        getNewMailer: getNewMailer,
        getMerchandises: getMerchandises,
        resetMailers: resetMailers,
        getPost: getPost,
        setPost: setPost
    };

});


// controller main --> gird mailer
app.controller('myCtrl', function ($scope, $http, $rootScope, mailerService, uiUploader) {

    $scope.mailers = mailerService.getMailers();
    $scope.customers = mailerService.getCustomers();
    $scope.mailerTypes = mailerService.getMailerTypes();
    $scope.payments = mailerService.getPayments();
    $scope.merchandises = mailerService.getMerchandises();

    $scope.postchoose = '';

    $scope.currentIdx = -1;

    $scope.addNewMailer = function () {
        mailerService.addNewMailer();
    };

    $scope.removeMailer = function (idx) {
        mailerService.removeMailer(idx);
    };

    $scope.copyMailer = function (idx) {
        var mailer = angular.copy($scope.mailers[idx]);
        mailerService.addMailer(mailer);
    };

    $scope.init = function () {
        $scope.postOffices = postOfficesData;

        $scope.postchoose = '';

        if ($scope.postOffices.length === 1) {
            $scope.postchoose = $scope.postOffices[0];
            mailerService.setPost($scope.postchoose);
        } else {
            showModelFix('choosePostOfficeModal');
        }

    };

    $scope.init();

    $scope.choosePostHandle = function () {
        if ($scope.postchoose === '') {
            alert('Chọn bưu cục nếu không sẽ không thể thao tác');
        } else {
            $scope.postchoose = $scope.postchoose;
            mailerService.setPost($scope.postchoose);
            hideModel('choosePostOfficeModal');
        }
    };
    $scope.changeCus = function (idx) {
        var mailer = $scope.mailers[idx];

        var cus = mailerService.findCustomer(mailer.SenderID);

        if (cus) {
            $scope.mailers[idx].SenderName = cus.name;
            $scope.mailers[idx].SenderPhone = cus.phone;
            $scope.mailers[idx].SenderProvinceID = cus.provinceId;
            $scope.mailers[idx].SenderAddress = cus.address;
            $scope.mailers[idx].SenderDistrictID = cus.districtId;
            $scope.mailers[idx].SenderWardID = cus.wardId;
        }
    };

    $scope.editMailer = function (idx) {
        $scope.currentIdx = idx;
        var mailer = $scope.mailers[idx];
        $rootScope.$broadcast('insert-started', { mailer: mailer, actionEdit: true });

        showModel('insertmodal');

    };

    // 
    $scope.getLocation = function (val) {
        return $http.get('//maps.googleapis.com/maps/api/geocode/json?components=country:VN', {
            params: {
                address: val,
                sensor: false
            }
        }).then(function (response) {
            return response.data.results.map(function (item) {
                return item;
            });
        });
    };

    $scope.onSelectAddress = function ($item, $model, $label, idx, type) {
        console.log($item);
        var result = handleAutoCompleteAddress($item);

        console.log(result);

        var url = "/MailerInit/GetProvinceFromAddress?province=" + result.administrative_area_level_1 + "&district=" + result.administrative_area_level_2 + "&ward=" + result.political;

        $http.get(url).then(function (response) {

            if (type === "send") {
                $scope.mailers[idx].SenderProvinceID = response.data.provinceId;
                $scope.mailers[idx].SenderDistrictID = response.data.districtId;
                $scope.mailers[idx].SenderWardID = response.data.wardId;
            } else {
                $scope.mailers[idx].RecieverProvinceID = response.data.provinceId;
                $scope.mailers[idx].RecieverDistrictID = response.data.districtId;
                $scope.mailers[idx].RecieverWardID = response.data.wardId;
            }


        });
    };

    $scope.getMailerCode = function (idx) {
        var mailer = $scope.mailers[idx];

        if (mailer.MailerID === "" || mailer.MailerID === null) {
            showLoader(true);
            var url = "/MailerInit/GeneralCode?cusId=" + mailer.SenderID;

            $http.get(url).then(function (response) {

                $scope.mailers[idx].MailerID = response.data.code;
                showLoader(false);
            });
        } else {
            showNotify('Xóa mã đang nhập nếu muốn tạo mới');
        }

    };


    $scope.finishForm = function (valid) {

        if ($scope.postchoose === '') {
            alert('Chọn bưu cục xử lý');
        } else if ($scope.mailers.length === 0) {
            alert("Không có đơn nào để cập nhật");
        } else {
            showLoader(true);

            $http({
                method: "POST",
                url: "/mailerinit/InsertMailers",
                data: {
                    mailers: $scope.mailers,
                    postId: $scope.postchoose
                }
            }).then(function mySuccess(response) {

                mailerService.resetMailers();
                $scope.mailers = mailerService.getMailers();
                for (var i = 0; i < response.data.data.length; i++) {
                    mailerService.addMailer(response.data.data[i]);
                }

                showLoader(false);

                alert('Đã thêm hoàn thành');

            }, function myError(response) {
                alert('Connect error');
                showLoader(false);
            });
        }

    };

    $scope.createMailerDetail = function () {

        $rootScope.$broadcast('insert-started', { mailer: mailerService.getNewMailer(), actionEdit: false });

        showModel('insertmodal');

    };

    $scope.$on('result-edit-started', function (event, args) {
        var mailer = args.mailer;

        $scope.mailers[$scope.currentIdx] = angular.copy(mailer);
    });

    $scope.senderExcelInfo = {};
    $scope.provinceSend = provinceSendGet;
    $scope.districtSend = [];
    $scope.wardSend = [];
    $scope.addByExcelFile = function () {
        showModel('insertbyexcel');
    };


    $scope.changeExcelCus = function (idx) {

        var cus = mailerService.findCustomer($scope.senderExcelInfo.senderID);

        if (cus) {
            $scope.senderExcelInfo.senderName = cus.name;
            $scope.senderExcelInfo.senderPhone = cus.phone;
            $scope.senderExcelInfo.senderProvince = cus.provinceId;
            $scope.senderExcelInfo.senderAddress = cus.address;
            $scope.senderExcelInfo.senderDistrict = cus.districtId;
            $scope.senderExcelInfo.senderWard = cus.wardId;
            $scope.senderExcelInfo.postId = mailerService.getPost();
        }
    };

    $scope.provinceExcelChange = function (pType, type) {

        var url = '/mailerinit/GetProvinces?';

        if (pType === "district") {
            url = url + "parentId=" + $scope.senderExcelInfo.senderProvince + "&type=district";
        }

        if (pType === "ward") {
            url = url + "parentId=" + $scope.senderExcelInfo.senderDistrict + "&type=ward";
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

    $scope.calPrice = function (index) {
        var info = $scope.mailers[index];
        $http({
            method: "POST",
            url: "/mailer/calbillprice",
            data: {
                'weight': info.Weight,
                'customerId': info.SenderID,
                'provinceId': info.SenderProvinceID,
                'serviceTypeId': info.MailerTypeID,
                'postId': mailerService.getPost(),
                'cod': info.COD,
                'goodPrice': info.MerchandiseValue
            }
        }).then(function mySuccess(response) {

            $scope.mailers[index].PriceMain = response.data.price;
            $scope.mailers[index].CODPrice = response.data.codPrice;

            $scope.mailers[index].PriceDefault = $scope.mailers[index].PriceMain + $scope.mailers[index].CODPrice + $scope.mailers[index].PriceService;

        }, function myError(response) {
            alert('Connect error');
        });

    };

    // upload file
    $scope.files = [];
    var inserByExcelElement = document.getElementById('insertByExcel');
    inserByExcelElement.addEventListener('change', function (e) {
        uiUploader.removeAll();
        var files = e.target.files;
        uiUploader.addFiles(files);
        $scope.files = uiUploader.getFiles();
        $scope.$apply();
    });

    $scope.insertByExcel = function () {

        if ($scope.senderExcelInfo.senderProvince === null || $scope.senderExcelInfo.senderDistrict === null || $scope.senderExcelInfo.senderWard === null) {
            alert('Thiếu thông tin tỉnh thành');
        } else {
            showLoader(true);
            uiUploader.startUpload({
                url: '/mailerinit/insertbyexcel',
                concurrency: 2,
                paramName: 'files',
                data: $scope.senderExcelInfo,
                onProgress: function (file) {
                    $scope.$apply();
                },
                onCompleted: function (file, response) {

                    showLoader(false);
                    var result = angular.fromJson(response);
                    console.log(result);
                    if (result.error === 1) {
                        alert(result.msg);
                    } else {
                        uiUploader.removeAll();
                        inserByExcelElement.value = "";
                        hideModel('insertbyexcel');
                        for (var i = 0; i < result.data.length; i++) {
                            mailerService.addMailer(result.data[i]);
                        }
                        $scope.$apply();
                    }
                }
            });
        }


    };

});

// controller detail --> create, edit mailer
app.controller('ctrlAddDetail', function ($scope, $rootScope, $http, mailerService) {

    $scope.customers = mailerService.getCustomers();
    $scope.mailerTypes = mailerService.getMailerTypes();
    $scope.payments = mailerService.getPayments();
    $scope.otherServices = angular.copy(servicesGet);
    $scope.merchandises = mailerService.getMerchandises();
    $scope.mailer = {};
    $scope.actionEdit = false;

    $scope.$on('insert-started', function (event, args) {

        $scope.mailer = angular.copy(args.mailer);
        console.log($scope.mailer);
        $scope.otherServices = angular.copy(servicesGet);
        $scope.actionEdit = args.actionEdit;

        for (var i = 0; i < $scope.mailer.Services.length; i++) {
            for (var j = 0; j < $scope.otherServices.length; j++) {

                if ($scope.mailer.Services[i].code === $scope.otherServices[j].code) {
                    $scope.otherServices[j].choose = true;
                    $scope.otherServices[j].price = $scope.mailer.Services[i].price;
                }

            }
        }

        console.log($scope.otherServices);
    });




    $scope.provinceRecei = angular.copy($scope.provinceSend);
    $scope.provinceSend = provinceSendGet;
    $scope.districtSend = [];
    $scope.wardSend = [];

    $scope.districtRecei = [];
    $scope.wardRecei = [];

    $scope.actionEdit = mailerService.getActionEdit();

    $scope.addSeviceMorePrice = function () {
        var total = 0;
        $scope.mailer.Services = [];
        for (var i = 0; i < $scope.otherServices.length; i++) {
            if ($scope.otherServices[i].choose) {
                total = total + $scope.otherServices[i].price;
                $scope.mailer.Services.push($scope.otherServices[i]);
            }
        }
        console.log(total);
        $scope.mailer.PriceService = total;
        $scope.changePrice();
    };


    $scope.getAddressDetailInfo = function (url, type, address) {

        $http.get(url).then(function (response) {

            if (type === "send") {
                $scope.mailer.SenderAddress = address;
                $scope.mailer.SenderProvinceID = response.data.provinceId;
                $scope.mailer.SenderDistrictID = response.data.districtId;
                $scope.mailer.SenderWardID = response.data.wardId;
            } else {
                $scope.mailer.RecieverAddress = address;
                $scope.mailer.RecieverProvinceID = response.data.provinceId;
                $scope.mailer.RecieverDistrictID = response.data.districtId;
                $scope.mailer.RecieverWardID = response.data.wardId;
            }

            $scope.getDistrictAndWard(type);

        });
    };
    $scope.changeCus = function () {

        var cus = mailerService.findCustomer($scope.mailer.SenderID);

        if (cus) {
            $scope.mailer.SenderPhone = cus.phone;
            $scope.mailer.SenderProvinceID = cus.provinceId;
            $scope.mailer.SenderAddress = cus.address;
            $scope.mailer.SenderDistrictID = cus.districtId;
            $scope.mailer.SenderWardID = cus.wardId;

            $scope.getDistrictAndWard('send');
        }
    };

    $scope.getDistrictAndWard = function (type) {

        var url = '/mailerinit/GetDistrictAndWard?provinceId=';

        if (type === 'send') {
            url = url + $scope.mailer.SenderProvinceID + "&districtId=" + $scope.mailer.SenderDistrictID;
        } else {
            url = url + $scope.mailer.RecieverProvinceID + "&districtId=" + $scope.mailer.RecieverDistrictID;
        }

        $http.get(url).then(function (response) {
            if (type === "send") {
                $scope.districtSend = angular.copy(response.data.districts);
                $scope.wardSend = angular.copy(response.data.wards);
            } else {
                $scope.districtRecei = angular.copy(response.data.districts);
                $scope.wardRecei = angular.copy(response.data.wards);
            }
        });
    };

    $scope.finishForm = function (isValid) {
        if ($scope.actionEdit) {
            $rootScope.$broadcast('result-edit-started', { mailer: $scope.mailer });
        } else {
            mailerService.addMailer($scope.mailer);
        }

        hideModel('insertmodal');
    };

    $scope.provinceChange = function (pType, type) {

        var url = '/mailerinit/GetProvinces?';

        if (type === 'send') {

            if (pType === "district") {
                url = url + "parentId=" + $scope.mailer.SenderProvinceID + "&type=district";
            }

            if (pType === "ward") {
                url = url + "parentId=" + $scope.mailer.SenderDistrictID + "&type=ward";
            }

        } else {
            if (pType === "district") {
                url = url + "parentId=" + $scope.mailer.RecieverProvinceID + "&type=district";
            }

            if (pType === "ward") {
                url = url + "parentId=" + $scope.mailer.RecieverDistrictID + "&type=ward";
            }
        }

        $http.get(url).then(function (response) {

            if (type === 'send') {

                if (pType === "district") {
                    $scope.districtSend = angular.copy(response.data);
                }

                if (pType === "ward") {
                    $scope.wardSend = angular.copy(response.data);
                }

            } else {
                if (pType === "district") {
                    $scope.districtRecei = angular.copy(response.data);
                }

                if (pType === "ward") {
                    $scope.wardRecei = angular.copy(response.data);
                }
            }

        });

    };


    $scope.showTest = function () {
        console.log($scope.mailer);
        console.log(mailerService.getMailerCurrent());
    };

    $scope.getMailerCode = function () {

        if ($scope.mailer.MailerID === "") {
            showLoader(true);
            var url = "/MailerInit/GeneralCode?cusId=" + $scope.mailer.SenderID;

            $http.get(url).then(function (response) {

                $scope.mailer.MailerID = response.data.code;
                showLoader(false);
            });
        } else {
            alert('Xóa mã đang nhập nếu muốn tạo mới');
        }

    };

    $scope.calPrice = function () {
        $http({
            method: "POST",
            url: "/mailer/calbillprice",
            data: {
                'weight': $scope.mailer.Weight,
                'customerId': $scope.mailer.SenderID,
                'provinceId': $scope.mailer.SenderProvinceID,
                'serviceTypeId': $scope.mailer.MailerTypeID,
                'postId': mailerService.getPost(),
                'cod': $scope.mailer.COD,
                'goodPrice': $scope.mailer.MerchandiseValue
            }
        }).then(function mySuccess(response) {

            $scope.mailer.PriceMain = response.data.price;
            $scope.mailer.CODPrice = response.data.codPrice;

            $scope.mailer.PriceDefault = $scope.mailer.PriceMain + $scope.mailer.CODPrice + $scope.mailer.PriceService;

        }, function myError(response) {
            alert('Connect error');
        });
    };

    $scope.changePrice = function () {
        console.log('tinh gia tong');
        $scope.mailer.PriceDefault = $scope.mailer.PriceMain + $scope.mailer.CODPrice + $scope.mailer.PriceService;
    };

});


var autocompleteSend;
var autocompleteRecei;

function fillInAddressSend() {

    var place = autocompleteSend.getPlace();

    var result = handleAutoCompleteAddress(place);

    var url = "/MailerInit/GetProvinceFromAddress?province=" + result.administrative_area_level_1 + "&district=" + result.administrative_area_level_2 + "&ward=" + result.sublocality_level_1;

    angular.element(document.getElementById('ctrlAddDetail')).scope().getAddressDetailInfo(url, 'send', place.formatted_address);

};

function fillInAddressRecei() {

    var place = autocompleteRecei.getPlace();
    var result = handleAutoCompleteAddress(place);

    var url = "/MailerInit/GetProvinceFromAddress?province=" + result.administrative_area_level_1 + "&district=" + result.administrative_area_level_2 + "&ward=" + result.sublocality_level_1;

    angular.element(document.getElementById('ctrlAddDetail')).scope().getAddressDetailInfo(url, 'recei', place.formatted_address);

};



function init() {
    autocompleteSend = new createAutoCompleteAddress('autocompleteSend', fillInAddressSend);
    autocompleteRecei = new createAutoCompleteAddress('autocompleteRecei', fillInAddressRecei);
};



