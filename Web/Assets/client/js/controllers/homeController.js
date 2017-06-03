var homeController = {
    init: function () {
        homeController.registerEvent();
    },
    registerEvent: function (e) {
        $('[data-toggle="tooltip"]').tooltip();

        $("#btnCartItem").off('click').on('click', function () {
            window.location.href = "/gio-hang";
        });

        $(".lbtnSeeProductDetail").off('click').on('click', function (e) {
            e.preventDefault();

            var id = $(this).data('id');

            $.ajax({
                url: '/Home/ProductDetail',
                type: 'GET',
                data: {
                    id: id
                },
                dataType: 'json',
                success: function (response) {
                    if (response.data != null) {
                        var data = response.data;

                        $("#imgImageDetail").attr('src', '/UploadedFiles/images/' + data.Image);
                        $("#imgImageDetail").attr('alt', data.Name);
                        $("#lblNameDetail").text(data.Name);
                        $("#lblDescriptionDetail").text(data.Description);

                        if (data.PromotionPrice == null) {
                            $("#lblPriceDetail").html('<i class="item_price">' + data.Price.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + ' vnđ</i>');
                        } else {
                            $("#lblPriceDetail").html('<span>' + data.Price.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + ' vnđ</span> <i class="item_price">' + data.PromotionPrice.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,") + ' vnđ</i>');
                        }
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        });

        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function( request, response ) {
                $.ajax( {
                    url: "/Home/GetListProductByKeyword",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function( res ) {
                        response( res.data );
                    }
                } );
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.Name);

                return false;
            }
        })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
              .append("<div>" + item.Name + "<br>" + item.Price + "</div>")
              .appendTo(ul);
        };
    },
}

homeController.init();