/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);

    productCategoryEditController.$inject = ['$scope', '$rootScope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams'];

    function productCategoryEditController($scope, $rootScope, apiService, notificationService, $state, commonService, $stateParams) {
        $rootScope.pageTitle = "Sửa thông tin thể loại sản phẩm";
        $scope.productCategory = {
            
        }
        $scope.editProductCategory = editProductCategory;
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

        function getProductCategoryById() {
            apiService.get('/api/productcategory/getbyid/' + $stateParams.id, null, function (result) {
                $scope.productCategory = result.data;
            }, function (error) {
                console.log(error);
            });
        }

        function editProductCategory() {
            apiService.put('/api/productcategory/edit', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + " đã được cập nhật.");

                $state.go('product_categories');
            }, function (error) {
                console.log(error);
            });
        }

        getProductCategoryById();
    }
})(angular.module('productCategories.module'));
