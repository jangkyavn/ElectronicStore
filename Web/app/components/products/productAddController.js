/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope', '$rootScope', 'apiService', 'notificationService', '$state', 'commonService'];

    function productAddController($scope, $rootScope, apiService, notificationService, $state, commonService) {
        $rootScope.pageTitle = "Thêm mới sản phẩm";
        $scope.product = {
            Status: true,
            ViewCount: 0
        }
        $scope.addProduct = addProduct;
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

        function addProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);

            apiService.post('/api/product/add', $scope.product, function (result) {
                notificationService.displaySuccess(result.data.Name + " đã được thêm mới.");

                $state.go('products');
            }, function (error) {
                console.log(error);
            });
        }

        getProductCategories();
    }
})(angular.module('products.module'));
