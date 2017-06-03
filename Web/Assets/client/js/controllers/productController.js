var productController = {
    init: function () {
        productController.registerEvent();
    },
    registerEvent: function () {
        $("#ddlChangeSort").change(function () {
            window.location.href = "?sort=" + this.value;
        });

        $("#ddlChangeSortSearch").change(function () {
            var keyword = $("#hidKeyword").val();

            window.location.href = "?keyword=" + keyword + "&sort=" + this.value;
        });
    }
}

productController.init();