/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('productCategories.module', ['common.module']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('product_categories', {
            url: '/product_categories',
            templateUrl: '/app/components/product_categories/productCategoryListView.html',
            parent: 'base',
            controller: 'productCategoryListController'
        }).state('product_category_add', {
            url: '/product_category_add',
            parent: 'base',
            templateUrl: '/app/components/product_categories/productCategoryAddView.html',
            controller: 'productCategoryAddController'
        }).state('product_category_edit', {
            url: '/product_category_edit/:id',
            parent: 'base',
            templateUrl: '/app/components/product_categories/productCategoryEditView.html',
            controller: 'productCategoryEditController'
        });
    }
})();