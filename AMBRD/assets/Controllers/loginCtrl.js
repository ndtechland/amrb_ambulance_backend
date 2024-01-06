﻿/// <reference path="../Library/angular.js" />
var app = angular.module('myApp', []);
 
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.9/angular.min.js"></script>

app.controller('loginCtrl', function ($scope, loginFactory) {
    $scope.login = function (user) {
        $scope.color = "grey";
        $scope.message = "matching your credentials......";
        loginFactory.login(user).then(function (response) {
            debugger
            if (response.data == "ok")
                location.href = "/Default/Dashboard";
            else {
                $scope.color = "red";
                $scope.message = response.data;
            }
        })
    }
});

app.factory('loginFactory', function ($http) {
    var fac = {};
    fac.login = function (user) {
        return $http({
            url: '/Default/CheckUserAuthentication',
            method: 'post',
            data: user,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8'
        });
    }
    return fac;
});

app.controller('AthCtrl', function ($scope, Authfactory) {
    $scope.Auth = function (pwd) {
        $scope.color = "grey";
        $scope.message = "Matching Password";
        Authfactory.Auth(pwd).then(function (response) {
            if (response.data == "ok")
                location.href = "/Default/Payout?IsApproved=true";
            else {
                $scope.color = "red";
                $scope.message = response.data;
            }
        })
    }
});

app.factory('Authfactory', function ($http) {
    var fac = {};
    fac.Auth = function (pwd) {
        return $http({
            url: '/Default/TransactionLogin',
            method: 'post',
            data: pwd,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8'
        });
    }
    return fac;
});

