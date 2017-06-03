var cartItemController = {
    init: function () {
        cartItemController.loadData();
        cartItemController.registerEvent();
    },
    registerEvent: function () {
        $('.colorful').bootstrapNumber({
            upClass: 'success',
            downClass: 'danger'
        });

        $('.txt-quantity').on('keyup change blur', function () {
            var productId = $(this).data('id');
            var quantity = $(this).val();

            if (quantity == "" || quantity < 1 || quantity > 10) {
                $(this).val(1);
            } else {
                cartItemController.updateCartItem(productId, quantity);
            }
        });

        $(".btn-add-cart").off('click').on('click', function () {
            var productId = $(this).data('id');

            cartItemController.addCartItem(productId);
        });

        $('.btn-delete-cart').off('click').on('click', function () {
            var productId = $(this).data('id');

            cartItemController.deleteCartItem(productId);
        });
    },
    deleteCartItem: function (productId) {
        $.ajax({
            url: '/CartItem/DeleteCartItem',
            type: 'POST',
            data: {
                productId: productId
            },
            dataType: 'json',
            success: function (status) {
                if (status) {
                    cartItemController.loadData();
                }
            }
        });
    },
    addCartItem: function (productId) {
        $.ajax({
            url: '/CartItem/AddCartItem',
            type: 'POST',
            data: {
                productId: productId
            },
            dataType: 'json',
            success: function (status) {
                if (status) {
                    alert("Thêm vào giỏ hàng thành công.");
                }
            }
        });
    },
    updateCartItem: function (productId, quantity) {
        $.ajax({
            url: '/CartItem/UpdateCartItem',
            type: 'POST',
            data: {
                productId: productId,
                quantity: quantity
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    $("#lblTotalCountCartItem").text("(" + response.totalCount + ")");
                    $('.lblAmount').text(numeral(parseFloat(response.totalMoney)).format(0, 0));
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/CartItem/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                $("#lblTotalCountCartItem").text("(" + response.totalCount + ")");

                $(".lblAmount").text(numeral(response.totalMoney).format(0, 0));

                var data = response.data;

                if (data.length > 0) {
                    var html = '';

                    var template = $("#templateCartItem").html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductId: item.ProductId,
                            Image: item.Product.Image,
                            Name: item.Product.Name,
                            Price: numeral(item.Product.Price).format(0, 0),
                            Quantity: item.Quantity
                        });
                    });

                    $("#lblCartItemData").html(html);

                    cartItemController.registerEvent();
                } else {
                    $("#lblCartItemData").html("<div class='alert alert-danger' id='lblAlertCartItem'>Không có sản phẩm nào trong giỏ hàng.</div>");
                }
            }
        });
    }
}

cartItemController.init();