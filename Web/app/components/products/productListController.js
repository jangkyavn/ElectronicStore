/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productListController', productListController);

    productListController.$inject = ['$scope', '$rootScope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productListController($scope, $rootScope, apiService, notificationService, $ngBootbox, $filter) {
        $rootScope.pageTitle = "Danh sách sản phẩm";
        $scope.products = [];
        $scope.getProducts = getProducts;
        $scope.changeStatus = changeStatus;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteProduct = deleteProduct;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            $ngBootbox.confirm("Bạn có chắc chắn muốn xóa bản ghi được chọn không").then(function () {
                var listId = [];
                $.each($scope.selected, function (i, item) {
                    listId.push(item.ID);
                });

                var config = {
                    params: {
                        checkedProducts: JSON.stringify(listId)
                    }
                }

                apiService.del('/api/product/deletemulti', config, function (result) {
                    notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                    search();
                }, function (error) {
                    console.log(error);
                });
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.products, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.products, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.checkedAll = false;
        $scope.$watch("products", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                var length = n.length;
                if (checked.length == length) {
                    $scope.checkedAll = true;
                    $scope.isAll = true;
                } else {
                    $scope.checkedAll = false;
                    $scope.isAll = false;
                }
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteProduct(id) {
            $ngBootbox.confirm("Bạn có chắc chắn muốn xóa không").then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }

                apiService.del('/api/product/delete', config, function (result) {
                    notificationService.displaySuccess("Xóa thành công " + result.data.Name);

                    search();
                }, function (error) {
                    console.log(error);
                });
            });
        }

        function search() {
            getProducts();
        }

        function changeStatus(id) {
            var config = {
                params: {
                    id: id
                }
            }

            apiService.get('/api/product/changestatus', config, function (result) {
                notificationService.displaySuccess("Đã chuyển trạng thái sang " + (result.data.Status ? "Kích hoạt" : "Khóa"));

                $scope.getProducts();
            }, function (error) {
                console.log(error);
            });
        }

        function getProducts(page) {
            page = page || 0;

            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }

            apiService.get('/api/product/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning("Không tìm thấy bản ghi nào.");
                }

                $scope.products = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function (error) {
                console.log(error);
            });
        }

        $scope.getProducts();
    }
})(angular.module('products.module'));
