/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);

    productCategoryAddController.$inject = ['$scope', '$rootScope', 'apiService', 'notificationService', '$state', 'commonService'];

    function productCategoryAddController($scope, $rootScope, apiService, notificationService, $state, commonService) {
        $rootScope.pageTitle = "Sửa thông tin thể loại sản phẩm";
        $scope.productCategory = {
            Status: true
        }
        $scope.addProductCategory = addProductCategory;
        $scope.getSeoTitle = getSeoTitle;
        $scope.selectImage = selectImage;

        function selectImage() {
            var finder = new CKFinder();

            finder.selectActionFunction = function (image) {
                $scope.$apply(function () {
                    var imageLength = image.length;
                    var imageLastIndexOf = image.lastIndexOf("/");
                    var imageSubstr = image.substr(imageLastIndexOf + 1, imageLength - imageLastIndexOf);

                    $scope.productCategory.Image = imageSubstr;
                });
            }

            finder.popup();
        }

        function getSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }

        function addProductCategory() {
            apiService.post('/api/productcategory/add', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + " đã được thêm mới.");

                $state.go('product_categories');
            }, function (error) {
                console.log(error);
            });
        }
    }
})(angular.module('productCategories.module'));
