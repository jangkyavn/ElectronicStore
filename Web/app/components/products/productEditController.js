/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope', '$rootScope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams'];

    function productEditController($scope, $rootScope, apiService, notificationService, $state, commonService, $stateParams) {
        $rootScope.pageTitle = "Sửa thông tin sản phẩm";
        $scope.product = {
            Status: true,
            HotFlag: false
        }
        $scope.editProduct = editProduct;
        $scope.getSeoTitle = getSeoTitle;
        $scope.selectImage = selectImage;
        $scope.productCategories = [];
        $scope.moreImages = [];
        $scope.selectMoreImages = selectMoreImages;

        function selectMoreImages() {
            var finder = new CKFinder();

            finder.selectActionFunction = function (image) {
                $scope.$apply(function () {
                    var imageLength = image.length;
                    var imageLastIndexOf = image.lastIndexOf("/");
                    var imageSubstr = image.substr(imageLastIndexOf + 1, imageLength - imageLastIndexOf);

                    $scope.moreImages.push(imageSubstr);
                });
            }

            finder.popup();
        }

        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }

        function selectImage() {
            var finder = new CKFinder();

            finder.selectActionFunction = function (image) {
                $scope.$apply(function () {
                    var imageLength = image.length;
                    var imageLastIndexOf = image.lastIndexOf("/");
                    var imageSubstr = image.substr(imageLastIndexOf + 1, imageLength - imageLastIndexOf);

                    $scope.product.Image = imageSubstr;
                });
            }

            finder.popup();
        }

        function getSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        function getProductCategories() {
            apiService.get('/api/productcategory/getallparent', null, function (result) {
                $scope.productCategories = result.data;
            }, function (error) {
                console.log(error);
            });
        }

        function loadProductDetail() {
            apiService.get('/api/product/getbyid/' + $stateParams.id, null, function (result) {
                $scope.product = result.data;

                if (result.data.MoreImages != null) {
                    $scope.moreImages = JSON.parse(result.data.MoreImages);
                }
            }, function (error) {
                console.log(error);
            });
        }

        function editProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);

            apiService.put('/api/product/edit', $scope.product, function (result) {
                notificationService.displaySuccess(result.data.Name + " đã được cập nhật.");

                $state.go('products');
            }, function (error) {
                console.log(error);
            });
        }

        getProductCategories();
        loadProductDetail();
    }
})(angular.module('products.module'));
