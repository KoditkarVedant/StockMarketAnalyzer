var app = angular.module("MyApp", []);

app.controller("HomeController", function ($scope,$http) {
    $scope.query = "";
    $scope.result = [];
    $scope.searchCompany = function () {
        $scope.result = "";
        $http({
            method: "GET",
            url: "/home/SearchCompany?query=" + $scope.query
        }).then(function (response) {
            $scope.result = response.data;
            console.log($scope.result);
        }, function (response) {
            $scope.result = "";
        });
    }

    $scope.getCompanyDetails = function (ticker) {
        
        window.location.assign("/home/LookupCompany?query=" + ticker);
    }
});