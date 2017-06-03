/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('homeController', homeController);

    homeController.$inject = ['$rootScope'];

    function homeController($rootScope) {
        $rootScope.pageTitle = "Trang chủ";
    }
})(angular.module('home.module'));
