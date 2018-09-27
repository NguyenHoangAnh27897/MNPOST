var app = angular.module('myApp', ['ui.bootstrap']);

app.controller('myCtrl', function ($scope, $http, $rootScope) {

    $scope.mailers = [];

    $scope.checkMailers = false;

    $scope.mailerId = '';

    $scope.checkAll = function () {
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

        showLoader(true);
        var url = '/mailerimport/getdata?postId=' + $scope.postHandle;
        $http.get(url).then(function (response) {

            showLoader(false);
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

    $scope.addMailerImport = function(isValid) {

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

    $scope.init = function () {
        $scope.postOffices = postOfficeDatas;

        $scope.postHandle = '';

        if ($scope.postOffices.length === 1) {
            $scope.postHandle = $scope.postOffices[0];
            $scope.getData();
        } else {
            showModelFix('choosePostOfficeModal');
        }

    };

    $scope.choosePostHandle = function () {
        if ($scope.postHandle === '') {
            alert('Chọn bưu cục nếu không sẽ không thể thao tác');
        } else {
            $scope.getData();
            hideModel('choosePostOfficeModal');
        }
    };

    $scope.init();
});